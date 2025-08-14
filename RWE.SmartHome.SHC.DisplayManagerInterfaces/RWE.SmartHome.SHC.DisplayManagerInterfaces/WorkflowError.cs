namespace RWE.SmartHome.SHC.DisplayManagerInterfaces;

public enum WorkflowError
{
	SystemStartupError,
	NetworkAdapterNotOperational,
	NoDhcpIpAddress,
	NoDhcpDefaultGateway,
	NameResolutionFailed,
	NameResolutionFailedNetworkDown,
	NtpUnavailable,
	SoftwareUpdateServiceUnavailable,
	SoftwareUpdateServiceResponseInvalid,
	SoftwareDownloadServiceUnavailable,
	SoftwareDownloadServiceResponseInvalid,
	FileSystemError,
	DirectFlashWriteError,
	InvalidSerialNumber,
	NoDefaultCertificateFound,
	UsbStickLogExport_UnsupportedFileSystem,
	UsbStickLogExport_WriteFailed,
	UsbStickLogExport_OtherError,
	InitializationServiceUnavailable,
	RelayServiceUnavailable,
	CoprocessorUpdateFailed
}
