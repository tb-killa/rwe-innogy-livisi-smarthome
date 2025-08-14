using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.DomainModel.Actions;
using SmartHome.SHC.API.Protocols.Lemonbeat;
using SmartHome.SHC.API.Protocols.wMBus;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public interface IApplicationsHost
{
	event Action<ApplicationLoadStateChangedEventArgs> ApplicationStateChanged;

	event Action ApplicationsLoaded;

	T GetCustomDevice<T>(string applicationId) where T : class;

	IEnumerable<T> GetCustomDevices<T>() where T : class;

	ExecutionResult ExecuteAction(string appId, ActionDescription action);

	IWMBusDeviceHandler GetWMBusDeviceHandler(WMBusDeviceTypeIdentifier deviceType);

	IDeviceHandler GetLemonbeatDeviceHandler(LemonbeatDeviceTypeIdentifier deviceType);

	string GetAddinVersion(string appId);

	void DropDiscoveredDevices(BaseDevice[] devices);

	void LoadApplication(ApplicationTokenEntry appEntry, bool lastAttempt, bool isNewApplication);

	void CleanupAppRepository(ApplicationsToken token);
}
