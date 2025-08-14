using System;
using System.Net.Sockets;
using onrkn;

namespace Rebex.Net;

public class ProxySocketException : NetworkSessionException
{
	private const string ruvyd = "ProxyStatus";

	private const string embzi = "ErrorCode";

	public new ProxySocketExceptionStatus Status
	{
		get
		{
			object obj = base.Data["ProxyStatus"];
			if (obj is ProxySocketExceptionStatus && 0 == 0)
			{
				return (ProxySocketExceptionStatus)obj;
			}
			return ProxySocketExceptionStatus.UnclassifiableError;
		}
		set
		{
			base.Data["ProxyStatus"] = value;
			hhdrb();
		}
	}

	public int ErrorCode
	{
		get
		{
			object obj = base.Data["ErrorCode"];
			if (obj is int && 0 == 0)
			{
				return (int)obj;
			}
			return 0;
		}
		set
		{
			base.Data["ErrorCode"] = value;
		}
	}

	internal ProxySocketException(string message, ProxySocketExceptionStatus status, int errorCode, Exception inner)
		: base(message, inner)
	{
		Status = status;
		ErrorCode = errorCode;
	}

	internal ProxySocketException(string message, ProxySocketExceptionStatus status, int errorCode)
		: base(message)
	{
		Status = status;
		ErrorCode = errorCode;
	}

	public ProxySocketException(string message, ProxySocketExceptionStatus status)
		: base(message)
	{
		Status = status;
	}

	public ProxySocketException(string message, ProxySocketExceptionStatus status, Exception innerException)
		: base(message, innerException)
	{
		Status = status;
	}

	public ProxySocketException(SocketException e)
		: this(e.astkw(), kjcuz(e), e)
	{
		ErrorCode = e.skehp();
	}

	internal ProxySocketException(SocketException e, ProxySocketExceptionStatus status)
		: this(e.astkw(), status, e)
	{
		ErrorCode = e.skehp();
	}

	private static ProxySocketExceptionStatus kjcuz(SocketException p0)
	{
		switch (p0.skehp())
		{
		case 10051:
		case 10060:
		case 10061:
		case 10064:
		case 10065:
			return ProxySocketExceptionStatus.ConnectFailure;
		case 10053:
			return ProxySocketExceptionStatus.ConnectionClosed;
		case 10057:
			return ProxySocketExceptionStatus.NotConnected;
		default:
			return ProxySocketExceptionStatus.SocketError;
		}
	}

	public static string GetSocketExceptionMessage(SocketException error)
	{
		return error.astkw();
	}

	private void hhdrb()
	{
		NetworkSessionExceptionStatus networkSessionExceptionStatus;
		switch (Status)
		{
		case ProxySocketExceptionStatus.ConnectFailure:
		case ProxySocketExceptionStatus.NotConnected:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ConnectFailure;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.ConnectionClosed;
		case ProxySocketExceptionStatus.ConnectionClosed:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ConnectionClosed;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.SocketError;
		case ProxySocketExceptionStatus.SocketError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.SocketError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.NameResolutionFailure;
		case ProxySocketExceptionStatus.NameResolutionFailure:
		case ProxySocketExceptionStatus.ProxyNameResolutionFailure:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.NameResolutionFailure;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.ProtocolError;
		case ProxySocketExceptionStatus.ProtocolError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ProtocolError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.ReceiveFailure;
		case ProxySocketExceptionStatus.ReceiveFailure:
		case ProxySocketExceptionStatus.ServerProtocolViolation:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ServerProtocolViolation;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.UnclassifiableError;
		case ProxySocketExceptionStatus.UnclassifiableError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.UnclassifiableError;
			if (networkSessionExceptionStatus == NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.AsyncError;
		case ProxySocketExceptionStatus.AsyncError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.AsyncError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case ProxySocketExceptionStatus.SendRetryTimeout;
		case ProxySocketExceptionStatus.SendRetryTimeout:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.Timeout;
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
