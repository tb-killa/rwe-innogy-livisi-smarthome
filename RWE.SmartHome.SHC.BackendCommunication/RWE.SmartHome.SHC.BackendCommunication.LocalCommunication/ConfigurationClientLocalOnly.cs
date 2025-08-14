using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class ConfigurationClientLocalOnly : IConfigurationClient
{
	public void ReleaseServiceClient()
	{
	}

	public ConfigurationResultCode GetShcSyncRecord(string certificateThumbprint, string shcSerial, out ShcSyncRecord syncRecord)
	{
		syncRecord = new ShcSyncRecord();
		return ConfigurationResultCode.Success;
	}

	public ConfigurationResultCode ConfirmShcSyncRecord(string certificateThumbprint, string shcSerial)
	{
		return ConfigurationResultCode.Success;
	}

	public ConfigurationResultCode SetManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint)
	{
		return ConfigurationResultCode.Success;
	}

	public ConfigurationResultCode AddManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint)
	{
		return ConfigurationResultCode.Success;
	}

	public ConfigurationResultCode SetUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, byte[] data, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint)
	{
		return ConfigurationResultCode.Success;
	}

	public ConfigurationResultCode DeleteManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, bool createRestorePoint)
	{
		return ConfigurationResultCode.Success;
	}

	public ConfigurationResultCode DeleteUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, bool createRestorePoint)
	{
		return ConfigurationResultCode.Success;
	}

	public ConfigurationResultCode GetManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, out string result)
	{
		result = null;
		return ConfigurationResultCode.NotAuthorized;
	}

	public ConfigurationResultCode GetUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, out byte[] data)
	{
		data = null;
		return ConfigurationResultCode.NotAuthorized;
	}

	public ConfigurationResultCode GetRestorePointShcConfiguration(string certificateThumbprint, string restorePointId, string configDataVersion, out string result)
	{
		result = string.Empty;
		return ConfigurationResultCode.Success;
	}
}
