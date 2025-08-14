using System;
using System.Runtime.InteropServices;

namespace Org.Mentalis.Security;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct CRYPT_KEY_PROV_INFO
{
	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwszContainerName;

	[MarshalAs(UnmanagedType.LPWStr)]
	public string pwszProvName;

	public int dwProvType;

	public int dwFlags;

	public int cProvParam;

	public IntPtr rgProvParam;

	public int dwKeySpec;
}
