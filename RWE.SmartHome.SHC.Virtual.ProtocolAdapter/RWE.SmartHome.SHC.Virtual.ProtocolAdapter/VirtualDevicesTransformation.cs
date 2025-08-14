using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Rules;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

internal class VirtualDevicesTransformation : BaseProtocolSpecificTransformation
{
	private readonly IShcBaseDeviceWatchers shcBaseDeviceWatchers;

	private Property[] oldShcBaseDeviceProperties;

	public override ProtocolIdentifier ProtocolId => ProtocolIdentifier.Virtual;

	internal VirtualDevicesTransformation(IRepository configRepository, IShcBaseDeviceWatchers shcBaseDeviceWatchers)
		: base(configRepository)
	{
		this.shcBaseDeviceWatchers = shcBaseDeviceWatchers;
	}

	public override bool PrepareTransformation(IElementaryRuleRepository elementaryRuleRepository)
	{
		ProcessElementaryRules(elementaryRuleRepository);
		oldShcBaseDeviceProperties = configRepository.GetOriginalBaseDevices().FirstOrDefault(ShcBaseDeviceIRepositoryExtensions.ShcBaseDevicesPredicate)?.Properties.ToArray();
		return false;
	}

	public override void CommitTransformationResults(IEnumerable<Guid> devicesToDelete)
	{
		try
		{
			BaseDevice baseDevice = configRepository.GetBaseDevices().FirstOrDefault(ShcBaseDeviceIRepositoryExtensions.ShcBaseDevicesPredicate);
			shcBaseDeviceWatchers.ProcessUpdate(oldShcBaseDeviceProperties, baseDevice?.Properties.ToArray());
		}
		catch (Exception ex)
		{
			Log.Exception(Module.VirtualProtocolAdapter, ex, "Base device watchers update failed");
		}
		oldShcBaseDeviceProperties = null;
	}

	public override void DiscardTransformationResults()
	{
		oldShcBaseDeviceProperties = null;
	}

	protected override bool AccelerateRule(ElementaryRule rule)
	{
		return false;
	}
}
