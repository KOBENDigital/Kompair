using System;

namespace Koben.Kompair.Dashboard.Services
{
	public interface IKompairApiKeyService
	{
		string GetNonce();
		string GetTimestampString(DateTime utcNow);

		string EncryptPayload(string clientId, string clientSecret, string nonce, string timestamp, string data);
		string EncryptPayload(string clientId, string clientSecret, string nonce, string timestamp, byte[] data);

		string BuildAuthHeaderValue(string clientId, string payload, string nonce, string timestamp);
		string[] GetAuthHeaderValues(string headerValue);
		ulong GetTimestamp(DateTime utcNow);
	}
}
