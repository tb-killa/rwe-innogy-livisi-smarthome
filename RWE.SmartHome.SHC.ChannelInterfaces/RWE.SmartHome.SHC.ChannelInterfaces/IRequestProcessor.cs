using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

namespace RWE.SmartHome.SHC.ChannelInterfaces;

public interface IRequestProcessor
{
	bool ProcessRequest(ChannelContext context, BaseRequest request, out BaseResponse response, out Action postSendAction);

	bool ProcessRequest(ChannelContext context, BaseRequest request, out string response, out Action postSendAction);
}
