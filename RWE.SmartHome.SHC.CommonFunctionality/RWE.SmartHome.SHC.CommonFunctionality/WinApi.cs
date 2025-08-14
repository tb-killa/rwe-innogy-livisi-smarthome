using System;
using System.Runtime.InteropServices;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class WinApi
{
	private enum EventFlags
	{
		PULSE = 1,
		RESET,
		SET
	}

	public const int SYNCHRONIZE = 1048576;

	public const int STANDARD_RIGHTS_REQUIRED = 983040;

	public const int EVENT_ALL_ACCESS = 2031619;

	public const uint WAIT_OBJECT_0 = 0u;

	public const uint WAIT_ABANDONED = 128u;

	public const uint WAIT_ABANDONED_0 = 128u;

	public const uint WAIT_FAILED = uint.MaxValue;

	public const uint INFINITE = uint.MaxValue;

	public static int NOTIFICATION_EVENT_RS232_DETECTED = 9;

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, EntryPoint = "CreateEventW", SetLastError = true)]
	public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string name);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, EntryPoint = "OpenEventW", SetLastError = true)]
	public static extern IntPtr OpenEvent([In][MarshalAs(UnmanagedType.U4)] int desiredAccess, bool inheritHandle, string name);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern uint WaitForSingleObject(IntPtr Handle, uint Wait);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool EventModify(IntPtr hEvent, [In][MarshalAs(UnmanagedType.U4)] int dEvent);

	public static bool SetEvent(IntPtr hEvent)
	{
		return EventModify(hEvent, 3);
	}

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool CeRunAppAtEvent(string userEvent, int systemEvent);
}
