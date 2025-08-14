using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public class HomeSetupSerialization : IEntitySerialization
{
	private XmlSerializer serializer;

	public HomeSetupSerialization()
	{
		serializer = new XmlSerializer(typeof(HomeSetup));
	}

	public XmlSerializer GetSerializer()
	{
		return serializer;
	}

	public string GetTableName()
	{
		return "HomeSetupsXml";
	}

	public string GetCollectionTagName()
	{
		return "HMSs";
	}
}
