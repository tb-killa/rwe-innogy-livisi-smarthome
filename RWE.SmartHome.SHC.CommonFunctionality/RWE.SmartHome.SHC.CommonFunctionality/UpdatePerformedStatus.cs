using System;

namespace RWE.SmartHome.SHC.CommonFunctionality;

[Flags]
public enum UpdatePerformedStatus
{
	Controlled = 1,
	ApplicationOnly = 2,
	Successful = 4
}
