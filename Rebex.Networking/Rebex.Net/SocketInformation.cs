using System.Net;

namespace Rebex.Net;

public class SocketInformation
{
	private readonly ISocket qfofk;

	public EndPoint LocalEndPoint => qfofk.LocalEndPoint;

	public EndPoint RemoteEndPoint => qfofk.RemoteEndPoint;

	public SocketInformation(ISocket socket)
	{
		qfofk = socket;
	}
}
