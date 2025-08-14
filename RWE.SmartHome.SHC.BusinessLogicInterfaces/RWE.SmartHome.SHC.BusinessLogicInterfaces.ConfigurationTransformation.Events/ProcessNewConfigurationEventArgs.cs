namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;

public class ProcessNewConfigurationEventArgs
{
	public bool CreateRestorePoint { get; private set; }

	public ProcessNewConfigurationEventArgs(bool createRestorePoint)
	{
		CreateRestorePoint = createRestorePoint;
	}
}
