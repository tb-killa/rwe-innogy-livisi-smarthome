using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class DeviceUpdateServiceClient : CFClientBase<IDeviceUpdateService>, IDeviceUpdateService, IDisposable
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

	private bool disposed;

	public DeviceUpdateServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public DeviceUpdateServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private CheckForDeviceUpdateResponse CheckForDeviceUpdate(CheckForDeviceUpdateRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2015/02/09/PublicFacingServices/IDeviceUpdateService/CheckForDeviceUpdate";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2015/02/09/PublicFacingServices/IDeviceUpdateService/CheckForDeviceUpdateResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<CheckForDeviceUpdateRequest, CheckForDeviceUpdateResponse>(cFInvokeInfo, request);
	}

	public DeviceUpdateResultCode CheckForDeviceUpdate(DeviceDescriptor deviceDescriptor, out DeviceUpdateInfo updateInfo)
	{
		CheckForDeviceUpdateRequest request = new CheckForDeviceUpdateRequest(deviceDescriptor);
		CheckForDeviceUpdateResponse checkForDeviceUpdateResponse = CheckForDeviceUpdate(request);
		updateInfo = checkForDeviceUpdateResponse.updateInfo;
		return checkForDeviceUpdateResponse.CheckForDeviceUpdateResult;
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
		if (CFClientBase<IDeviceUpdateService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IDeviceUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2015/02/09/PublicFacingServices/IDeviceUpdateService/CheckForDeviceUpdate", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IDeviceUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2015/02/09/PublicFacingServices/IDeviceUpdateService/CheckForDeviceUpdate", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IDeviceUpdateService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDeviceUpdateService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IDeviceUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2015/02/09/PublicFacingServices/IDeviceUpdateService/CheckForDeviceUpdateResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDeviceUpdateService>.ApplyProtection("http://rwe.com/SmartHome/2015/02/09/PublicFacingServices/IDeviceUpdateService/CheckForDeviceUpdateResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
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

	~DeviceUpdateServiceClient()
	{
		Dispose(disposing: false);
	}
}
