using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "UploadSystemInfoResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class UploadSystemInfoResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public UploadFileResponse UploadSystemInfoResult;

	public UploadSystemInfoResponse()
	{
	}

	public UploadSystemInfoResponse(UploadFileResponse UploadSystemInfoResult)
	{
		this.UploadSystemInfoResult = UploadSystemInfoResult;
	}
}
