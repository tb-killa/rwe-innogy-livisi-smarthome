using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

internal class ConstantStringBinder : BaseDataBinder<ConstantStringBinding>
{
	protected override IComparable GetValue(ConstantStringBinding binding, EventContext context)
	{
		return binding.Value;
	}

	public override DataBinderType GetBinderType(DataBinding binding)
	{
		return DataBinderType.String;
	}
}
