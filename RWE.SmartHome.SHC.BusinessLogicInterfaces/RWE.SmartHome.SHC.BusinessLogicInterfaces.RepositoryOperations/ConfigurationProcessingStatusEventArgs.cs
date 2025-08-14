namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

public class ConfigurationProcessingStatusEventArgs
{
	public ConfigurationProcessingStatus Status { get; private set; }

	public object Details { get; private set; }

	public ConfigurationProcessingStatusEventArgs(ConfigurationProcessingStatus status, object statusDetails)
	{
		Status = status;
		Details = statusDetails;
	}
}
