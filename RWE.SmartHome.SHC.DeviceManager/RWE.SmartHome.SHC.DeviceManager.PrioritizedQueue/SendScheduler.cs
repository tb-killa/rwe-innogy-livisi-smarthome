using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.Configuration;
using RWE.SmartHome.SHC.DeviceManager.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

internal class SendScheduler : ISendScheduler
{
	private const string LoggingSource = "SendScheduler";

	private const int StartingByteRoutedDeviceAddress = 40;

	private readonly List<IQueueItem> deviceSpecificQueues = new List<IQueueItem>();

	private readonly Thread workerThread;

	private readonly IDeviceList deviceList;

	private readonly TimeSpan waitForAppAckRouterPresent;

	private readonly ManualResetEvent overallWaitObject = new ManualResetEvent(initialState: false);

	private readonly AutoResetEvent loopWaitObject = new AutoResetEvent(initialState: false);

	private readonly ManualResetEvent suspendedWaitObject = new ManualResetEvent(initialState: true);

	private readonly IRandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

	private ulong roundCounter;

	private readonly WaitingMaxItemCollection<PacketSequence> pendingApplicationAckPackets;

	private bool isRunning;

	private readonly ISIPcosHandler transceiver;

	private readonly IICMPHandler icmpHandler;

	private readonly IEventManager eventManager;

	private static readonly TimeSpan MaximumLoopWaitTimeSpan = new TimeSpan(0, 0, 1, 0);

	private readonly Stopwatch stopwatch = new Stopwatch();

	private readonly RfStatistics rfStatistics;

	private object SyncRoot => deviceList.SyncRoot;

	public event EventHandler<SequenceFinishedEventArgs> SequenceFinished;

	public event EventHandler<DeviceReachableChangedEventArgs> ReachableChanged;

	public SendScheduler(ISIPcosHandler transceiver, IICMPHandler icmpHandler, IDeviceList deviceList, IEventManager eventManager, IScheduler scheduler, RWE.SmartHome.SHC.DeviceManager.Configuration.Configuration configuration)
	{
		long reportIntervall = 60L;
		if (configuration != null)
		{
			reportIntervall = configuration.StatisticsObservationPeriod ?? 60;
		}
		rfStatistics = new RfStatistics(eventManager, scheduler, reportIntervall);
		int maximumCount = 2;
		if (configuration != null)
		{
			maximumCount = configuration.MaxPendingAcks ?? 2;
		}
		pendingApplicationAckPackets = new WaitingMaxItemCollection<PacketSequence>(maximumCount);
		int valueOrDefault = Defaults.WaitForAppAckRouterPresent;
		if (configuration != null)
		{
			valueOrDefault = configuration.RouterWaitForAppAckTime.GetValueOrDefault(Defaults.WaitForAppAckRouterPresent);
		}
		waitForAppAckRouterPresent = TimeSpan.FromSeconds(valueOrDefault);
		stopwatch.Start();
		this.eventManager = eventManager;
		this.icmpHandler = icmpHandler;
		icmpHandler.ReceiveData += IcmpMessageReceived;
		this.deviceList = deviceList;
		this.transceiver = transceiver;
		workerThread = new Thread(SendLoop);
	}

	private void IcmpMessageReceived(ICMPMessage message)
	{
		bool flag = false;
		PacketSequence packetSequence = null;
		if (message.Header.IpSource.SequenceEqual(SipCosAddress.InvalidAddress))
		{
			Log.Error(Module.DeviceManager, "SendScheduler", $"Received ICMP message with empty IPSource from address {message.Header.MacSource.ToReadable()} ");
			return;
		}
		Log.Debug(Module.DeviceManager, "SendScheduler", $"ICMP {message.Type} message received.");
		lock (SyncRoot)
		{
			if (message.Type == ICMP_type.ECHO_REPLY)
			{
				IDeviceInformation deviceInformation = deviceList[message.Header.IpSource];
				if (deviceInformation != null)
				{
					IQueueItem queueItem = deviceSpecificQueues.Find((IQueueItem q) => q.QueueType == QueueType.Icmp && q.DeviceInformation.Address.Compare(message.Header.IpSource));
					if (queueItem != null)
					{
						Log.Debug(Module.DeviceManager, "SendScheduler", "Answer for ICMP Ping received.");
						packetSequence = queueItem.Peek();
						flag = !MoveToNextPacketInSequence(packetSequence);
					}
				}
			}
		}
		if (flag)
		{
			RaiseSequenceFinished(packetSequence, SequenceState.Success);
		}
	}

	private void SendLoop()
	{
		while (isRunning)
		{
			try
			{
				Log.Debug(Module.DeviceManager, "SendScheduler", "------------------ Start loop ------------------");
				IQueueItem highestPrioritizedQueue = GetHighestPrioritizedQueue();
				suspendedWaitObject.WaitOne();
				if (!isRunning || highestPrioritizedQueue == null)
				{
					break;
				}
				Monitor.Enter(SyncRoot);
				bool flag;
				PacketSequence packetSequence;
				SequenceState sequenceState;
				int overallWaitTime;
				try
				{
					if (highestPrioritizedQueue.Count == 0)
					{
						continue;
					}
					flag = SendPacket(highestPrioritizedQueue, out packetSequence, out sequenceState, out overallWaitTime);
					goto IL_0071;
				}
				finally
				{
					try
					{
						Monitor.Exit(SyncRoot);
					}
					catch
					{
					}
				}
				IL_0071:
				if (flag)
				{
					RaiseSequenceFinished(packetSequence, sequenceState);
				}
				if (overallWaitTime > 0)
				{
					Log.Debug(Module.DeviceManager, "SendScheduler", $"Overall wait for {overallWaitTime} ms");
					overallWaitObject.WaitOne(overallWaitTime, exitContext: false);
				}
			}
			catch (Exception ex)
			{
				Log.Error(Module.DeviceManager, "SendScheduler", $"SendLoop threw exception {ex.Message} with details: {ex}");
			}
		}
	}

