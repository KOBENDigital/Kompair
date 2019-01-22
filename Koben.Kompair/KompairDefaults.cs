using System.Security.Cryptography.X509Certificates;
using Koben.Kompair;

namespace Koben.SanityCheck
{
	public static class KompairDefaults
	{
		public static string GetDocumentTypesForComparisonPath = "/umbraco/api/Kompair/GetDocumentTypesForComparison";
		public static string CertificateThumbprintAppSetting = "Kompair.CerificateThumbprint";
		public static string CertificateStoreNameAppSetting = "Kompair.StoreName";
		public static string CertificateStoreLocationAppSetting = "Kompair.StoreLocation";
		public static string ValidCertificatesOnlyAppSetting = "Kompair.ValidCertificatesOnly";
		public static string AuthenticationModeAppSetting = "Kompair.AuthenticationMode";

		public static StoreName CertificateStoreName = StoreName.My;
		public static StoreLocation CertificateStoreLocation = StoreLocation.CurrentUser;
		public static bool ValidCertificatesOnly = true;
		public static KompairAuthenticationMode AuthenticationMode = KompairAuthenticationMode.Certificate;
	}
}
