using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

internal class ConstantDateTimeBinder : BaseDataBinder<ConstantDateTimeBinding>
{
	protected override IComparable GetValue(ConstantDateTimeBinding binding, EventContext context)
	{
		return binding.Value;
	}

	public override DataBinderType GetBinderType(DataBinding binding)
	{
		return DataBinderType.DateTime;
	}
}