	private IQueueItem GetHighestPrioritizedQueue()
	{
		IQueueItem queueItem = null;
		while (isRunning && queueItem == null)
		{
			pendingApplicationAckPackets.Wait(RemoveTimedOutAppAcks);
			suspendedWaitObject.WaitOne();
			TimeSpan wait;
			lock (SyncRoot)
			{
				queueItem = GetHighestPrioritizedQueue(out wait);
			}
			Log.Debug(Module.DeviceManager, "SendScheduler", $"Wait for {wait.TotalMilliseconds} ms");
			loopWaitObject.WaitOne((int)wait.TotalMilliseconds, exitContext: false);
		}
		return queueItem;
	}

	private bool SendPacket(IQueueItem highestPrioritizedQueue, out PacketSequence packetSequence, out SequenceState sequenceState, out int overallWaitTime)
	{
		Log.Debug(Module.DeviceManager, "SendScheduler", $"Highest prioritized ({highestPrioritizedQueue.Priority}) queue is for device {deviceList.LogInfoByAddress(highestPrioritizedQueue.DeviceInformation.Address)} and has {highestPrioritizedQueue.Count} sequences");
		highestPrioritizedQueue.RoundCounter = ++roundCounter;
		Log.Debug(Module.DeviceManager, "SendScheduler", $"Current round counter = {roundCounter}");
		bool flag = false;
		sequenceState = SequenceState.WrongFormat;
		overallWaitTime = 0;
		packetSequence = highestPrioritizedQueue.Peek();
		if (packetSequence.CheckLifetime(stopwatch))
		{
			SendStatus sendStatus = SendMessage(highestPrioritizedQueue, packetSequence);
			packetSequence.SendStatus = sendStatus;
			rfStatistics.AddSendResult(sendStatus);
			if (sendStatus == SendStatus.ACK || sendStatus == SendStatus.MULTI_CAST)
			{
				if (packetSequence.Current.Header is SIPcosHeader sIPcosHeader)
				{
					if (!sIPcosHeader.BiDi || sendStatus == SendStatus.MULTI_CAST)
					{
						packetSequence.Current.State = PacketSendState.Done;
						sequenceState = SequenceState.Success;
						flag = !MoveToNextPacketInSequence(packetSequence);
					}
					highestPrioritizedQueue.DeviceInformation.UpdateAwakeState(sIPcosHeader.StayAwake ? AwakeModifier.StayAwake : AwakeModifier.None);
				}
			}
			else
			{
				RemovePendingAckPacket(packetSequence, AckResult.OtherError);
				packetSequence.Current.State = PacketSendState.Open;
				BackoffTime backoffTime = Defaults.BackoffTimes[sendStatus];
				if (backoffTime.DeviceSpecificWaitTime > 0)
				{
					Log.Debug(Module.DeviceManager, "SendScheduler", $"Queue wait for {backoffTime.DeviceSpecificWaitTime} ms");
					highestPrioritizedQueue.SuspendUntil = DateTime.UtcNow.AddMilliseconds(backoffTime.DeviceSpecificWaitTime);
				}
				overallWaitTime = backoffTime.OverallWaitTime;
				packetSequence.ErrorCount += backoffTime.ErrorCountIncrease;
				if (sendStatus == SendStatus.NO_REPLY)
				{
					MarkDeviceAsSleeping(packetSequence.Parent);
					if (IsUnreachableBidcosStatusInfo(highestPrioritizedQueue))
					{
						flag = true;
						sequenceState = SequenceState.Timeout;
					}
				}
				if (sendStatus == SendStatus.MODE_ERROR)
				{
					flag = true;
					Log.Error(Module.DeviceManager, "SendScheduler", $"Send status was Mode Error. Sequence will be discarded.");
				}
				else
				{
					CheckForExceededErrorCount(packetSequence, sendStatus);
				}
			}
			if (packetSequence.SequenceType == SequenceType.Icmp)
			{
				packetSequence.Parent.SuspendUntil = Defaults.GetIcmpPendingTime(DateTime.UtcNow, randomNumberGenerator, 120);
			}
			CheckIfBidCosInclusionNotificationNeedsToBeTriggered(sendStatus, packetSequence);
		}
		else
		{
			Log.Warning(Module.DeviceManager, "SendScheduler", "Lifetime of sequence reached");
			sequenceState = SequenceState.Timeout;
			flag = true;
		}
		if (flag && sequenceState != SequenceState.Success)
		{
			if (highestPrioritizedQueue.Count > 0)
			{
				highestPrioritizedQueue.Dequeue();
			}
			RemoveQueueIfEmpty(highestPrioritizedQueue);
		}
		return flag;
	}

	private void CheckIfBidCosInclusionNotificationNeedsToBeTriggered(SendStatus sendStatus, PacketSequence packetSequence)
	{
		if (packetSequence.Parent.DeviceInformation.DeviceInclusionState != DeviceInclusionState.Included && packetSequence.Parent.DeviceInformation.ProtocolType == ProtocolType.BidCos && packetSequence.SequenceType == SequenceType.Inclusion)
		{
			switch (sendStatus)
			{
			case SendStatus.TIMEOUT:
			case SendStatus.NO_REPLY:
			case SendStatus.ERROR:
			case SendStatus.CRC_ERROR:
			case SendStatus.MODE_ERROR:
			case SendStatus.DUTY_CYCLE:
			case SendStatus.MULTI_CAST:
			case SendStatus.BIDCOS_INCLUSION_FAILED:
			case SendStatus.BIDCOS_GROUP_ADDRESS_FAILED:
				eventManager.GetEvent<DeviceInclusionTimeoutEvent>().Publish(new DeviceInclusionTimeoutEventArgs(packetSequence.Parent.DeviceInformation.DeviceId));
				break;
			default:
				throw new ArgumentOutOfRangeException("sendStatus");
			case SendStatus.ACK:
			case SendStatus.BUSY:
			case SendStatus.SERIAL_TIMEOUT:
			case SendStatus.MEDIUM_BUSY:
			case SendStatus.INCOMMING:
				break;
			}
		}
	}

