using System;
using System.Collections.Generic;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class ReceiveFrameHandlerAnswer : ReceiveFrameHandler<Wsd2Adapter>
{
	public ReceiveFrameHandlerAnswer(Wsd2Adapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(ReceiveFrameData frameData)
	{
		return frameData.bidcosHeader.FrameType == BIDCOSFrameType.Answer;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
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
				SendAesSolution(bIDCOSAnswerFrame.Header.Sender, base.DeviceAdapter.wsdComm.PendingMessage.ToArray(), base.DeviceAdapter.bidCosHandler.answer.AesProblem, base.DeviceAdapter.CurrentKey());
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
		if (base.DeviceAdapter.Included && !base.DeviceAdapter.bidCosHandler.m_block_answer && bIDCOSAnswerFrame != null)
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
		List<byte> data2 = base.DeviceAdapter.wsdComm.BuildBidCosMessage(base.DeviceAdapter.bidCosHandler.GetDefaultHeader(), base.DeviceAdapter.bidCosHandler.DefaultIP, address, 3, data, SendMode.Normal, IncrementFrameType.None, useSpecialCounter: false, 0);
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
