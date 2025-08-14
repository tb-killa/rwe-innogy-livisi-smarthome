using System;
using System.Collections;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

internal interface IDeviceList : IEnumerable<IDeviceInformation>, IEnumerable
{
	IDeviceInformation this[Guid deviceId] { get; }

	IDeviceInformation this[byte[] identification] { get; }

	object SyncRoot { get; }

	event EventHandler<DeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	bool Contains(byte[] identification);

	void AddDevice(IDeviceInformation deviceInformation);

	string LogInfoByGuid(Guid deviceId);

	void Remove(Guid deviceId);
}
