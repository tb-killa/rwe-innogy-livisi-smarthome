using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class SoftwareUpdateServiceClient : CFClientBase<ISoftwareUpdateService>, IDisposable, ISoftwareUpdateService
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

	~SoftwareUpdateServiceClient()
	{
		Dispose(disposing: false);
	}

	public SoftwareUpdateServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public SoftwareUpdateServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private CheckForSoftwareUpdateResponse CheckForSoftwareUpdate(CheckForSoftwareUpdateRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/CheckForSoftwareUpdate";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/CheckForSoftwareUpdateResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<CheckForSoftwareUpdateRequest, CheckForSoftwareUpdateResponse>(cFInvokeInfo, request);
	}

	public SwUpdateResultCode CheckForSoftwareUpdate(string shcSerial, ShcVersionInfo shcVersionInfo, out UpdateInfo updateInfo)
	{
		CheckForSoftwareUpdateRequest request = new CheckForSoftwareUpdateRequest(shcSerial, shcVersionInfo);
		CheckForSoftwareUpdateResponse checkForSoftwareUpdateResponse = CheckForSoftwareUpdate(request);
		updateInfo = checkForSoftwareUpdateResponse.updateInfo;
		return checkForSoftwareUpdateResponse.CheckForSoftwareUpdateResult;
	}

	private ShcSoftwareUpdatedResponse ShcSoftwareUpdated(ShcSoftwareUpdatedRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/ShcSoftwareUpdated";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/ShcSoftwareUpdatedResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<ShcSoftwareUpdatedRequest, ShcSoftwareUpdatedResponse>(cFInvokeInfo, request);
	}

	public ShcUpdateAnnouncementResultCode ShcSoftwareUpdated(ShcVersionInfo newShcVersion)
	{
		ShcSoftwareUpdatedRequest request = new ShcSoftwareUpdatedRequest(newShcVersion);
		ShcSoftwareUpdatedResponse shcSoftwareUpdatedResponse = ShcSoftwareUpdated(request);
		return shcSoftwareUpdatedResponse.ShcSoftwareUpdatedResult;
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
		if (CFClientBase<ISoftwareUpdateService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/CheckForSoftwareUpdate", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/CheckForSoftwareUpdate", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/ShcSoftwareUpdated", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/ShcSoftwareUpdated", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<ISoftwareUpdateService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<ISoftwareUpdateService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/CheckForSoftwareUpdateResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/CheckForSoftwareUpdateResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/ShcSoftwareUpdatedResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<ISoftwareUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2010/09/08/PublicFacingServices/ISoftwareUpdateService/ShcSoftwareUpdatedResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			base.Parameters.Add(channelProtectionRequirements);
		}
	}
}
