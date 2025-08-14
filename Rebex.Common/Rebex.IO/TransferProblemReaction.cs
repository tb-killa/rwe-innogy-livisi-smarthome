namespace Rebex.IO;

public enum TransferProblemReaction
{
	Cancel = 1,
	Fail = 2,
	Skip = 4,
	Retry = 8,
	Overwrite = 0x10,
	Rename = 0x20,
	Resume = 0x40,
	FollowLink = 0x80
}
