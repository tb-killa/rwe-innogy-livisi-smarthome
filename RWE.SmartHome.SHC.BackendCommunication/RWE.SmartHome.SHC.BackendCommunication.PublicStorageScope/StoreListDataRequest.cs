using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[XmlRoot(ElementName = "StoreListData", Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class StoreListDataRequest
{
	[XmlArray(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage", Order = 0)]
	public TrackData[] deviceTrackingEntities;

	public StoreListDataRequest()
	{
	}

	public StoreListDataRequest(TrackData[] deviceTrackingEntities)
	{
		this.deviceTrackingEntities = deviceTrackingEntities;
	}
}
