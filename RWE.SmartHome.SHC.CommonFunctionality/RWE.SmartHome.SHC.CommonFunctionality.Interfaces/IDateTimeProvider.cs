using System;

namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

public interface IDateTimeProvider
{
	DateTime Now { get; }

	DateTime UtcNow { get; }

	DateTime LocalToUtc(DateTime localTime);

	DateTime UtcToLocal(DateTime utcTime);
}
