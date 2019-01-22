using System;
using System.Security.Cryptography;
using System.Text;

namespace Koben.Kompair.Services
{
	public class KompairApiKeyService : IKompairApiKeyService
	{
		public const int ExpectedSplitAuthHeaderLength = 4;

		public string GetNonce()
		{
			return Guid.NewGuid().ToString("N");
		}

		public ulong GetTimestamp(DateTime utcNow)
		{
			DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan timeSpan = utcNow - epochStart;
			return Convert.ToUInt64(timeSpan.TotalSeconds);
		}

		public string GetTimestampString(DateTime utcNow)
		{
			return GetTimestamp(utcNow).ToString();
		}

		public string EncryptPayload(string clientId, string clientSecret, string nonce, string timestamp, string data)
		{
			return EncryptPayload(clientId, clientSecret, nonce, timestamp, Encoding.UTF8.GetBytes(data));
		}

		public string EncryptPayload(string clientId, string clientSecret, string nonce, string timestamp, byte[] data)
		{
			MD5 md5 = MD5.Create();
			byte[] requestContentHash = md5.ComputeHash(data);
			string requestContentBase64String = Convert.ToBase64String(requestContentHash);

			string signatureRawData = $"{clientId}{timestamp}{nonce}{requestContentBase64String}";

			byte[] secretKeyByteArray = Encoding.UTF8.GetBytes(clientSecret);
			byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

			using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
			{
				byte[] signatureBytes = hmac.ComputeHash(signature);
				return Convert.ToBase64String(signatureBytes);
			}
		}

		public string BuildAuthHeaderValue(string clientId, string payload, string nonce, string timestamp)
		{
			return $"{clientId}:{payload}:{nonce}:{timestamp}";
		}

		public string[] GetAuthHeaderValues(string headerValue)
		{
			var splitPayload = headerValue.Split(':');

			return splitPayload.Length == ExpectedSplitAuthHeaderLength ? splitPayload : null;
		}
	}
}