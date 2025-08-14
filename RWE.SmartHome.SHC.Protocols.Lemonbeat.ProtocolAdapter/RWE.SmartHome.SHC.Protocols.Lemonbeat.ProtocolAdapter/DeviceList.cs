using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

public class DeviceList : IDeviceList
{
	private readonly Dictionary<Guid, DeviceInformation> deviceList = new Dictionary<Guid, DeviceInformation>();

	private readonly object syncRoot = new object();

	public DeviceInformation this[Guid deviceId]
	{
		get
		{
			lock (syncRoot)
			{
				return deviceList.ContainsKey(deviceId) ? deviceList[deviceId] : null;
			}
		}
		set
		{
			lock (syncRoot)
			{
				deviceList[deviceId] = value;
			}
		}
	}

	public DeviceInformation this[DeviceIdentifier address]
	{
		get
		{
			lock (syncRoot)
			{
				return deviceList.Values.FirstOrDefault((DeviceInformation info) => info.Identifier.Equals(address));
			}
		}
	}

	public DeviceInformation this[SGTIN96 sgtin]
	{
		get
		{
			lock (syncRoot)
			{
				return deviceList.Values.FirstOrDefault((DeviceInformation info) => info.DeviceDescription.SGTIN.Equals(sgtin));
			}
		}
	}

	public int Count
	{
		get
		{
			lock (syncRoot)
			{
				return deviceList.Count;
			}
		}
	}

	public event EventHandler<LemonbeatDeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	public event EventHandler<DeviceConfiguredEventArgs> DeviceConfiguredStateChanged;

	public event EventHandler<DeviceReachabilityChangedEventArgs> DeviceReachabilityChanged;

	public event EventHandler<DeviceUpdateStateChangedEventArgs> DeviceUpdateStateChanged;

	public void AddDevice(DeviceInformation deviceInformation)
	{
		deviceInformation.DeviceInclusionStateChanged += InvokeDeviceInclusionStateChanged;
		deviceInformation.DeviceConfiguredStateChanged += InvokeDeviceConfiguredStateChanged;
		deviceInformation.DeviceReachabilityChanged += InvokeDeviceReachabilityChanged;
		deviceInformation.DeviceUpdateStateChanged += InvokeDeviceUpdateStateChanged;
		lock (syncRoot)
		{
			deviceList.Add(deviceInformation.DeviceId, deviceInformation);
		}
	}

	public bool Contains(Guid deviceId)
	{
		lock (syncRoot)
		{
			return deviceList.ContainsKey(deviceId);
		}
	}

	public bool Contains(SGTIN96 sgtin)
	{
		lock (syncRoot)
		{
			return deviceList.Values.Any((DeviceInformation info) => info.DeviceDescription.SGTIN.Equals(sgtin));
		}
	}

	public bool Contains(IPAddress address)
	{
		lock (syncRoot)
		{
			return deviceList.Values.Any((DeviceInformation info) => info.Identifier.IPAddress.Equals(address));
		}
	}

	public List<DeviceInformation> SyncWhere(Func<DeviceInformation, bool> predicate)
	{
		lock (syncRoot)
		{
			return deviceList.Values.Where(predicate).ToList();
		}
	}

	public List<TResult> SyncSelect<TResult>(Func<DeviceInformation, TResult> selector)
	{
		lock (syncRoot)
		{
			return deviceList.Values.Select(selector).ToList();
		}
	}

	public IEnumerable<DeviceInformation> GetDevicesByIPAddress(IPAddress address)
	{
		lock (syncRoot)
		{
			return deviceList.Values.Where((DeviceInformation devInfo) => devInfo.Identifier.IPAddress.Equals(address)).ToList();
		}
	}

	public IEnumerator<DeviceInformation> GetEnumerator()
	{
		Dictionary<Guid, DeviceInformation>.Enumerator enumerator = deviceList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current.Value;
		}
	}

	public void Remove(Guid deviceId)
	{
		DeviceInformation deviceInformation = this[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceInclusionStateChanged -= InvokeDeviceInclusionStateChanged;
			deviceInformation.DeviceConfiguredStateChanged -= InvokeDeviceConfiguredStateChanged;
			deviceInformation.DeviceReachabilityChanged -= InvokeDeviceReachabilityChanged;
			deviceInformation.DeviceUpdateStateChanged -= InvokeDeviceUpdateStateChanged;
		}
		lock (syncRoot)
		{
			deviceList.Remove(deviceId);
		}
	}

	public void InvokeDeviceInclusionStateChanged(object sender, LemonbeatDeviceInclusionStateChangedEventArgs e)
	{
		this.DeviceInclusionStateChanged?.Invoke(sender, e);
	}

	public void InvokeDeviceConfiguredStateChanged(object sender, DeviceConfiguredEventArgs deviceConfiguredEventArgs)
	{
		this.DeviceConfiguredStateChanged?.Invoke(sender, deviceConfiguredEventArgs);
	}

	private void InvokeDeviceReachabilityChanged(object sender, DeviceReachabilityChangedEventArgs e)
	{
		this.DeviceReachabilityChanged?.Invoke(sender, e);
	}

	private void InvokeDeviceUpdateStateChanged(object sender, DeviceUpdateStateChangedEventArgs args)
	{
		this.DeviceUpdateStateChanged?.Invoke(sender, args);
	}
}
