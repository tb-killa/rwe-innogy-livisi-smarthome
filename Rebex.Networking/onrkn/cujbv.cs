using System;
using System.Collections;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal class cujbv : ofuit
{
	private byte[] ubvdh;

	private ushort[] nnoyz;

	private DistinguishedName[] zpvtg;

	public DistinguishedName[] ilqau => zpvtg;

	public override int nimwj
	{
		get
		{
			int num = 4;
			num += 1 + ubvdh.Length;
			if (nnoyz != null && 0 == 0)
			{
				num += 2 + nnoyz.Length * 2;
			}
			num += 2;
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0038;
			}
			goto IL_0050;
			IL_0038:
			num += zpvtg[num2].ToArray().Length + 2;
			num2++;
			goto IL_0050;
			IL_0050:
			if (num2 < zpvtg.Length)
			{
				goto IL_0038;
			}
			return num;
		}
	}

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		p1 += 4;
		p0[p1] = (byte)ubvdh.Length;
		p1++;
		ubvdh.CopyTo(p0, p1);
		p1 += ubvdh.Length;
		int num2;
		if (nnoyz != null && 0 == 0)
		{
			int num = nnoyz.Length * 2;
			p0[p1] = (byte)((num >> 8) & 0xFF);
			p0[p1 + 1] = (byte)(num & 0xFF);
			p1 += 2;
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_007e;
			}
			goto IL_00ab;
		}
		goto IL_00b6;
		IL_00c4:
		int num3;
		byte[] array = zpvtg[num3].ToArray();
		p0[p1] = (byte)((array.Length >> 8) & 0xFF);
		p0[p1 + 1] = (byte)(array.Length & 0xFF);
		p1 += 2;
		array.CopyTo(p0, p1);
		p1 += array.Length;
		num3++;
		goto IL_0111;
		IL_0111:
		int num4;
		if (num3 >= zpvtg.Length)
		{
			p1 -= num4;
			p0[num4 - 2] = (byte)((p1 >> 8) & 0xFF);
			p0[num4 - 1] = (byte)(p1 & 0xFF);
			return;
		}
		goto IL_00c4;
		IL_007e:
		p0[p1] = (byte)(nnoyz[num2] >> 8);
		p0[p1 + 1] = (byte)(nnoyz[num2] & 0xFF);
		p1 += 2;
		num2++;
		goto IL_00ab;
		IL_00ab:
		if (num2 < nnoyz.Length)
		{
			goto IL_007e;
		}
		goto IL_00b6;
		IL_00b6:
		p1 += 2;
		num4 = p1;
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_00c4;
		}
		goto IL_0111;
	}

	public cujbv(DistinguishedName[] names, TlsProtocol protocol)
		: base(nsvut.jctye)
	{
		zpvtg = names;
		ubvdh = new byte[2] { 1, 2 };
		if (protocol >= TlsProtocol.TLS12)
		{
			nnoyz = new ushort[6] { 257, 513, 1025, 1281, 1537, 514 };
		}
	}

	public cujbv(byte[] buffer, int offset, int length, TlsProtocol protocol)
		: base(nsvut.jctye)
	{
		int num = offset + 4;
		if (offset + length < num + 1)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateRequest"));
		}
		int num2 = buffer[num];
		num++;
		if (offset + length < num + num2)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateRequest"));
		}
		ubvdh = new byte[num2];
		Array.Copy(buffer, num, ubvdh, 0, num2);
		num += num2;
		int num3;
		if (protocol >= TlsProtocol.TLS12)
		{
			if (offset + length < num + 2)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateRequest"));
			}
			num2 = buffer[num] * 256 + buffer[num + 1];
			num += 2;
			if (offset + length < num + num2 || (num2 & 1) != 0)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateRequest"));
			}
			num2 /= 2;
			nnoyz = new ushort[num2];
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_0129;
			}
			goto IL_0149;
		}
		goto IL_014d;
		IL_0149:
		if (num3 < num2)
		{
			goto IL_0129;
		}
		goto IL_014d;
		IL_014d:
		if (offset + length < num + 2)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateRequest"));
		}
		num2 = buffer[num] * 256 + buffer[num + 1];
		num += 2;
		int num4 = num + num2;
		ArrayList arrayList = new ArrayList();
		for (; num < num4; num += 2 + num2)
		{
			if (offset + length < num + 2)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateRequest"));
			}
			num2 = buffer[num] * 256 + buffer[num + 1];
			if (offset + length < num + 2 + num2)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateRequest"));
			}
			byte[] array = new byte[num2];
			Array.Copy(buffer, num + 2, array, 0, num2);
			arrayList.Add(new DistinguishedName(array));
		}
		zpvtg = (DistinguishedName[])arrayList.ToArray(typeof(DistinguishedName));
		return;
		IL_0129:
		nnoyz[num3] = (ushort)(buffer[num] * 256 + buffer[num + 1]);
		num += 2;
		num3++;
		goto IL_0149;
	}
}
