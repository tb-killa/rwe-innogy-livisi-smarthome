using System;
using System.Net;
using System.Net.Sockets;

namespace Org.Mentalis.Security.Ssl;

public class VirtualSocket
{
	protected Socket InternalSocket { get; set; }

	public virtual bool Blocking
	{
		get
		{
			return InternalSocket.Blocking;
		}
		set
		{
			InternalSocket.Blocking = value;
		}
	}

	public virtual AddressFamily AddressFamily => InternalSocket.AddressFamily;

	public virtual int Available => InternalSocket.Available;

	public virtual bool Connected => InternalSocket.Connected;

	public virtual IntPtr Handle => InternalSocket.Handle;

	public virtual EndPoint LocalEndPoint => InternalSocket.LocalEndPoint;

	public virtual ProtocolType ProtocolType => InternalSocket.ProtocolType;

	public virtual EndPoint RemoteEndPoint => InternalSocket.RemoteEndPoint;

	public virtual SocketType SocketType => InternalSocket.SocketType;

	public VirtualSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
	{
		InternalSocket = new Socket(addressFamily, socketType, protocolType);
	}

	protected VirtualSocket(Socket internalSocket)
	{
		if (internalSocket == null)
		{
			throw new ArgumentNullException();
		}
		InternalSocket = internalSocket;
	}

	public virtual VirtualSocket Accept()
	{
		return new VirtualSocket(InternalAccept());
	}

	protected virtual Socket InternalAccept()
	{
		return InternalSocket.Accept();
	}

	public virtual IAsyncResult BeginAccept(AsyncCallback callback, object state)
	{
		return InternalSocket.BeginAccept(callback, state);
	}

	public virtual IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
	{
		return InternalSocket.BeginConnect(remoteEP, callback, state);
	}

	public virtual IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		return InternalSocket.BeginReceive(buffer, offset, size, socketFlags, callback, state);
	}

	public virtual IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
	{
		return InternalSocket.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
	}

	public virtual IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		return InternalSocket.BeginSend(buffer, offset, size, socketFlags, callback, state);
	}

	public virtual IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state)
	{
		return InternalSocket.BeginSendTo(buffer, offset, size, socketFlags, remoteEP, callback, state);
	}

	public virtual void Bind(EndPoint localEP)
	{
		InternalSocket.Bind(localEP);
	}

	public virtual void Close()
	{
		InternalSocket.Close();
	}

	public virtual void Connect(EndPoint remoteEP)
	{
		InternalSocket.Connect(remoteEP);
	}

	public virtual VirtualSocket EndAccept(IAsyncResult asyncResult)
	{
		return new VirtualSocket(InternalEndAccept(asyncResult));
	}

	protected virtual Socket InternalEndAccept(IAsyncResult asyncResult)
	{
		return InternalSocket.EndAccept(asyncResult);
	}

	public virtual void EndConnect(IAsyncResult asyncResult)
	{
		InternalSocket.EndConnect(asyncResult);
	}

	public virtual int EndReceive(IAsyncResult asyncResult)
	{
		return InternalSocket.EndReceive(asyncResult);
	}

	public virtual int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
	{
		return InternalSocket.EndReceiveFrom(asyncResult, ref endPoint);
	}

	public virtual int EndSend(IAsyncResult asyncResult)
	{
		return InternalSocket.EndSend(asyncResult);
	}

	public virtual int EndSendTo(IAsyncResult asyncResult)
	{
		return InternalSocket.EndSendTo(asyncResult);
	}

	public override int GetHashCode()
	{
		return InternalSocket.GetHashCode();
	}

	public virtual object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
	{
		return InternalSocket.GetSocketOption(optionLevel, optionName);
	}

	public virtual void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
	{
		InternalSocket.GetSocketOption(optionLevel, optionName, optionValue);
	}

	public virtual byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
	{
		return InternalSocket.GetSocketOption(optionLevel, optionName, optionLength);
	}

	public virtual int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
	{
		return InternalSocket.IOControl(ioControlCode, optionInValue, optionOutValue);
	}

	public virtual void Listen(int backlog)
	{
		InternalSocket.Listen(backlog);
	}

	public virtual bool Poll(int microSeconds, SelectMode mode)
	{
		return InternalSocket.Poll(microSeconds, mode);
	}

	public virtual int Receive(byte[] buffer)
	{
		return InternalSocket.Receive(buffer);
	}

	public virtual int Receive(byte[] buffer, SocketFlags socketFlags)
	{
		return InternalSocket.Receive(buffer, socketFlags);
	}

	public virtual int Receive(byte[] buffer, int size, SocketFlags socketFlags)
	{
		return InternalSocket.Receive(buffer, size, socketFlags);
	}

	public virtual int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		return InternalSocket.Receive(buffer, offset, size, socketFlags);
	}

	public virtual int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)
	{
		return InternalSocket.ReceiveFrom(buffer, ref remoteEP);
	}

	public virtual int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP)
	{
		return InternalSocket.ReceiveFrom(buffer, socketFlags, ref remoteEP);
	}

	public virtual int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
	{
		return InternalSocket.ReceiveFrom(buffer, size, socketFlags, ref remoteEP);
	}

	public virtual int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
	{
		return InternalSocket.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP);
	}

	public virtual int Send(byte[] buffer)
	{
		return InternalSocket.Send(buffer);
	}

	public virtual int Send(byte[] buffer, SocketFlags socketFlags)
	{
		return InternalSocket.Send(buffer, socketFlags);
	}

	public virtual int Send(byte[] buffer, int size, SocketFlags socketFlags)
	{
		return InternalSocket.Send(buffer, size, socketFlags);
	}

	public virtual int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		return InternalSocket.Send(buffer, offset, size, socketFlags);
	}

	public virtual int SendTo(byte[] buffer, EndPoint remoteEP)
	{
		return InternalSocket.SendTo(buffer, remoteEP);
	}

	public virtual int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP)
	{
		return InternalSocket.SendTo(buffer, socketFlags, remoteEP);
	}

	public virtual int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP)
	{
		return InternalSocket.SendTo(buffer, size, socketFlags, remoteEP);
	}

	public virtual int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP)
	{
		return InternalSocket.SendTo(buffer, offset, size, socketFlags, remoteEP);
	}

	public virtual void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
	{
		InternalSocket.SetSocketOption(optionLevel, optionName, optionValue);
	}

	public virtual void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
	{
		InternalSocket.SetSocketOption(optionLevel, optionName, optionValue);
	}

	public virtual void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
	{
		InternalSocket.SetSocketOption(optionLevel, optionName, optionValue);
	}

	public virtual void Shutdown(SocketShutdown how)
	{
		InternalSocket.Shutdown(how);
	}
}
