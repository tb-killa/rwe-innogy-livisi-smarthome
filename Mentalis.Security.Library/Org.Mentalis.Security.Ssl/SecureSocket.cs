using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Org.Mentalis.Security.Certificates;
using Org.Mentalis.Security.Ssl.Shared;

namespace Org.Mentalis.Security.Ssl;

public class SecureSocket : VirtualSocket
{
	private AsyncAcceptResult m_AcceptResult;

	private AsyncResult m_ConnectResult;

	private SocketController m_Controller;

	private bool m_IsDisposed;

	private SecurityOptions m_Options;

	private bool m_SentShutdownNotification;

	private AsyncResult m_ShutdownResult;

	public override int Available
	{
		get
		{
			if (SecureProtocol == SecureProtocol.None)
			{
				return base.Available;
			}
			if (m_IsDisposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return m_Controller.Available;
		}
	}

	public Certificate LocalCertificate => m_Options.Certificate;

	public Certificate RemoteCertificate
	{
		get
		{
			if (m_Controller == null)
			{
				return null;
			}
			return m_Controller.RemoteCertificate;
		}
	}

	public SecureProtocol SecureProtocol => m_Options.Protocol;

	public ConnectionEnd Entity => m_Options.Entity;

	public string CommonName => m_Options.CommonName;

	public CredentialVerification VerificationType => m_Options.VerificationType;

	public CertVerifyEventHandler Verifier => m_Options.Verifier;

	public SecurityFlags SecurityFlags => m_Options.Flags;

	public SslAlgorithms ActiveEncryption
	{
		get
		{
			if (m_Controller == null)
			{
				return SslAlgorithms.NONE;
			}
			return m_Controller.ActiveEncryption;
		}
	}

	public override bool Blocking
	{
		get
		{
			return base.Blocking;
		}
		set
		{
			if (!value && SecureProtocol != SecureProtocol.None)
			{
				throw new NotSupportedException("Non-blocking sockets are not supported in SSL or TLS mode. Use the asynchronous methods instead.");
			}
			base.Blocking = value;
		}
	}

	public SecureSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
		: this(addressFamily, socketType, protocolType, new SecurityOptions(SecureProtocol.None))
	{
	}

	public SecureSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, SecurityOptions options)
		: base(addressFamily, socketType, protocolType)
	{
		m_SentShutdownNotification = false;
		ChangeSecurityProtocol(options);
	}

	internal SecureSocket(Socket accepted, SecurityOptions options)
		: base(accepted)
	{
		m_SentShutdownNotification = false;
		ChangeSecurityProtocol(options);
	}

	public void ChangeSecurityProtocol(SecurityOptions options)
	{
		if (options == null)
		{
			throw new ArgumentNullException();
		}
		if (m_Options != null && m_Options.Protocol != SecureProtocol.None)
		{
			throw new ArgumentException("Only changing from a normal connection to a secure connection is supported.");
		}
		if (base.ProtocolType != ProtocolType.Tcp && options.Protocol != SecureProtocol.None)
		{
			throw new SecurityException("Security protocols require underlying TCP connections!");
		}
		if (options.Protocol != SecureProtocol.None)
		{
			if (options.Entity == ConnectionEnd.Server && options.Certificate == null)
			{
				throw new ArgumentException("The certificate cannot be set to a null reference when creating a server socket.");
			}
			if (options.Certificate != null && !options.Certificate.HasPrivateKey())
			{
				throw new ArgumentException("If a certificate is specified, it must have a private key.");
			}
			if ((options.AllowedAlgorithms & SslAlgorithms.NULL_COMPRESSION) == 0)
			{
				throw new ArgumentException("The allowed algorithms field must contain at least one compression algorithm.");
			}
			if ((options.AllowedAlgorithms ^ SslAlgorithms.NULL_COMPRESSION) == SslAlgorithms.NONE)
			{
				throw new ArgumentException("The allowed algorithms field must contain at least one cipher suite.");
			}
			if (options.VerificationType == CredentialVerification.Manual && options.Verifier == null)
			{
				throw new ArgumentException("A CertVerifyEventHandler is required when using manual certificate verification.");
			}
		}
		m_Options = (SecurityOptions)options.Clone();
		if (options.Protocol != SecureProtocol.None && Connected)
		{
			m_Controller = new SocketController(this, base.InternalSocket, options);
		}
	}

