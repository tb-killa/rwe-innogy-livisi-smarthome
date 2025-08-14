using System;
using RWE.SmartHome.SHC.BackendCommunication.SmsScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

public static class SmsExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.MessageAppResultCode Convert(this RWE.SmartHome.SHC.BackendCommunication.SmsScope.MessageAppResultCode resultCode)
	{
		return resultCode switch
		{
			RWE.SmartHome.SHC.BackendCommunication.SmsScope.MessageAppResultCode.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.MessageAppResultCode.Success, 
			RWE.SmartHome.SHC.BackendCommunication.SmsScope.MessageAppResultCode.Failure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.MessageAppResultCode.Failure, 
			_ => throw new ArgumentOutOfRangeException("resultCode"), 
		};
	}
}
