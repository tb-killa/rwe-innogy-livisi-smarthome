using System;
using System.IO;
using System.Security.Cryptography;
using Mentalis.Security.Library.Security;
using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl.Shared;

internal class RecordLayer : IDisposable
{
	private const int m_MaxMessageLength = 16384;

	private readonly SocketController m_Controller;

	private ICryptoTransform m_BulkDecryption;

	private ICryptoTransform m_BulkEncryption;

	private HandshakeLayer m_HandshakeLayer;

	private byte[] m_IncompleteMessage;

	private ulong m_InputSequenceNumber;

	private bool m_IsDisposed;

	private CompressionAlgorithm m_LocalCompressor;

	private KeyedHashAlgorithm0 m_LocalHasher;

	private ulong m_OutputSequenceNumber;

	private CompressionAlgorithm m_RemoteCompressor;

	private KeyedHashAlgorithm0 m_RemoteHasher;

	public SecureSocket Parent => m_Controller.Parent;

	public SslAlgorithms ActiveEncryption => m_HandshakeLayer.ActiveEncryption;

	public Certificate RemoteCertificate => m_HandshakeLayer.RemoteCertificate;

	internal HandshakeLayer HandshakeLayer
	{
		get
		{
			return m_HandshakeLayer;
		}
		set
		{
			m_HandshakeLayer = value;
		}
	}

	public RecordLayer(SocketController controller, HandshakeLayer handshakeLayer)
	{
		m_IsDisposed = false;
		m_Controller = controller;
		m_HandshakeLayer = handshakeLayer;
		m_IncompleteMessage = new byte[0];
		ChangeLocalState(null, null, null);
		ChangeRemoteState(null, null, null);
	}

	public void Dispose()
	{
		if (!m_IsDisposed)
		{
			m_IsDisposed = true;
			if (m_BulkEncryption != null)
			{
				m_BulkEncryption.Dispose();
			}
			if (m_BulkDecryption != null)
			{
				m_BulkDecryption.Dispose();
			}
			if (m_LocalHasher != null)
			{
				m_LocalHasher.Clear();
			}
			if (m_RemoteHasher != null)
			{
				m_RemoteHasher.Clear();
			}
			m_InputSequenceNumber = 0uL;
			m_OutputSequenceNumber = 0uL;
			m_HandshakeLayer.Dispose();
		}
	}

	public void ChangeLocalState(CompressionAlgorithm compressor, ICryptoTransform encryptor, KeyedHashAlgorithm0 localHasher)
	{
		m_LocalCompressor = compressor;
		m_BulkEncryption = encryptor;
		m_LocalHasher = localHasher;
		m_OutputSequenceNumber = 0uL;
	}

	public void ChangeRemoteState(CompressionAlgorithm decompressor, ICryptoTransform decryptor, KeyedHashAlgorithm0 remoteHasher)
	{
		m_RemoteCompressor = decompressor;
		m_BulkDecryption = decryptor;
		m_RemoteHasher = remoteHasher;
		m_InputSequenceNumber = 0uL;
	}

