using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "CheckForDeviceUpdate", Namespace = "http://rwe.com/SmartHome/2015/02/09/PublicFacingServices")]
[DebuggerStepThrough]
public class CheckForDeviceUpdateRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2015/02/09/PublicFacingServices", Order = 0)]
	public DeviceDescriptor deviceDescriptor;

	public CheckForDeviceUpdateRequest()
	{
	}

	public CheckForDeviceUpdateRequest(DeviceDescriptor deviceDescriptor)
	{
		this.deviceDescriptor = deviceDescriptor;
	}
}
