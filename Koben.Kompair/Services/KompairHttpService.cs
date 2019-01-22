using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Koben.SanityCheck.DTOs;
using Newtonsoft.Json;

namespace Koben.SanityCheck.Services
{
	public class KompairHttpService : IKompairHttpService
	{
		private readonly KompairHttpServiceConfig _Config;
		private readonly IKompairCertificateService _CertificateService;

		public KompairHttpService(KompairHttpServiceConfig config, IKompairCertificateService certificateService)
		{
			_Config = config ?? throw new ArgumentNullException(nameof(config));
			_CertificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
		}

		public async Task<KompairDocumentTypesAndEditors> GetTargetDocumentTypes(Uri targetUrl)
		{
			WebRequestHandler requestHandler = new WebRequestHandler();

			if (_Config.UseImportMethod)
			{
				requestHandler.ClientCertificates.AddRange(_CertificateService.ImportClientCertificate(_Config.CerificatePath, _Config.CerificatePassword));
			}
			else
			{
				var certificate = _CertificateService.GetClientCertificate(_Config.Store, _Config.Location, _Config.CerificateThumbprint, _Config.ValidCerificatesOnly);
				requestHandler.ClientCertificates.Add(certificate);
			}

			if (!_Config.ValidCerificatesOnly)
			{
				ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
			}

			using (HttpClient client = new HttpClient(requestHandler))
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
		public StoreName Store { get; set; }
		public StoreLocation Location { get; set; }
		public string CerificateThumbprint { get; set; }
		public string CerificatePath { get; set; }
		public string CerificatePassword { get; set; }
		public bool ValidCerificatesOnly { get; set; }
		public bool UseImportMethod { get; set; }
	}
}