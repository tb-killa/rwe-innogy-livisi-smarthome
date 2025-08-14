using System;
using System.Collections.Generic;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class ReceiveFrameHandlerAnswer : ReceiveFrameHandler<SirAdapter>
{
	public ReceiveFrameHandlerAnswer(SirAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(ReceiveFrameData frameData)
	{
		return frameData.bidcosHeader.FrameType == BIDCOSFrameType.Answer;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
	{
		if (frameData.data[0] == 1)
		{
			return HandleACKStatusInfo(frameData);
		}
		return HandleOtherACK(frameData);
	}

	private BIDCOSMessage HandleACKStatusInfo(ReceiveFrameData frameData)
	{
		BIDCOSAnswerInfoFrame result = null;
		if (frameData.data != null && frameData.data.Count > 2)
		{
			BIDCOSAnswerInfoFrame bIDCOSAnswerInfoFrame = new BIDCOSAnswerInfoFrame(frameData.bidcosHeader);
			bIDCOSAnswerInfoFrame.Data = frameData.data;
			result = bIDCOSAnswerInfoFrame;
		}
		if (frameData.bidcosHeader.FrameCounter == base.DeviceAdapter.bidCosHandler.m_frameCount)
		{
			base.DeviceAdapter.bidCosHandler.answerFrameReceivedEvent.Set();
		}
		if (!base.DeviceAdapter.Included)
		{
			return null;
		}
		return result;
	}

	private BIDCOSMessage HandleOtherACK(ReceiveFrameData frameData)
	{
		BIDCOSAnswerFrame bIDCOSAnswerFrame = new BIDCOSAnswerFrame(frameData.bidcosHeader);
		if (!bIDCOSAnswerFrame.Parse(frameData.data))
		{
			bIDCOSAnswerFrame = null;
		}
		if (frameData.bidcosHeader.FrameCounter == base.DeviceAdapter.bidCosHandler.m_frameCount)
		{
			base.DeviceAdapter.bidCosHandler.m_answer_ack = bIDCOSAnswerFrame?.Ack ?? false;
			base.DeviceAdapter.bidCosHandler.answer = bIDCOSAnswerFrame;
			base.DeviceAdapter.bidCosHandler.answerFrameReceiveTime = frameData.receiveTime;
			if (bIDCOSAnswerFrame != null && bIDCOSAnswerFrame.AesProblem != null)
			{
				if (base.DeviceAdapter.CurrentKey() != null || base.DeviceAdapter.EnsureCurrentNodeDefaultKey())
				{
					SendAesSolution(bIDCOSAnswerFrame.Header.Sender, base.DeviceAdapter.sirComm.PendingMessage.ToArray(), base.DeviceAdapter.bidCosHandler.answer.AesProblem, base.DeviceAdapter.CurrentKey());
				}
				else
				{
					Log.Error(Module.SerialCommunication, $"No device key found for bidcos device (devicetype: {base.DeviceAdapter.Node.DeviceType})");
				}
				return null;
			}
			if (base.DeviceAdapter.bidCosHandler.aesChallengeResponse != null)
			{
				try
				{
					base.DeviceAdapter.bidCosHandler.m_answer_ack = base.DeviceAdapter.bidCosHandler.aesChallengeResponse.InterpretResponseReply(base.DeviceAdapter.bidCosHandler.answer.AckAesBytes) == SolutionConfirmationType.ACK;
				}
				catch (Exception ex)
				{
					Log.Error(Module.SerialCommunication, "Exception encountered while receiving an answer: " + ex.ToString());
				}
				base.DeviceAdapter.bidCosHandler.aesChallengeResponse = null;
			}
			base.DeviceAdapter.bidCosHandler.answerFrameReceivedEvent.Set();
		}
		base.DeviceAdapter.answerFrameIgnore--;
		if (base.DeviceAdapter.Included && !base.DeviceAdapter.bidCosHandler.m_block_answer && bIDCOSAnswerFrame != null && base.DeviceAdapter.answerFrameIgnore <= 0)
		{
			return bIDCOSAnswerFrame;
		}
		return null;
	}

	private bool SendAesSolution(byte[] address, byte[] originalMessage, byte[] problem, byte[] key)
	{
		base.DeviceAdapter.bidCosHandler.aesChallengeResponse = new AesChallengeResponse(key, originalMessage, problem);
		byte[] data = base.DeviceAdapter.bidCosHandler.aesChallengeResponse.Solve();
		TimeSpan timeSpan = DateTime.UtcNow - base.DeviceAdapter.bidCosHandler.answerFrameReceiveTime;
		List<byte> data2 = base.DeviceAdapter.sirComm.BuildBidCosMessage(base.DeviceAdapter.bidCosHandler.GetDefaultHeader(), base.DeviceAdapter.bidCosHandler.DefaultIP, address, 3, data, SendMode.Normal, IncrementFrameType.None, useSpecialCounter: false, 0);
		SendStatus sendStatus = SendStatus.ACK;
		for (int i = 0; i < 3; i++)
		{
			sendStatus = base.DeviceAdapter.bidCosHandler.BroadcastFrameToAir(data2, SendMode.Normal);
			if (sendStatus == SendStatus.ACK)
			{
				break;
			}
			Thread.Sleep(11);
		}
		Log.Information(Module.SerialCommunication, $"AesSolution handled in: {timeSpan.TotalMilliseconds}ms.");
		return sendStatus == SendStatus.ACK;
	}
}
