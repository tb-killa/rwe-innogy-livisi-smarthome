using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace RWE.SmartHome.SHC.Core;

public interface ICertificateManager : IService
{
	string DefaultCertificateThumbprint { get; }

	string PersonalCertificateThumbprint { get; }

	void ExtractPersonalCertificateThumbprint();

	void DeletePersonalCertificate();

	X509Certificate2 GetPersonalCertificate();

	List<byte[]> AddCertificate(string appId, byte[] cert, string storeId);

	void RemoveCertificate(string appId, byte[] thumbprint, string storeId);

	void CleanupCertificates(string appId);
}
