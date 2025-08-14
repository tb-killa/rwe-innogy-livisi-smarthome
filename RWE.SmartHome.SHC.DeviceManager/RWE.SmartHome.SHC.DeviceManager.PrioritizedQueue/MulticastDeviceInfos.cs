using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class MulticastDeviceInfos
{
	private class BasicDeviceInfo : IBasicDeviceInformation
	{
		private DeviceConfigurationState deviceConfigurationState;

		public byte[] Address { get; set; }

		public byte SequenceNumber { get; set; }

		public ProtocolType ProtocolType { get; set; }

		public DeviceInfoOperationModes BestOperationMode { get; set; }

		public bool DeviceUnreachable { get; set; }

		public byte Rssi { get; set; }

		public DeviceInclusionState DeviceInclusionState
		{
			get
			{
				return DeviceInclusionState.Included;
			}
			set
			{
			}
		}

		public DeviceConfigurationState DeviceConfigurationState
		{
			get
			{
				Log.Error(Module.DeviceManager, "Device configuration state asked from the wrong place");
				return deviceConfigurationState;
			}
			set
			{
				Log.Error(Module.DeviceManager, "Setting device configuration state in the wrong place");
				deviceConfigurationState = value;
			}
		}

		public Guid DeviceId => Guid.Empty;

		public BasicDeviceInfo(byte[] address, DeviceInfoOperationModes operationModes, ProtocolType protocolType)
		{
			Address = address;
			ProtocolType = protocolType;
			BestOperationMode = operationModes;
		}

		public bool Awake()
		{
			return BestOperationMode == DeviceInfoOperationModes.MainsPowered;
		}

		public void UpdateAwakeState(AwakeModifier awakeModifier)
		{
		}

		public void MarkDeviceAsSleeping()
		{
		}

		public bool IsDeviceSleeping()
		{
			return false;
		}
	}

	public static IBasicDeviceInformation Create(byte[] address)
	{
		if (address.Compare(SipCosAddress.AllDevices))
		{
			return new BasicDeviceInfo(address, DeviceInfoOperationModes.TripleBurstListener, ProtocolType.SipCos);
		}
		if (address.Compare(SipCosAddress.AllMainsPoweredDevices))
		{
			return new BasicDeviceInfo(address, DeviceInfoOperationModes.MainsPowered, ProtocolType.SipCos);
		}
		if (address.Compare(SipCosAddress.AllBurstListeningDevices))
		{
			return new BasicDeviceInfo(address, DeviceInfoOperationModes.BurstListener, ProtocolType.SipCos);
		}
		if (address.Compare(SipCosAddress.AllTripleBurstListeningDevices))
		{
			return new BasicDeviceInfo(address, DeviceInfoOperationModes.TripleBurstListener, ProtocolType.SipCos);
		}
		throw new InvalidOperationException("Address not supported.");
	}

	public static bool IsValidMulticastAddress(byte[] address)
	{
		if (address != null && (address[0] & 0xF0) == 240)
		{
			return address.Length == 3;
		}
		return false;
	}
}
