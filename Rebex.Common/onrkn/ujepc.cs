using System;
using System.Net;

namespace onrkn;

internal class ujepc : WebException
{
	private ezmya wgdmi;

	private thths ttyww;

	public ezmya zhmeu
	{
		get
		{
			return wgdmi;
		}
		private set
		{
			wgdmi = value;
		}
	}

	public thths jmubg
	{
		get
		{
			return ttyww;
		}
		private set
		{
			ttyww = value;
		}
	}

	internal ujepc(string message, ezmya status)
		: this(message, status, null, null)
	{
	}

	internal ujepc(string message, ezmya status, thths response)
		: this(message, status, response, null)
	{
	}

	internal ujepc(string message, ezmya status, Exception innerException)
		: this(message, status, null, innerException)
	{
	}

	internal ujepc(string message, ezmya status, thths response, Exception innerException)
		: base(message, innerException, (WebExceptionStatus)status, null)
	{
		zhmeu = status;
		jmubg = response;
	}
}
