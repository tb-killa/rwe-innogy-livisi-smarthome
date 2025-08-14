using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Calibration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

public class NotificationList
{
	[XmlArrayItem(typeof(ConfigurationSaveProgressNotification))]
	[XmlArrayItem(typeof(MemberStateChangedNotification))]
	[XmlArrayItem(typeof(DeviceDiscoveryStatusChangedNotification))]
	[XmlArrayItem(typeof(CalibrationNotification))]
	[XmlArrayItem(typeof(InvalidateTokenCacheNotification))]
	[XmlArrayItem(typeof(EntityDeploymentNotification))]
	[XmlArrayItem(typeof(CommitConfigurationErrorNotification))]
	[XmlArrayItem(typeof(SaveConfigurationErrorNotification))]
	[XmlArrayItem(typeof(HomeStateChangedNotification))]
	[XmlArrayItem(typeof(LogicalDeviceStatesChangedNotification))]
	[XmlArrayItem(typeof(NewMessageNotification))]
	[XmlArrayItem(typeof(MessageStateChangedNotification))]
	[XmlArrayItem(typeof(PhysicalDeviceFoundNotification))]
	[XmlArrayItem(typeof(MessageDeletionNotification))]
	[XmlArrayItem(typeof(LogoutNotification))]
	[XmlArrayItem(typeof(PhysicalDeviceStateChangedNotification))]
	[XmlArrayItem(typeof(SaveConfigurationNotification))]
	[XmlArrayItem(typeof(GenericNotification))]
	[XmlArrayItem(typeof(DeviceDiscoveryFailureNotification))]
	public List<BaseNotification> Notifications { get; set; }

	[XmlAttribute]
	public Guid NotificationListId { get; set; }

	public NotificationList()
	{
		NotificationListId = Guid.NewGuid();
	}
}
