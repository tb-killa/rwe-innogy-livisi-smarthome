using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[Serializable]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/09/08/Common")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public enum ConfigurationResultCode
{
	Success,
	Failure,
	NotAuthorized,
	InvalidNodePath,
	InvalidConfigurationIdentifier,
	InvalidConfigurationNode,
	IncorrectSequence
}
