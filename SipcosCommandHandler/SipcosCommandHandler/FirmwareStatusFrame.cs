using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class FirmwareStatusFrame : SIPCOSMessage
{
	private FirmwareCommands m_command;

	private FirmwareReplyStatus m_status;

	private byte[] m_nextSequence;

	public FirmwareCommands Command
	{
		get
		{
			return m_command;
		}
		set
		{
			m_command = value;
		}
	}

	public FirmwareReplyStatus Status
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

	public byte[] NextSequence
	{
		get
		{
			return m_nextSequence;
		}
		set
		{
			m_nextSequence = value;
		}
	}

	public FirmwareStatusFrame(SIPcosHeader header)
		: base(header)
	{
	}

	public void parse(List<byte> data)
	{
		if (data.Count > 2)
		{
			m_command = (FirmwareCommands)data[1];
			m_status = (FirmwareReplyStatus)data[2];
			if (data.Count > 4)
			{
				m_nextSequence = new byte[2];
				m_nextSequence[0] = data[3];
				m_nextSequence[1] = data[4];
			}
		}
	}
}
