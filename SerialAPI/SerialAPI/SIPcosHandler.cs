using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Scheduler;
using SerialAPI.BidCosLayer;

namespace SerialAPI;

public sealed class SIPcosHandler : SerialHandler
{
	public delegate void ReceiveMessage(SIPCOSMessage Message);

	private Hashtable m_handlers = new Hashtable();

	private BidCosHandler2 m_bidcos;

	public SIPcosHandler(Core core, IBidCoSKeyRetriever bidcosKeyRetriever, IBidCoSPersistence bidcosStorage, IEventManager eventManager, IScheduler scheduler)
		: base(SerialHandlerType.SIPCOS_HANDLER, core)
	{
		m_bidcos = new BidCosHandler2(core, this, bidcosKeyRetriever, bidcosStorage, eventManager, scheduler);
	}

	public override void Dispose()
	{
		base.Dispose();
		m_bidcos.Dispose();
	}

	public void SetBidCosNodes(string bidcosNodes)
	{
		m_bidcos.SetBidcosNodes(bidcosNodes);
	}

	public bool IsSupportedEncryption(byte[] sgtin)
	{
		return m_bidcos.IsSupportedEncryption(sgtin);
	}

	protected override void HandleHeader(CORESTACKHeader header, List<byte> data)
	{
		if (header.CorestackFrameType != CorestackFrameType.SIPCOS_APPLICATION)
		{
			return;
		}
		SIPcosHeader sIPcosHeader = new SIPcosHeader(header);
		sIPcosHeader.parseHeader(data);
		SIPcosFrameType sIPcosFrameType = sIPcosHeader.FrameType;
		HandlingResult handlingResult = HandlingResult.NotHandled;
		if (!sIPcosHeader.MacSecurity && sIPcosFrameType != SIPcosFrameType.NETWORK_MANAGEMENT_FRAME && !header.SyncWord.SequenceEqual(Core.BIDCosDefaultSync))
		{
			return;
		}
		if (sIPcosFrameType == SIPcosFrameType.TIMESLOT_CC)
		{
			sIPcosFrameType = SIPcosFrameType.STATUSINFO;
		}
		if (m_handlers.ContainsKey(sIPcosFrameType))
		{
			handlingResult = ((ISIPcosCommandHandler)m_handlers[sIPcosFrameType]).Handle(sIPcosHeader, data.GetRange(sIPcosHeader.HeaderSize, data.Count - sIPcosHeader.HeaderSize));
		}
		if (sIPcosFrameType == SIPcosFrameType.ANSWER)
		{
			foreach (DictionaryEntry handler in m_handlers)
			{
				if ((SIPcosFrameType)handler.Key != SIPcosFrameType.ANSWER)
				{
					((ISIPcosCommandHandler)handler.Value).Handle(sIPcosHeader, data.GetRange(sIPcosHeader.HeaderSize, data.Count - sIPcosHeader.HeaderSize));
				}
			}
		}
		if (!sIPcosHeader.BiDi)
		{
			return;
		}
		if (!m_handlers.ContainsKey(sIPcosFrameType))
		{
			SendNAK(header);
			return;
		}
		switch (handlingResult)
		{
		case HandlingResult.Discarded:
			SendACK(header);
			break;
		case HandlingResult.NotHandled:
			SendNAK(header);
			break;
		case HandlingResult.Handled:
			break;
		}
	}

	private void SendNAK(CORESTACKHeader header)
	{
		SendAnswer(header, 128);
	}

	private void SendACK(CORESTACKHeader header)
	{
		SendAnswer(header, 0);
	}

	private void SendAnswer(CORESTACKHeader header, byte answerStatus)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.MacDestination = header.IpSource;
		sIPcosHeader.MacSource = header.IpDestination;
		sIPcosHeader.FrameType = SIPcosFrameType.ANSWER;
		sIPcosHeader.BiDi = false;
		sIPcosHeader.StayAwake = false;
		List<byte> list = new List<byte>();
		list.Add(answerStatus);
		SendMessage(new SIPCOSMessage(sIPcosHeader, list));
	}

	public SendStatus SendMessage(SIPCOSMessage Message)
	{
		SendStatus sendStatus = m_bidcos.SendCosIpMessage(Message.Header, Message.Data, Message.Mode);
		if (sendStatus == SendStatus.MODE_ERROR)
		{
			List<byte> list = new List<byte>();
			list.AddRange(Message.Header.getHeaderAsSerial());
			list.AddRange(Message.Data);
			if (Message.ExpectReply)
			{
				return BroadcastFrameToAir(list, Message.Mode);
			}
			return BroadcastFrameToAirWithoutReply(list, Message.Mode);
		}
		return sendStatus;
	}

	public byte? GetBidCosCounter(byte[] address)
	{
		return m_bidcos.GetBidCosCounter(address);
	}

	public SendStatus SendMessageDefaultSync(SIPcosHeader header, byte[] message, SendMode Mode)
	{
		SendStatus sendStatus = m_bidcos.SendCosIpMessage(header, message, Mode);
		if (sendStatus == SendStatus.MODE_ERROR)
		{
			List<byte> list = new List<byte>();
			list.AddRange(header.getHeaderAsSerial());
			list.AddRange(message);
			return writeDefaultSync(list, Mode);
		}
		return sendStatus;
	}

	internal override void HandleInit()
	{
	}

	public void RegisterCommandHandler(SIPcosFrameType type, ISIPcosCommandHandler handler)
	{
		if (!m_handlers.ContainsKey(type) && handler != null)
		{
			m_handlers.Add(type, handler);
		}
	}

	public void UnregisterCommandHandler(SIPcosFrameType type)
	{
		if (m_handlers.ContainsKey(type))
		{
			m_handlers.Remove(type);
		}
	}
}
