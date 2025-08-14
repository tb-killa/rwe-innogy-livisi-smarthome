using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class DeviceManagementServiceClient : CFClientBase<IDeviceManagementService>, IDeviceManagementService, IDisposable
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

	private bool disposed;

	public DeviceManagementServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public DeviceManagementServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private UploadLogFileResponse UploadLogFile(UploadLogFileRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadLogFile";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadLogFileResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<UploadLogFileRequest, UploadLogFileResponse>(cFInvokeInfo, request);
	}

	public UploadFileResponse UploadLogFile(string shcSerial, byte[] content, int currentPackage, int nextPackage, string correlationId)
	{
		UploadLogFileRequest request = new UploadLogFileRequest(shcSerial, content, currentPackage, nextPackage, correlationId);
		UploadLogFileResponse uploadLogFileResponse = UploadLogFile(request);
		return uploadLogFileResponse.UploadLogFileResult;
	}

	private UploadSystemInfoResponse UploadSystemInfo(UploadSystemInfoRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadSystemInfo";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadSystemInfoResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<UploadSystemInfoRequest, UploadSystemInfoResponse>(cFInvokeInfo, request);
	}

	public UploadFileResponse UploadSystemInfo(string shcSerial, string content, SystemInfoType contentType, string description, int currentPackage, int nextPackage, string correlationId)
	{
		UploadSystemInfoRequest request = new UploadSystemInfoRequest(shcSerial, content, contentType, description, currentPackage, nextPackage, correlationId);
		UploadSystemInfoResponse uploadSystemInfoResponse = UploadSystemInfo(request);
		return uploadSystemInfoResponse.UploadSystemInfoResult;
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
		if (CFClientBase<IDeviceManagementService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadLogFile", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadLogFile", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadSystemInfo", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadSystemInfo", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IDeviceManagementService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDeviceManagementService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadLogFileResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadLogFileResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadSystemInfoResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDeviceManagementService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IDeviceManagementService/UploadSystemInfoResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
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

	~DeviceManagementServiceClient()
	{
		Dispose(disposing: false);
	}
}
