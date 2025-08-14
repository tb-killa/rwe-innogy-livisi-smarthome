using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Mobile.ContainerModel;
using Microsoft.Win32;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceMonitoring;

public class DeviceMonitor : IDeviceMonitor
{
	private bool first = true;

	private List<BasicDriverInformation> activeDrivers = new List<BasicDriverInformation>();

	private IEventManager eventManager;

	private SubscriptionToken usbDeviceNotificationEvent;

	public List<BasicDriverInformation> MonitoredActiveDrivers => activeDrivers;

	public event Action<BasicDriverInformation> DeviceConnected;

	public DeviceMonitor(Container container)
	{
		eventManager = container.Resolve<IEventManager>();
	}

	public void Start()
	{
		usbDeviceNotificationEvent = eventManager.GetEvent<USBDeviceNotificationEvent>().Subscribe(OnUSBDeviceConfigurationChanged, null, ThreadOption.PublisherThread, null);
		CheckLoadedDrivers();
	}

	public void Stop()
	{
		if (usbDeviceNotificationEvent != null)
		{
			eventManager.GetEvent<USBDeviceNotificationEvent>().Unsubscribe(usbDeviceNotificationEvent);
			usbDeviceNotificationEvent = null;
		}
	}

	private List<BasicDriverInformation> ListActiveDrivers()
	{
		List<BasicDriverInformation> list = new List<BasicDriverInformation>();
		using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Drivers\\Active");
		List<string> list2 = registryKey.GetSubKeyNames().ToList();
		foreach (string item2 in list2)
		{
			RegistryKey registryKey2 = registryKey.OpenSubKey(item2);
			BasicDriverInformation item = new BasicDriverInformation
			{
				Index = item2,
				Name = (string)registryKey2.GetValue("Name"),
				Key = (string)registryKey2.GetValue("Key")
			};
			list.Add(item);
		}
		return list;
	}

	private void CheckLoadedDrivers()
	{
		try
		{
			List<BasicDriverInformation> list = ListActiveDrivers();
			if (!first)
			{
				List<BasicDriverInformation> list2 = new List<BasicDriverInformation>();
				BasicDriverInformation currentlyActiveDriver;
				foreach (BasicDriverInformation item in list)
				{
					currentlyActiveDriver = item;
					if (!MonitoredActiveDrivers.Any(delegate(BasicDriverInformation activeDriver)
					{
						string key = activeDriver.Key;
						BasicDriverInformation basicDriverInformation = currentlyActiveDriver;
						if (key == basicDriverInformation.Key)
						{
							string name = activeDriver.Name;
							BasicDriverInformation basicDriverInformation2 = currentlyActiveDriver;
							return name == basicDriverInformation2.Name;
						}
						return false;
					}))
					{
						list2.Add(currentlyActiveDriver);
					}
				}
				List<BasicDriverInformation> list3 = new List<BasicDriverInformation>();
				BasicDriverInformation currentlyActiveDriver2;
				foreach (BasicDriverInformation monitoredActiveDriver in MonitoredActiveDrivers)
				{
					currentlyActiveDriver2 = monitoredActiveDriver;
					if (!list.Any(delegate(BasicDriverInformation activeDriver)
					{
						string key = activeDriver.Key;
						BasicDriverInformation basicDriverInformation = currentlyActiveDriver2;
						if (key == basicDriverInformation.Key)
						{
							string name = activeDriver.Name;
							BasicDriverInformation basicDriverInformation2 = currentlyActiveDriver2;
							return name == basicDriverInformation2.Name;
						}
						return false;
					}))
					{
						list3.Add(currentlyActiveDriver2);
					}
				}
				activeDrivers = list;
				foreach (BasicDriverInformation item2 in list2)
				{
					OnDeviceConnected(item2, active: true);
				}
				{
					foreach (BasicDriverInformation item3 in list3)
					{
						OnDeviceConnected(item3, active: false);
					}
					return;
				}
			}
			first = false;
			activeDrivers = list;
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Device Monitoring failed {ex.Message}");
		}
	}

	private void OnUSBDeviceConfigurationChanged(USBDeviceNotificationEventArgs args)
	{
		CheckLoadedDrivers();
	}

	private void OnDeviceConnected(BasicDriverInformation key, bool active)
	{
		key.Active = active;
		this.DeviceConnected?.Invoke(key);
	}
}
