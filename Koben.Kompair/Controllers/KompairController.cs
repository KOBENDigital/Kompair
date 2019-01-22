using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Koben.SanityCheck.Services;
using Umbraco.Web.WebApi;

namespace Koben.SanityCheck.Controllers
{
	public class KompairController : UmbracoApiController
	{
		private readonly IKompairDataService _KompairDataService;
		private readonly IKompairCertificateService _KompairCertificateService;

		public KompairController()
		{
			_KompairDataService = new KompairDataService(new KompairContentTypesService(), new KompairDataTypesService());
			_KompairCertificateService = new KompairCertificateService();
		}

		[HttpGet]
		public IHttpActionResult GetDocumentTypesForComparison()
		{
			if (!bool.TryParse(ConfigurationManager.AppSettings[KompairDefaults.SkipAuthenticationAppSetting], out bool skipAuthentication))
			{
				skipAuthentication = KompairDefaults.SkipAuthentication;
			}

			if (skipAuthentication)
			{
				return Json(_KompairDataService.GetDocumentTypesForComparison());
			}

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

			if (requestCertificate?.Thumbprint == null ||
			    clientCertificate?.Thumbprint == null || 
			    !clientCertificate.Thumbprint.Equals(requestCertificate.Thumbprint))
			{
				AuthenticationHeaderValue authHeaderValue = new AuthenticationHeaderValue("ClientCert");
				return Unauthorized(authHeaderValue);
			}

			return Json(_KompairDataService.GetDocumentTypesForComparison());
		}
	}
}
