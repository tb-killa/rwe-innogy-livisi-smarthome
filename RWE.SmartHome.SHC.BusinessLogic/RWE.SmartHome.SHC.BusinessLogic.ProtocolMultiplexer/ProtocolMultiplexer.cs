using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.TypeManager;

namespace RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer;

public class ProtocolMultiplexer : IProtocolMultiplexer, IProtocolRegistration, IRestrictionManager
{
	private readonly List<IProtocolAdapter> stateProcessors = new List<IProtocolAdapter>();

	private readonly List<IProtocolSpecificTransformation> transformations = new List<IProtocolSpecificTransformation>();

	private readonly IDeviceController deviceController;

	private readonly ILogicalStateRequestor logicalState;

	private readonly IPhysicalStateHandler physicalState;

	private readonly ProtocolSpecificDataBackup protocolSpecificDataBackup;

	private readonly IEventManager eventManager;

	private readonly IShcTypeManager shcTypeManager;

	private readonly Dictionary<ShcRestriction, uint> defaultMaxDeviceCount = new Dictionary<ShcRestriction, uint>
	{
		{
			ShcRestriction.PhysicalDeviceInclusionTo0,
			0u
		},
		{
			ShcRestriction.PhysicalDeviceInclusionTo2,
			2u
		},
		{
			ShcRestriction.PhysicalDeviceInclusionTo5,
			5u
		},
		{
			ShcRestriction.PhysicalDeviceInclusionTo10,
			10u
		}
	};

	private readonly IRepository configurationRepository;

	public IDeviceController DeviceController => deviceController;

	public IPhysicalStateHandler PhysicalState => physicalState;

	public IProtocolSpecificDataBackup DataBackup => protocolSpecificDataBackup;

	public ILogicalStateRequestor LogicalState => logicalState;

	public ProtocolMultiplexer(IEventManager eventManager, IRepository repository, IApplicationsHost applicationsHost, IScheduler scheduler, IConfigurationManager configurationManager, IShcTypeManager typeManager, IRepository configurationRepository, ILogicalDeviceStateRepository stateRepository)
	{
		deviceController = new DeviceController(eventManager, repository, applicationsHost);
		logicalState = new LogicalStateRequestor(eventManager, repository, applicationsHost, stateRepository);
		physicalState = new PhysicalStateHandler(repository);
		protocolSpecificDataBackup = new ProtocolSpecificDataBackup(eventManager, scheduler, configurationManager);
		protocolSpecificDataBackup.Initialize();
		this.eventManager = eventManager;
		shcTypeManager = typeManager;
		if (shcTypeManager != null)
		{
			shcTypeManager.SubscribeRestrictionManager(ShcRestriction.PhysicalDeviceInclusionTo0 | ShcRestriction.PhysicalDeviceInclusionTo2 | ShcRestriction.PhysicalDeviceInclusionTo5 | ShcRestriction.PhysicalDeviceInclusionTo10, this);
		}
		this.configurationRepository = configurationRepository;
		eventManager.GetEvent<PhysicalDeviceFoundEvent>().Subscribe(OnPhysicalDeviceFound, null, ThreadOption.PublisherThread, null);
	}

	public void RegisterProtocolAdapter(IProtocolAdapter protocolAdapter)
	{
		stateProcessors.Add(protocolAdapter);
		if (protocolAdapter.DeviceController != null)
		{
			deviceController.RegisterProtocolSpecificDeviceController(protocolAdapter.ProtocolId, protocolAdapter.DeviceController);
		}
		if (protocolAdapter.LogicalState != null)
		{
			logicalState.RegisterProtocolSpecificStateRequestor(protocolAdapter.LogicalState);
		}
		if (protocolAdapter.PhysicalState != null)
		{
			physicalState.RegisterProtocolSpecificStateRequestor(protocolAdapter.ProtocolId, protocolAdapter.PhysicalState);
		}
		if (protocolAdapter.DataBackup != null)
		{
			protocolSpecificDataBackup.RegisterProtocolSpecificDataBackup(protocolAdapter.DataBackup);
		}
		if (protocolAdapter.Transformation != null)
		{
			transformations.Add(protocolAdapter.Transformation);
		}
	}

