using System;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class SchedulerSetpoint : IComparable<SchedulerSetpoint>
{
	public TimeSpan Time { get; set; }

	public decimal TemperatureInCentigrades { get; set; }

	public void SaveToLink(ParameterList link, byte offset)
	{
		if (offset == byte.MaxValue)
		{
			throw new ArgumentOutOfRangeException("offset", "Invalid offset for parameter in SchedulerSetpoint");
		}
		ConversionHelpers.CheckTemperatureRange(TemperatureInCentigrades, "PointTemperature");
		ConversionHelpers.CheckTimeOfDayRange(Time, "Time");
		short num = ConversionHelpers.ConvertTimeOfDay(Time);
		byte b = ConversionHelpers.ConvertCentigrade(TemperatureInCentigrades);
		link[offset] = (byte)(num >> 1);
		link[(byte)(offset + 1)] = (byte)(b + ((num & 1) << 7));
	}

	public static void SaveDefaultToLink(ParameterList link, byte offset)
	{
		if (offset == byte.MaxValue)
		{
			throw new ArgumentOutOfRangeException("offset", "Invalid offset for parameter in SchedulerSetpoint");
		}
		link[offset] = byte.MaxValue;
		link[(byte)(offset + 1)] = 128;
	}

	public int CompareTo(SchedulerSetpoint other)
	{
		return Time.CompareTo(other.Time);
	}
}
