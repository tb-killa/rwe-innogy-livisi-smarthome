using System;
using System.Collections;
using System.Collections.Generic;

namespace Rebex.Net;

public abstract class NetworkSessionException : Exception
{
	private readonly IDictionary uenjv;

	public new IDictionary Data => uenjv;

	public NetworkSessionExceptionStatus Status
	{
		get
		{
			object obj = Data["Status"];
			if (obj is NetworkSessionExceptionStatus && 0 == 0)
			{
				return (NetworkSessionExceptionStatus)obj;
			}
			return NetworkSessionExceptionStatus.UnclassifiableError;
		}
		protected set
		{
			Data["Status"] = value;
		}
	}

	public string ProtocolCode
	{
		get
		{
			return Data["ProtocolCode"] as string;
		}
		protected set
		{
			Data["ProtocolCode"] = value;
		}
	}

	public string ProtocolMessage
	{
		get
		{
			return Data["ProtocolMessage"] as string;
		}
		protected set
		{
			Data["ProtocolMessage"] = value;
		}
	}

	public NetworkSessionException(string message)
		: this(message, null)
	{
	}

	public NetworkSessionException(string message, Exception innerException)
		: base(message, innerException)
	{
		uenjv = new Dictionary<object, object>();
	}
}
