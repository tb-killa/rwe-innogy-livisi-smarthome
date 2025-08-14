using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public interface IEntitySerialization
{
	XmlSerializer GetSerializer();

	string GetTableName();

	string GetCollectionTagName();
}
