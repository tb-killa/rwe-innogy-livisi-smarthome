using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "UploadSystemInfo", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class UploadSystemInfoRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string content;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public SystemInfoType contentType;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string description;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	public int currentPackage;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 5)]
	public int nextPackage;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 6)]
	public string correlationId;

	public UploadSystemInfoRequest()
	{
	}

	public UploadSystemInfoRequest(string shcSerial, string content, SystemInfoType contentType, string description, int currentPackage, int nextPackage, string correlationId)
	{
		this.shcSerial = shcSerial;
		this.content = content;
		this.contentType = contentType;
		this.description = description;
		this.currentPackage = currentPackage;
		this.nextPackage = nextPackage;
		this.correlationId = correlationId;
	}
}
