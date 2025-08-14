namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class MemoryInformation
{
	public MemoryInfoType MemoryType { get; set; }

	public uint Count { get; set; }

	public uint Free { get; set; }
}
