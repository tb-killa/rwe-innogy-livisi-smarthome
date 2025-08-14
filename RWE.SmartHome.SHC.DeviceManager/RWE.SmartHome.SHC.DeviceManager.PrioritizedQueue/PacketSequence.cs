using System;
using System.Collections.Generic;
using System.Diagnostics;
using RWE.SmartHome.SHC.CommonFunctionality;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class PacketSequence
{
	public SendStatus SendStatus { get; set; }

	public SequenceType SequenceType { get; private set; }

	public IQueueItem Parent { get; set; }

	public byte[] Address { get; private set; }

	public long EnqueueDate { get; set; }

	public TimeSpan Lifetime { get; set; }

	private int ActualElement { get; set; }

	public int ErrorCount { get; set; }

	public Guid CorrelationId { get; private set; }

	private List<Packet> Packets { get; set; }

	public bool HighPriority { get; private set; }

	public bool ForceStayAwake { get; set; }

	public Packet Current
	{
		get
		{
			if (ActualElement > Packets.Count - 1)
			{
				return null;
			}
			return Packets[ActualElement];
		}
	}

	public bool Started { get; set; }

	public bool More => ActualElement < Packets.Count - 1;

	public PacketSequence()
		: this(SequenceType.Other)
	{
		HighPriority = false;
	}

	public PacketSequence(SequenceType sequenceType)
	{
		CorrelationId = Guid.NewGuid();
		SequenceType = sequenceType;
		Lifetime = TimeSpan.MaxValue;
		ActualElement = -1;
		ErrorCount = 0;
		Packets = new List<Packet>();
	}

	public PacketSequence(SequenceType sequenceType, bool highPriority)
		: this(sequenceType)
	{
		HighPriority = highPriority;
	}

	public PacketSequence(Packet packet, SequenceType sequenceType)
		: this(sequenceType)
	{
		Add(packet);
	}

	public PacketSequence(CORESTACKMessage message, SequenceType sequenceType)
		: this(sequenceType)
	{
		Add(message);
	}

	public PacketSequence(SIPCOSMessage message, SequenceType sequenceType)
		: this(sequenceType)
	{
		Add(message);
	}

	public PacketSequence(Packet packet)
		: this()
	{
		Add(packet);
	}

	public PacketSequence(CORESTACKMessage message)
		: this()
	{
		Add(message);
	}

	public PacketSequence(SIPCOSMessage message)
		: this()
	{
		Add(message);
	}

	public void Add(Packet packet)
	{
		if (Packets.Count == 0)
		{
			Address = packet.Header.MacDestination;
		}
		else if (!packet.Header.MacDestination.Compare(Address))
		{
			throw new InvalidOperationException("Sequences can contain only messages for one device.");
		}
		Packets.Add(packet);
		Lifetime = Lifetime.Min(Defaults.Lifetime(packet));
	}

	public void Add(CORESTACKMessage packet)
	{
		Add(new Packet(packet));
	}

	public void Add(SIPCOSMessage packet)
	{
		Add(new Packet(packet));
	}

	public bool MoveNext()
	{
		if (ActualElement + 1 >= Packets.Count)
		{
			return false;
		}
		ActualElement++;
		return true;
	}

	public void Reset(bool resetSendState)
	{
		foreach (Packet packet in Packets)
		{
			packet.State = PacketSendState.Open;
		}
		ActualElement = 0;
		ErrorCount = 0;
	}

	public bool CheckLifetime(Stopwatch timebase)
	{
		if (Lifetime == TimeSpan.MaxValue)
		{
			return true;
		}
		return (double)EnqueueDate + Lifetime.TotalMilliseconds > (double)timebase.ElapsedMilliseconds;
	}
}
