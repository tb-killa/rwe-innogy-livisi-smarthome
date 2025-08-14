using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetApplicationTokenHashResponse", Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement")]
public class GetApplicationTokenHashResponse
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement", Order = 0)]
	public string GetApplicationTokenHashResult;

	public GetApplicationTokenHashResponse()
	{
	}

	public GetApplicationTokenHashResponse(string GetApplicationTokenHashResult)
	{
		this.GetApplicationTokenHashResult = GetApplicationTokenHashResult;
	}
}
