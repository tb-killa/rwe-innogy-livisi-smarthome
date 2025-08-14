using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Rebex;
using Rebex.Security.Cryptography;

namespace onrkn;

internal static class dahxy
{
	internal delegate T0 ijcls<T0>(string text);

	private sealed class lcntq
	{
		public Action zzrhk;

		public Action<Exception> fpjke;

		public void vspwo(object p0)
		{
			try
			{
				zzrhk();
			}
			catch (Exception obj)
			{
				if (fpjke != null && 0 == 0)
				{
					fpjke(obj);
				}
			}
		}
	}

	public const string lcacp = "**********";

	private const long vzugi = 4294967296L;

	private static readonly char[] ososs;

	private static readonly char[] rmzcm;

	public static readonly char[] awabp;

	public static readonly char[] ujgsn;

	public static readonly char[] klbuu;

	public static readonly char[] fmacz;

	private static Version vdiiz;

	public static readonly Version qvcir;

	private static readonly object nkqxg;

	private static uint mzhsp;

	private static long hptpu;

	public static CultureInfo ldled => CultureInfo.InvariantCulture;

	private static string xhysd => ".NET Compact Framework 3.5";

	public static string nxwxy => "\r\n";

	public static char ymzqe => Path.DirectorySeparatorChar;

	public static char nmnrp => Path.AltDirectorySeparatorChar;

	public static char njlzv => Path.VolumeSeparatorChar;

	public static int qmuio => Thread.CurrentThread.ManagedThreadId;

	public static bool ucaou => !xzevd;

	public static bool odwvq
	{
		get
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			if (oSVersion.Platform == PlatformID.Win32NT)
			{
				if (oSVersion.Version.Major <= 6)
				{
					if (oSVersion.Version.Major == 6)
					{
						return oSVersion.Version.Minor >= 2;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public static bool pcktj
	{
		get
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			if (oSVersion.Platform == PlatformID.Win32NT && oSVersion.Version.Major == 6)
			{
				return oSVersion.Version.Minor == 0;
			}
			return false;
		}
	}

	public static bool sbpoj => false;

	public static bool hzbtt => false;

	public static bool xzevd => false;

	public static bool umrab => false;

	public static bool spzfa => false;

	public static bool hdfhq => false;

	public static bool lehmf => false;

	public static bool gtgwr => false;

	public static bool yaxnv => true;

	public static bool itsqr
	{
		get
		{
			if (!ucaou || 1 == 0)
			{
				return umrab;
			}
			return true;
		}
	}

	public static bool uttbp
	{
		get
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			if (oSVersion.Platform != PlatformID.Win32NT)
			{
				return false;
			}
			Version version = oSVersion.Version;
			int major = version.Major;
			return major >= 6;
		}
	}

	public static bool kxxtc
	{
		get
		{
			Version version = Environment.Version;
			if (version.Major <= 3)
			{
				if (version.Major == 3)
				{
					return version.Minor >= 9;
				}
				return false;
			}
			return true;
		}
	}

	public static Version sqqyy
	{
		get
		{
			if (vdiiz == null && 0 == 0)
			{
				vdiiz = qvcir;
				try
				{
					vdiiz = ((object)typeof(int)).GetType().Assembly.GetName().Version;
				}
				catch
				{
				}
			}
			return vdiiz;
		}
	}

	internal static long ntngc
	{
		get
		{
			uint tickCount = (uint)Environment.TickCount;
			lock (nkqxg)
			{
				long num = ((tickCount < mzhsp) ? (4294967296L + (long)tickCount - mzhsp) : (tickCount - mzhsp));
				if (num > 0)
				{
					mzhsp = tickCount;
					hptpu += num;
				}
				return hptpu;
			}
		}
	}

	static dahxy()
	{
		klbuu = new char[2] { '/', '\\' };
		fmacz = new char[2] { '*', '?' };
		qvcir = new Version(0, 0, 0, 0);
		nkqxg = new object();
		List<char> list = new List<char>();
		if (ucaou && 0 == 0)
		{
			list.Add('"');
			list.Add('<');
			list.Add('>');
		}
		list.Add('|');
		int num = 0;
		if (num != 0)
		{
			goto IL_0088;
		}
		goto IL_0094;
		IL_0088:
		list.Add((char)num);
		num++;
		goto IL_0094;
		IL_0094:
		if (num >= 32)
		{
			List<char> list2 = new List<char>(list) { '/' };
			if (ucaou && 0 == 0)
			{
				list2.Add('\\');
				list2.Add(':');
				list2.Add('*');
				list2.Add('?');
			}
			ososs = list.ToArray();
			rmzcm = list2.ToArray();
			if (ymzqe == nmnrp)
			{
				awabp = new char[1] { ymzqe };
				ujgsn = new char[2] { ymzqe, njlzv };
			}
			else
			{
				awabp = new char[2] { ymzqe, nmnrp };
				ujgsn = new char[3] { ymzqe, nmnrp, njlzv };
			}
			hptpu = (mzhsp = (uint)Environment.TickCount);
			return;
		}
		goto IL_0088;
	}

	public static Regex khmpt(string p0, bool p1)
	{
		return ezktu(p0, p1, p2: true, p3: true);
	}

	public static Regex ezktu(string p0, bool p1, bool p2, bool p3)
	{
		RegexOptions regexOptions;
		if (p1 && 0 == 0)
		{
			regexOptions = RegexOptions.None;
			if (regexOptions == RegexOptions.None)
			{
				goto IL_0015;
			}
		}
		regexOptions = RegexOptions.IgnoreCase;
		goto IL_0015;
		IL_0015:
		p0 = Regex.Escape(p0);
		p0 = p0.Replace("\\*", ".*").Replace("\\?", ".");
		if (p2 && 0 == 0)
		{
			p0 = "^" + p0;
		}
		if (p3 && 0 == 0)
		{
			p0 += "$";
		}
		return new Regex(p0, regexOptions);
	}

	public static string tfhfm(string p0)
	{
		p0 = p0.TrimEnd(awabp);
		if (p0.Length == 0 || 1 == 0)
		{
			p0 = ymzqe.ToString();
		}
		return p0;
	}

	public static void gbxkl(object p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException(p1);
		}
	}

	public static void mydnv(string p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException(p1);
		}
		if (p0.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Hostname cannot be empty.", p1);
		}
	}

