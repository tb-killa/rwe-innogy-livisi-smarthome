using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement")]
public enum ApplicationKind
{
	SHCAndSilverlight,
	SHCOnly,
	SilverlightOnly
}
