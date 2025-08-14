using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public static class PropertyExtensions
{
	public static void SetValueWithTimestampUpdate(this BooleanProperty prop, bool? value, DateTime timestamp)
	{
		if (Nullable.Compare(prop.Value, value) != 0)
		{
			prop.Value = value;
			prop.UpdateTimestamp = timestamp.ToUniversalTime();
		}
	}

	public static void SetValueWithTimestampUpdateIfNotNull(this BooleanProperty prop, BooleanProperty value, DateTime timestamp)
	{
		if (value != null && value.Value.HasValue)
		{
			prop.SetValueWithTimestampUpdate(value.Value, timestamp);
		}
	}

	public static void CopyValueAndTimestamp(this BooleanProperty prop, BooleanProperty source)
	{
		source = source ?? new BooleanProperty();
		prop.Value = source.Value;
		prop.UpdateTimestamp = DateTimePropertyAsUtc(source.UpdateTimestamp);
	}

	public static void SetValueWithTimestampUpdate(this DateTimeProperty prop, DateTime? value, DateTime timestamp)
	{
		if (Nullable.Compare(prop.Value, value) != 0)
		{
			prop.Value = value;
			prop.UpdateTimestamp = timestamp.ToUniversalTime();
		}
	}

	public static void SetValueWithTimestampUpdateIfNotNull(this DateTimeProperty prop, DateTimeProperty value, DateTime timestamp)
	{
		if (value != null && value.Value.HasValue)
		{
			prop.SetValueWithTimestampUpdate(value.Value, timestamp);
		}
	}

	public static void CopyValueAndTimestamp(this DateTimeProperty prop, DateTimeProperty source)
	{
		source = source ?? new DateTimeProperty();
		prop.Value = source.Value;
		prop.UpdateTimestamp = DateTimePropertyAsUtc(source.UpdateTimestamp);
	}

	public static void SetValueWithTimestampUpdate(this NumericProperty prop, decimal? value, DateTime timestamp)
	{
		if (Nullable.Compare(prop.Value, value) != 0)
		{
			prop.Value = value;
			prop.UpdateTimestamp = timestamp.ToUniversalTime();
		}
	}

	public static void SetValueWithTimestampUpdateIfNotNull(this NumericProperty prop, NumericProperty value, DateTime timestamp)
	{
		if (value != null && value.Value.HasValue)
		{
			prop.SetValueWithTimestampUpdate(value.Value, timestamp);
		}
	}

	public static void CopyValueAndTimestamp(this NumericProperty prop, NumericProperty source)
	{
		source = source ?? new NumericProperty();
		prop.Value = source.Value;
		prop.UpdateTimestamp = DateTimePropertyAsUtc(source.UpdateTimestamp);
	}

	public static void SetValueWithTimestampUpdate(this StringProperty prop, string value, DateTime timestamp)
	{
		if (!string.Equals(prop.Value, value))
		{
			prop.Value = value;
			prop.UpdateTimestamp = timestamp.ToUniversalTime();
		}
	}

	public static void SetValueWithTimestampUpdateIfNotNull(this StringProperty prop, StringProperty value, DateTime timestamp)
	{
		if (value != null && value.Value != null)
		{
			prop.SetValueWithTimestampUpdate(value.Value, timestamp);
		}
	}

	public static void CopyValueAndTimestamp(this StringProperty prop, StringProperty source)
	{
		source = source ?? new StringProperty();
		prop.Value = source.Value;
		prop.UpdateTimestamp = DateTimePropertyAsUtc(source.UpdateTimestamp);
	}

	private static DateTime DateTimePropertyAsUtc(DateTime? dateTime)
	{
		if (dateTime.HasValue && dateTime.HasValue)
		{
			return dateTime.Value.ToUniversalTime();
		}
		return default(DateTime);
	}
}
