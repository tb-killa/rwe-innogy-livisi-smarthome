using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class SetValuesMessage : ControlMessage
{
	public IEnumerable<NumberValue> NumberValues { get; private set; }

	public IEnumerable<StringValue> StringValues { get; private set; }

	public IEnumerable<HexBinaryValue> HexBinaryValues { get; private set; }

	public SetValuesMessage(IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues)
	{
		if (numberValues != null)
		{
			NumberValues = new ReadOnlyCollection<NumberValue>(numberValues.ToList());
		}
		if (stringValues != null)
		{
			StringValues = new ReadOnlyCollection<StringValue>(stringValues.ToList());
		}
	}

	public SetValuesMessage(IEnumerable<NumberValue> numberValues, IEnumerable<StringValue> stringValues, IEnumerable<HexBinaryValue> hexBinaryValues)
		: this(numberValues, stringValues)
	{
		if (hexBinaryValues != null)
		{
			HexBinaryValues = new ReadOnlyCollection<HexBinaryValue>(hexBinaryValues.ToList());
		}
	}
}
