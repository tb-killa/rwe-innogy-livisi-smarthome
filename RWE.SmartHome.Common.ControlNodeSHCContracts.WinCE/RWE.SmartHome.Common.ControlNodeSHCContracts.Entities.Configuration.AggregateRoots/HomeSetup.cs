using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

[XmlType("HMS")]
public class HomeSetup : Entity
{
	private Guid homeId;

	[XmlAttribute]
	public string AppId { get; set; }

	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }

	[XmlAttribute(AttributeName = "HmId")]
	public Guid HomeId
	{
		get
		{
			return homeId;
		}
		set
		{
			homeId = value;
		}
	}

	[XmlIgnore]
	public Home Home
	{
		get
		{
			return Resolver.ToHome(this, homeId);
		}
		set
		{
			homeId = Resolver.FromHome(value);
		}
	}

	[XmlArray(ElementName = "Ppts")]
	[XmlArrayItem(ElementName = "Ppt")]
	public List<Property> Properties { get; set; }

	public HomeSetup()
	{
		Properties = new List<Property>();
	}

	public new HomeSetup Clone()
	{
		return (HomeSetup)base.Clone();
	}

	public new HomeSetup Clone(Guid tag)
	{
		return (HomeSetup)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new HomeSetup();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		HomeSetup homeSetup = (HomeSetup)clone;
		homeSetup.Name = Name;
		homeSetup.Properties = Properties.Select((Property prop) => prop.Clone()).ToList();
		homeSetup.AppId = AppId;
		homeSetup.HomeId = HomeId;
		homeSetup.Tags = base.Tags;
	}
}
