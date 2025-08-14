using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class RollerShutterActuatorChannel : ActuatorChannel<RollerShutterLink>
{
	public int RunningTimeTopBottom { get; set; }

	public int RunningTimeBottomTop { get; set; }

	public RollerShutterActuatorChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount)
	{
		RunningTimeBottomTop = 500;
		RunningTimeTopBottom = 500;
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		int runningTimeBottomTop = RunningTimeBottomTop;
		int runningTimeTopBottom = RunningTimeTopBottom;
		ConversionHelpers.CheckTimeParameter(1, 60000, runningTimeBottomTop, "RunningTimeBottomTop");
		ConversionHelpers.CheckTimeParameter(1, 60000, runningTimeTopBottom, "RunningTimeTopBottom");
		ConfigurationLink configurationLink = new ConfigurationLink();
		ParameterList parameterList = configurationLink[1];
		parameterList[6] = (byte)((runningTimeBottomTop >> 8) & 0xFF);
		parameterList[7] = (byte)(runningTimeBottomTop & 0xFF);
		parameterList[4] = (byte)((runningTimeTopBottom >> 8) & 0xFF);
		parameterList[5] = (byte)(runningTimeTopBottom & 0xFF);
		return configurationLink;
	}
}
