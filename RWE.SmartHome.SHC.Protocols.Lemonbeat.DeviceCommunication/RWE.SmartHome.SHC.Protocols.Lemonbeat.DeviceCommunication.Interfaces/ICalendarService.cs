using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface ICalendarService
{
	IEnumerable<CalendarTask> GetAllCalendarEntries(DeviceIdentifier identifier);

	void SetAndDeleteCalendarEntries(DeviceIdentifier identifier, IEnumerable<CalendarTask> calendarEntriesToSet, IEnumerable<uint> calendarEntriesToDelete);

	void UpdateDeviceTimezone(DeviceIdentifier identifier, int newOffset);

	int? GetDeviceTimezone(DeviceIdentifier identifier);
}
