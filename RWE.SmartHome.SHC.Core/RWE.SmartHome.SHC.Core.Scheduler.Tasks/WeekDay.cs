using System;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

[Flags]
public enum WeekDay : byte
{
	Monday = 1,
	Tuesday = 2,
	Wednesday = 4,
	Thursday = 8,
	Friday = 0x10,
	Saturday = 0x20,
	Sunday = 0x40
}
