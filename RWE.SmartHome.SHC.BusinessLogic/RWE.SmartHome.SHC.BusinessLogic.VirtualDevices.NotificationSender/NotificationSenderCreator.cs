using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.NotificationSender;

public static class NotificationSenderCreator
{
	public static BaseDevice CreateNotificationSender()
	{
		BaseDevice baseDevice = new BaseDevice();
		baseDevice.Name = "Notification Sender";
		baseDevice.ProtocolId = ProtocolIdentifier.Virtual;
		baseDevice.SerialNumber = "";
		baseDevice.Manufacturer = "RWE";
		baseDevice.DeviceType = BuiltinPhysicalDeviceType.NotificationSender.ToString();
		baseDevice.AppId = CoreConstants.CoreAppId;
		baseDevice.DeviceVersion = "1.0";
		baseDevice.TimeOfAcceptance = ShcDateTime.UtcNow;
		return baseDevice;
	}
}
