using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceFirmwareUpdate;

public static class Extension
{
	public static DeviceDescriptor GetDescriptor(this RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceInfo)
	{
		if (deviceInfo == null)
		{
			return null;
		}
		DeviceDescriptor deviceDescriptor = new DeviceDescriptor();
		deviceDescriptor.AddInVersion = deviceInfo.AddInVersion;
		deviceDescriptor.HardwareVersion = deviceInfo.HardwareVersion;
		deviceDescriptor.FirmwareVersion = FormatInvalidVersion(deviceInfo.CurrentFirmwareVersion);
		deviceDescriptor.Manufacturer = deviceInfo.Manufacturer;
		deviceDescriptor.ProductId = deviceInfo.ProductId;
		return deviceDescriptor;
	}

	private static string FormatInvalidVersion(string firmwareVersion)
	{
		List<string> list = new List<string>();
		string[] array = firmwareVersion.Split('-');
		if (array.Length == 1)
		{
			return firmwareVersion;
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			switch (text.Split('.').Length - 1)
			{
			case 0:
				if (text.Length <= 3)
				{
					list.Add(text + ".0.0");
				}
				continue;
			case 1:
				list.Add(text + ".0");
				continue;
			case 2:
				list.Add(text);
				continue;
			}
			Log.Debug(Module.BusinessLogic, "A group should not contain more than 2 dots");
			break;
		}
		return string.Join("-", list.ToArray());
	}

	public static string FriendlyTrace(this DeviceDescriptor deviceDescriptor)
	{
		return $"([manuf={deviceDescriptor.Manufacturer}], [prod={deviceDescriptor.ProductId}], [hw={deviceDescriptor.HardwareVersion}], [fw={deviceDescriptor.FirmwareVersion}], [addin={deviceDescriptor.AddInVersion}])";
	}

	public static bool IdenticalHardware(this DeviceDescriptor deviceDescriptor, DeviceDescriptor targetDescriptor)
	{
		if (deviceDescriptor.ProductId == targetDescriptor.ProductId && deviceDescriptor.Manufacturer == targetDescriptor.Manufacturer)
		{
			return deviceDescriptor.HardwareVersion == targetDescriptor.HardwareVersion;
		}
		return false;
	}
}
