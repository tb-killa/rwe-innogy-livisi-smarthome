using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class ValveActuatorHandler : IActuatorHandlerEntityTypes, IActuatorHandler, ILogicalDeviceHandler
{
	public IEnumerable<Type> SupportedActuatorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(ValveActuator));
			return list;
		}
	}

	public IEnumerable<byte> StatusInfoChannels => new byte[1] { 1 };

	public bool IsStatusRequestAllowed => true;

	public int MinStatusRequestPollingIterval => 0;

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		return null;
	}

	public bool GetIsPeriodicStatusPollingActive(LogicalDevice logicalDevice)
	{
		if (logicalDevice != null && logicalDevice.BaseDevice != null)
		{
			return logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.FSC8;
		}
		return false;
	}

	public IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action)
	{
		throw new InvalidOperationException("A valve cannot be controlled directly.");
	}

	public bool CanExecuteAction(ActionDescription action)
	{
		return false;
	}
}
