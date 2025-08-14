using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IConfigurationService
{
	ConfigurationResultCode GetShcSyncRecord(string shcSerial, out ShcSyncRecord syncRecord);

	ConfigurationResultCode ConfirmShcSyncRecord(string shcSerial);

	ConfigurationResultCode SetManagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst);

	ConfigurationResultCode AddManagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst);

	ConfigurationResultCode SetUnmanagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, byte[] data, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst);

	ConfigurationResultCode DeleteManagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, bool createRestorePointFirst);

	ConfigurationResultCode DeleteUnmanagedSHCConfiguration(string shcSerialNo, string configDataVersion, ConfigurationType configType, bool createRestorePointFirst);

	ConfigurationResultCode GetManagedSHCConfiguration(string shcSerialNo, string configDataVersion, string configType, string xPath, out string result);

	ConfigurationResultCode GetUnmanagedSHCConfiguration(string shcSerialNo, string configDataVersion, string configType, out byte[] data);

	ConfigurationResultCode GetRestorePointShcConfiguration(string restorePointId, string configDataVersion, out string result);
}
