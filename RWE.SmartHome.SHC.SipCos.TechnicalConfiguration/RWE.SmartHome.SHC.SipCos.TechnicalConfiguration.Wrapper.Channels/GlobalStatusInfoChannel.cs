using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class GlobalStatusInfoChannel : BaseChannel<BaseLink>
{
	public byte[] StatusInfoDestinationAddress { get; set; }

	public byte DutyLimit { get; set; }

	public GlobalStatusInfoChannel(byte channelIndex)
		: base(channelIndex, (byte)0)
	{
		StatusInfoDestinationAddress = new byte[3] { 224, 0, 3 };
		DutyLimit = 90;
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		if (StatusInfoDestinationAddress.Length != 3)
		{
			throw ConversionHelpers.CreateParameterException(ValidationErrorCode.Unknown, "StatusInfoDestinationAddress", StatusInfoDestinationAddress);
		}
		ConfigurationLink configurationLink = new ConfigurationLink();
		ParameterList parameterList = configurationLink[0];
		parameterList[4] = StatusInfoDestinationAddress[0];
		parameterList[5] = StatusInfoDestinationAddress[1];
		parameterList[6] = StatusInfoDestinationAddress[2];
		parameterList[8] = DutyLimit;
		return configurationLink;
	}
}