	private SendStatus SendMessage(IQueueItem highestPrioritizedQueue, PacketSequence packetSequence)
	{
		SIPcosHeader sIPcosHeader = packetSequence.Current.Header as SIPcosHeader;
		packetSequence.Started = true;
		packetSequence.Current.State = PacketSendState.WaitingForMacAck;
		if (sIPcosHeader != null)
		{
			if (sIPcosHeader.BiDi)
			{
				MarkSequenceAsWaitingForAppAck(packetSequence);
			}
			else
			{
				packetSequence.Parent.AckPendingUntil = DateTime.MinValue;
			}
		}
		Monitor.Exit(SyncRoot);
		SendStatus sendStatus;
		if (sIPcosHeader != null)
		{
			sIPcosHeader.StayAwake = packetSequence.ForceStayAwake || highestPrioritizedQueue.More;
			if (sIPcosHeader.FrameType != SIPcosFrameType.ANSWER)
			{
				SyncSequenceNumber(sIPcosHeader, highestPrioritizedQueue);
			}
			SIPCOSMessage sIPCOSMessage = new SIPCOSMessage(sIPcosHeader, new List<byte>(packetSequence.Current.Message));
			sIPCOSMessage.Mode = highestPrioritizedQueue.GetCurrentMessageMode();
			SIPCOSMessage sIPCOSMessage2 = sIPCOSMessage;
			long timeToDeliver = this.stopwatch.ElapsedMilliseconds - packetSequence.EnqueueDate;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			IBasicDeviceInformation deviceInformation = packetSequence.Parent.DeviceInformation;
			sendStatus = (((deviceInformation.DeviceInclusionState == DeviceInclusionState.Included || deviceInformation.DeviceInclusionState == DeviceInclusionState.ExclusionPending || deviceInformation.DeviceInclusionState == DeviceInclusionState.Excluded) && !IsForwardedNetworkManagementFrame(sIPCOSMessage2)) ? transceiver.SendMessage(sIPCOSMessage2) : transceiver.SendMessageDefaultSync(sIPCOSMessage2.Header, sIPCOSMessage2.Data.ToArray(), sIPCOSMessage2.Mode));
			stopwatch.Stop();
			LogPacketSent(packetSequence, sIPcosHeader, sIPCOSMessage2, sendStatus, timeToDeliver);
			if (IsMessageDelivered(sendStatus))
			{
				highestPrioritizedQueue.DeviceInformation.SequenceNumber++;
			}
		}
		else
		{
			CORESTACKMessage cORESTACKMessage = new CORESTACKMessage(packetSequence.Current.Header, new List<byte>(packetSequence.Current.Message));
			cORESTACKMessage.Mode = highestPrioritizedQueue.GetCurrentMessageMode();
			CORESTACKMessage message = cORESTACKMessage;
			sendStatus = icmpHandler.SendICMPMessage(message);
			Log.Debug(Module.DeviceManager, "SendScheduler", $"Send corestack message ---> Type: {packetSequence.Current.Header.CorestackFrameType} Sequence Type: {packetSequence.SequenceType} Status: {sendStatus}");
		}
		Monitor.Enter(SyncRoot);
		return sendStatus;
	}

	private void SyncSequenceNumber(SIPcosHeader header, IQueueItem highestPrioritizedQueue)
	{
		bool flag = false;
		byte[] destination = header.Destination;
		IDeviceInformation deviceInformation = deviceList[destination];
		DeviceInfoManufacturerCode manufacturerCode = (DeviceInfoManufacturerCode)deviceInformation.ManufacturerCode;
		if (manufacturerCode == DeviceInfoManufacturerCode.eQ3)
		{
			DeviceTypesEq3 manufacturerDeviceType = (DeviceTypesEq3)deviceInformation.ManufacturerDeviceType;
			if (manufacturerDeviceType == DeviceTypesEq3.Sir && header.FrameType == SIPcosFrameType.DIRECT_EXECUTION)
			{
				byte? sequenceNumber = transceiver.GetSequenceNumber(destination);
				if (sequenceNumber.HasValue)
				{
					header.SequenceNumber = (byte)(sequenceNumber.Value + 1);
					flag = true;
				}
			}
		}
		if (!flag)
		{
			header.SequenceNumber = highestPrioritizedQueue.DeviceInformation.SequenceNumber;
		}
	}

	public Guid Enqueue(PacketSequence sequence)
	{
		return Enqueue(sequence, null);
	}

	public Guid Enqueue(PacketSequence sequence, Guid? deviceId)
	{
		lock (SyncRoot)
		{
			EnqueueSequenceInternal(sequence, deviceId);
			UnblockSendLoopCanPerformNewOperation();
		}
		return sequence.CorrelationId;
	}

	public void Enqueue(IEnumerable<PacketSequence> sequences)
	{
		lock (SyncRoot)
		{
			foreach (PacketSequence sequence in sequences)
			{
				EnqueueSequenceInternal(sequence, null);
			}
			UnblockSendLoopCanPerformNewOperation();
		}
	}

	public void RemoveDeviceSpecificQueue(Guid deviceId)
	{
		List<PacketSequence> list = new List<PacketSequence>();
		lock (SyncRoot)
		{
			pendingApplicationAckPackets.RemoveAll((PacketSequence ps) => ps.Parent.DeviceInformation.DeviceId == deviceId);
			List<IQueueItem> list2 = deviceSpecificQueues.FindAll((IQueueItem item) => item.DeviceInformation.DeviceId == deviceId);
			foreach (IQueueItem item in list2)
			{
				List<PacketSequence> collection = item.Remove((PacketSequence ps) => true);
				RemoveQueueIfEmpty(item);
				list.AddRange(collection);
			}
		}
		foreach (PacketSequence item2 in list)
		{
			RaiseSequenceFinished(item2, SequenceState.Aborted);
		}
	}

