using System;
using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public abstract class SIPcosCommandHandler : ISIPcosCommandHandler, IDisposable
{
	protected SIPcosHandler m_hander;

	protected SIPcosFrameType m_frameType;

	public SIPcosFrameType FrameType => m_frameType;

	protected SIPcosCommandHandler(SIPcosHandler handler, SIPcosFrameType type)
	{
		m_hander = handler;
		m_frameType = type;
		m_hander.RegisterCommandHandler(m_frameType, this);
	}

	public void Dispose()
	{
		m_hander.UnregisterCommandHandler(m_frameType);
	}

	public virtual HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		return HandlingResult.Handled;
	}

	protected SendStatus Send(SIPCOSMessage Message)
	{
		return m_hander.SendMessage(Message);
	}

	protected SendStatus SendDefaultSync(SIPCOSMessage Message)
	{
		return m_hander.SendMessageDefaultSync(Message.Header, Message.Data.ToArray(), Message.Mode);
	}
}
