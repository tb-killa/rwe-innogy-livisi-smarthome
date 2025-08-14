using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

[XmlType("LC")]
public class Location : Entity
{
	public string Name { get; set; }

	[XmlElement(ElementName = "Pos")]
	public int Position { get; set; }

	[XmlElement(ElementName = "RTyp")]
	public RoomType RoomType { get; set; }

	public Location()
	{
	}

	public Location(string name)
		: this()
	{
		Name = name;
	}

	protected override Entity CreateClone()
	{
		return new Location();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		Location location = (Location)clone;
		location.Name = Name;
		location.Position = Position;
		location.RoomType = RoomType;
	}

	public new Location Clone()
	{
		return (Location)base.Clone();
	}

	public new Location Clone(Guid tag)
	{
		return (Location)base.Clone(tag);
	}
}
