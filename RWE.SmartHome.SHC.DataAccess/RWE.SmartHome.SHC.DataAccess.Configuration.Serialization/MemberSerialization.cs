using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public class MemberSerialization : IEntitySerialization
{
	private XmlSerializer serializer;

	public MemberSerialization()
	{
		serializer = new XmlSerializer(typeof(Member));
	}

	public XmlSerializer GetSerializer()
	{
		return serializer;
	}

	public string GetTableName()
	{
		throw new NotImplementedException("Members are not persisted in database");
	}

	public string GetCollectionTagName()
	{
		return "Mmbs";
	}
}
