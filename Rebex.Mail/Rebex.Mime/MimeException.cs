using System;
using System.Collections;
using System.Collections.Generic;

namespace Rebex.Mime;

public class MimeException : Exception
{
	private readonly IDictionary demtk;

	public new IDictionary Data => demtk;

	public MimeExceptionStatus Status
	{
		get
		{
			object obj = Data["Status"];
			if (obj is MimeExceptionStatus && 0 == 0)
			{
				return (MimeExceptionStatus)obj;
			}
			return MimeExceptionStatus.UnspecifiedError;
		}
	}

	private int kklgo
	{
		get
		{
			object obj = Data["Position"];
			if (obj is int && 0 == 0)
			{
				return (int)obj;
			}
			return 0;
		}
	}

	public override string Message => Data["Message"] as string;

	public MimeException()
		: this("Unspecified MIME error.", null, MimeExceptionStatus.UnspecifiedError, null)
	{
	}

	public MimeException(string message)
		: this(message, null, MimeExceptionStatus.UnspecifiedError, null)
	{
	}

	public MimeException(string message, Exception inner)
		: this(message, null, MimeExceptionStatus.UnspecifiedError, inner)
	{
	}

	internal MimeException(string msg, MimeExceptionStatus status)
		: this(msg, null, status, null)
	{
	}

	internal MimeException(string msg, MimeExceptionStatus status, Exception inner)
		: this(msg, null, status, inner)
	{
	}

	private MimeException(string msg, string data, MimeExceptionStatus status, Exception inner)
		: base(msg, inner)
	{
		demtk = new Dictionary<object, object>();
		Data["Message"] = msg;
		Data["Status"] = status;
	}

	internal void dtyub(string p0)
	{
		Data["Message"] = Message.TrimEnd('.') + p0;
	}

	private static string wwdnv(string p0, int p1)
	{
		if (p1 < 0)
		{
			return p0;
		}
		p0 = p0.TrimEnd('.');
		object obj = p0;
		p0 = string.Concat(obj, " at position ", p1, ".");
		return p0;
	}

	internal static MimeException dngxr(int p0, string p1)
	{
		return new MimeException(p0, p1, null);
	}

	internal static MimeException zarvw(int p0, string p1, Exception p2)
	{
		return new MimeException(p0, p1, p2);
	}

	private MimeException(int position, string message, Exception inner)
		: base(wwdnv(message, position), inner)
	{
		demtk = new Dictionary<object, object>();
		Data["Position"] = position;
		Data["Message"] = base.Message;
		Data["Status"] = MimeExceptionStatus.HeaderParserError;
	}
}
