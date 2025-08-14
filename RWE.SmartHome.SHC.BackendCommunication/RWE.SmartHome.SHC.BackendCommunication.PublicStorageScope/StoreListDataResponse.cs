using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "StoreListDataResponse", Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
public class StoreListDataResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage", Order = 0)]
	public bool StoreListDataResult;

	public StoreListDataResponse()
	{
	}

	public StoreListDataResponse(bool StoreListDataResult)
	{
		this.StoreListDataResult = StoreListDataResult;
	}
}
