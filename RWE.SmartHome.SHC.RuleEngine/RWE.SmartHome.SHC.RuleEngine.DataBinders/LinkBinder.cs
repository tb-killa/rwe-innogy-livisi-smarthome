using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

internal class LinkBinder : BaseDataBinder<LinkBinding>
{
	protected override IComparable GetValue(LinkBinding binding, EventContext context)
	{
		EntityIdentificationData entityIdentificationData = new EntityIdentificationData();
		entityIdentificationData.EntityId = binding.EntityId;
		entityIdentificationData.EntityType = binding.LinkType;
		return entityIdentificationData;
	}

	public override DataBinderType GetBinderType(DataBinding binding)
	{
		return DataBinderType.Link;
	}
}
