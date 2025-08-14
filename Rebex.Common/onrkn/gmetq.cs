using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace onrkn;

internal class gmetq
{
	public const string ptqlv = "Microsoft Base DSS and Diffie-Hellman Cryptographic Provider";

	public const string uihvi = "Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider";

	public const string xyqvl = "Microsoft Base Cryptographic Provider v1.0";

	public const string peuef = "Microsoft RSA Signature Cryptographic Provider";

	public const string qbxya = "Microsoft RSA SChannel Strong Cryptographic Provider";

	public const string effjp = "Microsoft Enhanced Cryptographic Provider v1.0";

	public const string qynwu = "Microsoft Strong Cryptographic Provider";

	public const string upmsq = "Microsoft Enhanced RSA and AES Cryptographic Provider";

	public const string aewai = "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)";

	private static readonly object lzsxl = new object();

	private static IntPtr ijwmt;

	private static IntPtr jdhue;

	private static IntPtr nywxs;

	private static bool mmiul;

	private static bool uhbab;

	private static int pcboz;

	private static int xugls;

	private static string iuiao;

	private static int yhyym;

	public static bool xnmst()
	{
		ryhlf();
		return mmiul;
	}

	public static bool kiqyt()
	{
		gdehj(p0: false);
		return nywxs != IntPtr.Zero;
	}

	public static bool bzdqw()
	{
		gdehj(p0: false);
		return uhbab;
	}

	public static void qjkxo(ref string p0, ref int p1)
	{
		lock (lzsxl)
		{
			if (iuiao == null || 1 == 0)
			{
				string text = "Microsoft Enhanced RSA and AES Cryptographic Provider";
				int num = 24;
				int p2;
				IntPtr intPtr = wschq(text, num, 4026531840u, out p2);
				if (intPtr == IntPtr.Zero && 0 == 0)
				{
					iuiao = null;
					yhyym = 1;
				}
				else
				{
					if (uggao(intPtr) != text && 0 == 0)
					{
						text = p0;
						num = p1;
					}
					pothu.obsxv(intPtr, 0);
					iuiao = (p0 = text);
					yhyym = (p1 = num);
				}
			}
			p0 = iuiao;
			p1 = yhyym;
		}
	}

	public static string uggao(IntPtr p0)
	{
		byte[] array = new byte[256];
		int p1 = array.Length;
		if (pothu.dwkha(p0, 4, array, ref p1, 0) == 0 || 1 == 0)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			throw new CryptographicException(brgjd.edcru("Unable to determine provider name (0x{0:X8}).", lastWin32Error));
		}
		string text = Encoding.Unicode.GetString(array, 0, p1);
		char[] trimChars = new char[1];
		return text.TrimEnd(trimChars);
	}

	public static IntPtr cskhq(out int p0)
	{
		if (jdhue != IntPtr.Zero && 0 == 0)
		{
			p0 = 0;
			return jdhue;
		}
		jdhue = wschq("Microsoft Enhanced RSA and AES Cryptographic Provider", 24, 4026531840u, out p0);
		return jdhue;
	}

	public static IntPtr ryhlf()
	{
		if (ijwmt != IntPtr.Zero && 0 == 0)
		{
			return ijwmt;
		}
		ijwmt = wschq(null, 1, 0u, out var p);
		if (ijwmt == IntPtr.Zero && 0 == 0)
		{
			throw iiyyx("RSA", p);
		}
		byte[] array = new byte[256];
		int p2 = array.Length;
		if (pothu.dwkha(ijwmt, 4, array, ref p2, 0) != 0 && 0 == 0)
		{
			string text = Encoding.Unicode.GetString(array, 0, p2);
			if (!text.StartsWith("Microsoft Base", StringComparison.Ordinal) || 1 == 0)
			{
				mmiul = true;
			}
		}
		return ijwmt;
	}

	public static IntPtr gdehj(bool p0)
	{
		if (nywxs != IntPtr.Zero && 0 == 0)
		{
			return nywxs;
		}
		nywxs = wschq("Microsoft Enhanced DSS and Diffie-Hellman Cryptographic Provider", 13, 4026531840u, out pcboz);
		xugls = pcboz;
		if (nywxs == IntPtr.Zero && 0 == 0)
		{
			nywxs = wschq("Microsoft Base DSS and Diffie-Hellman Cryptographic Provider", 13, 4026531840u, out pcboz);
		}
		else
		{
			uhbab = true;
		}
		if (nywxs == IntPtr.Zero && 0 == 0)
		{
			if (p0 && 0 == 0)
			{
				throw iiyyx("DSA", pcboz);
			}
			return IntPtr.Zero;
		}
		return nywxs;
	}

	private static CryptographicException iiyyx(string p0, int p1)
	{
		string text = brgjd.edcru("Unable to acquire {0} CSP context", p0);
		text = ((p1 != -2146893795) ? (text + brgjd.edcru(" (0x{0:X8}).", p1)) : (text + brgjd.edcru(". The provider DLL file could not be loaded or failed to initialize.")));
		return new CryptographicException(text);
	}

	public static IntPtr wschq(string p0, int p1, uint p2, out int p3)
	{
		IntPtr p4;
		int num = pothu.qfori(out p4, null, p0, p1, p2);
		if (num == 0 || 1 == 0)
		{
			p3 = Marshal.GetLastWin32Error();
			if (p3 == -2146893802 && (p2 == 0 || 1 == 0))
			{
				num = pothu.qfori(out p4, null, p0, p1, 8u);
				if (num == 0 || 1 == 0)
				{
					p3 = Marshal.GetLastWin32Error();
				}
			}
			if (num == 0 || 1 == 0)
			{
				return IntPtr.Zero;
			}
		}
		p3 = 0;
		return p4;
	}
}
