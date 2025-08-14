using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.Core.Scheduler;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer;

internal class BidCosHandlerRef
{
	public const byte Wsd1AlarmOff = 1;

	public const byte Wsd1AlarmOn = 200;

	public const byte Wsd2AlarmOff = 0;

	public const byte Wsd2AlarmOn = 198;

	private readonly Func<byte[]> defaultIp;

	private readonly Func<List<byte>, SendMode, SendStatus> broadcastFrameToAir;

	public readonly SIPcosHandler m_sipcos;

	public byte m_frameCount;

	public volatile bool m_answer_ack;

	public byte[,] m_partner;

	public bool m_block_answer;

	public BIDCOSAnswerFrame answer;

	public readonly EventWaitHandle answerFrameReceivedEvent = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);

	public readonly object syncRoot = new object();

	public bool acceptInfoAsResponse;

	public bool forceAcceptSysInfoFrame;

	public byte[] nodePendingInclusion;

	public DateTime answerFrameReceiveTime;

	public AesChallengeResponse aesChallengeResponse;

	public bool LinkWSDGroups = true;

	public readonly IBidCoSKeyRetriever bidcosKeyRetriever;

	public readonly byte[] WsdAlarmOnCodes = new byte[2] { 200, 198 };

	public readonly byte[] WsdAlarmOffCodes = new byte[2] { 1, 0 };

	public BidCosNodesCollectionManager NodesManager { get; private set; }

	public byte[] DefaultIP => defaultIp();

	public BidCosHandlerRef(SIPcosHandler mSipcos, Func<byte[]> defaultIp, Func<List<byte>, SendMode, SendStatus> broadcastFrameToAir, IBidCoSPersistence bidcosStorage, IBidCoSKeyRetriever bidcosKeyRetriever, IScheduler scheduler)
	{
		m_sipcos = mSipcos;
		this.defaultIp = defaultIp;
		this.broadcastFrameToAir = broadcastFrameToAir;
		this.bidcosKeyRetriever = bidcosKeyRetriever;
		NodesManager = new BidCosNodesCollectionManager(this, bidcosStorage, scheduler);
	}

	public SendStatus BroadcastFrameToAir(List<byte> data, SendMode Mode)
	{
		return broadcastFrameToAir(data, Mode);
	}

	public byte GetDefaultHeader()
	{
		return 160;
	}

	public bool AddressesEqual(byte[] addr1, byte[] addr2)
	{
		if (addr1 == null && addr2 == null)
		{
			return true;
		}
		if (addr1 == null || addr2 == null)
		{
			return false;
		}
		return addr1.SequenceEqual(addr2);
	}

	internal void RepeatConditionalSwitchFrame(byte[] receiver, byte smokeAlarmValue)
	{
		byte[] message = null;
		if (WsdAlarmOnCodes.Contains(smokeAlarmValue))
		{
			message = new byte[4] { 1, 1, 200, 0 };
		}
		if (WsdAlarmOffCodes.Contains(smokeAlarmValue))
		{
			message = new byte[4] { 1, 1, 1, 0 };
		}
		if (AddressesEqual(receiver, NodesManager.Wsd1GroupAddress))
		{
			NodesManager.FindWsd2Device()?.SendConditionalSwitch(message);
		}
		if (AddressesEqual(receiver, NodesManager.Wsd2GroupAddress))
		{
			NodesManager.FindWsd1Device()?.SendConditionalSwitch(message);
		}
	}
}