	protected byte[] InternalEncryptBytes2(byte[] buffer, int offset, int size, ContentType type)
	{
		byte[] array = new byte[GetEncryptedLength(size) + 5];
		array[0] = (byte)type;
		array[1] = m_HandshakeLayer.GetVersion().major;
		array[2] = m_HandshakeLayer.GetVersion().minor;
		array[3] = (byte)((array.Length - 5) / 256);
		array[4] = (byte)((array.Length - 5) % 256);
		byte[] array2 = null;
		try
		{
			if (m_LocalHasher == null)
			{
				Array.Copy(buffer, offset, array, 5, size);
			}
			else
			{
				m_LocalHasher.Initialize();
				array2 = new byte[13];
				Array.Copy(GetULongBytes(m_OutputSequenceNumber), 0, array2, 0, 8);
				array2[8] = (byte)type;
				if (m_HandshakeLayer.GetProtocol() == SecureProtocol.Tls1)
				{
					array2[9] = array[1];
					array2[10] = array[2];
					array2[11] = (byte)(size / 256);
					array2[12] = (byte)(size % 256);
					m_LocalHasher.TransformBlock(array2, 0, 13, array2, 0);
				}
				else
				{
					array2[9] = (byte)(size / 256);
					array2[10] = (byte)(size % 256);
					m_LocalHasher.TransformBlock(array2, 0, 11, array2, 0);
				}
				m_LocalHasher.TransformFinalBlock(buffer, offset, size);
				array2 = m_LocalHasher.Hash;
				if (m_BulkEncryption.OutputBlockSize == 1)
				{
					m_BulkEncryption.TransformBlock(buffer, offset, size, array, 5);
					m_BulkEncryption.TransformBlock(array2, 0, array2.Length, array, size + 5);
				}
				else
				{
					int outputBlockSize = m_BulkEncryption.OutputBlockSize;
					byte b = (byte)((outputBlockSize - (size + array2.Length + 1) % outputBlockSize) % outputBlockSize);
					int num = size % outputBlockSize;
					int outputOffset = 5 + size - num;
					if (size - num != 0)
					{
						m_BulkEncryption.TransformBlock(buffer, offset, size - num, array, 5);
					}
					byte[] array3 = new byte[num + m_LocalHasher.HashSize / 8 + b + 1];
					if (m_HandshakeLayer.GetProtocol() == SecureProtocol.Tls1)
					{
						for (int i = num + array2.Length; i < array3.Length; i++)
						{
							array3[i] = b;
						}
					}
					else
					{
						m_HandshakeLayer.RNG.GetBytes(array3);
						array[array.Length - 1] = b;
					}
					if (num > 0)
					{
						Array.Copy(buffer, offset + size - num, array3, 0, num);
					}
					Array.Copy(array2, 0, array3, num, array2.Length);
					m_BulkEncryption.TransformBlock(array3, 0, array3.Length, array, outputOffset);
				}
			}
		}
		catch (Exception e)
		{
			throw new SslException(e, AlertDescription.InternalError, "An exception occurred");
		}
		m_OutputSequenceNumber++;
		return array;
	}

	private int GetEncryptedLength(int size)
	{
		if (m_LocalHasher != null)
		{
			if (m_BulkEncryption.OutputBlockSize == 1)
			{
				return size + m_LocalHasher.HashSize / 8;
			}
			int outputBlockSize = m_BulkEncryption.OutputBlockSize;
			byte b = (byte)((outputBlockSize - (size + m_LocalHasher.HashSize / 8 + 1) % outputBlockSize) % outputBlockSize);
			return size + m_LocalHasher.HashSize / 8 + b + 1;
		}
		return size;
	}

	protected byte[] InternalEncryptBytes(byte[] buffer, int offset, int size, ContentType type)
	{
		byte[] array = new byte[size];
		Array.Copy(buffer, offset, array, 0, size);
		RecordMessage recordMessage = new RecordMessage(MessageType.PlainText, type, m_HandshakeLayer.GetVersion(), array);
		WrapMessage(recordMessage);
		return recordMessage.ToBytes();
	}

	protected void WrapMessage(RecordMessage message)
	{
		if (message.length != message.fragment.Length)
		{
			throw new SslException(AlertDescription.IllegalParameter, "Message length is invalid.");
		}
		byte[] array = null;
		try
		{
			if (m_LocalCompressor != null)
			{
				message.fragment = m_LocalCompressor.Compress(message.fragment);
				message.length = (ushort)message.fragment.Length;
			}
			if (m_LocalHasher != null)
			{
				array = GetULongBytes(m_OutputSequenceNumber);
				m_LocalHasher.Initialize();
				m_LocalHasher.TransformBlock(array, 0, array.Length, array, 0);
				array = message.ToBytes();
				if (m_HandshakeLayer.GetProtocol() == SecureProtocol.Tls1)
				{
					m_LocalHasher.TransformFinalBlock(array, 0, array.Length);
				}
				else
				{
					if (m_HandshakeLayer.GetProtocol() != SecureProtocol.Ssl3)
					{
						throw new NotSupportedException("Only SSL3 and TLS1 are supported");
					}
					m_LocalHasher.TransformBlock(array, 0, 1, array, 0);
					m_LocalHasher.TransformFinalBlock(array, 3, array.Length - 3);
				}
				array = m_LocalHasher.Hash;
				if (m_BulkEncryption.OutputBlockSize == 1)
				{
					byte[] array2 = new byte[message.length + array.Length];
					m_BulkEncryption.TransformBlock(message.fragment, 0, message.length, array2, 0);
					m_BulkEncryption.TransformBlock(array, 0, array.Length, array2, message.length);
					message.fragment = array2;
				}
				else
				{
					int outputBlockSize = m_BulkEncryption.OutputBlockSize;
					byte b = (byte)((outputBlockSize - (message.length + array.Length + 1) % outputBlockSize) % outputBlockSize);
					byte[] array3 = new byte[message.length + array.Length + b + 1];
					Array.Copy(message.fragment, 0, array3, 0, message.length);
					Array.Copy(array, 0, array3, message.length, array.Length);
					if (m_HandshakeLayer.GetProtocol() == SecureProtocol.Tls1)
					{
						for (int i = message.length + array.Length; i < array3.Length; i++)
						{
							array3[i] = b;
						}
					}
					else
					{
						byte[] array4 = new byte[array3.Length - message.length - array.Length];
						m_HandshakeLayer.RNG.GetBytes(array4);
						Array.Copy(array4, 0, array3, message.length + array.Length, array4.Length);
						array3[array3.Length - 1] = b;
					}
					m_BulkEncryption.TransformBlock(array3, 0, array3.Length, array3, 0);
					message.fragment = array3;
				}
				message.length = (ushort)message.fragment.Length;
			}
		}
		catch (Exception e)
		{
			throw new SslException(e, AlertDescription.InternalError, "An exception occurred");
		}
		message.messageType = MessageType.Encrypted;
		m_OutputSequenceNumber++;
	}

