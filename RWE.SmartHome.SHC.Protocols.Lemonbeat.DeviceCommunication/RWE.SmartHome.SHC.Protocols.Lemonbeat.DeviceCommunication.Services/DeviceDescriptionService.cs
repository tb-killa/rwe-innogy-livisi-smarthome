using System;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.DeviceDescription;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class DeviceDescriptionService : DuplexService<network>, IDeviceDescriptionService
{
	private const string DefaultNamespace = "urn:device_descriptionxsd";

	public event EventHandler<DeviceDescriptionReceivedArgs> DeviceDescriptionReceived;

	public DeviceDescriptionService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.DeviceDescription, "urn:device_descriptionxsd")
	{
	}

	protected override void Handle(int gatewayId, IPAddress remoteAddress, network message)
	{
		if (message == null || message.device == null)
		{
			return;
		}
		networkDevice[] device = message.device;
		foreach (networkDevice networkDevice in device)
		{
			foreach (deviceDescriptionType item in networkDevice.Items.OfType<deviceDescriptionType>())
			{
				DeviceIdentifier identifier = new DeviceIdentifier(remoteAddress, networkDevice.device_idSpecified ? new uint?(networkDevice.device_id) : ((uint?)null), gatewayId);
				OnDeviceDescriptionReceived(identifier, item);
			}
		}
	}

	private void OnDeviceDescriptionReceived(DeviceIdentifier identifier, deviceDescriptionType message)
	{
		try
		{
			DeviceDescription deviceDescription = CreateDeviceDescription(message);
			DeviceDescriptionReceivedArgs e = new DeviceDescriptionReceivedArgs(identifier, deviceDescription);
			this.DeviceDescriptionReceived?.Invoke(this, e);
		}
		catch (Exception ex)
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, "Could not handle device description: " + ex);
		}
	}

	private static DeviceDescription CreateDeviceDescription(deviceDescriptionType message)
	{
		SGTIN96 sGTIN = SGTIN96.Create(message.info.Single((infoType i) => i.type_id == 3).hex);
		infoType infoType = message.info.SingleOrDefault((infoType i) => i.type_id == 1);
		infoType infoType2 = message.info.SingleOrDefault((infoType i) => i.type_id == 16);
		infoType infoType3 = message.info.SingleOrDefault((infoType i) => i.type_id == 14);
		infoType infoType4 = message.info.SingleOrDefault((infoType i) => i.type_id == 15);
		infoType infoType5 = message.info.SingleOrDefault((infoType i) => i.type_id == 6);
		infoType infoType6 = message.info.SingleOrDefault((infoType i) => i.type_id == 7);
		infoType infoType7 = message.info.SingleOrDefault((infoType i) => i.type_id == 8);
		infoType infoType8 = message.info.SingleOrDefault((infoType i) => i.type_id == 17);
		infoType infoType9 = message.info.SingleOrDefault((infoType i) => i.type_id == 18);
		DeviceDescription deviceDescription = new DeviceDescription();
		deviceDescription.DeviceType = ((infoType != null) ? new uint?((uint)infoType.number) : ((uint?)null));
		deviceDescription.Included = (int)message.info.Single((infoType i) => i.type_id == 11).number == 1;
		deviceDescription.MacAddress = message.info.Single((infoType i) => i.type_id == 4).hex;
		deviceDescription.ManufacturerId = (uint)message.info.Single((infoType i) => i.type_id == 2).number;
		deviceDescription.ManufacturerProductId = (uint)message.info.Single((infoType i) => i.type_id == 10).number;
		deviceDescription.Name = message.info.Single((infoType i) => i.type_id == 12).@string;
		deviceDescription.RadioMode = (RadioMode)message.info.Single((infoType i) => i.type_id == 13).number;
		deviceDescription.SGTIN = sGTIN;
		deviceDescription.HardwareVersion = message.info.Single((infoType i) => i.type_id == 5).@string;
		deviceDescription.WakeupChannel = ((infoType2 != null) ? new uint?((uint)infoType2.number) : ((uint?)null));
		deviceDescription.WakeupInterval = ((infoType3 != null) ? new uint?((uint)infoType3.number) : ((uint?)null));
		deviceDescription.WakeupOffset = ((infoType4 != null) ? new uint?((uint)infoType4.number) : ((uint?)null));
		deviceDescription.BootloaderVersion = ((infoType5 != null) ? infoType5.@string : string.Empty);
		deviceDescription.StackVersion = ((infoType6 != null) ? infoType6.@string : string.Empty);
		deviceDescription.ApplicationVersion = ((infoType7 != null) ? infoType7.@string : string.Empty);
		deviceDescription.ChannelMap = ((infoType8 != null) ? new uint?((uint)infoType8.number) : ((uint?)null));
		deviceDescription.ChannelScanTime = ((infoType9 != null) ? new uint?((uint)infoType9.number) : ((uint?)null));
		return deviceDescription;
	}

	public DeviceDescription GetDeviceDescription(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(isSet: false);
		network.device[0].Items = new object[1]
		{
			new deviceDescriptionGetType()
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		network network2 = SendRequest(identifier, network);
		DeviceDescription result = null;
		if (network2 != null && network2.device != null && network2.device.Length > 0)
		{
			deviceDescriptionType[] array = network2.device[0].Items.OfType<deviceDescriptionType>().ToArray();
			if (array.Length > 0)
			{
				try
				{
					result = CreateDeviceDescription(array[0]);
				}
				catch (Exception ex)
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, "Could not extract device description: " + ex);
				}
			}
		}
		return result;
	}

	public void IncludeDevice(DeviceIdentifier identifier)
	{
		SetInclusionProperty(identifier, isIncluded: true);
	}

	public void ExcludeDevice(DeviceIdentifier identifier)
	{
		SetInclusionProperty(identifier, isIncluded: false);
	}

	public void UpdateDeviceTimezone(DeviceIdentifier identifier, string newOffset)
	{
	}

	private void SetInclusionProperty(DeviceIdentifier identifier, bool isIncluded)
	{
		network network = CreateNetworkMessage(isSet: true);
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		(network.device[0].Items[0] as deviceDescriptionType).info = new infoType[1]
		{
			new infoType
			{
				type_id = 11u,
				number = (ulong)(isIncluded ? 1 : 0),
				numberSpecified = true
			}
		};
		SendMessage(identifier, network, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Connection);
	}

	private network CreateNetworkMessage(bool isSet)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				ItemsElementName = new ItemsChoiceType[1] { isSet ? ItemsChoiceType.device_description_set : ItemsChoiceType.device_description_get },
				Items = new deviceDescriptionType[1]
				{
					new deviceDescriptionType()
				}
			}
		};
		return network;
	}
}
