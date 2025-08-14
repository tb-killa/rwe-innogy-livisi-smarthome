using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.SHC.API.Configuration;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class ConfigurationConverters
{
	public static Link ToApi(this LinkBinding link)
	{
		return new Link(link.LinkType.ToApi(), link.EntityId);
	}

	public static global::SmartHome.SHC.API.Configuration.ActionDescription ToApi(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription action)
	{
		return new global::SmartHome.SHC.API.Configuration.ActionDescription(action.Id, new Link(action.Target.LinkType.ToApi(), action.Target.EntityId), action.ActionType, action.Data.Select((Parameter p) => p.ToApiProperty()));
	}
}
