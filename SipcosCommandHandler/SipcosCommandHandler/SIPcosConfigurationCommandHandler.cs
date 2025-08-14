using System;
using System.Collections.Generic;
using System.Threading;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosConfigurationCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveRequestConfigUpdateFrame(SIPcosHeader header, byte Channel);

	public delegate void ReceiveReportLinkPartnerProblemFrame(SIPcosHeader header, byte[] partnerIP);

	public delegate void ReceiveResponseLinkPartnerListFrame(SIPcosHeader header, byte Channel, List<SipCosPartner> partners);

	public delegate void ReceiveResponseConfigurationDataFrame(SIPcosHeader header, byte Channel, byte[] Partner, byte OtherChannel, byte ListNumber, List<SipCosConfigurationData> values);

	public delegate void ReceiveSGTINFrame(SIPcosHeader header, byte[] SGTIN);

	public delegate void ReceiveTestStatusFrame(SIPcosHeader header, byte Status);

	private EventWaitHandle m_ewh = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);

	private List<byte> m_configurationData = new List<byte>();

	public event ReceiveRequestConfigUpdateFrame ReceiveRequestConfigUpdate;

	public event ReceiveReportLinkPartnerProblemFrame ReceiveReportLinkPartnerProblem;

	public event ReceiveResponseLinkPartnerListFrame ReceiveResponseLinkPartnerList;

	public event ReceiveResponseConfigurationDataFrame ReceiveResponseConfigurationData;

	public event ReceiveSGTINFrame ReceiveSGTINData;

	public event ReceiveTestStatusFrame ReceiveTestStatusData;

	public SIPcosConfigurationCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.CONFIGURATION)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		HandlingResult result = HandlingResult.NotHandled;
		m_configurationData.Clear();
		m_configurationData.AddRange(message);
		m_ewh.Set();
		if (message.Count < 2 || header.FrameType != SIPcosFrameType.CONFIGURATION)
		{
			return result;
		}
		switch ((SipCosConfigurationCommands)message[1])
		{
		case SipCosConfigurationCommands.ResponseLinkPartnerList:
			if (this.ReceiveResponseLinkPartnerList != null)
			{
				ParseLinkPartnerList(out var partners);
				this.ReceiveResponseLinkPartnerList(header, m_configurationData[0], partners);
				result = HandlingResult.Handled;
			}
			break;
		case SipCosConfigurationCommands.ResponseConfigurationData:
			if (this.ReceiveResponseConfigurationData != null)
			{
				ParseConfigurationData(out var values);
				this.ReceiveResponseConfigurationData(header, m_configurationData[0], message.GetRange(2, 3).ToArray(), m_configurationData[5], m_configurationData[6], values);
				result = HandlingResult.Handled;
			}
			break;
		case SipCosConfigurationCommands.RequestConfigUpdate:
			if (this.ReceiveRequestConfigUpdate != null)
			{
				this.ReceiveRequestConfigUpdate(header, message[0]);
				result = HandlingResult.Handled;
			}
			break;
		case SipCosConfigurationCommands.ReportLinkPartnerProblem:
			if (this.ReceiveReportLinkPartnerProblem != null && message.Count > 4)
			{
				byte[] partnerIP = new byte[3]
				{
					message[2],
					message[3],
					message[4]
				};
				this.ReceiveReportLinkPartnerProblem(header, partnerIP);
				result = HandlingResult.Handled;
			}
			break;
		case SipCosConfigurationCommands.RequestSGTIN:
			if (this.ReceiveSGTINData != null && message[2] == 1)
			{
				byte[] sGTIN = message.GetRange(3, 12).ToArray();
				this.ReceiveSGTINData(header, sGTIN);
				result = HandlingResult.Handled;
			}
			break;
		case SipCosConfigurationCommands.TestStatus:
			if (this.ReceiveTestStatusData != null)
			{
				this.ReceiveTestStatusData(header, message[2]);
				result = HandlingResult.Handled;
			}
			break;
		}
		return result;
	}

	private void ParseLinkPartnerList(out List<SipCosPartner> partners)
	{
		partners = new List<SipCosPartner>();
		for (int i = 3; i < m_configurationData.Count; i += 4)
		{
			SipCosPartner item = new SipCosPartner
			{
				ip = new byte[3]
			};
			item.ip[0] = m_configurationData[i];
			item.ip[1] = m_configurationData[i + 1];
			item.ip[2] = m_configurationData[i + 2];
			item.channel = m_configurationData[i + 3];
			item.last = (m_configurationData[2] & 0x80) == 128;
			partners.Add(item);
		}
	}

	private void ParseConfigurationData(out List<SipCosConfigurationData> values)
	{
		values = new List<SipCosConfigurationData>();
		for (int i = 8; i < m_configurationData.Count && i + 1 < m_configurationData.Count; i += 2)
		{
			SipCosConfigurationData item = new SipCosConfigurationData
			{
				index = m_configurationData[i],
				value = m_configurationData[i + 1]
			};
			if (m_configurationData[7] == byte.MaxValue)
			{
				item.lastValue = true;
			}
			values.Add(item);
		}
	}

	protected SendStatus SendGetCommand(SIPCOSMessage Message)
	{
		SendStatus sendStatus = SendStatus.TIMEOUT;
		m_ewh.Reset();
		sendStatus = Send(Message);
		if (sendStatus == SendStatus.ACK && !m_ewh.WaitOne((int)new TimeSpan(0, 0, 10).TotalMilliseconds, exitContext: false))
		{
			return SendStatus.TIMEOUT;
		}
		return sendStatus;
	}

	public SendStatus Subscribe(SIPcosHeader header, SendMode Mode, byte Channel, byte[] partner, byte OperationMode, byte[] KeyData)
	{
		SIPCOSMessage sIPCOSMessage = GenerateSubscribe(header, Channel, partner, OperationMode, KeyData);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK && m_configurationData[0] != 0)
		{
			sendStatus = SendStatus.ERROR;
		}
		return sendStatus;
	}

	public SendStatus Unsubscribe(SIPcosHeader header, SendMode Mode, byte Channel, byte[] partner, byte[] KeyData)
	{
		SIPCOSMessage sIPCOSMessage = GenerateUnsubscribe(header, Channel, partner, KeyData);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK && m_configurationData[0] != 0)
		{
			sendStatus = SendStatus.ERROR;
		}
		return sendStatus;
	}

	public SendStatus StartConfiguration(SIPcosHeader header, SendMode Mode, byte Channel, byte[] partner, byte OperationMode, byte Key, byte ListNumber)
	{
		SIPCOSMessage sIPCOSMessage = GenerateStartConfiguration(header, Channel, partner, OperationMode, Key, ListNumber);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK && m_configurationData[0] != 0)
		{
			sendStatus = SendStatus.ERROR;
		}
		return sendStatus;
	}

	public SendStatus EndConfiguration(SIPcosHeader header, SendMode Mode, byte Channel)
	{
		SIPCOSMessage sIPCOSMessage = GenerateEndConfiguration(header, Channel);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK && m_configurationData[0] != 0)
		{
			sendStatus = SendStatus.ERROR;
		}
		return sendStatus;
	}

	public SendStatus OffsetConfiguration(SIPcosHeader header, SendMode Mode, byte Channel, byte Offset, byte[] OffsetData)
	{
		SIPCOSMessage sIPCOSMessage = GenerateOffsetConfiguration(header, Channel, Offset, OffsetData);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK && m_configurationData[0] != 0)
		{
			sendStatus = SendStatus.ERROR;
		}
		return sendStatus;
	}

	public SendStatus IndexConfiguration(SIPcosHeader header, SendMode Mode, byte Channel, byte[] IndexData)
	{
		SIPCOSMessage sIPCOSMessage = GenerateIndexConfiguration(header, Channel, IndexData);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK && m_configurationData[0] != 0)
		{
			sendStatus = SendStatus.ERROR;
		}
		return sendStatus;
	}

	public SendStatus RequestList(SIPcosHeader header, SendMode Mode, byte Channel, byte index, out List<SipCosPartner> partners)
	{
		SIPCOSMessage sIPCOSMessage = GenerateRequestList(header, Channel, index);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			ParseLinkPartnerList(out partners);
		}
		else
		{
			partners = new List<SipCosPartner>();
		}
		return sendStatus;
	}

	public SendStatus RequestConfigurationData(SIPcosHeader header, SendMode Mode, byte Channel, byte[] LinkPartner, byte OtherChannel, byte ListNr, byte start, out List<SipCosConfigurationData> values)
	{
		SIPCOSMessage sIPCOSMessage = GenerateRequestConfigurationData(header, Channel, LinkPartner, OtherChannel, ListNr, start);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			ParseConfigurationData(out values);
		}
		else
		{
			values = new List<SipCosConfigurationData>();
		}
		return sendStatus;
	}

	public SendStatus RequestSGTIN(SIPcosHeader header, SendMode Mode, out byte[] SGTIN)
	{
		SIPCOSMessage sIPCOSMessage = GenerateRequestSGTIN(header);
		sIPCOSMessage.Mode = Mode;
		SGTIN = new byte[12];
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if ((sendStatus == SendStatus.ACK || sendStatus == SendStatus.INCOMMING) && m_configurationData.Count >= 12)
		{
			for (int i = 0; i < 12; i++)
			{
				SGTIN[i] = m_configurationData[i + 3];
			}
			return SendStatus.ACK;
		}
		return sendStatus;
	}

	public SendStatus GetTestStatus(SIPcosHeader header, SendMode Mode, out byte Status)
	{
		SIPCOSMessage sIPCOSMessage = GenerateGetTestStatus(header);
		sIPCOSMessage.Mode = Mode;
		Status = 0;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			if (m_configurationData.Count > 2 && m_configurationData[1] == 130)
			{
				Status = m_configurationData[2];
			}
			else
			{
				sendStatus = SendStatus.ERROR;
			}
		}
		return sendStatus;
	}

	public SendStatus SetTestStatus(SIPcosHeader header, SendMode Mode, byte Status)
	{
		SIPCOSMessage sIPCOSMessage = GenerateSetTestStatus(header, Status);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK && m_configurationData.Count >= 1 && m_configurationData[0] != 0)
		{
			sendStatus = SendStatus.ERROR;
		}
		return sendStatus;
	}

	public SIPCOSMessage GenerateSubscribe(SIPcosHeader header, byte Channel, byte[] partner, byte OperationMode, byte[] KeyData)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(1);
		list.AddRange(partner);
		if (KeyData.Length > 0)
		{
			list.Add(KeyData[0]);
		}
		else
		{
			list.Add(0);
		}
		if (KeyData.Length > 1)
		{
			list.Add(KeyData[1]);
		}
		else
		{
			list.Add(0);
		}
		list.Add(OperationMode);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateUnsubscribe(SIPcosHeader header, byte Channel, byte[] partner, byte[] KeyData)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(2);
		list.AddRange(partner);
		list.AddRange(KeyData);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateStartConfiguration(SIPcosHeader header, byte Channel, byte[] partner, byte OperationMode, byte Key, byte ListNumber)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(5);
		list.AddRange(partner);
		list.Add(Key);
		list.Add(ListNumber);
		list.Add(OperationMode);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateEndConfiguration(SIPcosHeader header, byte Channel)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(6);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateOffsetConfiguration(SIPcosHeader header, byte Channel, byte Offset, byte[] OffsetData)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(7);
		list.Add(Offset);
		list.AddRange(OffsetData);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateIndexConfiguration(SIPcosHeader header, byte Channel, byte[] IndexData)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(8);
		list.AddRange(IndexData);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateRequestList(SIPcosHeader header, byte Channel, byte index)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(3);
		list.Add(index);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateRequestConfigurationData(SIPcosHeader header, byte Channel, byte[] LinkPartner, byte OtherChannel, byte ListNr, byte start)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(4);
		list.AddRange(LinkPartner);
		list.Add(OtherChannel);
		list.Add(ListNr);
		list.Add(start);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateRequestSGTIN(SIPcosHeader header)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(0);
		list.Add(9);
		list.Add(0);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateGetTestStatus(SIPcosHeader header)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(0);
		list.Add(129);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateSetTestStatus(SIPcosHeader header, byte Status)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.CONFIGURATION;
		List<byte> list = new List<byte>();
		list.Add(0);
		list.Add(128);
		list.Add(Status);
		return new SIPCOSMessage(header, list);
	}
}
