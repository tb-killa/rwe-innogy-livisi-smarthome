using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class ApplicationTokenServiceClient : CFClientBase<IApplicationTokenService>, IApplicationTokenService, IDisposable
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

	private bool disposed;

	public ApplicationTokenServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public ApplicationTokenServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private GetApplicationTokenResponse GetApplicationToken(GetApplicationTokenRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationToken";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetApplicationTokenRequest, GetApplicationTokenResponse>(cFInvokeInfo, request);
	}

	public ApplicationsToken GetApplicationToken()
	{
		GetApplicationTokenRequest request = new GetApplicationTokenRequest();
		GetApplicationTokenResponse applicationToken = GetApplicationToken(request);
		return applicationToken.GetApplicationTokenResult;
	}

	private GetApplicationTokenHashResponse GetApplicationTokenHash(GetApplicationTokenHashRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenHash";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenHashResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetApplicationTokenHashRequest, GetApplicationTokenHashResponse>(cFInvokeInfo, request);
	}

	public string GetApplicationTokenHash()
	{
		GetApplicationTokenHashRequest request = new GetApplicationTokenHashRequest();
		GetApplicationTokenHashResponse applicationTokenHash = GetApplicationTokenHash(request);
		return applicationTokenHash.GetApplicationTokenHashResult;
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
		if (CFClientBase<IApplicationTokenService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationToken", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationToken", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenHash", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenHash", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IApplicationTokenService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IApplicationTokenService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenHashResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IApplicationTokenService>.ApplyProtection("http://rwe.com/SmartHome/2011/11/15/ApplicationManagement/IApplicationTokenService/GetApplicationTokenHashResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
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

	~ApplicationTokenServiceClient()
	{
		Dispose(disposing: false);
	}
}
