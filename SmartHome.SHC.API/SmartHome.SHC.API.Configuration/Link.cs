using System;

namespace SmartHome.SHC.API.Configuration;

public class Link
{
	public LinkType Type { get; set; }

	public string Id { get; set; }

	public Link()
	{
	}

	public Link(LinkType type, string id)
	{
		Type = type;
		Id = id;
	}

	public Link(LinkType type, Guid id)
		: this(type, id.ToString("N"))
	{
	}
}
