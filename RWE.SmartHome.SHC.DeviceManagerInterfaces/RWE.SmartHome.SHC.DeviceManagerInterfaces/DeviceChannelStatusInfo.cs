using System;
using System.Collections.Generic;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public class DeviceChannelStatusInfo
{
	public SortedList<byte, ChannelState> ChannelStates { get; private set; }

	public DeviceChannelStatusInfo(SIPcosStatusFrame statusFrame)
	{
		ChannelStates = new SortedList<byte, ChannelState>();
		ChannelState value;
		if (statusFrame.Type == SIPcosStatusType.STATUS_FRAME_CC_SENSOR)
		{
			value = CreateChannelState(0, 0, 200, statusFrame.Temperature);
			ChannelStates.Add(200, value);
			value = CreateChannelState(0, 0, 201, statusFrame.Humidity);
			ChannelStates.Add(201, value);
			value = CreateChannelState(0, 0, 5, ((int)(statusFrame.TimeSlotMode & SIPcosStatusTimeSlotMode.MODE_WINDOW) > 0) ? 200 : 0);
			ChannelStates.Add(5, value);
		}
		value = CreateChannelState(statusFrame.ChannelError, (byte)statusFrame.Condition, statusFrame.KeyChannelNumber, statusFrame.IsLevel);
		ChannelStates.Add(value.KeyChannelNumber, value);
	}

	public DeviceChannelStatusInfo(SIPcosAnswerFrame answerFrame)
	{
		ChannelStates = new SortedList<byte, ChannelState>();
		ChannelState channelState = CreateChannelState(answerFrame.ChannelError, (byte)answerFrame.Condition, answerFrame.KeyChannelNumber, answerFrame.IsLevel);
		ChannelStates.Add(channelState.KeyChannelNumber, channelState);
	}

	private static ChannelState CreateChannelState(byte channelError, byte condition, byte keyChannelNumber, int isLevel)
	{
		ChannelState channelState = new ChannelState();
		channelState.ChannelError = channelError;
		ChannelState channelState2 = channelState;
		if (Enum.IsDefined(typeof(DeviceCondition), condition))
		{
			channelState2.Condition = (DeviceCondition)condition;
		}
		else
		{
			channelState2.Condition = DeviceCondition.UnknownLevel;
		}
		channelState2.KeyChannelNumber = keyChannelNumber;
		channelState2.Value = isLevel;
		return channelState2;
	}
}
