using System;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class ApplicationsTokenHandler : ICommandHandler, IBaseCommandHandler
{
	private readonly ITokenCache tokenCache;

	private readonly IApplicationsHost appsHost;

	private readonly INotificationHandler notificationHandler;

	private readonly IBusinessLogic businessLogic;

	internal ApplicationsTokenHandler(ITokenCache tokenCache, IApplicationsHost appsHost, INotificationHandler notificationHandler, IBusinessLogic businessLogic)
	{
		this.tokenCache = tokenCache;
		this.appsHost = appsHost;
		this.notificationHandler = notificationHandler;
		this.businessLogic = businessLogic;
	}

	public void Initialize()
	{
		if (appsHost != null)
		{
			appsHost.ApplicationsLoaded += OnTokenUpdated;
			appsHost.ApplicationStateChanged += OnApplicationStateChanged;
		}
	}

	public void Uninitialize()
	{
		if (appsHost != null)
		{
			appsHost.ApplicationsLoaded -= OnTokenUpdated;
			appsHost.ApplicationStateChanged -= OnApplicationStateChanged;
		}
	}

	BaseResponse ICommandHandler.HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is GetApplicationTokenRequest getApplicationTokenRequest)
		{
			using TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("ApplicationsTokenHandler/GetAppTokenReq");
			GetApplicationTokenResponse getApplicationTokenResponse = new GetApplicationTokenResponse();
			getApplicationTokenResponse.CorrespondingRequestId = getApplicationTokenRequest.RequestId;
			getApplicationTokenResponse.Token = tokenCacheContext.AppsToken;
			return getApplicationTokenResponse;
		}
		if (request is GetApplicationTokenHashRequest getApplicationTokenHashRequest)
		{
			using TokenCacheContext tokenCacheContext2 = tokenCache.GetAndLockCurrentToken("ApplicationsTokenHandler/GetAppTokenHashReq");
			GetApplicationTokenHashResponse getApplicationTokenHashResponse = new GetApplicationTokenHashResponse();
			getApplicationTokenHashResponse.CorrespondingRequestId = getApplicationTokenHashRequest.RequestId;
			getApplicationTokenHashResponse.Hash = ((tokenCacheContext2.AppsToken == null) ? string.Empty : tokenCacheContext2.AppsToken.GetHash());
			return getApplicationTokenHashResponse;
		}
		if (request is RefreshApplicationTokenRequest refreshApplicationTokenRequest)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				businessLogic.PerformBackendCommunicationWithRetries("Failed to update token.", (bool lastTrial) => tokenCache.UpdateToken("ApplicationsTokenHandler/RefreshAppTokenReq"));
			});
			AcknowledgeResponse acknowledgeResponse = new AcknowledgeResponse();
			acknowledgeResponse.CorrespondingRequestId = refreshApplicationTokenRequest.RequestId;
			return acknowledgeResponse;
		}
		return null;
	}

	private void OnTokenUpdated()
	{
		notificationHandler.SendNotification(new InvalidateTokenCacheNotification());
	}

	private void OnApplicationStateChanged(ApplicationLoadStateChangedEventArgs obj)
	{
		notificationHandler.SendNotification(new InvalidateTokenCacheNotification
		{
			AppId = obj.Application.ApplicationId,
			AppVersion = obj.ApplicationVersion,
			Properties = obj.Parameters
		});
	}
}
