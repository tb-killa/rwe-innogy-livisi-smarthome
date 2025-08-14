using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class NotificationServiceClient : CFClientBase<INotificationService>, IDisposable, INotificationService
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
			try
			{
				Close();
			}
			catch (Exception)
			{
			}
		}
		disposed = true;
	}

	~NotificationServiceClient()
	{
		Dispose(disposing: false);
	}

	public NotificationServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public NotificationServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private SendNotificationsResponse SendNotifications(SendNotificationsRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendNotifications";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendNotificationsResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendNotificationsRequest, SendNotificationsResponse>(cFInvokeInfo, request);
	}

	public NotificationResponse SendNotifications(CustomNotification notification)
	{
		SendNotificationsRequest request = new SendNotificationsRequest(notification);
		SendNotificationsResponse sendNotificationsResponse = SendNotifications(request);
		return sendNotificationsResponse.SendNotificationsResult;
	}

	private SendSystemNotificationsResponse SendSystemNotifications(SendSystemNotificationsRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendSystemNotifications";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendSystemNotificationsResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SendSystemNotificationsRequest, SendSystemNotificationsResponse>(cFInvokeInfo, request);
	}

	public NotificationResponse SendSystemNotifications(SystemNotification notification)
	{
		SendSystemNotificationsRequest request = new SendSystemNotificationsRequest(notification);
		SendSystemNotificationsResponse sendSystemNotificationsResponse = SendSystemNotifications(request);
		return sendSystemNotificationsResponse.SendSystemNotificationsResult;
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
		if (CFClientBase<INotificationService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendNotifications", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendNotifications", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendSystemNotifications", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendSystemNotifications", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<INotificationService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<INotificationService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendNotificationsResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendNotificationsResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendSystemNotificationsResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<INotificationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/INotificationService/SendSystemNotificationsResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			base.Parameters.Add(channelProtectionRequirements);
		}
	}
}
