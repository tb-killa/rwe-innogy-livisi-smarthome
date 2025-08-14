using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public interface IProtocolSpecificTransformation
{
	ProtocolIdentifier ProtocolId { get; }

	IList<ErrorEntry> Errors { get; }

	IEnumerable<LogicalDeviceState> ImmediateStateChanges { get; }

	bool PrepareTransformation(IElementaryRuleRepository elementaryRuleRepository);

	void CommitTransformationResults(IEnumerable<Guid> devicesToDelete);

	void DiscardTransformationResults();
}
