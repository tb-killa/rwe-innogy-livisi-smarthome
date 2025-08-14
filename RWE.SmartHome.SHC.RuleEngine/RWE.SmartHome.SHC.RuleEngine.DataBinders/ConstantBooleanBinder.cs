using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

internal class ConstantBooleanBinder : BaseDataBinder<ConstantBooleanBinding>
{
	protected override IComparable GetValue(ConstantBooleanBinding binding, EventContext context)
	{
		return binding.Value;
	}

	public override DataBinderType GetBinderType(DataBinding binding)
	{
		return DataBinderType.Boolean;
	}
}
