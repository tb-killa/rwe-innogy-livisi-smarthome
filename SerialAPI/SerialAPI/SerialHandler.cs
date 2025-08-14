using System;
using System.Collections.Generic;
using SerialApiInterfaces;

namespace SerialAPI;

public abstract class SerialHandler : DebugInterface, IDisposable
{
	protected Core m_core;

	public SerialHandlerType HandlerType { get; set; }

	public byte DutyCycle => m_core.DutyCycle;

	public byte RSSI => m_core.RSSI;

	public byte[] SyncWord
	{
		get
		{
			return m_core.SyncWord;
		}
		set
		{
			m_core.SyncWord = value;
		}
	}

	public byte[] DefaultSyncWord
	{
		get
		{
			return m_core.DefaultSyncWord;
		}
		set
		{
			m_core.DefaultSyncWord = value;
		}
	}

	public byte[] DefaultIP
	{
		get
		{
			return m_core.DefaultIP;
		}
		set
		{
			m_core.DefaultIP = value;
		}
	}

	public int SerialResendCount
	{
		get
		{
			return m_core.SerialResendCount;
		}
		set
		{
			m_core.SerialResendCount = value;
		}
	}

	public SerialHandler(SerialHandlerType type, Core core)
	{
		HandlerType = type;
		m_core = core;
		core.RegisterHandler(this);
		hookup(m_core);
	}

	private void core_ReceiveDebugData(string data)
	{
		debug(data);
	}

	public virtual void Dispose()
	{
		m_core.UnregisterHandler(this);
	}

	public void Handle(CORESTACKHeader header, List<byte> data)
	{
		HandleHeader(header, data);
	}

	public void HandleFrame(List<byte> data, DateTime receiveTime)
	{
		HandleData(data, receiveTime);
	}

	protected virtual void HandleData(List<byte> data, DateTime receiveTime)
	{
	}

	protected virtual void HandleHeader(CORESTACKHeader header, List<byte> data)
	{
	}

	internal virtual void HandleInit()
	{
	}

	public bool Open(string port)
	{
		return m_core.Open(port);
	}

	public bool Open(string port, Baudrate baudrate)
	{
		return m_core.Open(port, baudrate);
	}

	public SendStatus SendMessage(CORESTACKMessage Message)
	{
		List<byte> headerAsSerial = Message.Header.getHeaderAsSerial();
		headerAsSerial.AddRange(Message.Data);
		return m_core.WriteSerial(HandlerType, UseDefaultSyncWord: false, Message.Mode, headerAsSerial);
	}

	protected SendStatus BroadcastFrameToAir(List<byte> data, SendMode Mode)
	{
		return m_core.WriteSerial(HandlerType, UseDefaultSyncWord: false, Mode, data);
	}

	protected SendStatus BroadcastFrameToAirWithoutReply(List<byte> data, SendMode Mode)
	{
		return m_core.WriteSerialWithoutReply(HandlerType, UseDefaultSyncWord: false, Mode, data);
	}

	protected SendStatus writeDefaultSync(List<byte> data)
	{
		return m_core.WriteSerial(HandlerType, UseDefaultSyncWord: true, SendMode.Normal, data);
	}

	protected SendStatus writeDefaultSync(List<byte> data, SendMode Mode)
	{
		return m_core.WriteSerial(HandlerType, UseDefaultSyncWord: true, Mode, data);
	}

	protected SendStatus register(List<byte> data)
	{
		return m_core.RegisterSerial(HandlerType, SendMode.Normal, data);
	}

	protected SendStatus unregister(List<byte> data)
	{
		return m_core.UnregisterSerial(HandlerType, SendMode.Normal, data);
	}

	public void RemoveSequenseNumber(byte[] ip)
	{
		m_core.RemoveSequenceCount(ip);
	}
}
