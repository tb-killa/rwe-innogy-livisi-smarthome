using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Persistence;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal class WMBusProtocolAdapter : IProtocolAdapter
{
	private readonly IDeviceList deviceList;

	private readonly IDeviceListPersistence deviceListPersistence;

	private readonly IProtocolSpecificPhysicalStateHandler physicalStateHandler;

	private readonly WMBusTransformation wMBusTransformation;

	private readonly WMBusManager wmBusManager;

	public ProtocolIdentifier ProtocolId => ProtocolIdentifier.wMBus;

	public IProtocolSpecificLogicalStateRequestor LogicalState => null;

	public IProtocolSpecificPhysicalStateHandler PhysicalState => physicalStateHandler;

	public IProtocolSpecificDeviceController DeviceController => null;

	public IProtocolSpecificTransformation Transformation => wMBusTransformation;

	public IProtocolSpecificDataBackup DataBackup => null;

	public WMBusProtocolAdapter(IEventManager eventManager, IProtocolSpecificDataPersistence protocolSpecificDataPersistence, IApplicationsHost applicationsHost, IDeviceMonitor deviceMonitor, IRepository configRepository, IDeviceKeyRepository deviceKeyRepository)
	{
		deviceList = new DeviceList();
		deviceListPersistence = new DeviceListPersistence(protocolSpecificDataPersistence);
		physicalStateHandler = new WMBusPhysicalStateHandler(deviceList);
		wmBusManager = new WMBusManager(eventManager, deviceList, deviceListPersistence, applicationsHost, deviceMonitor, deviceKeyRepository);
		wMBusTransformation = new WMBusTransformation(wmBusManager, configRepository);
	}

	public IEnumerable<Guid> GetHandledDevices()
	{
		return deviceList.Select((IDeviceInformation deviceInfo) => deviceInfo.DeviceId).ToList();
	}

	public string GetDeviceDescription(Guid deviceId)
	{
		return deviceList.LogInfoByGuid(deviceId);
	}

	public void ResetDeviceInclusionState(Guid deviceId)
	{
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		wmBusManager.DropDiscoveredDevices(devices);
	}

	public ProtocolSpecificInformation GetProtocolSpecificInformation()
	{
		return null;
	}
}
