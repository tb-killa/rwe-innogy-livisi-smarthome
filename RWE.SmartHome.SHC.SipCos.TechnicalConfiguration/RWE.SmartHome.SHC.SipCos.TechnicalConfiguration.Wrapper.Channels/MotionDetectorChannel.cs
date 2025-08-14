using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class MotionDetectorChannel : SensorChannel<BaseLink>
{
	public int NumberOfMovements { get; set; }

	public int FilterInterval { get; set; }

	public int NumberOfBrightnessMeasurements { get; set; }

	public bool CaptureWhileIntervalToNextEvent { get; set; }

	public int IntervalToNextEvent { get; set; }

	public MotionDetectorChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount, needsLinkUpdatePendingFlag: true)
	{
		NumberOfMovements = 0;
		FilterInterval = 1;
		NumberOfBrightnessMeasurements = 0;
		CaptureWhileIntervalToNextEvent = false;
		IntervalToNextEvent = 3;
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		CheckTimeParameter(0, 15, FilterInterval, "FilterInterval");
		CheckTimeParameter(0, 7, IntervalToNextEvent, "IntervalToNextEvent");
		ConfigurationLink configurationLink = new ConfigurationLink();
		ParameterList parameterList = configurationLink[1];
		byte value = (byte)((NumberOfMovements << 4) + FilterInterval);
		byte b = (byte)((NumberOfBrightnessMeasurements << 4) + IntervalToNextEvent);
		if (CaptureWhileIntervalToNextEvent)
		{
			b += 8;
		}
		parameterList[1] = value;
		parameterList[2] = b;
		return configurationLink;
	}

	private static void CheckTimeParameter(int minimum, int maximum, int toCheck, string name)
	{
		if (toCheck < minimum || toCheck > maximum)
		{
			throw ConversionHelpers.CreateParameterException(ValidationErrorCode.TimeSpan, name, toCheck);
		}
	}
}
