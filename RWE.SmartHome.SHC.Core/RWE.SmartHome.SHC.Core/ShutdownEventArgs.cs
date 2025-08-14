namespace RWE.SmartHome.SHC.Core;

public class ShutdownEventArgs
{
	public int TimeoutMilliseconds { get; set; }

	public ShutdownEventArgs(int timeoutMilliseconds)
	{
		TimeoutMilliseconds = timeoutMilliseconds;
	}

	public ShutdownEventArgs()
	{
	}
}
