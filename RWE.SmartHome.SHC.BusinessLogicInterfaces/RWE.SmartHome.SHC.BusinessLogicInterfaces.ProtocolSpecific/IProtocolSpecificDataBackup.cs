namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public interface IProtocolSpecificDataBackup
{
	void Backup();

	void Restore(bool restoreDefaults);
}
