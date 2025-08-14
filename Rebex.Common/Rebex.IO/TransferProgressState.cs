namespace Rebex.IO;

public enum TransferProgressState
{
	DirectoryProcessing = 1,
	FileTransferring,
	FileTransferred,
	DataBlockProcessed,
	TransferCompleted
}
