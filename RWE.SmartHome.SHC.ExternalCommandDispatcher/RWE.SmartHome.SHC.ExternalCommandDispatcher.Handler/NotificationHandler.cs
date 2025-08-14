using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class NotificationHandler : ICommandHandler, IBaseCommandHandler, INotificationHandler
{
	private const string LoggingSource = "NotificationHandler";

	private readonly IBaseChannel communicationChannel;

	private int sequenceNumber;

	private object sequenceLock = new object();

	public NotificationHandler(IBaseChannel communicationChannel)
	{
		this.communicationChannel = communicationChannel;
		sequenceNumber = 0;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is NotificationRequest)
		{
			return new AcknowledgeResponse();
		}
		return null;
	}

	void INotificationHandler.SendNotification(BaseNotification notification)
	{
		if (notification is GenericNotification { EventType: "UtilityRead" })
		{
			if (communicationChannel is IChannelMultiplexer channelMultiplexer)
			{
				channelMultiplexer.GetChannels().FirstOrDefault((ICommunicationChannel c) => c.ChannelType == ChannelType.Local)?.QueueNotification(notification);
			}
			return;
		}
		lock (sequenceLock)
		{
			sequenceNumber++;
			notification.SequenceNumber = sequenceNumber;
		}
		communicationChannel.QueueNotification(notification);
		Log.Debug(Module.ExternalCommandDispatcher, "NotificationHandler", $"NotificationHandler: sent {notification.GetType().Name} notification.", isPersisted: false);
	}
}
