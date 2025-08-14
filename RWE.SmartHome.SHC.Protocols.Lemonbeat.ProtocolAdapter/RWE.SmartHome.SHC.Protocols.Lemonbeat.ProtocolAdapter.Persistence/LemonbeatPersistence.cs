using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Persistence;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Persistence;

public class LemonbeatPersistence : ILemonbeatPersistence
{
	private const string DeviceDataId = "DeviceList";

	private const string DongleDataId = "Dongle";

	private const string NetworkKeyDataId = "NetworkKey";

	private const string LemonbeatSequenceCounterDataId = "SequenceCounter";

	private const string ProfileMulticastAddressId = "ProfileAddress";

	private const string ShcValuesId = "ShcValues";

	private const string PhysicalConfigId = "Config";

	private const ProtocolIdentifier ProtocolId = ProtocolIdentifier.Lemonbeat;

	private readonly IProtocolSpecificDataPersistence protocolSpecificDataPersistence;

	private readonly XmlSerializer deviceInfoSerializer = new XmlSerializer(typeof(DeviceInformationEntity));

	private readonly XmlSerializer deviceConfigSerializer = new XmlSerializer(typeof(PhysicalConfigurationEntity));

	private readonly XmlSerializer shcValueSerializer = new XmlSerializer(typeof(AddInSpecificShcValues[]));

	private readonly Func<Guid, bool> DeviceKnown;

	public LemonbeatPersistence(IProtocolSpecificDataPersistence protocolSpecificDataPersistence, Func<Guid, bool> isKnownDevice)
	{
		this.protocolSpecificDataPersistence = protocolSpecificDataPersistence;
		DeviceKnown = isKnownDevice;
	}

	public IEnumerable<DeviceInformation> LoadAllDevices()
	{
		List<ProtocolSpecificDataEntity> source = protocolSpecificDataPersistence.LoadAll(ProtocolIdentifier.Lemonbeat, "DeviceList");
		return (from protocolSpecificDataEntity in source
			select protocolSpecificDataEntity.Data into value
			where !string.IsNullOrEmpty(value)
			select deviceInfoSerializer.Deserialize<DeviceInformationEntity>(value).Convert()).ToList();
	}

	public List<DeviceInformationEntity> GetAllDevicesInformation()
	{
		List<ProtocolSpecificDataEntity> source = protocolSpecificDataPersistence.LoadAll(ProtocolIdentifier.Lemonbeat, "DeviceList");
		return (from e in source
			where !string.IsNullOrEmpty(e.Data)
			select deviceInfoSerializer.Deserialize<DeviceInformationEntity>(e.Data)).ToList();
	}

	public DeviceInformation LoadDongle()
	{
		string text = protocolSpecificDataPersistence.Load(ProtocolIdentifier.Lemonbeat, "Dongle", string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return deviceInfoSerializer.Deserialize<DeviceInformationEntity>(text).Convert();
	}

	public DeviceInformationEntity GetDongleInformation()
	{
		string text = protocolSpecificDataPersistence.Load(ProtocolIdentifier.Lemonbeat, "Dongle", string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return deviceInfoSerializer.Deserialize<DeviceInformationEntity>(text);
	}

	public void SaveDongle(DeviceInformation dongle, bool suppressEvent)
	{
		string data = deviceInfoSerializer.Serialize(new DeviceInformationEntity(dongle));
		protocolSpecificDataPersistence.SaveInTransaction(ProtocolIdentifier.Lemonbeat, "Dongle", string.Empty, data, suppressEvent);
	}

	public void SaveAllDevices(IEnumerable<DeviceInformation> deviceInformations, bool suppressEvent)
	{
		List<ProtocolSpecificDataEntity> entities = deviceInformations.Select((DeviceInformation deviceInformation) => new ProtocolSpecificDataEntity(ProtocolIdentifier.Lemonbeat, "DeviceList", deviceInformation.DeviceId.ToString(), deviceInfoSerializer.Serialize(new DeviceInformationEntity(deviceInformation)))).ToList();
		protocolSpecificDataPersistence.SaveAll(entities, suppressEvent);
	}

	public void SaveInTransaction(DeviceInformation deviceInformation, bool suppressEvent)
	{
		if (DeviceKnown(deviceInformation.DeviceId))
		{
			string data = deviceInfoSerializer.Serialize(new DeviceInformationEntity(deviceInformation));
			protocolSpecificDataPersistence.SaveInTransaction(ProtocolIdentifier.Lemonbeat, "DeviceList", deviceInformation.DeviceId.ToString(), data, suppressEvent);
		}
		else
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Record for device with id {deviceInformation.DeviceId} was not persisted.Unknown device Id. Device inclusion state={deviceInformation.DeviceInclusionState}");
		}
	}

	public void DeleteInTransaction(Guid deviceId, bool suppressEvent)
	{
		protocolSpecificDataPersistence.DeleteInTransaction(ProtocolIdentifier.Lemonbeat, "DeviceList", deviceId.ToString(), suppressEvent);
	}

	public void SaveNetworkKey(string networkKey, bool suppressEvent)
	{
		protocolSpecificDataPersistence.SaveInTransaction(ProtocolIdentifier.Lemonbeat, "NetworkKey", string.Empty, networkKey, suppressEvent);
	}

	public string LoadNetworkKey()
	{
		return protocolSpecificDataPersistence.Load(ProtocolIdentifier.Lemonbeat, "NetworkKey", string.Empty);
	}

	public uint LoadSequenceCounter()
	{
		uint result = 0u;
		try
		{
			string text = protocolSpecificDataPersistence.Load(ProtocolIdentifier.Lemonbeat, "SequenceCounter", string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				result = uint.Parse(text);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"Error retrieving Lemonbeat sequence counter. {ex.Message}");
		}
		return result;
	}

	public void SaveSequenceCounter(uint sequence)
	{
		try
		{
			protocolSpecificDataPersistence.SaveInTransaction(ProtocolIdentifier.Lemonbeat, "SequenceCounter", string.Empty, sequence.ToString(), suppressEvent: true);
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"Error storing Lemonbeat sequence counter. {ex.Message}");
		}
	}

