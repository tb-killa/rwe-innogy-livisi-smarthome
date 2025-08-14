using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface ISmsClient
{
	MessageAppResultCode GetSmsRemainingQuota(string certificateThumbprint, string shcSerial, out int? remainingQuota);
}
