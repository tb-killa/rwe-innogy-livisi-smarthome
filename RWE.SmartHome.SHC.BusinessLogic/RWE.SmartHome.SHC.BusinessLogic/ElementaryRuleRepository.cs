using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.DomainModel.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic;

internal class ElementaryRuleRepository : IElementaryRuleRepository
{
	private readonly List<ElementaryRule> elementaryRules;

	public ElementaryRuleRepository(IRepository repository)
	{
		elementaryRules = repository.GetInteractions().SelectMany((Interaction x) => ElementaryRule.SplitInteraction(x)).ToList();
	}

	public IEnumerable<ElementaryRule> ListUnhandledRules()
	{
		return elementaryRules.Where((ElementaryRule x) => !x.IsHandled);
	}
}
