using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[Serializable]
[XmlType(Namespace = "http://schemas.datacontract.org/2004/07/RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public enum EventType
{
	DalEntry,
	ShcTrackingEvent,
	Both
}
