using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Koben.Kompair.Dashboard.Services;

namespace Koben.Kompair.Dashboard.WebRequestHandlers
{
	public class KompairApiKeyRequestHandler : WebRequestHandler
	{
		private readonly KompairApiKeyRequestHandlerConfig _Config;
		private readonly IKompairApiKeyService _ApiKeyService;

		public KompairApiKeyRequestHandler(KompairApiKeyRequestHandlerConfig config, IKompairApiKeyService apiKeyService)
		{
			_Config = config ?? throw new ArgumentNullException(nameof(config));
			_ApiKeyService = apiKeyService ?? throw new ArgumentNullException(nameof(apiKeyService));
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			byte[] content = Encoding.UTF8.GetBytes(request.RequestUri.ToString());
			string timestamp = _ApiKeyService.GetTimestampString(DateTime.UtcNow);
			string nonce = _ApiKeyService.GetNonce();
			string encryptedPayload = _ApiKeyService.EncryptPayload(_Config.ClientId, _Config.ClientSecret, nonce, timestamp, content);
			string authHeaderValue = _ApiKeyService.BuildAuthHeaderValue(_Config.ClientId, encryptedPayload, nonce, timestamp);

			request.Headers.Authorization = new AuthenticationHeaderValue(KompairDefaults.ApiKeyAuthHeader, authHeaderValue);

			return await base.SendAsync(request, cancellationToken);
		}
	}

	public class KompairApiKeyRequestHandlerConfig
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
	}
}
