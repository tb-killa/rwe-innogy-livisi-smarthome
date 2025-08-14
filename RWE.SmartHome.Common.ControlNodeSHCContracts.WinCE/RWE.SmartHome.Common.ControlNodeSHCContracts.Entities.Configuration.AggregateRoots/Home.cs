using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

[XmlType("HM")]
public class Home : Entity
{
	private List<Guid> members;

	private Guid homeSetupId;

	[XmlAttribute]
	public string AppId { get; set; }

	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }

	[XmlArray(ElementName = "Ppts")]
	[XmlArrayItem(ElementName = "Ppt")]
	public List<Property> Properties { get; set; }

	[XmlArrayItem(ElementName = "Mmb")]
	[XmlArray(ElementName = "Mmbs")]
	public List<Guid> MemberIds
	{
		get
		{
			if (members == null)
			{
				members = new List<Guid>();
			}
			return members;
		}
		set
		{
			members = value;
		}
	}

	[XmlAttribute(AttributeName = "HStpId")]
	public Guid HomeSetupId
	{
		get
		{
			return homeSetupId;
		}
		set
		{
			homeSetupId = value;
		}
	}

	[XmlIgnore]
	public HomeSetup HomeSetup
	{
		get
		{
			return Resolver.ToHomeSetup(this, homeSetupId);
		}
		set
		{
			homeSetupId = Resolver.FromHomeSetup(value);
		}
	}

	public Home()
	{
		Properties = new List<Property>();
	}

	public new Home Clone()
	{
		return (Home)base.Clone();
	}

	public new Home Clone(Guid tag)
	{
		return (Home)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new Home();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		Home home = (Home)clone;
		home.Name = Name;
		home.Properties = Properties.Select((Property prop) => prop.Clone()).ToList();
		home.HomeSetupId = HomeSetupId;
		home.MemberIds.AddRange(MemberIds);
		home.AppId = AppId;
		home.Tags = base.Tags;
	}
}
