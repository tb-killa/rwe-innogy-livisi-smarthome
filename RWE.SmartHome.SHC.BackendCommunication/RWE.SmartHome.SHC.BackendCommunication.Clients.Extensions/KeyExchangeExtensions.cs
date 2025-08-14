using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using SmartHome.SHC.BackendCommunication.KeyExchangeScope;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class KeyExchangeExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult Convert(this global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult result)
	{
		return result switch
		{
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.Success, 
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult.DeviceNotFound => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.DeviceNotFound, 
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult.UnexpectedException => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.UnexpectedException, 
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult.InvalidTenant => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.InvalidTenant, 
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult.DeviceBlacklisted => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.DeviceBlacklisted, 
			_ => throw new ArgumentOutOfRangeException("result"), 
		};
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] Convert(this global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] deviceSgtinAndKey)
	{
		List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary> list = new List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary>();
		foreach (global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary arrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary in deviceSgtinAndKey)
		{
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary arrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary2 = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary();
			arrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary2.Key = arrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary.Value;
			arrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary2.SGTIN = arrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary.Key;
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary item = arrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary2;
			list.Add(item);
		}
		return list.ToArray();
	}
}
