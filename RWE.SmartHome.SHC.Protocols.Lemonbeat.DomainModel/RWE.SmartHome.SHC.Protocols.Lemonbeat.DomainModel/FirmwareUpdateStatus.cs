namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class FirmwareUpdateStatus
{
	public UpdateStatusCode StatusCode { get; set; }

	public uint? ExpectedOffset { get; set; }
}
