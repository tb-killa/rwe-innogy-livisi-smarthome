using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

internal class ShcBaseDeviceWatchers : IShcBaseDeviceWatchers
{
	private List<IShcBaseDeviceWatcher> watchers = new List<IShcBaseDeviceWatcher>();

	public void RegisterWatcher(IShcBaseDeviceWatcher watcher)
	{
		lock (watchers)
		{
			if (!watchers.Contains(watcher))
			{
				watchers.Add(watcher);
			}
		}
	}

	public void UnregisterWatcher(IShcBaseDeviceWatcher watcher)
	{
		lock (watchers)
		{
			watchers.Remove(watcher);
		}
	}

	public void ProcessUpdate(Property[] originalBaseDeviceProperties, Property[] baseDeviceProperties)
	{
		lock (watchers)
		{
			watchers.ForEach(delegate(IShcBaseDeviceWatcher w)
			{
				w.ProcessUpdate(originalBaseDeviceProperties, baseDeviceProperties);
			});
		}
	}
}
