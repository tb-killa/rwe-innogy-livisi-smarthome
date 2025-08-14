using System.Resources;
using RWE.SmartHome.SHC.ErrorHandling;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.ErrorHandling;

internal static class ExceptionFactory
{
	public static ShcException GetException(SendStatus lastStatus, params object[] values)
	{
		ErrorCode errorCode = ErrorCode.Unknown;
		switch (lastStatus)
		{
		case SendStatus.BUSY:
			errorCode = ErrorCode.Busy;
			break;
		case SendStatus.TIMEOUT:
			errorCode = ErrorCode.Timeout;
			break;
		case SendStatus.SERIAL_TIMEOUT:
			errorCode = ErrorCode.SerialTimeout;
			break;
		case SendStatus.MEDIUM_BUSY:
			errorCode = ErrorCode.MediumBusy;
			break;
		case SendStatus.NO_REPLY:
			errorCode = ErrorCode.NoReply;
			break;
		case SendStatus.ERROR:
			errorCode = ErrorCode.Error;
			break;
		case SendStatus.INCOMMING:
			errorCode = ErrorCode.Incoming;
			break;
		case SendStatus.CRC_ERROR:
			errorCode = ErrorCode.CrcError;
			break;
		case SendStatus.MODE_ERROR:
			errorCode = ErrorCode.ModeError;
			break;
		case SendStatus.DUTY_CYCLE:
			errorCode = ErrorCode.DutyCycle;
			break;
		case SendStatus.BIDCOS_INCLUSION_FAILED:
			errorCode = ErrorCode.BidcosInclusionFailed;
			break;
		case SendStatus.BIDCOS_GROUP_ADDRESS_FAILED:
			errorCode = ErrorCode.BidcosGroupAddressFailed;
			break;
		}
		return GetException(errorCode, values);
	}

	public static ShcException GetException(ErrorCode errorCode, params object[] values)
	{
		ResourceManager resourceManager = new ResourceManager(typeof(ErrorStrings));
		string text = resourceManager.GetString(errorCode.ToString());
		string message;
		if (string.IsNullOrEmpty(text))
		{
			text = resourceManager.GetString("CommandHandlerError");
			message = string.Format(text, values[0], errorCode);
		}
		else
		{
			message = string.Format(text, values);
		}
		string[] array = new string[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = values[i].ToString();
		}
		return new ShcException(message, "DeviceController", (int)errorCode, array);
	}
}
