using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface IValueHandler
{
	void SetValues(Guid deviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues);

	void SetValues(Guid deviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues, Transport transport);

	void SetValues(Guid deviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues, IEnumerable<HexBinaryValue> hexBinaryValues);

	void SetValues(Guid deviceId, IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues, IEnumerable<HexBinaryValue> hexBinaryValues, Transport transport);

	void RequestValueAsync(Guid deviceId);

	void RequestValueAsync(Guid deviceId, uint[] valueIds);
}