	public List<ProtocolSpecificInformation> GetProtocolSpecificInformation()
	{
		List<ProtocolSpecificInformation> list = new List<ProtocolSpecificInformation>();
		foreach (IProtocolAdapter stateProcessor in stateProcessors)
		{
			ProtocolSpecificInformation protocolSpecificInformation = stateProcessor.GetProtocolSpecificInformation();
			if (protocolSpecificInformation != null)
			{
				list.Add(protocolSpecificInformation);
			}
		}
		return list;
	}

	public IEnumerable<IProtocolSpecificTransformation> GetProtocolSpecificTransformations()
	{
		return transformations;
	}

	public string GetDeviceDescription(Guid deviceId)
	{
		string text = string.Empty;
		foreach (IProtocolAdapter stateProcessor in stateProcessors)
		{
			IEnumerable<Guid> handledDevices = stateProcessor.GetHandledDevices();
			if (handledDevices != null && handledDevices.Any((Guid g) => g == deviceId))
			{
				text = stateProcessor.GetDeviceDescription(deviceId);
				if (!string.IsNullOrEmpty(text))
				{
					break;
				}
			}
		}
		return text;
	}

	public List<Guid> GetHandledDevices()
	{
		List<Guid> list = new List<Guid>();
		foreach (IProtocolAdapter stateProcessor in stateProcessors)
		{
			IEnumerable<Guid> handledDevices = stateProcessor.GetHandledDevices();
			if (handledDevices != null)
			{
				list.AddRange(handledDevices);
			}
		}
		return list;
	}

	public void ResetDeviceInclusionState(Guid deviceId)
	{
		foreach (IProtocolAdapter stateProcessor in stateProcessors)
		{
			stateProcessor.ResetDeviceInclusionState(deviceId);
		}
	}

	public void ActivateDeviceDiscovery(List<string> appIds)
	{
		eventManager.GetEvent<DeviceDiscoveryStatusChangedEvent>().Publish(new DeviceDiscoveryStatusChangedEventArgs
		{
			Phase = DiscoveryPhase.Prepare,
			AppIds = appIds
		});
		eventManager.GetEvent<DeviceDiscoveryStatusChangedEvent>().Publish(new DeviceDiscoveryStatusChangedEventArgs
		{
			Phase = DiscoveryPhase.Activate,
			AppIds = appIds
		});
	}

	public void DeactivateDeviceDiscovery()
	{
		eventManager.GetEvent<DeviceDiscoveryStatusChangedEvent>().Publish(new DeviceDiscoveryStatusChangedEventArgs
		{
			Phase = DiscoveryPhase.Deactivate,
			AppIds = null
		});
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		foreach (IProtocolAdapter stateProcessor in stateProcessors)
		{
			IProtocolAdapter adapter = stateProcessor;
			stateProcessor.DropDiscoveredDevices(devices.Where((BaseDevice x) => x.ProtocolId == adapter.ProtocolId).ToArray());
		}
	}

	public List<ShcTypeParameterState> GetRestrictionState(ShcRestriction restriction)
	{
		if ((restriction & (ShcRestriction.PhysicalDeviceInclusionTo0 | ShcRestriction.PhysicalDeviceInclusionTo2 | ShcRestriction.PhysicalDeviceInclusionTo5 | ShcRestriction.PhysicalDeviceInclusionTo10)) == 0)
		{
			return null;
		}
		uint defaultQuotaOfPhysicalDevices = GetDefaultQuotaOfPhysicalDevices();
		uint acquiredQuotaOfPhysicalDevices = GetAcquiredQuotaOfPhysicalDevices();
		uint num = (uint)(from pd in configurationRepository.GetBaseDevices()
			where pd.ProtocolId != ProtocolIdentifier.Virtual
			select pd).Count();
		List<ShcTypeParameterState> list = new List<ShcTypeParameterState>(3);
		list.Add(new ShcTypeParameterState
		{
			ParameterName = "APP_QUOTA",
			CurrentState = ((defaultQuotaOfPhysicalDevices == uint.MaxValue) ? "*" : defaultQuotaOfPhysicalDevices.ToString())
		});
		list.Add(new ShcTypeParameterState
		{
			ParameterName = "ACQUIRED_QUOTA",
			CurrentState = acquiredQuotaOfPhysicalDevices switch
			{
				0u => null, 
				uint.MaxValue => "*", 
				_ => acquiredQuotaOfPhysicalDevices.ToString(), 
			}
		});
		list.Add(new ShcTypeParameterState
		{
			ParameterName = "REMAINING",
			CurrentState = ((defaultQuotaOfPhysicalDevices == uint.MaxValue || acquiredQuotaOfPhysicalDevices == uint.MaxValue) ? "*" : ((defaultQuotaOfPhysicalDevices + acquiredQuotaOfPhysicalDevices > num) ? (defaultQuotaOfPhysicalDevices + acquiredQuotaOfPhysicalDevices - num) : 0u).ToString())
		});
		return list;
	}

