using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "StoreData", Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class StoreDataRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage", Order = 0)]
	public TrackData deviceTrackingEntity;

	public StoreDataRequest()
	{
	}

	public StoreDataRequest(TrackData deviceTrackingEntity)
	{
		this.deviceTrackingEntity = deviceTrackingEntity;
	}
}
