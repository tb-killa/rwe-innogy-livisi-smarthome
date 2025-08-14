using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IConfigurationClient
{
	ConfigurationResultCode GetShcSyncRecord(string certificateThumbprint, string shcSerial, out ShcSyncRecord syncRecord);

	ConfigurationResultCode ConfirmShcSyncRecord(string certificateThumbprint, string shcSerial);

	ConfigurationResultCode SetManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint);

	ConfigurationResultCode AddManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint);

	ConfigurationResultCode SetUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, byte[] data, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint);

	ConfigurationResultCode DeleteManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, bool createRestorePoint);

	ConfigurationResultCode DeleteUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, bool createRestorePoint);

	ConfigurationResultCode GetManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, out string result);

	ConfigurationResultCode GetUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, ConfigurationType configType, out byte[] data);

	ConfigurationResultCode GetRestorePointShcConfiguration(string certificateThumbprint, string restorePointId, string configDataVersion, out string result);

	void ReleaseServiceClient();
}
