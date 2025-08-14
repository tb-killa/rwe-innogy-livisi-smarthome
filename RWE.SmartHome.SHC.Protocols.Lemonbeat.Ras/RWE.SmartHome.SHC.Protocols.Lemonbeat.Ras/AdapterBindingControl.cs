using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class AdapterBindingControl
{
	private static class NativeMethods
	{
		[DllImport("coredll")]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, ref uint lpBytesReturned, IntPtr lpOverlapped);

		[DllImport("coredll")]
		public static extern IntPtr CreateFile(string filename, uint desiredAccess, uint shareMode, IntPtr securityAttributes, ECreationDisposition creationDisposition, int flags, IntPtr template);
	}

	private enum ECreationDisposition : uint
	{
		New = 1u,
		CreateAlways,
		OpenExisting,
		OpenAlways,
		TruncateExisting
	}

	private const uint GENERIC_READ = 2147483648u;

	private const uint GENERIC_WRITE = 1073741824u;

	private const uint FILE_SHARE_READ = 1u;

	private const uint FILE_SHARE_WRITE = 2u;

	private const int IOCTL_NDIS_UNBIND_ADAPTER = 1507382;

	private const string DD_NDIS_DEVICE_NAME = "NDS0:";

	public static void UnbindProtocol(string adapterName, string protocol)
	{
		IntPtr intPtr = NativeMethods.CreateFile("NDS0:", 3221225472u, 3u, IntPtr.Zero, ECreationDisposition.OpenAlways, 0, IntPtr.Zero);
		byte[] bytes = Encoding.Unicode.GetBytes(adapterName);
		byte[] bytes2 = Encoding.Unicode.GetBytes(protocol);
		int num = bytes.Length + 2 + bytes2.Length + 2 + 2;
		byte[] array = new byte[num];
		Array.Copy(bytes, array, bytes.Length);
		Array.Copy(bytes2, 0, array, bytes.Length + 2, bytes2.Length);
		uint lpBytesReturned = 0u;
		bool flag = NativeMethods.DeviceIoControl(intPtr, 1507382u, array, array.Length, null, 0, ref lpBytesReturned, IntPtr.Zero);
		NativeMethods.CloseHandle(intPtr);
		if (!flag)
		{
			throw new AdapterException(Marshal.GetLastWin32Error());
		}
	}
}
