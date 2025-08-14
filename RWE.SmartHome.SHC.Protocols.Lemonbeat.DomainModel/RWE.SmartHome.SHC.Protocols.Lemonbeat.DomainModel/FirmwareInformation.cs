namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class FirmwareInformation
{
	public uint Size { get; set; }

	public uint ID { get; set; }

	public uint ReceivedSize { get; set; }

	public uint ChunkSize { get; set; }
}