	private bool RemoveSequencesInternal(Predicate<PacketSequence> predicate, SequenceState sequenceState)
	{
		bool result = true;
		List<PacketSequence> list = new List<PacketSequence>();
		lock (SyncRoot)
		{
			int count = deviceSpecificQueues.Count;
			for (int num = count; num > 0; num--)
			{
				IQueueItem queueItem = deviceSpecificQueues[num - 1];
				list.AddRange(queueItem.Remove((PacketSequence sequence) => predicate(sequence) && !pendingApplicationAckPackets.Contains(sequence)));
				RemoveQueueIfEmpty(queueItem);
			}
			for (int num2 = 0; num2 < pendingApplicationAckPackets.Count; num2++)
			{
				if (predicate(pendingApplicationAckPackets[num2]))
				{
					pendingApplicationAckPackets[num2].Lifetime = TimeSpan.Zero;
					result = false;
				}
			}
		}
		foreach (PacketSequence item in list)
		{
			if (sequenceState == SequenceState.Aborted)
			{
				sequenceState = (item.Started ? SequenceState.Dirty : SequenceState.Aborted);
			}
			RaiseSequenceFinished(item, sequenceState);
		}
		return result;
	}

	public void AcknowledgePacket(byte[] address, byte sequenceNumber, SIPcosAnswerFrameStatus status, bool isStatusInfo)
	{
		bool flag = false;
		PacketSequence packetSequence = null;
		bool flag2 = false;
		Log.Debug(Module.DeviceManager, "SendScheduler", $"Request Lock to acknowledge packet from {deviceList.LogInfoByAddress(address)}...");
		lock (SyncRoot)
		{
			Log.Debug(Module.DeviceManager, "SendScheduler", $"Lock to acknowledge packet from {deviceList.LogInfoByAddress(address)} gained.");
			int count = pendingApplicationAckPackets.Count;
			for (int i = 0; i < count; i++)
			{
				packetSequence = pendingApplicationAckPackets[i];
				Packet current = packetSequence.Current;
				if (IsAckForPacket(current, address, sequenceNumber, isStatusInfo))
				{
					flag2 = true;
					switch (status)
					{
					case SIPcosAnswerFrameStatus.ACK:
					case SIPcosAnswerFrameStatus.STATUSACK:
					case SIPcosAnswerFrameStatus.ACKAESPROBLEM:
						current.State = PacketSendState.Ack;
						break;
					case SIPcosAnswerFrameStatus.NAK:
					case SIPcosAnswerFrameStatus.NAKINHIBIT:
					case SIPcosAnswerFrameStatus.NAKPEERNOTCONFIGURED:
						current.State = PacketSendState.Nak;
						break;
					default:
						throw new ArgumentOutOfRangeException("status");
					}
					if (current.State == PacketSendState.Ack)
					{
						flag = !MoveToNextPacketInSequence(packetSequence);
						break;
					}
					packetSequence.ErrorCount = 4;
					CheckForExceededErrorCount(packetSequence, SendStatus.ACK);
					break;
				}
			}
			if (isStatusInfo)
			{
				if (flag2)
				{
					Log.Debug(Module.DeviceManager, "SendScheduler", $"StatusInfo packet from {deviceList.LogInfoByAddress(address)} with sequence number {sequenceNumber} found");
				}
			}
			else
			{
				Log.Debug(Module.DeviceManager, "SendScheduler", string.Format("Acknowledge packet from {0} with status {1} and sequence number {2}{3} found", deviceList.LogInfoByAddress(address), status, sequenceNumber, flag2 ? "" : " NOT"));
			}
			if (flag2)
			{
				RemovePendingAckPacket(packetSequence, AckResult.Ack);
				UnblockSendLoopCanPerformNewOperation();
			}
			Log.Debug(Module.DeviceManager, "SendScheduler", $"Lock to acknowledge packet from {deviceList.LogInfoByAddress(address)} released.");
		}
		if (flag)
		{
			RaiseSequenceFinished(packetSequence, SequenceState.Success);
		}
	}

	public void UpdateLastOnTimeOfDevice(byte[] address, AwakeModifier awakeModifier)
	{
		lock (SyncRoot)
		{
			if (deviceList.Contains(address))
			{
				IBasicDeviceInformation deviceInformation = deviceList[address];
				UpdateLastOnTimeTime(deviceInformation, awakeModifier);
			}
		}
	}

	public void UpdateLastOnTimeOfDevice(IDeviceInformation deviceInformation, AwakeModifier awakeModifier)
	{
		lock (SyncRoot)
		{
			if (deviceList.Contains(deviceInformation))
			{
				UpdateLastOnTimeTime(deviceInformation, awakeModifier);
			}
		}
	}

	private void UpdateLastOnTimeTime(IBasicDeviceInformation deviceInformation, AwakeModifier awakeModifier)
	{
		bool flag = deviceInformation.DeviceUnreachable || !deviceInformation.Awake();
		deviceInformation.UpdateAwakeState(awakeModifier);
		DeviceReachableChanged(deviceInformation, reachable: true);
		if (flag)
		{
			UnblockSendLoopCanPerformNewOperation();
		}
	}

	private void RemoveTimedOutAppAcks()
	{
		lock (SyncRoot)
		{
			Log.Debug(Module.DeviceManager, "SendScheduler", $"Checking for timed out app ACKs. There are currently {pendingApplicationAckPackets.Count} outstanding application ACKs.");
			int count = pendingApplicationAckPackets.Count;
			for (int num = count; num > 0; num--)
			{
				PacketSequence packetSequence = pendingApplicationAckPackets[num - 1];
				if (packetSequence.Parent.AckPendingUntil <= DateTime.UtcNow)
				{
					if (packetSequence.Current.State != PacketSendState.WaitingForAppAck)
					{
						Log.Warning(Module.DeviceManager, "SendScheduler", $"Found timed out packet with wrong LogicalState {packetSequence.Current.State}.");
					}
					packetSequence.ErrorCount++;
					RemovePendingAckPacket(packetSequence, AckResult.Timeout);
					packetSequence.Current.State = PacketSendState.Open;
					LogTimeoutError(packetSequence);
					CheckForExceededErrorCount(packetSequence, SendStatus.NO_REPLY);
				}
			}
		}
	}

	public void Suspend()
	{
		lock (SyncRoot)
		{
			suspendedWaitObject.Reset();
		}
	}

	public void Resume()
	{
		lock (SyncRoot)
		{
			suspendedWaitObject.Set();
		}
	}

