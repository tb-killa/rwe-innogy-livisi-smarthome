using System;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.ErrorHandling;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal static class ExceptionFactory
{
	public static Exception GetException(ErrorCode errorCode, Guid deviceId)
	{
		string message = $"Device {deviceId} reported error {errorCode}";
		return new ShcException(message, Module.wMBusProtocolAdapter.ToString(), (int)errorCode);
	}
}
