using System;
using Rebex.IO;

namespace Rebex.Net;

public interface IFtpSettings : ICloneable
{
	LinkProcessingMode MultiFileLinkMode { get; set; }

	MoveMode MultiFileMoveMode { get; set; }

	ItemDateTimes RestoreDateTime { get; set; }

	TimeComparisonGranularity TimeComparisonGranularity { get; set; }

	bool RecheckItemExistence { get; set; }

	bool DisableProgressPercentage { get; set; }

	bool SkipDuplicateItems { get; set; }
}
