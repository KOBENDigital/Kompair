using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Koben.Kompair.Services;

namespace Koben.Kompair.WebRequestHandlers
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
			if (request.Content == null)
			{
				return await base.SendAsync(request, cancellationToken);
			}

			byte[] content = await request.Content.ReadAsByteArrayAsync();

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
