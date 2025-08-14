using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class ValueCollection
{
	public IList<CoreNumberValue> NumberValues { get; set; }

	public IList<CoreStringValue> StringValues { get; set; }

	public IList<CoreHexBinaryValue> HexBinaryValues { get; set; }

	public ValueCollection()
	{
		NumberValues = new List<CoreNumberValue>();
		StringValues = new List<CoreStringValue>();
		HexBinaryValues = new List<CoreHexBinaryValue>();
	}
}
