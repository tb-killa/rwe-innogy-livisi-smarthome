namespace Rebex.IO;

public enum OverwriteCondition
{
	None = 0,
	SizeDiffers = 1,
	Older = 2,
	ChecksumDiffers = 4
}
