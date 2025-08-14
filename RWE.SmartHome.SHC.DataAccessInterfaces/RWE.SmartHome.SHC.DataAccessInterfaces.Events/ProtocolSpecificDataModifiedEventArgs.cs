using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.Events;

public class ProtocolSpecificDataModifiedEventArgs
{
	public ProtocolIdentifier ProtocolId { get; set; }

	public string DataId { get; set; }

	public ModificationType Modification { get; set; }

	public string SubId { get; set; }
}
