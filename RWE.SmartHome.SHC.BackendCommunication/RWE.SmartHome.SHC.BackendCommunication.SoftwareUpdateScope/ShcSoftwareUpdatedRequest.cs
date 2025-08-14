using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "ShcSoftwareUpdated", Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices")]
public class ShcSoftwareUpdatedRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices", Order = 0)]
	public ShcVersionInfo newShcVersion;

	public ShcSoftwareUpdatedRequest()
	{
	}

	public ShcSoftwareUpdatedRequest(ShcVersionInfo newShcVersion)
	{
		this.newShcVersion = newShcVersion;
	}
}
