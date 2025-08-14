using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports.Enums;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports;

public class StatusReport
{
	public StatusLevel Level { get; private set; }

	public byte[] Data { get; private set; }

	public uint Code { get; private set; }

	public StatusType Type { get; private set; }

	public StatusReport(uint typeId, uint statusLevel, uint code, byte[] data)
	{
		Type = (StatusType)typeId;
		Level = (StatusLevel)statusLevel;
		Data = data;
		Code = code;
	}
}
