using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

public class ValueReportReceivedArgs : EventArgs
{
	public IEnumerable<CoreNumberValue> NumberValues { get; private set; }

	public IEnumerable<CoreStringValue> StringValues { get; private set; }

	public IEnumerable<CoreHexBinaryValue> HexBinaryValues { get; private set; }

	public DeviceIdentifier DeviceIdentifier { get; private set; }

	public ValueReportReceivedArgs(DeviceIdentifier identifier, IEnumerable<CoreNumberValue> numberValues, IEnumerable<CoreStringValue> stringValues, IEnumerable<CoreHexBinaryValue> hexBinaryValues)
	{
		DeviceIdentifier = identifier;
		NumberValues = numberValues;
		StringValues = stringValues;
		HexBinaryValues = hexBinaryValues;
	}
}
