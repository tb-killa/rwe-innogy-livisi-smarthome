using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

internal class EntityRelationsGraph
{
	private readonly IEntityRelationsGraphConfigurationProxy configurationRepositoryProxy;

	private readonly Dictionary<Guid, List<Guid>> interactionBaseDevices = new Dictionary<Guid, List<Guid>>();

	public EntityRelationsGraph(IEntityRelationsGraphConfigurationProxy configurationRepositoryProxy)
	{
		this.configurationRepositoryProxy = configurationRepositoryProxy;
		Init();
	}

	public List<BaseDevice> GetRelatedBaseDevices(Interaction interaction)
	{
		if (!interactionBaseDevices.TryGetValue(interaction.Id, out var value))
		{
			value = new List<Guid>();
		}
		return value.Select((Guid x) => configurationRepositoryProxy.GetBaseDevice(x)).ToList();
	}

	private void Init()
	{
		foreach (Interaction interaction in configurationRepositoryProxy.GetInteractions())
		{
			Guid interactionId = interaction.Id;
			InteractionProcessor interactionProcessor = new InteractionProcessor();
			interactionProcessor.OnBaseDeviceFound = delegate(Guid baseDeviceId)
			{
				OnBaseDeviceFound(interactionId, baseDeviceId);
			};
			interactionProcessor.OnLogicalDeviceFound = delegate(Guid logicalDeviceId)
			{
				OnLogicalDeviceFound(interactionId, logicalDeviceId);
			};
			interactionProcessor.Process(interaction);
		}
	}

	private void OnBaseDeviceFound(Guid interactionId, Guid baseDeviceId)
	{
		BaseDevice baseDevice = configurationRepositoryProxy.GetBaseDevice(baseDeviceId);
		if (baseDevice != null)
		{
			AddIdsToList(interactionBaseDevices, interactionId, new Guid[1] { baseDevice.Id });
		}
	}

	private void OnLogicalDeviceFound(Guid interactionId, Guid logicalDeviceId)
	{
		LogicalDevice logicalDevice = configurationRepositoryProxy.GetLogicalDevice(logicalDeviceId);
		if (logicalDevice != null)
		{
			AddIdsToList(interactionBaseDevices, interactionId, new Guid[1] { logicalDevice.BaseDeviceId });
		}
	}

	private void AddIdsToList(Dictionary<Guid, List<Guid>> stash, Guid ownerId, IEnumerable<Guid> ids)
	{
		if (!stash.TryGetValue(ownerId, out var value))
		{
			value = new List<Guid>();
			stash.Add(ownerId, value);
		}
		value.AddRange(ids);
		stash[ownerId] = value.Distinct().ToList();
	}
}
