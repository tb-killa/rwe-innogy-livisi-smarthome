using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Rebex.Net;

namespace onrkn;

internal class tjlhp : IDisposable
{
	private class dharg
	{
		internal readonly ISocket covrd;

		internal readonly WeakReference hsoms;

		internal readonly Thread bdjbh;

		internal volatile bool unpcb;

		public dharg(kehni owner, string name)
		{
			covrd = owner.Socket;
			hsoms = new WeakReference(owner, trackResurrection: false);
			bdjbh = new Thread(yowzu);
			bdjbh.Name = name;
			bdjbh.IsBackground = true;
		}

		public void fsago()
		{
			bdjbh.Start();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool ykwzv()
		{
			if (!(hsoms.Target is kehni kehni2) || 1 == 0)
			{
				return false;
			}
			return kehni2.jcfcv();
		}

		private void yowzu()
		{
			while (!unpcb)
			{
				bool flag;
				try
				{
					flag = covrd.Poll(5000000, SocketSelectMode.SelectRead);
				}
				catch
				{
					flag = true;
					unpcb = true;
				}
				if (flag && 0 == 0)
				{
					if (!ykwzv() || 1 == 0)
					{
						break;
					}
				}
				else if (!hsoms.IsAlive || 1 == 0)
				{
					break;
				}
			}
		}
	}

	private readonly int xfelc;

	private readonly dharg jwmmy;

	public int drifr => xfelc;

	public tjlhp(kehni owner, string name)
	{
		jwmmy = new dharg(owner, name);
		xfelc = jwmmy.bdjbh.ManagedThreadId;
	}

	public void lefxl()
	{
		jwmmy.fsago();
	}

	~tjlhp()
	{
		pljgt(p0: false);
	}

	public void Dispose()
	{
		pljgt(p0: true);
		GC.SuppressFinalize(this);
	}

	private void pljgt(bool p0)
	{
		dharg dharg = jwmmy;
		if (dharg != null && 0 == 0)
		{
			dharg.unpcb = true;
		}
	}
}
