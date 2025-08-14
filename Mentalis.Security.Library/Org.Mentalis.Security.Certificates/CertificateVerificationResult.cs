using System;
using System.Threading;

namespace Org.Mentalis.Security.Certificates;

internal class CertificateVerificationResult : IAsyncResult
{
	private readonly object m_AsyncState;

	private readonly AsyncCallback m_Callback;

	private readonly CertificateChain m_Chain;

	private readonly VerificationFlags m_Flags;

	private readonly string m_Server;

	private readonly AuthType m_Type;

	private bool m_IsCompleted;

	private CertificateStatus m_Status;

	private Exception m_ThrowException;

	private ManualResetEvent m_WaitHandle;

	public CertificateChain Chain => m_Chain;

	public string Server => m_Server;

	public AuthType Type => m_Type;

	public VerificationFlags Flags => m_Flags;

	public bool HasEnded { get; set; }

	public Exception ThrowException => m_ThrowException;

	public CertificateStatus Status => m_Status;

	public bool CompletedSynchronously => false;

	public bool IsCompleted => m_IsCompleted;

	public WaitHandle AsyncWaitHandle
	{
		get
		{
			if (m_WaitHandle == null)
			{
				m_WaitHandle = new ManualResetEvent(initialState: false);
			}
			return m_WaitHandle;
		}
	}

	public object AsyncState => m_AsyncState;

	public CertificateVerificationResult(CertificateChain chain, string server, AuthType type, VerificationFlags flags, AsyncCallback callback, object asyncState)
	{
		m_Chain = chain;
		m_Server = server;
		m_Type = type;
		m_Flags = flags;
		m_AsyncState = asyncState;
		m_Callback = callback;
		m_WaitHandle = null;
		HasEnded = false;
	}

	internal void VerificationCompleted(Exception error, CertificateStatus status)
	{
		m_ThrowException = error;
		m_Status = status;
		m_IsCompleted = true;
		if (m_Callback != null)
		{
			m_Callback(this);
		}
		if (m_WaitHandle != null)
		{
			m_WaitHandle.Set();
		}
	}
}
