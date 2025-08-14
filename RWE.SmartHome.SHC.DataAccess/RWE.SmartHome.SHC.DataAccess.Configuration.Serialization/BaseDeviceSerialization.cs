using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public class BaseDeviceSerialization : IEntitySerialization
{
	private XmlSerializer serializer;

	public BaseDeviceSerialization()
	{
		serializer = new XmlSerializer(typeof(BaseDevice));
	}

	public XmlSerializer GetSerializer()
	{
		return serializer;
	}

	public string GetTableName()
	{
		return "BaseDevicesXml";
	}

	public string GetCollectionTagName()
	{
		return "BDs";
	}
}
