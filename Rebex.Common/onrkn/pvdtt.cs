using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace onrkn;

internal static class pvdtt
{
	[DllImport("iphlpapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int GetNetworkParams(IntPtr pFixedInfo, ref int pOutBufLen);

	public static string[] pkjxw()
	{
		if (dahxy.xzevd && 0 == 0)
		{
			throw new NotSupportedException("Method is not supported on this platform.");
		}
		int pOutBufLen = 0;
		GetNetworkParams(IntPtr.Zero, ref pOutBufLen);
		if (pOutBufLen == 0 || 1 == 0)
		{
			throw new Exception("Unable to determine DNS servers.");
		}
		IntPtr intPtr = Marshal.AllocHGlobal(pOutBufLen);
		try
		{
			if (GetNetworkParams(intPtr, ref pOutBufLen) != 0 && 0 == 0)
			{
				throw new Exception("Unable to determine DNS servers.");
			}
			List<string> list = new List<string>();
			IntPtr intPtr2 = new IntPtr(intPtr.ToInt64() + 128 + 4 + 128 + 4 + IntPtr.Size);
			while ((intPtr2 != IntPtr.Zero) ? true : false)
			{
				string item = samhn.dztsn(new IntPtr(intPtr2.ToInt64() + IntPtr.Size));
				intPtr2 = samhn.yjjps(intPtr2, 0);
				list.Add(item);
			}
			return list.ToArray();
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}
}
