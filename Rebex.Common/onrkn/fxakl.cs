using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace onrkn;

internal class fxakl
{
	private Stream unzzh;

	private int rfcjv;

	private int eerjx;

	public fxakl(Stream output)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		unzzh = output;
		eerjx = -1;
	}

	public void imfsc()
	{
		unzzh = null;
	}

	public void vxuxe(int p0)
	{
		if (p0 < 128)
		{
			unzzh.WriteByte((byte)p0);
			return;
		}
		byte[] bytes = BitConverter.GetBytes(p0);
		Array.Reverse(bytes, 0, bytes.Length);
		int num = 0;
		if (num != 0)
		{
			goto IL_002c;
		}
		goto IL_003d;
		IL_003d:
		if (num < 4)
		{
			goto IL_002c;
		}
		goto IL_0041;
		IL_002c:
		if (bytes[num] == 0 || 1 == 0)
		{
			num++;
			goto IL_003d;
		}
		goto IL_0041;
		IL_0041:
		unzzh.WriteByte((byte)(132 - num));
		unzzh.Write(bytes, num, 4 - num);
	}

	public void afwyb()
	{
		if (rfcjv >= 0)
		{
			rfcjv++;
		}
	}

	public void xljze()
	{
		if (rfcjv >= 0)
		{
			if (rfcjv == 0 || 1 == 0)
			{
				throw new CryptographicException("Unable to end distinguished encoding that was not started.");
			}
			rfcjv--;
		}
	}

	private static IEnumerable bmzfk(IEnumerable p0)
	{
		SortedList<nnzwd, object> sortedList = new SortedList<nnzwd, object>();
		IEnumerator enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				lnabj p1 = (lnabj)enumerator.Current;
				byte[] data = kncuz(p1);
				sortedList.Add(new nnzwd(data), null);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		return sortedList.Keys;
	}

	public void qjrka(IEnumerable<lnabj> p0)
	{
		aiflg(rmkkr.osptv, p0);
	}

	public void suudj(params lnabj[] p0)
	{
		aiflg(rmkkr.osptv, p0);
	}

	public void aiflg(rmkkr p0, IEnumerable p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("nodes");
		}
		byte value = ((eerjx < 0) ? ((byte)((rmkkr)32 | p0)) : ((byte)(160 + eerjx)));
		eerjx = -1;
		if (rfcjv != 0 && 0 == 0)
		{
			if (p0 == rmkkr.wguaf)
			{
				p1 = bmzfk(p1);
			}
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				fxakl fxakl2 = new fxakl(memoryStream);
				fxakl2.rfcjv = -1;
				IEnumerator enumerator = p1.GetEnumerator();
				try
				{
					while (enumerator.MoveNext() ? true : false)
					{
						lnabj lnabj2 = (lnabj)enumerator.Current;
						lnabj2.vlfdh(fxakl2);
					}
				}
				finally
				{
					if (enumerator is IDisposable disposable && 0 == 0)
					{
						disposable.Dispose();
					}
				}
				fxakl2.imfsc();
				unzzh.WriteByte(value);
				vxuxe((int)memoryStream.Length);
				unzzh.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				return;
			}
			finally
			{
				if (memoryStream != null && 0 == 0)
				{
					((IDisposable)memoryStream).Dispose();
				}
			}
		}
		unzzh.WriteByte(value);
		unzzh.WriteByte(128);
		IEnumerator enumerator2 = p1.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext() ? true : false)
			{
				lnabj lnabj3 = (lnabj)enumerator2.Current;
				lnabj3.vlfdh(this);
			}
		}
		finally
		{
			if (enumerator2 is IDisposable disposable2 && 0 == 0)
			{
				disposable2.Dispose();
			}
		}
		unzzh.WriteByte(0);
		unzzh.WriteByte(0);
	}

	public void kfyej(lnabj p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("node");
		}
		p0.vlfdh(this);
	}

	public void uuhqt(int p0, lnabj p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("node");
		}
		if (eerjx != -1)
		{
			throw new CryptographicException("Double-tagged implicit node encountered.");
		}
		eerjx = p0;
		p1.vlfdh(this);
		eerjx = -1;
	}

	public void xadip(int p0, lnabj p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("node");
		}
		if (eerjx != -1)
		{
			throw new CryptographicException("Double-tagged explicit node encountered.");
		}
		if (p1 is gqgpl gqgpl2 && 0 == 0)
		{
			unzzh.WriteByte((byte)(160 + p0));
			vxuxe(gqgpl2.ddoam());
			gqgpl2.vlfdh(this);
		}
		else
		{
			eerjx = p0;
			aiflg(rmkkr.osptv, new lnabj[1] { p1 });
		}
	}

	public void enfzf(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		if (eerjx != -1)
		{
			int num = p0[0];
			num &= 0x20;
			num |= 0x80;
			num += eerjx;
			unzzh.WriteByte((byte)num);
			unzzh.Write(p0, 1, p0.Length - 1);
			eerjx = -1;
		}
		else
		{
			unzzh.Write(p0, 0, p0.Length);
		}
	}

	public void loggt(rmkkr p0, byte[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		int num = (int)p0;
		if (eerjx != -1)
		{
			num &= 0x20;
			num |= 0x80;
			num += eerjx;
			eerjx = -1;
		}
		unzzh.WriteByte((byte)num);
		vxuxe(p1.Length);
		unzzh.Write(p1, 0, p1.Length);
	}

	public void pihkg(rmkkr p0, opjbe p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		int num = (int)p0;
		if (eerjx != -1)
		{
			num &= 0x20;
			num |= 0x80;
			num += eerjx;
			eerjx = -1;
		}
		unzzh.WriteByte((byte)num);
		vxuxe((int)p1.Length);
		p1.njguo(unzzh);
	}

	public static byte[] kncuz(lnabj p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("node");
		}
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			fxakl fxakl2 = new fxakl(memoryStream);
			fxakl2.rfcjv = -1;
			p0.vlfdh(fxakl2);
			fxakl2.imfsc();
			return memoryStream.ToArray();
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}
}
