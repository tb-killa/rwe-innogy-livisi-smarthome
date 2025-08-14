using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration;

public enum AccessEnum
{
	[XmlEnum("ro")]
	ReadOnly,
	[XmlEnum("rw")]
	ReadWrite
}
