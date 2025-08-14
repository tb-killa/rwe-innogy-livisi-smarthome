using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface ISmsService
{
	MessageAppResultCode SendSystemSms(string receiverPhoneNo, string shcSerial, SmsTemplates template, string templateParameter);

	MessageAppResultCode SendSms(string[] receivers, string message, string shcSerial, out int? remainingQuota);

	MessageAppResultCode GetSmsRemainingQuota(string shcSerial, out int? remainingQuota);
}
