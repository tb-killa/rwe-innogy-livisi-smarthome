using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

internal class LogicalDeviceSerialization : IEntitySerialization
{
	private XmlSerializer serializer;

	public LogicalDeviceSerialization()
	{
		serializer = new XmlSerializer(typeof(LogicalDevice));
	}

	public XmlSerializer GetSerializer()
	{
		return serializer;
	}

	public string GetTableName()
	{
		return "LogicalDevicesXml";
	}

	public string GetCollectionTagName()
	{
		return "LDs";
	}
}
