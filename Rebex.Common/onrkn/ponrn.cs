using System;
using Rebex;
using Rebex.IO;

namespace onrkn;

internal static class ponrn
{
	public static T uekgz<T>(T p0, T p1) where T : struct
	{
		if (!typeof(T).IsEnum || 1 == 0)
		{
			throw new ArgumentException("Not an enum.", "value");
		}
		if (p0.Equals(default(T)) && 0 == 0)
		{
			return p1;
		}
		return p0;
	}

	public static void unjnj(TimeComparisonGranularity p0, string p1)
	{
		switch (p0)
		{
		case TimeComparisonGranularity.None:
		case TimeComparisonGranularity.Seconds:
		case TimeComparisonGranularity.TwoSeconds:
		case TimeComparisonGranularity.Days:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Invalid enum value.");
	}

	public static void xquiy(TraversalMode p0, string p1)
	{
		switch (p0)
		{
		case TraversalMode.Recursive:
		case TraversalMode.NonRecursive:
		case TraversalMode.MatchFilesShallow:
		case TraversalMode.MatchFilesDeep:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Invalid enum value.");
	}

	public static void ctygl(TransferMethod p0, string p1)
	{
		switch (p0)
		{
		case TransferMethod.Copy:
		case TransferMethod.Move:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Invalid enum value.");
	}

	public static void huluw(ActionOnExistingFiles p0, string p1)
	{
		switch (p0)
		{
		case ActionOnExistingFiles.ThrowException:
		case ActionOnExistingFiles.SkipAll:
		case ActionOnExistingFiles.OverwriteAll:
		case ActionOnExistingFiles.OverwriteOlder:
		case ActionOnExistingFiles.OverwriteDifferentSize:
		case ActionOnExistingFiles.ResumeIfPossible:
		case ActionOnExistingFiles.Rename:
		case ActionOnExistingFiles.OverwriteDifferentChecksum:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Invalid enum value.");
	}

	public static void lbayr(LinkProcessingMode p0, string p1)
	{
		switch (p0)
		{
		case LinkProcessingMode.FollowLinks:
		case LinkProcessingMode.SkipLinks:
		case LinkProcessingMode.ThrowExceptionOnLinks:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Invalid enum value.");
	}

	public static void bwnrl(LinkType p0, string p1)
	{
		switch (p0)
		{
		case LinkType.Symbolic:
		case LinkType.Hard:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Invalid enum value.");
	}

	public static void rzmkm(MoveMode p0, string p1)
	{
		switch (p0)
		{
		case MoveMode.All:
		case MoveMode.FilesOnly:
			return;
		}
		throw hifyx.nztrs(p1, p0, "Invalid enum value.");
	}

	public static void cagxo(ItemDateTimes p0, string p1)
	{
		if ((p0 & ~ItemDateTimes.All) != ItemDateTimes.None && 0 == 0)
		{
			throw hifyx.nztrs(p1, p0, "Invalid enum value.");
		}
	}
}
