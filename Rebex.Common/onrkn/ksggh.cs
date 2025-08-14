using System;
using System.Net.Sockets;

namespace onrkn;

internal class ksggh : Exception
{
	private mlaam ovcrk;

	public ksggh(SocketException e, mlaam status)
		: this(e.Message, status, e)
	{
	}

	public ksggh(string message, mlaam status, Exception e)
		: base(message, e)
	{
		ovcrk = status;
	}
}
