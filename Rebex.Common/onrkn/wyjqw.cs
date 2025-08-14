using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class wyjqw : jcgcz
{
	internal const string kfupr = "2.5.4.3";

	internal const string kbsec = "1.2.840.113549.1.9.1";

	internal const string eyobx = "1.2.840.113549.1.9.3";

	internal const string secrp = "1.2.840.113549.1.9.4";

	internal const string ccqtf = "1.2.840.113549.1.9.5";

	internal const string vepmg = "1.2.840.113549.1.9.16.2.11";

	internal const string ndjem = "1.2.840.113549.1.9.15";

	internal const string kkegy = "1.3.6.1.4.1.311.16.4";

	internal const string klkov = "1.2.840.113549.1.9.2";

	internal const string fjmcz = "1.2.840.113549.1.7.1";

	internal const string ulkue = "1.2.840.113549.1.7.2";

	internal const string xxprj = "1.2.840.113549.1.7.3";

	private ObjectIdentifier ekgtm;

	public ObjectIdentifier scakm => ekgtm;

	public wyjqw()
	{
	}

	public wyjqw(string oid)
		: this(new ObjectIdentifier(oid))
	{
	}

	public wyjqw(ObjectIdentifier oid)
		: base(rmkkr.rqoqx, onvtp(oid))
	{
		ekgtm = new ObjectIdentifier(oid);
	}

	private static byte[] onvtp(ObjectIdentifier p0)
	{
		if (p0 == null || false || p0.Value == null || false || p0.Value.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Invalid OID.", "oid");
		}
		return jizxm(p0.Value);
	}

	internal static byte[] jizxm(string p0)
	{
		string[] array = p0.Split('.');
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			agzrh(memoryStream, num * 40 + num2);
			int num3 = 2;
			if (num3 == 0)
			{
				goto IL_0044;
			}
			goto IL_005a;
			IL_0044:
			agzrh(memoryStream, int.Parse(array[num3]));
			num3++;
			goto IL_005a;
			IL_005a:
			if (num3 < array.Length)
			{
				goto IL_0044;
			}
			return memoryStream.ToArray();
		}
		catch (Exception)
		{
			throw new CryptographicException("Invalid OID.");
		}
		finally
		{
			memoryStream.Close();
		}
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.rqoqx, p0, p1);
		base.zkxnk(p0, p1, p2);
	}

	public override void somzq()
	{
		base.somzq();
		long[] p = hwxxc(base.rtrhq);
		ekgtm = new ObjectIdentifier(boeyk(p));
	}

	private static void agzrh(Stream p0, long p1)
	{
		int num = 56;
		if (num == 0)
		{
			goto IL_0007;
		}
		goto IL_0029;
		IL_0007:
		if (p1 >= 1L << num)
		{
			p0.WriteByte((byte)((p1 >> num) | 0x80));
		}
		num -= 7;
		goto IL_0029;
		IL_0029:
		if (num >= 7)
		{
			goto IL_0007;
		}
		p0.WriteByte((byte)(p1 & 0x7F));
	}

	public static wyjqw ewqkw(byte[] p0)
	{
		wyjqw wyjqw2 = new wyjqw();
		hfnnn.oalpn(wyjqw2, p0, 0, p0.Length);
		return wyjqw2;
	}

	private static long mbewh(Stream p0)
	{
		long num = 0L;
		int num2 = 0;
		byte b;
		do
		{
			int num3 = p0.ReadByte();
			if (num3 < 0)
			{
				throw new CryptographicException("OID subID is incomplete.");
			}
			num2 += 7;
			if (num2 > 63)
			{
				throw new CryptographicException("OID subID is too long.");
			}
			b = (byte)num3;
			num <<= 7;
			num |= (long)((ulong)b & 0x7FuL);
		}
		while ((b & 0x80) == 128);
		return num;
	}

	private static long[] hwxxc(byte[] p0)
	{
		Stream stream = new MemoryStream(p0);
		try
		{
			int num = p0.Length;
			ArrayList arrayList = new ArrayList();
			long num2 = mbewh(stream);
			if (num2 < 40)
			{
				arrayList.Add(0L);
				arrayList.Add(num2);
			}
			else if (num2 < 80)
			{
				arrayList.Add(1L);
				arrayList.Add(num2 - 40);
			}
			else
			{
				arrayList.Add(2L);
				arrayList.Add(num2 - 80);
			}
			while (stream.Position < num)
			{
				num2 = mbewh(stream);
				arrayList.Add(num2);
			}
			return (long[])arrayList.ToArray(typeof(long));
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	private static string boeyk(long[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0018;
		}
		goto IL_0037;
		IL_0018:
		if (num > 0)
		{
			stringBuilder.Append('.');
		}
		stringBuilder.arumx(p0[num]);
		num++;
		goto IL_0037;
		IL_0037:
		if (num < p0.Length)
		{
			goto IL_0018;
		}
		return stringBuilder.ToString();
	}
}
