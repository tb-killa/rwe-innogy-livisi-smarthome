using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.Core;
using SmartHome.SHC.API.SystemServices.CertificateServices;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.CertificateServices;

public class CertificateServices : ICertificateServices
{
	private readonly ICertificateManager manager;

	public CertificateServices(ICertificateManager manager)
	{
		this.manager = manager;
	}

	public List<byte[]> AddCertificate(string appId, byte[] cert, CertificateStore store)
	{
		string storeId = APICertificateStoreToMentalisCertificateStore(store);
		return manager.AddCertificate(appId, cert, storeId);
	}

	public void RemoveCertificate(string appId, byte[] thumbprint, CertificateStore store)
	{
		string storeId = APICertificateStoreToMentalisCertificateStore(store);
		manager.RemoveCertificate(appId, thumbprint, storeId);
	}

	public void RegisterCertificateValidator(string server, Func<ServicePoint, X509Certificate, WebRequest, int, bool> validationFunction)
	{
		CertificateValidationHook.RegisterCertificateValidator(server, validationFunction);
	}

	public void UnregisterCertificateValidator(string server)
	{
		CertificateValidationHook.UnregisterCertificateValidator(server);
	}

	private static string APICertificateStoreToMentalisCertificateStore(CertificateStore store)
	{
		return store switch
		{
			CertificateStore.CAStore => "CA", 
			CertificateStore.MyStore => "My", 
			CertificateStore.RootStore => "Root", 
			CertificateStore.SoftwarePublisherStore => "SPC", 
			CertificateStore.TrustStore => "Trust", 
			CertificateStore.UnTrustedStore => "Disallowed", 
			_ => "Disallowed", 
		};
	}
}
