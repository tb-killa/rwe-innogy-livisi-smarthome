namespace Rebex.Net;

public class SshChannelExitStatus
{
	private int vcqpw;

	private string fvncb;

	private bool mhqtu;

	private string qtcds;

	public long ExitCode => vcqpw;

	public string SignalName => fvncb;

	public bool CoreDumped => mhqtu;

	public string ErrorMessage => qtcds;

	internal void jxfol(uint p0)
	{
		vcqpw = (int)p0;
	}

	internal void pwabz(string p0, bool p1, string p2)
	{
		fvncb = p0;
		mhqtu = p1;
		qtcds = p2;
	}

	internal SshChannelExitStatus()
	{
	}
}
