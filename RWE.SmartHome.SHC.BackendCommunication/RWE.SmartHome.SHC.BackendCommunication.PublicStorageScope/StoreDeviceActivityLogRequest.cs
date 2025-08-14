using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[XmlRoot(ElementName = "StoreDeviceActivityLog", Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class StoreDeviceActivityLogRequest
{
	[XmlArray(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage", Order = 0)]
	public DeviceActivityLog[] dalEntries;

	public StoreDeviceActivityLogRequest()
	{
	}

	public StoreDeviceActivityLogRequest(DeviceActivityLog[] dalEntries)
	{
		this.dalEntries = dalEntries;
	}
}
