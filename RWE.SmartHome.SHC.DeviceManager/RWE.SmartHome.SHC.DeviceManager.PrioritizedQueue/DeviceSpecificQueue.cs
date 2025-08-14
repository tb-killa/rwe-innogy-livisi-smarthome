using System;
using System.Collections.Generic;
using System.Diagnostics;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class DeviceSpecificQueue : PriorityQueue<PacketSequence, SendPriority>, IQueueItem
{
	private readonly Stopwatch timeBase;

	public IBasicDeviceInformation DeviceInformation { get; private set; }

	public DateTime SuspendUntil { get; set; }

	public ulong RoundCounter { get; set; }

	public DateTime AckPendingUntil { get; set; }

	public QueueType QueueType { get; private set; }

	public bool More
	{
		get
		{
			if (base.Count == 1)
			{
				PacketSequence packetSequence = Peek();
				return packetSequence.More;
			}
			return true;
		}
	}

	public int Priority
	{
		get
		{
			if (base.Count == 0)
			{
				return -1;
			}
			bool flag = DeviceInformation.Awake();
			SendPriority priority = PeekItem().Priority;
			if (!flag)
			{
				return priority.Sleeping;
			}
			return priority.Awake;
		}
	}

	public DeviceSpecificQueue(IBasicDeviceInformation deviceInformation, QueueType queueType, Stopwatch timeBase)
		: base((IComparer<SendPriority>)new SendPriorityComparer(), (Predicate<PacketSequence>)((PacketSequence sequence) => sequence.Started))
	{
		this.timeBase = timeBase;
		QueueType = queueType;
		SuspendUntil = DateTime.MinValue;
		DeviceInformation = deviceInformation;
	}

	public override void Enqueue(PacketSequence value, SendPriority priority)
	{
		value.Parent = this;
		value.EnqueueDate = timeBase.ElapsedMilliseconds;
		base.Enqueue(value, priority);
	}

	public SendMode GetCurrentMessageMode()
	{
		SendMode result = SendMode.Normal;
		if (!DeviceInformation.Awake() || DeviceInformation.DeviceInclusionState != DeviceInclusionState.Included)
		{
			switch (DeviceInformation.BestOperationMode)
			{
			case DeviceInfoOperationModes.BurstListener:
				result = SendMode.Burst;
				break;
			case DeviceInfoOperationModes.TripleBurstListener:
				result = SendMode.TripleBurst;
				break;
			}
		}
		return result;
	}
}
