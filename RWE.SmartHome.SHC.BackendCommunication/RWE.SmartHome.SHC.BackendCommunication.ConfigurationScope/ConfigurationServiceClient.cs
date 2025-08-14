using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Tools.ServiceModel;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class ConfigurationServiceClient : CFClientBase<IConfigurationService>, IDisposable, IConfigurationService
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

	~ConfigurationServiceClient()
	{
		Dispose(disposing: false);
	}

	public ConfigurationServiceClient()
		: this(CreateDefaultBinding(), EndpointAddress)
	{
	}

	public ConfigurationServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
		addProtectionRequirements(binding);
	}

	private GetShcSyncRecordResponse GetShcSyncRecord(GetShcSyncRecordRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetShcSyncRecord";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetShcSyncRecordResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetShcSyncRecordRequest, GetShcSyncRecordResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode GetShcSyncRecord(string shcSerial, out ShcSyncRecord syncRecord)
	{
		GetShcSyncRecordRequest request = new GetShcSyncRecordRequest(shcSerial);
		GetShcSyncRecordResponse shcSyncRecord = GetShcSyncRecord(request);
		syncRecord = shcSyncRecord.syncRecord;
		return shcSyncRecord.GetShcSyncRecordResult;
	}

	private ConfirmShcSyncRecordResponse ConfirmShcSyncRecord(ConfirmShcSyncRecordRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/ConfirmShcSyncRecord";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/ConfirmShcSyncRecordResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<ConfirmShcSyncRecordRequest, ConfirmShcSyncRecordResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode ConfirmShcSyncRecord(string shcSerial)
	{
		ConfirmShcSyncRecordRequest request = new ConfirmShcSyncRecordRequest(shcSerial);
		ConfirmShcSyncRecordResponse confirmShcSyncRecordResponse = ConfirmShcSyncRecord(request);
		return confirmShcSyncRecordResponse.ConfirmShcSyncRecordResult;
	}

	private SetManagedSHCConfigurationResponse SetManagedSHCConfiguration(SetManagedSHCConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetManagedSHCConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetManagedSHCConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SetManagedSHCConfigurationRequest, SetManagedSHCConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode SetManagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst)
	{
		SetManagedSHCConfigurationRequest managedSHCConfiguration = new SetManagedSHCConfigurationRequest(shcSerialNo, configDataVersion, configType, xPath, bulkXMLData, currentPacketNumber, nextPacketNumber, correlationID, createRestorePointFirst);
		SetManagedSHCConfigurationResponse setManagedSHCConfigurationResponse = SetManagedSHCConfiguration(managedSHCConfiguration);
		return setManagedSHCConfigurationResponse.SetManagedSHCConfigurationResult;
	}

	private AddManagedSHCConfigurationResponse AddManagedSHCConfiguration(AddManagedSHCConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/AddManagedSHCConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/AddManagedSHCConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<AddManagedSHCConfigurationRequest, AddManagedSHCConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode AddManagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst)
	{
		AddManagedSHCConfigurationRequest request = new AddManagedSHCConfigurationRequest(shcSerialNo, configDataVersion, configType, xPath, bulkXMLData, currentPacketNumber, nextPacketNumber, correlationID, createRestorePointFirst);
		AddManagedSHCConfigurationResponse addManagedSHCConfigurationResponse = AddManagedSHCConfiguration(request);
		return addManagedSHCConfigurationResponse.AddManagedSHCConfigurationResult;
	}

	private SetUnmanagedSHCConfigurationResponse SetUnmanagedSHCConfiguration(SetUnmanagedSHCConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetUnmanagedSHCConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetUnmanagedSHCConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<SetUnmanagedSHCConfigurationRequest, SetUnmanagedSHCConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode SetUnmanagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, byte[] data, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst)
	{
		SetUnmanagedSHCConfigurationRequest unmanagedSHCConfiguration = new SetUnmanagedSHCConfigurationRequest(shcSerialNo, configDataVersion, configType, data, currentPacketNumber, nextPacketNumber, correlationID, createRestorePointFirst);
		SetUnmanagedSHCConfigurationResponse setUnmanagedSHCConfigurationResponse = SetUnmanagedSHCConfiguration(unmanagedSHCConfiguration);
		return setUnmanagedSHCConfigurationResponse.SetUnmanagedSHCConfigurationResult;
	}

	private DeleteManagedSHCConfigurationResponse DeleteManagedSHCConfiguration(DeleteManagedSHCConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteManagedSHCConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteManagedSHCConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<DeleteManagedSHCConfigurationRequest, DeleteManagedSHCConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode DeleteManagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, bool createRestorePointFirst)
	{
		DeleteManagedSHCConfigurationRequest request = new DeleteManagedSHCConfigurationRequest(shcSerialNo, configDataVersion, configType, xPath, createRestorePointFirst);
		DeleteManagedSHCConfigurationResponse deleteManagedSHCConfigurationResponse = DeleteManagedSHCConfiguration(request);
		return deleteManagedSHCConfigurationResponse.DeleteManagedSHCConfigurationResult;
	}

	private DeleteUnmanagedSHCConfigurationResponse DeleteUnmanagedSHCConfiguration(DeleteUnmanagedSHCConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteUnmanagedSHCConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteUnmanagedSHCConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<DeleteUnmanagedSHCConfigurationRequest, DeleteUnmanagedSHCConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode DeleteUnmanagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, bool createRestorePointFirst)
	{
		DeleteUnmanagedSHCConfigurationRequest request = new DeleteUnmanagedSHCConfigurationRequest(shcSerialNo, configDataVersion, configType, createRestorePointFirst);
		DeleteUnmanagedSHCConfigurationResponse deleteUnmanagedSHCConfigurationResponse = DeleteUnmanagedSHCConfiguration(request);
		return deleteUnmanagedSHCConfigurationResponse.DeleteUnmanagedSHCConfigurationResult;
	}

	private GetManagedSHCConfigurationResponse GetManagedSHCConfiguration(GetManagedSHCConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetManagedSHCConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetManagedSHCConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetManagedSHCConfigurationRequest, GetManagedSHCConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode GetManagedSHCConfiguration(string shcSerialNo, string configDataVersion, string configType, string xPath, out string result)
	{
		GetManagedSHCConfigurationRequest request = new GetManagedSHCConfigurationRequest(shcSerialNo, configDataVersion, configType, xPath);
		GetManagedSHCConfigurationResponse managedSHCConfiguration = GetManagedSHCConfiguration(request);
		result = managedSHCConfiguration.result;
		return managedSHCConfiguration.GetManagedSHCConfigurationResult;
	}

	private GetUnmanagedSHCConfigurationResponse GetUnmanagedSHCConfiguration(GetUnmanagedSHCConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetUnmanagedSHCConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetUnmanagedSHCConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetUnmanagedSHCConfigurationRequest, GetUnmanagedSHCConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode GetUnmanagedSHCConfiguration(string shcSerialNo, string configDataVersion, string configType, out byte[] data)
	{
		GetUnmanagedSHCConfigurationRequest request = new GetUnmanagedSHCConfigurationRequest(shcSerialNo, configDataVersion, configType);
		GetUnmanagedSHCConfigurationResponse unmanagedSHCConfiguration = GetUnmanagedSHCConfiguration(request);
		data = unmanagedSHCConfiguration.data;
		return unmanagedSHCConfiguration.GetUnmanagedSHCConfigurationResult;
	}

	private GetRestorePointShcConfigurationResponse GetRestorePointShcConfiguration(GetRestorePointShcConfigurationRequest request)
	{
		CFInvokeInfo cFInvokeInfo = new CFInvokeInfo();
		cFInvokeInfo.Action = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetRestorePointShcConfiguration";
		cFInvokeInfo.RequestIsWrapped = true;
		cFInvokeInfo.ReplyAction = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetRestorePointShcConfigurationResponse";
		cFInvokeInfo.ResponseIsWrapped = true;
		return Invoke<GetRestorePointShcConfigurationRequest, GetRestorePointShcConfigurationResponse>(cFInvokeInfo, request);
	}

	public ConfigurationResultCode GetRestorePointShcConfiguration(string restorePointId, string configDataVersion, out string result)
	{
		GetRestorePointShcConfigurationRequest request = new GetRestorePointShcConfigurationRequest(restorePointId, configDataVersion);
		GetRestorePointShcConfigurationResponse restorePointShcConfiguration = GetRestorePointShcConfiguration(request);
		result = restorePointShcConfiguration.result;
		return restorePointShcConfiguration.GetRestorePointShcConfigurationResult;
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
		if (CFClientBase<IConfigurationService>.IsSecureMessageBinding(binding))
		{
			ChannelProtectionRequirements channelProtectionRequirements = new ChannelProtectionRequirements();
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetShcSyncRecord", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetShcSyncRecord", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/ConfirmShcSyncRecord", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/ConfirmShcSyncRecord", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetManagedSHCConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetManagedSHCConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/AddManagedSHCConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/AddManagedSHCConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetUnmanagedSHCConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetUnmanagedSHCConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteManagedSHCConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteManagedSHCConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteUnmanagedSHCConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteUnmanagedSHCConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetManagedSHCConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetManagedSHCConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetUnmanagedSHCConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetUnmanagedSHCConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetRestorePointShcConfiguration", channelProtectionRequirements.IncomingSignatureParts, protection: true);
			CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetRestorePointShcConfiguration", channelProtectionRequirements.IncomingEncryptionParts, protection: true);
			if (binding.MessageVersion.Addressing == AddressingVersion.None)
			{
				CFClientBase<IConfigurationService>.ApplyProtection("*", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("*", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			else
			{
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetShcSyncRecordResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetShcSyncRecordResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/ConfirmShcSyncRecordResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/ConfirmShcSyncRecordResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/AddManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/AddManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetUnmanagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/SetUnmanagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteUnmanagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/DeleteUnmanagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetManagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetUnmanagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetUnmanagedSHCConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetRestorePointShcConfigurationResponse", channelProtectionRequirements.OutgoingSignatureParts, protection: true);
				CFClientBase<IConfigurationService>.ApplyProtection("http://rwe.com/SmartHome/2010/11/08/PublicFacingServices/IConfigurationService/GetRestorePointShcConfigurationResponse", channelProtectionRequirements.OutgoingEncryptionParts, protection: true);
			}
			base.Parameters.Add(channelProtectionRequirements);
		}
	}
}
