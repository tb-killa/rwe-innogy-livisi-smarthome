using System;

namespace RWE.SmartHome.SHC.DomainModel.Types;

public static class SIRChannelMapper
{
	private const string None = "None";

	private const string Alarm = "Alarm";

	private const string Notification = "Notification";

	private const string Feedback = "Feedback";

	private const string Sabotage = "Sabotage";

	public static string GetStringValue(SIRChannel channel)
	{
		return channel switch
		{
			SIRChannel.None => "None", 
			SIRChannel.Alarm => "Alarm", 
			SIRChannel.Feedback => "Feedback", 
			SIRChannel.Notification => "Notification", 
			SIRChannel.Sabotage => "Sabotage", 
			_ => throw new ArgumentException($"Channel value unknown (value:{channel})"), 
		};
	}

	public static SIRChannel GetChannel(string val)
	{
		return val switch
		{
			"None" => SIRChannel.None, 
			"Alarm" => SIRChannel.Alarm, 
			"Feedback" => SIRChannel.Feedback, 
			"Notification" => SIRChannel.Notification, 
			"Sabotage" => SIRChannel.Sabotage, 
			_ => throw new ArgumentException($"String value (val:{val}) for channel unknown"), 
		};
	}
}
