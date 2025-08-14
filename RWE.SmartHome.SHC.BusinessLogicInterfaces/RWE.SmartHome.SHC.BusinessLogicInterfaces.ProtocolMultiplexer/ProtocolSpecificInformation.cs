using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;

public class ProtocolSpecificInformation
{
	public ProtocolIdentifier ProtocolId { get; set; }

	public string XmlInformation { get; set; }

	public ProtocolSpecificInformation()
	{
	}

	public ProtocolSpecificInformation(ProtocolIdentifier protocolId)
	{
		ProtocolId = protocolId;
	}
}
