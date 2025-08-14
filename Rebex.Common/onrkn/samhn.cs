using System;
using System.Runtime.InteropServices;
using System.Text;

namespace onrkn;

internal class samhn : IDisposable
{
	private int nvgrx;

	private IntPtr mrqnd;

	private bool pvoik;

	public int enukg => nvgrx;

	public samhn(int length)
	{
		pvoik = true;
		nvgrx = length;
		mrqnd = Marshal.AllocHGlobal(length);
	}

	public IntPtr inyna()
	{
		return mrqnd;
	}

	public void yyxwj(int p0, byte p1)
	{
		Marshal.WriteByte(mrqnd, p0, p1);
	}

	public byte viuzs(int p0)
	{
		return Marshal.ReadByte(mrqnd, p0);
	}

	internal static void ixapi(IntPtr p0, int p1, long p2)
	{
		Marshal.WriteInt64(p0, p1, p2);
	}

	internal static long wabvn(IntPtr p0, int p1)
	{
		return Marshal.ReadInt64(p0, p1);
	}

	public void akvob(int p0, long p1)
	{
		Marshal.WriteInt64(mrqnd, p0, p1);
	}

	public long dohcs(int p0)
	{
		return Marshal.ReadInt64(mrqnd, p0);
	}

	public void fpzdi(int p0, int p1)
	{
		Marshal.WriteInt32(mrqnd, p0, p1);
	}

	public int sqmvb(int p0)
	{
		return Marshal.ReadInt32(mrqnd, p0);
	}

	public void ftbfw(int p0, short p1)
	{
		Marshal.WriteInt16(mrqnd, p0, p1);
	}

	public short zfddh(int p0)
	{
		return Marshal.ReadInt16(mrqnd, p0);
	}

	internal static void zjauo(IntPtr p0, int p1, IntPtr p2)
	{
		if (IntPtr.Size == 8)
		{
			ixapi(p0, p1, p2.ToInt64());
		}
		else
		{
			Marshal.WriteInt32(p0, p1, p2.ToInt32());
		}
	}

	internal static IntPtr yjjps(IntPtr p0, int p1)
	{
		if (IntPtr.Size == 8)
		{
			return new IntPtr(wabvn(p0, p1));
		}
		return new IntPtr(Marshal.ReadInt32(p0, p1));
	}

	public void qurik(int p0, IntPtr p1)
	{
		zjauo(mrqnd, p0, p1);
	}

	public IntPtr weojx(int p0)
	{
		return yjjps(mrqnd, p0);
	}

	internal byte[] uocau(int p0)
	{
		int num = Marshal.ReadInt32(mrqnd, p0);
		byte[] array = new byte[num];
		if (num > 0)
		{
			IntPtr source = weojx(p0 + IntPtr.Size);
			Marshal.Copy(source, array, 0, num);
		}
		return array;
	}

	public static string dztsn(IntPtr p0)
	{
		int i;
		for (i = 0; Marshal.ReadByte(p0, i) != 0; i++)
		{
		}
		return blzsy(p0, i);
	}

	public static string blzsy(IntPtr p0, int p1)
	{
		byte[] array = new byte[p1];
		Marshal.Copy(p0, array, 0, p1);
		return Encoding.Default.GetString(array, 0, p1);
	}

	internal void zqmse(byte[] p0, int p1, int p2, int p3)
	{
		IntPtr destination = new IntPtr(mrqnd.ToInt64() + p3);
		Marshal.Copy(p0, p1, destination, p2);
	}

	internal void wdcfj(int p0, int p1, byte[] p2, int p3)
	{
		IntPtr source = new IntPtr(mrqnd.ToInt64() + p0);
		Marshal.Copy(source, p2, p3, p1);
	}

	public static samhn ojtyh(byte[] p0)
	{
		samhn samhn2 = new samhn(p0.Length);
		samhn2.zqmse(p0, 0, p0.Length, 0);
		return samhn2;
	}

	public static samhn yccdb(string p0)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(p0);
		samhn samhn2 = new samhn(bytes.Length + 2);
		samhn2.zqmse(bytes, 0, bytes.Length, 0);
		samhn2.ftbfw(bytes.Length, 0);
		return samhn2;
	}

	public byte[] hsdhr()
	{
		byte[] array = new byte[nvgrx];
		Marshal.Copy(mrqnd, array, 0, nvgrx);
		return array;
	}

	public byte[] zsxzz(int p0, int p1)
	{
		if (p0 < 0 || p0 >= nvgrx)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (p0 + p1 > nvgrx)
		{
			throw new ArgumentOutOfRangeException("len");
		}
		byte[] array = new byte[p1];
		wdcfj(p0, p1, array, 0);
		return array;
	}

	public void Dispose()
	{
		tstpp(p0: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void tstpp(bool p0)
	{
		if (mrqnd != IntPtr.Zero && 0 == 0)
		{
			if (pvoik && 0 == 0)
			{
				Marshal.FreeHGlobal(mrqnd);
			}
			mrqnd = IntPtr.Zero;
		}
	}

	~samhn()
	{
		tstpp(p0: false);
	}
}
