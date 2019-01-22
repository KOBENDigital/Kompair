using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Koben.Kompair.DTOs;
using Koben.Kompair.WebRequestHandlers;
using Newtonsoft.Json;

namespace Koben.Kompair.Services
{
	public class KompairHttpService : IKompairHttpService
	{
		private readonly KompairHttpServiceConfig _Config;
		private readonly IKompairCertificateService _CertificateService;
		private readonly IKompairApiKeyService _ApiKeyService;

		public KompairHttpService(KompairHttpServiceConfig config, IKompairCertificateService certificateService,
		                          IKompairApiKeyService apiKeyService)
		{
			_Config = config ?? throw new ArgumentNullException(nameof(config));
			_CertificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
			_ApiKeyService = apiKeyService ?? throw new ArgumentNullException(nameof(apiKeyService));
		}

		public async Task<KompairDocumentTypesAndEditors> GetTargetDocumentTypes(Uri targetUrl)
		{
			WebRequestHandler requestHandler = null;

			switch (_Config.AuthenticationMode)
			{
				case KompairAuthenticationMode.Certificate:
				{
					requestHandler = new WebRequestHandler();

					if (_Config.UseImportCertificateMethod)
					{
						requestHandler.ClientCertificates.AddRange(
							_CertificateService.ImportClientCertificate(_Config.CertificatePath,
							                                            _Config.CertificatePassword));
					}
					else
					{
						var certificate = _CertificateService.GetClientCertificate(_Config.CertificateStore,
						                                                           _Config.CertificateLocation,
						                                                           _Config.CertificateThumbprint,
						                                                           _Config.ValidCertificatesOnly);
						requestHandler.ClientCertificates.Add(certificate);
					}

					if (!_Config.ValidCertificatesOnly)
					{
						ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
					}

					break;
				}
				case KompairAuthenticationMode.Key:
					requestHandler = new KompairApiKeyRequestHandler(new KompairApiKeyRequestHandlerConfig
					{
						ClientId = _Config.ClientId,
						ClientSecret = _Config.ClientSecret
					}, _ApiKeyService);
					break;
			}

			using (HttpClient client = requestHandler == null ? new HttpClient() : new HttpClient(requestHandler))
			{
				HttpResponseMessage response = await client.GetAsync(targetUrl);
				response.EnsureSuccessStatusCode();
				string responseContent = await response.Content.ReadAsStringAsync();

				if (string.IsNullOrWhiteSpace(responseContent))
				{
					return null;
				}

				return JsonConvert.DeserializeObject<KompairDocumentTypesAndEditors>(responseContent);
			}
		}
	}

	public class KompairHttpServiceConfig
	{
		public StoreName CertificateStore { get; set; }
		public StoreLocation CertificateLocation { get; set; }
		public string CertificateThumbprint { get; set; }
		public string CertificatePath { get; set; }
		public string CertificatePassword { get; set; }
		public bool ValidCertificatesOnly { get; set; }
		public bool UseImportCertificateMethod { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public KompairAuthenticationMode AuthenticationMode { get; set; }
	}
}