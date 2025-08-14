using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "ShcSoftwareUpdatedResponse", Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices")]
public class ShcSoftwareUpdatedResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices", Order = 0)]
	public ShcUpdateAnnouncementResultCode ShcSoftwareUpdatedResult;

	public ShcSoftwareUpdatedResponse()
	{
	}

	public ShcSoftwareUpdatedResponse(ShcUpdateAnnouncementResultCode ShcSoftwareUpdatedResult)
	{
		this.ShcSoftwareUpdatedResult = ShcSoftwareUpdatedResult;
	}
}
