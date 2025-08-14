using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "GetApplicationTokenResponse", Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class GetApplicationTokenResponse
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement", Order = 0)]
	public ApplicationsToken GetApplicationTokenResult;

	public GetApplicationTokenResponse()
	{
	}

	public GetApplicationTokenResponse(ApplicationsToken GetApplicationTokenResult)
	{
		this.GetApplicationTokenResult = GetApplicationTokenResult;
	}
}
