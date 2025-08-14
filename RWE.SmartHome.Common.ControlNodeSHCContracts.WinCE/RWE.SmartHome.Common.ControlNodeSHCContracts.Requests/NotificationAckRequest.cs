using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class NotificationAckRequest : BaseRequest
{
	public Guid NotificationCorrelationId { get; set; }
}
