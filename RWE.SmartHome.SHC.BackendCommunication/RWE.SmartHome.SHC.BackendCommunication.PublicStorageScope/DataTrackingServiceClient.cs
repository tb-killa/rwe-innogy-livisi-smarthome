using System;
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
public class DataTrackingServiceClient : CFClientBase<IDataTrackingService>, IDataTrackingService, IDisposable
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

	private bool disposed;

	public DataTrackingServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public DataTrackingServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private StoreDataResponse StoreData(StoreDataRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreData";
		cFInvokeInfo.ExtraTypes = new Type[1] { typeof(Property[]) };
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreDataResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<StoreDataRequest, StoreDataResponse>(cFInvokeInfo, request);
	}

	public bool StoreData(TrackData deviceTrackingEntity)
	{
		StoreDataRequest request = new StoreDataRequest(deviceTrackingEntity);
		StoreDataResponse storeDataResponse = StoreData(request);
		return storeDataResponse.StoreDataResult;
	}

	private StoreListDataResponse StoreListData(StoreListDataRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreListData";
		cFInvokeInfo.ExtraTypes = new Type[1] { typeof(Property[]) };
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreListDataResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<StoreListDataRequest, StoreListDataResponse>(cFInvokeInfo, request);
	}

	public bool StoreListData(TrackData[] deviceTrackingEntities)
	{
		StoreListDataRequest request = new StoreListDataRequest(deviceTrackingEntities);
		StoreListDataResponse storeListDataResponse = StoreListData(request);
		return storeListDataResponse.StoreListDataResult;
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
		if (CFClientBase<IDataTrackingService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreData", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreData", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreListData", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreListData", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IDataTrackingService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDataTrackingService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreDataResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreDataResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreListDataResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IDataTrackingService>.ApplyProtection("http://rwe.com/SmartHome/2012/04/15/ShcTableStorage/IDataTrackingService/StoreListDataResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
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

	~DataTrackingServiceClient()
	{
		Dispose(disposing: false);
	}
}
