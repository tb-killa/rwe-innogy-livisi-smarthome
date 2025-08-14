using System;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI.BidCosLayer.CommandFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class AdapterFrameHandlerVirtual : AdapterFrameHandler<SirAdapter>
{
	private const byte SoundIndexId = 171;

	public AdapterFrameHandlerVirtual(SirAdapter adapter)
		: base(adapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.header.FrameType == SIPcosFrameType.VIRTUAL_BIDCOS_COMMAND;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		return (VirtualCommandType)message.message[0] switch
		{
			VirtualCommandType.TestSound => SendTestSound(new VirtualTestSoundCommand(message.message), message.mode), 
			VirtualCommandType.ConfigParameter => SendConfig(new VirtualConfigParamsCommand(message.message), message.mode), 
			_ => throw new ArgumentException($"Unknown bidcos frame type (type:{message.message[0]})"), 
		};
	}

	private SendStatus SendTestSound(VirtualTestSoundCommand command, SendMode mode)
	{
		if (base.DeviceAdapter.CurrentKey() == null && !base.DeviceAdapter.EnsureCurrentNodeDefaultKey())
		{
			Log.Error(Module.SerialCommunication, "Cannot retrieve device key from BE");
			return SendStatus.MODE_ERROR;
		}
		SendStatus sendStatus = ConfigParameters(command.DestinationAddress, command.Channel, new byte[2] { 171, command.SoundId }, mode);
		if (sendStatus == SendStatus.ACK)
		{
			base.DeviceAdapter.sirComm.SendRampStart(command.DestinationAddress, command.Channel, 100.0, command.DelayMs);
			sendStatus = ConfigParameters(command.DestinationAddress, command.Channel, new byte[2] { 171, command.CurrentSoundId }, mode);
		}
		return sendStatus;
	}

	private SendStatus SendConfig(VirtualConfigParamsCommand command, SendMode mode)
	{
		if (base.DeviceAdapter.CurrentKey() == null && !base.DeviceAdapter.EnsureCurrentNodeDefaultKey())
		{
			Log.Error(Module.SerialCommunication, "Cannot retrieve device key from BE");
			return SendStatus.MODE_ERROR;
		}
		return ConfigParameters(command.DestinationAddress, command.Channel, command.Parameters, mode);
	}

	private SendStatus ConfigParameters(byte[] destination, byte channel, byte[] parameters, SendMode mode)
	{
		base.DeviceAdapter.answerFrameIgnore = 3;
		return (!base.DeviceAdapter.sirComm.SendConfigDataTransaction(channel, destination, parameters, mode)) ? SendStatus.MODE_ERROR : SendStatus.ACK;
	}
}
