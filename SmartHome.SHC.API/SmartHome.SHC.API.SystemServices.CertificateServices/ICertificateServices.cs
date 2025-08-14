using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace SmartHome.SHC.API.SystemServices.CertificateServices;

public interface ICertificateServices
{
	List<byte[]> AddCertificate(string appId, byte[] cert, CertificateStore store);

	void RemoveCertificate(string appId, byte[] thumbprint, CertificateStore store);

	void RegisterCertificateValidator(string server, Func<ServicePoint, X509Certificate, WebRequest, int, bool> validationFunction);

	void UnregisterCertificateValidator(string server);
}
