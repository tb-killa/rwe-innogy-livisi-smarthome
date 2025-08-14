using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[Serializable]
[XmlType(Namespace = "http://rwe.com/SmartHome/2015/02/09/Common")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public enum DeviceUpdateResultCode
{
	AlreadyLatestVersion,
	NewerVersionAvailable,
	Failure,
	NotAuthorized
}
