using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public class InteractionsSerialization : IEntitySerialization
{
	private XmlSerializer serializer;

	public InteractionsSerialization()
	{
		serializer = new XmlSerializer(typeof(Interaction));
	}

	public XmlSerializer GetSerializer()
	{
		return serializer;
	}

	public string GetTableName()
	{
		return "InteractionsXml";
	}

	public string GetCollectionTagName()
	{
		return "Interactions";
	}
}
