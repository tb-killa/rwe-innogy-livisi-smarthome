using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.DomainModel.Types;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ActuatorHandler.SirenHandler;

public static class ChannelsMapperBidcos
{
	private const byte alarmChannel = 3;

	private const byte feedbackChannel = 2;

	private const byte notificationChannel = 1;

	public static SIRChannel GetLocgicChannel(byte? bidcosChannel)
	{
		SIRChannel result = SIRChannel.None;
		if (bidcosChannel.HasValue)
		{
			return bidcosChannel.Value switch
			{
				3 => SIRChannel.Alarm, 
				2 => SIRChannel.Feedback, 
				1 => SIRChannel.Notification, 
				_ => throw new ArgumentException($"Unknown channel (ch:{bidcosChannel.Value})"), 
			};
		}
		return result;
	}

	public static byte? GetBidcosChannel(SIRChannel channel)
	{
		return channel switch
		{
			SIRChannel.None => null, 
			SIRChannel.Alarm => 3, 
			SIRChannel.Feedback => 2, 
			SIRChannel.Notification => 1, 
			_ => throw new ArgumentException($"Unknown channel (ch:{channel.ToString()})"), 
		};
	}

	public static string GetChannelPropertyName(byte channel)
	{
		return channel switch
		{
			3 => "AlarmSoundId", 
			2 => "FeedbackSoundId", 
			1 => "NotificationSoundId", 
			_ => throw new ArgumentException($"Unknown channel (ch:{channel.ToString()})"), 
		};
	}

	public static List<byte> GetAllChannels()
	{
		List<byte> list = new List<byte>();
		list.Add(3);
		list.Add(2);
		list.Add(1);
		return list;
	}
}
