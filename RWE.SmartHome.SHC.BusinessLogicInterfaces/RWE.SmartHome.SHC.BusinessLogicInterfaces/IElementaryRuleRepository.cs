using System.Collections.Generic;
using RWE.SmartHome.SHC.DomainModel.Rules;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public interface IElementaryRuleRepository
{
	IEnumerable<ElementaryRule> ListUnhandledRules();
}
