using System;

namespace SmartHome.SHC.API.Protocols.wMBus;

public class DateTimeValue : VariableDataEntry
{
	public DateTime DateTime { get; set; }

	public DateTimeValue()
		: base(ValueType.DateTime)
	{
	}

	public DateTimeValue(DateTime dateTime)
		: base(ValueType.DateTime)
	{
		DateTime = dateTime;
	}
}
