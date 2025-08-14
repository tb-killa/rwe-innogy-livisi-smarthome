using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[XmlRoot(ElementName = "StoreDeviceActivityLogResponse", Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class StoreDeviceActivityLogResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage", Order = 0)]
	public ShcTableStorageStoreResult StoreDeviceActivityLogResult;

	public StoreDeviceActivityLogResponse()
	{
	}

	public StoreDeviceActivityLogResponse(ShcTableStorageStoreResult StoreDeviceActivityLogResult)
	{
		this.StoreDeviceActivityLogResult = StoreDeviceActivityLogResult;
	}
}
