using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class EntityMetadata
{
	[XmlAttribute]
	public EntityType EntityType { get; set; }

	[XmlAttribute]
	public Guid Id { get; set; }
}
