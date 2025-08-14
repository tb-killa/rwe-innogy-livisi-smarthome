using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
[XmlRoot(ElementName = "UploadLogFile", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class UploadLogFileRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public byte[] content;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public int currentPackage;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public int nextPackage;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	public string correlationId;

	public UploadLogFileRequest()
	{
	}

	public UploadLogFileRequest(string shcSerial, byte[] content, int currentPackage, int nextPackage, string correlationId)
	{
		this.shcSerial = shcSerial;
		this.content = content;
		this.currentPackage = currentPackage;
		this.nextPackage = nextPackage;
		this.correlationId = correlationId;
	}
}
