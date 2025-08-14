namespace Rebex.IO;

public enum DeleteProgressState
{
	FileDeleting = 1,
	FileDeleted,
	DirectoryProcessing,
	DirectoryDeleted,
	DeleteCompleted
}
