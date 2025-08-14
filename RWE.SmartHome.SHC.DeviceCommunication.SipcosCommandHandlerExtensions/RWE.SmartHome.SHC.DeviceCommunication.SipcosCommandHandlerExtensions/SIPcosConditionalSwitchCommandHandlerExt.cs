using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class SIPcosConditionalSwitchCommandHandlerExt : SIPcosCommandHandler, ISIPcosConditionalSwitchCommandHandlerExt
{
	public delegate void ReceivedConditionalSwitchCommandHandler(SIPcosHeader header, ConditionalSwitchCommand switchCommand);

	private readonly FrameEvaluator frameEvaluator;

	public event ReceivedConditionalSwitchCommandHandler ReceivedConditionalSwitchCommand;

	public SIPcosConditionalSwitchCommandHandlerExt(SIPcosHandler handler, FrameEvaluator frameEvaluator)
		: base(handler, SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND)
	{
		this.frameEvaluator = frameEvaluator;
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		byte[] array = message.ToArray();
		if (header.FrameType != SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND)
		{
			return HandlingResult.NotHandled;
		}
		ReceivedConditionalSwitchCommandHandler receivedConditionalSwitchCommand = this.ReceivedConditionalSwitchCommand;
		try
		{
			ConditionalSwitchCommand conditionalSwitchCommand = new ConditionalSwitchCommand(message.ToArray());
			if (receivedConditionalSwitchCommand != null)
			{
				if (!frameEvaluator.IsDuplicatedOrOutOfOrderFrame(header.Source, conditionalSwitchCommand.KeyChannelNumber, header.SequenceNumber, header.BiDi))
				{
					receivedConditionalSwitchCommand(header, conditionalSwitchCommand);
					return HandlingResult.Handled;
				}
				return HandlingResult.Discarded;
			}
		}
		catch (Exception ex)
		{
			string text = (header.IpSource.SequenceEqual(header.MacSource) ? $"address {header.Source.ToReadable()}" : $"IpSource {header.IpSource.ToReadable()} and MacSource {header.MacSource.ToReadable()}");
			Log.Error(Module.DeviceCommunication, string.Format("Handling of the {0} frame from {1} with sequence number {2} failed with the following error: {3}.\nReceived frame data{4}: ", header.FrameType, text, header.SequenceNumber, ex, BitConverter.ToString(array).Replace("-", " ")));
		}
		return HandlingResult.NotHandled;
	}
}
