using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class DalStorageServiceClient : CFClientBase<IDalStorageService>, IDalStorageService
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

	public DalStorageServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public DalStorageServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private StoreDeviceActivityLogResponse StoreDeviceActivityLog(StoreDeviceActivityLogRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/StoreDeviceActivityLog";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/StoreDeviceActivityLogResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<StoreDeviceActivityLogRequest, StoreDeviceActivityLogResponse>(cFInvokeInfo, request);
	}

	public ShcTableStorageStoreResult StoreDeviceActivityLog(DeviceActivityLog[] dalEntries)
	{
		StoreDeviceActivityLogRequest request = new StoreDeviceActivityLogRequest(dalEntries);
		StoreDeviceActivityLogResponse storeDeviceActivityLogResponse = StoreDeviceActivityLog(request);
		return storeDeviceActivityLogResponse.StoreDeviceActivityLogResult;
	}

	private PurgeDeviceActivityLogResponse PurgeDeviceActivityLog(PurgeDeviceActivityLogRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/PurgeDeviceActivityLog";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/PurgeDeviceActivityLogResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<PurgeDeviceActivityLogRequest, PurgeDeviceActivityLogResponse>(cFInvokeInfo, request);
	}

	public void PurgeDeviceActivityLog()
	{
		PurgeDeviceActivityLogRequest request = new PurgeDeviceActivityLogRequest();
		PurgeDeviceActivityLog(request);
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
		if (CFClientBase<IDalStorageService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/StoreDeviceActivityLog", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/StoreDeviceActivityLog", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/PurgeDeviceActivityLog", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/PurgeDeviceActivityLog", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IDalStorageService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDalStorageService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/StoreDeviceActivityLogResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/StoreDeviceActivityLogResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/PurgeDeviceActivityLogResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDalStorageService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDalStorageService/PurgeDeviceActivityLogResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			base.Parameters.Add(channelProtectionRequirements);
		}
	}
}