	public uint GetMaximumNumberOfHandledDevices()
	{
		if (!shcTypeManager.IsUpdated)
		{
			return uint.MaxValue;
		}
		uint defaultQuotaOfPhysicalDevices = GetDefaultQuotaOfPhysicalDevices();
		if (defaultQuotaOfPhysicalDevices == uint.MaxValue)
		{
			return uint.MaxValue;
		}
		uint acquiredQuotaOfPhysicalDevices = GetAcquiredQuotaOfPhysicalDevices();
		if (acquiredQuotaOfPhysicalDevices == uint.MaxValue)
		{
			return uint.MaxValue;
		}
		ulong num = (ulong)defaultQuotaOfPhysicalDevices + (ulong)acquiredQuotaOfPhysicalDevices;
		if (num >= uint.MaxValue)
		{
			return uint.MaxValue;
		}
		return (uint)num;
	}

	private uint GetDefaultQuotaOfPhysicalDevices()
	{
		uint num = uint.MaxValue;
		foreach (ShcRestriction key in defaultMaxDeviceCount.Keys)
		{
			if (shcTypeManager.GetRestrictionData(key, string.Empty).IsRestrictionActive && defaultMaxDeviceCount[key] < num)
			{
				num = defaultMaxDeviceCount[key];
			}
		}
		return num;
	}

	private uint GetAcquiredQuotaOfPhysicalDevices()
	{
		uint num = 0u;
		foreach (ShcRestriction key in defaultMaxDeviceCount.Keys)
		{
			RestrictionData restrictionData = shcTypeManager.GetRestrictionData(key, string.Empty);
			if (!restrictionData.IsRestrictionActive || !RestrictionToApplicationMap.RestrictionToApplications.ContainsKey(key))
			{
				continue;
			}
			foreach (string item in RestrictionToApplicationMap.RestrictionToApplications[key])
			{
				try
				{
					restrictionData = shcTypeManager.GetRestrictionData(key, item);
					KeyValuePair<string, string> keyValuePair = restrictionData.ApplicationParameters.FirstOrDefault((KeyValuePair<string, string> p) => p.Key == "APP_QUOTA");
					if (keyValuePair.Key != "APP_QUOTA")
					{
						Log.Debug(Module.ProtocolMultiplexer, string.Format("App {0} has no param called {1}, or the application is not yet provisioned.", item, "APP_QUOTA"));
						continue;
					}
					if (keyValuePair.Value == "*")
					{
						return uint.MaxValue;
					}
					num += uint.Parse(keyValuePair.Value);
				}
				catch (Exception arg)
				{
					Log.Error(Module.ProtocolMultiplexer, $"Could not read quota for {item}: {arg}");
				}
			}
		}
		return num;
	}

	private void OnPhysicalDeviceFound(DeviceFoundEventArgs eventArgs)
	{
		if (eventArgs.FoundDevice.ProtocolId != ProtocolIdentifier.Virtual && GetMaximumNumberOfHandledDevices() <= GetIncludedDevicesCount())
		{
			eventArgs.State = DeviceFoundState.MaximumNumberOfDevicesReached;
		}
		eventManager.GetEvent<DeviceFoundEvent>().Publish(eventArgs);
	}

	private uint GetIncludedDevicesCount()
	{
		return (uint)configurationRepository.GetBaseDevices().Count((BaseDevice pd) => pd.ProtocolId != ProtocolIdentifier.Virtual);
	}
}
