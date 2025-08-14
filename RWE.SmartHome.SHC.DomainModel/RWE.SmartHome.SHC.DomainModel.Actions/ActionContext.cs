using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.DomainModel.Actions;

public class ActionContext
{
	public ContextType Type { get; private set; }

	public Guid AssociatedId { get; private set; }

	public string SessionId { get; private set; }

	public LinkBinding ReferenceTrigger { get; private set; }

	public string InteractionName { get; private set; }

	public ActionContext(ContextType type, Guid associatedId, LinkBinding referenceTrigger, string interactionName)
	{
		InteractionName = interactionName;
		ReferenceTrigger = referenceTrigger;
		Type = type;
		AssociatedId = associatedId;
		SessionId = string.Empty;
	}

	public ActionContext(ContextType type, Guid associatedId)
	{
		InteractionName = string.Empty;
		Type = type;
		AssociatedId = associatedId;
		SessionId = string.Empty;
	}

	public override string ToString()
	{
		return "Context: [" + Type.ToString() + " with associated ID " + AssociatedId.ToString("N") + "]";
	}
}
