using System;
using System.Security.Cryptography.X509Certificates;

namespace Koben.Kompair.Services
{
	public class KompairCertificateService : IKompairCertificateService
	{
		public X509Certificate2 GetClientCertificate(StoreName store, StoreLocation location, string thumbprint, bool onlyValidCertificates)
		{
			X509Store userCaStore = new X509Store(store, location);
			try
			{
				userCaStore.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
				X509Certificate2Collection findResult = certificatesInStore.Find(X509FindType.FindByThumbprint, thumbprint, onlyValidCertificates);
				X509Certificate2 clientCertificate;

				if (findResult.Count == 1)
				{
					clientCertificate = findResult[0];
				}
				else
				{
					throw new Exception("Unable to locate the correct client certificate.");
				}

				return clientCertificate;
			}
			finally
			{
				userCaStore.Close();
			}
		}
		public X509Certificate2Collection ImportClientCertificate(string certificatePath, string password)
		{
			X509Certificate2Collection certificates = new X509Certificate2Collection();
			certificates.Import(certificatePath, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
			return certificates;
		}
	}
}