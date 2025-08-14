using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.DeviceManager.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

internal interface ISendScheduler
{
	event EventHandler<SequenceFinishedEventArgs> SequenceFinished;

	event EventHandler<DeviceReachableChangedEventArgs> ReachableChanged;

	Guid Enqueue(PacketSequence sequence);

	void Enqueue(IEnumerable<PacketSequence> sequences);

	Guid Enqueue(PacketSequence sequence, Guid? deviceId);

	bool RemoveConfigurationSequences();

	void RemoveSequencesConditionally(Guid deviceId, Predicate<PacketSequence> predicate, SequenceState sequenceState);

	bool ContainsSequences(Guid deviceId, Predicate<PacketSequence> predicate);

	void RemoveDeviceSpecificQueue(Guid deviceId);

	void AcknowledgePacket(byte[] address, byte sequenceNumber, SIPcosAnswerFrameStatus status, bool isStatusInfo);

	void UpdateLastOnTimeOfDevice(byte[] address, AwakeModifier awakeModifier);

	void UpdateLastOnTimeOfDevice(IDeviceInformation deviceInformation, AwakeModifier awakeModifier);

	void Start();

	void Stop();

	void Suspend();

	void Resume();

	void ForceEchoRequestForUnreachableDevices();
}
