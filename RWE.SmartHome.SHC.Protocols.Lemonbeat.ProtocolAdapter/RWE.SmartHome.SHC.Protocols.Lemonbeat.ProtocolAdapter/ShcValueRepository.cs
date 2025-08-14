using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

internal class ShcValueRepository : IShcValueRepository
{
	private object syncRoot = new object();

	private Dictionary<string, Dictionary<string, uint>> valueDictionary;

	private Dictionary<string, Dictionary<string, uint>> tempValueDictionary;

	private uint nextValueId;

	private ILemonbeatPersistence persistence;

	private IApplicationsHost applicationsHost;

	public ShcValueRepository(ILemonbeatPersistence LemonbeatPersistence, IApplicationsHost applicationsHost, IEventManager eventManager)
	{
		persistence = LemonbeatPersistence;
		this.applicationsHost = applicationsHost;
		applicationsHost.ApplicationStateChanged += OnApplicationUninstalled;
		ShcStartupCompletedEvent shcStartupCompletedEvent = eventManager.GetEvent<ShcStartupCompletedEvent>();
		shcStartupCompletedEvent.Subscribe(Initialize, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.DatabaseAvailable, ThreadOption.PublisherThread, null);
	}

	internal void Initialize(ShcStartupCompletedEventArgs args)
	{
		lock (syncRoot)
		{
			valueDictionary = persistence.LoadAllShcValues();
		}
		IEnumerable<KeyValuePair<string, uint>> source = valueDictionary.SelectMany((KeyValuePair<string, Dictionary<string, uint>> appValues) => appValues.Value);
		if (source.Any())
		{
			nextValueId = source.Max((KeyValuePair<string, uint> registeredValue) => registeredValue.Value) + 1;
		}
		else
		{
			nextValueId = 1u;
		}
	}

	public uint RegisterValue(string appId, string valueName)
	{
		if (tempValueDictionary == null)
		{
			throw new InvalidOperationException("Repository not in transaction. The operation can only be done during the transformation.");
		}
		lock (syncRoot)
		{
			if (!tempValueDictionary.ContainsKey(appId))
			{
				tempValueDictionary[appId] = new Dictionary<string, uint>();
			}
			tempValueDictionary[appId][valueName] = nextValueId;
			return nextValueId++;
		}
	}

	public uint GetValueId(string appId, string valueName)
	{
		lock (syncRoot)
		{
			return valueDictionary[appId][valueName];
		}
	}

	public Dictionary<string, uint> GetUsedValues(string appId)
	{
		lock (syncRoot)
		{
			return valueDictionary.ContainsKey(appId) ? valueDictionary[appId] : null;
		}
	}

	public bool TryGetValueId(string appId, string name, out uint valueId)
	{
		lock (syncRoot)
		{
			if (valueDictionary.TryGetValue(appId, out var value))
			{
				return value.TryGetValue(name, out valueId);
			}
		}
		valueId = 0u;
		return false;
	}

	public void RemoveValue(string appId, string name)
	{
		if (tempValueDictionary == null)
		{
			throw new InvalidOperationException("Repository not in transaction. The operation can only be done during the transformation.");
		}
		lock (syncRoot)
		{
			if (tempValueDictionary.TryGetValue(appId, out var value))
			{
				value.Remove(name);
			}
		}
	}

	public void BeginUpdate()
	{
		if (tempValueDictionary != null)
		{
			return;
		}
		tempValueDictionary = valueDictionary.ToDictionary((KeyValuePair<string, Dictionary<string, uint>> kv) => kv.Key, (KeyValuePair<string, Dictionary<string, uint>> kv) => kv.Value.ToDictionary((KeyValuePair<string, uint> k) => k.Key, (KeyValuePair<string, uint> k) => k.Value));
	}

	public void CommitChanges()
	{
		if (tempValueDictionary == null)
		{
			throw new InvalidOperationException("Repository not in transaction. The operation can only be done during the transformation.");
		}
		valueDictionary = tempValueDictionary;
		tempValueDictionary = null;
		persistence.SaveAllShcValues(valueDictionary);
	}

	public void RollbackChanges()
	{
		tempValueDictionary = null;
	}

	private void OnApplicationUninstalled(ApplicationLoadStateChangedEventArgs obj)
	{
		if (obj.ApplicationState == ApplicationStates.ApplicationsUninstalled)
		{
			lock (syncRoot)
			{
				valueDictionary.Remove(obj.Application.ApplicationId);
			}
		}
	}
}
