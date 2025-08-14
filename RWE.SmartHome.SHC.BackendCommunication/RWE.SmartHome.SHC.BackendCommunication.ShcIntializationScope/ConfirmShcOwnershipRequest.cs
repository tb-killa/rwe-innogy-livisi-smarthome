using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "ConfirmShcOwnership", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class ConfirmShcOwnershipRequest
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string sessionToken;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public ShcMetadata shcMetadata;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public string shcInitializationResult;

	public ConfirmShcOwnershipRequest()
	{
	}

	public ConfirmShcOwnershipRequest(string sessionToken, ShcMetadata shcMetadata, string shcInitializationResult)
	{
		this.sessionToken = sessionToken;
		this.shcMetadata = shcMetadata;
		this.shcInitializationResult = shcInitializationResult;
	}
}
