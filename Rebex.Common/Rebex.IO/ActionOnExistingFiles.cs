namespace Rebex.IO;

public enum ActionOnExistingFiles
{
	ThrowException,
	SkipAll,
	OverwriteAll,
	OverwriteOlder,
	OverwriteDifferentSize,
	ResumeIfPossible,
	Rename,
	OverwriteDifferentChecksum
}
