using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[XmlRoot(ElementName = "StoreDataResponse", Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class StoreDataResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage", Order = 0)]
	public bool StoreDataResult;

	public StoreDataResponse()
	{
	}

	public StoreDataResponse(bool StoreDataResult)
	{
		this.StoreDataResult = StoreDataResult;
	}
}
