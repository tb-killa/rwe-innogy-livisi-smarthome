using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common")]
public enum SystemInfoType
{
	GeneralInformation,
	CustomApplication
}