	public bool ContainsSequences(Guid deviceId, Predicate<PacketSequence> predicate)
	{
		lock (SyncRoot)
		{
			List<IQueueItem> source = deviceSpecificQueues.FindAll((IQueueItem item) => item.DeviceInformation.DeviceId == deviceId);
			return source.Any((IQueueItem qi) => qi.Contains(predicate));
		}
	}

	public bool RemoveConfigurationSequences()
	{
		Predicate<PacketSequence> predicate = (PacketSequence sequence) => sequence.SequenceType == SequenceType.Configuration || sequence.SequenceType == SequenceType.Inclusion || sequence.SequenceType == SequenceType.Exclusion;
		bool flag = RemoveSequencesInternal(predicate, SequenceState.Aborted);
		Log.Debug(Module.DeviceManager, "SendScheduler", string.Format("Configuration {0}all sequences have been removed", flag ? "" : "NOT "));
		return flag;
	}

	public void RemoveSequencesConditionally(Guid deviceId, Predicate<PacketSequence> predicate, SequenceState sequenceState)
	{
		RemoveSequencesInternal((PacketSequence sequence) => sequence.Parent.DeviceInformation.DeviceId == deviceId && predicate(sequence), sequenceState);
	}

	private void DeviceReachableChanged(IBasicDeviceInformation deviceInformation, bool reachable)
	{
		EventHandler<DeviceReachableChangedEventArgs> reachableChanged = this.ReachableChanged;
		if (reachableChanged != null)
		{
			reachableChanged(this, new DeviceReachableChangedEventArgs(deviceInformation, reachable));
		}
		else
		{
			deviceInformation.DeviceUnreachable = !reachable;
		}
	}

	private void LogPacketSent(PacketSequence packetSequence, SIPcosHeader sipCosHeader, SIPCOSMessage message, SendStatus sendStatus, long timeToDeliver)
	{
		string message2 = sipCosHeader.FrameType switch
		{
			SIPcosFrameType.NETWORK_MANAGEMENT_FRAME => $"Send message --->  Type: {sipCosHeader.FrameType} Subtype: {(SIPcosNetworkManagementFrameType)packetSequence.Current.Message[0]} Source: {sipCosHeader.MacSource.ToReadable()} Destination: {sipCosHeader.Destination.ToReadable()} sequence number {sipCosHeader.SequenceNumber} Type: {packetSequence.SequenceType} Mode: {message.Mode} Stay Awake: {sipCosHeader.StayAwake} Time to deliver: {timeToDeliver} Result: {sendStatus}", 
			SIPcosFrameType.CONFIGURATION => $"Send message --->  Type: {sipCosHeader.FrameType} Subtype: {(SipCosConfigurationCommands)packetSequence.Current.Message[1]} Source: {sipCosHeader.MacSource.ToReadable()} Destination: {sipCosHeader.Destination.ToReadable()} Channel: {packetSequence.Current.Message[0]} sequence number {sipCosHeader.SequenceNumber} Type: {packetSequence.SequenceType} Mode: {message.Mode} Stay Awake: {sipCosHeader.StayAwake} Time to deliver: {timeToDeliver} Result: {sendStatus}", 
			_ => $"Send message --->  Type: {sipCosHeader.FrameType} Source: {sipCosHeader.MacSource.ToReadable()} Destination: {deviceList.LogInfoByAddress(sipCosHeader.Destination)} sequence number {sipCosHeader.SequenceNumber} Type: {packetSequence.SequenceType} Mode: {message.Mode} Stay Awake: {sipCosHeader.StayAwake}  Time to deliver: {timeToDeliver} Result: {sendStatus}", 
		};
		if (IsMessageDelivered(sendStatus))
		{
			Log.Debug(Module.DeviceManager, "SendScheduler", message2);
		}
		else
		{
			Log.Error(Module.DeviceManager, "SendScheduler", message2);
		}
	}

	private void EnqueueSequenceInternal(PacketSequence sequence, Guid? deviceId)
	{
		QueueType queueType = ((sequence.SequenceType != SequenceType.Icmp) ? QueueType.Application : QueueType.Icmp);
		IQueueItem queueItem = ((!deviceId.HasValue) ? deviceSpecificQueues.FirstOrDefault((IQueueItem specificQueue) => specificQueue.DeviceInformation.Address.Compare(sequence.Address) && specificQueue.QueueType == queueType) : deviceSpecificQueues.FirstOrDefault((IQueueItem specificQueue) => specificQueue.DeviceInformation.DeviceId == deviceId.Value && specificQueue.QueueType == queueType));
		if (queueItem == null)
		{
			IDeviceInformation deviceInformation = (deviceId.HasValue ? deviceList[deviceId.Value] : deviceList[sequence.Address]);
			IBasicDeviceInformation deviceInformation2;
			if (deviceInformation != null)
			{
				deviceInformation2 = deviceInformation;
			}
			else
			{
				if (!MulticastDeviceInfos.IsValidMulticastAddress(sequence.Address))
				{
					try
					{
						throw new ArgumentException("sequence");
					}
					catch (Exception arg)
					{
						Log.Error(Module.DeviceManager, $"Trying to send message to invalid address {sequence.Address.ToReadable()}: {arg}");
						return;
					}
				}
				deviceInformation2 = MulticastDeviceInfos.Create(sequence.Address);
			}
			queueItem = new DeviceSpecificQueue(deviceInformation2, queueType, stopwatch);
			deviceSpecificQueues.Add(queueItem);
		}
		SendPriority sendPriority = Defaults.GetSendPriority(queueItem.DeviceInformation, sequence);
		queueItem.Enqueue(sequence, sendPriority);
		Log.Debug(Module.DeviceManager, "SendScheduler", $"New Packet {queueType} with priority [Awake={sendPriority.Awake} | Sleeping={sendPriority.Sleeping}] for device {deviceList.LogInfoByAddress(sequence.Address)} enqueued. Now there are {sequence.Parent.Count} remaining sequences.");
	}

	public void Start()
	{
		if (!isRunning)
		{
			isRunning = true;
			overallWaitObject.Reset();
			workerThread.Start();
		}
	}

