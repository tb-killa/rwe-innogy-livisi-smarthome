using System.Collections.Generic;
using System.Linq;

namespace SerialAPI.BidCoSFrames;

internal class BIDCOSAnswerInfoFrame : BIDCOSMessage
{
	private AnswerStatus m_status;

	public BIDCOSAnswerInfoFrame(BIDCOSHeader header)
		: base(header)
	{
		m_status = AnswerStatus.ACK_STATUS;
	}

	public bool Parse(List<byte> message)
	{
		if (message.Count < 1)
		{
			return false;
		}
		m_status = (AnswerStatus)message[0];
		m_data = message;
		return m_status == AnswerStatus.ACK_STATUS;
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
		data.AddRange(base.Data.Skip(1));
	}
}