	protected void UnwrapMessage(RecordMessage message)
	{
		if (message.length != message.fragment.Length)
		{
			throw new SslException(AlertDescription.IllegalParameter, "Message length is invalid.");
		}
		byte[] array = null;
		byte[] array2 = null;
		byte[] array3 = null;
		bool flag = false;
		if (m_BulkDecryption != null)
		{
			if (message.length <= m_RemoteHasher.HashSize / 8)
			{
				throw new SslException(AlertDescription.DecodeError, "Message is too small.");
			}
			if (message.length % m_BulkDecryption.OutputBlockSize != 0)
			{
				throw new SslException(AlertDescription.DecryptError, "Message length is invalid.");
			}
			if (m_BulkDecryption.OutputBlockSize == 1)
			{
				array2 = new byte[message.length];
				m_BulkDecryption.TransformBlock(message.fragment, 0, message.length, array2, 0);
				array = new byte[m_RemoteHasher.HashSize / 8];
				Array.Copy(array2, message.length - array.Length, array, 0, array.Length);
				message.fragment = new byte[array2.Length - array.Length];
				Array.Copy(array2, 0, message.fragment, 0, message.fragment.Length);
				message.length = (ushort)message.fragment.Length;
			}
			else
			{
				array2 = new byte[message.fragment.Length];
				m_BulkDecryption.TransformBlock(message.fragment, 0, array2.Length, array2, 0);
				byte b = array2[array2.Length - 1];
				if (message.length < b + m_RemoteHasher.HashSize / 8 + 1)
				{
					flag = true;
					array = new byte[m_RemoteHasher.HashSize / 8];
				}
				else
				{
					int num = message.length - b - 1;
					array = new byte[m_RemoteHasher.HashSize / 8];
					Array.Copy(array2, num - array.Length, array, 0, array.Length);
					message.fragment = new byte[num - array.Length];
					Array.Copy(array2, 0, message.fragment, 0, message.fragment.Length);
					message.length = (ushort)message.fragment.Length;
					if (m_HandshakeLayer.GetProtocol() == SecureProtocol.Tls1)
					{
						for (int i = num; i < array2.Length; i++)
						{
							if (array2[i] != b)
							{
								flag = true;
							}
						}
					}
				}
			}
			array3 = GetULongBytes(m_InputSequenceNumber);
			m_RemoteHasher.Initialize();
			m_RemoteHasher.TransformBlock(array3, 0, array3.Length, array3, 0);
			array3 = message.ToBytes();
			if (m_HandshakeLayer.GetProtocol() == SecureProtocol.Tls1)
			{
				m_RemoteHasher.TransformFinalBlock(array3, 0, array3.Length);
			}
			else
			{
				if (m_HandshakeLayer.GetProtocol() != SecureProtocol.Ssl3)
				{
					throw new NotSupportedException("Only SSL3 and TLS1 are supported");
				}
				m_RemoteHasher.TransformBlock(array3, 0, 1, array3, 0);
				m_RemoteHasher.TransformFinalBlock(array3, 3, array3.Length - 3);
			}
			array3 = m_RemoteHasher.Hash;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != array3[j])
				{
					flag = true;
				}
			}
			if (flag)
			{
				throw new SslException(AlertDescription.BadRecordMac, "An error occurred during the decryption and verification process.");
			}
		}
		if (m_RemoteCompressor != null)
		{
			message.fragment = m_RemoteCompressor.Decompress(message.fragment);
			message.length = (ushort)message.fragment.Length;
		}
		message.messageType = MessageType.PlainText;
		m_InputSequenceNumber++;
	}

	protected byte[] GetULongBytes(ulong number)
	{
		byte[] bytes = BitConverter.GetBytes(number);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	public byte[] EncryptBytes(byte[] buffer, int offset, int size, ContentType type)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset + size > buffer.Length || size < 0)
		{
			throw new ArgumentException();
		}
		MemoryStream memoryStream = new MemoryStream(size + (size / 16384 + 1) * 25);
		for (int i = 0; i < size; i += 16384)
		{
			if (i + 16384 > size)
			{
				byte[] array = InternalEncryptBytes(buffer, offset + i, size - i, type);
				memoryStream.Write(array, 0, array.Length);
			}
			else
			{
				byte[] array = InternalEncryptBytes(buffer, offset + i, 16384, type);
				memoryStream.Write(array, 0, array.Length);
			}
		}
		return memoryStream.ToArray();
	}

	protected bool IsRecordMessageComplete(byte[] buffer, int offset)
	{
		if (buffer.Length < offset + 6)
		{
			return false;
		}
		int num = buffer[offset + 3] * 256 + buffer[offset + 4];
		return buffer.Length >= offset + 5 + num;
	}

	public SslRecordStatus ProcessBytes(byte[] buffer, int offset, int size)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset + size > buffer.Length || size <= 0)
		{
			throw new ArgumentException();
		}
		SslRecordStatus result = new SslRecordStatus
		{
			Status = SslStatus.MessageIncomplete
		};
		MemoryStream memoryStream = new MemoryStream();
		MemoryStream memoryStream2 = new MemoryStream();
		byte[] array = new byte[m_IncompleteMessage.Length + size];
		Array.Copy(m_IncompleteMessage, 0, array, 0, m_IncompleteMessage.Length);
		Array.Copy(buffer, offset, array, m_IncompleteMessage.Length, size);
		int i;
		int num;
		for (i = 0; IsRecordMessageComplete(array, i); i += num)
		{
			RecordMessage recordMessage = new RecordMessage(array, i);
			num = recordMessage.length + 5;
			UnwrapMessage(recordMessage);
			if (recordMessage.contentType == ContentType.ApplicationData)
			{
				if (m_HandshakeLayer.IsNegotiating())
				{
					throw new SslException(AlertDescription.UnexpectedMessage, "The handshake procedure was not completed successfully before application data was received.");
				}
				memoryStream.Write(recordMessage.fragment, 0, recordMessage.fragment.Length);
				result.Status = SslStatus.OK;
			}
			else
			{
				SslHandshakeStatus sslHandshakeStatus = m_HandshakeLayer.ProcessMessages(recordMessage);
				if (sslHandshakeStatus.Message != null)
				{
					memoryStream2.Write(sslHandshakeStatus.Message, 0, sslHandshakeStatus.Message.Length);
				}
				result.Status = sslHandshakeStatus.Status;
			}
		}
		if (i > 0)
		{
			m_IncompleteMessage = new byte[array.Length - i];
			Array.Copy(array, i, m_IncompleteMessage, 0, m_IncompleteMessage.Length);
		}
		else
		{
			m_IncompleteMessage = array;
		}
		if (memoryStream.Length > 0)
		{
			result.Decrypted = memoryStream.ToArray();
		}
		memoryStream.Close();
		if (memoryStream2.Length > 0)
		{
			result.Buffer = memoryStream2.ToArray();
		}
		memoryStream2.Close();
		return result;
	}

	public SslRecordStatus ProcessSsl2Hello(byte[] hello)
	{
		SslHandshakeStatus sslHandshakeStatus = m_HandshakeLayer.ProcessSsl2Hello(hello);
		return new SslRecordStatus(sslHandshakeStatus.Status, sslHandshakeStatus.Message, null);
	}

	public byte[] GetControlBytes(ControlType type)
	{
		return m_HandshakeLayer.GetControlBytes(type);
	}

	public bool IsNegotiating()
	{
		return m_HandshakeLayer.IsNegotiating();
	}
}
