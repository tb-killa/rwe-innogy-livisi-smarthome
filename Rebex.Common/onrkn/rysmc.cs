using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class rysmc : IHashTransform, IDisposable
{
	private byte[] bkeqx;

	private int hlwwb;

	private ulong gqptc;

	private ulong nuond;

	public abstract int HashSize { get; }

	protected abstract int rbegz { get; }

	protected abstract int qvxcs { get; }

	protected abstract void ygxqo(byte[] p0, int p1);

	protected abstract byte[] tsqhg();

	protected rysmc()
	{
		bkeqx = new byte[rbegz];
	}

	public static rysmc miwhl(HashingAlgorithmId p0)
	{
		return p0 switch
		{
			HashingAlgorithmId.SHA224 => new qlmbz(fullSize: false), 
			HashingAlgorithmId.SHA256 => new qlmbz(fullSize: true), 
			HashingAlgorithmId.SHA384 => new wcixh(fullSize: false), 
			HashingAlgorithmId.SHA512 => new wcixh(fullSize: true), 
			_ => throw new CryptographicException("Unsupported hash algorithm."), 
		};
	}

	public void Process(byte[] buffer, int offset, int count)
	{
		ulong num = (ulong)((long)count << 3);
		nuond += num;
		if (nuond < num)
		{
			if (qvxcs == 8 || gqptc == ulong.MaxValue)
			{
				throw new InvalidOperationException("Data too long.");
			}
			gqptc++;
		}
		if (hlwwb > 0 && hlwwb + count >= rbegz)
		{
			int num2 = rbegz - hlwwb;
			Buffer.BlockCopy(buffer, offset, bkeqx, hlwwb, num2);
			offset += num2;
			count -= num2;
			ygxqo(bkeqx, 0);
			hlwwb = 0;
		}
		while (count > 0)
		{
			if (count >= rbegz)
			{
				ygxqo(buffer, offset);
				offset += rbegz;
				count -= rbegz;
			}
			else
			{
				Buffer.BlockCopy(buffer, offset, bkeqx, hlwwb, count);
				hlwwb += count;
				count = 0;
			}
		}
	}

	public byte[] GetHash()
	{
		bkeqx[hlwwb++] = 128;
		if (hlwwb > rbegz - qvxcs)
		{
			Array.Clear(bkeqx, hlwwb, rbegz - hlwwb);
			ygxqo(bkeqx, 0);
			hlwwb = 0;
		}
		Array.Clear(bkeqx, hlwwb, rbegz - qvxcs - hlwwb);
		if (qvxcs == 16)
		{
			if (gqptc == 0)
			{
				Array.Clear(bkeqx, rbegz - 16, 8);
			}
			else
			{
				jqzjf(bkeqx, rbegz - 16, gqptc);
			}
		}
		jqzjf(bkeqx, rbegz - 8, nuond);
		ygxqo(bkeqx, 0);
		return tsqhg();
	}

	internal void nkmjd(byte[] p0, int p1, uint p2)
	{
		p0[p1++] = (byte)(p2 >> 24);
		p0[p1++] = (byte)(p2 >> 16);
		p0[p1++] = (byte)(p2 >> 8);
		p0[p1] = (byte)p2;
	}

	internal void jqzjf(byte[] p0, int p1, ulong p2)
	{
		p0[p1++] = (byte)(p2 >> 56);
		p0[p1++] = (byte)(p2 >> 48);
		p0[p1++] = (byte)(p2 >> 40);
		p0[p1++] = (byte)(p2 >> 32);
		p0[p1++] = (byte)(p2 >> 24);
		p0[p1++] = (byte)(p2 >> 16);
		p0[p1++] = (byte)(p2 >> 8);
		p0[p1] = (byte)p2;
	}

	public virtual void Reset()
	{
		hlwwb = 0;
		gqptc = 0uL;
		nuond = 0uL;
		Array.Clear(bkeqx, 0, bkeqx.Length);
	}

	public void Dispose()
	{
		Reset();
	}
}
