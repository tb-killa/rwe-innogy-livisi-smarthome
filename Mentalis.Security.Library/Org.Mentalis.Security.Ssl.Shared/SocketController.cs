using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl.Shared;

internal class SocketController : IDisposable
{
	private const int m_ReceiveBufferLength = 4096;

	private TransferItem m_ActiveReceive;

	private TransferItem m_ActiveSend;

	private CompatibilityLayer m_Compatibility;

	private XBuffer m_DecryptedBuffer;

	private bool m_IsDisposed;

	private bool m_IsSending;

	private bool m_IsShuttingDown;

	private SecureSocket m_Parent;

	private byte[] m_ReceiveBuffer;

	private RecordLayer m_RecordLayer;

	private ArrayList m_SentList;

	private AsyncResult m_ShutdownCallback;

	private Socket m_Socket;

	private ArrayList m_ToSendList;

	public int Available
	{
		get
		{
			lock (this)
			{
				return (int)m_DecryptedBuffer.Length;
			}
		}
	}

	public SecureSocket Parent => m_Parent;

	public SslAlgorithms ActiveEncryption
	{
		get
		{
			if (m_RecordLayer != null)
			{
				return m_RecordLayer.ActiveEncryption;
			}
			return SslAlgorithms.NONE;
		}
	}

	public Certificate RemoteCertificate => m_RecordLayer.RemoteCertificate;

	public SocketController(SecureSocket parent, Socket socket, SecurityOptions options)
	{
		m_Parent = parent;
		m_Socket = socket;
		m_IsDisposed = false;
		m_ActiveSend = null;
		m_ActiveReceive = null;
		m_DecryptedBuffer = new XBuffer();
		m_ToSendList = new ArrayList(2);
		m_SentList = new ArrayList(2);
		m_ReceiveBuffer = new byte[4096];
		m_Compatibility = new CompatibilityLayer(this, options);
		try
		{
			m_Socket.BeginReceive(m_ReceiveBuffer, 0, 4096, SocketFlags.None, OnReceive, null);
		}
		catch (Exception e)
		{
			CloseConnection(e);
		}
		if (options.Entity == ConnectionEnd.Client)
		{
			byte[] clientHello = m_Compatibility.GetClientHello();
			BeginSend(clientHello, 0, clientHello.Length, null, DataType.ProtocolData);
		}
	}

	public void Dispose()
	{
		lock (this)
		{
			CloseConnection(null);
		}
	}

	protected void OnReceive(IAsyncResult ar)
	{
		while (Available > 16384)
		{
			Thread.Sleep(100);
		}
		lock (this)
		{
			try
			{
				int num = m_Socket.EndReceive(ar);
				if (num == 0)
				{
					CloseConnection(null);
					return;
				}
				SslRecordStatus sslRecordStatus;
				if (m_RecordLayer == null)
				{
					CompatibilityResult compatibilityResult = m_Compatibility.ProcessHello(m_ReceiveBuffer, 0, num);
					m_RecordLayer = compatibilityResult.RecordLayer;
					sslRecordStatus = compatibilityResult.Status;
					if (m_RecordLayer != null)
					{
						m_Compatibility = null;
					}
				}
				else
				{
					sslRecordStatus = m_RecordLayer.ProcessBytes(m_ReceiveBuffer, 0, num);
				}
				if (sslRecordStatus.Buffer != null)
				{
					if (sslRecordStatus.Status == SslStatus.Close)
					{
						m_IsShuttingDown = true;
					}
					BeginSend(sslRecordStatus.Buffer, 0, sslRecordStatus.Buffer.Length, null, DataType.ProtocolData);
				}
				else if (sslRecordStatus.Status == SslStatus.Close)
				{
					m_Socket.Shutdown(SocketShutdown.Both);
					CloseConnection(null);
				}
				else if (sslRecordStatus.Status == SslStatus.OK)
				{
					ResumeSending();
				}
				if (sslRecordStatus.Decrypted != null)
				{
					ProcessDecryptedBytes(sslRecordStatus.Decrypted);
				}
				if (!m_IsDisposed && !m_IsShuttingDown)
				{
					m_Socket.BeginReceive(m_ReceiveBuffer, 0, 4096, SocketFlags.None, OnReceive, null);
				}
			}
			catch (Exception e)
			{
				CloseConnection(e);
			}
		}
	}

	protected void OnSent(IAsyncResult ar)
	{
		lock (this)
		{
			try
			{
				if (m_IsDisposed)
				{
					return;
				}
				int num = m_Socket.EndSend(ar);
				m_ActiveSend.Transferred += num;
				if (m_ActiveSend.Transferred != m_ActiveSend.Size)
				{
					m_Socket.BeginSend(m_ActiveSend.Buffer, m_ActiveSend.Offset + m_ActiveSend.Transferred, m_ActiveSend.Size - m_ActiveSend.Transferred, SocketFlags.None, OnSent, null);
					return;
				}
				m_IsSending = false;
				if (m_ActiveSend.AsyncResult != null)
				{
					m_SentList.Add(m_ActiveSend);
					m_ActiveSend.AsyncResult.Notify(null);
				}
				if (m_IsShuttingDown && (m_ToSendList.Count == 0 || ((TransferItem)m_ToSendList[0]).Type == DataType.ApplicationData))
				{
					CloseConnection(null);
				}
				else
				{
					ResumeSending();
				}
			}
			catch (Exception e)
			{
				CloseConnection(e);
			}
		}
	}

