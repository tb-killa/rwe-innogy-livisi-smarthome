using System;
using System.Text;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class rbplv : DeriveBytes
{
	private IHashTransform wtxjh;

	private int bdtyr;

	private byte[] uxffv;

	private int yvtqn;

	private byte[] ggdym;

	private byte[] mojbq;

	private int yctlq;

	private int stvrw;

	private byte[] kzemw;

	public int fgvmb
	{
		get
		{
			return yvtqn;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			yvtqn = value;
			lofwm();
		}
	}

	public byte[] ofzng
	{
		get
		{
			return (byte[])ggdym.Clone();
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length < 8)
			{
				throw new ArgumentException("Not enough data.", "value");
			}
			ggdym = (byte[])value.Clone();
			lofwm();
		}
	}

	private static byte[] bspgs(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		return Encoding.UTF8.GetBytes(p0);
	}

	public rbplv(HashingAlgorithmId alg, string password, byte[] salt, int iterations)
		: this(alg, bspgs(password), salt, iterations)
	{
	}

	public rbplv(HashingAlgorithmId alg, byte[] password, byte[] salt, int iterations)
	{
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		wtxjh = new HashingAlgorithm(alg).CreateTransform();
		bdtyr = wtxjh.HashSize / 8;
		uxffv = password;
		ofzng = salt;
		fgvmb = iterations;
		lofwm();
	}

	private byte[] xhojm()
	{
		byte[] array = new byte[mojbq.Length + uxffv.Length + ggdym.Length];
		mojbq.CopyTo(array, 0);
		uxffv.CopyTo(array, mojbq.Length);
		ggdym.CopyTo(array, mojbq.Length + uxffv.Length);
		mojbq = array;
		int num = 0;
		if (num != 0)
		{
			goto IL_006e;
		}
		goto IL_00a8;
		IL_006e:
		wtxjh.Process(mojbq, 0, mojbq.Length);
		mojbq = wtxjh.GetHash();
		wtxjh.Reset();
		num++;
		goto IL_00a8;
		IL_00a8:
		if (num < yvtqn)
		{
			goto IL_006e;
		}
		return mojbq;
	}

	private void lofwm()
	{
		if (kzemw != null && 0 == 0)
		{
			Array.Clear(kzemw, 0, kzemw.Length);
		}
		kzemw = new byte[bdtyr];
		if (mojbq != null && 0 == 0)
		{
			Array.Clear(mojbq, 0, mojbq.Length);
		}
		mojbq = new byte[0];
		yctlq = 0;
		stvrw = 0;
	}

	public override void Reset()
	{
		lofwm();
	}

	public override byte[] GetBytes(int cb)
	{
		if (cb <= 0)
		{
			throw new ArgumentOutOfRangeException("cb");
		}
		byte[] array = new byte[cb];
		int num = stvrw - yctlq;
		int i = 0;
		if (num > 0)
		{
			if (cb < num)
			{
				Array.Copy(kzemw, yctlq, array, 0, cb);
				yctlq += cb;
				return array;
			}
			Array.Copy(kzemw, yctlq, array, 0, num);
			i += num;
			yctlq = 0;
			stvrw = 0;
		}
		for (; i < cb; i += bdtyr)
		{
			byte[] sourceArray = xhojm();
			num = cb - i;
			if (num <= bdtyr)
			{
				Array.Copy(sourceArray, 0, array, i, num);
				i += num;
				Array.Copy(sourceArray, num, kzemw, yctlq, bdtyr - num);
				stvrw = stvrw + bdtyr - num;
				return array;
			}
			Array.Copy(sourceArray, 0, array, i, bdtyr);
		}
		return array;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && 0 == 0 && wtxjh != null && 0 == 0)
		{
			wtxjh.Dispose();
			wtxjh = null;
		}
	}
}