	public override void Connect(EndPoint remoteEP)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			base.Connect(remoteEP);
		}
		else
		{
			EndConnect(BeginConnect(remoteEP, null, null));
		}
	}

	public override IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			return base.BeginConnect(remoteEP, callback, state);
		}
		if (remoteEP == null)
		{
			throw new ArgumentNullException();
		}
		if (m_ConnectResult != null)
		{
			throw new SocketException();
		}
		AsyncResult result = (m_ConnectResult = new AsyncResult(callback, state, null));
		base.BeginConnect(remoteEP, OnConnect, null);
		return result;
	}

	private void OnConnect(IAsyncResult ar)
	{
		try
		{
			base.EndConnect(ar);
			m_Controller = new SocketController(this, base.InternalSocket, m_Options);
		}
		catch (Exception asyncException)
		{
			m_ConnectResult.AsyncException = asyncException;
		}
		m_ConnectResult.Notify();
	}

	public override void EndConnect(IAsyncResult asyncResult)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			base.EndConnect(asyncResult);
			return;
		}
		if (asyncResult == null)
		{
			throw new ArgumentNullException();
		}
		if (m_ConnectResult == null)
		{
			throw new InvalidOperationException();
		}
		if (asyncResult != m_ConnectResult)
		{
			throw new ArgumentException();
		}
		AsyncResult connectResult = m_ConnectResult;
		while (!connectResult.IsCompleted)
		{
			connectResult.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		m_ConnectResult = null;
		if (connectResult.AsyncException == null)
		{
			return;
		}
		throw connectResult.AsyncException;
	}

	public override VirtualSocket Accept()
	{
		return EndAccept(BeginAccept(null, null));
	}

	public override IAsyncResult BeginAccept(AsyncCallback callback, object state)
	{
		if (m_AcceptResult != null)
		{
			throw new SocketException();
		}
		AsyncAcceptResult result = (m_AcceptResult = new AsyncAcceptResult(callback, state, null));
		base.BeginAccept(OnAccept, null);
		return result;
	}

	private void OnAccept(IAsyncResult ar)
	{
		try
		{
			m_AcceptResult.AcceptedSocket = new SecureSocket(base.InternalEndAccept(ar), m_Options);
		}
		catch (Exception asyncException)
		{
			m_AcceptResult.AsyncException = asyncException;
		}
		m_AcceptResult.Notify();
	}

	public override VirtualSocket EndAccept(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException();
		}
		if (m_AcceptResult == null)
		{
			throw new InvalidOperationException();
		}
		if (m_AcceptResult != asyncResult)
		{
			throw new ArgumentException();
		}
		AsyncAcceptResult acceptResult = m_AcceptResult;
		while (!acceptResult.IsCompleted)
		{
			acceptResult.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		m_AcceptResult = null;
		if (acceptResult.AsyncException != null)
		{
			throw acceptResult.AsyncException;
		}
		return acceptResult.AcceptedSocket;
	}

	public override int Send(byte[] buffer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		return Send(buffer, 0, buffer.Length, SocketFlags.None);
	}

	public override int Send(byte[] buffer, SocketFlags socketFlags)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		return Send(buffer, 0, buffer.Length, socketFlags);
	}

	public override int Send(byte[] buffer, int size, SocketFlags socketFlags)
	{
		return Send(buffer, 0, size, socketFlags);
	}

	public override int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			return base.Send(buffer, offset, size, socketFlags);
		}
		return EndSend(BeginSend(buffer, offset, size, socketFlags, null, null));
	}

	public override IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			return base.BeginSend(buffer, offset, size, socketFlags, callback, state);
		}
		if (!Connected)
		{
			throw new SocketException();
		}
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (size == 0)
		{
			throw new ArgumentException();
		}
		if (offset < 0 || offset >= buffer.Length || size > buffer.Length - offset || size < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		return m_Controller.BeginSend(buffer, offset, size, callback, state);
	}

	public override int EndSend(IAsyncResult asyncResult)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			return base.EndSend(asyncResult);
		}
		if (asyncResult == null)
		{
			throw new ArgumentNullException();
		}
		TransferItem transferItem = m_Controller.EndSend(asyncResult);
		if (transferItem == null)
		{
			throw new ArgumentException();
		}
		while (!transferItem.AsyncResult.IsCompleted)
		{
			transferItem.AsyncResult.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		if (transferItem.AsyncResult.AsyncException != null)
		{
			throw new SecurityException("An error occurs while communicating with the remote host.", transferItem.AsyncResult.AsyncException);
		}
		return transferItem.OriginalSize;
	}

	public override int Receive(byte[] buffer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		return Receive(buffer, 0, buffer.Length, SocketFlags.None);
	}

	public override int Receive(byte[] buffer, SocketFlags socketFlags)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		return Receive(buffer, 0, buffer.Length, socketFlags);
	}

	public override int Receive(byte[] buffer, int size, SocketFlags socketFlags)
	{
		return Receive(buffer, 0, size, socketFlags);
	}

	public override int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			return base.Receive(buffer, offset, size, socketFlags);
		}
		return EndReceive(BeginReceive(buffer, offset, size, socketFlags, null, null));
	}

	public override IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			return base.BeginReceive(buffer, offset, size, socketFlags, callback, state);
		}
		if (!Connected && m_SentShutdownNotification)
		{
			throw new SocketException();
		}
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || (offset >= buffer.Length && size != 0) || size > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException();
		}
		return m_Controller.BeginReceive(buffer, offset, size, callback, state);
	}

	public override int EndReceive(IAsyncResult asyncResult)
	{
		if (SecureProtocol == SecureProtocol.None)
		{
			return base.EndReceive(asyncResult);
		}
		if (asyncResult == null)
		{
			throw new ArgumentNullException();
		}
		TransferItem transferItem = m_Controller.EndReceive(asyncResult);
		if (transferItem == null)
		{
			throw new ArgumentException();
		}
		while (!transferItem.AsyncResult.IsCompleted)
		{
			transferItem.AsyncResult.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		if (transferItem.AsyncResult.AsyncException != null)
		{
			throw new SecurityException("An error occurs while communicating with the remote host.\r\n" + transferItem.AsyncResult.AsyncException.ToString(), transferItem.AsyncResult.AsyncException);
		}
		if (transferItem.Transferred == 0)
		{
			m_SentShutdownNotification = true;
		}
		return transferItem.Transferred;
	}

	public override void Shutdown(SocketShutdown how)
	{
		EndShutdown(BeginShutdown(null, null));
	}

	public IAsyncResult BeginShutdown(AsyncCallback callback, object state)
	{
		if (m_ShutdownResult != null)
		{
			throw new InvalidOperationException();
		}
		AsyncResult asyncResult = (m_ShutdownResult = new AsyncResult(callback, state, null));
		if (!Connected)
		{
			asyncResult.Notify(null);
		}
		else if (SecureProtocol == SecureProtocol.None)
		{
			base.Shutdown(SocketShutdown.Both);
			asyncResult.Notify(null);
		}
		else
		{
			m_Controller.BeginShutdown(OnShutdown, null);
		}
		return asyncResult;
	}

	private void OnShutdown(IAsyncResult ar)
	{
		try
		{
			m_Controller.EndShutdown(ar);
		}
		catch
		{
		}
		m_ShutdownResult.Notify();
	}

	public void EndShutdown(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException();
		}
		if (m_ShutdownResult == null)
		{
			throw new InvalidOperationException();
		}
		if (asyncResult != m_ShutdownResult)
		{
			throw new ArgumentException();
		}
		AsyncResult shutdownResult = m_ShutdownResult;
		while (!shutdownResult.IsCompleted)
		{
			shutdownResult.AsyncWaitHandle.WaitOne(200, exitContext: false);
		}
		m_ShutdownResult = null;
	}

	public void QueueRenegotiate()
	{
		if (!Connected)
		{
			throw new SocketException();
		}
		m_Controller.QueueRenegotiate();
	}

	public override void Close()
	{
		base.Close();
		if (!m_IsDisposed)
		{
			if (m_Controller != null)
			{
				m_Controller.Dispose();
			}
			m_IsDisposed = true;
		}
	}

	~SecureSocket()
	{
		Close();
	}

	public override bool Poll(int microSeconds, SelectMode mode)
	{
		if (SecureProtocol != SecureProtocol.None)
		{
			int num = 0;
			int num2 = 10;
			do
			{
				if (Available > 0)
				{
					return true;
				}
				Thread.Sleep(num2);
				num += num2 * 1000;
			}
			while (num < microSeconds);
			return false;
		}
		return base.Poll(microSeconds, mode);
	}
}
