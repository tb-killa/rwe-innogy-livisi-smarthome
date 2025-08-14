using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class SipCosValueChangedEventArgs : EventArgs
{
	public SortedList<byte, ChannelState> ChannelStates { get; private set; }

	public Guid DeviceId { get; private set; }

	public SipCosValueChangedEventArgs(Guid deviceId, SortedList<byte, ChannelState> channelStates)
	{
		ChannelStates = channelStates;
		DeviceId = deviceId;
	}
}
