using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices;

public class CertificateValidationHook : ICertificatePolicy
{
	private ICertificatePolicy originalPolicy;

	private Dictionary<string, Func<ServicePoint, X509Certificate, WebRequest, int, bool>> validators = new Dictionary<string, Func<ServicePoint, X509Certificate, WebRequest, int, bool>>();

	private static CertificateValidationHook instance;

	private static object syncRoot = new object();

	public static CertificateValidationHook Instance
	{
		get
		{
			if (instance == null)
			{
				lock (syncRoot)
				{
					if (instance == null)
					{
						instance = new CertificateValidationHook();
					}
				}
			}
			return instance;
		}
	}

	private CertificateValidationHook()
	{
		originalPolicy = ServicePointManager.CertificatePolicy;
		ServicePointManager.CertificatePolicy = this;
	}

	public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
	{
		if (originalPolicy.CheckValidationResult(srvPoint, certificate, request, certificateProblem))
		{
			return true;
		}
		Func<ServicePoint, X509Certificate, WebRequest, int, bool> value;
		lock (syncRoot)
		{
			if (!validators.TryGetValue(srvPoint.Address.Host, out value))
			{
				return false;
			}
		}
		return value(srvPoint, certificate, request, certificateProblem);
	}

	public static void RegisterCertificateValidator(string server, Func<ServicePoint, X509Certificate, WebRequest, int, bool> validationFunction)
	{
		if (string.IsNullOrEmpty(server) || validationFunction == null)
		{
			return;
		}
		lock (syncRoot)
		{
			Instance.validators[server] = validationFunction;
		}
	}

	public static void UnregisterCertificateValidator(string server)
	{
		if (string.IsNullOrEmpty(server))
		{
			return;
		}
		lock (syncRoot)
		{
			Instance.validators.Remove(server);
		}
	}
}
