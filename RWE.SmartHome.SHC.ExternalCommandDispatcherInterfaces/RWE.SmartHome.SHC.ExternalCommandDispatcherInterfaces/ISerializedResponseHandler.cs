using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.SHC.ChannelInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

public interface ISerializedResponseHandler : IBaseCommandHandler
{
	SerializationResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction);
}
