namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;

public class ProcessRestoredConfigurationEventArgs
{
	public string RestorePointId { get; set; }

	public bool CreateRestorePoint { get; private set; }

	public ProcessRestoredConfigurationEventArgs(string restorePointId, bool createRestorePoint)
	{
		RestorePointId = restorePointId;
		CreateRestorePoint = createRestorePoint;
	}
}
