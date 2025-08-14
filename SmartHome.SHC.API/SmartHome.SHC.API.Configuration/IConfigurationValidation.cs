using System.Collections.Generic;

namespace SmartHome.SHC.API.Configuration;

public interface IConfigurationValidation
{
	IEnumerable<ConfigurationError> OnConfigurationChanging(IEnumerable<Capability> capabilities, IEnumerable<Device> devices, IEnumerable<ActionDescription> actionDescriptions, IEnumerable<Trigger> triggers);
}
