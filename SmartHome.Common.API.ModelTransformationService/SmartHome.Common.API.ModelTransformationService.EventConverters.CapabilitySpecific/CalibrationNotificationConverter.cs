using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Calibration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters.CapabilitySpecific;

public class CalibrationNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(CalibrationNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		if (!(notification is CalibrationNotification { LogicalDeviceId: var logicalDeviceId } calibrationNotification))
		{
			return new List<Event>();
		}
		string arg = logicalDeviceId.ToString("N");
		Event obj = new Event();
		obj.Properties = new List<Property>
		{
			new Property
			{
				Name = "CalibrationStep",
				Value = calibrationNotification.CalibrationStep.ToString()
			}
		};
		Event obj2 = obj;
		if (calibrationNotification.Value.HasValue)
		{
			obj2.Properties.Add(new Property
			{
				Name = "Value",
				Value = calibrationNotification.Value
			});
		}
		obj2.Timestamp = notification.Timestamp;
		obj2.Type = "RollerShutterCalibrated";
		obj2.Link = $"/capability/{arg}";
		obj2.SequenceNumber = notification.SequenceNumber;
		obj2.Namespace = notification.Namespace;
		logger.Debug($"Event received: {obj2} ");
		List<Event> list = new List<Event>(1);
		list.Add(obj2);
		return list;
	}
}
