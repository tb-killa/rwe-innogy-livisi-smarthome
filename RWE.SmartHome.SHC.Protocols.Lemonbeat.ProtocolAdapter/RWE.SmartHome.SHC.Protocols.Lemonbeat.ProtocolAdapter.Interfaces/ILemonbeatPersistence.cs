using System;
using System.Collections.Generic;
using System.Net;
using RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Persistence;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

public interface ILemonbeatPersistence
{
	IEnumerable<DeviceInformation> LoadAllDevices();

	List<DeviceInformationEntity> GetAllDevicesInformation();

	DeviceInformation LoadDongle();

	DeviceInformationEntity GetDongleInformation();

	void SaveDongle(DeviceInformation dongle, bool suppressEvent);

	void SaveAllDevices(IEnumerable<DeviceInformation> deviceInformations, bool suppressEvent);

	void SaveInTransaction(DeviceInformation deviceInformation, bool suppressEvent);

	void DeleteInTransaction(Guid deviceId, bool suppressEvent);

	void SaveNetworkKey(string networkKey, bool suppressEvent);

	string LoadNetworkKey();

	uint LoadSequenceCounter();

	void SaveSequenceCounter(uint sequence);

	Dictionary<Guid, IPAddress> LoadProfileMulticastAddresses();

	void SaveProfileAddress(Guid profileId, IPAddress address);

	void DeleteProfileAddress(Guid profileId);

	IEnumerable<KeyValuePair<Guid, PhysicalConfiguration>> LoadAllConfigurations();

	void SaveAllConfigurations(IEnumerable<KeyValuePair<Guid, PhysicalConfiguration>> configurations, bool suppressEvent);

	void SaveConfiguration(Guid deviceId, PhysicalConfiguration configuration, bool suppressEvent);

	void DeleteConfiguration(Guid deviceId, bool suppressEvent);

	void SaveAllShcValues(Dictionary<string, Dictionary<string, uint>> shcValues);

	Dictionary<string, Dictionary<string, uint>> LoadAllShcValues();
}
