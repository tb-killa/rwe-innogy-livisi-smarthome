namespace Rebex.IO;

public enum TransferProblemType
{
	FileExists = 1,
	LinkDetected,
	InfiniteLoopDetected,
	CannotCreateDirectory,
	CannotTransferFile,
	CannotReadFromDirectory,
	CannotFindFile,
	FileNameIsInvalidOnTargetFileSystem,
	DirectoryNameIsInvalidOnTargetFileSystem,
	CannotFindDirectory,
	CannotFindLink,
	CannotResolveLink,
	NotFileOrDirectory,
	OperationCanceled,
	UnsupportedFeature,
	CannotDeleteFile,
	CannotDeleteDirectory,
	CannotCalculateChecksum
}
