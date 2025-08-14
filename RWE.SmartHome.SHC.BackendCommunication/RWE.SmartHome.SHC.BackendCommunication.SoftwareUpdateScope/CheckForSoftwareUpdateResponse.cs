using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "CheckForSoftwareUpdateResponse", Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class CheckForSoftwareUpdateResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices", Order = 0)]
	public SwUpdateResultCode CheckForSoftwareUpdateResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices", Order = 1)]
	public UpdateInfo updateInfo;

	public CheckForSoftwareUpdateResponse()
	{
	}

	public CheckForSoftwareUpdateResponse(SwUpdateResultCode CheckForSoftwareUpdateResult, UpdateInfo updateInfo)
	{
		this.CheckForSoftwareUpdateResult = CheckForSoftwareUpdateResult;
		this.updateInfo = updateInfo;
	}
}
