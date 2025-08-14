using System;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SerialAPI;
using SerialAPI.BidCosLayer.CommandFrames;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.DeviceHandler;

internal class DeviceController : IDeviceController
{
	private readonly ISendScheduler transceiver;

	private readonly IBasicDeviceInformation deviceInfo;

	private readonly SIPCosStatusInfoCommandHandler statusInfoHandler;

	public DeviceController(ISendScheduler transceiver, IBasicDeviceInformation deviceInfo, SIPCosStatusInfoCommandHandler statusInfoHandler)
	{
		this.transceiver = transceiver;
		this.deviceInfo = deviceInfo;
		this.statusInfoHandler = statusInfoHandler;
	}

	public Guid ChangeDeviceState(RampMode rampMode, int rampTime, byte state, byte targetChannel, int offTimer)
	{
		transceiver.RemoveSequencesConditionally(deviceInfo.DeviceId, (PacketSequence s) => ShouldRemove(s, SequenceType.DirectExecution, targetChannel), SequenceState.Aborted);
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Destination = deviceInfo.Address;
		sIPcosHeader.BiDi = true;
		sIPcosHeader.FrameType = SIPcosFrameType.DIRECT_EXECUTION;
		SIPcosHeader header = sIPcosHeader;
		DirectExecutionCommand directExecutionCommand = new DirectExecutionCommand();
		directExecutionCommand.ActionCode = ((rampMode == RampMode.RampStart) ? ActionCode.StartRamp : ActionCode.StopRamp);
		directExecutionCommand.Channel = targetChannel;
		directExecutionCommand.ActionData = new ActionDataRampStart
		{
			RampTime = TimeConverter.ConvertMsToUshort(rampTime * 100),
			SwitchLevel = state,
			OnTime = TimeConverter.ConvertMsToUshort(offTimer * 100)
		}.ToArray();
		DirectExecutionCommand directExecutionCommand2 = directExecutionCommand;
		transceiver.RemoveSequencesConditionally(deviceInfo.DeviceId, (PacketSequence s) => ShouldRemove(s, SequenceType.DirectExecution, targetChannel), SequenceState.Aborted);
		PacketSequence sequence = new PacketSequence(new Packet(header, directExecutionCommand2.ToArray()), SequenceType.DirectExecution);
		return transceiver.Enqueue(sequence);
	}

	public Guid SendSwitchCommand(ActivationTime activationTime, byte[] sourceAddress, byte channel, byte keyStrokeCounter, byte? decisionValue)
	{
		transceiver.RemoveSequencesConditionally(deviceInfo.DeviceId, (PacketSequence s) => ShouldRemove(s, SequenceType.DirectExecution, channel), SequenceState.Aborted);
		bool flag = deviceInfo.ProtocolType == ProtocolType.BidCos;
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Source = sourceAddress;
		sIPcosHeader.Destination = deviceInfo.Address;
		sIPcosHeader.BiDi = !flag;
		SIPcosHeader sIPcosHeader2 = sIPcosHeader;
		byte[] message;
		if (((int?)decisionValue).HasValue)
		{
			ConditionalSwitchCommand conditionalSwitchCommand = new ConditionalSwitchCommand();
			conditionalSwitchCommand.ActivationTime = activationTime == ActivationTime.LongPress;
			conditionalSwitchCommand.KeyChannelNumber = channel;
			conditionalSwitchCommand.KeyStrokeCounter = keyStrokeCounter;
			conditionalSwitchCommand.DecisionValue = decisionValue.Value;
			message = conditionalSwitchCommand.ToArray();
			sIPcosHeader2.FrameType = SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND;
		}
		else
		{
			SwitchCommand switchCommand = new SwitchCommand();
			switchCommand.ActivationTime = activationTime == ActivationTime.LongPress;
			switchCommand.KeyStrokeCounter = keyStrokeCounter;
			switchCommand.KeyChannelNumber = channel;
			message = switchCommand.ToArray();
			sIPcosHeader2.FrameType = SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND;
		}
		PacketSequence packetSequence = new PacketSequence(new Packet(sIPcosHeader2, message));
		if (flag)
		{
			SIPcosHeader sIPcosHeader3 = new SIPcosHeader();
			sIPcosHeader3.Source = sourceAddress;
			sIPcosHeader3.Destination = deviceInfo.Address;
			sIPcosHeader3.BiDi = true;
			SIPcosHeader header = sIPcosHeader3;
			SIPCOSMessage packet = statusInfoHandler.GenerateRequestStatusInfo(header, channel);
			packetSequence.Add(packet);
		}
		return transceiver.Enqueue(packetSequence);
	}

