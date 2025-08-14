using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.SHC.CommonFunctionality;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService;
using SmartHome.Common.Generic.LogManager;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Services;

internal class NotificationService
{
	private static List<Type> ExcludedNotifications = new List<Type> { typeof(LogoutNotification) };

	private readonly ILogger logger = LogManager.Instance.GetLogger<NotificationService>();

	private IEventConverterService eventConverter;

	private IMessageSender sender;

	public NotificationService(IEventConverterService eventConverter, IMessageSender sender)
	{
		this.eventConverter = eventConverter;
		this.sender = sender;
	}

	public void HandleNotification(BaseNotification notification)
	{
		try
		{
			if (ExcludedNotifications.Contains(notification.GetType()))
			{
				return;
			}
			List<Event> list = eventConverter.FromNotification(notification);
			if (list == null)
			{
				return;
			}
			foreach (Event item in list)
			{
				sender.SendMessage(item.ToJson());
			}
		}
		catch (Exception exception)
		{
			logger.Error($"Exception cought while handling notfication {notification.GetType()}", exception);
		}
	}

	public void NotifyFactoryReset()
	{
		Event obj = new Event();
		obj.Type = "ControllerConnectivityChanged";
		obj.Timestamp = DateTime.Now;
		obj.Link = "System";
		obj.SequenceNumber = -1;
		obj.Namespace = "core.RWE";
		obj.Properties = new List<Property>
		{
			new Property
			{
				Name = "IsConnected",
				Value = false
			},
			new Property
			{
				Name = "SerialNumber",
				Value = SHCSerialNumber.SerialNumber()
			}
		};
		Event obj2 = obj;
		sender.SendMessage(obj2.ToJson());
	}
}
