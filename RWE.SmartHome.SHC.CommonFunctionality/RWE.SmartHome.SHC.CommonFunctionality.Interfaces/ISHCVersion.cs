namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

internal interface ISHCVersion
{
	string HardwareVersion { get; }

	string ApplicationVersion { get; }

	string OsVersion { get; }
}
