using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

internal class RefreshConfigCache
{
	private Dictionary<Guid, string> bds = new Dictionary<Guid, string>();

	private Dictionary<Guid, string> lds = new Dictionary<Guid, string>();

	public void AddBaseDevice(Guid id, string appId)
	{
		bds.Add(id, appId);
	}

	public void AddLogicalDevice(Guid id, string appId)
	{
		lds.Add(id, appId);
	}

	public string GetBaseDeviceAppId(Guid id)
	{
		if (!bds.ContainsKey(id))
		{
			return string.Empty;
		}
		return bds[id];
	}

	public string GetLogicalDeviceAppId(Guid id)
	{
		if (!lds.ContainsKey(id))
		{
			return string.Empty;
		}
		return lds[id];
	}
}
