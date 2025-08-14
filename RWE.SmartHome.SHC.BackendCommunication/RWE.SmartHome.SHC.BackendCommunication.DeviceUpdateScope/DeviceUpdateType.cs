using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://schemas.datacontract.org/2004/07/Innogy.SmartHome.Backend.Common.DomainObjects.DeviceManagement")]
public enum DeviceUpdateType
{
	BackgroundTransfer,
	UserAssistedTransfer
}
