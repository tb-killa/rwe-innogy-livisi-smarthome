using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.ApplicationsHost.SystemServices.CertificateServices;
using RWE.SmartHome.SHC.ApplicationsHost.SystemServices.WebSocketsService;
using RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Scheduler;
using SmartHome.SHC.API.SystemServices;
using SmartHome.SHC.API.SystemServices.CertificateServices;
using SmartHome.SHC.API.SystemServices.Dns;
using SmartHome.SHC.API.SystemServices.WebSocketsService;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices;

public class SystemServices : ISystemServices
{
	private readonly IScheduler scheduler;

	private readonly ICertificateServices certificateServices;

	public ICertificateServices CertificateServices => certificateServices;

	public SystemServices(ICertificateManager certificateManager, IScheduler scheduler)
	{
		this.scheduler = scheduler;
		certificateServices = new RWE.SmartHome.SHC.ApplicationsHost.SystemServices.CertificateServices.CertificateServices(certificateManager);
	}

	public IDnsResolver CreateMDnsResolver()
	{
		return new MDnsResolver();
	}

	[Obsolete("Deprecated, use the methods available via the CertificateServices property")]
	public void RegisterCertificateValidator(string server, Func<ServicePoint, X509Certificate, WebRequest, int, bool> validationFunction)
	{
		CertificateValidationHook.RegisterCertificateValidator(server, validationFunction);
	}

	[Obsolete("Deprecated, use the methods available via the CertificateServices property")]
	public void UnregisterCertificateValidator(string server)
	{
		CertificateValidationHook.UnregisterCertificateValidator(server);
	}

	public IWebSocketSecureClient GetWebSocketSecureClient(WSOptions options)
	{
		return new WebSocketSecureClientWrapper("", options);
	}
}
