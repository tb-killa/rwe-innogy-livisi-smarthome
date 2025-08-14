namespace SHCWrapper.FirmwareUpdater;

public static class WinCEFirmwareManager
{
	public static void ResetPlatform()
	{
		PrivateWrapper.Reset();
	}

	public static bool EraseRawPartition()
	{
		return PrivateWrapper.EraseRawPartition();
	}

	public static bool WriteRawPartition(string file_to_write)
	{
		return PrivateWrapper.WriteRawPartition(file_to_write);
	}
}
