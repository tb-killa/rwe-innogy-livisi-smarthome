using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class ValueDescriptionService : SenderService<network>, IValueDescriptionService
{
	private const string DefaultNamespace = "urn:value_descriptionxsd";

	private const uint ModeReadOnly = 1u;

	private const uint ModeReadWrite = 2u;

	private const uint ModeWriteOnly = 3u;

	public ValueDescriptionService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.ValueDescription, "urn:value_descriptionxsd")
	{
	}

	public List<ValueDescription> GetValueDescriptions(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.value_description_get };
		network.device[0].Items = new object[1]
		{
			new valueDescriptionGetType()
		};
		network network2 = SendRequest(identifier, network);
		List<ValueDescription> list = new List<ValueDescription>();
		if (network2 != null && network2.device != null)
		{
			uint subDeviceId = identifier.SubDeviceId ?? 0;
			networkDevice networkDevice = network2.device.Where((networkDevice d) => d.device_id == subDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Items != null)
			{
				IEnumerable<valueDescriptionReportType> enumerable = networkDevice.Items.OfType<valueDescriptionReportType>();
				foreach (valueDescriptionReportType item in enumerable)
				{
					list.AddRange(item.value_description.Select((valueDescriptionType valueDescription) => Convert(valueDescription)));
				}
			}
		}
		return list;
	}

	public void AddAndDeleteValueDescriptions(DeviceIdentifier identifier, IEnumerable<ValueDescription> valueDescriptionsToSet, IEnumerable<uint> valueDescriptionsToDelete)
	{
		valueDescriptionType[] array = valueDescriptionsToSet.Select((ValueDescription vd) => Convert(vd)).ToArray();
		if (array.Length > 0)
		{
			network message = CreateSetRequest(identifier, new valueDescriptionAddType
			{
				value_description = array
			});
			SendMessage(identifier, message, TransportType.Connection);
		}
		foreach (uint item in valueDescriptionsToDelete)
		{
			network message2 = CreateDeleteRequest(identifier, new valueDescriptionDeleteType
			{
				value_description_id = (byte)item,
				value_description_idSpecified = true
			});
			SendMessage(identifier, message2, TransportType.Connection);
		}
	}

	public void AddValueDescription(DeviceIdentifier identifier, ValueDescription valueDescription)
	{
		network message = CreateSetRequest(identifier, new valueDescriptionAddType
		{
			value_description = new valueDescriptionType[1] { Convert(valueDescription) }
		});
		SendMessage(identifier, message, TransportType.Connection);
	}

	public void AddValueDescriptions(DeviceIdentifier identifier, IEnumerable<ValueDescription> valueDescriptions)
	{
		network message = CreateSetRequest(identifier, new valueDescriptionAddType
		{
			value_description = valueDescriptions.Select((ValueDescription vd) => Convert(vd)).ToArray()
		});
		SendMessage(identifier, message, TransportType.Connection);
	}

	public void DeleteValueDescription(DeviceIdentifier identifier, uint index)
	{
		network message = CreateDeleteRequest(identifier, new valueDescriptionDeleteType
		{
			value_description_id = (byte)index,
			value_description_idSpecified = true
		});
		SendMessage(identifier, message, TransportType.Connection);
	}

	private network CreateNetworkMessage(DeviceIdentifier identifier)
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
		network network2 = network;
		network2.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network2.device[0].device_id = identifier.SubDeviceId ?? 0;
		return network2;
	}

	private network CreateSetRequest(DeviceIdentifier identifier, valueDescriptionAddType set)
	{
		network network = CreateNetworkMessage(identifier);
		networkDevice obj = network.device[0];
		ItemsChoiceType[] itemsElementName = new ItemsChoiceType[1];
		obj.ItemsElementName = itemsElementName;
		network.device[0].Items = new object[1] { set };
		return network;
	}

	private network CreateDeleteRequest(DeviceIdentifier identifier, valueDescriptionDeleteType delete)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.value_description_delete };
		network.device[0].Items = new object[1] { delete };
		return network;
	}

	private ValueDescription Convert(valueDescriptionType valueDescriptionType)
	{
		ValueDescription valueDescription = new ValueDescription();
		valueDescription.Id = valueDescriptionType.value_id;
		valueDescription.Type = valueDescriptionType.type_id;
		valueDescription.Name = valueDescriptionType.name;
		valueDescription.Readable = valueDescriptionType.mode == 1 || valueDescriptionType.mode == 2;
		valueDescription.Writeable = valueDescriptionType.mode == 3 || valueDescriptionType.mode == 2;
		valueDescription.Persistent = valueDescriptionType.persistent == 1;
		valueDescription.MinLogInterval = (valueDescriptionType.min_log_intervalSpecified ? new uint?(valueDescriptionType.min_log_interval) : ((uint?)null));
		valueDescription.MaxLogValues = (valueDescriptionType.max_log_valuesSpecified ? new uint?(valueDescriptionType.max_log_values) : ((uint?)null));
		valueDescription.IsVirtual = valueDescriptionType.virtualSpecified && valueDescriptionType.@virtual == 1;
		ValueDescription valueDescription2 = valueDescription;
		if (valueDescriptionType.Item is numberFormatType numberFormatType)
		{
			valueDescription2.NumberFormat = new NumberFormat(numberFormatType.unit, numberFormatType.min, numberFormatType.max, numberFormatType.step);
		}
		else if (valueDescriptionType.Item is stringFormatType stringFormatType)
		{
			valueDescription2.StringFormat = new StringFormat(stringFormatType.max_length, stringFormatType.valid_value);
		}
		else if (valueDescriptionType.Item is hexBinaryFormatType hexBinaryFormatType)
		{
			valueDescription2.HexBinaryFormat = new HexBinaryFormat(hexBinaryFormatType.max_length);
		}
		return valueDescription2;
	}

	private valueDescriptionType Convert(ValueDescription valueDescription)
	{
		valueDescriptionType valueDescriptionType = new valueDescriptionType();
		valueDescriptionType.value_id = valueDescription.Id;
		valueDescriptionType.type_id = valueDescription.Type;
		valueDescriptionType.name = valueDescription.Name;
		valueDescriptionType.mode = ((valueDescription.Readable && !valueDescription.Writeable) ? 1u : ((valueDescription.Readable && valueDescription.Writeable) ? 2u : 3u));
		valueDescriptionType.persistent = (valueDescription.Persistent ? 1u : 0u);
		valueDescriptionType.min_log_interval = (valueDescription.MinLogInterval.HasValue ? valueDescription.MinLogInterval.Value : 0u);
		valueDescriptionType.min_log_intervalSpecified = valueDescription.MinLogInterval.HasValue;
		valueDescriptionType.max_log_values = (valueDescription.MaxLogValues.HasValue ? valueDescription.MaxLogValues.Value : 0u);
		valueDescriptionType.max_log_valuesSpecified = valueDescription.MaxLogValues.HasValue;
		valueDescriptionType.@virtual = (valueDescription.IsVirtual ? 1u : 0u);
		valueDescriptionType.virtualSpecified = valueDescription.IsVirtual;
		valueDescriptionType valueDescriptionType2 = valueDescriptionType;
		if (valueDescription.NumberFormat != null)
		{
			valueDescriptionType2.Item = new numberFormatType
			{
				unit = valueDescription.NumberFormat.Unit,
				min = valueDescription.NumberFormat.Min,
				max = valueDescription.NumberFormat.Max,
				step = valueDescription.NumberFormat.Step
			};
		}
		else if (valueDescription.StringFormat != null)
		{
			valueDescriptionType2.Item = new stringFormatType
			{
				max_length = valueDescription.StringFormat.MaximumLength,
				valid_value = valueDescription.StringFormat.ValidValues
			};
		}
		else if (valueDescription.HexBinaryFormat != null)
		{
			valueDescriptionType2.Item = new hexBinaryFormatType
			{
				max_length = valueDescription.HexBinaryFormat.MaximumLength
			};
		}
		return valueDescriptionType2;
	}
}
