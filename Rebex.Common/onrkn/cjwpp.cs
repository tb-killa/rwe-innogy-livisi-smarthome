using System;
using System.Collections.Generic;
using System.Linq;

namespace onrkn;

internal class cjwpp : IDisposable
{
	private sealed class rwwfa
	{
		public Action tplzb;

		public bool tivox(pbpve p0)
		{
			return p0.jzbvv(tplzb);
		}
	}

	private const int wakjn = 10;

	private readonly List<pbpve> jfoao = new List<pbpve>(10);

	private readonly Queue<Action> gvsjl = new Queue<Action>();

	private int dtjth = 10;

	private int owfez;

	public int nbdns
	{
		get
		{
			return dtjth;
		}
		set
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (value < owfez)
			{
				owfez = value;
			}
			dtjth = value;
			fgqzz();
		}
	}

	public int zjflq
	{
		get
		{
			return owfez;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (value > dtjth)
			{
				dtjth = value;
			}
			owfez = value;
			fgqzz();
		}
	}

	public int nocze
	{
		get
		{
			lock (jfoao)
			{
				return gvsjl.Count;
			}
		}
	}

	public cjwpp()
	{
		fgqzz();
	}

	public void Dispose()
	{
		owfez = 0;
		dtjth = 0;
		fgqzz();
	}

	private void fgqzz()
	{
		lock (jfoao)
		{
			int num = owfez - jfoao.Count;
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0026;
			}
			goto IL_0046;
			IL_0026:
			jfoao.Add(new pbpve(jzrld));
			num2++;
			goto IL_0046;
			IL_0046:
			if (num2 < num)
			{
				goto IL_0026;
			}
			if (num > 0)
			{
				jzrld();
			}
			int num3 = jfoao.Count - 1;
			while (num3 >= 0 && jfoao.Count > dtjth)
			{
				pbpve pbpve2 = jfoao[num3];
				if (pbpve2.matht(p0: false) && 0 == 0)
				{
					jfoao.RemoveAt(num3);
				}
				num3--;
			}
			int num4 = 0;
			if (num4 != 0)
			{
				goto IL_00b0;
			}
			goto IL_00ed;
			IL_00b0:
			int index = jfoao.Count - 1 - num4;
			pbpve pbpve3 = jfoao[index];
			pbpve3.matht(p0: true);
			jfoao.RemoveAt(index);
			num4++;
			goto IL_00ed;
			IL_00ed:
			if (num4 >= jfoao.Count - dtjth)
			{
				return;
			}
			goto IL_00b0;
		}
	}

	private void jzrld()
	{
		lock (jfoao)
		{
			while (gvsjl.Count > 0)
			{
				rwwfa rwwfa = new rwwfa();
				rwwfa.tplzb = gvsjl.Peek();
				if (jfoao.Any(rwwfa.tivox) && 0 == 0)
				{
					gvsjl.Dequeue();
					continue;
				}
				if (jfoao.Count < dtjth)
				{
					pbpve pbpve2 = new pbpve(jzrld);
					if (!pbpve2.jzbvv(rwwfa.tplzb) || 1 == 0)
					{
						throw new InvalidOperationException();
					}
					jfoao.Add(pbpve2);
					gvsjl.Dequeue();
					continue;
				}
				break;
			}
		}
	}

	public void atxrq(Action p0)
	{
		lock (jfoao)
		{
			gvsjl.Enqueue(p0);
			jzrld();
		}
	}
}
