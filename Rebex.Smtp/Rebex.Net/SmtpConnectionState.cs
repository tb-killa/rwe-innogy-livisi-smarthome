using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

public class SmtpConnectionState
{
	private readonly bool bnqex;

	private readonly int tchgt;

	internal static readonly SmtpConnectionState qzdff = new SmtpConnectionState(connected: true, 0);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public bool Connection => bnqex;

	public bool Connected => bnqex;

	public int NativeErrorCode => tchgt;

	internal SmtpConnectionState(bool connected, int errorCode)
	{
		bnqex = connected;
		tchgt = errorCode;
	}
}
