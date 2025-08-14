using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public static class Defaults
{
	private enum PrioritizedFrames
	{
		ApplicationAck,
		NetworkAcceptFrame,
		NetworkInfoFrame,
		TimeInformation,
		DirectExecutionCommand,
		ConfigurationCommand,
		RemoveLink,
		CreateLink,
		ConfigureLink,
		SwitchCommand,
		NetworkExclusion,
		FirmwareUpdate,
		Icmp,
		HighPriority
	}

	public const byte MaxPendingAcks = 2;

	public const byte MaxErrorCount = 3;

	public static readonly TimeSpan WaitForAppAck = new TimeSpan(0, 0, 0, 10);

	public static readonly int WaitForAppAckRouterPresent = 30;

	public static readonly TimeSpan WaitForIcmp = new TimeSpan(0, 0, 10, 0);

	public static readonly TimeSpan WaitForRouterInit = new TimeSpan(0, 0, 0, 20);

	public static readonly Dictionary<SendStatus, BackoffTime> BackoffTimes = new Dictionary<SendStatus, BackoffTime>
	{
		{
			SendStatus.ACK,
			new BackoffTime(0, 0, 0)
		},
		{
			SendStatus.BUSY,
			new BackoffTime(50, 0, 0)
		},
		{
			SendStatus.TIMEOUT,
			new BackoffTime(50, 0, 1)
		},
		{
			SendStatus.SERIAL_TIMEOUT,
			new BackoffTime(50, 0, 0)
		},
		{
			SendStatus.MEDIUM_BUSY,
			new BackoffTime(100, 0, 0)
		},
		{
			SendStatus.NO_REPLY,
			new BackoffTime(0, 300, 1)
		},
		{
			SendStatus.ERROR,
			new BackoffTime(0, 300, 1)
		},
		{
			SendStatus.INCOMMING,
			new BackoffTime(300, 0, 0)
		},
		{
			SendStatus.CRC_ERROR,
			new BackoffTime(0, 0, 1)
		},
		{
			SendStatus.MODE_ERROR,
			new BackoffTime(50, 0, 3)
		},
		{
			SendStatus.DUTY_CYCLE,
			new BackoffTime(1000, 0, 0)
		},
		{
			SendStatus.MULTI_CAST,
			new BackoffTime(0, 0, 0)
		},
		{
			SendStatus.BIDCOS_INCLUSION_FAILED,
			new BackoffTime(0, 300, 1)
		},
		{
			SendStatus.BIDCOS_GROUP_ADDRESS_FAILED,
			new BackoffTime(0, 300, 1)
		}
	};

	private static readonly Dictionary<PrioritizedFrames, SendPriority> EventCyclicPriorities = new Dictionary<PrioritizedFrames, SendPriority>
	{
		{
			PrioritizedFrames.ApplicationAck,
			new SendPriority(1000, 0)
		},
		{
			PrioritizedFrames.NetworkAcceptFrame,
			new SendPriority(990, 0)
		},
		{
			PrioritizedFrames.NetworkInfoFrame,
			new SendPriority(990, 0)
		},
		{
			PrioritizedFrames.HighPriority,
			new SendPriority(989, 0)
		},
		{
			PrioritizedFrames.TimeInformation,
			new SendPriority(980, 0)
		},
		{
			PrioritizedFrames.DirectExecutionCommand,
			new SendPriority(970, 0)
		},
		{
			PrioritizedFrames.ConfigurationCommand,
			new SendPriority(700, 0)
		},
		{
			PrioritizedFrames.RemoveLink,
			new SendPriority(703, 0)
		},
		{
			PrioritizedFrames.CreateLink,
			new SendPriority(702, 0)
		},
		{
			PrioritizedFrames.ConfigureLink,
			new SendPriority(701, 0)
		},
		{
			PrioritizedFrames.SwitchCommand,
			new SendPriority(600, 0)
		},
		{
			PrioritizedFrames.NetworkExclusion,
			new SendPriority(500, 0)
		},
		{
			PrioritizedFrames.FirmwareUpdate,
			new SendPriority(380, 0)
		},
		{
			PrioritizedFrames.Icmp,
			new SendPriority(199, 0)
		}
	};

	private static readonly Dictionary<PrioritizedFrames, SendPriority> TripleBurstPriorities = new Dictionary<PrioritizedFrames, SendPriority>
	{
		{
			PrioritizedFrames.ApplicationAck,
			new SendPriority(940, 0)
		},
		{
			PrioritizedFrames.NetworkAcceptFrame,
			new SendPriority(890, 770)
		},
		{
			PrioritizedFrames.NetworkInfoFrame,
			new SendPriority(890, 770)
		},
		{
			PrioritizedFrames.HighPriority,
			new SendPriority(889, 769)
		},
		{
			PrioritizedFrames.TimeInformation,
			new SendPriority(790, 740)
		},
		{
			PrioritizedFrames.DirectExecutionCommand,
			new SendPriority(780, 730)
		},
		{
			PrioritizedFrames.ConfigurationCommand,
			new SendPriority(350, 50)
		},
		{
			PrioritizedFrames.RemoveLink,
			new SendPriority(353, 53)
		},
		{
			PrioritizedFrames.CreateLink,
			new SendPriority(352, 52)
		},
		{
			PrioritizedFrames.ConfigureLink,
			new SendPriority(351, 51)
		},
		{
			PrioritizedFrames.SwitchCommand,
			new SendPriority(300, 20)
		},
		{
			PrioritizedFrames.NetworkExclusion,
			new SendPriority(250, 17)
		},
		{
			PrioritizedFrames.FirmwareUpdate,
			new SendPriority(190, 15)
		},
		{
			PrioritizedFrames.Icmp,
			new SendPriority(25, 8)
		}
	};

	private static readonly Dictionary<PrioritizedFrames, SendPriority> BurstPriorities = new Dictionary<PrioritizedFrames, SendPriority>
	{
		{
			PrioritizedFrames.ApplicationAck,
			new SendPriority(930, 0)
		},
		{
			PrioritizedFrames.NetworkAcceptFrame,
			new SendPriority(880, 830)
		},
		{
			PrioritizedFrames.NetworkInfoFrame,
			new SendPriority(880, 790)
		},
		{
			PrioritizedFrames.HighPriority,
			new SendPriority(879, 789)
		},
		{
			PrioritizedFrames.TimeInformation,
			new SendPriority(780, 730)
		},
		{
			PrioritizedFrames.DirectExecutionCommand,
			new SendPriority(770, 720)
		},
		{
			PrioritizedFrames.ConfigurationCommand,
			new SendPriority(250, 60)
		},
		{
			PrioritizedFrames.RemoveLink,
			new SendPriority(253, 63)
		},
		{
			PrioritizedFrames.CreateLink,
			new SendPriority(252, 62)
		},
		{
			PrioritizedFrames.ConfigureLink,
			new SendPriority(251, 61)
		},
		{
			PrioritizedFrames.SwitchCommand,
			new SendPriority(150, 25)
		},
		{
			PrioritizedFrames.NetworkExclusion,
			new SendPriority(125, 15)
		},
		{
			PrioritizedFrames.FirmwareUpdate,
			new SendPriority(95, 8)
		},
		{
			PrioritizedFrames.Icmp,
			new SendPriority(12, 7)
		}
	};

	private static readonly Dictionary<PrioritizedFrames, SendPriority> PermanentPriorities = new Dictionary<PrioritizedFrames, SendPriority>
	{
		{
			PrioritizedFrames.ApplicationAck,
			new SendPriority(920)
		},
		{
			PrioritizedFrames.NetworkAcceptFrame,
			new SendPriority(870)
		},
		{
			PrioritizedFrames.NetworkInfoFrame,
			new SendPriority(870)
		},
		{
			PrioritizedFrames.HighPriority,
			new SendPriority(869)
		},
		{
			PrioritizedFrames.TimeInformation,
			new SendPriority(770)
		},
		{
			PrioritizedFrames.DirectExecutionCommand,
			new SendPriority(760)
		},
		{
			PrioritizedFrames.ConfigurationCommand,
			new SendPriority(87)
		},
		{
			PrioritizedFrames.RemoveLink,
			new SendPriority(90)
		},
		{
			PrioritizedFrames.CreateLink,
			new SendPriority(89)
		},
		{
			PrioritizedFrames.ConfigureLink,
			new SendPriority(88)
		},
		{
			PrioritizedFrames.SwitchCommand,
			new SendPriority(75)
		},
		{
			PrioritizedFrames.NetworkExclusion,
			new SendPriority(70)
		},
		{
			PrioritizedFrames.FirmwareUpdate,
			new SendPriority(47)
		},
		{
			PrioritizedFrames.Icmp,
			new SendPriority(10)
		}
	};

	private static readonly SortedList<AwakeModifier, TimeSpan> AwakeTimesSipCos = new SortedList<AwakeModifier, TimeSpan>
	{
		{
			AwakeModifier.Bidi,
			new TimeSpan(0, 0, 10)
		},
		{
			AwakeModifier.DeviceInfoReceived,
			new TimeSpan(0, 0, 10)
		},
		{
			AwakeModifier.StayAwake,
			new TimeSpan(0, 0, 30)
		},
		{
			AwakeModifier.None,
			TimeSpan.Zero
		}
	};

	private static readonly SortedList<AwakeModifier, TimeSpan> AwakeTimesBidCos = new SortedList<AwakeModifier, TimeSpan>
	{
		{
			AwakeModifier.Bidi,
			new TimeSpan(0, 0, 3)
		},
		{
			AwakeModifier.DeviceInfoReceived,
			new TimeSpan(0, 0, 20)
		},
		{
			AwakeModifier.StayAwake,
			new TimeSpan(0, 0, 3)
		},
		{
			AwakeModifier.None,
			TimeSpan.Zero
		}
	};

	private static readonly SortedList<AwakeModifier, TimeSpan> AwakeTimesPermanentListener = new SortedList<AwakeModifier, TimeSpan>
	{
		{
			AwakeModifier.Bidi,
			TimeSpan.MaxValue
		},
		{
			AwakeModifier.DeviceInfoReceived,
			TimeSpan.MaxValue
		},
		{
			AwakeModifier.StayAwake,
			TimeSpan.MaxValue
		},
		{
			AwakeModifier.None,
			TimeSpan.MaxValue
		}
	};

	public static TimeSpan Lifetime(Packet packet)
	{
		TimeSpan timeSpan = TimeSpan.MaxValue;
		if (packet.Header.CorestackFrameType != CorestackFrameType.COMPRESSED_ICMP)
		{
			SIPcosHeader sIPcosHeader = packet.Header as SIPcosHeader;
			switch (sIPcosHeader.FrameType)
			{
			case SIPcosFrameType.ANSWER:
			case SIPcosFrameType.DIRECT_EXECUTION:
			case SIPcosFrameType.TIME_INFORMATION:
			case SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND:
			case SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND:
			case SIPcosFrameType.LEVEL_COMMAND:
				timeSpan = timeSpan.Min(new TimeSpan(0, 1, 0));
				break;
			}
		}
		return timeSpan;
	}

	public static SendPriority GetSendPriority(IBasicDeviceInformation deviceInformation, PacketSequence sequence)
	{
		Dictionary<PrioritizedFrames, SendPriority> dictionary = deviceInformation.BestOperationMode switch
		{
			DeviceInfoOperationModes.MainsPowered => PermanentPriorities, 
			DeviceInfoOperationModes.EventListener => EventCyclicPriorities, 
			DeviceInfoOperationModes.BurstListener => BurstPriorities, 
			DeviceInfoOperationModes.TripleBurstListener => TripleBurstPriorities, 
			DeviceInfoOperationModes.CyclicListener => EventCyclicPriorities, 
			_ => throw new ArgumentOutOfRangeException("deviceInformation", "deviceInformation.BestOperationMode is out of range"), 
		};
		SendPriority sendPriority = new SendPriority(0);
		while (sequence.MoveNext())
		{
			if (sequence.Current.Header.CorestackFrameType == CorestackFrameType.COMPRESSED_ICMP)
			{
				sendPriority = dictionary[PrioritizedFrames.Icmp];
				continue;
			}
			SIPcosHeader sIPcosHeader = sequence.Current.Header as SIPcosHeader;
			switch (sIPcosHeader.FrameType)
			{
			case SIPcosFrameType.ANSWER:
				sendPriority = dictionary[PrioritizedFrames.ApplicationAck];
				break;
			case SIPcosFrameType.NETWORK_MANAGEMENT_FRAME:
				switch ((SIPcosNetworkManagementFrameType)sequence.Current.Message[0])
				{
				case SIPcosNetworkManagementFrameType.NetworkAcceptFrame:
				case SIPcosNetworkManagementFrameType.ForwardedNetworkAcceptFrame:
					sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.NetworkAcceptFrame]);
					break;
				case SIPcosNetworkManagementFrameType.NetworkInfoFrame:
				case SIPcosNetworkManagementFrameType.ForwardedNetworkInfoFrame:
					sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.NetworkInfoFrame]);
					break;
				case SIPcosNetworkManagementFrameType.NetworkExcludeFrame:
					sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.NetworkExclusion]);
					break;
				}
				break;
			case SIPcosFrameType.TIME_INFORMATION:
				sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.TimeInformation]);
				break;
			case SIPcosFrameType.DIRECT_EXECUTION:
				sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.DirectExecutionCommand]);
				break;
			case SIPcosFrameType.CONFIGURATION:
				switch ((SipCosConfigurationCommands)sequence.Current.Message[1])
				{
				case SipCosConfigurationCommands.RemoveLink:
					sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.RemoveLink]);
					break;
				case SipCosConfigurationCommands.CreateLink:
					sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.CreateLink]);
					break;
				case SipCosConfigurationCommands.ParameterOffset:
				case SipCosConfigurationCommands.ParameterIndex:
					sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.ConfigureLink]);
					break;
				default:
					sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.ConfigurationCommand]);
					break;
				}
				break;
			case SIPcosFrameType.STATUSINFO:
			case SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND:
			case SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND:
			case SIPcosFrameType.LEVEL_COMMAND:
				sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.SwitchCommand]);
				break;
			case SIPcosFrameType.FIRMWARE_UPDATE:
				sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.FirmwareUpdate]);
				break;
			case SIPcosFrameType.VIRTUAL_BIDCOS_COMMAND:
				sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.ConfigurationCommand]);
				break;
			}
		}
		if (sequence.HighPriority)
		{
			sendPriority = sendPriority.Max(dictionary[PrioritizedFrames.HighPriority]);
		}
		sequence.Reset(resetSendState: false);
		return sendPriority;
	}

	public static SortedList<AwakeModifier, TimeSpan> GetAwakeTimes(ProtocolType protocolType, DeviceInfoOperationModes operationMode)
	{
		switch (protocolType)
		{
		case ProtocolType.SipCos:
			if (protocolType == ProtocolType.SipCos)
			{
				switch (operationMode)
				{
				case DeviceInfoOperationModes.EventListener:
				case DeviceInfoOperationModes.BurstListener:
				case DeviceInfoOperationModes.TripleBurstListener:
				case DeviceInfoOperationModes.CyclicListener:
					return AwakeTimesSipCos;
				case DeviceInfoOperationModes.MainsPowered:
					return AwakeTimesPermanentListener;
				}
			}
			break;
		case ProtocolType.BidCos:
			return AwakeTimesBidCos;
		}
		return null;
	}

	public static DateTime GetIcmpPendingTime(DateTime now, IRandomNumberGenerator randomNumberGenerator, int rangeInSeconds)
	{
		return now.Add(WaitForIcmp).AddSeconds(-rangeInSeconds).AddSeconds(randomNumberGenerator.Next(0, rangeInSeconds * 2));
	}
}
