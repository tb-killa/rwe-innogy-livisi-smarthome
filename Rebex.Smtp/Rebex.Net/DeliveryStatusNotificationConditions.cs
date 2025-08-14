using System;

namespace Rebex.Net;

[Flags]
public enum DeliveryStatusNotificationConditions
{
	None = 0,
	Success = 1,
	Failure = 2,
	Delay = 4,
	All = 7
}
