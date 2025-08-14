using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.SHC.RuleEngine.Triggers;

public class TriggerData
{
	public TriggerDataType TriggerDataType { get; private set; }

	public LinkBinding TriggerEntity { get; private set; }

	public List<Property> NewStateProperties { get; private set; }

	public List<Property> OldStateProperties { get; private set; }

	public TriggerData(TriggerDataType dataType, LinkBinding triggerEntity, LogicalDeviceState oldState, LogicalDeviceState newState)
		: this(dataType, triggerEntity, oldState?.GetProperties(), newState?.GetProperties())
	{
	}

	public TriggerData(TriggerDataType dataType, LinkBinding triggerEntity, List<Property> oldState, List<Property> newState)
	{
		TriggerEntity = triggerEntity;
		TriggerDataType = dataType;
		if (oldState == null)
		{
			if (newState != null)
			{
				NewStateProperties = newState.Where((Property p) => p.GetValueAsComparable() != null).ToList();
			}
		}
		else
		{
			NewStateProperties = newState ?? oldState;
			OldStateProperties = oldState;
		}
	}
}
