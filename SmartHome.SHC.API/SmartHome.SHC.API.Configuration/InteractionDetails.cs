using System;

namespace SmartHome.SHC.API.Configuration;

public class InteractionDetails
{
	public string InteractionName { get; private set; }

	public Guid IntearctionId { get; private set; }

	public InteractionDetails(Guid interactionId, string interactionName)
	{
		IntearctionId = interactionId;
		InteractionName = interactionName;
	}
}
