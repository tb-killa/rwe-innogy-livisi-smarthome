using System;
using System.Net.Sockets;
using onrkn;

namespace Rebex.Net;

public class SmtpException : NetworkSessionException
{
	private const string xkylg = "SmtpStatus";

	private const string zqeiz = "Response";

	private const string spryf = "Rejected";

	public new SmtpExceptionStatus Status
	{
		get
		{
			object obj = base.Data["SmtpStatus"];
			if (obj is SmtpExceptionStatus && 0 == 0)
			{
				return (SmtpExceptionStatus)obj;
			}
			return SmtpExceptionStatus.UnclassifiableError;
		}
		private set
		{
			base.Data["SmtpStatus"] = value;
			pgxrn();
		}
	}

	public SmtpResponse Response
	{
		get
		{
			return base.Data["Response"] as SmtpResponse;
		}
		private set
		{
			base.Data["Response"] = value;
		}
	}

	private SmtpRejectedRecipient[] zzkug
	{
		get
		{
			return base.Data["Rejected"] as SmtpRejectedRecipient[];
		}
		set
		{
			base.Data["Rejected"] = value;
		}
	}

	public SmtpRejectedRecipient[] GetRejectedRecipients()
	{
		if (zzkug == null || false || zzkug.Length == 0 || 1 == 0)
		{
			return new SmtpRejectedRecipient[0];
		}
		return (SmtpRejectedRecipient[])zzkug.Clone();
	}

	public SmtpException()
		: this("Unclassifiable SMTP error.", null, SmtpExceptionStatus.UnclassifiableError)
	{
	}

	public SmtpException(string message)
		: this(message, null, SmtpExceptionStatus.UnclassifiableError)
	{
	}

	public SmtpException(string message, SmtpExceptionStatus status)
		: this(message, null, status)
	{
	}

	public SmtpException(string message, SmtpExceptionStatus status, SmtpRejectedRecipient[] rejected)
		: this(message, null, status)
	{
		zzkug = rejected;
	}

	internal SmtpException(SmtpException e)
		: base(e.Message, e)
	{
		Status = e.Status;
		Response = e.Response;
		zzkug = e.zzkug;
		base.Status = ((NetworkSessionException)e).Status;
	}

	public SmtpException(string message, Exception innerException)
		: this(message, innerException, SmtpExceptionStatus.UnclassifiableError)
	{
	}

	public SmtpException(string message, Exception innerException, SmtpExceptionStatus status)
		: base(message, innerException)
	{
		if (status == SmtpExceptionStatus.ProtocolError)
		{
			Status = SmtpExceptionStatus.UnclassifiableError;
		}
		else
		{
			Status = status;
		}
	}

	public SmtpException(SmtpResponse response)
		: base((response == null) ? "" : (response.Description.Trim(' ', '.') + " (" + response.Code + ")."))
	{
		if (response == null || 1 == 0)
		{
			throw new ArgumentNullException("response");
		}
		Response = response;
		Status = SmtpExceptionStatus.ProtocolError;
		pgxrn();
	}

	internal SmtpException(SocketException innerException, SmtpExceptionStatus status)
		: this(innerException.astkw(), innerException, status)
	{
	}

	private void pgxrn()
	{
		NetworkSessionExceptionStatus networkSessionExceptionStatus;
		switch (Status)
		{
		case SmtpExceptionStatus.ConnectFailure:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ConnectFailure;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.ConnectionClosed;
		case SmtpExceptionStatus.ConnectionClosed:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ConnectionClosed;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.SocketError;
		case SmtpExceptionStatus.SocketError:
		case SmtpExceptionStatus.ReceiveFailure:
		case SmtpExceptionStatus.SendFailure:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.SocketError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.NameResolutionFailure;
		case SmtpExceptionStatus.NameResolutionFailure:
		case SmtpExceptionStatus.ProxyNameResolutionFailure:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.NameResolutionFailure;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.Pending;
		case SmtpExceptionStatus.Pending:
		case SmtpExceptionStatus.OperationFailure:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.OperationFailure;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.ProtocolError;
		case SmtpExceptionStatus.ProtocolError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ProtocolError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.OperationAborted;
		case SmtpExceptionStatus.OperationAborted:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.OperationAborted;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.UnclassifiableError;
		case SmtpExceptionStatus.UnclassifiableError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.UnclassifiableError;
			if (networkSessionExceptionStatus == NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.ServerProtocolViolation;
		case SmtpExceptionStatus.ServerProtocolViolation:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ServerProtocolViolation;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.Timeout;
		case SmtpExceptionStatus.Timeout:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.Timeout;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SmtpExceptionStatus.AsyncError;
		case SmtpExceptionStatus.AsyncError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.AsyncError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto default;
		default:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.UnclassifiableError;
			break;
		}
		base.Status = networkSessionExceptionStatus;
	}
}
