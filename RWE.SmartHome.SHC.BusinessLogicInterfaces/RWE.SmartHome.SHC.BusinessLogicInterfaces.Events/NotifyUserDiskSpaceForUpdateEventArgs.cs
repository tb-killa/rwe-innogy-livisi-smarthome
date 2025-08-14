namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class NotifyUserDiskSpaceForUpdateEventArgs
{
	public bool IsEnoughSpace { get; set; }

	public string DiskSpaceNecessaryToEmpty { get; set; }

	public NotifyUserDiskSpaceForUpdateEventArgs(bool isEnoughSpace)
	{
		IsEnoughSpace = isEnoughSpace;
	}
}
