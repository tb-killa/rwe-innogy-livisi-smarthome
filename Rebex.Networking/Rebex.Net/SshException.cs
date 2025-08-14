using System;
using System.Collections;
using System.Globalization;
using onrkn;

namespace Rebex.Net;

public class SshException : NetworkSessionException
{
	[NonSerialized]
	private string puguj;

	public new SshExceptionStatus Status
	{
		get
		{
			object obj = base.Data["SshStatus"];
			if (obj is SshExceptionStatus && 0 == 0)
			{
				return (SshExceptionStatus)obj;
			}
			return SshExceptionStatus.UnclassifiableError;
		}
	}

	internal tcpjq kpcdk
	{
		get
		{
			object obj = base.Data["SshReason"];
			if (obj is tcpjq && 0 == 0)
			{
				return (tcpjq)obj;
			}
			return tcpjq.bxwwb;
		}
	}

	internal bool drbjp
	{
		get
		{
			object obj = base.Data["SshRemote"];
			if (obj is bool && 0 == 0)
			{
				return (bool)obj;
			}
			return false;
		}
	}

	internal bool everl
	{
		get
		{
			if (!drbjp || 1 == 0)
			{
				return kpcdk != tcpjq.bxwwb;
			}
			return true;
		}
	}

	internal string nqhfo
	{
		get
		{
			return base.Data["SshCause"] as string;
		}
		set
		{
			base.Data["SshCause"] = value;
		}
	}

	internal string izlra
	{
		get
		{
			return puguj;
		}
		set
		{
			puguj = value;
		}
	}

	public SshServerInfo GetServerInfo()
	{
		return base.Data["SshServerInfo"] as SshServerInfo;
	}

	internal void zgdgc(SshServerInfo p0)
	{
		base.Data["SshServerInfo"] = p0;
	}

	internal string hndyi()
	{
		string text = puguj;
		if (text == null || 1 == 0)
		{
			return Message;
		}
		return brgjd.edcru(text, Message);
	}

	private static string afhxo(tcpjq p0, bool p1, string p2)
	{
		string text = p0 switch
		{
			tcpjq.byugp => "host is not allowed to connect.", 
			tcpjq.svqut => "protocol error.", 
			tcpjq.ziezw => "key exchange failed.", 
			tcpjq.zbwim => "data integrity error.", 
			tcpjq.hmhzl => "compression error.", 
			tcpjq.wyzlr => "service not available.", 
			tcpjq.clcxg => "unsupported protocol version.", 
			tcpjq.jpcem => "host key not verifiable.", 
			tcpjq.jgdcv => "connection lost.", 
			tcpjq.kxdpn => "disconnected by application.", 
			tcpjq.iqqfl => "too many connections.", 
			tcpjq.rlxea => "authentication canceled by user.", 
			tcpjq.llifs => "no suitable authentication method available.", 
			tcpjq.pwqqd => "illegal user name.", 
			_ => brgjd.edcru("error {0} reported.", (int)p0), 
		};
		return brgjd.edcru("Connection has been closed by the {0} connection end; {1} {2}.", (p1 ? true : false) ? "remote" : "local", text, p2);
	}

	internal SshException(tcpjq reason, bool remote, string description)
		: this(reason, remote, afhxo(reason, remote, description), null)
	{
	}

	internal SshException(tcpjq reason, string message)
		: this(reason, remote: false, message, null)
	{
	}

	internal SshException(tcpjq reason, string message, Exception inner)
		: this(reason, remote: false, message, inner)
	{
	}

