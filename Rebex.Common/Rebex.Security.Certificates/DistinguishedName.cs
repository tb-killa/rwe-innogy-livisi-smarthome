using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using onrkn;

namespace Rebex.Security.Certificates;

public class DistinguishedName
{
	private byte[] nkkyg;

	private byte[] mioef => nkkyg;

	public DistinguishedName(byte[] dn)
	{
		nkkyg = dn;
	}

	public DistinguishedName(string dn)
	{
		if (dn == null || 1 == 0)
		{
			throw new ArgumentNullException("dn");
		}
		int pcbEncoded = 0;
		if (pothu.CertStrToName(1u, dn, 33554435u, IntPtr.Zero, null, ref pcbEncoded, IntPtr.Zero) == 0 || 1 == 0)
		{
			throw new CertificateException(brgjd.edcru("Unable to convert string to DN ({0}).", Marshal.GetLastWin32Error()));
		}
		nkkyg = new byte[pcbEncoded];
		if (pothu.CertStrToName(1u, dn, 33554435u, IntPtr.Zero, nkkyg, ref pcbEncoded, IntPtr.Zero) == 0 || 1 == 0)
		{
			throw new CertificateException(brgjd.edcru("Unable to convert string to DN ({0}).", Marshal.GetLastWin32Error()));
		}
	}

	internal samhn btuxq()
	{
		byte[] array = nkkyg;
		samhn samhn = new samhn(array.Length + 2 * IntPtr.Size);
		samhn.fpzdi(0, array.Length);
		samhn.qurik(IntPtr.Size, new IntPtr(samhn.inyna().ToInt64() + 2 * IntPtr.Size));
		samhn.zqmse(array, 0, array.Length, 2 * IntPtr.Size);
		return samhn;
	}

	public string GetCommonName()
	{
		byte[] p = mioef;
		suzxs suzxs = new suzxs();
		hfnnn.qnzgo(suzxs, p);
		IEnumerator<hjdlb> enumerator = suzxs.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				hjdlb current = enumerator.Current;
				IEnumerator<wusby> enumerator2 = current.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext() ? true : false)
					{
						wusby current2 = enumerator2.Current;
						if (current2.sdyid.Value == "2.5.4.3" && 0 == 0)
						{
							vesyi vesyi = vesyi.onwas(current2.fajfk);
							return vesyi.dcokg;
						}
					}
				}
				finally
				{
					if (enumerator2 != null && 0 == 0)
					{
						enumerator2.Dispose();
					}
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return null;
	}

	public string[] GetMailAddresses()
	{
		byte[] p = mioef;
		List<string> list = new List<string>();
		suzxs suzxs = new suzxs();
		hfnnn.qnzgo(suzxs, p);
		IEnumerator<hjdlb> enumerator = suzxs.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				hjdlb current = enumerator.Current;
				IEnumerator<wusby> enumerator2 = current.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext() ? true : false)
					{
						wusby current2 = enumerator2.Current;
						if (current2.sdyid.Value == "1.2.840.113549.1.9.1" && 0 == 0)
						{
							vesyi vesyi = vesyi.onwas(current2.fajfk);
							if (vesyi.dcokg != null && 0 == 0)
							{
								list.Add(vesyi.dcokg);
							}
						}
					}
				}
				finally
				{
					if (enumerator2 != null && 0 == 0)
					{
						enumerator2.Dispose();
					}
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return list.ToArray();
	}

	internal bool gcuoq(string p0)
	{
		byte[] p1 = mioef;
		suzxs suzxs = new suzxs();
		hfnnn.qnzgo(suzxs, p1);
		IEnumerator<hjdlb> enumerator = suzxs.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				hjdlb current = enumerator.Current;
				IEnumerator<wusby> enumerator2 = current.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext() ? true : false)
					{
						wusby current2 = enumerator2.Current;
						if (current2.sdyid.Value == "1.2.840.113549.1.9.1" && 0 == 0)
						{
							vesyi vesyi = vesyi.onwas(current2.fajfk);
							if (string.Compare(vesyi.ToString(), p0, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
							{
								return true;
							}
						}
					}
				}
				finally
				{
					if (enumerator2 != null && 0 == 0)
					{
						enumerator2.Dispose();
					}
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return false;
	}

	internal DistinguishedName qspkt()
	{
		byte[] p = mioef;
		suzxs suzxs = new suzxs();
		hfnnn.qnzgo(suzxs, p);
		int num = 0;
		if (num != 0)
		{
			goto IL_001c;
		}
		goto IL_0092;
		IL_001c:
		hjdlb hjdlb = suzxs[num];
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_002b;
		}
		goto IL_0069;
		IL_002b:
		wusby wusby = hjdlb[num2];
		if (wusby.sdyid.Value == "1.2.840.113549.1.9.2" && 0 == 0)
		{
			hjdlb.RemoveAt(num2);
			num2--;
		}
		num2++;
		goto IL_0069;
		IL_0092:
		if (num >= suzxs.Count)
		{
			p = fxakl.kncuz(suzxs);
			return new DistinguishedName(p);
		}
		goto IL_001c;
		IL_0069:
		if (num2 < hjdlb.Count)
		{
			goto IL_002b;
		}
		if (hjdlb.Count == 0 || 1 == 0)
		{
			suzxs.RemoveAt(num);
			num--;
		}
		num++;
		goto IL_0092;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is DistinguishedName distinguishedName) || 1 == 0)
		{
			return false;
		}
		byte[] array = mioef;
		byte[] array2 = distinguishedName.mioef;
		if (array.Length != array2.Length)
		{
			return false;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0032;
		}
		goto IL_0040;
		IL_0032:
		if (array[num] != array2[num])
		{
			return false;
		}
		num++;
		goto IL_0040;
		IL_0040:
		if (num < array.Length)
		{
			goto IL_0032;
		}
		return true;
	}

	public override int GetHashCode()
	{
		byte[] array = mioef;
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_000f;
		}
		goto IL_0020;
		IL_000f:
		num = (num << 4) ^ (array[num2] ^ (num >> 28));
		num2++;
		goto IL_0020;
		IL_0020:
		if (num2 < array.Length)
		{
			goto IL_000f;
		}
		return num;
	}

	public byte[] ToArray()
	{
		return (byte[])mioef.Clone();
	}

	public override string ToString()
	{
		samhn samhn = null;
		samhn samhn2 = null;
		try
		{
			samhn = btuxq();
			int num = pothu.CertNameToStr(1u, samhn.inyna(), 33554435u, IntPtr.Zero, 0);
			if (num <= 1)
			{
				return null;
			}
			samhn2 = new samhn(num * 2);
			num = pothu.CertNameToStr(65537u, samhn.inyna(), 33554435u, samhn2.inyna(), num);
			if (num <= 1)
			{
				return null;
			}
			return Marshal.PtrToStringUni(samhn2.inyna(), num - 1);
		}
		finally
		{
			if (samhn != null && 0 == 0)
			{
				samhn.Dispose();
			}
			if (samhn2 != null && 0 == 0)
			{
				samhn2.Dispose();
			}
		}
	}

	public static byte[] FromString(string dn)
	{
		return zeydn.oabhp(dn);
	}

	public static string ToString(byte[] dn)
	{
		return rcbhr.hufkx(dn);
	}
}
