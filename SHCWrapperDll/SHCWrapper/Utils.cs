using System;
using System.Runtime.InteropServices;

namespace SHCWrapper;

public class Utils
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct SYSTEMTIME
	{
		public short wYear;

		public short wMonth;

		public short wDayOfWeek;

		public short wDay;

		public short wHour;

		public short wMinute;

		public short wSecond;

		public short wMilliseconds;
	}

	public const int WAIT_OBJECT_0 = 0;

	public const int INFINITE = -1;

	public const int WAIT_FAILED = -1;

	public const int WAIT_TIMEOUT = 258;

	public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	public static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	public static extern bool CloseHandle(IntPtr hHandle);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	public static extern bool SetSystemTime([In] ref SYSTEMTIME st);

	public static bool SetSystemTime(DateTime date_time)
	{
		SYSTEMTIME st = new SYSTEMTIME
		{
			wYear = (short)date_time.Year,
			wMonth = (short)date_time.Month,
			wDay = (short)date_time.Day,
			wHour = (short)date_time.Hour,
			wMinute = (short)date_time.Minute,
			wSecond = (short)date_time.Second,
			wMilliseconds = (short)date_time.Millisecond
		};
		return SetSystemTime(ref st);
	}
}