	internal SshException(tcpjq reason, bool remote, string message, Exception inner)
		: base(message, inner)
	{
		SshExceptionStatus sshExceptionStatus;
		switch (reason)
		{
		case tcpjq.byugp:
		case tcpjq.svqut:
		case tcpjq.ziezw:
		case tcpjq.zbwim:
		case tcpjq.hmhzl:
		case tcpjq.wyzlr:
		case tcpjq.clcxg:
		case tcpjq.jpcem:
		case tcpjq.iqqfl:
		case tcpjq.rlxea:
		case tcpjq.llifs:
		case tcpjq.pwqqd:
			if (remote && 0 == 0)
			{
				sshExceptionStatus = SshExceptionStatus.ProtocolError;
				if (sshExceptionStatus != SshExceptionStatus.UnclassifiableError)
				{
					break;
				}
			}
			sshExceptionStatus = SshExceptionStatus.OperationFailure;
			if (sshExceptionStatus != SshExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case tcpjq.jgdcv;
		case tcpjq.jgdcv:
		case tcpjq.kxdpn:
			sshExceptionStatus = SshExceptionStatus.ConnectionClosed;
			if (sshExceptionStatus != SshExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto default;
		default:
			sshExceptionStatus = SshExceptionStatus.UnclassifiableError;
			break;
		}
		pfwhz(sshExceptionStatus, remote, reason, inner);
	}

	internal SshException(SshExceptionStatus status, string message)
		: this(message, null, status)
	{
	}

	internal SshException(SshException e)
		: base(e.hndyi(), e)
	{
		pfwhz(e.Status, e.drbjp, e.kpcdk, e);
	}

	public SshException()
		: this(null, null)
	{
	}

	public SshException(string message)
		: this(message, null)
	{
	}

	public SshException(string message, Exception inner)
		: this(message, inner, SshExceptionStatus.UnclassifiableError)
	{
	}

	public SshException(string message, Exception inner, SshExceptionStatus status)
		: base(message, inner)
	{
		tcpjq tcpjq;
		switch (status)
		{
		case SshExceptionStatus.ConnectFailure:
		case SshExceptionStatus.ConnectionClosed:
			tcpjq = tcpjq.jgdcv;
			if (tcpjq != tcpjq.bxwwb)
			{
				break;
			}
			goto case SshExceptionStatus.ProtocolError;
		case SshExceptionStatus.ProtocolError:
		case SshExceptionStatus.Timeout:
			tcpjq = tcpjq.kxdpn;
			if (tcpjq != tcpjq.bxwwb)
			{
				break;
			}
			goto case SshExceptionStatus.UnexpectedMessage;
		case SshExceptionStatus.UnexpectedMessage:
			tcpjq = tcpjq.svqut;
			if (tcpjq != tcpjq.bxwwb)
			{
				break;
			}
			goto default;
		default:
			tcpjq = tcpjq.bxwwb;
			break;
		}
		pfwhz(status, p1: false, tcpjq, inner);
	}

	private void pfwhz(SshExceptionStatus p0, bool p1, tcpjq p2, Exception p3)
	{
		NetworkSessionExceptionStatus networkSessionExceptionStatus;
		switch (p0)
		{
		default:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.UnclassifiableError;
			if (networkSessionExceptionStatus == NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SshExceptionStatus.ConnectFailure;
		case SshExceptionStatus.ConnectFailure:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ConnectFailure;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SshExceptionStatus.ConnectionClosed;
		case SshExceptionStatus.ConnectionClosed:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ConnectionClosed;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SshExceptionStatus.ProtocolError;
		case SshExceptionStatus.ProtocolError:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ProtocolError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SshExceptionStatus.UnexpectedMessage;
		case SshExceptionStatus.UnexpectedMessage:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.ProtocolError;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SshExceptionStatus.Timeout;
		case SshExceptionStatus.Timeout:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.Timeout;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SshExceptionStatus.OperationFailure;
		case SshExceptionStatus.OperationFailure:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.OperationFailure;
			if (networkSessionExceptionStatus != NetworkSessionExceptionStatus.UnclassifiableError)
			{
				break;
			}
			goto case SshExceptionStatus.PasswordChangeRequired;
		case SshExceptionStatus.PasswordChangeRequired:
			networkSessionExceptionStatus = NetworkSessionExceptionStatus.OperationFailure;
			break;
		}
		base.Data["SshStatus"] = p0;
		base.Data["SshRemote"] = p1;
		base.Data["Status"] = networkSessionExceptionStatus;
		IDictionary data = base.Data;
		int num = (int)p2;
		data["ProtocolCode"] = num.ToString(CultureInfo.InvariantCulture);
		base.Data["ProtocolMessage"] = dfhzu(p2);
		if (p3 is SshException ex && 0 == 0)
		{
			string text = ex.nqhfo;
			if (text != null && 0 == 0)
			{
				nqhfo = text;
			}
		}
	}

	private static string dfhzu(tcpjq p0)
	{
		switch (p0)
		{
		case tcpjq.yeheh:
			return "Unknown";
		case tcpjq.bxwwb:
			return "Unspecified";
		case tcpjq.byugp:
			return "HostNotAllowedToConnect";
		case tcpjq.svqut:
			return "ProtocolError";
		case tcpjq.ziezw:
			return "KeyExchangeFailed";
		case tcpjq.rtrle:
			return "Reserved";
		case tcpjq.zbwim:
			return "MacError";
		case tcpjq.hmhzl:
			return "CompressionError";
		case tcpjq.wyzlr:
			return "ServiceNotAvailable";
		case tcpjq.clcxg:
			return "ProtocolVersionNotSupported";
		case tcpjq.jpcem:
			return "HostKeyNotVerifiable";
		case tcpjq.jgdcv:
			return "ConnectionLost";
		case tcpjq.kxdpn:
			return "DisconnectByApplication";
		case tcpjq.iqqfl:
			return "TooManyConnections";
		case tcpjq.rlxea:
			return "AuthenticationCancelledByUser";
		case tcpjq.llifs:
			return "NoMoreAuthenticationMethodsAvailable";
		case tcpjq.pwqqd:
			return "IllegalUserName";
		default:
		{
			int num = (int)p0;
			return num.ToString(CultureInfo.InvariantCulture);
		}
		}
	}
}
