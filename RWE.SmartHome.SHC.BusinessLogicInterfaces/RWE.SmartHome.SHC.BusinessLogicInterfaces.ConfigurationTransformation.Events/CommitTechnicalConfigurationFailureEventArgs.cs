namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;

public class CommitTechnicalConfigurationFailureEventArgs
{
	public int ConfigurationVersion { get; private set; }

	public string Message { get; private set; }

	public CommitTechnicalConfigurationFailureEventArgs(int configurationVersion, string message)
	{
		ConfigurationVersion = configurationVersion;
		Message = message;
	}
}