	public Guid RequestStatusInfo(byte targetChannel)
	{
		transceiver.RemoveSequencesConditionally(deviceInfo.DeviceId, (PacketSequence s) => ShouldRemove(s, SequenceType.StatusRequest, targetChannel), SequenceState.Aborted);
		SIPCOSMessage message = statusInfoHandler.GenerateRequestStatusInfo(new SIPcosHeader
		{
			Destination = deviceInfo.Address
		}, targetChannel);
		return transceiver.Enqueue(new PacketSequence(new Packet(message), SequenceType.StatusRequest));
	}

	public Guid SendTestSound(byte[] sourceAddress, byte channel, byte soundId, byte currentSoundId, int delayMs)
	{
		transceiver.RemoveSequencesConditionally(deviceInfo.DeviceId, (PacketSequence s) => ShouldRemove(s, SequenceType.Configuration, channel), SequenceState.Aborted);
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Source = sourceAddress;
		sIPcosHeader.Destination = deviceInfo.Address;
		sIPcosHeader.BiDi = true;
		sIPcosHeader.FrameType = SIPcosFrameType.VIRTUAL_BIDCOS_COMMAND;
		SIPcosHeader header = sIPcosHeader;
		PacketSequence packetSequence = new PacketSequence();
		packetSequence.Add(new Packet(header, new VirtualTestSoundCommand(channel, deviceInfo.Address)
		{
			CurrentSoundId = currentSoundId,
			DelayMs = delayMs,
			SoundId = soundId
		}.ToArray()));
		SIPcosHeader sIPcosHeader2 = new SIPcosHeader();
		sIPcosHeader2.Source = sourceAddress;
		sIPcosHeader2.Destination = deviceInfo.Address;
		sIPcosHeader2.BiDi = true;
		SIPcosHeader header2 = sIPcosHeader2;
		SIPCOSMessage packet = statusInfoHandler.GenerateRequestStatusInfo(header2, channel);
		packetSequence.Add(packet);
		return transceiver.Enqueue(packetSequence);
	}

	public Guid SendVirtualConfigCommand(byte[] sourceAddress, byte channel, byte[] parameters)
	{
		transceiver.RemoveSequencesConditionally(deviceInfo.DeviceId, (PacketSequence s) => ShouldRemove(s, SequenceType.Configuration, channel), SequenceState.Aborted);
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Source = sourceAddress;
		sIPcosHeader.Destination = deviceInfo.Address;
		sIPcosHeader.BiDi = true;
		sIPcosHeader.FrameType = SIPcosFrameType.VIRTUAL_BIDCOS_COMMAND;
		SIPcosHeader header = sIPcosHeader;
		PacketSequence packetSequence = new PacketSequence();
		packetSequence.Add(new Packet(header, new VirtualConfigParamsCommand(channel, deviceInfo.Address)
		{
			Channel = channel,
			Parameters = parameters
		}.ToArray()));
		SIPcosHeader sIPcosHeader2 = new SIPcosHeader();
		sIPcosHeader2.Source = sourceAddress;
		sIPcosHeader2.Destination = deviceInfo.Address;
		sIPcosHeader2.BiDi = true;
		SIPcosHeader header2 = sIPcosHeader2;
		SIPCOSMessage packet = statusInfoHandler.GenerateRequestStatusInfo(header2, channel);
		packetSequence.Add(packet);
		return transceiver.Enqueue(packetSequence);
	}

	private static bool ShouldRemove(PacketSequence sequence, SequenceType sequenceType, byte targetChannel)
	{
		if (sequence.SequenceType != sequenceType)
		{
			return false;
		}
		byte[] message = sequence.Current.Message;
		if (message != null && message.Length >= 2)
		{
			return message[1] == targetChannel;
		}
		return false;
	}
}
