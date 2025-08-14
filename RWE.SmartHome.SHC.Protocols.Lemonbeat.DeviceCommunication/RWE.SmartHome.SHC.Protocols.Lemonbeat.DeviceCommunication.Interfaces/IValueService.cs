using System;
using System.Collections.Generic;
using System.ComponentModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IValueService
{
	event EventHandler<ValueReportReceivedArgs> ValueReportReceived;

	event EventHandler<RequestStatusCompletedEventArgs> RequestStatusCompleted;

	event EventHandler<AsyncCompletedEventArgs> SetValueCompleted;

	event EventHandler<AsyncCompletedEventArgs> ReportValueCompleted;

	ValueCollection RequestStatus(DeviceIdentifier identifier);

	ValueCollection RequestStatus(DeviceIdentifier identifier, uint index);

	void RequestStatusAsync(DeviceIdentifier identifier, object userState);

	void RequestStatusAsync(DeviceIdentifier identifier, uint index, object userState);

	void RequestStatusAsync(DeviceIdentifier identifier, uint[] indexes, object userState);

	void SetValueAsync(DeviceIdentifier identifier, uint valueId, string value, Transport transport, object userState);

	void SetValueAsync(DeviceIdentifier identifier, uint valueId, double value, Transport transport, object userState);

	void SetValueAsync(DeviceIdentifier identifier, uint valueId, byte[] value, Transport transport, object userState);

	void SetValueAsync(DeviceIdentifier identifier, IEnumerable<CoreNumberValue> numberValues, IEnumerable<CoreStringValue> stringValues, IEnumerable<CoreHexBinaryValue> hexBinaryValues, Transport transport, object userState);

	void ReportValueAsync(DeviceIdentifier identifier, uint valueId, double state, Transport transport, object userState);
}
