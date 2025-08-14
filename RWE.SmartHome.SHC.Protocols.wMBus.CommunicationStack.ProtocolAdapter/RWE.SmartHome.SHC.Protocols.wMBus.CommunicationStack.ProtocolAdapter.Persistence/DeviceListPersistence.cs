using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Persistence;

internal class DeviceListPersistence : IDeviceListPersistence
{
	private const string DataId = "DeviceList";

	private readonly IProtocolSpecificDataPersistence protocolSpecificDataPersistence;

	private readonly XmlSerializer serializer = new XmlSerializer(typeof(DeviceInformationEntity));

	public DeviceListPersistence(IProtocolSpecificDataPersistence protocolSpecificDataPersistence)
	{
		this.protocolSpecificDataPersistence = protocolSpecificDataPersistence;
	}

	public void SaveInTransaction(IDeviceInformation deviceInformation, bool suppressEvent)
	{
		string data = serializer.Serialize(new DeviceInformationEntity(deviceInformation));
		protocolSpecificDataPersistence.SaveInTransaction(ProtocolIdentifier.wMBus, "DeviceList", deviceInformation.DeviceId.ToString(), data, suppressEvent);
	}

	public void DeleteInTransaction(Guid deviceId, bool suppressEvent)
	{
		protocolSpecificDataPersistence.DeleteInTransaction(ProtocolIdentifier.wMBus, "DeviceList", deviceId.ToString(), suppressEvent);
	}

	public IEnumerable<IDeviceInformation> LoadAll()
	{
		List<ProtocolSpecificDataEntity> source = protocolSpecificDataPersistence.LoadAll(ProtocolIdentifier.wMBus, "DeviceList");
		return (from protocolSpecificDataEntity in source
			select protocolSpecificDataEntity.Data into value
			where !string.IsNullOrEmpty(value)
			select serializer.Deserialize<DeviceInformationEntity>(value).Convert()).ToList();
	}

	public void SaveAll(IEnumerable<DeviceInformation> deviceInformations, bool suppressEvent)
	{
		List<ProtocolSpecificDataEntity> entities = deviceInformations.Select((DeviceInformation deviceInformation) => new ProtocolSpecificDataEntity(ProtocolIdentifier.wMBus, "DeviceList", deviceInformation.DeviceId.ToString(), serializer.Serialize(new DeviceInformationEntity(deviceInformation)))).ToList();
		protocolSpecificDataPersistence.SaveAll(entities, suppressEvent);
	}
}
