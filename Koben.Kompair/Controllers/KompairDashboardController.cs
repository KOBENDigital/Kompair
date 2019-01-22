using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Koben.SanityCheck.Services;
using Umbraco.Web.Editors;

namespace Koben.SanityCheck.Controllers
{
	[Umbraco.Web.Mvc.PluginController("Kompair")]
	public class KompairDashboardController : UmbracoAuthorizedJsonController
	{
		private readonly IKompairService _KompairService;
		private readonly IKompairDataService _KompairDataService;
		private readonly IKompairHttpService _KompairHttpService;

		public KompairDashboardController()
		{
			_KompairService = new KompairService();
			_KompairDataService = new KompairDataService(new KompairContentTypesService(), new KompairDataTypesService());
			
			string certificateName = ConfigurationManager.AppSettings[KompairDefaults.CertificateThumbprintAppSetting];

			var config = new KompairHttpServiceConfig
			{
				CerificateThumbprint = certificateName
			};

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

			config.Store = store;
			config.Location = location;
			config.ValidCerificatesOnly = validCertificatesOnly;

			_KompairHttpService = new KompairHttpService(config, new KompairCertificateService());
		}
		
		[HttpPost]
		public async Task<IHttpActionResult> Compare(CompareRequest request)
		{
			if (string.IsNullOrWhiteSpace(request?.TargetUrl))
			{
				return BadRequest("targetUrl must be provided");
			}

			string comparePath = ConfigurationManager.AppSettings["Kompair.GetDocumentTypesForComparisonPath"] ??
			                     KompairDefaults.GetDocumentTypesForComparisonPath;
			var source = _KompairDataService.GetDocumentTypesForComparison();
			var target = await _KompairHttpService.GetTargetDocumentTypes(new Uri(new Uri(request.TargetUrl), comparePath));

			var results = _KompairService.Compare(source, target);

			return Json(results);
		}
	}

	public class CompareRequest
	{
		public string TargetUrl { get; set; }
	}
}
