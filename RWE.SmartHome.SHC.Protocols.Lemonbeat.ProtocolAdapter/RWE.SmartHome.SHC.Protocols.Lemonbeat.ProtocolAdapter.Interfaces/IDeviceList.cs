using System;
using System.Collections.Generic;
using System.Net;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

public interface IDeviceList
{
	DeviceInformation this[Guid deviceId] { get; set; }

	DeviceInformation this[DeviceIdentifier identifier] { get; }

	DeviceInformation this[SGTIN96 sgtin] { get; }

	int Count { get; }

	event EventHandler<LemonbeatDeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	event EventHandler<DeviceConfiguredEventArgs> DeviceConfiguredStateChanged;

	event EventHandler<DeviceReachabilityChangedEventArgs> DeviceReachabilityChanged;

	event EventHandler<DeviceUpdateStateChangedEventArgs> DeviceUpdateStateChanged;

	List<DeviceInformation> SyncWhere(Func<DeviceInformation, bool> predicate);

	List<TResult> SyncSelect<TResult>(Func<DeviceInformation, TResult> selector);

	IEnumerable<DeviceInformation> GetDevicesByIPAddress(IPAddress address);

	void AddDevice(DeviceInformation deviceInformation);

	bool Contains(Guid deviceId);

	bool Contains(SGTIN96 sgtin);

	bool Contains(IPAddress address);

	void Remove(Guid deviceId);

	void InvokeDeviceInclusionStateChanged(object sender, LemonbeatDeviceInclusionStateChangedEventArgs e);

	void InvokeDeviceConfiguredStateChanged(object sender, DeviceConfiguredEventArgs e);
}