	public void Stop()
	{
		if (isRunning)
		{
			isRunning = false;
			overallWaitObject.Set();
			suspendedWaitObject.Set();
			workerThread.Join();
			pendingApplicationAckPackets.WaitUntilEmpty(RemoveTimedOutAppAcks);
		}
	}

	public void ForceEchoRequestForUnreachableDevices()
	{
		DateTime suspendUntil = DateTime.UtcNow.Add(Defaults.WaitForRouterInit);
		foreach (IQueueItem deviceSpecificQueue in deviceSpecificQueues)
		{
			if (deviceSpecificQueue.QueueType == QueueType.Icmp && deviceSpecificQueue.Contains((PacketSequence sequence) => sequence.Current != null && sequence.Current.Message != null && sequence.SequenceType == SequenceType.Icmp && sequence.Current.Message.Length >= 3 && sequence.Current.Message[2] == 129))
			{
				deviceSpecificQueue.SuspendUntil = suspendUntil;
			}
		}
	}

	private void CheckForExceededErrorCount(PacketSequence packetSequence, SendStatus sendStatus)
	{
		if (packetSequence == null)
		{
			return;
		}
		IBasicDeviceInformation basicDeviceInformation = packetSequence.Parent.DeviceInformation;
		if (sendStatus == SendStatus.NO_REPLY && basicDeviceInformation.BestOperationMode == DeviceInfoOperationModes.EventListener && basicDeviceInformation.BestOperationMode == DeviceInfoOperationModes.EventListener)
		{
			MarkDeviceAsSleeping(packetSequence.Parent);
		}
		IDeviceInformation deviceInformation = basicDeviceInformation as IDeviceInformation;
		if (sendStatus == SendStatus.NO_REPLY && packetSequence.SequenceType == SequenceType.Icmp && deviceInformation != null && deviceInformation.ManufacturerDeviceType == 10 && !deviceInformation.DeviceUnreachable)
		{
			DeviceReachableChanged(basicDeviceInformation, reachable: false);
		}
		if (packetSequence.SequenceType == SequenceType.Icmp || packetSequence.ErrorCount < 3)
		{
			return;
		}
		if (packetSequence.Current != null && IsForwardedNetworkAcceptSequence(packetSequence.Current, out var routedDeviceAddress) && packetSequence.SendStatus == SendStatus.ACK && routedDeviceAddress != null)
		{
			IDeviceInformation deviceInformation2 = deviceList[routedDeviceAddress];
			if (deviceInformation2 != null)
			{
				basicDeviceInformation = deviceInformation2;
			}
		}
		if (sendStatus == SendStatus.NO_REPLY && basicDeviceInformation.BestOperationMode != DeviceInfoOperationModes.EventListener)
		{
			DeviceReachableChanged(basicDeviceInformation, reachable: false);
		}
		if (basicDeviceInformation.BestOperationMode != DeviceInfoOperationModes.MainsPowered)
		{
			MarkDeviceAsSleeping(packetSequence.Parent);
		}
		packetSequence.Reset(resetSendState: true);
	}

	private void MarkSequenceAsWaitingForAppAck(PacketSequence packetSequence)
	{
		packetSequence.Current.State = PacketSendState.WaitingForAppAck;
		packetSequence.Parent.AckPendingUntil = GetAckPendingTime();
		pendingApplicationAckPackets.Add(packetSequence);
	}

	private DateTime GetAckPendingTime()
	{
		if (!deviceList.ContainsRouter)
		{
			return DateTime.UtcNow.Add(Defaults.WaitForAppAck);
		}
		return DateTime.UtcNow.Add(waitForAppAckRouterPresent);
	}

	private void RaiseSequenceFinished(PacketSequence packetSequence, SequenceState state)
	{
		Log.Debug(Module.DeviceManager, "SendScheduler", $"Sequence for device {packetSequence.Parent.DeviceInformation} finished with LogicalState {state}");
		if (this.SequenceFinished != null)
		{
			EventHandler<SequenceFinishedEventArgs> sequenceFinished = this.SequenceFinished;
			sequenceFinished(this, new SequenceFinishedEventArgs(packetSequence.CorrelationId, state));
		}
		UnblockSendLoopCanPerformNewOperation();
	}

	private IQueueItem GetHighestPrioritizedQueue(out TimeSpan wait)
	{
		IQueueItem queueItem = null;
		wait = MaximumLoopWaitTimeSpan;
		Log.Debug(Module.DeviceManager, "SendScheduler", $"Choose highest prioritized queue out of {deviceSpecificQueues.Count}.");
		DateTime utcNow = DateTime.UtcNow;
		int num = 1;
		foreach (IQueueItem deviceSpecificQueue in deviceSpecificQueues)
		{
			TimeSpan timeSpan = deviceSpecificQueue.SuspendUntil.Subtract(utcNow);
			if (timeSpan <= TimeSpan.Zero)
			{
				timeSpan = deviceSpecificQueue.AckPendingUntil.Subtract(utcNow);
				if (timeSpan < TimeSpan.Zero)
				{
					if (ExecuteQueueGuard(deviceSpecificQueue, num) && IsAllowedToBeExecuted(deviceSpecificQueue))
					{
						Log.Debug(Module.DeviceManager, "SendScheduler", $"Device {deviceList.LogInfoByAddress(deviceSpecificQueue.DeviceInformation.Address)} is a candidate. Awake {deviceSpecificQueue.DeviceInformation.Awake()}. Suspended until {deviceSpecificQueue.SuspendUntil.Ticks}. Timestamp {DateTime.UtcNow.Ticks}, Queue number {num}. Queue Type {deviceSpecificQueue.QueueType}.");
						if (queueItem == null)
						{
							queueItem = deviceSpecificQueue;
						}
						else if (queueItem.Priority < deviceSpecificQueue.Priority)
						{
							queueItem = deviceSpecificQueue;
						}
						else if (queueItem.Priority == deviceSpecificQueue.Priority && queueItem.RoundCounter > deviceSpecificQueue.RoundCounter)
						{
							queueItem = deviceSpecificQueue;
						}
						wait = TimeSpan.Zero;
					}
				}
				else
				{
					wait = wait.Min(timeSpan);
					Log.Debug(Module.DeviceManager, "SendScheduler", $"Device {deviceList.LogInfoByAddress(deviceSpecificQueue.DeviceInformation.Address)} is waiting for application ack. Queue number {num}. Queue type {deviceSpecificQueue.QueueType}. Now {utcNow}. Ack pending until {deviceSpecificQueue.AckPendingUntil}. Suspend until {deviceSpecificQueue.SuspendUntil}");
				}
			}
			else
			{
				wait = wait.Min(timeSpan);
				Log.Debug(Module.DeviceManager, "SendScheduler", $"Device {deviceList.LogInfoByAddress(deviceSpecificQueue.DeviceInformation.Address)} suspended until {deviceSpecificQueue.SuspendUntil}. Queue number {num}. Queue type {deviceSpecificQueue.QueueType}.");
			}
			num++;
		}
		return queueItem;
	}

