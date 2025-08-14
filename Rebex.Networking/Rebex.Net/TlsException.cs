using System;
using System.Globalization;
using onrkn;

namespace Rebex.Net;

public class TlsException : NetworkSessionException
{
	private const string lwyuc = "TlsAlert";

	private const string iqcwd = "TlsIsRemote";

	private const string nqfjx = "TlsIsFatal";

	private const string nabgv = "TlsData";

	internal zppmb rkbch
	{
		get
		{
			return base.Data["TlsAlert"] as zppmb;
		}
		set
		{
			if (value != null && 0 == 0)
			{
				base.Data["TlsAlert"] = value;
				base.Data["TlsIsFatal"] = value.bgpwy;
				base.ProtocolCode = value.jmwmm.ToString(CultureInfo.InvariantCulture);
				base.ProtocolMessage = zppmb.grlvu((mjddr)value.jmwmm);
			}
			else
			{
				base.ProtocolCode = null;
				base.ProtocolMessage = null;
			}
		}
	}

	internal bool dipvo
	{
		get
		{
			object obj = base.Data["TlsIsRemote"];
			if (obj is bool && 0 == 0)
			{
				return (bool)obj;
			}
			return false;
		}
		set
		{
			base.Data["TlsIsRemote"] = value;
			if (value && 0 == 0)
			{
				base.Status = NetworkSessionExceptionStatus.ProtocolError;
			}
			else
			{
				base.Status = NetworkSessionExceptionStatus.OperationFailure;
			}
		}
	}

	internal static string feqgy(mjddr p0, bool p1)
	{
		if (p1 && 0 == 0)
		{
			return brgjd.edcru("Fatal error '{0}' has been reported by the remote connection end.", zppmb.grlvu(p0));
		}
		return brgjd.edcru("Fatal error '{0}' has been encountered on the local connection end.", zppmb.grlvu(p0));
	}

	private static string ipdgt(mjddr p0)
	{
		return feqgy(p0, p1: false);
	}

	internal TlsException(rtzwv level, mjddr description)
		: this(level, description, ipdgt(description), null)
	{
	}

	internal TlsException(mjddr description, Exception inner)
		: this(rtzwv.iogyt, description, ipdgt(description), inner)
	{
	}

	internal TlsException(mjddr description)
		: this(rtzwv.iogyt, description, ipdgt(description), null)
	{
	}

	internal TlsException(mjddr description, string message)
		: this(rtzwv.iogyt, description, message, null)
	{
	}

	public TlsException(string message)
		: this(message, null)
	{
	}

	internal TlsException(string message, NetworkSessionExceptionStatus status)
		: this(message, status, null)
	{
	}

	internal TlsException(string message, NetworkSessionExceptionStatus status, Exception inner)
		: this(message, inner)
	{
		base.Status = status;
	}

	internal TlsException(bool isRemote, string message)
		: this(message, null)
	{
		dipvo = isRemote;
	}

	internal TlsException(rtzwv level, mjddr description, string message, Exception inner)
		: base(message, inner)
	{
		rkbch = new zppmb(level, description);
		dipvo = false;
	}

	public TlsException(string message, Exception inner)
		: base(message, inner)
	{
		if (!(inner is TlsException ex) || 1 == 0)
		{
			rkbch = null;
			dipvo = false;
			return;
		}
		rkbch = ex.rkbch;
		dipvo = ex.dipvo;
		base.Status = ex.Status;
		object obj = ex.Data["TlsData"];
		if (obj != null && 0 == 0)
		{
			base.Data["TlsData"] = obj;
		}
	}

	internal static TlsException yxoes(Exception p0)
	{
		return new TlsException(p0.Message, p0);
	}
}
