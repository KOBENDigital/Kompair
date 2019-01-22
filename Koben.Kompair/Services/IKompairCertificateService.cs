using System.Security.Cryptography.X509Certificates;

namespace Koben.SanityCheck.Services
{
	public interface IKompairCertificateService
	{
		X509Certificate2 GetClientCertificate(StoreName store, StoreLocation location, string thumbprint,
		                                      bool onlyValidCertificates);

		X509Certificate2Collection ImportClientCertificate(string certificatePath, string password);
	}
}