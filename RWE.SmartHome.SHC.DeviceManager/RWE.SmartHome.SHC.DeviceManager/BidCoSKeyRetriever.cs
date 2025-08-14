using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DeviceManager.BidcosKeyRequests;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager;

internal class BidCoSKeyRetriever : IBidCoSKeyRetriever
{
	private readonly IEventManager eventManager;

	private readonly IDeviceKeyRepository deviceKeyRepository;

	private readonly Dictionary<SGTIN96, byte[]> keys = new Dictionary<SGTIN96, byte[]>();

	private readonly object syncRoot = new object();

	private readonly BidcosKeyRequestsAsyncManager requestsAsyncManager;

	public BidCoSKeyRetriever(IEventManager eventManager, IDeviceKeyRepository deviceKeyRepository)
	{
		this.eventManager = eventManager;
		this.deviceKeyRepository = deviceKeyRepository;
		requestsAsyncManager = new BidcosKeyRequestsAsyncManager(eventManager);
	}

	public void GetDeviceKey(SGTIN96 deviceSgtin, Action<byte[]> keyRetrieved, Action keyRetrievalFailed, int timeout)
	{
		byte[] array = null;
		lock (syncRoot)
		{
			if (keys.ContainsKey(deviceSgtin))
			{
				array = keys[deviceSgtin];
			}
		}
		if (array != null)
		{
			keyRetrieved?.Invoke(keys[deviceSgtin]);
			return;
		}
		byte[] deviceKeyFromCsvStorage = deviceKeyRepository.GetDeviceKeyFromCsvStorage(deviceSgtin);
		if (deviceKeyFromCsvStorage != null)
		{
			keys.Add(deviceSgtin, deviceKeyFromCsvStorage);
			keyRetrieved(deviceKeyFromCsvStorage);
			return;
		}
		BidcosKeyRequest bidcosKeyRequest = new BidcosKeyRequest(eventManager, deviceSgtin, delegate(SGTIN96 id, byte[] newKey)
		{
			lock (syncRoot)
			{
				if (!keys.ContainsKey(id))
				{
					keys.Add(id, newKey);
				}
			}
		}, timeout);
		array = bidcosKeyRequest.RetrieveKey();
		if (array != null)
		{
			keyRetrieved?.Invoke(array);
		}
		else
		{
			keyRetrievalFailed?.Invoke();
		}
	}

	public void StoreDeviceKeyInCache(SGTIN96 deviceSgtin)
	{
		bool flag = false;
		lock (syncRoot)
		{
			flag = keys.ContainsKey(deviceSgtin);
		}
		if (flag)
		{
			return;
		}
		byte[] deviceKeyFromCsvStorage = deviceKeyRepository.GetDeviceKeyFromCsvStorage(deviceSgtin);
		if (deviceKeyFromCsvStorage != null)
		{
			keys.Add(deviceSgtin, deviceKeyFromCsvStorage);
			return;
		}
		requestsAsyncManager.RequestKey(deviceSgtin, delegate(SGTIN96 id, byte[] newKey)
		{
			lock (syncRoot)
			{
				if (!keys.ContainsKey(id))
				{
					keys.Add(id, newKey);
				}
			}
		});
	}
}
