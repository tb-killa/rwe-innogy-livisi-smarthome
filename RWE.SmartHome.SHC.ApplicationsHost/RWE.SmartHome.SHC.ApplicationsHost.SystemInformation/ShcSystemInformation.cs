using System.IO;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using SmartHome.SHC.API;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemInformation;

internal class ShcSystemInformation : ISystemInformation
{
	private readonly ICertificateManager certificateManager;

	private readonly INetworkingMonitor networkMonitor;

	private readonly IEventManager eventManager;

	private readonly IRepository configRepository;

	bool ISystemInformation.IsInternetAccessAllowed => networkMonitor.InternetAccessAllowed;

	string ISystemInformation.ShcPersonalizedCertificateThumbprint => certificateManager.PersonalCertificateThumbprint;

	string ISystemInformation.ShcSerialNumber => SHCSerialNumber.SerialNumber();

	bool ISystemInformation.LocalCommunicationEnabled => FilePersistence.LocalAccessEnabled;

	bool ISystemInformation.IsShcLocalOnly => File.Exists(LocalCommunicationConstants.IsShcLocalOnlyFlagPath);

	public ShcSystemInformation(ICertificateManager certificateManager, INetworkingMonitor networkMonitor, IEventManager eventManager, IRepository configRepository)
	{
		this.certificateManager = certificateManager;
		this.networkMonitor = networkMonitor;
		this.eventManager = eventManager;
		this.configRepository = configRepository;
	}
}
