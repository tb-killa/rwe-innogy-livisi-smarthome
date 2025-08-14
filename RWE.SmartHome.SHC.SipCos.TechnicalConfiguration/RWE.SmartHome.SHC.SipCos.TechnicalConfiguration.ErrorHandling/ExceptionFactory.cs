using System.Resources;
using RWE.SmartHome.SHC.ErrorHandling;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;

internal static class ExceptionFactory
{
	public static ShcException GetException(ErrorCode errorCode, params object[] values)
	{
		ResourceManager resourceManager = new ResourceManager(typeof(ErrorStrings));
		string text = errorCode.ToString();
		string text2 = resourceManager.GetString(text);
		string message = (string.IsNullOrEmpty(text2) ? text : string.Format(text2, values));
		string[] array = new string[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = values[i].ToString();
		}
		return new ShcException(message, "TechnicalConfiguration", (int)errorCode, array);
	}
}
