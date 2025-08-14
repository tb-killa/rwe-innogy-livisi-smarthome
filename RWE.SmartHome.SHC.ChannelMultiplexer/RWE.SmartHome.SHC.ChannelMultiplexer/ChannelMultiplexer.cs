using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;

namespace RWE.SmartHome.SHC.ChannelMultiplexer;

public class ChannelMultiplexer : IChannelMultiplexer, IBaseChannel, IRequestProcessor
{
	private IRequestProcessor requestProcessor;

	private readonly List<ICommunicationChannel> channels = new List<ICommunicationChannel>();

	private readonly object requestSerializationMutex = new object();

	public void AddCommunicationChannel(ICommunicationChannel communicationChannel)
	{
		communicationChannel.SubscribeRequestProcessor(this);
		channels.Add(communicationChannel);
	}

	public IEnumerable<ICommunicationChannel> GetChannels()
	{
		return channels;
	}

	public void SubscribeRequestProcessor(IRequestProcessor processor)
	{
		requestProcessor = processor;
	}

	public void QueueNotification(BaseNotification notification)
	{
		foreach (ICommunicationChannel channel in channels)
		{
			channel.QueueNotification(notification);
		}
	}

	public bool ProcessRequest(ChannelContext context, BaseRequest request, out BaseResponse response, out Action postSendAction)
	{
		lock (requestSerializationMutex)
		{
			return requestProcessor.ProcessRequest(context, request, out response, out postSendAction);
		}
	}

	public bool ProcessRequest(ChannelContext context, BaseRequest request, out string response, out Action postSendAction)
	{
		lock (requestSerializationMutex)
		{
			return requestProcessor.ProcessRequest(context, request, out response, out postSendAction);
		}
	}
}
