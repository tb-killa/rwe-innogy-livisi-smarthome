using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

public class UpdateReport
{
	private const string AllApsId = "AllApsId";

	private readonly List<string> requiredValidations;

	private readonly List<ProtocolIdentifier> requiredProtocolUpdates;

	private readonly List<string> requiredAppUpdates;

	public List<EntityMetadata> RepositoryDeletionReport { get; set; }

	public bool IsValidationRequired => requiredValidations.Any();

	public bool IsTechnicalConfigurationUpdateRequired => requiredProtocolUpdates.Any();

	public List<string> AllUpdatedApps => requiredAppUpdates;

	public UpdateReport()
	{
		requiredValidations = new List<string>();
		requiredProtocolUpdates = new List<ProtocolIdentifier>();
		requiredAppUpdates = new List<string>();
	}

	public bool IsAppValidationRequired(string appId)
	{
		if (!requiredValidations.Contains("AllApsId"))
		{
			return requiredValidations.Contains(appId);
		}
		return true;
	}

	public void AddAppValidation(string appId)
	{
		if (!requiredValidations.Contains(appId))
		{
			requiredValidations.Add(appId);
		}
	}

	public void AddValidateAll()
	{
		AddAppValidation("AllApsId");
	}

	public void AddProtocolUpdate(ProtocolIdentifier protocolIdentifier)
	{
		if (!requiredProtocolUpdates.Contains(protocolIdentifier))
		{
			requiredProtocolUpdates.Add(protocolIdentifier);
		}
	}

	public void AddProtocolUpdateAll()
	{
		AddProtocolUpdate(ProtocolIdentifier.Cosip);
		AddProtocolUpdate(ProtocolIdentifier.Lemonbeat);
		AddProtocolUpdate(ProtocolIdentifier.Virtual);
		AddProtocolUpdate(ProtocolIdentifier.wMBus);
	}

	public bool IsProtocolUpdateRequired(ProtocolIdentifier protocolIdentifier)
	{
		return requiredProtocolUpdates.Contains(protocolIdentifier);
	}

	public void AddAppUpdate(string appId)
	{
		if (!requiredAppUpdates.Contains(appId))
		{
			requiredAppUpdates.Add(appId);
		}
	}

	public void AddUpdateAll()
	{
		AddAppUpdate("AllApsId");
	}

	public bool IsAppUpdateRequired(string appId)
	{
		if (!requiredAppUpdates.Contains("AllApsId"))
		{
			return requiredAppUpdates.Contains(appId);
		}
		return true;
	}
}
