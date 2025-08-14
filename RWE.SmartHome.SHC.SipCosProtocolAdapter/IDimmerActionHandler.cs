using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public interface IDimmerActionHandler
{
	IEnumerable<string> SupportedActions { get; }

	SwitchSettings CreateSwitchSettings(ActionDescription action, decimal? currentDimLevel, int technicalMinValue, int technicalMaxValue);
}
