using System;
using System.Collections.Generic;
using System.IO;

namespace onrkn;

internal abstract class jxtqv : IDisposable
{
	internal const long rdnok = -2226271756974174256L;

	internal const int zmiiz = -1;

	internal const int czwbd = -2;

	internal const int boedh = -3;

	internal const int sguyd = -4;

	internal const char ytqes = '/';

	private bool pdfgi;

	internal abstract Stream kzier { get; }

	internal abstract xxolr ulhnp { get; }

	internal abstract Dictionary<string, jnkze> vsjhd { get; }

	internal abstract jnkze hsold { get; }

	internal abstract List<int> vtwew { get; }

	internal abstract List<int> iqsju { get; }

	internal abstract List<int> jhtad { get; }

	internal abstract long bogao { get; }

	internal abstract int xgzqc { get; }

	internal abstract int yoagm { get; }

	internal abstract int nrrwk { get; }

	internal abstract int mhpfn { get; }

	internal abstract int ngrnv { get; }

	internal abstract int imrfe { get; }

	internal abstract int ngjzc { get; set; }

	public jnkze this[string path] => vmjtu(path);

	protected jxtqv(string filePath)
	{
		if (filePath == null || 1 == 0)
		{
			throw new ArgumentNullException("filePath", "Path cannot be null.");
		}
		pdfgi = true;
	}

	protected jxtqv(Stream fileStream, bool leaveOpen)
	{
		if (fileStream == null || 1 == 0)
		{
			throw new ArgumentNullException("fileStream", "Stream cannot be null.");
		}
		if (!fileStream.CanSeek || 1 == 0)
		{
			throw new NotSupportedException("Only seekable streams are supported.");
		}
		pdfgi = !leaveOpen;
	}

	public xxolr xbrtt(string p0)
	{
		jnkze jnkze2 = rjhpb(p0, p1: true);
		return jnkze2.mzhde();
	}

	public jnkze[] gqvpk(string p0)
	{
		jnkze jnkze2 = rjhpb(p0, p1: true);
		return jnkze2.hmtsk();
	}

	public jnkze vmjtu(string p0)
	{
		return rjhpb(p0, p1: false);
	}

	internal jnkze rjhpb(string p0, bool p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path", "Path cannot be null.");
		}
		p0 = ddkiu(p0);
		if (p0.Length == 0 || 1 == 0)
		{
			return hsold;
		}
		if (vsjhd.TryGetValue(p0, out var value) && 0 == 0)
		{
			return value;
		}
		if (p1 && 0 == 0)
		{
			throw new InvalidOperationException("Specified path not found.");
		}
		return null;
	}

	internal long bjbna(int p0)
	{
		return bogao + xgzqc + ((long)p0 << nrrwk);
	}

	internal long afnxt(int p0)
	{
		return (long)p0 << ngrnv;
	}

	internal void jcuza(int p0)
	{
		iqsju.Add(p0);
	}

	internal void lngir(int p0)
	{
		vtwew.Add(p0);
		if ((vtwew.Count & (yoagm - 1)) != 1)
		{
			return;
		}
		jhtad.Add(vtwew.Count);
		vtwew.Add(-3);
		if (jhtad.Count > 109 && (jhtad.Count - 109) % (yoagm - 1) == 1)
		{
			if (jhtad.Count == 110)
			{
				ngjzc = vtwew.Count;
			}
			vtwew.Add(-4);
		}
	}

	internal static string ddkiu(string p0)
	{
		return p0.Trim('/');
	}

	internal static string motnn(string p0, string p1)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			return p1;
		}
		if (p1.Length == 0 || 1 == 0)
		{
			return p0;
		}
		bool flag = p0[p0.Length - 1] == '/';
		bool flag2 = p1[0] == '/';
		if (flag2 && 0 == 0 && flag && 0 == 0)
		{
			return p0.TrimEnd('/') + p1;
		}
		if ((!flag2 || 1 == 0) && (!flag || 1 == 0))
		{
			return brgjd.edcru("{0}{1}{2}", p0, '/', p1);
		}
		return p0 + p1;
	}

	internal static string briaq(string p0)
	{
		int num = brgjd.hhssl(p0, '/');
		if (num < 0)
		{
			return p0;
		}
		return p0.Substring(num + 1);
	}

	internal static string wklvv(string p0)
	{
		int num = brgjd.hhssl(p0, '/');
		if (num < 0)
		{
			return string.Empty;
		}
		if (num == 0 || 1 == 0)
		{
			return p0[0].ToString();
		}
		return p0.Substring(0, num);
	}

	protected virtual void dzpke(bool p0)
	{
		if (p0 && 0 == 0 && pdfgi && 0 == 0)
		{
			kzier.Close();
		}
	}

	public void mvpfe()
	{
		dzpke(p0: true);
		GC.SuppressFinalize(this);
	}

	private void lwwmc()
	{
		mvpfe();
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in lwwmc
		this.lwwmc();
	}
}
