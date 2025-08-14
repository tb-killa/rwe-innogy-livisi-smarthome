using System;
using Org.Mentalis.Security.Ssl.Ssl3;
using Org.Mentalis.Security.Ssl.Tls1;

namespace Org.Mentalis.Security.Ssl.Shared;

internal sealed class CompatibilityLayer
{
	private readonly RecordLayer m_MinLayer;

	private readonly ProtocolVersion m_MinVersion;

	private readonly SecurityOptions m_Options;

	private byte[] m_Buffer;

	private byte[] m_Hello;

	private ProtocolVersion m_MaxVersion;

	public CompatibilityLayer(SocketController controller, SecurityOptions options)
	{
		m_Buffer = new byte[0];
		m_MinVersion = GetMinProtocol(options.Protocol);
		m_MaxVersion = GetMaxProtocol(options.Protocol);
		if (m_MinVersion.GetVersionInt() == 30)
		{
			if (options.Entity == ConnectionEnd.Client)
			{
				m_MinLayer = new RecordLayer(controller, new Ssl3ClientHandshakeLayer(null, options));
			}
			else
			{
				m_MinLayer = new RecordLayer(controller, new Ssl3ServerHandshakeLayer(null, options));
			}
		}
		else if (options.Entity == ConnectionEnd.Client)
		{
			m_MinLayer = new RecordLayer(controller, new Tls1ClientHandshakeLayer(null, options));
		}
		else
		{
			m_MinLayer = new RecordLayer(controller, new Tls1ServerHandshakeLayer(null, options));
		}
		m_MinLayer.HandshakeLayer.RecordLayer = m_MinLayer;
		m_Options = options;
	}

	public byte[] GetClientHello()
	{
		if (m_Hello == null)
		{
			m_Hello = m_MinLayer.GetControlBytes(ControlType.ClientHello);
		}
		return m_Hello;
	}

	public CompatibilityResult ProcessHello(byte[] bytes, int offset, int size)
	{
		if (m_Options.Entity == ConnectionEnd.Client)
		{
			return ProcessServerHello(bytes, offset, size);
		}
		return ProcessClientHello(bytes, offset, size);
	}

	private CompatibilityResult ProcessServerHello(byte[] bytes, int offset, int size)
	{
		byte[] array = new byte[m_Buffer.Length + size];
		Array.Copy(m_Buffer, 0, array, 0, m_Buffer.Length);
		Array.Copy(bytes, offset, array, m_Buffer.Length, size);
		if (IsInvalidSsl3Hello(array))
		{
			throw new SslException(AlertDescription.HandshakeFailure, "The server hello message uses a protocol that was not recognized.");
		}
		if (m_Buffer.Length + size < 11)
		{
			m_Buffer = array;
			return new CompatibilityResult(null, new SslRecordStatus(SslStatus.MessageIncomplete, null, null));
		}
		ProtocolVersion pv = new ProtocolVersion(array[9], array[10]);
		if (SupportsProtocol(m_Options.Protocol, pv))
		{
			if (m_MinLayer.HandshakeLayer.GetVersion().GetVersionInt() != pv.GetVersionInt())
			{
				if (pv.GetVersionInt() == 30)
				{
					m_MinLayer.HandshakeLayer = new Ssl3ClientHandshakeLayer(m_MinLayer.HandshakeLayer);
				}
				else
				{
					m_MinLayer.HandshakeLayer = new Tls1ClientHandshakeLayer(m_MinLayer.HandshakeLayer);
				}
			}
			return new CompatibilityResult(m_MinLayer, m_MinLayer.ProcessBytes(array, 0, array.Length));
		}
		throw new SslException(AlertDescription.HandshakeFailure, "The client and server could not agree on the protocol version to use.");
	}

	private bool IsInvalidSsl3Hello(byte[] buffer)
	{
		if ((buffer.Length <= 0 || buffer[0] == 22) && (buffer.Length <= 1 || buffer[1] == 3))
		{
			if (buffer.Length > 2 && buffer[2] != 0)
			{
				return buffer[2] != 1;
			}
			return false;
		}
		return true;
	}

	private bool IsInvalidSsl2Hello(byte[] buffer)
	{
		if (buffer.Length < 6)
		{
			return false;
		}
		int num = (((buffer[0] & 0x80) == 0) ? 3 : 2);
		if (buffer[num] == 1 && buffer[num + 1] == 3)
		{
			if (buffer[num + 2] != 0)
			{
				return buffer[num + 2] != 1;
			}
			return false;
		}
		return true;
	}

