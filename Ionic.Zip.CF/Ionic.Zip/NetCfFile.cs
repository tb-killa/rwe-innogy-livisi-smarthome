using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip;

internal class NetCfFile
{
	public static int SetTimes(string filename, DateTime ctime, DateTime atime, DateTime mtime)
	{
		IntPtr intPtr = (IntPtr)CreateFileCE(filename, 1073741824u, 2u, 0, 3u, 0u, 0);
		if ((int)intPtr == -1)
		{
			return Marshal.GetLastWin32Error();
		}
		SetFileTime(intPtr, BitConverter.GetBytes(ctime.ToFileTime()), BitConverter.GetBytes(atime.ToFileTime()), BitConverter.GetBytes(mtime.ToFileTime()));
		CloseHandle(intPtr);
		return 0;
	}

	public static int SetLastWriteTime(string filename, DateTime mtime)
	{
		IntPtr intPtr = (IntPtr)CreateFileCE(filename, 1073741824u, 2u, 0, 3u, 0u, 0);
		if ((int)intPtr == -1)
		{
			return Marshal.GetLastWin32Error();
		}
		SetFileTime(intPtr, null, null, BitConverter.GetBytes(mtime.ToFileTime()));
		CloseHandle(intPtr);
		return 0;
	}

	[DllImport("coredll.dll", EntryPoint = "CreateFile", SetLastError = true)]
	internal static extern int CreateFileCE(string lpFileName, uint dwDesiredAccess, uint dwShareMode, int lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, int hTemplateFile);

	[DllImport("coredll", EntryPoint = "GetFileAttributes", SetLastError = true)]
	internal static extern uint GetAttributes(string lpFileName);

	[DllImport("coredll", EntryPoint = "SetFileAttributes", SetLastError = true)]
	internal static extern bool SetAttributes(string lpFileName, uint dwFileAttributes);

	[DllImport("coredll", SetLastError = true)]
	internal static extern bool SetFileTime(IntPtr hFile, byte[] lpCreationTime, byte[] lpLastAccessTime, byte[] lpLastWriteTime);

	[DllImport("coredll.dll", SetLastError = true)]
	internal static extern bool CloseHandle(IntPtr hObject);
}
