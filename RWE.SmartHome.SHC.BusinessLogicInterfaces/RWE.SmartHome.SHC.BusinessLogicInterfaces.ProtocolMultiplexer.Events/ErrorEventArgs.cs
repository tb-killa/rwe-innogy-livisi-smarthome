using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class ErrorEventArgs
{
	public string DeviceId { get; set; }

	public MessageType MessageType { get; set; }

	public bool ErrorConditionActive { get; set; }
}
