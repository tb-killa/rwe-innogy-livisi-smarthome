using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

[XmlType("Mmb")]
public class Member : Entity
{
	private Guid presenceDeviceId;

	private Guid homeId;

	[XmlAttribute]
	public string AppId { get; set; }

	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }

	[XmlArrayItem(ElementName = "Ppt")]
	[XmlArray(ElementName = "Ppts")]
	public List<Property> Properties { get; set; }

	[XmlAttribute(AttributeName = "PrDId")]
	public Guid PresenceDeviceId
	{
		get
		{
			bool flag = 0 == 0;
			return presenceDeviceId;
		}
		set
		{
			presenceDeviceId = value;
		}
	}

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

	[XmlIgnore]
	public BaseDevice PresenceDevice
	{
		get
		{
			return Resolver.ToBaseDevice(this, presenceDeviceId);
		}
		set
		{
			presenceDeviceId = Resolver.FromBaseDevice(value);
		}
	}

	public Member()
	{
		Properties = new List<Property>();
	}

	public new Member Clone()
	{
		return (Member)base.Clone();
	}

	public new Member Clone(Guid tag)
	{
		return (Member)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new Member();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		Member member = (Member)clone;
		member.Name = Name;
		member.Properties = Properties.Select((Property prop) => prop.Clone()).ToList();
		member.HomeId = HomeId;
		member.PresenceDeviceId = PresenceDeviceId;
		member.AppId = AppId;
	}
}
