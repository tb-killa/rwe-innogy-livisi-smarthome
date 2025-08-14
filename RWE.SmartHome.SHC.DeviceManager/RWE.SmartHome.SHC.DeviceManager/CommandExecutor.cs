using System;
using RWE.SmartHome.SHC.DeviceManager.ErrorHandling;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager;

public static class CommandExecutor
{
	public static void ExecuteCommand(int sendRetries, Func<SendStatus> func, string funcName)
	{
		SendStatus sendStatus = SendStatus.ERROR;
		for (int num = sendRetries; num != 0; num--)
		{
			sendStatus = func();
			SendStatus sendStatus2 = sendStatus;
			if (sendStatus2 == SendStatus.ACK || sendStatus2 == SendStatus.NO_REPLY)
			{
				num = 1;
			}
		}
		if (sendStatus != SendStatus.ACK)
		{
			throw ExceptionFactory.GetException(sendStatus, funcName);
		}
	}
}
