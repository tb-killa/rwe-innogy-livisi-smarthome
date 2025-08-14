using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Org.Mentalis.Security.Ssl.Shared;

namespace Org.Mentalis.Security.Ssl;

public class SecureNetworkStream : Stream
{
	private readonly bool m_CanRead;

	private readonly bool m_CanWrite;

	private readonly bool m_OwnsSocket;

	private readonly SecureSocket m_Socket;

	public IPEndPoint LocalEndPoint => (IPEndPoint)m_Socket.LocalEndPoint;

	public IPEndPoint RemoteEndPoint => (IPEndPoint)m_Socket.RemoteEndPoint;

	public override bool CanRead => m_CanRead;

	public override bool CanWrite => m_CanWrite;

	public override bool CanSeek => false;

	public override long Length => m_Socket.Available;

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	protected SecureSocket Socket => m_Socket;

	internal TransferItem WriteResult { get; set; }

	public virtual bool DataAvailable
	{
		get
		{
			if (Socket == null)
			{
				return false;
			}
			try
			{
				return Socket.Available > 0;
			}
			catch
			{
				return false;
			}
		}
	}

	public SecureNetworkStream(SecureSocket socket)
		: this(socket, FileAccess.ReadWrite, ownsSocket: false)
	{
	}

	public SecureNetworkStream(SecureSocket socket, bool ownsSocket)
		: this(socket, FileAccess.ReadWrite, ownsSocket)
	{
	}

	public SecureNetworkStream(SecureSocket socket, FileAccess access)
		: this(socket, access, ownsSocket: false)
	{
	}

	public SecureNetworkStream(SecureSocket socket, FileAccess access, bool ownsSocket)
	{
		if (socket == null)
		{
			throw new ArgumentNullException();
		}
		if (!socket.Blocking)
		{
			throw new IOException();
		}
		if (!socket.Connected || socket.SocketType != SocketType.Stream)
		{
			throw new ArgumentException();
		}
		m_CanRead = access == FileAccess.Read || access == FileAccess.ReadWrite;
		m_CanWrite = access == FileAccess.Write || access == FileAccess.ReadWrite;
		m_OwnsSocket = ownsSocket;
		m_Socket = socket;
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int size)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset > buffer.Length || size < 0 || size > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (Socket == null)
		{
			throw new IOException();
		}
		try
		{
			return Socket.Receive(buffer, offset, size, SocketFlags.None);
		}
		catch (Exception innerException)
		{
			throw new IOException("An I/O exception occurred.", innerException);
		}
	}

	public override void Write(byte[] buffer, int offset, int size)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset > buffer.Length || size < 0 || size > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (Socket == null)
		{
			throw new IOException();
		}
		try
		{
			Socket.Send(buffer, offset, size, SocketFlags.None);
		}
		catch (Exception innerException)
		{
			throw new IOException("An I/O exception occurred.", innerException);
		}
	}

	public void ChangeSecurityProtocol(SecurityOptions options)
	{
		Socket.ChangeSecurityProtocol(options);
	}

	public override void Close()
	{
		if (m_OwnsSocket)
		{
			InternalClose();
		}
	}

	public void CloseForcefully()
	{
		InternalClose();
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset > buffer.Length || size < 0 || size > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (Socket == null)
		{
			throw new IOException();
		}
		try
		{
			return Socket.BeginReceive(buffer, offset, size, SocketFlags.None, callback, state);
		}
		catch (Exception innerException)
		{
			throw new IOException("An I/O exception occurred.", innerException);
		}
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException();
		}
		if (Socket == null)
		{
			throw new IOException();
		}
		try
		{
			return Socket.EndReceive(asyncResult);
		}
		catch (Exception innerException)
		{
			throw new IOException("An I/O exception occurred.", innerException);
		}
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset > buffer.Length || size < 0 || size > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (Socket == null)
		{
			throw new IOException();
		}
		if (WriteResult != null)
		{
			throw new IOException();
		}
		TransferItem transferItem = (WriteResult = new TransferItem(new byte[size], 0, size, new AsyncResult(callback, state, null), DataType.ApplicationData));
		Array.Copy(buffer, offset, transferItem.Buffer, 0, size);
		try
		{
			Socket.BeginSend(transferItem.Buffer, 0, size, SocketFlags.None, OnBytesSent, 0);
			return transferItem.AsyncResult;
		}
		catch
		{
			throw new IOException();
		}
	}

	private void OnBytesSent(IAsyncResult asyncResult)
	{
		try
		{
			int num = Socket.EndSend(asyncResult);
			num += (int)asyncResult.AsyncState;
			if (num == WriteResult.Buffer.Length)
			{
				OnWriteComplete(null);
			}
			else
			{
				Socket.BeginSend(WriteResult.Buffer, num, WriteResult.Buffer.Length - num, SocketFlags.None, OnBytesSent, num);
			}
		}
		catch (Exception e)
		{
			OnWriteComplete(e);
		}
	}

	private void OnWriteComplete(Exception e)
	{
		if (WriteResult.AsyncResult != null)
		{
			WriteResult.AsyncResult.AsyncException = e;
			WriteResult.AsyncResult.Notify();
		}
	}

	private void InternalClose()
	{
		try
		{
			Socket.Shutdown(SocketShutdown.Both);
		}
		catch
		{
		}
		finally
		{
			Socket.Close();
		}
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException();
		}
		if (asyncResult != WriteResult.AsyncResult)
		{
			throw new ArgumentException();
		}
		if (Socket == null)
		{
			throw new IOException();
		}
		WriteResult = null;
		if (((AsyncResult)asyncResult).AsyncException != null)
		{
			throw new IOException();
		}
	}
}
