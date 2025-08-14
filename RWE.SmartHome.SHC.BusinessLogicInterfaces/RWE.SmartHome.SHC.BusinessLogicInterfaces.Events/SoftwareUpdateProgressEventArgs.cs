namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class SoftwareUpdateProgressEventArgs
{
	public SoftwareUpdateState State { get; set; }

	public SoftwareUpdateProgressEventArgs(SoftwareUpdateState state)
	{
		State = state;
	}
}
