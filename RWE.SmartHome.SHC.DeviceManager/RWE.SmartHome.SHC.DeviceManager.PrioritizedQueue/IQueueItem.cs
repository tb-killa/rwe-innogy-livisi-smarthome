using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public interface IQueueItem
{
	DateTime SuspendUntil { get; set; }

	DateTime AckPendingUntil { get; set; }

	IBasicDeviceInformation DeviceInformation { get; }

	int Priority { get; }

	ulong RoundCounter { get; set; }

	int Count { get; }

	QueueType QueueType { get; }

	bool More { get; }

	PacketSequence Peek();

	PacketSequence Dequeue();

	void Enqueue(PacketSequence sequence, SendPriority getSendPriority);

	List<PacketSequence> Remove(Predicate<PacketSequence> predicate);

	void Remove(PacketSequence value);

	SendMode GetCurrentMessageMode();

	bool Contains(Predicate<PacketSequence> predicate);
}
