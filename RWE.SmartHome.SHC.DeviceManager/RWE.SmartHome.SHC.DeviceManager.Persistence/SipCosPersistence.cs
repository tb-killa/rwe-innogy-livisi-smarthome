using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;

namespace RWE.SmartHome.SHC.DeviceManager.Persistence;

public class SipCosPersistence : ISipCosPersistence
{
	private const string deviceListId = "deviceListId";

	private const string networkParameterId = "networkParameters";

	private const string bidCosMappingsId = "bidCosMappings";

	private readonly ProtocolIdentifier protocolId = ProtocolIdentifier.Cosip;

	private readonly IProtocolSpecificDataPersistence protocolSpecificDataPersistence;

	private readonly SortedList<string, XmlSerializer> serializers = new SortedList<string, XmlSerializer>();

	private string data;

	public SipCosPersistence(IProtocolSpecificDataPersistence protocolSpecificDataPersistence)
	{
		this.protocolSpecificDataPersistence = protocolSpecificDataPersistence;
		serializers.Add(typeof(DeviceInformationEntity).ToString(), new XmlSerializer(typeof(DeviceInformationEntity)));
		serializers.Add(typeof(SIPCosNetworkParameter).ToString(), new XmlSerializer(typeof(SIPCosNetworkParameter)));
	}

	public void SaveInTransaction(IDeviceInformation deviceInformation, bool suppressEvent)
	{
		string text = Serialize<DeviceInformationEntity>(DeviceInformationConverter.ConvertToDeviceInformationEntity(deviceInformation));
		protocolSpecificDataPersistence.SaveInTransaction(protocolId, "deviceListId", deviceInformation.DeviceId.ToString(), text, suppressEvent);
	}

	public void Save(IDeviceInformation deviceInformation, bool suppressEvent)
	{
		string text = Serialize<DeviceInformationEntity>(DeviceInformationConverter.ConvertToDeviceInformationEntity(deviceInformation));
		protocolSpecificDataPersistence.Save(protocolId, "deviceListId", deviceInformation.DeviceId.ToString(), text, suppressEvent);
	}

	public IEnumerable<IDeviceInformation> LoadAll()
	{
		List<IDeviceInformation> list = new List<IDeviceInformation>();
		List<ProtocolSpecificDataEntity> list2 = protocolSpecificDataPersistence.LoadAll(protocolId, "deviceListId");
		foreach (ProtocolSpecificDataEntity item2 in list2)
		{
			string text = item2.Data;
			if (!string.IsNullOrEmpty(text))
			{
				DeviceInformationEntity entity = (DeviceInformationEntity)serializers[typeof(DeviceInformationEntity).ToString()].Deserialize(new StringReader(text));
				DeviceInformation item = DeviceInformationConverter.ConvertToDeviceInformation(entity);
				list.Add(item);
			}
		}
		return list;
	}

	public IEnumerable<DeviceInformationEntity> LoadAllEntities()
	{
		List<DeviceInformationEntity> list = new List<DeviceInformationEntity>();
		foreach (IDeviceInformation item in LoadAll())
		{
			list.Add(DeviceInformationConverter.ConvertToDeviceInformationEntity(item));
		}
		return list;
	}

	public SIPCosNetworkParameter LoadSIPCosNetworkParameter()
	{
		SIPCosNetworkParameter result = null;
		string text = protocolSpecificDataPersistence.Load(protocolId, "networkParameters", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			result = Deserialize<SIPCosNetworkParameter>(text);
		}
		return result;
	}

	public void SaveSIPCosNetworkParameterInTransaction(SIPCosNetworkParameter sipCosNetworkParameter, bool suppressEvent)
	{
		string text = Serialize<SIPCosNetworkParameter>(sipCosNetworkParameter);
		protocolSpecificDataPersistence.SaveInTransaction(protocolId, "networkParameters", string.Empty, text, suppressEvent);
	}

	public string LoadBidCosMappings()
	{
		return protocolSpecificDataPersistence.Load(protocolId, "bidCosMappings", string.Empty);
	}

	public void SaveBidCosMappingsInTransaction(string bidCosMappings)
	{
		protocolSpecificDataPersistence.Save(protocolId, "bidCosMappings", string.Empty, bidCosMappings, suppressEvent: false);
	}

	public void DeleteInTransaction(Guid deviceId, bool suppressEvent)
	{
		protocolSpecificDataPersistence.DeleteInTransaction(protocolId, "deviceListId", deviceId.ToString(), suppressEvent);
	}

	public void SaveSIPCosNetworkParameter(SIPCosNetworkParameter sipCosNetworkParameter, bool suppressEvent)
	{
		string text = Serialize<SIPCosNetworkParameter>(sipCosNetworkParameter);
		protocolSpecificDataPersistence.Save(protocolId, "networkParameters", string.Empty, text, suppressEvent);
	}

	public void SaveAll(List<DeviceInformationEntity> entities, bool suppressEvent)
	{
		List<ProtocolSpecificDataEntity> list = new List<ProtocolSpecificDataEntity>();
		foreach (DeviceInformationEntity entity in entities)
		{
			data = Serialize<DeviceInformationEntity>(entity);
			ProtocolSpecificDataEntity item = new ProtocolSpecificDataEntity(protocolId, "deviceListId", entity.DeviceId.ToString(), data);
			list.Add(item);
		}
		protocolSpecificDataPersistence.SaveAll(list, suppressEvent);
	}

	private T Deserialize<T>(string serializedObject)
	{
		using StringReader textReader = new StringReader(serializedObject);
		return (T)serializers[typeof(T).ToString()].Deserialize(textReader);
	}

	private string Serialize<T>(object o)
	{
		using MemoryStream memoryStream = new MemoryStream();
		serializers[typeof(T).ToString()].Serialize(memoryStream, o);
		memoryStream.Position = 0L;
		using StreamReader streamReader = new StreamReader(memoryStream);
		return streamReader.ReadToEnd();
	}
}