	private bool IsAllowedToBeExecuted(IQueueItem queue)
	{
		bool result = true;
		if ((queue.DeviceInformation.BestOperationMode == DeviceInfoOperationModes.TripleBurstListener || queue.DeviceInformation.BestOperationMode == DeviceInfoOperationModes.BurstListener) && !queue.DeviceInformation.Awake())
		{
			result = pendingApplicationAckPackets.Count == 0;
		}
		return result;
	}

	private bool IsUnreachableBidcosStatusInfo(IQueueItem queue)
	{
		if (queue.DeviceInformation.DeviceUnreachable && queue.DeviceInformation.ProtocolType == ProtocolType.BidCos && queue.QueueType == QueueType.Application)
		{
			IDeviceInformation deviceInformation = deviceList[queue.DeviceInformation.DeviceId];
			bool flag = deviceInformation != null && deviceInformation.ManufacturerCode == 1 && deviceInformation.ManufacturerDeviceType == 22;
			if (flag)
			{
				RescheduleStatusFrameWithHigherPriority(queue);
			}
			PacketSequence packetSequence = queue.Peek();
			if (packetSequence.SequenceType != SequenceType.StatusRequest && packetSequence.SequenceType != SequenceType.Other)
			{
				if (flag)
				{
					return packetSequence.SequenceType == SequenceType.DirectExecution;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private void RescheduleStatusFrameWithHigherPriority(IQueueItem queueItem)
	{
		if (queueItem.Contains((PacketSequence m) => m.SequenceType == SequenceType.StatusRequest))
		{
			List<PacketSequence> list = queueItem.Remove((PacketSequence m) => m.SequenceType == SequenceType.StatusRequest);
			list.ForEach(delegate(PacketSequence m)
			{
				queueItem.Enqueue(m, new SendPriority(999, 999));
			});
		}
	}

	private bool ExecuteQueueGuard(IQueueItem queue, int number)
	{
		bool flag = false;
		PacketSequence packetSequence = ((queue.Count > 0) ? queue.Peek() : null);
		DeviceInclusionState deviceInclusionState = queue.DeviceInformation.DeviceInclusionState;
		if (packetSequence != null && packetSequence.Current != null && IsForwardedNetworkAcceptSequence(packetSequence.Current, out var routedDeviceAddress))
		{
			IDeviceInformation deviceInformation = deviceList[routedDeviceAddress];
			if (deviceInformation != null && ((deviceInformation.BestOperationMode != DeviceInfoOperationModes.EventListener && deviceInformation.DeviceUnreachable) || (deviceInformation.BestOperationMode != DeviceInfoOperationModes.MainsPowered && deviceInformation.IsDeviceSleeping())))
			{
				flag = true;
			}
		}
		if ((!queue.DeviceInformation.DeviceUnreachable && !flag) || queue.QueueType == QueueType.Icmp || IsUnreachableBidcosStatusInfo(queue))
		{
			if (queue.Priority > 0)
			{
				if (queue.QueueType == QueueType.Icmp)
				{
					return true;
				}
				if (deviceInclusionState == DeviceInclusionState.Included)
				{
					return true;
				}
				if (packetSequence == null)
				{
					Log.Error(Module.DeviceManager, "SendScheduler", $"Queue number {number} with type {queue.QueueType} for device {deviceList.LogInfoByAddress(queue.DeviceInformation.Address)} arrived with empty packet.");
					return false;
				}
				SequenceType sequenceType = packetSequence.SequenceType;
				if ((sequenceType == SequenceType.Inclusion && deviceInclusionState == DeviceInclusionState.InclusionPending) || (sequenceType == SequenceType.Exclusion && deviceInclusionState == DeviceInclusionState.ExclusionPending) || (sequenceType == SequenceType.Exclusion && deviceInclusionState == DeviceInclusionState.Excluded) || sequenceType == SequenceType.Ack || (sequenceType == SequenceType.CollisionNotification && (deviceInclusionState == DeviceInclusionState.FactoryResetWithAddressCollision || deviceInclusionState == DeviceInclusionState.FoundWithAddressCollision)))
				{
					return true;
				}
				Log.Debug(Module.DeviceManager, "SendScheduler", $"Device {deviceList.LogInfoByAddress(queue.DeviceInformation.Address)} not taken. Inclusion LogicalState {deviceInclusionState}. Sequence Type {sequenceType}.");
			}
			else if (queue.Priority == 0)
			{
				Log.Debug(Module.DeviceManager, "SendScheduler", $"Device {deviceList.LogInfoByAddress(queue.DeviceInformation.Address)} is sleeping. Awake {queue.DeviceInformation.Awake()}. Queue number {number}. Queue type {queue.QueueType}.");
			}
			else
			{
				Log.Debug(Module.DeviceManager, "SendScheduler", $"Device {deviceList.LogInfoByAddress(queue.DeviceInformation.Address)} has an empty queue. Queue number {number}. Queue type {queue.QueueType}.");
			}
		}
		else
		{
			Log.Debug(Module.DeviceManager, "SendScheduler", $"Device {deviceList.LogInfoByAddress(queue.DeviceInformation.Address)} is not reachable. Queue number {number}. Queue type {queue.QueueType}.");
		}
		return false;
	}

	private bool MoveToNextPacketInSequence(PacketSequence currentSequence)
	{
		Log.Debug(Module.DeviceManager, "SendScheduler", $"Moved to next packet in current sequence for device {deviceList.LogInfoByAddress(currentSequence.Address)}.");
		if (!currentSequence.MoveNext())
		{
			IQueueItem queueItem = currentSequence.Parent as DeviceSpecificQueue;
			if (queueItem != null)
			{
				queueItem.Remove(currentSequence);
				RemoveQueueIfEmpty(queueItem);
			}
			return false;
		}
		return true;
	}

	private void RemoveQueueIfEmpty(IQueueItem deviceSpecificQueue)
	{
		if (deviceSpecificQueue != null && deviceSpecificQueue.Count == 0 && !deviceSpecificQueue.DeviceInformation.Address.Compare(SipCosAddress.AllDevices))
		{
			deviceSpecificQueues.Remove(deviceSpecificQueue);
			Log.Debug(Module.DeviceManager, "SendScheduler", $"Queue for device {deviceList.LogInfoByAddress(deviceSpecificQueue.DeviceInformation.Address)} removed. {deviceSpecificQueue.Count} more to go.");
		}
	}

	private void MarkDeviceAsSleeping(IQueueItem queue)
	{
		PacketSequence packetSequence = ((queue.Count > 0) ? queue.Peek() : null);
		byte[] routedDeviceAddress;
		IBasicDeviceInformation basicDeviceInformation = ((packetSequence == null || packetSequence.Current == null || !IsForwardedNetworkAcceptSequence(packetSequence.Current, out routedDeviceAddress)) ? queue.DeviceInformation : (deviceList[routedDeviceAddress] ?? queue.DeviceInformation));
		Log.Information(Module.DeviceManager, "SendScheduler", $"Awake LogicalState of device {deviceList.LogInfoByGuid(basicDeviceInformation.DeviceId)} changed to false.");
		basicDeviceInformation.MarkDeviceAsSleeping();
	}

	private void LogTimeoutError(PacketSequence sequence)
	{
		string text;
		string text2;
		if (sequence.Current.Header is SIPcosHeader sIPcosHeader)
		{
			text = sIPcosHeader.FrameType.ToString();
			text2 = sIPcosHeader.FrameType switch
			{
				SIPcosFrameType.NETWORK_MANAGEMENT_FRAME => "subtype: " + (SIPcosNetworkManagementFrameType)sequence.Current.Message[0], 
				SIPcosFrameType.CONFIGURATION => "subtype: " + (SipCosConfigurationCommands)sequence.Current.Message[1], 
				_ => "sequence type: " + sequence.SequenceType, 
			};
		}
		else
		{
			text = "CORESTACK";
			text2 = sequence.Current.Header.CorestackFrameType.ToString();
		}
		Log.Warning(Module.DeviceManager, "SendScheduler", $"Timed out app ack detected for packet of type: {text}, {text2} for device {deviceList.LogInfoByAddress(sequence.Current.Header.MacDestination)}, Pending until: {sequence.Parent.AckPendingUntil.Ticks}, Timestamp: {DateTime.UtcNow.Ticks}, Error count: {sequence.ErrorCount}.");
	}

	private void RemovePendingAckPacket(PacketSequence sequence, AckResult ackResult)
	{
		rfStatistics.AddAckResult(ackResult);
		pendingApplicationAckPackets.Remove(sequence);
		sequence.Parent.AckPendingUntil = DateTime.MinValue;
		Log.Debug(Module.DeviceManager, "SendScheduler", $"Removed sequence for device {deviceList.LogInfoByAddress(sequence.Address)} from the list of pending application ACKs. The list now has {pendingApplicationAckPackets.Count} entries.");
	}

	private void UnblockSendLoopCanPerformNewOperation()
	{
		loopWaitObject.Set();
		Log.Debug(Module.DeviceManager, "SendScheduler", "Unblocked send loop");
	}

	private bool IsMessageDelivered(SendStatus sendStatus)
	{
		if (sendStatus != SendStatus.ACK)
		{
			return sendStatus == SendStatus.MULTI_CAST;
		}
		return true;
	}

	private bool IsAckForPacket(Packet packet, byte[] ackRecAddress, byte sequenceNumber, bool isStatusInfo)
	{
		if (!(packet.Header is SIPcosHeader sIPcosHeader))
		{
			return false;
		}
		if (IsForwardedNetworkAcceptSequence(packet, out var routedDeviceAddress))
		{
			if (routedDeviceAddress != null && routedDeviceAddress.SequenceEqual(ackRecAddress) && sIPcosHeader.SequenceNumber == sequenceNumber)
			{
				return true;
			}
			return false;
		}
		if (sIPcosHeader.Destination.Compare(ackRecAddress) && sIPcosHeader.SequenceNumber == sequenceNumber && (sIPcosHeader.FrameType != SIPcosFrameType.STATUSINFO || isStatusInfo))
		{
			return true;
		}
		return false;
	}

	private bool IsForwardedNetworkManagementFrame(SIPCOSMessage message)
	{
		if (message.Header != null && message.Header.FrameType == SIPcosFrameType.NETWORK_MANAGEMENT_FRAME && message.Data != null && message.Data.Count > 0 && (message.Data[0] == 5 || message.Data[0] == 3))
		{
			return true;
		}
		return false;
	}

	private bool IsForwardedNetworkAcceptSequence(Packet packet, out byte[] routedDeviceAddress)
	{
		SIPcosHeader sIPcosHeader = packet.Header as SIPcosHeader;
		byte[] message = packet.Message;
		if (sIPcosHeader != null && message != null && sIPcosHeader.FrameType == SIPcosFrameType.NETWORK_MANAGEMENT_FRAME && message.Length >= 43 && message[0] == 5)
		{
			routedDeviceAddress = new byte[3]
			{
				message[40],
				message[41],
				message[42]
			};
			return true;
		}
		routedDeviceAddress = null;
		return false;
	}
}
