using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[XmlRoot(ElementName = "CheckForSoftwareUpdate", Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class CheckForSoftwareUpdateRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices", Order = 1)]
	public ShcVersionInfo shcVersionInfo;

	public CheckForSoftwareUpdateRequest()
	{
	}

	public CheckForSoftwareUpdateRequest(string shcSerial, ShcVersionInfo shcVersionInfo)
	{
		this.shcSerial = shcSerial;
		this.shcVersionInfo = shcVersionInfo;
	}
}