	public AsyncResult BeginSend(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
	{
		lock (this)
		{
			if (m_IsDisposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (m_IsShuttingDown)
			{
				throw new SocketException();
			}
			return BeginSend(buffer, offset, size, new AsyncResult(callback, state, this), DataType.ApplicationData);
		}
	}

	protected AsyncResult BeginSend(byte[] buffer, int offset, int size, AsyncResult asyncResult, DataType type)
	{
		int index = m_ToSendList.Count;
		if (type == DataType.ProtocolData)
		{
			index = 0;
		}
		TransferItem transferItem = new TransferItem(buffer, offset, size, asyncResult, type);
		m_ToSendList.Insert(index, transferItem);
		ResumeSending();
		return transferItem.AsyncResult;
	}

	protected int FindIndex(IAsyncResult ar, ArrayList list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (((TransferItem)list[i]).AsyncResult == ar)
			{
				return i;
			}
		}
		return -1;
	}

	public TransferItem EndSend(IAsyncResult ar)
	{
		TransferItem transferItem;
		lock (this)
		{
			int num = FindIndex(ar, m_SentList);
			if (num < 0)
			{
				if (m_ActiveSend != null && m_ActiveSend.AsyncResult == ar)
				{
					transferItem = m_ActiveSend;
				}
				else
				{
					num = FindIndex(ar, m_ToSendList);
					if (num < 0)
					{
						return null;
					}
					transferItem = (TransferItem)m_ToSendList[num];
				}
			}
			else
			{
				transferItem = (TransferItem)m_SentList[num];
			}
		}
		while (!transferItem.AsyncResult.IsCompleted)
		{
			transferItem.AsyncResult.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		lock (this)
		{
			m_SentList.Remove(transferItem);
			return transferItem;
		}
	}

	protected void ProcessDecryptedBytes(byte[] buffer)
	{
		if (buffer != null)
		{
			m_DecryptedBuffer.Seek(0L, SeekOrigin.End);
			m_DecryptedBuffer.Write(buffer, 0, buffer.Length);
		}
		if (m_ActiveReceive != null && m_ActiveReceive.Transferred == 0 && (m_ActiveReceive.AsyncResult == null || !m_ActiveReceive.AsyncResult.IsCompleted))
		{
			if (m_DecryptedBuffer.Length > m_ActiveReceive.Size)
			{
				m_DecryptedBuffer.Seek(0L, SeekOrigin.Begin);
				m_DecryptedBuffer.Read(m_ActiveReceive.Buffer, m_ActiveReceive.Offset, m_ActiveReceive.Size);
				m_DecryptedBuffer.RemoveXBytes(m_ActiveReceive.Size);
				m_ActiveReceive.Transferred = m_ActiveReceive.Size;
			}
			else
			{
				m_DecryptedBuffer.Seek(0L, SeekOrigin.Begin);
				m_DecryptedBuffer.Read(m_ActiveReceive.Buffer, m_ActiveReceive.Offset, (int)m_DecryptedBuffer.Length);
				m_ActiveReceive.Transferred = (int)m_DecryptedBuffer.Length;
				m_DecryptedBuffer.SetLength(0L);
			}
			if (m_ActiveReceive.AsyncResult != null)
			{
				m_ActiveReceive.AsyncResult.Notify(null);
			}
		}
	}

	public AsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
	{
		lock (this)
		{
			if (m_ActiveReceive != null)
			{
				throw new SocketException();
			}
			AsyncResult asyncResult = new AsyncResult(callback, state, this);
			m_ActiveReceive = new TransferItem(buffer, offset, size, asyncResult, DataType.ApplicationData);
			if (m_DecryptedBuffer.Length > 0)
			{
				ProcessDecryptedBytes(null);
			}
			else if (!m_Socket.Connected && m_ActiveReceive.AsyncResult != null)
			{
				m_ActiveReceive.AsyncResult.Notify(null);
			}
			return asyncResult;
		}
	}

	public TransferItem EndReceive(IAsyncResult ar)
	{
		TransferItem activeReceive;
		lock (this)
		{
			if (ar != m_ActiveReceive.AsyncResult)
			{
				return null;
			}
			activeReceive = m_ActiveReceive;
		}
		while (!activeReceive.AsyncResult.IsCompleted)
		{
			activeReceive.AsyncResult.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		lock (this)
		{
			m_ActiveReceive = null;
			return activeReceive;
		}
	}

	protected byte[] AppendBytes(byte[] buffer1, int offset1, int size1, byte[] buffer2, int offset2, int size2)
	{
		byte[] array = new byte[size1 + size2];
		Array.Copy(buffer1, offset1, array, 0, size1);
		Array.Copy(buffer2, offset2, array, size1, size2);
		return array;
	}

	protected byte[] SplitBytes(ref byte[] buffer, int maxReturnLength)
	{
		if (buffer.Length < maxReturnLength)
		{
			maxReturnLength = buffer.Length;
		}
		byte[] array = new byte[maxReturnLength];
		Array.Copy(buffer, 0, array, 0, maxReturnLength);
		byte[] array2 = new byte[buffer.Length - maxReturnLength];
		if (array2.Length > 0)
		{
			Array.Copy(buffer, maxReturnLength, array2, 0, array2.Length);
		}
		buffer = array2;
		return array;
	}

	protected void ResumeSending()
	{
		if (m_IsSending || m_ToSendList.Count == 0 || (m_RecordLayer == null && ((TransferItem)m_ToSendList[0]).Type == DataType.ApplicationData) || (m_RecordLayer != null && m_RecordLayer.IsNegotiating() && ((TransferItem)m_ToSendList[0]).Type == DataType.ApplicationData))
		{
			return;
		}
		m_ActiveSend = (TransferItem)m_ToSendList[0];
		m_ToSendList.RemoveAt(0);
		m_IsSending = true;
		try
		{
			if (m_ActiveSend.Type == DataType.ApplicationData)
			{
				m_ActiveSend.Buffer = m_RecordLayer.EncryptBytes(m_ActiveSend.Buffer, m_ActiveSend.Offset, m_ActiveSend.Size, ContentType.ApplicationData);
				m_ActiveSend.Offset = 0;
				m_ActiveSend.Size = m_ActiveSend.Buffer.Length;
			}
			m_Socket.BeginSend(m_ActiveSend.Buffer, m_ActiveSend.Offset, m_ActiveSend.Size, SocketFlags.None, OnSent, null);
		}
		catch (Exception e)
		{
			CloseConnection(e);
		}
	}

	protected void CloseConnection(Exception e)
	{
		if (!m_IsDisposed)
		{
			m_IsDisposed = true;
			try
			{
				m_Socket.Shutdown(SocketShutdown.Both);
			}
			catch
			{
			}
			m_Socket.Close();
			if (m_ActiveSend != null && m_ActiveSend.AsyncResult != null)
			{
				m_SentList.Add(m_ActiveSend);
				m_ActiveSend.AsyncResult.Notify(e);
			}
			Exception ex = e;
			if (ex == null)
			{
				ex = new SslException(AlertDescription.UnexpectedMessage, "The bytes could not be sent because the connection has been closed.");
			}
			for (int i = 0; i < m_ToSendList.Count; i++)
			{
				m_ActiveSend = (TransferItem)m_ToSendList[i];
				m_SentList.Add(m_ActiveSend);
				m_ActiveSend.AsyncResult.Notify(ex);
			}
			m_ToSendList.Clear();
			if (m_ActiveReceive != null && m_ActiveReceive.AsyncResult != null)
			{
				m_ActiveReceive.AsyncResult.Notify(e);
			}
			if (m_ShutdownCallback != null)
			{
				m_ShutdownCallback.Notify(e);
			}
			if (m_RecordLayer != null)
			{
				m_RecordLayer.Dispose();
			}
		}
	}

	private void OnShutdownSent(IAsyncResult ar)
	{
		lock (this)
		{
			int num = FindIndex(ar, m_SentList);
			if (num < 0)
			{
				return;
			}
			m_SentList.RemoveAt(num);
			try
			{
				if (!m_IsDisposed)
				{
					m_Socket.Shutdown(SocketShutdown.Send);
				}
			}
			catch
			{
			}
			if (m_ShutdownCallback != null)
			{
				AsyncResult shutdownCallback = m_ShutdownCallback;
				m_ShutdownCallback = null;
				shutdownCallback.Notify(null);
			}
		}
	}

	public AsyncResult BeginShutdown(AsyncCallback callback, object state)
	{
		lock (this)
		{
			AsyncResult asyncResult = new AsyncResult(callback, state, this);
			byte[] controlBytes = m_RecordLayer.GetControlBytes(ControlType.Shutdown);
			m_ShutdownCallback = asyncResult;
			if (m_IsDisposed)
			{
				asyncResult.Notify(null);
			}
			else
			{
				BeginSend(controlBytes, 0, controlBytes.Length, new AsyncResult(OnShutdownSent, null, this), DataType.ProtocolData);
			}
			return asyncResult;
		}
	}

	public AsyncResult EndShutdown(IAsyncResult ar)
	{
		AsyncResult shutdownCallback;
		lock (this)
		{
			if (m_ShutdownCallback == null)
			{
				return null;
			}
			shutdownCallback = m_ShutdownCallback;
			m_ShutdownCallback = null;
		}
		while (!shutdownCallback.IsCompleted)
		{
			shutdownCallback.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		return shutdownCallback;
	}

	public void QueueRenegotiate()
	{
		lock (this)
		{
			byte[] controlBytes = m_RecordLayer.GetControlBytes(ControlType.Renegotiate);
			if (controlBytes != null)
			{
				BeginSend(controlBytes, 0, controlBytes.Length, null, DataType.ProtocolData);
			}
		}
	}
}
