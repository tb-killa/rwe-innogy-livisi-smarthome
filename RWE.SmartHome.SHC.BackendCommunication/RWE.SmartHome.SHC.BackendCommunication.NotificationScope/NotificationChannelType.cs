using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[Serializable]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/DomainObjects/Notifications")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public enum NotificationChannelType
{
	Email,
	SMS,
	Push
}
