using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

public class ConfigFixEntityDeletedEventArgs
{
	public const string EntityName = "EntityName";

	public const string EntityLocation = "EntityLocation";

	public const string EntitySerialNumber = "EntitySerialNumber";

	public const string EntityType = "EntityType";

	public const string EntityDeviceType = "EntityDeviceType";

	public readonly Dictionary<string, string> EventParams = new Dictionary<string, string>();

	public ConfigFixEntityDeletedEventArgs(Location location)
	{
		if (location != null)
		{
			EventParams.Add("EntityName", location.Name);
		}
		EventParams.Add("EntityType", RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Location.ToString());
	}

	public ConfigFixEntityDeletedEventArgs(BaseDevice baseDevice)
	{
		if (baseDevice != null)
		{
			EventParams.Add("EntityName", baseDevice.Name);
			EventParams.Add("EntityLocation", (baseDevice.Location != null) ? baseDevice.Location.Name : string.Empty);
			EventParams.Add("EntitySerialNumber", baseDevice.SerialNumber);
			EventParams.Add("EntityDeviceType", baseDevice.DeviceType);
		}
		EventParams.Add("EntityType", RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.BaseDevice.ToString());
	}

	public ConfigFixEntityDeletedEventArgs(Interaction interaction)
	{
		if (interaction != null)
		{
			EventParams.Add("EntityName", interaction.Name);
		}
		EventParams.Add("EntityType", RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Interaction.ToString());
	}
}
