using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

internal class ProfileMulticastAddressProvider
{
	private Dictionary<Guid, IPAddress> profileAddresses;

	private readonly IDeviceList deviceList;

	private readonly ILemonbeatPersistence persistence;

	private readonly IRepository repository;

	private List<IPAddress> reservedLemonbeatMulticastAddresses = new List<IPAddress>
	{
		IPAddress.Parse("ff02::1"),
		IPAddress.Parse("ff02::2"),
		IPAddress.Parse("ff02::9"),
		IPAddress.Parse("ff02::16")
	};

	internal ProfileMulticastAddressProvider(IDeviceList deviceList, ILemonbeatPersistence persistence, IRepository repository)
	{
		this.deviceList = deviceList;
		this.persistence = persistence;
		this.repository = repository;
	}

	internal IPAddress GetProfileMulticastAddress(Guid profileId)
	{
		if (profileAddresses == null || !profileAddresses.ContainsKey(profileId))
		{
			Refresh();
		}
		profileAddresses.TryGetValue(profileId, out var value);
		return value;
	}

	private void Refresh()
	{
		profileAddresses = new Dictionary<Guid, IPAddress>();
		Dictionary<Guid, IPAddress> dictionary = persistence.LoadProfileMulticastAddresses();
		foreach (KeyValuePair<Guid, IPAddress> item in dictionary)
		{
			profileAddresses.Add(item.Key, item.Value);
		}
	}

	private IPAddress GenerateMulticastAddress()
	{
		IPAddress candidate;
		do
		{
			byte[] array = Guid.NewGuid().ToByteArray();
			array[0] = byte.MaxValue;
			array[1] = 2;
			for (int i = 2; i < 10; i++)
			{
				array[i] = 0;
			}
			candidate = new IPAddress(array);
		}
		while (profileAddresses.Values.Any((IPAddress pi) => pi.Equals(candidate)) && !reservedLemonbeatMulticastAddresses.Contains(candidate));
		return candidate;
	}
}
