namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public enum ConfigurationResultCode
{
	Success,
	Failure,
	NotAuthorized,
	InvalidNodePath,
	InvalidConfigurationIdentifier,
	InvalidConfigurationNode,
	IncorrectSequence
}
