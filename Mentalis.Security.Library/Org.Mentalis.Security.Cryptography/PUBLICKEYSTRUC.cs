using System;

namespace Org.Mentalis.Security.Cryptography;

internal struct PUBLICKEYSTRUC
{
	public byte bType;

	public byte bVersion;

	public short reserved;

	public IntPtr aiKeyAlg;
}
