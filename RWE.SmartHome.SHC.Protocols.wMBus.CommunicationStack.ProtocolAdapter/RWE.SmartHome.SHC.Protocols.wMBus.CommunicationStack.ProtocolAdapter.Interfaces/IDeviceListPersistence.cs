using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

internal interface IDeviceListPersistence
{
	void SaveInTransaction(IDeviceInformation deviceInformation, bool suppressEvent);

	void DeleteInTransaction(Guid deviceId, bool suppressEvent);

	IEnumerable<IDeviceInformation> LoadAll();
}
