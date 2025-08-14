namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class ConfigurationPersistenceEventArgs
{
	public BackendPersistenceResult Result { get; private set; }

	public ConfigurationPersistenceEventArgs(BackendPersistenceResult result)
	{
		Result = result;
	}
}
