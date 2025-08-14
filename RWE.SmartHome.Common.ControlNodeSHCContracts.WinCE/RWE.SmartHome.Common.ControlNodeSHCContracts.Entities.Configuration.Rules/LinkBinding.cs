using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class LinkBinding : DataBinding
{
	[XmlAttribute(AttributeName = "Lt")]
	public EntityType LinkType { get; set; }

	[XmlAttribute(AttributeName = "Id")]
	public string EntityId { get; set; }

	public LinkBinding()
	{
	}

	public LinkBinding(EntityType linkType, Guid entityId)
	{
		LinkType = linkType;
		EntityId = entityId.ToString("N");
	}

	protected override DataBinding CreateClone()
	{
		return new LinkBinding();
	}

	protected override void TransferProperties(DataBinding clone)
	{
		if (!(clone is LinkBinding linkBinding))
		{
			throw new InvalidOperationException("LinkBinding: Invalid transfer properties call");
		}
		linkBinding.LinkType = LinkType;
		linkBinding.EntityId = EntityId;
	}

	public new LinkBinding Clone()
	{
		return base.Clone() as LinkBinding;
	}

	public new LinkBinding Clone(Guid tag)
	{
		return base.Clone(tag) as LinkBinding;
	}

	public Guid EntityIdAsGuid()
	{
		return new Guid(EntityId);
	}
}
