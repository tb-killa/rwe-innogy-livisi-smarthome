using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

internal class TransformationProcessor
{
	private readonly List<ErrorEntry> errors = new List<ErrorEntry>();

	private readonly IEnumerable<IProtocolSpecificTransformation> protocolSpecificTransformations = new List<IProtocolSpecificTransformation>();

	private readonly IRulesRepository rulesRepository;

	private readonly IEnumerable<IConfigurationValidator> validators;

	private IElementaryRuleRepository elementaryRuleRepository;

	public IEnumerable<LogicalDeviceState> ImmediateStateChanges { get; private set; }

	public TransformationProcessor(IEnumerable<IProtocolSpecificTransformation> protocolSpecificTransformations, IRulesRepository rulesRepository, IEnumerable<IConfigurationValidator> validators)
	{
		this.protocolSpecificTransformations = protocolSpecificTransformations;
		this.rulesRepository = rulesRepository;
		this.validators = validators;
	}

	public List<ErrorEntry> Validate(IRepository repository, uint physicalDevicesCountLimit, IEnumerable<Guid> knownPhysicalDevices, RepositoryUpdateContextData updateContextData)
	{
		LogicalConfigurationValidation logicalConfigurationValidation = new LogicalConfigurationValidation(repository, validators);
		errors.AddRange(logicalConfigurationValidation.ValidateConfiguration(physicalDevicesCountLimit, knownPhysicalDevices, updateContextData));
		return errors;
	}

	public List<ErrorEntry> PrepareTransformation(IRepository repository, uint physicalDevicesCountLimit, IEnumerable<Guid> knownPhysicalDevices, RepositoryUpdateContextData updateContextData)
	{
		elementaryRuleRepository = new ElementaryRuleRepository(repository);
		if (errors.Any())
		{
			return errors;
		}
		bool flag = false;
		foreach (IProtocolSpecificTransformation protocolSpecificTransformation in protocolSpecificTransformations)
		{
			if (updateContextData == null || updateContextData.UpdateReport == null || updateContextData.UpdateReport.IsProtocolUpdateRequired(protocolSpecificTransformation.ProtocolId))
			{
				try
				{
					flag |= protocolSpecificTransformation.PrepareTransformation(elementaryRuleRepository);
				}
				catch (TransformationException ex)
				{
					errors.Add(ex.Error);
					flag = true;
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (flag)
		{
			foreach (IProtocolSpecificTransformation protocolSpecificTransformation2 in protocolSpecificTransformations)
			{
				if (updateContextData == null || updateContextData.UpdateReport == null || updateContextData.UpdateReport.IsProtocolUpdateRequired(protocolSpecificTransformation2.ProtocolId))
				{
					errors.AddRange(protocolSpecificTransformation2.Errors);
				}
			}
			if (errors.Count == 0)
			{
				Log.Warning(Module.ConfigurationTransformation, "Inconsistent error state reported.");
				errors.Add(new ErrorEntry
				{
					ErrorCode = ValidationErrorCode.Unknown
				});
			}
			return errors;
		}
		List<LogicalDeviceState> list = new List<LogicalDeviceState>();
		foreach (IProtocolSpecificTransformation protocolSpecificTransformation3 in protocolSpecificTransformations)
		{
			if (updateContextData == null || updateContextData.UpdateReport == null || updateContextData.UpdateReport.IsProtocolUpdateRequired(protocolSpecificTransformation3.ProtocolId))
			{
				list.AddRange(protocolSpecificTransformation3.ImmediateStateChanges);
			}
		}
		ImmediateStateChanges = list;
		rulesRepository.BeginUpdate();
		foreach (ElementaryRule item in elementaryRuleRepository.ListUnhandledRules())
		{
			rulesRepository.Add(item.ToRule());
		}
		return errors;
	}

	public void CommitTransformationResults(IEnumerable<Guid> devicesToDelete, RepositoryUpdateContextData updateContextData)
	{
		List<Guid> devicesToDelete2 = devicesToDelete.ToList();
		foreach (IProtocolSpecificTransformation protocolSpecificTransformation in protocolSpecificTransformations)
		{
			if (updateContextData == null || updateContextData.UpdateReport == null || updateContextData.UpdateReport.IsProtocolUpdateRequired(protocolSpecificTransformation.ProtocolId))
			{
				protocolSpecificTransformation.CommitTransformationResults(devicesToDelete2);
			}
		}
		rulesRepository.CommitChanges();
	}

	public void DiscardTransformationResults()
	{
		foreach (IProtocolSpecificTransformation protocolSpecificTransformation in protocolSpecificTransformations)
		{
			protocolSpecificTransformation.DiscardTransformationResults();
		}
	}
}
