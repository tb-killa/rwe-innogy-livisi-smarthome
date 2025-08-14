using System;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.DeviceManager.BidcosKeyRequests;

internal class BidcosKeyRequest
{
	private readonly SGTIN96 deviceSgtin;

	private readonly IEventManager eventManager;

	private readonly EventWaitHandle replyWaitHandler = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);

	private readonly SubscriptionToken subscriptionToken;

	private readonly Action<SGTIN96, byte[]> onKeyRetrieved;

	private byte[] deviceKey;

	private int responseTimeout;

	public BidcosKeyRequest(IEventManager eventManager, SGTIN96 deviceSgtin, Action<SGTIN96, byte[]> onKeyRetrieved, int responseTimeout)
	{
		this.eventManager = eventManager;
		this.deviceSgtin = deviceSgtin;
		this.responseTimeout = responseTimeout;
		subscriptionToken = this.eventManager.GetEvent<CheckDeviceInclusionResultEvent>().Subscribe(OnBackendRequestKeyResponse, null, ThreadOption.BackgroundThread, null);
		this.onKeyRetrieved = onKeyRetrieved;
	}

	private void OnBackendRequestKeyResponse(GetDeviceKeyEventArgs args)
	{
		if (!args.Sgtin.Equals(deviceSgtin))
		{
			return;
		}
		switch (args.Result)
		{
		case EncryptedKeyResponseStatus.BackendServiceNotReachable:
			Log.Information(Module.DeviceManager, "Backend service not available. No BidCos device key retrieved.");
			break;
		case EncryptedKeyResponseStatus.Success:
			deviceKey = args.DeviceKey;
			if (onKeyRetrieved != null)
			{
				onKeyRetrieved(deviceSgtin, deviceKey);
			}
			Log.Information(Module.DeviceManager, $"Received key for BidCos device (sgtin:{args.Sgtin.ToString()}).");
			break;
		default:
			Log.Error(Module.DeviceManager, $"Could not retrieve key for generic BidCos devices (sgtin:{args.Sgtin.ToString()}).");
			break;
		}
		replyWaitHandler.Set();
		eventManager.GetEvent<CheckDeviceInclusionResultEvent>().Unsubscribe(subscriptionToken);
	}

	public byte[] RetrieveKey()
	{
		CheckDeviceInclusionEventArgs e = new CheckDeviceInclusionEventArgs();
		e.Sgtin = deviceSgtin.GetSerialData().ToArray();
		CheckDeviceInclusionEventArgs payload = e;
		eventManager.GetEvent<GetDeviceKeyEvent>().Publish(payload);
		replyWaitHandler.WaitOne(responseTimeout, exitContext: false);
		return deviceKey;
	}
}
