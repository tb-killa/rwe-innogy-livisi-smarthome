using System.Runtime.InteropServices;

namespace Org.Mentalis.Security;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct PUBLIC_KEY_BLOB
{
	public byte bType;

	public byte bVersion;

	public short reserved;

	public int aiKeyAlg;

	public int magic;

	public int bitlen;

	public int pubexp;
}
