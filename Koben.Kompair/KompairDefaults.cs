using System.Security.Cryptography.X509Certificates;
using Koben.Kompair;

namespace Koben.Kompair
{
	public static class KompairDefaults
	{
		public static string GetDocumentTypesForComparisonPath = "/umbraco/api/Kompair/GetDocumentTypesForComparison";
		public static string ApiKeyClientsConfigPath = @"App_Plugins\Koben.Kompair\clients.json";
		public static string ApiKeyAuthHeader = "X-KOBEN-KOMPAIR-AUTH";
		public static string CacheKey = "Kompair.Cache.Nonce-";

		public static string CertificateThumbprintAppSetting = "Kompair.Certificate.Thumbprint";
		public static string CertificatePathAppSetting = "Kompair.Certificate.Path";
		public static string CertificatePasswordAppSetting = "Kompair.Certificate.Password";
		public static string CertificateStoreNameAppSetting = "Kompair.Certificate.StoreName";
		public static string CertificateStoreLocationAppSetting = "Kompair.Certificate.StoreLocation";
		public static string ValidCertificatesOnlyAppSetting = "Kompair.Certificate.ValidOnly";
		public static string CertificateUseImportMethodAppSetting = "Kompair.Certificate.UseImportMethod";
		public static string AuthenticationModeAppSetting = "Kompair.AuthenticationMode";
		public static string ApiKeyClientIdAppSetting = "Kompair.ApiKey.ClientId";
		public static string ApiKeyClientSecretAppSetting = "Kompair.ApiKey.ClientSecret";
		public static string ApiKeyClientsConfigPathAppSetting = "Kompair.ApiKey.ClientsConfigPath";
		public static string GetDocumentTypesForComparisonPathAppSetting = "Kompair.GetDocumentTypesForComparisonPath";

		public static StoreName CertificateStoreName = StoreName.My;
		public static StoreLocation CertificateStoreLocation = StoreLocation.CurrentUser;
		public static bool ValidCertificatesOnly = true;
		public static bool UseImportCertificateMethod = false;
		public static KompairAuthenticationMode AuthenticationMode = KompairAuthenticationMode.Certificate;
	}
}
