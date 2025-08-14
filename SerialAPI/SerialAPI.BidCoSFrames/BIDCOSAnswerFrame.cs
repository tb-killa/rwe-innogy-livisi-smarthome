using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace SerialAPI.BidCoSFrames;

internal class BIDCOSAnswerFrame : BIDCOSMessage
{
	private AnswerStatus m_status;

	private byte[] aesProblem;

	private byte[] ackAesBytes;

	public AnswerStatus Status
	{
		get
		{
			return m_status;
		}
		set
		{
			m_status = value;
		}
	}

	public bool Ack => ((byte)m_status & 0x80) != 128;

	public byte[] AesProblem
	{
		get
		{
			return aesProblem;
		}
		set
		{
			aesProblem = value;
		}
	}

	public byte[] AckAesBytes
	{
		get
		{
			return ackAesBytes;
		}
		set
		{
			ackAesBytes = value;
		}
	}

	public BIDCOSAnswerFrame(BIDCOSHeader header)
		: base(header)
	{
	}

	public bool Parse(List<byte> message)
	{
		if (message.Count < 1)
		{
			return false;
		}
		m_data = message;
		m_status = (AnswerStatus)message[0];
		aesProblem = null;
		if (m_status == AnswerStatus.ACK_AES)
		{
			if (message.Count >= 7)
			{
				aesProblem = message.Skip(1).Take(6).ToArray();
			}
			else
			{
				Log.Information(Module.SerialCommunication, "ACK AES challenge received with incomplete problem: " + message.ToArray().ToReadable());
			}
		}
		if (m_status == AnswerStatus.ACK && message.Count >= 5)
		{
			ackAesBytes = message.Skip(1).Take(4).ToArray();
		}
		return true;
	}

	public override void GenerateSIPCOSMessage(ref CORESTACKHeader header, ref List<byte> data)
	{
		base.GenerateSIPCOSMessage(ref header, ref data);
		data.Clear();
		byte b = 16;
		if ((m_header.HeaderBits & BIDCOSHeaderBitField.Bidi) == BIDCOSHeaderBitField.Bidi)
		{
			b |= 0x80;
		}
		if ((m_header.HeaderBits & BIDCOSHeaderBitField.WakeUp) == BIDCOSHeaderBitField.WakeUp)
		{
			b |= 0x40;
		}
		data.Add(b);
		data.Add(m_header.FrameCounter);
		data.Add((byte)m_status);
	}
}
