using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;

public class ProtocolSpecificDataEntity
{
	public ProtocolIdentifier ProtocolId { get; set; }

	public string DataId { get; set; }

	public string SubId { get; set; }

	public string Data { get; set; }

	public ProtocolSpecificDataEntity()
	{
	}

	public ProtocolSpecificDataEntity(ProtocolIdentifier protocolId, string dataId, string subId, string data)
	{
		ProtocolId = protocolId;
		SubId = subId;
		DataId = dataId;
		Data = data;
	}

	public ProtocolSpecificDataEntity(ProtocolIdentifier protocolId, string dataId, string data)
		: this(protocolId, dataId, string.Empty, data)
	{
	}
}