	public Dictionary<Guid, IPAddress> LoadProfileMulticastAddresses()
	{
		Dictionary<Guid, IPAddress> dictionary = new Dictionary<Guid, IPAddress>();
		IEnumerable<ProfileAddressEntity> enumerable = from psde in protocolSpecificDataPersistence.LoadAll(ProtocolIdentifier.Lemonbeat, "ProfileAddress")
			select new ProfileAddressEntity
			{
				ProfileId = new Guid(psde.SubId),
				AssignedMulticastAddress = IPAddress.Parse(psde.Data)
			};
		foreach (ProfileAddressEntity item in enumerable)
		{
			dictionary.Add(item.ProfileId, item.AssignedMulticastAddress);
		}
		return dictionary;
	}

	public void SaveProfileAddress(Guid profileId, IPAddress address)
	{
		protocolSpecificDataPersistence.Save(ProtocolIdentifier.Lemonbeat, "ProfileAddress", profileId.ToString(), address.ToString(), suppressEvent: false);
	}

	public void DeleteProfileAddress(Guid profileId)
	{
		protocolSpecificDataPersistence.DeleteInTransaction(ProtocolIdentifier.Lemonbeat, "ProfileAddress", profileId.ToString(), suppressEvent: false);
	}

	public IEnumerable<KeyValuePair<Guid, PhysicalConfiguration>> LoadAllConfigurations()
	{
		List<ProtocolSpecificDataEntity> list = protocolSpecificDataPersistence.LoadAll(ProtocolIdentifier.Lemonbeat, "Config");
		SortedList<Guid, PhysicalConfiguration> sortedList = new SortedList<Guid, PhysicalConfiguration>();
		foreach (ProtocolSpecificDataEntity item in list)
		{
			if (!string.IsNullOrEmpty(item.Data))
			{
				KeyValuePair<Guid, PhysicalConfiguration> keyValuePair = ToKeyValuePair(deviceConfigSerializer.Deserialize<PhysicalConfigurationEntity>(item.Data));
				sortedList.Add(new Guid(item.SubId), keyValuePair.Value);
			}
		}
		return sortedList;
	}

	public void SaveAllConfigurations(IEnumerable<KeyValuePair<Guid, PhysicalConfiguration>> configurations, bool suppressEvent)
	{
		List<ProtocolSpecificDataEntity> entities = configurations.Select((KeyValuePair<Guid, PhysicalConfiguration> keyValuePair) => new ProtocolSpecificDataEntity(ProtocolIdentifier.Lemonbeat, "Config", keyValuePair.Key.ToString(), deviceConfigSerializer.Serialize(ToPhysicalConfigurationEntity(keyValuePair.Key, keyValuePair.Value)))).ToList();
		protocolSpecificDataPersistence.SaveAll(entities, suppressEvent);
	}

	public void SaveConfiguration(Guid deviceId, PhysicalConfiguration configuration, bool suppressEvent)
	{
		string data = deviceConfigSerializer.Serialize(ToPhysicalConfigurationEntity(deviceId, configuration));
		protocolSpecificDataPersistence.SaveInTransaction(ProtocolIdentifier.Lemonbeat, "Config", deviceId.ToString(), data, suppressEvent);
	}

