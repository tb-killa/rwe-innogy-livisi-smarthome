using System;

namespace onrkn;

internal class hzdap
{
	private readonly object jrrbr = new object();

	private readonly byte[] givlx;

	private int pdlwj;

	private int jnkef;

	public int llzpe => givlx.Length;

	public int tvcie => givlx.Length - jnkef;

	public int jgwnt => jnkef;

	public hzdap(int capacity)
	{
		givlx = new byte[capacity];
	}

	public void nujhp(byte[] p0, int p1, int p2)
	{
		lock (jrrbr)
		{
			if (p2 > tvcie)
			{
				throw new InvalidOperationException("Not enough space in the buffer.");
			}
			int num = (pdlwj + jnkef) % givlx.Length;
			jnkef += p2;
			int num2 = givlx.Length - num;
			if (p2 <= num2)
			{
				Array.Copy(p0, p1, givlx, num, p2);
				return;
			}
			Array.Copy(p0, p1, givlx, num, num2);
			Array.Copy(p0, p1 + num2, givlx, 0, p2 - num2);
		}
	}

	public int kknzz(byte[] p0, int p1, int p2)
	{
		lock (jrrbr)
		{
			p2 = Math.Min(p2, jnkef);
			if (p2 == 0 || 1 == 0)
			{
				return 0;
			}
			int num = givlx.Length - pdlwj;
			if (p2 <= num)
			{
				Array.Copy(givlx, pdlwj, p0, p1, p2);
				pdlwj = (pdlwj + p2) % givlx.Length;
			}
			else
			{
				Array.Copy(givlx, pdlwj, p0, p1, num);
				pdlwj = p2 - num;
				Array.Copy(givlx, 0, p0, p1 + num, pdlwj);
			}
			jnkef -= p2;
			return p2;
		}
	}
}
