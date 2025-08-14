using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal static class dvfml
{
	public enum vlgfn
	{
		mbqzs,
		lhvup
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct toapj
	{
		public uint dyffe;

		public uint gkzuh;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ggvit
	{
		public uint kkcvj;

		public toapj oxyjs;

		public toapj bildo;

		public toapj ibxwp;

		public uint kqzax;

		public uint qbeak;

		public uint mmkll;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string hhxbq;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct txocr
	{
		public uint lhgzr;

		public toapj lljru;

		public toapj xpgro;

		public toapj esldj;

		public uint qpdga;

		public uint odbve;
	}

	public const string lmspq = "\\\\?\\";

	public const uint gwduz = 2147483648u;

	public const uint ziroa = 1073741824u;

	public const uint eietd = 536870912u;

	public const uint mtdyh = 268435456u;

	public const uint mxkoh = 1u;

	public const uint xxrcj = 2u;

	public const int qedbk = 1;

	public const int sdjok = 2;

	public const int csodu = 3;

	public const int cgboh = 4;

	public const int iyjnt = 5;

	public const uint tkxnx = 1u;

	public const uint zshvc = 2u;

	public const uint klonl = 4u;

	public const uint ywzhr = 16u;

	public const uint ktnlx = 32u;

	public const uint blhwr = 64u;

	public const uint nyien = 128u;

	public const uint wdrkm = 256u;

	public const uint lstpq = 512u;

	public const uint mgzdz = 1024u;

	public const uint mqbwi = 2048u;

	public const uint fxunt = 4096u;

	public const uint pmkgn = 8192u;

	public const uint cyfpp = 16384u;

	public const uint dmytz = 2147483648u;

	public const uint pdrsu = 1073741824u;

	public const uint ertix = 536870912u;

	public const uint peqgs = 268435456u;

	public const uint qcpsy = 134217728u;

	public const uint unnoc = 67108864u;

	public const uint hjarz = 33554432u;

	public const uint gwufv = 16777216u;

	public const uint ngqdf = 2097152u;

	public const uint tyflb = 1048576u;

	public const uint ogxkj = 524288u;

	public const uint mlwmj = 0u;

	public const uint oegcs = 65536u;

	public const uint fcrue = 131072u;

	public const uint fssez = 196608u;

	public const uint zrmwm = 262144u;

	public const uint rhzvk = 524288u;

	public const uint limce = 1048576u;

	public const uint srrzy = 2031616u;

	private static readonly DateTime zwscc = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static readonly IntPtr zhchs = new IntPtr(-1);

	[DllImport("coredll.dll", SetLastError = true)]
	public static extern int CloseHandle(IntPtr hObject);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

	[DllImport("coredll.dll", SetLastError = true)]
	public static extern bool SetFileTime(IntPtr hFile, toapj[] lpCreationTime, toapj[] lpLastAccessTime, toapj[] lpLastWriteTime);

	[DllImport("coredll.dll", SetLastError = true)]
	public static extern bool SetEndOfFile(IntPtr hFile);

	[DllImport("coredll.dll", SetLastError = true)]
	private static extern uint SetFilePointer(IntPtr hFile, uint lDistanceToMove, ref uint lpDistanceToMoveHigh, vnfav dwMoveMethod);

	public static bool umllv(IntPtr p0, long p1, out long p2, vnfav p3)
	{
		uint lDistanceToMove = (uint)(p1 & 0xFFFFFFFFu);
		uint lpDistanceToMoveHigh = (uint)((ulong)p1 >> 32);
		lDistanceToMove = SetFilePointer(p0, lDistanceToMove, ref lpDistanceToMoveHigh, p3);
		if (lDistanceToMove == uint.MaxValue && Marshal.GetLastWin32Error() != 0 && 0 == 0)
		{
			p2 = 0L;
			return false;
		}
		ulong num = lpDistanceToMoveHigh;
		num = (num << 32) + lDistanceToMove;
		p2 = (long)num;
		return true;
	}

	[DllImport("coredll.dll", SetLastError = true)]
	public static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

	[DllImport("coredll.dll", SetLastError = true)]
	public static extern bool WriteFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

	[DllImport("coredll.dll", SetLastError = true)]
	public static extern bool FlushFileBuffers(IntPtr hFile);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern IntPtr FindFirstFile(string pFileName, ref ggvit pFindFileData);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool FindNextFile(IntPtr hFindFile, ref ggvit lpFindFileData);

	[DllImport("coredll.dll", SetLastError = true)]
	public static extern bool FindClose(IntPtr hFindFile);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool GetFileAttributesEx(string lpFileName, vlgfn fInfoLevelId, out txocr lpFileInformation);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool DeleteFile(string lpFileName);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool RemoveDirectory(string lpPathName);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool CreateDirectory(string lpPathName, IntPtr lpSecurityAttributes);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool MoveFile(string lpExistingFileName, string lpNewFileName);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, IntPtr lpProgressRoutine, IntPtr lpData, IntPtr pbCancel, int dwCopyFlags);

	public static toapj pxrli(DateTime p0)
	{
		long num = p0.ToUniversalTime().Ticks - zwscc.Ticks;
		if (num < 0)
		{
			num = 0L;
		}
		return new toapj
		{
			gkzuh = (uint)(num >> 32),
			dyffe = (uint)(num & 0xFFFFFFFFu)
		};
	}

	public static DateTime tlucp(toapj p0)
	{
		long num = p0.gkzuh;
		num = (num << 32) + p0.dyffe;
		return zwscc.AddTicks(num);
	}

	public static bool zhwer(IntPtr p0)
	{
		long num = p0.ToInt64();
		if (num == 0 || num == -1)
		{
			return true;
		}
		return false;
	}
}
