using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class RuleExecutionFailedEventArgs
{
	public Guid RuleId { get; set; }
}
