using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Persistence;

public class PartnerEntity
{
	public uint Id { get; set; }

	public byte[] EncryptionKey { get; set; }

	public byte[] IpAddress { get; set; }

	public uint? SubDeviceId { get; set; }

	public int? GatewayId { get; set; }

	public RadioMode? WakeupMode { get; set; }

	public uint? WakeupChannel { get; set; }

	public uint? WakeupInterval { get; set; }

	public uint? WakeupOffset { get; set; }
}