	private bool IsSsl2HelloComplete(byte[] buffer)
	{
		if (buffer.Length < 3)
		{
			return false;
		}
		if ((buffer[0] & 0x80) != 0)
		{
			return buffer.Length == (((buffer[0] & 0x7F) << 8) | (buffer[1] + 2));
		}
		return buffer.Length == (((buffer[0] & 0x3F) << 8) | (buffer[1] + 3));
	}

	private byte[] ExtractSsl2Content(byte[] buffer)
	{
		byte[] array = (((buffer[0] & 0x80) == 0) ? new byte[buffer.Length - 3] : new byte[buffer.Length - 2]);
		Array.Copy(buffer, buffer.Length - array.Length, array, 0, array.Length);
		return array;
	}

	private ProtocolVersion ExtractSsl2Version(byte[] buffer)
	{
		if ((buffer[0] & 0x80) != 0)
		{
			return new ProtocolVersion(buffer[3], buffer[4]);
		}
		return new ProtocolVersion(buffer[4], buffer[5]);
	}

	private CompatibilityResult ProcessClientHello(byte[] bytes, int offset, int size)
	{
		byte[] array = new byte[m_Buffer.Length + size];
		Array.Copy(m_Buffer, 0, array, 0, m_Buffer.Length);
		Array.Copy(bytes, offset, array, m_Buffer.Length, size);
		if (IsInvalidSsl3Hello(array) && IsInvalidSsl2Hello(array))
		{
			throw new SslException(AlertDescription.HandshakeFailure, "The client hello message uses a protocol that was not recognized.");
		}
		if (m_Buffer.Length + bytes.Length < 11 || (IsInvalidSsl3Hello(array) && !IsSsl2HelloComplete(array)))
		{
			m_Buffer = array;
			return new CompatibilityResult(null, new SslRecordStatus(SslStatus.MessageIncomplete, null, null));
		}
		ProtocolVersion pv = (IsInvalidSsl3Hello(array) ? ExtractSsl2Version(array) : new ProtocolVersion(array[9], array[10]));
		if (pv.GetVersionInt() > m_MaxVersion.GetVersionInt())
		{
			pv = m_MaxVersion;
		}
		if (SupportsProtocol(m_Options.Protocol, pv))
		{
			if (m_MinLayer.HandshakeLayer.GetVersion().GetVersionInt() != pv.GetVersionInt())
			{
				if (pv.GetVersionInt() == 30)
				{
					m_MinLayer.HandshakeLayer = new Ssl3ServerHandshakeLayer(m_MinLayer.HandshakeLayer);
				}
				else
				{
					m_MinLayer.HandshakeLayer = new Tls1ServerHandshakeLayer(m_MinLayer.HandshakeLayer);
				}
			}
			if (!IsInvalidSsl3Hello(array))
			{
				return new CompatibilityResult(m_MinLayer, m_MinLayer.ProcessBytes(array, 0, array.Length));
			}
			return new CompatibilityResult(m_MinLayer, m_MinLayer.ProcessSsl2Hello(ExtractSsl2Content(array)));
		}
		throw new SslException(AlertDescription.HandshakeFailure, "The client and server could not agree on the protocol version to use.");
	}

	public static bool SupportsSsl3(SecureProtocol protocol)
	{
		return (protocol & SecureProtocol.Ssl3) != 0;
	}

	public static bool SupportsTls1(SecureProtocol protocol)
	{
		return (protocol & SecureProtocol.Tls1) != 0;
	}

	public static bool SupportsProtocol(SecureProtocol protocol, ProtocolVersion pv)
	{
		if (pv.GetVersionInt() == 30)
		{
			return SupportsSsl3(protocol);
		}
		return SupportsTls1(protocol);
	}

	public static ProtocolVersion GetMinProtocol(SecureProtocol protocol)
	{
		if (SupportsSsl3(protocol))
		{
			return new ProtocolVersion(3, 0);
		}
		return new ProtocolVersion(3, 1);
	}

	public static ProtocolVersion GetMaxProtocol(SecureProtocol protocol)
	{
		if (SupportsTls1(protocol))
		{
			return new ProtocolVersion(3, 1);
		}
		return new ProtocolVersion(3, 0);
	}
}
