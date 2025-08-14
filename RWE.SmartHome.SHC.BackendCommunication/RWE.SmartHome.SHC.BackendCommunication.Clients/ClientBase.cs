using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using Microsoft.Tools.ServiceModel;
using RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;
using RWE.SmartHome.SHC.BackendCommunication.ErrorHandling;
using RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.ErrorHandling;
using SmartHome.SHC.BackendCommunication.KeyExchangeScope;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients;

internal abstract class ClientBase<TClient, TInterface> where TClient : CFClientBase<TInterface> where TInterface : class
{
	private const string ModuleName = "BackendCommunication";

	protected EndpointAddress Address { get; private set; }

	private INetworkingMonitor NetworkingMonitor { get; set; }

	private IRegistrationService RegistrationService { get; set; }

	protected ClientBase(INetworkingMonitor networkingMonitor, string endpointAddress, IRegistrationService registrationService)
	{
		NetworkingMonitor = networkingMonitor;
		Address = new EndpointAddress(endpointAddress);
		RegistrationService = registrationService;
	}

	protected TClient CreateClient(string certificateThumbprint, Func<TClient> constructorCall)
	{
		try
		{
			CheckForInternetAccessSwitch();
			TClient val = constructorCall();
			IsShcRegisteredInBackend(val);
			val.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, certificateThumbprint);
			return val;
		}
		catch (ShcException ex)
		{
			if (ex.ErrorCode == 0 || ex.ErrorCode == 1)
			{
				return null;
			}
			throw;
		}
	}

	protected void CheckForInternetAccessSwitch()
	{
		if (!NetworkingMonitor.InternetAccessAllowed)
		{
			throw new ShcException(ErrorStrings.InternetAccessDisabled, "BackendCommunication", 0);
		}
	}

	protected void IsShcRegisteredInBackend(TClient client)
	{
		if (client is KeyExchangeServiceClient || client is SoftwareUpdateClient || client is SoftwareUpdateServiceClient || client is ApplicationTokenServiceClient || !RegistrationService.IsShcLocalOnly)
		{
			return;
		}
		throw new ShcException("SHC is local only", "BackendCommunication", 1);
	}
}
