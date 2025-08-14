using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.DeviceManager.BidcosKeyRequests;

internal class BidcosKeyRequestAsync
{
	private readonly SGTIN96 deviceSgtin;

	private readonly IEventManager eventManager;

	private readonly Action<SGTIN96, byte[]> onKeyRetrievedCallback;

	private readonly SubscriptionToken subscriptionToken;

	private bool isFinished;

	private DateTime requestedTime;

	public BidcosKeyRequestAsync(IEventManager eventManager, SGTIN96 deviceSgtin, Action<SGTIN96, byte[]> onKeyRetrievedCallback)
	{
		this.eventManager = eventManager;
		this.deviceSgtin = deviceSgtin;
		this.onKeyRetrievedCallback = onKeyRetrievedCallback;
		isFinished = false;
		subscriptionToken = this.eventManager.GetEvent<CheckDeviceInclusionResultEvent>().Subscribe(OnBackendRequestKeyResponse, null, ThreadOption.BackgroundThread, null);
	}

	public void RetrieveKeyAync()
	{
		requestedTime = DateTime.Now;
		CheckDeviceInclusionEventArgs e = new CheckDeviceInclusionEventArgs();
		e.Sgtin = deviceSgtin.GetSerialData().ToArray();
		CheckDeviceInclusionEventArgs payload = e;
		eventManager.GetEvent<GetDeviceKeyEvent>().Publish(payload);
	}

	public bool IsRequestFinished()
	{
		return isFinished;
	}

	public DateTime GetRequestedTime()
	{
		return requestedTime;
	}

	public SGTIN96 GetDeviceSGTIN()
	{
		return deviceSgtin;
	}

	private void OnBackendRequestKeyResponse(GetDeviceKeyEventArgs args)
	{
		if (args.Sgtin.Equals(deviceSgtin))
		{
			switch (args.Result)
			{
			case EncryptedKeyResponseStatus.BackendServiceNotReachable:
				Log.Information(Module.DeviceManager, "Backend service not available. No BidCos device key retrieved.");
				break;
			case EncryptedKeyResponseStatus.Success:
				InvokeKeyRetrievedCallback(args.DeviceKey);
				Log.Information(Module.DeviceManager, $"Received key for BidCos device (sgtin:{args.Sgtin.ToString()}).");
				break;
			default:
				Log.Error(Module.DeviceManager, $"Could not retrieve key for generic BidCos devices (sgtin:{args.Sgtin.ToString()}).");
				break;
			}
			eventManager.GetEvent<CheckDeviceInclusionResultEvent>().Unsubscribe(subscriptionToken);
			isFinished = true;
		}
	}

	private void InvokeKeyRetrievedCallback(byte[] deviceKey)
	{
		if (onKeyRetrievedCallback != null)
		{
			onKeyRetrievedCallback(deviceSgtin, deviceKey);
		}
	}
}
