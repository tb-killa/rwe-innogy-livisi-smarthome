using System;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public class SslCertificateValidationEventArgs : EventArgs
{
	private TlsSocket ejppq;

	private string azoge;

	private CertificateChain bfhby;

	private ValidationOptions aaesw;

	private TlsCertificateAcceptance? xctzi;

	public TlsSocket Socket
	{
		get
		{
			return ejppq;
		}
		private set
		{
			ejppq = value;
		}
	}

	public string ServerName
	{
		get
		{
			return azoge;
		}
		private set
		{
			azoge = value;
		}
	}

	public CertificateChain CertificateChain
	{
		get
		{
			return bfhby;
		}
		private set
		{
			bfhby = value;
		}
	}

	public Certificate Certificate => CertificateChain.LeafCertificate;

	public ValidationOptions Options
	{
		get
		{
			return aaesw;
		}
		internal set
		{
			aaesw = value;
		}
	}

	protected TlsCertificateAcceptance? Result
	{
		get
		{
			return xctzi;
		}
		private set
		{
			xctzi = value;
		}
	}

	public void Accept()
	{
		if (!Result.HasValue || 1 == 0)
		{
			Result = TlsCertificateAcceptance.Accept;
		}
	}

	public void Reject()
	{
		Reject(TlsCertificateAcceptance.Other);
	}

	public void Reject(TlsCertificateAcceptance reason)
	{
		if (reason == TlsCertificateAcceptance.Accept || 1 == 0)
		{
			throw hifyx.nztrs("reason", reason, "Invalid rejection reason.");
		}
		Result = reason;
	}

	public void Reject(ValidationStatus status)
	{
		if (status == (ValidationStatus)0L)
		{
			throw hifyx.nztrs("status", status, "Invalid rejection status.");
		}
		Result = CertificateVerifier.syoyk(status);
	}

	protected SslCertificateValidationEventArgs(TlsSocket socket, string serverName, CertificateChain certificateChain)
	{
		if (socket == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		if (certificateChain == null || 1 == 0)
		{
			throw new ArgumentNullException("certificateChain");
		}
		Socket = socket;
		ServerName = serverName;
		CertificateChain = certificateChain;
	}
}
