using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.DeviceHandler;

internal class MulticastController : IMulticastController
{
	private readonly IDeviceList deviceList;

	private readonly Dictionary<Guid, MulticastSwitchParameters> pendingMultiCasts = new Dictionary<Guid, MulticastSwitchParameters>();

	private readonly object pendingMulticastLock = new object();

	private readonly CommunicationWrapper communicationWrapper;

	public MulticastController(CommunicationWrapper communicationWrapper)
	{
		this.communicationWrapper = communicationWrapper;
		deviceList = communicationWrapper.DeviceList;
		communicationWrapper.SendScheduler.SequenceFinished += SendSchedulerSequenceFinished;
	}

	private void SendSchedulerSequenceFinished(object sender, SequenceFinishedEventArgs e)
	{
		MulticastSwitchParameters value;
		lock (pendingMulticastLock)
		{
			pendingMultiCasts.TryGetValue(e.CorrelationId, out value);
		}
		if (value != null)
		{
			SendUnicasts(value);
			lock (pendingMulticastLock)
			{
				pendingMultiCasts.Remove(e.CorrelationId);
			}
		}
	}

	public Guid SendMultiCastUnconditionalSwitch(IEnumerable<Guid> targetDeviceIds, byte[] sourceIp, byte sourceChannel, bool longPress, byte keystrokeCounter)
	{
		SwitchCommand switchCommand = new SwitchCommand();
		switchCommand.ActivationTime = longPress;
		switchCommand.KeyStrokeCounter = keystrokeCounter;
		switchCommand.KeyChannelNumber = sourceChannel;
		SwitchCommand switchCommand2 = switchCommand;
		return SendMulticastSwitch(targetDeviceIds, SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND, switchCommand2.ToArray(), sourceIp);
	}

	public Guid SendMultiCastConditionalSwitch(IEnumerable<Guid> targetDeviceIds, byte[] sourceIp, byte sourceChannel, bool longPress, byte keystrokeCounter, byte decisionValue)
	{
		ConditionalSwitchCommand conditionalSwitchCommand = new ConditionalSwitchCommand();
		conditionalSwitchCommand.ActivationTime = longPress;
		conditionalSwitchCommand.KeyStrokeCounter = keystrokeCounter;
		conditionalSwitchCommand.KeyChannelNumber = sourceChannel;
		conditionalSwitchCommand.DecisionValue = decisionValue;
		ConditionalSwitchCommand conditionalSwitchCommand2 = conditionalSwitchCommand;
		return SendMulticastSwitch(targetDeviceIds, SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND, conditionalSwitchCommand2.ToArray(), sourceIp);
	}

	private Guid SendMulticastSwitch(IEnumerable<Guid> targetDeviceIds, SIPcosFrameType frameType, byte[] commandBuffer, byte[] sourceIp)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.BiDi = false;
		sIPcosHeader.FrameType = frameType;
		sIPcosHeader.Destination = SipCosAddress.AllDevices;
		sIPcosHeader.MacDestination = SipCosAddress.AllDevices;
		sIPcosHeader.Source = sourceIp;
		sIPcosHeader.MacSource = sourceIp;
		SIPcosHeader header = sIPcosHeader;
		PacketSequence packetSequence = new PacketSequence(new Packet(header, commandBuffer));
		Guid correlationId = packetSequence.CorrelationId;
		lock (pendingMulticastLock)
		{
			pendingMultiCasts.Add(correlationId, new MulticastSwitchParameters(targetDeviceIds, frameType, commandBuffer, sourceIp));
		}
		communicationWrapper.SendScheduler.Enqueue(packetSequence);
		return correlationId;
	}

	private void SendUnicasts(MulticastSwitchParameters multicastSwitchParameters)
	{
		List<byte[]> list = new List<byte[]>();
		lock (deviceList)
		{
			list.AddRange(from deviceId in multicastSwitchParameters.TargetDeviceIds
				select deviceList[deviceId] into deviceInformation
				where deviceInformation != null
				select deviceInformation.Address);
		}
		foreach (byte[] item in list)
		{
			SIPcosHeader sIPcosHeader = new SIPcosHeader();
			sIPcosHeader.BiDi = true;
			sIPcosHeader.FrameType = multicastSwitchParameters.FrameType;
			sIPcosHeader.Destination = item;
			sIPcosHeader.MacDestination = item;
			sIPcosHeader.Source = multicastSwitchParameters.SourceAddress;
			sIPcosHeader.MacSource = multicastSwitchParameters.SourceAddress;
			SIPcosHeader header = sIPcosHeader;
			communicationWrapper.SendScheduler.Enqueue(new PacketSequence(new Packet(header, multicastSwitchParameters.CommandBuffer)));
		}
	}
}
