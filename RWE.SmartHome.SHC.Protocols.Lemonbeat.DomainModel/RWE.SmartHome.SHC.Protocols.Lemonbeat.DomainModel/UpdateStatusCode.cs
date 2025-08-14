namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public enum UpdateStatusCode
{
	OK = 1,
	NotInitialized,
	WrongOffset,
	SizeTooLarge,
	ChecksumError,
	DataOverflow,
	ChunkTooLarge,
	DataMissing,
	ChunkSizeTooSmall
}
