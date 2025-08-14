using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal class DeviceList : IDeviceList, IEnumerable<IDeviceInformation>, IEnumerable
{
	private readonly List<IDeviceInformation> internalDeviceList = new List<IDeviceInformation>();

	private readonly object syncRoot = new object();

	public IDeviceInformation this[Guid deviceId] => internalDeviceList.FirstOrDefault((IDeviceInformation information) => information.DeviceId == deviceId);

	public IDeviceInformation this[byte[] identification] => internalDeviceList.FirstOrDefault((IDeviceInformation information) => information.DeviceIdentifier.Compare(identification));

	public object SyncRoot => syncRoot;

	public event EventHandler<DeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	public IEnumerator<IDeviceInformation> GetEnumerator()
	{
		List<IDeviceInformation>.Enumerator enumerator = internalDeviceList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public bool Contains(byte[] identification)
	{
		return internalDeviceList.Any((IDeviceInformation information) => information.DeviceIdentifier.Compare(identification));
	}

	public void AddDevice(IDeviceInformation deviceInformation)
	{
		deviceInformation.DeviceInclusionStateChanged += InvokeDeviceInclusionStateChanged;
		internalDeviceList.Add(deviceInformation);
	}

	public string LogInfoByGuid(Guid deviceId)
	{
		IDeviceInformation deviceInformation = internalDeviceList.FirstOrDefault((IDeviceInformation info) => info.DeviceId == deviceId);
		if (deviceInformation == null)
		{
			return string.Empty;
		}
		return deviceInformation.ToString();
	}

	public void Remove(Guid deviceId)
	{
		IDeviceInformation deviceInformation = this[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceInclusionStateChanged -= InvokeDeviceInclusionStateChanged;
		}
		lock (SyncRoot)
		{
			internalDeviceList.RemoveAll((IDeviceInformation devInfo) => devInfo.DeviceId == deviceId);
		}
	}

	public void InvokeDeviceInclusionStateChanged(object sender, DeviceInclusionStateChangedEventArgs e)
	{
		this.DeviceInclusionStateChanged?.Invoke(sender, e);
	}
}
