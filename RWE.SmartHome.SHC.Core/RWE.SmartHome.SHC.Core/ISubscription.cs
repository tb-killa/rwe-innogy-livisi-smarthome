using System;

namespace RWE.SmartHome.SHC.Core;

public interface ISubscription
{
	SubscriptionToken SubscriptionToken { get; set; }

	Action<object[]> GetExecutionStrategy();
}
