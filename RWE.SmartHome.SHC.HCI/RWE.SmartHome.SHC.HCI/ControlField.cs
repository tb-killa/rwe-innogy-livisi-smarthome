using System;

namespace RWE.SmartHome.SHC.HCI;

[Flags]
public enum ControlField
{
	Reserved = 0,
	TimeStampFieldAttached = 2,
	RSSIFieldAttached = 4,
	CRC16FieldAttached = 8
}
