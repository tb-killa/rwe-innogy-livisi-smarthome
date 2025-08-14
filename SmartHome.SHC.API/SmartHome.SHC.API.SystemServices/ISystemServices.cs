using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using SmartHome.SHC.API.SystemServices.CertificateServices;
using SmartHome.SHC.API.SystemServices.Dns;
using SmartHome.SHC.API.SystemServices.WebSocketsService;

namespace SmartHome.SHC.API.SystemServices;

public interface ISystemServices
{
	ICertificateServices CertificateServices { get; }

	IDnsResolver CreateMDnsResolver();

	[Obsolete("Deprecated, use the methods available via the CertificateServices property")]
	void RegisterCertificateValidator(string server, Func<ServicePoint, X509Certificate, WebRequest, int, bool> validationFunction);

	[Obsolete("Deprecated, use the methods available via the CertificateServices property")]
	void UnregisterCertificateValidator(string server);

	IWebSocketSecureClient GetWebSocketSecureClient(WSOptions options);
}
