using System;

namespace RWE.SmartHome.SHC.CommonFunctionality;

[Flags]
public enum PendingUpdateTasksStatus
{
	NotNecessary = 0,
	DatabaseUpdateNecessary = 1,
	RestoreSequenceCounterNecessary = 2
}
