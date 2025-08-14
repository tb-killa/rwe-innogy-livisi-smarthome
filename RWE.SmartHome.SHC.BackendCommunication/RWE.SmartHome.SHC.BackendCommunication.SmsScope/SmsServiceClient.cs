using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class SmsServiceClient : CFClientBase<ISmsService>, ISmsService, IDisposable
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://sh70a0100.shmtest.rwe.local/PublicFacingServicesShc/SmsServices/SendSmsService.svc");

	private bool disposed;

	public SmsServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public SmsServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private SendSystemSmsResponse SendSystemSms(SendSystemSmsRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSystemSms";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSystemSmsResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendSystemSmsRequest, SendSystemSmsResponse>(cFInvokeInfo, request);
	}

	public MessageAppResultCode SendSystemSms(string receiverPhoneNo, string shcSerial, SmsTemplates template, string templateParameter)
	{
		SendSystemSmsRequest request = new SendSystemSmsRequest(receiverPhoneNo, shcSerial, template, templateParameter);
		SendSystemSmsResponse sendSystemSmsResponse = SendSystemSms(request);
		return sendSystemSmsResponse.SendSystemSmsResult;
	}

	private SendSmsResponse SendSms(SendSmsRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSms";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSmsResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendSmsRequest, SendSmsResponse>(cFInvokeInfo, request);
	}

	public MessageAppResultCode SendSms(string[] receivers, string message, string shcSerial, out int? remainingQuota)
	{
		SendSmsRequest request = new SendSmsRequest(receivers, message, shcSerial);
		SendSmsResponse sendSmsResponse = SendSms(request);
		remainingQuota = sendSmsResponse.remainingQuota;
		return sendSmsResponse.SendSmsResult;
	}

	private GetSmsRemainingQuotaResponse GetSmsRemainingQuota(GetSmsRemainingQuotaRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/GetSmsRemainingQuota";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/GetSmsRemainingQuotaResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetSmsRemainingQuotaRequest, GetSmsRemainingQuotaResponse>(cFInvokeInfo, request);
	}

	public MessageAppResultCode GetSmsRemainingQuota(string shcSerial, out int? remainingQuota)
	{
		GetSmsRemainingQuotaRequest request = new GetSmsRemainingQuotaRequest(shcSerial);
		GetSmsRemainingQuotaResponse smsRemainingQuota = GetSmsRemainingQuota(request);
		remainingQuota = smsRemainingQuota.remainingQuota;
		return smsRemainingQuota.GetSmsRemainingQuotaResult;
	}

	public static Binding CreateDefaultBinding()
	{
		CustomBinding customBinding = new CustomBinding();
		customBinding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8));
		HttpsTransportBindingElement httpsTransportBindingElement = new HttpsTransportBindingElement();
		httpsTransportBindingElement.RequireClientCertificate = true;
		customBinding.Elements.Add(httpsTransportBindingElement);
		return customBinding;
	}

	private void addProtectionRequirements(Binding binding)
	{
		if (CFClientBase<ISmsService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSystemSms", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSystemSms", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSms", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSms", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/GetSmsRemainingQuota", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/GetSmsRemainingQuota", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<ISmsService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<ISmsService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSystemSmsResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSystemSmsResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSmsResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/SendSmsResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/GetSmsRemainingQuotaResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<ISmsService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/ISmsService/GetSmsRemainingQuotaResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			base.Parameters.Add(channelProtectionRequirements);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			Close();
		}
		disposed = true;
	}

	~SmsServiceClient()
	{
		Dispose(disposing: false);
	}
}
