using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

internal class SipCosLogicalStateRequestor : IProtocolSpecificLogicalStateRequestor
{
	private readonly IDeviceManager deviceManager;

	private readonly LogicalDeviceHandlerCollection logicalDeviceHandlerCollection;

	private readonly IRepository configurationRepository;

	public ProtocolIdentifier ProtocolId => ProtocolIdentifier.Cosip;

	public SipCosLogicalStateRequestor(IDeviceManager deviceManager, LogicalDeviceHandlerCollection logicalDeviceHandlerCollection, IRepository configurationRepository)
	{
		this.configurationRepository = configurationRepository;
		this.logicalDeviceHandlerCollection = logicalDeviceHandlerCollection;
		this.deviceManager = deviceManager;
	}

	private void RequestStatusInfoForPhysicalDevice(Guid physicalDeviceId, IActuatorHandler actuatorHandler)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[physicalDeviceId];
		if (deviceInformation == null || deviceInformation.DeviceInclusionState != DeviceInclusionState.Included || !actuatorHandler.IsStatusRequestAllowed)
		{
			return;
		}
		IEnumerable<byte> statusInfoChannels = actuatorHandler.StatusInfoChannels;
		IDeviceController deviceController = deviceManager[deviceInformation];
		List<byte> list = new List<byte>();
		foreach (byte item in statusInfoChannels)
		{
			if (list.IndexOf(item) == -1)
			{
				deviceController.RequestStatusInfo(item);
				list.Add(item);
			}
		}
	}

	private void RequestStatusInfoForPhysicalDevice(Guid physicalDeviceId, ISensorHandler actuatorHandler)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[physicalDeviceId];
		if (deviceInformation == null || deviceInformation.DeviceInclusionState != DeviceInclusionState.Included || !actuatorHandler.IsStatusRequestAllowed)
		{
			return;
		}
		IEnumerable<byte> statusInfoChannels = actuatorHandler.StatusInfoChannels;
		IDeviceController deviceController = deviceManager[deviceInformation];
		List<byte> list = new List<byte>();
		foreach (byte item in statusInfoChannels)
		{
			if (list.IndexOf(item) == -1)
			{
				deviceController.RequestStatusInfo(item);
				list.Add(item);
			}
		}
	}

	public void RequestState(LogicalDevice logicalDevice)
	{
		if (logicalDeviceHandlerCollection.TryGetHandler(logicalDevice, out var logicalDeviceHandler))
		{
			if (logicalDeviceHandler is IActuatorHandler actuatorHandler)
			{
				RequestStatusInfoForPhysicalDevice(logicalDevice.BaseDeviceId, actuatorHandler);
			}
			if (logicalDeviceHandler is ISensorHandler actuatorHandler2)
			{
				RequestStatusInfoForPhysicalDevice(logicalDevice.BaseDeviceId, actuatorHandler2);
			}
		}
	}

	public void RequestState(BaseDevice baseDevice)
	{
		foreach (LogicalDevice item in from ld in configurationRepository.GetOriginalLogicalDevices()
			where ld.BaseDeviceId == baseDevice.Id
			select ld)
		{
			RequestState(item);
		}
	}
}
