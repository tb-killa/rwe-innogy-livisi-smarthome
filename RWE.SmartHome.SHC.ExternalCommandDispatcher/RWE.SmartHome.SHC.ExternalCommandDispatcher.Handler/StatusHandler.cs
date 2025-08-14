using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class StatusHandler : ICommandHandler, IBaseCommandHandler
{
	private const string LoggingSource = "StatusHandler";

	private readonly IEventManager eventManager;

	public StatusHandler(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is UploadSysInfoRequest)
		{
			return HandleUploadSysInfo();
		}
		return null;
	}

	private BaseResponse HandleUploadSysInfo()
	{
		Log.Information(Module.ExternalCommandDispatcher, "StatusHandler", "Command from Backend: Upload SysInfo.");
		eventManager.GetEvent<UploadSysInfoEvent>().Publish(new UploadSysInfoEventArgs());
		return new AcknowledgeResponse();
	}
}
