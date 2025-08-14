using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class poojt
{
	public PublicKeyInfo oezmx(byte[] p0)
	{
		throw new NotImplementedException();
	}

	public byte[] xopmb(PublicKeyInfo p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("publicKeyInfo");
		}
		RSAParameters p1 = p0.GetRSAParameters();
		return cjuar(ref p1);
	}

	private byte[] cjuar(ref RSAParameters p0)
	{
		milge.khiul(ref p0);
		int num = Marshal.SizeOf(typeof(pothu.swccb)) + p0.Modulus.Length;
		byte[] buffer = new byte[num];
		wmbjj wmbjj2 = new wmbjj(buffer);
		wmbjj2.ywmoe(6);
		wmbjj2.ywmoe(2);
		wmbjj2.seieo(0);
		wmbjj2.kplbc(41984);
		wmbjj2.kplbc(826364754);
		int p1 = p0.Modulus.Length * 8;
		wmbjj2.kplbc(p1);
		byte[] array = new byte[4];
		Array.Copy(p0.Exponent, 0, array, array.Length - p0.Exponent.Length, p0.Exponent.Length);
		Array.Reverse(array, 0, array.Length);
		wmbjj2.udtyl(array);
		byte[] array2 = p0.Modulus.wwots(0, p0.Modulus.Length);
		Array.Reverse(array2, 0, array2.Length);
		wmbjj2.udtyl(array2);
		return wmbjj2.ihelo();
	}
}
