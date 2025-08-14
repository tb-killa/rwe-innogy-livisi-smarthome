using System.Collections.Generic;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;

public class EntityDeploymentNotification : BaseNotification
{
	[XmlArrayItem(ElementName = "EDIs")]
	[XmlArray(ElementName = "EDs")]
	public List<EntityDeploymentInformation> EntityDeployments { get; set; }

	public EntityDeploymentNotification()
	{
		EntityDeployments = new List<EntityDeploymentInformation>();
	}
}
