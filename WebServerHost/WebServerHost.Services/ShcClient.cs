using System;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ChannelInterfaces;
using SmartHome.Common.Generic.LogManager;

namespace WebServerHost.Services;

internal class ShcClient : IShcClient
{
	private ILogger logger = LogManager.Instance.GetLogger<ShcClient>();

	private IRequestProcessor requestProcessor;

	public ShcClient(IRequestProcessor requestProcessor)
	{
		this.requestProcessor = requestProcessor;
	}

	public BaseResponse GetResponse(BaseRequest request)
	{
		BaseResponse response = null;
		if (requestProcessor.ProcessRequest(new ChannelContext("", "Core.Local"), request, out response, out Action postAction) && postAction != null)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					postAction();
				}
				catch (Exception exception)
				{
					logger.Error("Error occured on postAction request", exception);
				}
			});
		}
		return response;
	}
}
