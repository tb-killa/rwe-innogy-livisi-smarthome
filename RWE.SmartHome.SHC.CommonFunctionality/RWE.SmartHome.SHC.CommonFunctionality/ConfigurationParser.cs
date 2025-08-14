namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class ConfigurationParser
{
	public static string TryGetStringValueFromPersistence(string persistenceValue, string defaultValue)
	{
		if (string.IsNullOrEmpty(persistenceValue))
		{
			return defaultValue;
		}
		return persistenceValue;
	}

	public static int TryGetIntValueFromPersistence(int? persistenceValue, int defaultValue)
	{
		if (persistenceValue.HasValue)
		{
			return persistenceValue.Value;
		}
		return defaultValue;
	}
}
