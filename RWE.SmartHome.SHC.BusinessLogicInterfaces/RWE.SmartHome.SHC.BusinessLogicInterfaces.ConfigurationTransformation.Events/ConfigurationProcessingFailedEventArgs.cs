using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;

public class ConfigurationProcessingFailedEventArgs
{
	public int ConfigurationVersion { get; private set; }

	public List<ErrorEntry> ErrorList { get; private set; }

	public SaveConfigurationError ErrorReason { get; private set; }

	public ConfigurationProcessingFailedEventArgs(int configurationVersion, SaveConfigurationError errorReason, List<ErrorEntry> errorList)
	{
		ConfigurationVersion = configurationVersion;
		ErrorReason = errorReason;
		ErrorList = errorList;
	}
}
