using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[XmlRoot(ElementName = "ShcResetByOwnerResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class ShcResetByOwnerResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public InitializationErrorCode ShcResetByOwnerResult;

	public ShcResetByOwnerResponse()
	{
	}

	public ShcResetByOwnerResponse(InitializationErrorCode ShcResetByOwnerResult)
	{
		this.ShcResetByOwnerResult = ShcResetByOwnerResult;
	}
}
