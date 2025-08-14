using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class KeyExchangeServiceClient : CFClientBase<IKeyExchangeService>, IKeyExchangeService, IDisposable
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

	private bool disposed;

	public KeyExchangeServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public KeyExchangeServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private EncryptNetworkKeyResponse EncryptNetworkKey(EncryptNetworkKeyRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/EncryptNetworkKey";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/EncryptNetworkKeyResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<EncryptNetworkKeyRequest, EncryptNetworkKeyResponse>(cFInvokeInfo, request);
	}

	public KeyExchangeResult EncryptNetworkKey(byte[] sgtin, byte[] secNumber, byte[] encOnceNetworkKey, string deviceFirmwareVersion, out byte[] encTwiceNetworkKey, out byte[] keyAuthentication)
	{
		EncryptNetworkKeyRequest request = new EncryptNetworkKeyRequest(sgtin, secNumber, encOnceNetworkKey, deviceFirmwareVersion);
		EncryptNetworkKeyResponse encryptNetworkKeyResponse = EncryptNetworkKey(request);
		encTwiceNetworkKey = encryptNetworkKeyResponse.encTwiceNetworkKey;
		keyAuthentication = encryptNetworkKeyResponse.keyAuthentication;
		return encryptNetworkKeyResponse.EncryptNetworkKeyResult;
	}

	private GetDeviceKeyResponse GetDeviceKey(GetDeviceKeyRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDeviceKey";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDeviceKeyResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetDeviceKeyRequest, GetDeviceKeyResponse>(cFInvokeInfo, request);
	}

	public KeyExchangeResult GetDeviceKey(byte[] sgtin, out byte[] deviceKey)
	{
		GetDeviceKeyRequest request = new GetDeviceKeyRequest(sgtin);
		GetDeviceKeyResponse deviceKey2 = GetDeviceKey(request);
		deviceKey = deviceKey2.deviceKey;
		return deviceKey2.GetDeviceKeyResult;
	}

	private GetMasterKeyResponse GetMasterKey(GetMasterKeyRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetMasterKey";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetMasterKeyResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetMasterKeyRequest, GetMasterKeyResponse>(cFInvokeInfo, request);
	}

	public KeyExchangeResult GetMasterKey(out string masterKey)
	{
		GetMasterKeyRequest request = new GetMasterKeyRequest();
		GetMasterKeyResponse masterKey2 = GetMasterKey(request);
		masterKey = masterKey2.masterKey;
		return masterKey2.GetMasterKeyResult;
	}

	private GetDevicesKeysResponse GetDevicesKeys(GetDevicesKeysRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDevicesKeys";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDevicesKeysResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetDevicesKeysRequest, GetDevicesKeysResponse>(cFInvokeInfo, request);
	}

	public KeyExchangeResult GetDevicesKeys(byte[][] sgtins, out ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] devicesKeys)
	{
		GetDevicesKeysRequest request = new GetDevicesKeysRequest(sgtins);
		GetDevicesKeysResponse devicesKeys2 = GetDevicesKeys(request);
		devicesKeys = devicesKeys2.devicesKeys;
		return devicesKeys2.GetDevicesKeysResult;
	}

	public static Binding CreateDefaultBinding()
	{
		CustomBinding customBinding = new CustomBinding();
		customBinding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8));
		HttpsTransportBindingElement httpsTransportBindingElement = new HttpsTransportBindingElement();
		httpsTransportBindingElement.RequireClientCertificate = false;
		customBinding.Elements.Add(httpsTransportBindingElement);
		return customBinding;
	}

	private void addProtectionRequirements(Binding binding)
	{
		if (CFClientBase<IKeyExchangeService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/EncryptNetworkKey", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/EncryptNetworkKey", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDeviceKey", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDeviceKey", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetMasterKey", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetMasterKey", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDevicesKeys", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDevicesKeys", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IKeyExchangeService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/EncryptNetworkKeyResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/EncryptNetworkKeyResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDeviceKeyResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDeviceKeyResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetMasterKeyResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetMasterKeyResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDevicesKeysResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IKeyExchangeService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IKeyExchangeService/GetDevicesKeysResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
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

	~KeyExchangeServiceClient()
	{
		Dispose(disposing: false);
	}
}
