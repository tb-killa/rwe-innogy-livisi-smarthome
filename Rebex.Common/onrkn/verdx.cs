using System;
using System.Runtime.InteropServices;
using System.Text;

namespace onrkn;

internal static class verdx
{
	private const char buzvj = '\0';

	public static IntPtr loswp(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			return IntPtr.Zero;
		}
		byte[] bytes = Encoding.Unicode.GetBytes(p0 + '\0');
		IntPtr intPtr = Marshal.AllocHGlobal(bytes.Length);
		Marshal.Copy(bytes, 0, intPtr, bytes.Length);
		return intPtr;
	}
}
