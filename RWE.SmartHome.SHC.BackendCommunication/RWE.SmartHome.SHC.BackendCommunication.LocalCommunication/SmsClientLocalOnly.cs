using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class SmsClientLocalOnly : ISmsClient
{
	public MessageAppResultCode GetSmsRemainingQuota(string certificateThumbprint, string shcSerial, out int? remainingQuota)
	{
		remainingQuota = 0;
		return MessageAppResultCode.Success;
	}
}
