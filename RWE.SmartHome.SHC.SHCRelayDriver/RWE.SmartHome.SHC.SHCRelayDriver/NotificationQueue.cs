using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

internal class NotificationQueue : NotificationValidator
{
	private class NotificationListElement
	{
		internal string SerializedNotificationList { get; set; }

		internal string NotificationListId { get; set; }
	}

	private enum QueueState
	{
		Idle,
		Armed
	}

	private const string LoggingSource = "NotificationQueue";

	private const int MaxNotificationsCount = 100;

	private static readonly XmlSerializer notificationSerializer = new XmlSerializer(typeof(NotificationList));

	private readonly List<BaseNotification> sendList = new List<BaseNotification>();

	private NotificationListElement pendingElement = new NotificationListElement();

	private QueueState state;

	private readonly object syncRoot = new object();

	private DateTime armedTime;

	private NotificationSendParameters sendParameters;

	private XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

	internal bool IsIdle
	{
		get
		{
			if (state == QueueState.Idle)
			{
				return sendList.Count == 0;
			}
			return false;
		}
	}

	internal NotificationQueue(NotificationSendParameters param)
		: base(notificationSerializer)
	{
		sendParameters = param;
		state = QueueState.Idle;
		namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
	}

	internal bool ProcessMessages()
	{
		lock (syncRoot)
		{
			switch (state)
			{
			case QueueState.Idle:
				if (sendList.Count > 0 && sendParameters.Driver != null && sendParameters.Driver.Connected)
				{
					NotificationList notificationList = new NotificationList();
					bool flag = true;
					if (sendList.Count > 100)
					{
						notificationList.Notifications = sendList.GetRange(0, 100);
						flag = false;
					}
					else
					{
						notificationList.Notifications = sendList;
					}
					using (StringWriter stringWriter = new StringWriter())
					{
						using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
						{
							Indent = false,
							OmitXmlDeclaration = true
						});
						notificationSerializer.Serialize(xmlWriter, notificationList, namespaces);
						pendingElement.SerializedNotificationList = stringWriter.ToString();
						pendingElement.NotificationListId = notificationList.NotificationListId.ToString();
						xmlWriter.Close();
					}
					if (flag)
					{
						sendList.Clear();
					}
					else
					{
						sendList.RemoveRange(0, 100);
					}
					SendMessage();
					return false;
				}
				return true;
			case QueueState.Armed:
				lock (syncRoot)
				{
					if (DateTime.Now.Subtract(armedTime).TotalMilliseconds > (double)sendParameters.SendDelay)
					{
						state = QueueState.Idle;
					}
				}
				return false;
			default:
				return false;
			}
		}
	}

	private void SendMessage()
	{
		try
		{
			sendParameters.Driver.SendMessageToAllClients(pendingElement.SerializedNotificationList);
			Log.Debug(Module.RelayDriver, "NotificationQueue", $"NotificationList send. Channel: [{sendParameters.Driver.ChannelId}] Content: {ContentSummary(pendingElement.SerializedNotificationList)}");
		}
		catch (Exception ex)
		{
			Log.Warning(Module.RelayDriver, "NotificationQueue", "Failed to send notification " + pendingElement.NotificationListId + ":" + ex.Message);
		}
	}

	private string ContentSummary(string content)
	{
		if (content.Length > 5120)
		{
			return content.Substring(0, 256) + $"[... {content.Length - 256} characters stripped]";
		}
		return content;
	}

	internal void QueueNotification(BaseNotification notification)
	{
		ValidateNotification(notification);
		lock (syncRoot)
		{
			if (state == QueueState.Idle && sendList.Count == 0)
			{
				state = QueueState.Armed;
				armedTime = DateTime.Now;
			}
			sendList.Add(notification);
		}
		Log.Debug(Module.RelayDriver, "NotificationQueue", $"Notification [ID={notification.NotificationId}] queued on Channel: [{sendParameters.Driver.ChannelId}]");
	}
}
