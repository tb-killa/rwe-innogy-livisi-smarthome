using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;

public interface ISipCosPersistence
{
	void SaveInTransaction(IDeviceInformation deviceInformation, bool suppressEvent);

	void Save(IDeviceInformation deviceInformation, bool suppressEvent);

	IEnumerable<IDeviceInformation> LoadAll();

	IEnumerable<DeviceInformationEntity> LoadAllEntities();

	SIPCosNetworkParameter LoadSIPCosNetworkParameter();

	void SaveSIPCosNetworkParameterInTransaction(SIPCosNetworkParameter sipCosNetworkParameter, bool suppressEvent);

	string LoadBidCosMappings();

	void SaveBidCosMappingsInTransaction(string bidCosMappings);

	void DeleteInTransaction(Guid deviceId, bool suppressEvent);

	void SaveSIPCosNetworkParameter(SIPCosNetworkParameter networkParameter, bool suppressEvent);

	void SaveAll(List<DeviceInformationEntity> entities, bool suppressEvent);
}
