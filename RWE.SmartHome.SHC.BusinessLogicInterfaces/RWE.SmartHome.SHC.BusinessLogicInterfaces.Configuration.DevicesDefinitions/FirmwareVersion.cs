using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public abstract class FirmwareVersion : IComparable
{
	public abstract int CompareTo(object obj);
}
