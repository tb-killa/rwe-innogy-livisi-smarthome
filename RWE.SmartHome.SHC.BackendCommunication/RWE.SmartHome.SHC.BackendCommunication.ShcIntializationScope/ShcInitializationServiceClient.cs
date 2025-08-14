using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class ShcInitializationServiceClient : CFClientBase<IShcInitializationService>, IShcInitializationService, IDisposable
{
	public static EndpointAddress EndpointAddress = new EndpointAddress("https://localhost/Service");

	private bool disposed;

	public ShcInitializationServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public ShcInitializationServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private SubmitCertificateRequestResponse SubmitCertificateRequest(SubmitCertificateRequestRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitCertificateRequest";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitCertificateRequestResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SubmitCertificateRequestRequest, SubmitCertificateRequestResponse>(cFInvokeInfo, request);
	}

	public InitializationErrorCode SubmitCertificateRequest(string shcSerial, string pin, string certificateRequest, out string sessionToken)
	{
		SubmitCertificateRequestRequest request = new SubmitCertificateRequestRequest(shcSerial, pin, certificateRequest);
		SubmitCertificateRequestResponse submitCertificateRequestResponse = SubmitCertificateRequest(request);
		sessionToken = submitCertificateRequestResponse.sessionToken;
		return submitCertificateRequestResponse.SubmitCertificateRequestResult;
	}

	private RetrieveInitializationDataResponse RetrieveInitializationData(RetrieveInitializationDataRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveInitializationData";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveInitializationDataResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<RetrieveInitializationDataRequest, RetrieveInitializationDataResponse>(cFInvokeInfo, request);
	}

	public InitializationErrorCode RetrieveInitializationData(string sessionToken, out string issuedCertificate, out ShcSyncRecord shcSyncRecord, out bool furtherPollingRequired, out int pollAfterSeconds)
	{
		RetrieveInitializationDataRequest request = new RetrieveInitializationDataRequest(sessionToken);
		RetrieveInitializationDataResponse retrieveInitializationDataResponse = RetrieveInitializationData(request);
		issuedCertificate = retrieveInitializationDataResponse.issuedCertificate;
		shcSyncRecord = retrieveInitializationDataResponse.shcSyncRecord;
		furtherPollingRequired = retrieveInitializationDataResponse.furtherPollingRequired;
		pollAfterSeconds = retrieveInitializationDataResponse.pollAfterSeconds;
		return retrieveInitializationDataResponse.RetrieveInitializationDataResult;
	}

	private ConfirmShcOwnershipResponse ConfirmShcOwnership(ConfirmShcOwnershipRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ConfirmShcOwnership";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ConfirmShcOwnershipResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<ConfirmShcOwnershipRequest, ConfirmShcOwnershipResponse>(cFInvokeInfo, request);
	}

	public InitializationErrorCode ConfirmShcOwnership(string sessionToken, ShcMetadata shcMetadata, string shcInitializationResult)
	{
		ConfirmShcOwnershipRequest request = new ConfirmShcOwnershipRequest(sessionToken, shcMetadata, shcInitializationResult);
		ConfirmShcOwnershipResponse confirmShcOwnershipResponse = ConfirmShcOwnership(request);
		return confirmShcOwnershipResponse.ConfirmShcOwnershipResult;
	}

	private ShcResetByOwnerResponse ShcResetByOwner(ShcResetByOwnerRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ShcResetByOwner";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ShcResetByOwnerResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<ShcResetByOwnerRequest, ShcResetByOwnerResponse>(cFInvokeInfo, request);
	}

	public InitializationErrorCode ShcResetByOwner(string shcSerial)
	{
		ShcResetByOwnerRequest request = new ShcResetByOwnerRequest(shcSerial);
		ShcResetByOwnerResponse shcResetByOwnerResponse = ShcResetByOwner(request);
		return shcResetByOwnerResponse.ShcResetByOwnerResult;
	}

	private SubmitOwnershipRequestResponse SubmitOwnershipRequest(SubmitOwnershipRequestRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitOwnershipRequest";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitOwnershipRequestResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SubmitOwnershipRequestRequest, SubmitOwnershipRequestResponse>(cFInvokeInfo, request);
	}

	public InitializationErrorCode SubmitOwnershipRequest(string shcSerial, string pin, out string sessionToken)
	{
		SubmitOwnershipRequestRequest request = new SubmitOwnershipRequestRequest(shcSerial, pin);
		SubmitOwnershipRequestResponse submitOwnershipRequestResponse = SubmitOwnershipRequest(request);
		sessionToken = submitOwnershipRequestResponse.sessionToken;
		return submitOwnershipRequestResponse.SubmitOwnershipRequestResult;
	}

	private RetrieveOwnershipDataResponse RetrieveOwnershipData(RetrieveOwnershipDataRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveOwnershipData";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveOwnershipDataResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<RetrieveOwnershipDataRequest, RetrieveOwnershipDataResponse>(cFInvokeInfo, request);
	}

	public InitializationErrorCode RetrieveOwnershipData(string sessionToken, out ShcSyncRecord shcSyncRecord, out bool furtherPollingRequired, out int pollAfterSeconds)
	{
		RetrieveOwnershipDataRequest request = new RetrieveOwnershipDataRequest(sessionToken);
		RetrieveOwnershipDataResponse retrieveOwnershipDataResponse = RetrieveOwnershipData(request);
		shcSyncRecord = retrieveOwnershipDataResponse.shcSyncRecord;
		furtherPollingRequired = retrieveOwnershipDataResponse.furtherPollingRequired;
		pollAfterSeconds = retrieveOwnershipDataResponse.pollAfterSeconds;
		return retrieveOwnershipDataResponse.RetrieveOwnershipDataResult;
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
		if (CFClientBase<IShcInitializationService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitCertificateRequest", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitCertificateRequest", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveInitializationData", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveInitializationData", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ConfirmShcOwnership", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ConfirmShcOwnership", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ShcResetByOwner", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ShcResetByOwner", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitOwnershipRequest", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitOwnershipRequest", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveOwnershipData", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveOwnershipData", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IShcInitializationService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitCertificateRequestResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitCertificateRequestResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveInitializationDataResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveInitializationDataResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ConfirmShcOwnershipResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ConfirmShcOwnershipResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ShcResetByOwnerResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/ShcResetByOwnerResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitOwnershipRequestResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/SubmitOwnershipRequestResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveOwnershipDataResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IShcInitializationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IShcInitializationService/RetrieveOwnershipDataResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
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

	~ShcInitializationServiceClient()
	{
		Dispose(disposing: false);
	}
}
