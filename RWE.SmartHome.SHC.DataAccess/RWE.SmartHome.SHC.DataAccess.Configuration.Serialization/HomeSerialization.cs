using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public class HomeSerialization : IEntitySerialization
{
	private XmlSerializer serializer;

	public HomeSerialization()
	{
		serializer = new XmlSerializer(typeof(Home));
	}

	public XmlSerializer GetSerializer()
	{
		return serializer;
	}

	public string GetTableName()
	{
		throw new NotImplementedException("Homes are not persisted in database");
	}

	public string GetCollectionTagName()
	{
		return "HMs";
	}
}
