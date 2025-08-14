namespace RWE.SmartHome.SHC.CommonFunctionality;

public class MemoryStatus
{
	public int LoadPercentage { get; private set; }

	public int TotalPhysical { get; private set; }

	public int AvailablePhysical { get; private set; }

	public MemoryStatus(int loadPercentage, int total, int available)
	{
		LoadPercentage = loadPercentage;
		TotalPhysical = total;
		AvailablePhysical = available;
	}
}
