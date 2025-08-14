using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class ShcMessagingServiceClient : CFClientBase<IShcMessagingService>, IDisposable, IShcMessagingService
{
	private bool disposed;

	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

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

	~ShcMessagingServiceClient()
	{
		Dispose(disposing: false);
	}

	public ShcMessagingServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public ShcMessagingServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private SendSmokeDetectionNotificationResponse SendSmokeDetectionNotification(SendSmokeDetectionNotificationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotificationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendSmokeDetectionNotificationRequest, SendSmokeDetectionNotificationResponse>(cFInvokeInfo, request);
	}

	public SendSmokeDetectedNotificationResult SendSmokeDetectionNotification(string shcSerialNo, string roomName, DateTime date, int shcTimeOffset)
	{
		SendSmokeDetectionNotificationRequest request = new SendSmokeDetectionNotificationRequest(shcSerialNo, roomName, date, shcTimeOffset);
		SendSmokeDetectionNotificationResponse sendSmokeDetectionNotificationResponse = SendSmokeDetectionNotification(request);
		return sendSmokeDetectionNotificationResponse.SendSmokeDetectionNotificationResult;
	}

	private SendSmokeDetectionNotification14Response SendSmokeDetectionNotification14(SendSmokeDetectionNotification14Request request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification14";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification14Response";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendSmokeDetectionNotification14Request, SendSmokeDetectionNotification14Response>(cFInvokeInfo, request);
	}

	public SendSmokeDetectedNotificationResult SendSmokeDetectionNotification14(string shcSerialNo, string roomName, DateTime date, string culture, int shcTimeOffset)
	{
		SendSmokeDetectionNotification14Request request = new SendSmokeDetectionNotification14Request(shcSerialNo, roomName, date, culture, shcTimeOffset);
		SendSmokeDetectionNotification14Response sendSmokeDetectionNotification14Response = SendSmokeDetectionNotification14(request);
		return sendSmokeDetectionNotification14Response.SendSmokeDetectionNotification14Result;
	}

	private SendNotificationEmailResponse SendNotificationEmail(SendNotificationEmailRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendNotificationEmail";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendNotificationEmailResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendNotificationEmailRequest, SendNotificationEmailResponse>(cFInvokeInfo, request);
	}

	public SendNotificationEmailResult SendNotificationEmail(string shcSerialNo, EmailTemplates emailTemplate, DateTime localDate, int shcTimeOffset, KeyValuePairOfstringstring[] templateParameters)
	{
		SendNotificationEmailRequest request = new SendNotificationEmailRequest(shcSerialNo, emailTemplate, localDate, shcTimeOffset, templateParameters);
		SendNotificationEmailResponse sendNotificationEmailResponse = SendNotificationEmail(request);
		return sendNotificationEmailResponse.SendNotificationEmailResult;
	}

	private SendEmailResponse SendEmail(SendEmailRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendEmail";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendEmailResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendEmailRequest, SendEmailResponse>(cFInvokeInfo, request);
	}

	public MessageAppResultCode SendEmail(string[] receivers, string message, string shcSerial, out int? remainingQuota)
	{
		SendEmailRequest request = new SendEmailRequest(receivers, message, shcSerial);
		SendEmailResponse sendEmailResponse = SendEmail(request);
		remainingQuota = sendEmailResponse.remainingQuota;
		return sendEmailResponse.SendEmailResult;
	}

	private SendSystemEmailResponse SendSystemEmail(SendSystemEmailRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSystemEmail";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSystemEmailResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendSystemEmailRequest, SendSystemEmailResponse>(cFInvokeInfo, request);
	}

	public MessageAppResultCode SendSystemEmail(string receiverEmail, string shcSerial, EmailTemplates template, string templateParameter)
	{
		SendSystemEmailRequest request = new SendSystemEmailRequest(receiverEmail, shcSerial, template, templateParameter);
		SendSystemEmailResponse sendSystemEmailResponse = SendSystemEmail(request);
		return sendSystemEmailResponse.SendSystemEmailResult;
	}

	private GetEmailRemainingQuotaResponse GetEmailRemainingQuota(GetEmailRemainingQuotaRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/GetEmailRemainingQuota";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/GetEmailRemainingQuotaResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetEmailRemainingQuotaRequest, GetEmailRemainingQuotaResponse>(cFInvokeInfo, request);
	}

	public MessageAppResultCode GetEmailRemainingQuota(string shcSerial, out int? remainingQuota)
	{
		GetEmailRemainingQuotaRequest request = new GetEmailRemainingQuotaRequest(shcSerial);
		GetEmailRemainingQuotaResponse emailRemainingQuota = GetEmailRemainingQuota(request);
		remainingQuota = emailRemainingQuota.remainingQuota;
		return emailRemainingQuota.GetEmailRemainingQuotaResult;
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
		if (CFClientBase<IShcMessagingService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification14", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification14", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendNotificationEmail", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendNotificationEmail", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendEmail", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendEmail", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSystemEmail", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSystemEmail", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/GetEmailRemainingQuota", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/GetEmailRemainingQuota", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IShcMessagingService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotificationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotificationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification14Response", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSmokeDetectionNotification14Response", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendNotificationEmailResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendNotificationEmailResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendEmailResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendEmailResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSystemEmailResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/SendSystemEmailResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/GetEmailRemainingQuotaResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcMessagingService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcMessagingService/GetEmailRemainingQuotaResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			base.Parameters.Add(channelProtectionRequirements);
		}
	}
}
