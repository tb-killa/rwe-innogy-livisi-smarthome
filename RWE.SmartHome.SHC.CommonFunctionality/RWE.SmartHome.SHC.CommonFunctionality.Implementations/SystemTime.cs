using System;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

internal struct SystemTime
{
	public short wYear;

	public short wMonth;

	public short wDayOfWeek;

	public short wDay;

	public short wHour;

	public short wMinute;

	public short wSecond;

	public short wMilliseconds;

	public static implicit operator SystemTime(DateTime dt)
	{
		return new SystemTime
		{
			wYear = (short)dt.Year,
			wMonth = (short)dt.Month,
			wDay = (short)dt.Day,
			wDayOfWeek = (short)dt.DayOfWeek,
			wHour = (short)dt.Hour,
			wMinute = (short)dt.Minute,
			wSecond = (short)dt.Second,
			wMilliseconds = (short)dt.Millisecond
		};
	}

	public static implicit operator DateTime(SystemTime st)
	{
		return new DateTime(st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);
	}
}
