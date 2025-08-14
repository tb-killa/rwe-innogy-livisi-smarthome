using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class VrccStateHandler
{
	private readonly IRepository configRepo;

	private readonly ILogicalDeviceStateRepository stateRepo;

	private readonly Dictionary<Guid, IVrccDevice> vrccs = new Dictionary<Guid, IVrccDevice>();

	private readonly object syncRoot = new object();

	private readonly Dispatcher dispatcher = new Dispatcher();

	public event Action<VrccStateHandlerUpdateArgs> OnVrccStateUpdate;

	public VrccStateHandler(IRepository configRepo, ILogicalDeviceStateRepository stateRepo, IEventManager eventManager)
	{
		this.configRepo = configRepo;
		this.stateRepo = stateRepo;
		eventManager.GetEvent<LogicalDeviceStateChangedEvent>().Subscribe(OnLogicalDeviceStateChanged, null, ThreadOption.SubscriberThread, dispatcher);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, (ConfigurationProcessedEventArgs x) => x.ConfigurationPhase == ConfigurationProcessedPhase.UINotified, ThreadOption.PublisherThread, null);
		dispatcher.Start();
		Initialize();
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs e)
	{
		lock (syncRoot)
		{
			List<Guid> dropBdsIds = (from bd2 in e.DeletedBaseDevices
				where bd2.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.VRCC
				select bd2.Id).ToList();
			Guid[] array = (from bd2 in e.ModifiedBaseDevices
				where bd2.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.VRCC
				select bd2.Id).ToArray();
			dropBdsIds.AddRange(array);
			Guid[] modifiedLdIds = (from ld2 in e.ModifiedLogicalDevices
				where ld2.DeviceType == "RoomTemperature" || ld2.DeviceType == "RoomHumidity" || ld2.DeviceType == "RoomSetpoint"
				select ld2.Id).ToArray();
			List<Guid> list = vrccs.Values.Where((IVrccDevice x) => dropBdsIds.Contains(x.BaseDeviceId) || modifiedLdIds.Contains(x.Id)).SelectMany((IVrccDevice y) => y.GetGroupIds()).Distinct()
				.ToList();
			try
			{
				foreach (Guid item in list)
				{
					if (vrccs.ContainsKey(item))
					{
						vrccs.Remove(item);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Exception(Module.BusinessLogic, ex, "Error occured while processing configuration changed");
			}
			if (array.Length > 0 || modifiedLdIds.Length > 0)
			{
				Initialize();
			}
		}
	}

	private void Initialize()
	{
		try
		{
			Guid[] currentBdIds = vrccs.Values.Select((IVrccDevice x) => x.BaseDeviceId).ToArray();
			IEnumerable<BaseDevice> unhandledBaseDevices = from x in configRepo.GetBaseDevices()
				where !currentBdIds.Contains(x.Id)
				select x;
			CompositeDeviceBuilder compositeDeviceBuilder = new CompositeDeviceBuilder(configRepo, stateRepo);
			IEnumerable<CompositeDevice> enumerable = compositeDeviceBuilder.BuildDevices(unhandledBaseDevices);
			List<ActionDescription> list = new List<ActionDescription>();
			foreach (CompositeDevice item in enumerable)
			{
				foreach (UnderlyingDevice underlyingDevice in item.UnderlyingDevices)
				{
					vrccs.Add(underlyingDevice.Id, underlyingDevice);
					LogicalDeviceState logicalDeviceState = stateRepo.GetLogicalDeviceState(underlyingDevice.Id);
					underlyingDevice.StateValue = logicalDeviceState.GetStateValue();
				}
				vrccs.Add(item.Id, item);
				LogicalDeviceState logicalDeviceState2 = stateRepo.GetLogicalDeviceState(item.Id);
				list.AddRange(item.SetInitialState(logicalDeviceState2));
			}
			RaiseStateChange(list);
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "Error occured while initializing VRCC State handler");
		}
	}

	private void OnLogicalDeviceStateChanged(LogicalDeviceStateChangedEventArgs e)
	{
		lock (syncRoot)
		{
			IVrccDevice cachedVrccDevice = GetCachedVrccDevice<IVrccDevice>(e.LogicalDeviceId);
			if (cachedVrccDevice != null)
			{
				Log.Debug(Module.BusinessLogic, $"VRCC state handling: device {cachedVrccDevice.Id} - old state [{e.OldLogicalDeviceState}], new state [{e.NewLogicalDeviceState}]");
				IEnumerable<ActionDescription> setStateActions = cachedVrccDevice.HandleStateChange(e);
				RaiseStateChange(setStateActions);
			}
		}
	}

	private void SetInitialStateForDevice(IVrccDevice vrccDevice, LogicalDeviceState newLogicalDeviceState)
	{
		if (vrccDevice is UnderlyingDevice underlyingDevice && newLogicalDeviceState != null && newLogicalDeviceState.GetProperties().Count != 0)
		{
			underlyingDevice.StateValue = newLogicalDeviceState.GetStateValue();
		}
	}

	private void RaiseStateChange(IEnumerable<ActionDescription> setStateActions)
	{
		this.OnVrccStateUpdate?.Invoke(new VrccStateHandlerUpdateArgs(setStateActions));
	}

	public static void AddDefaultVrccStateUpdateEventHandler(VrccStateHandler vrccStateHandler, IProtocolMultiplexer multiplexer)
	{
		vrccStateHandler.OnVrccStateUpdate = (Action<VrccStateHandlerUpdateArgs>)Delegate.Combine(vrccStateHandler.OnVrccStateUpdate, (Action<VrccStateHandlerUpdateArgs>)delegate(VrccStateHandlerUpdateArgs args)
		{
			foreach (ActionDescription stateUpdateAction in args.StateUpdateActions)
			{
				multiplexer.DeviceController.ExecuteAction(new ActionContext(ContextType.ClimateControlSync, Guid.NewGuid()), stateUpdateAction);
			}
		});
	}

	private T GetCachedVrccDevice<T>(Guid id) where T : class, IVrccDevice
	{
		if (vrccs.TryGetValue(id, out var value))
		{
			return value as T;
		}
		return null;
	}
}
