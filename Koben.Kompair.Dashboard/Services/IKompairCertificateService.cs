using System.Security.Cryptography.X509Certificates;

namespace Koben.Kompair.Dashboard.Services
{
	public interface IKompairCertificateService
	{
		X509Certificate2 GetClientCertificate(StoreName store, StoreLocation location, string thumbprint, bool onlyValidCertificates);
	}
}