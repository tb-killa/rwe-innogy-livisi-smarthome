using System;

namespace Rebex.Security.Certificates;

[Flags]
public enum KeySetOptions
{
	Exportable = 1,
	UserProtected = 2,
	MachineKeySet = 0x20,
	UserKeySet = 0x1000,
	PersistKeySet = 0x8000,
	AlwaysCng = 0x200,
	PreferCng = 0x100
}