	public void DeleteConfiguration(Guid deviceId, bool suppressEvent)
	{
		protocolSpecificDataPersistence.DeleteInTransaction(ProtocolIdentifier.Lemonbeat, "Config", deviceId.ToString(), suppressEvent);
	}

	private PhysicalConfigurationEntity ToPhysicalConfigurationEntity(Guid deviceId, PhysicalConfiguration configuration)
	{
		PhysicalConfigurationEntity physicalConfigurationEntity = new PhysicalConfigurationEntity();
		physicalConfigurationEntity.DeviceId = deviceId;
		physicalConfigurationEntity.PartnerGroups = configuration.PartnerGroups;
		physicalConfigurationEntity.Partners = ((configuration.Partners == null) ? null : configuration.Partners.Select((Partner p) => new PartnerEntity
		{
			IpAddress = p.Identifier.IPAddress.GetAddressBytes(),
			SubDeviceId = p.Identifier.SubDeviceId,
			WakeupChannel = p.WakeupChannel,
			WakeupInterval = p.WakeupInterval,
			WakeupMode = p.WakeupMode,
			WakeupOffset = p.WakeupOffset,
			Id = p.Id
		}).ToList());
		physicalConfigurationEntity.Links = configuration.Links;
		physicalConfigurationEntity.Timers = configuration.Timers;
		physicalConfigurationEntity.CalendarEntries = configuration.CalendarEntries;
		physicalConfigurationEntity.Actions = configuration.Actions;
		physicalConfigurationEntity.Calculations = configuration.Calculations;
		physicalConfigurationEntity.StateMachines = configuration.StateMachines;
		physicalConfigurationEntity.VirtualValueDescriptions = configuration.VirtualValueDescriptions;
		return physicalConfigurationEntity;
	}

	private KeyValuePair<Guid, PhysicalConfiguration> ToKeyValuePair(PhysicalConfigurationEntity dto)
	{
		PhysicalConfiguration physicalConfiguration = new PhysicalConfiguration();
		physicalConfiguration.PartnerGroups = dto.PartnerGroups;
		physicalConfiguration.Partners = ((dto.Partners == null) ? null : dto.Partners.Select((PartnerEntity p) => new Partner
		{
			Id = p.Id,
			Identifier = new DeviceIdentifier(new IPAddress(p.IpAddress), p.SubDeviceId, LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID),
			WakeupChannel = p.WakeupChannel,
			WakeupInterval = p.WakeupInterval,
			WakeupMode = p.WakeupMode,
			WakeupOffset = p.WakeupOffset
		}).ToList());
		physicalConfiguration.Links = dto.Links;
		physicalConfiguration.Timers = dto.Timers;
		physicalConfiguration.CalendarEntries = dto.CalendarEntries;
		physicalConfiguration.Actions = dto.Actions;
		physicalConfiguration.Calculations = dto.Calculations;
		physicalConfiguration.StateMachines = dto.StateMachines;
		physicalConfiguration.VirtualValueDescriptions = dto.VirtualValueDescriptions;
		return new KeyValuePair<Guid, PhysicalConfiguration>(dto.DeviceId, physicalConfiguration);
	}

	public void SaveAllShcValues(Dictionary<string, Dictionary<string, uint>> shcValues)
	{
		AddInSpecificShcValues[] obj = shcValues.Select((KeyValuePair<string, Dictionary<string, uint>> kv) => new AddInSpecificShcValues
		{
			AppId = kv.Key,
			ShcValues = new List<ShcValue>(kv.Value.Select((KeyValuePair<string, uint> kvp) => new ShcValue
			{
				ValueName = kvp.Key,
				ValueId = kvp.Value
			}))
		}).ToArray();
		string data = shcValueSerializer.Serialize(obj);
		protocolSpecificDataPersistence.SaveInTransaction(ProtocolIdentifier.Lemonbeat, "ShcValues", string.Empty, data, suppressEvent: false);
	}

	public Dictionary<string, Dictionary<string, uint>> LoadAllShcValues()
	{
		string text = protocolSpecificDataPersistence.Load(ProtocolIdentifier.Lemonbeat, "ShcValues", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			try
			{
				return shcValueSerializer.Deserialize<AddInSpecificShcValues[]>(text).ToDictionary((AddInSpecificShcValues asv) => asv.AppId, (AddInSpecificShcValues asv) => asv.ShcValues.ToDictionary((ShcValue shcv) => shcv.ValueName, (ShcValue shcv) => shcv.ValueId));
			}
			catch (ArgumentException)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "Error loading shc values from database because of duplicate app ids or value names");
				throw;
			}
		}
		return new Dictionary<string, Dictionary<string, uint>>();
	}
}