	public static void kykxy(int p0, string p1)
	{
		if (p0 >= 1 && p0 <= 65535)
		{
			return;
		}
		throw hifyx.nztrs(p1, p0, "Port is out of range of valid values.");
	}

	public static void lyxgi(ArraySegment<byte> p0, string p1)
	{
		if (p0.Array == null || 1 == 0)
		{
			throw new ArgumentException(p1);
		}
	}

	public static void dionp(byte[] p0, int p1, int p2)
	{
		valft(p0, "buffer", p1, "offset", p2, "count");
	}

	public static void valft(byte[] p0, string p1, int p2, string p3, int p4, string p5)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException(p1);
		}
		if (p2 < 0 || p2 > p0.Length)
		{
			throw hifyx.nztrs(p3, p2, "Buffer offset is out of range.");
		}
		if (p4 < 0)
		{
			throw hifyx.nztrs(p5, p4, "Count is negative.");
		}
		int num = p2 + p4;
		if (num >= 0 && num <= p0.Length)
		{
			return;
		}
		throw hifyx.nztrs(p5, p4, "Count is greater than the number of elements from index to the end of the array.");
	}

	public static Version onrln(Assembly p0)
	{
		if ((object)p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("assembly");
		}
		return p0.GetName().Version;
	}

	public static Version clyhv(Type p0)
	{
		if ((object)p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("type");
		}
		string assemblyQualifiedName = p0.AssemblyQualifiedName;
		int num = assemblyQualifiedName.IndexOf("ersion=");
		if (num < 0)
		{
			return new Version(0, 0);
		}
		num += 7;
		int num2 = assemblyQualifiedName.IndexOf(",", num);
		if (num < 0)
		{
			return new Version(0, 0);
		}
		string version = assemblyQualifiedName.Substring(num, num2 - num).Trim();
		return new Version(version);
	}

	public static Version kmpbw()
	{
		return clyhv(typeof(dahxy));
	}

	public static string hvexk()
	{
		Encoding encoding = EncodingTools.Default;
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		return brgjd.edcru("{0}; {1}", (currentCulture != null) ? currentCulture.TwoLetterISOLanguageName : "unknown", (encoding != null) ? encoding.WebName : "unknown");
	}

	internal static bool kfygb()
	{
		if (spzfa && 0 == 0)
		{
			return hzbtt;
		}
		return false;
	}

	public static string phtxx()
	{
		OperatingSystem oSVersion = Environment.OSVersion;
		bool flag = false;
		bool flag2 = false;
		string text;
		switch (oSVersion.Platform)
		{
		case PlatformID.Unix:
			text = ((oSVersion.Version.Major < 8) ? "Linux" : "macOS");
			break;
		case PlatformID.Win32NT:
			text = "Windows";
			flag = true;
			flag2 = true;
			if (flag2)
			{
				break;
			}
			goto case PlatformID.WinCE;
		case PlatformID.WinCE:
			text = "Windows CE";
			flag = true;
			if (flag)
			{
				break;
			}
			goto default;
		default:
			text = null;
			break;
		}
		if (text == null || 1 == 0)
		{
			text = "Unknown";
		}
		else
		{
			Version version = oSVersion.Version;
			string text2 = ((!flag) ? version.ToString() : brgjd.edcru("{0}.{1}.{2}", version.Major, version.Minor, version.Build));
			string text3 = "";
			if (flag2 && 0 == 0 && CryptoHelper.bwwly && 0 == 0)
			{
				text3 = " (FIPS-only)";
			}
			text = brgjd.edcru("{0} {1} {2}-bit{3}", text, text2, IntPtr.Size * 8, text3);
		}
		string text4 = Environment.Version.ToString();
		return brgjd.edcru("Platform: {0}; CLR: {1}", text, text4);
	}

	public static void kdslf()
	{
		string message = brgjd.edcru("This component has been compiled for {0}. Please use assemblies compiled for your platform.", xhysd);
		if (Environment.OSVersion.Platform != PlatformID.WinCE)
		{
			throw new NotSupportedException(message);
		}
		if (Environment.Version.Major >= 3 && (Environment.Version.Major != 3 || Environment.Version.Minor >= 5))
		{
			return;
		}
		throw new NotSupportedException("This component has been compiled for .NET Compact Framework 3.5 or higher, but you are trying to use it in .NET Compact Framework 2.0 or lower. Please use assemblies compiled for your platform.");
	}

	public static char[] mnelc()
	{
		return (char[])ososs.Clone();
	}

	public static char[] cexjz()
	{
		return (char[])rmzcm.Clone();
	}

	public static bool tmjpq(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		return p0.IndexOfAny(rmzcm) < 0;
	}

	public static string knaqv(string p0, string p1, char[] p2)
	{
		return ekgvz(p0, p1, p2, p3: false, p4: false);
	}

	public static string ekgvz(string p0, string p1, char[] p2, bool p3, bool p4)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("currentPath");
		}
		if (p1.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "currentPath");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("separators");
		}
		if (p2.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Array cannot be empty.", "separators");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			return p1;
		}
		if (Array.IndexOf(p2, p0[0]) >= 0)
		{
			return wysxc(p0, p2, p3, p4);
		}
		return wysxc(p1 + p2[0] + p0, p2, p3, p4);
	}

	public static string wysxc(string p0, char[] p1, bool p2, bool p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "path");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("separators");
		}
		if (p1.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Array cannot be empty.", "separators");
		}
		string text = p1[0].ToString();
		bool flag = Array.IndexOf(p1, p0[0]) >= 0;
		int length = p0.Length;
		p0 = p0.TrimEnd(p1);
		if (p0.Length == 0 || 1 == 0)
		{
			return text;
		}
		bool flag2 = p3 && 0 == 0 && p0.Length < length;
		int num = 0;
		List<string> list = new List<string>();
		string[] array = p0.Split(p1);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_00e6;
		}
		goto IL_01aa;
		IL_01db:
		int num3;
		if (num3 < num)
		{
			goto IL_01c7;
		}
		goto IL_01e1;
		IL_01a4:
		num2++;
		goto IL_01aa;
		IL_01e1:
		if ((list.Count != 0) ? true : false)
		{
			p0 = ((list.Count != 1) ? string.Join(text, list.ToArray()) : list[0]);
		}
		else
		{
			if (flag && 0 == 0)
			{
				return text;
			}
			p0 = ".";
		}
		if (flag && 0 == 0)
		{
			p0 = text + p0;
		}
		if (flag2 && 0 == 0)
		{
			return p0 + text;
		}
		return p0;
		IL_01c7:
		list.Insert(num3, "..");
		num3++;
		goto IL_01db;
		IL_00e6:
		string text2;
		if ((text2 = array[num2]) == null)
		{
			goto IL_0198;
		}
		if (!(text2 == "") || 1 == 0)
		{
			if (!(text2 == ".") || 1 == 0)
			{
				if (!(text2 == "..") || 1 == 0)
				{
					goto IL_0198;
				}
				if (!p2 || 1 == 0)
				{
					list.Add(array[num2]);
				}
				else if (list.Count > 0)
				{
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					num++;
				}
			}
			else if (!p2 || 1 == 0)
			{
				list.Add(array[num2]);
			}
		}
		goto IL_01a4;
		IL_0198:
		list.Add(array[num2]);
		goto IL_01a4;
		IL_01aa:
		if (num2 < array.Length)
		{
			goto IL_00e6;
		}
		if (!flag || 1 == 0)
		{
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_01c7;
			}
			goto IL_01db;
		}
		goto IL_01e1;
	}

	public static int? movxk(DateTime? p0, DateTime? p1, TimeComparisonGranularity p2)
	{
		if (!p0.HasValue || false || !p1.HasValue || 1 == 0)
		{
			return null;
		}
		DateTime dateTime = p0.Value;
		DateTime value = p1.Value;
		if (dateTime.Kind != DateTimeKind.Unspecified && 0 == 0 && value.Kind != DateTimeKind.Unspecified && 0 == 0 && dateTime.Kind != value.Kind)
		{
			dateTime = dateTime.ToUniversalTime();
			value = value.ToUniversalTime();
		}
		switch (p2)
		{
		case TimeComparisonGranularity.Seconds:
			dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
			value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
			break;
		case TimeComparisonGranularity.TwoSeconds:
			dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second & -2);
			value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second & -2);
			break;
		case TimeComparisonGranularity.Days:
			dateTime = dateTime.Date;
			value = value.Date;
			break;
		default:
			throw hifyx.nztrs("granularity", p2, "Invalid enum value.");
		case TimeComparisonGranularity.None:
			break;
		}
		return dateTime.CompareTo(value);
	}

	internal static bool feudq<T>(ijcls<T> p0, bool p1, string p2, out T p3)
	{
		p3 = default(T);
		if (p2 == null || 1 == 0)
		{
			return false;
		}
		int num;
		if (p1 && 0 == 0)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0023;
			}
			goto IL_004a;
		}
		goto IL_005f;
		IL_0023:
		if (p2[num] != '-' && (!char.IsDigit(p2[num]) || 1 == 0))
		{
			return false;
		}
		num++;
		goto IL_004a;
		IL_005f:
		try
		{
			p3 = p0(p2);
			return true;
		}
		catch (FormatException)
		{
		}
		catch (OverflowException)
		{
		}
		return false;
		IL_004a:
		if (num >= p2.Length)
		{
			goto IL_005f;
		}
		goto IL_0023;
	}

	public static bool crqjb(string p0, out int p1)
	{
		return feudq(int.Parse, p1: true, p0, out p1);
	}

	public static bool gusxd(string p0)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			return true;
		}
		if (p0.IndexOf('\0') >= 0)
		{
			return false;
		}
		if (p0.Split('/').Length > 2)
		{
			return false;
		}
		if (p0.Split('\\').Length > 2)
		{
			return false;
		}
		return true;
	}

	public static IDisposable irhnj(Action p0, int p1)
	{
		return nqapv(p0, null, p1);
	}

	public static IDisposable nqapv(Action p0, Action<Exception> p1, int p2)
	{
		lcntq lcntq = new lcntq();
		lcntq.zzrhk = p0;
		lcntq.fpjke = p1;
		TimerCallback p3 = lcntq.vspwo;
		return jhgqc.ghlqp(p3, null, p2);
	}

	public static T ziyhk<T>(params T[] p0) where T : IComparable
	{
		T result = p0[0];
		int num = 1;
		if (num == 0)
		{
			goto IL_000e;
		}
		goto IL_0036;
		IL_000e:
		object obj = p0[num];
		if (result.CompareTo(obj) > 0)
		{
			result = p0[num];
		}
		num++;
		goto IL_0036;
		IL_0036:
		if (num < p0.Length)
		{
			goto IL_000e;
		}
		return result;
	}

	public static DateTime qsrcs(DateTime p0)
	{
		if (p0.Kind == DateTimeKind.Utc)
		{
			return p0;
		}
		if (p0.Kind == DateTimeKind.Unspecified || 1 == 0)
		{
			return new DateTime(p0.Ticks, DateTimeKind.Utc);
		}
		return p0.ToUniversalTime();
	}

	public static void ynzte<T>(ref T p0, ref T p1)
	{
		T val = p0;
		p0 = p1;
		p1 = val;
	}

	public static bool wahav(object p0)
	{
		if (p0 is IDisposable disposable && 0 == 0)
		{
			disposable.Dispose();
			return true;
		}
		return false;
	}
}
