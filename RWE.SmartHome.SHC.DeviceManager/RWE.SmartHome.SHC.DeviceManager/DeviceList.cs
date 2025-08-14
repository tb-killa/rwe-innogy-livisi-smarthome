using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

namespace RWE.SmartHome.SHC.DeviceManager;

public class DeviceList : IDeviceList, IEnumerable<IDeviceInformation>, IEnumerable
{
	private const string LogInfoErrorString = "[unknown or non-included device]";

	private readonly object syncRoot = new object();

	private readonly Dictionary<Guid, IDeviceInformation> deviceList = new Dictionary<Guid, IDeviceInformation>();

	private bool containsRouter;

	private readonly int routerLimit = 1;

	public object SyncRoot => syncRoot;

	public bool ContainsRouter
	{
		get
		{
			if (!ForceDetectionOfRouters.HasValue || ForceDetectionOfRouters.Value)
			{
				ForceDetectionOfRouters = false;
				lock (SyncRoot)
				{
					containsRouter = deviceList.Values.Any((IDeviceInformation device) => device != null && !device.DeviceUnreachable && device.DeviceInclusionState == DeviceInclusionState.Included && device.ManufacturerDeviceType == 10);
				}
			}
			return containsRouter;
		}
		set
		{
			containsRouter = value;
			ForceDetectionOfRouters = false;
		}
	}

	public bool? ForceDetectionOfRouters { private get; set; }

	public IDeviceInformation this[Guid deviceId]
	{
		get
		{
			lock (SyncRoot)
			{
				return deviceList.ContainsKey(deviceId) ? deviceList[deviceId] : null;
			}
		}
		set
		{
			lock (SyncRoot)
			{
				deviceList[deviceId] = value;
			}
		}
	}

	public IDeviceInformation this[byte[] address]
	{
		get
		{
			lock (SyncRoot)
			{
				return deviceList.Values.FirstOrDefault((IDeviceInformation info) => info.Address.Compare(address));
			}
		}
	}

	public event EventHandler<DeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	public event EventHandler<DeviceConfiguredEventArgs> DeviceConfiguredStateChanged;

	public event EventHandler<DeviceUpdateStateChangedEventArgs> DeviceUpdateStateChanged;

	public void AddDevice(IDeviceInformation deviceInformation)
	{
		deviceInformation.DeviceInclusionStateChanged += InvokeDeviceInclusionStateChanged;
		deviceInformation.DeviceConfiguredStateChanged += InvokeDeviceConfiguredStateChanged;
		deviceInformation.DeviceUpdateStateChanged += InvokeDeviceUpdateStateChanged;
		lock (SyncRoot)
		{
			deviceList.Add(deviceInformation.DeviceId, deviceInformation);
		}
	}

	public bool Contains(Guid deviceId)
	{
		lock (SyncRoot)
		{
			return deviceList.ContainsKey(deviceId);
		}
	}

	public bool Contains(byte[] address)
	{
		lock (SyncRoot)
		{
			return deviceList.Values.Any((IDeviceInformation info) => info.Address.Compare(address));
		}
	}

	public bool ContainsSGTIN(byte[] sgtin)
	{
		lock (SyncRoot)
		{
			return deviceList.Values.Any((IDeviceInformation info) => info.Sgtin.Compare(sgtin));
		}
	}

	public IDeviceInformation GetBySGTIN(byte[] sgtin)
	{
		lock (syncRoot)
		{
			return deviceList.Values.FirstOrDefault((IDeviceInformation info) => info.Sgtin.Compare(sgtin));
		}
	}

	public IEnumerator<IDeviceInformation> GetEnumerator()
	{
		Dictionary<Guid, IDeviceInformation>.Enumerator enumerator = deviceList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current.Value;
		}
	}

	public void Remove(Guid deviceId)
	{
		IDeviceInformation deviceInformation = this[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceInclusionStateChanged -= InvokeDeviceInclusionStateChanged;
			deviceInformation.DeviceConfiguredStateChanged -= InvokeDeviceConfiguredStateChanged;
		}
		lock (SyncRoot)
		{
			deviceList.Remove(deviceId);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public string LogInfoByAddress(byte[] address)
	{
		try
		{
			return LogInfoByDeviceInfo(this[address]);
		}
		catch
		{
			return "[unknown or non-included device]";
		}
	}

	public string LogInfoBySGTIN(byte[] sgtin)
	{
		try
		{
			return LogInfoByDeviceInfo(GetBySGTIN(sgtin));
		}
		catch
		{
			return "[unknown or non-included device]";
		}
	}

	public string LogInfoByGuid(Guid guid)
	{
		try
		{
			return LogInfoByDeviceInfo(this[guid]);
		}
		catch
		{
			return "[unknown or non-included device]";
		}
	}

	public string LogInfoByDeviceInfo(IDeviceInformation deviceInfo)
	{
		try
		{
			if (deviceInfo == null)
			{
				return "[unknown or non-included device]";
			}
			return deviceInfo.ToString();
		}
		catch
		{
			return "[unknown or non-included device]";
		}
	}

	public void InvokeDeviceInclusionStateChanged(object sender, DeviceInclusionStateChangedEventArgs e)
	{
		this.DeviceInclusionStateChanged?.Invoke(sender, e);
	}

	public void InvokeDeviceConfiguredStateChanged(object sender, DeviceConfiguredEventArgs e)
	{
		this.DeviceConfiguredStateChanged?.Invoke(sender, e);
	}

	public void InvokeDeviceUpdateStateChanged(object sender, DeviceUpdateStateChangedEventArgs e)
	{
		this.DeviceUpdateStateChanged?.Invoke(sender, e);
	}

	public bool CanIncludeRouter()
	{
		lock (SyncRoot)
		{
			List<IDeviceInformation> list = deviceList.Values.Where((IDeviceInformation device) => device != null && (device.DeviceInclusionState == DeviceInclusionState.Included || device.DeviceInclusionState == DeviceInclusionState.InclusionPending) && device.ManufacturerDeviceType == 10).ToList();
			return list.Count < routerLimit;
		}
	}
}
