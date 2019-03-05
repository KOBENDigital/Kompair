using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Http;
using Koben.Kompair.Services;
using Koben.Kompair.Dashboard.Services;
using Newtonsoft.Json;
using Umbraco.Web.WebApi;

namespace Koben.Kompair.Dashboard.Controllers
{
	public class KompairController : UmbracoApiController
	{
		private readonly UInt64 RequestMaxAgeInSeconds = 300;

		private readonly IKompairDataService _KompairDataService;
		private readonly IKompairCertificateService _KompairCertificateService;
		private readonly IKompairApiKeyService _ApiKeyService;

		public KompairController()
		{
			_KompairDataService = new KompairDataService(new KompairContentTypesService(), new KompairDataTypesService());
			_KompairCertificateService = new KompairCertificateService();
			_ApiKeyService = new KompairApiKeyService();
		}

		[HttpGet]
		public IHttpActionResult GetDocumentTypesForComparison()
		{
			if (!Enum.TryParse(ConfigurationManager.AppSettings[KompairDefaults.AuthenticationModeAppSetting], out KompairAuthenticationMode authMode))
			{
				authMode = KompairDefaults.AuthenticationMode;
			}

			AuthenticationHeaderValue authHeaderValue;

			switch (authMode)
			{
				case KompairAuthenticationMode.Certificate:
					if (AuthenticateCertificate())
					{
						return Json(_KompairDataService.GetDocumentTypesForComparison());
					}

					authHeaderValue = new AuthenticationHeaderValue("ClientCert");
					return Unauthorized(authHeaderValue);

				case KompairAuthenticationMode.Key:
					if (AuthenticateApiKey())
					{
						return Json(_KompairDataService.GetDocumentTypesForComparison());
					}

					authHeaderValue = new AuthenticationHeaderValue(KompairDefaults.ApiKeyAuthHeader);
					return Unauthorized(authHeaderValue);

				case KompairAuthenticationMode.None:
					return Json(_KompairDataService.GetDocumentTypesForComparison());

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private bool AuthenticateCertificate()
		{
			string thumbprint = ConfigurationManager.AppSettings[KompairDefaults.CertificateThumbprintAppSetting];

			if (!Enum.TryParse(ConfigurationManager.AppSettings[KompairDefaults.CertificateStoreNameAppSetting], out StoreName store))
			{
				store = KompairDefaults.CertificateStoreName;
			}

			if (!Enum.TryParse(ConfigurationManager.AppSettings[KompairDefaults.CertificateStoreLocationAppSetting], out StoreLocation location))
			{
				location = KompairDefaults.CertificateStoreLocation;
			}

			if (!bool.TryParse(ConfigurationManager.AppSettings[KompairDefaults.ValidCertificatesOnlyAppSetting], out bool validCertificatesOnly))
			{
				validCertificatesOnly = KompairDefaults.ValidCertificatesOnly;
			}

			X509Certificate2 clientCertificate = _KompairCertificateService.GetClientCertificate(store, location, thumbprint, validCertificatesOnly);
			X509Certificate2 requestCertificate = Request.GetClientCertificate();

			return requestCertificate?.Thumbprint != null &&
			       clientCertificate?.Thumbprint != null &&
			       clientCertificate.Thumbprint.Equals(requestCertificate.Thumbprint);
		}

		private bool AuthenticateApiKey()
		{
			byte[] content = Encoding.UTF8.GetBytes(Request.RequestUri.ToString());
			string requestAuth = Request.Headers.Authorization.Parameter;

			if (string.IsNullOrWhiteSpace(requestAuth))
			{
				return false;
			}

			string[] splitPayload = _ApiKeyService.GetAuthHeaderValues(requestAuth);

			if (splitPayload == null)
			{
				return false;
			}

			string requestClientId = splitPayload[0];
			string requestPayload = splitPayload[1];
			string requestNonce = splitPayload[2];
			string requestTimestamp = splitPayload[3];

			if (IsReplayRequest(requestNonce, requestTimestamp))
			{
				return false;
			}

			string clientConfigPath = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[KompairDefaults.ApiKeyClientsConfigPathAppSetting])
				? ConfigurationManager.AppSettings[KompairDefaults.ApiKeyClientsConfigPathAppSetting]
				: KompairDefaults.ApiKeyClientsConfigPath;
			string clientConfigAbsolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, clientConfigPath);

			if (!File.Exists(clientConfigAbsolutePath))
			{
				throw new NotSupportedException("Client config has not been set on the server");
			}

			string clientConfigString = File.ReadAllText(clientConfigAbsolutePath);

			if (string.IsNullOrWhiteSpace(clientConfigString))
			{
				throw new NotSupportedException("Client config has not been set on the server");
			}

			KompairClientConfigCollection clients = JsonConvert.DeserializeObject<KompairClientConfigCollection>(clientConfigString);

			if (clients == null || clients.Clients.Length == 0)
			{
				throw new NotSupportedException("Client config has not been set on the server");
			}

			KompairClientConfig client = clients.Clients.FirstOrDefault(c => c.ClientId == requestClientId);

			if (client == null)
			{
				return false;
			}

			string encryptedPayload = _ApiKeyService.EncryptPayload(client.ClientId, client.ClientSecret, requestNonce, requestTimestamp, content);

			return string.Equals(encryptedPayload, requestPayload, StringComparison.Ordinal);
		}

		private bool IsReplayRequest(string nonce, string requestTimeStamp)
		{
			string nonceCacheKey = KompairDefaults.CacheKey + nonce;

			if (ApplicationContext.ApplicationCache.RuntimeCache.GetCacheItem(nonceCacheKey) != null)
			{
				return true;
			}
		
			ulong serverTotalSeconds = Convert.ToUInt64(_ApiKeyService.GetTimestampString(DateTime.UtcNow));
			ulong requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

			if (serverTotalSeconds - requestTotalSeconds > RequestMaxAgeInSeconds)
			{
				return true;
			}

			ApplicationContext.ApplicationCache.RuntimeCache.InsertCacheItem(nonceCacheKey, () => nonce, TimeSpan.FromSeconds(RequestMaxAgeInSeconds));

			return false;
		}
	}
}
