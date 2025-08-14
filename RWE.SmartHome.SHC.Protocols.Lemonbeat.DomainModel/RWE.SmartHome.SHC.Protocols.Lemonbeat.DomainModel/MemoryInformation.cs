namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class MemoryInformation
{
	public MemoryType MemoryType { get; set; }

	public uint Count { get; set; }

	public uint Free { get; set; }
}
