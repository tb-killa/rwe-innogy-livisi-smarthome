using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;

public class EntityDeploymentInformation
{
	public EntityMetadata Entity { get; set; }

	public DeploymentState State { get; set; }
}
