using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common")]
public enum ShcTableStorageStoreResult
{
	Success,
	PartialSuccess,
	Failed,
	InvalidPayload,
	InvalidStorageType,
	UploadQuotaExceeded
}
