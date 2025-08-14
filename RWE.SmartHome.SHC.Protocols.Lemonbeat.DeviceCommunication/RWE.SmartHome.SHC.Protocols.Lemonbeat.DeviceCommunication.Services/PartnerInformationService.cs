using System.Collections.Generic;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class PartnerInformationService : SenderService<network>, IPartnerInformationService
{
	private const string DefaultNamespace = "urn:partner_informationxsd";

	private const int WakeOnRadio = 0;

	private const int WakeOnEvent = 1;

	public PartnerInformationService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.PartnerInformation, "urn:partner_informationxsd")
	{
	}

	public PartnerInformations GetAllPartnersAndGroups(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage();
		network.device[0].Items = new object[1]
		{
			new partnerInformationGetType()
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		network message = SendRequest(identifier, network);
		return ParsePartnerInformation(message, identifier.SubDeviceId ?? 0, identifier.GatewayId);
	}

	public void SetAndDeletePartners(DeviceIdentifier identifier, PartnerInformations partnerInformationsToSet, IEnumerable<uint> partnersToDelete)
	{
		network network = CreateNetworkMessage();
		List<object> list = new List<object>();
		foreach (Partner partner in partnerInformationsToSet.Partners)
		{
			list.Add(new partnerInformationSetType
			{
				Items = new object[1] { FromPartner(partner) }
			});
		}
		foreach (Group group in partnerInformationsToSet.Groups)
		{
			list.Add(new partnerInformationSetType
			{
				Items = new object[1] { FromGroup(group) }
			});
		}
		foreach (uint item in partnersToDelete)
		{
			list.Add(new partnerInformationDeleteType
			{
				partner_id = (byte)item,
				partner_idSpecified = true
			});
		}
		if (list.Count > 0)
		{
			network.device[0].Items = list.ToArray();
			network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
			network.device[0].device_id = identifier.SubDeviceId ?? 0;
			SendMessage(identifier, network, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Connection);
		}
	}

	private network CreateNetworkMessage()
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u
			}
		};
		return network;
	}

	private PartnerInformations ParsePartnerInformation(network message, uint subDeviceId, int gatewayId)
	{
		PartnerInformations partnerInformations = new PartnerInformations();
		if (message != null && message.device != null)
		{
			networkDevice networkDevice = message.device.Where((networkDevice d) => d.device_id == subDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Items != null)
			{
				foreach (partnerInformationReportType item in networkDevice.Items.OfType<partnerInformationReportType>())
				{
					if (item.Items == null)
					{
						continue;
					}
					object[] items = item.Items;
					foreach (object obj in items)
					{
						if (obj is groupType groupType)
						{
							partnerInformations.Groups.Add(ToGroup(groupType));
						}
						if (obj is partnerType partner)
						{
							partnerInformations.Partners.Add(ToPartner(partner, gatewayId));
						}
					}
				}
			}
		}
		return partnerInformations;
	}

	private static infoType CreateInfoField(InfoFieldType fieldType, string value)
	{
		infoType infoType = new infoType();
		infoType.type_id = (uint)fieldType;
		infoType.@string = value;
		return infoType;
	}

	private static infoType CreateInfoField(InfoFieldType fieldType, ulong value)
	{
		infoType infoType = new infoType();
		infoType.type_id = (uint)fieldType;
		infoType.number = value;
		infoType.numberSpecified = true;
		return infoType;
	}

	private static infoType CreateInfoField(InfoFieldType fieldType, byte[] value)
	{
		infoType infoType = new infoType();
		infoType.type_id = (uint)fieldType;
		infoType.hex = value;
		return infoType;
	}

	private static object FromPartner(Partner partner)
	{
		List<infoType> list = new List<infoType>();
		list.Add(CreateInfoField(InfoFieldType.IPV6Address, partner.Identifier.IPAddress.GetAddressBytes()));
		if (partner.WakeupMode.HasValue)
		{
			switch (partner.WakeupMode)
			{
			case RadioMode.WakeOnEvent:
				list.Add(CreateInfoField(InfoFieldType.RadioMode, 1uL));
				break;
			case RadioMode.WakeOnRadio:
				list.Add(CreateInfoField(InfoFieldType.RadioMode, 0uL));
				if (partner.WakeupInterval.HasValue)
				{
					list.Add(CreateInfoField(InfoFieldType.WakeupInterval, partner.WakeupInterval.GetValueOrDefault()));
				}
				if (partner.WakeupOffset.HasValue)
				{
					list.Add(CreateInfoField(InfoFieldType.WakeupOffset, partner.WakeupOffset.GetValueOrDefault()));
				}
				if (partner.WakeupChannel.HasValue)
				{
					list.Add(CreateInfoField(InfoFieldType.WakeupChannel, partner.WakeupChannel.GetValueOrDefault()));
				}
				break;
			}
		}
		partnerType partnerType = new partnerType();
		partnerType.partner_id = (byte)partner.Id;
		partnerType.info = list.ToArray();
		return partnerType;
	}

	private static object FromGroup(Group group)
	{
		groupType groupType = new groupType();
		groupType.partner_id = (byte)group.Id;
		groupType.partner = group.PartnerIds.Select((uint id) => new groupPartnerType
		{
			partner_id = id
		}).ToArray();
		return groupType;
	}

	private static RadioMode? GetRadioMode(ulong? wakeUpMode)
	{
		RadioMode? result = null;
		if (wakeUpMode.HasValue)
		{
			ulong valueOrDefault = wakeUpMode.GetValueOrDefault();
			if (wakeUpMode.HasValue)
			{
				switch (valueOrDefault)
				{
				case 0uL:
				case 1uL:
					switch (valueOrDefault)
					{
					case 0uL:
						result = RadioMode.WakeOnRadio;
						break;
					case 1uL:
						result = RadioMode.WakeOnEvent;
						break;
					}
					break;
				}
			}
		}
		return result;
	}

	private static string GetStringInfo(InfoFieldType fieldType, infoType[] infoCollection)
	{
		return infoCollection.SingleOrDefault((infoType i) => i.type_id == (uint)fieldType)?.@string;
	}

	private static ulong? GetUlongInfo(InfoFieldType fieldType, infoType[] infoCollection)
	{
		return infoCollection.SingleOrDefault((infoType i) => i.type_id == (uint)fieldType)?.number;
	}

	private static byte[] GetByteArrayInfo(InfoFieldType fieldType, infoType[] infoCollection)
	{
		return infoCollection.SingleOrDefault((infoType i) => i.type_id == (uint)fieldType)?.hex;
	}

	private static Partner ToPartner(partnerType partner, int gatewayId)
	{
		uint? subdeviceId = null;
		Partner partner2 = new Partner();
		partner2.Id = partner.partner_id;
		partner2.Identifier = new DeviceIdentifier(new IPAddress(GetByteArrayInfo(InfoFieldType.IPV6Address, partner.info)), subdeviceId, gatewayId);
		partner2.WakeupMode = GetRadioMode(GetUlongInfo(InfoFieldType.RadioMode, partner.info));
		partner2.WakeupChannel = (uint?)GetUlongInfo(InfoFieldType.WakeupChannel, partner.info);
		partner2.WakeupInterval = (uint?)GetUlongInfo(InfoFieldType.WakeupInterval, partner.info);
		partner2.WakeupOffset = (uint?)GetUlongInfo(InfoFieldType.WakeupOffset, partner.info);
		return partner2;
	}

	private static Group ToGroup(groupType group)
	{
		Group obj = new Group();
		obj.Id = group.partner_id;
		obj.PartnerIds = group.partner.Select((groupPartnerType p) => p.partner_id).ToArray();
		return obj;
	}
}
