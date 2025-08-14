using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

public class DeviceUpdateInfo
{
	public Guid DeviceId { get; set; }

	public short Manufacturer { get; set; }

	public uint ProductId { get; set; }

	public string HardwareVersion { get; set; }

	public string CurrentFirmwareVersion { get; set; }

	public string AppID { get; set; }

	public string DeviceType { get; set; }

	public string AddInVersion { get; set; }

	public bool IsReachable { get; set; }

	public bool IsEventListener { get; set; }

	public DeviceUpdateState UpdateState { get; set; }

	public override string ToString()
	{
		return $"Update for device={DeviceId.ToString()}, [manuf={Manufacturer}], [prod={ProductId}], [hw={HardwareVersion}], [fw={CurrentFirmwareVersion}], [appid={AppID}], [deviceType={DeviceType}], [addin={AddInVersion}], [eventListener={IsEventListener.ToString()}], [reachable={IsReachable.ToString()}]";
	}

	public bool IsAppIdValid()
	{
		return AppID.StartsWith("sh://");
	}
}
