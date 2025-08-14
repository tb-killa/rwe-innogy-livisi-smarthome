using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calendar;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class CalendarService : SenderService<network>, ICalendarService
{
	private const string DefaultNamespace = "urn:calendarxsd";

	public static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public CalendarService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Calendar, "urn:calendarxsd")
	{
	}

	public IEnumerable<CalendarTask> GetAllCalendarEntries(DeviceIdentifier identifier)
	{
		network request = CreateGetRequest(identifier, new calendarGetType());
		network network = SendRequest(identifier, request);
		if (network != null && network.device != null)
		{
			networkDevice networkDevice = network.device.Where((networkDevice d) => d.device_id == identifier.SubDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Items != null)
			{
				calendarReportType calendarReportType = networkDevice.Items.OfType<calendarReportType>().FirstOrDefault();
				if (calendarReportType != null && calendarReportType.task != null)
				{
					return calendarReportType.task.Select((taskType calendar) => CreateCalendarEntry(calendar));
				}
			}
		}
		return new List<CalendarTask>();
	}

	public void SetAndDeleteCalendarEntries(DeviceIdentifier identifier, IEnumerable<CalendarTask> calendarEntriesToSet, IEnumerable<uint> calendarEntriesToDelete)
	{
		taskType[] array = calendarEntriesToSet.Select((CalendarTask calendarEntry) => CreateCalendarTask(calendarEntry)).ToArray();
		if (array.Length > 0)
		{
			network message = CreateSetRequest(identifier, new calendarSetType
			{
				task = array
			});
			SendMessage(identifier, message, TransportType.Connection);
		}
		foreach (uint item in calendarEntriesToDelete)
		{
			network message2 = CreateDeleteRequest(identifier, new calendarDeleteType
			{
				task_id = (byte)item,
				task_idSpecified = true
			});
			SendMessage(identifier, message2, TransportType.Connection);
		}
	}

	public void SetCalendarEntry(DeviceIdentifier identifier, CalendarTask calendarEntry)
	{
		network message = CreateSetRequest(identifier, new calendarSetType
		{
			task = new taskType[1] { CreateCalendarTask(calendarEntry) }
		});
		SendMessage(identifier, message, TransportType.Connection);
	}

	public void SetCalendarEntries(DeviceIdentifier identifier, IEnumerable<CalendarTask> calendarEntries)
	{
		network message = CreateSetRequest(identifier, new calendarSetType
		{
			task = calendarEntries.Select((CalendarTask calendarEntry) => CreateCalendarTask(calendarEntry)).ToArray()
		});
		SendMessage(identifier, message, TransportType.Connection);
	}

	public void DeleteCalendarEntry(DeviceIdentifier identifier, uint index)
	{
		network message = CreateDeleteRequest(identifier, new calendarDeleteType
		{
			task_id = (byte)index,
			task_idSpecified = true
		});
		SendMessage(identifier, message, TransportType.Connection);
	}

	public void DeleteAllCalendarEntries(DeviceIdentifier identifier)
	{
		network message = CreateDeleteRequest(identifier, new calendarDeleteType());
		SendMessage(identifier, message, TransportType.Connection);
	}

	public void UpdateDeviceTimezone(DeviceIdentifier identifier, int newOffset)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.calendar_set_timezone };
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		network.device[0].Items = new object[1]
		{
			new calendarTimezoneSetType
			{
				offset = newOffset
			}
		};
		SendMessage(identifier, network, TransportType.Connection);
	}

	public int? GetDeviceTimezone(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.calendar_get_timezone };
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		network.device[0].Items = new object[1]
		{
			new calendarTimezoneGetType()
		};
		network network2 = SendRequest(identifier, network);
		if (network2 != null && network2.device[0] != null && network2.device[0].Items[0] is calendarTimezoneReportType)
		{
			return (network2.device[0].Items[0] as calendarTimezoneReportType).offset;
		}
		return null;
	}

	private network CreateNetworkMessage(DeviceIdentifier identifier)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				device_idSpecified = identifier.SubDeviceId.HasValue,
				device_id = (identifier.SubDeviceId ?? 0)
			}
		};
		return network;
	}

	private network CreateGetRequest(DeviceIdentifier identifier, calendarGetType get)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.calendar_get };
		network.device[0].Items = new object[1] { get };
		return network;
	}

	private network CreateSetRequest(DeviceIdentifier identifier, calendarSetType set)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.calendar_set };
		network.device[0].Items = new object[1] { set };
		return network;
	}

	private network CreateDeleteRequest(DeviceIdentifier identifier, calendarDeleteType delete)
	{
		network network = CreateNetworkMessage(identifier);
		networkDevice obj = network.device[0];
		ItemsChoiceType[] itemsElementName = new ItemsChoiceType[1];
		obj.ItemsElementName = itemsElementName;
		network.device[0].Items = new object[1] { delete };
		return network;
	}

	private static taskType CreateCalendarTask(CalendarTask calendarEntry)
	{
		taskType taskType = new taskType();
		if (calendarEntry.End.HasValue)
		{
			taskType.endSpecified = true;
			taskType.end = (uint)calendarEntry.End.Value.Subtract(EpochTime).TotalSeconds;
		}
		taskType.start = (uint)calendarEntry.Start.Subtract(EpochTime).TotalSeconds;
		taskType.action_id = (byte)calendarEntry.ActionId;
		taskType.action_idSpecified = true;
		taskType.repeat = (byte)(calendarEntry.Repeat ?? ((Repeat)0));
		taskType.repeatSpecified = calendarEntry.Repeat.HasValue;
		if (calendarEntry.WeekDays != null)
		{
			taskType.weekdays = ToLemonbeatWeekdays(calendarEntry.WeekDays);
			taskType.weekdaysSpecified = true;
		}
		else
		{
			taskType.weekdaysSpecified = false;
			taskType.weekdays = 0;
		}
		taskType.task_id = calendarEntry.Id;
		return taskType;
	}

	private CalendarTask CreateCalendarEntry(taskType taskType)
	{
		CalendarTask calendarTask = new CalendarTask();
		calendarTask.ActionId = taskType.action_id;
		CalendarTask calendarTask2 = calendarTask;
		if (taskType.endSpecified)
		{
			calendarTask2.End = EpochTime.AddSeconds(taskType.end);
		}
		calendarTask2.Start = EpochTime.AddSeconds(taskType.start);
		calendarTask2.Id = taskType.task_id;
		calendarTask2.Repeat = (taskType.repeatSpecified ? new Repeat?((Repeat)taskType.repeat) : ((Repeat?)null));
		calendarTask2.WeekDays = (taskType.weekdaysSpecified ? FromLemonbeatWeekdays(taskType.weekdays) : null);
		return calendarTask2;
	}

	internal static byte ToLemonbeatWeekdays(List<DayOfWeek> WeekDays)
	{
		byte LemonbeatWeekdayList = 0;
		WeekDays.ForEach(delegate(DayOfWeek day)
		{
			switch (day)
			{
			case DayOfWeek.Monday:
				LemonbeatWeekdayList |= 1;
				break;
			case DayOfWeek.Tuesday:
				LemonbeatWeekdayList |= 2;
				break;
			case DayOfWeek.Wednesday:
				LemonbeatWeekdayList |= 4;
				break;
			case DayOfWeek.Thursday:
				LemonbeatWeekdayList |= 8;
				break;
			case DayOfWeek.Friday:
				LemonbeatWeekdayList |= 16;
				break;
			case DayOfWeek.Saturday:
				LemonbeatWeekdayList |= 32;
				break;
			case DayOfWeek.Sunday:
				LemonbeatWeekdayList |= 64;
				break;
			}
		});
		return LemonbeatWeekdayList;
	}

	internal static List<DayOfWeek> FromLemonbeatWeekdays(byte LemonbeatWeekdayList)
	{
		List<DayOfWeek> list = new List<DayOfWeek>();
		if (LemonbeatWeekdayList > 0)
		{
			if ((LemonbeatWeekdayList & 1) != 0)
			{
				list.Add(DayOfWeek.Monday);
			}
			if ((LemonbeatWeekdayList & 2) != 0)
			{
				list.Add(DayOfWeek.Tuesday);
			}
			if ((LemonbeatWeekdayList & 4) != 0)
			{
				list.Add(DayOfWeek.Wednesday);
			}
			if ((LemonbeatWeekdayList & 8) != 0)
			{
				list.Add(DayOfWeek.Thursday);
			}
			if ((LemonbeatWeekdayList & 0x10) != 0)
			{
				list.Add(DayOfWeek.Friday);
			}
			if ((LemonbeatWeekdayList & 0x20) != 0)
			{
				list.Add(DayOfWeek.Saturday);
			}
			if ((LemonbeatWeekdayList & 0x40) != 0)
			{
				list.Add(DayOfWeek.Sunday);
			}
		}
		return list;
	}
}
