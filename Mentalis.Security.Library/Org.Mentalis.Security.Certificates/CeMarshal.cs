using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Org.Mentalis.Security.Certificates;

public class CeMarshal
{
	internal static string PtrToStringAnsi(IntPtr p)
	{
		int i;
		for (i = 0; Marshal.ReadByte(p, i) != 0; i++)
		{
		}
		byte[] array = new byte[i];
		Marshal.Copy(p, array, 0, i);
		return Encoding.ASCII.GetString(array, 0, i);
	}

	internal static IntPtr StringToHGlobalAnsi(string input)
	{
		return StringToHGlobalUni(input);
	}

	internal static IntPtr StringToHGlobalUni(string input)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(input.Length * 2 + 2);
		Marshal.Copy(Encoding.Unicode.GetBytes(input), 0, intPtr, input.Length * 2);
		return intPtr;
	}
}
