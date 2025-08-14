using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
[XmlRoot(ElementName = "CheckForDeviceUpdateResponse", Namespace = "http://rwe.com/SmartHome/2015/02/09/PublicFacingServices")]
public class CheckForDeviceUpdateResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2015/02/09/PublicFacingServices", Order = 0)]
	public DeviceUpdateResultCode CheckForDeviceUpdateResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2015/02/09/PublicFacingServices", Order = 1)]
	public DeviceUpdateInfo updateInfo;

	public CheckForDeviceUpdateResponse()
	{
	}

	public CheckForDeviceUpdateResponse(DeviceUpdateResultCode CheckForDeviceUpdateResult, DeviceUpdateInfo updateInfo)
	{
		this.CheckForDeviceUpdateResult = CheckForDeviceUpdateResult;
		this.updateInfo = updateInfo;
	}
}
