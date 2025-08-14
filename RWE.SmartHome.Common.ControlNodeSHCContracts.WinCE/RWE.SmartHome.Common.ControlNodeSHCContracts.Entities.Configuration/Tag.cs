using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class Tag
{
	[XmlAttribute]
	public string Name { get; set; }

	[XmlAttribute]
	public string Value { get; set; }

	public Tag Clone()
	{
		Tag tag = new Tag();
		tag.Name = string.Copy(Name);
		tag.Value = string.Copy(Value);
		return tag;
	}
}
