using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class SIPcosUnconditionalSwitchCommandHandlerExt : SIPcosCommandHandler, ISIPcosUnconditionalSwitchCommandHandlerExt
{
	public delegate void ReceivedUnconditionalSwitchCommandHandler(SIPcosHeader header, SwitchCommand switchCommand);

	private readonly FrameEvaluator frameEvaluator;

	public event ReceivedUnconditionalSwitchCommandHandler ReceivedUnconditionalSwitchCommand;

	public SIPcosUnconditionalSwitchCommandHandlerExt(SIPcosHandler handler, FrameEvaluator frameEvaluator)
		: base(handler, SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND)
	{
		this.frameEvaluator = frameEvaluator;
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		if (header.FrameType != SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND)
		{
			return HandlingResult.NotHandled;
		}
		byte[] array = message.ToArray();
		ReceivedUnconditionalSwitchCommandHandler receivedUnconditionalSwitchCommand = this.ReceivedUnconditionalSwitchCommand;
		try
		{
			SwitchCommand switchCommand = new SwitchCommand(message.ToArray());
			if (receivedUnconditionalSwitchCommand != null)
			{
				if (!frameEvaluator.IsDuplicatedOrOutOfOrderFrame(header.Source, switchCommand.KeyChannelNumber, header.SequenceNumber, header.BiDi))
				{
					receivedUnconditionalSwitchCommand(header, switchCommand);
					return HandlingResult.Handled;
				}
				return HandlingResult.Discarded;
			}
		}
		catch (Exception ex)
		{
			string text = (header.IpSource.SequenceEqual(header.MacSource) ? $"address {header.Source.ToReadable()}" : $"IpSource {header.IpSource.ToReadable()} and MacSource {header.MacSource.ToReadable()}");
			Log.Error(Module.DeviceCommunication, string.Format("Handling of the {0} frame from {1} with sequence number {2} failed with the following error: {3}.\nReceived frame data: {4}", header.FrameType, text, header.SequenceNumber, ex, BitConverter.ToString(array).Replace("-", " ")));
		}
		return HandlingResult.NotHandled;
	}
}
