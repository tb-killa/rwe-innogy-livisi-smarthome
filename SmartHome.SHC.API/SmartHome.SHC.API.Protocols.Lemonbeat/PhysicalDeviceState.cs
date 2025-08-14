using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class PhysicalDeviceState
{
	public IEnumerable<NumberValue> NumberValues { get; set; }

	public IEnumerable<StringValue> StringValues { get; set; }

	public IEnumerable<HexBinaryValue> HexBinaryValues { get; set; }
}
