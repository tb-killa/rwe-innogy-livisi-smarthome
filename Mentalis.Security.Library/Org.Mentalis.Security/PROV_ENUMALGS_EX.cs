using System.Runtime.InteropServices;

namespace Org.Mentalis.Security;

internal struct PROV_ENUMALGS_EX
{
	public int aiAlgid;

	public int dwDefaultLen;

	public int dwMinLen;

	public int dwMaxLen;

	public int dwProtocols;

	public int dwNameLen;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
	public string szName;

	public int dwLongNameLen;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
	public string szLongName;
}
