using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class CcWindowReductionChannel : ActuatorChannel<WindowReductionLink>
{
	public decimal WindowOpenTemperatureCentigrade { get; set; }

	public CcWindowReductionChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount)
	{
	}

	public bool AddLink(LinkPartner sensor, ActionDescription action, SwitchAction switchAction, SwitchAction aboveAction, SwitchAction belowAction, int? comparisonValuePercent, Guid ruleId, Guid physicalDeviceId)
	{
		bool result = false;
		if (action.ActionType == "WindowReduction")
		{
			result = CreateWindowReductionLink(sensor, switchAction, comparisonValuePercent, ruleId, physicalDeviceId);
		}
		return result;
	}

	private bool CreateWindowReductionLink(LinkPartner sensor, SwitchAction switchAction, int? comparisonValuePercent, Guid ruleId, Guid physicalDeviceId)
	{
		bool result = false;
		try
		{
			WindowReductionLink windowReductionLink = new WindowReductionLink();
			windowReductionLink.OpenTemperatureCentigrade = WindowOpenTemperatureCentigrade;
			windowReductionLink.ComparisonValuePercent = comparisonValuePercent;
			windowReductionLink.MaxOpenTimeSeconds = 0;
			WindowReductionLink config = windowReductionLink;
			AddLink(sensor, config);
			result = true;
		}
		catch (Exception ex)
		{
			Log.Exception(Module.TechnicalConfiguration, ex, "Error occurred while creating WR link.");
		}
		return result;
	}
}
