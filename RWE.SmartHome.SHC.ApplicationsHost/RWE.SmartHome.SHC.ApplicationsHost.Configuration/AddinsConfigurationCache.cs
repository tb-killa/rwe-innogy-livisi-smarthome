using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.SHC.API.Configuration;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

public class AddinsConfigurationCache
{
	private readonly Dictionary<string, AddinConfiguration> addinsCaches = new Dictionary<string, AddinConfiguration>();

	public AddinConfiguration GetAddinConfiguration(string appId)
	{
		addinsCaches.TryGetValue(appId, out var value);
		if (value != null)
		{
			return value;
		}
		value = new AddinConfiguration(appId);
		addinsCaches.Add(appId, value);
		return value;
	}

	public IEnumerable<string> ListAddinsIds()
	{
		return addinsCaches.Keys.ToList();
	}

	public void AddDevice(string appId, Device device)
	{
		GetAddinConfiguration(appId).Devices.Add(device);
	}

	public void AddCapability(string appId, Capability capability)
	{
		GetAddinConfiguration(appId).Capabilities.Add(capability);
	}

	public void AddActionDescription(string appId, ActionDescription action)
	{
		GetAddinConfiguration(appId).ActionDescriptions.Add(action);
	}

	public void AddTrigger(string appId, Trigger trigger)
	{
		GetAddinConfiguration(appId).Triggers.Add(trigger);
	}

	public void AddCustomTrigger(string appId, CustomTrigger trigger)
	{
		GetAddinConfiguration(appId).CustomTriggers.Add(trigger);
	}

	internal AddinsConfigurationCache Clone()
	{
		AddinsConfigurationCache addinsConfigurationCache = new AddinsConfigurationCache();
		foreach (string key in addinsCaches.Keys)
		{
			addinsConfigurationCache.addinsCaches.Add(key, addinsCaches[key].Clone());
		}
		return addinsConfigurationCache;
	}

	internal void UpdateBaseDevice(string appId, Device entity)
	{
		AddinConfiguration addinConfiguration = GetAddinConfiguration(appId);
		addinConfiguration.Devices.RemoveAll((Device x) => x.Id == entity.Id);
		addinConfiguration.Devices.Add(entity);
	}

	internal void UpdateCapability(string appId, Capability entity)
	{
		AddinConfiguration addinConfiguration = GetAddinConfiguration(appId);
		addinConfiguration.Capabilities.RemoveAll((Capability x) => x.Id == entity.Id);
		addinConfiguration.Capabilities.Add(entity);
	}

	internal void DeleteBaseDevice(string appId, Guid entityId)
	{
		AddinConfiguration addinConfiguration = GetAddinConfiguration(appId);
		addinConfiguration.Devices.RemoveAll((Device x) => x.Id == entityId);
	}

	internal void DeleteLogicalDevice(string appId, Guid entityId)
	{
		AddinConfiguration addinConfiguration = GetAddinConfiguration(appId);
		addinConfiguration.Capabilities.RemoveAll((Capability x) => x.Id == entityId);
	}

	internal string GetLogicalDeviceAppId(Guid guid)
	{
		string result = null;
		foreach (string key in addinsCaches.Keys)
		{
			AddinConfiguration addinConfiguration = addinsCaches[key];
			Capability capability = addinConfiguration.Capabilities.FirstOrDefault((Capability x) => x.Id == guid);
			if (capability != null)
			{
				result = key;
				break;
			}
		}
		return result;
	}

	internal string GetBaseDeviceAppId(Guid guid)
	{
		string result = null;
		foreach (string key in addinsCaches.Keys)
		{
			AddinConfiguration addinConfiguration = addinsCaches[key];
			Device device = addinConfiguration.Devices.FirstOrDefault((Device x) => x.Id == guid);
			if (device != null)
			{
				result = key;
				break;
			}
		}
		return result;
	}

	internal void ClearInteractionsInfo()
	{
		foreach (string key in addinsCaches.Keys)
		{
			AddinConfiguration addinConfiguration = addinsCaches[key];
			addinConfiguration.Triggers.Clear();
			addinConfiguration.CustomTriggers.Clear();
			addinConfiguration.ActionDescriptions.Clear();
		}
	}
}
