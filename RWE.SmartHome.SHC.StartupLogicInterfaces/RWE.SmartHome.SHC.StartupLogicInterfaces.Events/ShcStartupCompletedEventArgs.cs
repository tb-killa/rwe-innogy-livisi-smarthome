namespace RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

public class ShcStartupCompletedEventArgs
{
	public StartupProgress Progress { get; private set; }

	public ShcStartupCompletedEventArgs(StartupProgress progress)
	{
		Progress = progress;
	}
}
