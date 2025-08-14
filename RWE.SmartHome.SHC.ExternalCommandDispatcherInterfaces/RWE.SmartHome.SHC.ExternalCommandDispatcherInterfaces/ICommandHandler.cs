using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ChannelInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

public interface ICommandHandler : IBaseCommandHandler
{
	BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction);
}
