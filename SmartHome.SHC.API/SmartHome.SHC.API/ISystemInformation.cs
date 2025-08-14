namespace SmartHome.SHC.API;

public interface ISystemInformation
{
	string ShcPersonalizedCertificateThumbprint { get; }

	string ShcSerialNumber { get; }

	bool IsInternetAccessAllowed { get; }

	bool LocalCommunicationEnabled { get; }

	bool IsShcLocalOnly { get; }
}
