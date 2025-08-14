namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

public class DeviceFirmwareDescriptor
{
	public string TargetVersion { get; set; }

	public string CurrentVersion { get; set; }

	public string ImageFile { get; set; }

	public override bool Equals(object other)
	{
		if (!(other is DeviceFirmwareDescriptor deviceFirmwareDescriptor))
		{
			return false;
		}
		if (ImageFile == deviceFirmwareDescriptor.ImageFile)
		{
			return TargetVersion == deviceFirmwareDescriptor.TargetVersion;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return "Firmware " + TargetVersion + "@ " + ImageFile;
	}
}
