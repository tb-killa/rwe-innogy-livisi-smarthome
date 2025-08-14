using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.NotificationSender;

public class SentNotificationsCache
{
	private Dictionary<Guid, int> currentMessageCount = new Dictionary<Guid, int>();

	private Dictionary<Guid, DateTime> sendTimeInterval_Start = new Dictionary<Guid, DateTime>();

	public bool IsEntryInCache(Guid key)
	{
		if (sendTimeInterval_Start.ContainsKey(key) && currentMessageCount.ContainsKey(key))
		{
			return true;
		}
		return false;
	}

	public int GetCurrentMessagesCount(Guid actionId)
	{
		if (!currentMessageCount.TryGetValue(actionId, out var value))
		{
			return 0;
		}
		return value;
	}

	public void IncrementCurrentMessagesCount(Guid actionId, int amount)
	{
		if (IsEntryInCache(actionId))
		{
			currentMessageCount[actionId] += amount;
		}
	}

	public void DeleteCachedData(Predicate<Guid> shouldRemove)
	{
		sendTimeInterval_Start.Keys.Where((Guid x) => shouldRemove(x)).ToList().ForEach(delegate(Guid x)
		{
			sendTimeInterval_Start.Remove(x);
		});
		currentMessageCount.Keys.Where((Guid x) => shouldRemove(x)).ToList().ForEach(delegate(Guid x)
		{
			currentMessageCount.Remove(x);
		});
	}

	public void InitializeOrUpdateSentLimitCountersForAction(Guid actionId, string limitInterval)
	{
		if (!sendTimeInterval_Start.TryGetValue(actionId, out var value))
		{
			ResetCountersForAction(actionId);
		}
		else if (IsPeriodPast(limitInterval, value))
		{
			ResetCountersForAction(actionId);
		}
	}

	private void ResetCountersForAction(Guid actionId)
	{
		currentMessageCount[actionId] = 0;
		sendTimeInterval_Start[actionId] = ShcDateTime.UtcNow;
	}

	private bool IsPeriodPast(string periodType, DateTime periodDate)
	{
		if (periodType == null)
		{
			return true;
		}
		DateTime now = ShcDateTime.Now;
		string text;
		string text2;
		switch (periodType)
		{
		case "Month":
			text = periodDate.Month.ToString();
			text2 = now.Month.ToString();
			break;
		case "Week":
			text = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(periodDate).ToString();
			text2 = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(now).ToString();
			break;
		case "Day":
			text = CultureInfo.InvariantCulture.Calendar.GetDayOfYear(periodDate).ToString();
			text2 = CultureInfo.InvariantCulture.Calendar.GetDayOfYear(now).ToString();
			break;
		default:
			return true;
		}
		if (now.Year != periodDate.Year || text != text2)
		{
			return true;
		}
		return false;
	}
}
