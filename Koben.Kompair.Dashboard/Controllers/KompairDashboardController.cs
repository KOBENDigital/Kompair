﻿using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using Koben.Kompair.DTOs;
using Koben.Kompair.Services;
using Koben.Kompair.Dashboard.Services;
using Umbraco.Web.Editors;

namespace Koben.Kompair.Dashboard.Controllers
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
			
			string certificateThumbprint = ConfigurationManager.AppSettings[KompairDefaults.CertificateThumbprintAppSetting];
			string clientId = ConfigurationManager.AppSettings[KompairDefaults.ApiKeyClientIdAppSetting];
			string clientSecret = ConfigurationManager.AppSettings[KompairDefaults.ApiKeyClientSecretAppSetting];

			var config = new KompairHttpServiceConfig
			{
				CertificateThumbprint = certificateThumbprint,
				ClientId = clientId,
				ClientSecret = clientSecret
			};

			if (!Enum.TryParse(ConfigurationManager.AppSettings[KompairDefaults.AuthenticationModeAppSetting], out KompairAuthenticationMode authMode))
			{
				authMode = KompairDefaults.AuthenticationMode;
			}

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

			config.AuthenticationMode = authMode;
			config.CertificateStore = store;
			config.CertificateLocation = location;
			config.ValidCertificatesOnly = validCertificatesOnly;

			_KompairHttpService = new KompairHttpService(config, new KompairCertificateService(), new KompairApiKeyService());
		}
		
		[HttpPost]
		public async Task<IHttpActionResult> Compare(CompareRequest request)
		{
			if (string.IsNullOrWhiteSpace(request?.TargetUrl))
			{
				return BadRequest("targetUrl must be provided");
			}

			string comparePath = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[KompairDefaults.GetDocumentTypesForComparisonPathAppSetting])
				? ConfigurationManager.AppSettings[KompairDefaults.GetDocumentTypesForComparisonPathAppSetting]
				: KompairDefaults.GetDocumentTypesForComparisonPath;
			var source = _KompairDataService.GetDocumentTypesForComparison();
			var target = await _KompairHttpService.GetTargetDocumentTypes(new Uri(new Uri(request.TargetUrl), comparePath));

			var results = _KompairService.Compare(source, target);

			return Json(results);
		}
	}
}
