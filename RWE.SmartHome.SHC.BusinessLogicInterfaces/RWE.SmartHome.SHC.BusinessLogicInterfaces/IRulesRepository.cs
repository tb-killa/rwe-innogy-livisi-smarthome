using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public interface IRulesRepository
{
	IEnumerable<Rule> Rules { get; }

	IEnumerable<Rule> GetRulesForDevice(Guid deviceId);

	void Add(Rule rule);

	bool Remove(Guid ruleId);

	void BeginUpdate();

	void CommitChanges();
}
