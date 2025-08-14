using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using SHCWrapper.Misc;

namespace RWE.SmartHome.SHC.DeviceManager;

internal class AddressGenerator
{
	private readonly DeviceList deviceList;

	private readonly byte numberOfShcAddresses;

	private readonly byte sendRetries;

	private readonly ISipCosPersistence sipCosPersistence;

	private readonly SIPCosNetworkParameter loadedSipCosNetworkParameter;

	private readonly ICommunicationWrapper communicationWrapper;

	public AddressGenerator(ICommunicationWrapper communicationWrapper, DeviceList deviceList, ISipCosPersistence sipCosPersistence, byte numberOfShcAddresses, byte sendRetries)
	{
		this.communicationWrapper = communicationWrapper;
		this.sipCosPersistence = sipCosPersistence;
		this.sendRetries = sendRetries;
		this.numberOfShcAddresses = numberOfShcAddresses;
		this.deviceList = deviceList;
		loadedSipCosNetworkParameter = sipCosPersistence.LoadSIPCosNetworkParameter();
	}

	private byte[] GetAndRemoveOldAddress()
	{
		byte[] result = null;
		byte[] sgtin = SGTIN.GetSGTIN().ToByteArray();
		IDeviceInformation bySGTIN;
		lock (deviceList.SyncRoot)
		{
			bySGTIN = deviceList.GetBySGTIN(sgtin);
		}
		if (bySGTIN != null)
		{
			result = bySGTIN.Address;
		}
		return result;
	}

	public List<byte[]> DefineAddresses()
	{
		bool flag = false;
		bool flag2 = false;
		List<byte[]> shcAddresses = loadedSipCosNetworkParameter.ShcAddresses;
		if (shcAddresses.Count == 0)
		{
			byte[] andRemoveOldAddress = GetAndRemoveOldAddress();
			if (andRemoveOldAddress != null)
			{
				flag = true;
				flag2 = true;
				shcAddresses.Add(andRemoveOldAddress);
			}
		}
		while (shcAddresses.Count < numberOfShcAddresses)
		{
			shcAddresses.Add(GenerateUniqueAddress());
			flag2 = true;
		}
		byte[] address = null;
		CommandExecutor.ExecuteCommand(sendRetries, () => communicationWrapper.CommandHandler.GetDefaultIP(out address), "GetDefaultIP");
		if (!address.Compare(shcAddresses[0]))
		{
			CommandExecutor.ExecuteCommand(sendRetries, () => communicationWrapper.CommandHandler.SetDefaultIP(shcAddresses[0]), "SetDefaultIP");
		}
		List<byte[]> ips = new List<byte[]>();
		List<byte[]> list = new List<byte[]>(shcAddresses);
		CommandExecutor.ExecuteCommand(sendRetries, () => communicationWrapper.CommandHandler.GetRegisteredIPS(ref ips), "GetRegisteredIPs");
		byte[] currentAddress;
		foreach (byte[] item in ips)
		{
			currentAddress = item;
			byte[] array = list.Find((byte[] c) => c.Compare(currentAddress));
			if (array == null)
			{
				CommandExecutor.ExecuteCommand(sendRetries, () => communicationWrapper.CommandHandler.UnregisterIP(currentAddress), "UnregisterIP");
			}
			else
			{
				list.Remove(array);
			}
		}
		byte[] shcAddress;
		foreach (byte[] item2 in list)
		{
			shcAddress = item2;
			CommandExecutor.ExecuteCommand(sendRetries, () => communicationWrapper.CommandHandler.RegisterIP(shcAddress), "RegisterIP");
		}
		if (flag2)
		{
			sipCosPersistence.SaveSIPCosNetworkParameterInTransaction(loadedSipCosNetworkParameter, suppressEvent: false);
		}
		if (flag)
		{
			Guid deviceId = deviceList[shcAddresses[0]].DeviceId;
			sipCosPersistence.DeleteInTransaction(deviceId, suppressEvent: false);
			deviceList.Remove(deviceId);
		}
		return loadedSipCosNetworkParameter.ShcAddresses;
	}

	private byte[] GenerateUniqueAddress()
	{
		byte[] desiredAddress;
		do
		{
			desiredAddress = RandomByteGenerator.Instance.GenerateRandomByteSequence(3u);
		}
		while (desiredAddress[0] > 191 || (desiredAddress[0] == 0 && desiredAddress[1] == 0) || deviceList.Contains(desiredAddress) || loadedSipCosNetworkParameter.ShcAddresses.Exists((byte[] address) => address.Compare(desiredAddress)));
		return desiredAddress;
	}
}
