using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "UploadLogFileResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class UploadLogFileResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public UploadFileResponse UploadLogFileResult;

	public UploadLogFileResponse()
	{
	}

	public UploadLogFileResponse(UploadFileResponse UploadLogFileResult)
	{
		this.UploadLogFileResult = UploadLogFileResult;
	}
}
