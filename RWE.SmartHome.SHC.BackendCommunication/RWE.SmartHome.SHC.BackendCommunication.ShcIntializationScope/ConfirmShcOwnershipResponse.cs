using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "ConfirmShcOwnershipResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class ConfirmShcOwnershipResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public InitializationErrorCode ConfirmShcOwnershipResult;

	public ConfirmShcOwnershipResponse()
	{
	}

	public ConfirmShcOwnershipResponse(InitializationErrorCode ConfirmShcOwnershipResult)
	{
		this.ConfirmShcOwnershipResult = ConfirmShcOwnershipResult;
	}
}
