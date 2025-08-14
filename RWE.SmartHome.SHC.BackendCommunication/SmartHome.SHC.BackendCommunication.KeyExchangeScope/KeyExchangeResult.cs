using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/09/08/Common")]
public enum KeyExchangeResult
{
	Success,
	DeviceNotFound,
	UnexpectedException,
	InvalidTenant,
	DeviceBlacklisted,
	TooManyArguments
}
