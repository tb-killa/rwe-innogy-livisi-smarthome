using System;

namespace RWE.SmartHome.SHC.DomainModel.Actions;

public class Link
{
	public Guid Id { get; private set; }

	public string AppId { get; private set; }

	public LinkType Type { get; private set; }

	public Link(string id, LinkType type)
	{
		Type = type;
		if (type == LinkType.Product)
		{
			AppId = id;
		}
		else
		{
			Id = new Guid(id);
		}
	}
}
