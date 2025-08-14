using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public class LocationSerialization : IEntitySerialization
{
	private XmlSerializer serializer;

	public LocationSerialization()
	{
		serializer = new XmlSerializer(typeof(Location));
	}

	public XmlSerializer GetSerializer()
	{
		return serializer;
	}

	public string GetTableName()
	{
		return "LocationsXml";
	}

	public string GetCollectionTagName()
	{
		return "LCs";
	}
}
