using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace onrkn;

internal class znuay : IDisposable
{
	private sealed class ztvrb
	{
		public znuay kiyha;

		private static Action<object> cooef;

		public tfsbt mmnxw(ddmlv p0)
		{
			if (cooef == null || 1 == 0)
			{
				cooef = gmwtf;
			}
			return p0.kjwdi(cooef, kiyha);
		}

		private static void gmwtf(object p0)
		{
			((znuay)p0).pvutk();
		}
	}

	private static readonly List<tfsbt> dbuoy = new List<tfsbt>();

	private List<tfsbt> mgysx;

	private volatile bool lxnsa;

	private bool fbubv;

	private IEnumerable<tfsbt> stxqw;

	public bool mpjbd => lxnsa;

	public ddmlv qncaj
	{
		get
		{
			pbuai();
			return new ddmlv(this);
		}
	}

	public znuay()
	{
		fbubv = false;
		stxqw = Enumerable.Empty<tfsbt>();
	}

	public void pvutk()
	{
		hjjcw(p0: false);
	}

	public void hjjcw(bool p0)
	{
		pbuai();
		lxnsa = true;
		ocpsb(p0);
	}

	public void Dispose()
	{
		if (fbubv)
		{
			return;
		}
		IEnumerable<tfsbt> enumerable = stxqw;
		stxqw = Enumerable.Empty<tfsbt>();
		IEnumerator<tfsbt> enumerator = enumerable.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				enumerator.Current.Dispose();
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		List<tfsbt> list = mgysx;
		if (list != null && 0 == 0 && (!object.ReferenceEquals(list, dbuoy) || 1 == 0))
		{
			lock (list)
			{
				list.Clear();
			}
		}
		fbubv = true;
	}

	public static znuay fwsky(params ddmlv[] p0)
	{
		ztvrb ztvrb = new ztvrb();
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tokens");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("tokens");
		}
		ztvrb.kiyha = new znuay();
		ztvrb.kiyha.stxqw = p0.Select(ztvrb.mmnxw).ToArray();
		return ztvrb.kiyha;
	}

	internal tfsbt rfcpx(Action p0)
	{
		return jqhui(p0, null, null);
	}

	internal tfsbt kycky(Action<object> p0, object p1)
	{
		return jqhui(null, p0, p1);
	}

	internal void chzfq(tfsbt p0)
	{
		List<tfsbt> list = Interlocked.CompareExchange(ref mgysx, null, null);
		if (list != null && 0 == 0 && (!object.ReferenceEquals(list, dbuoy) || 1 == 0))
		{
			lock (list)
			{
				list.Remove(p0);
			}
		}
	}

	private tfsbt jqhui(Action p0, Action<object> p1, object p2)
	{
		if (lxnsa && 0 == 0)
		{
			bfipn(p0, p1, p2);
			return default(tfsbt);
		}
		List<tfsbt> list = null;
		if (mgysx == null || 1 == 0)
		{
			Interlocked.CompareExchange(ref mgysx, new List<tfsbt>(), null);
		}
		list = Interlocked.CompareExchange(ref mgysx, null, null);
		if (object.ReferenceEquals(list, dbuoy) && 0 == 0)
		{
			bfipn(p0, p1, p2);
			return default(tfsbt);
		}
		tfsbt tfsbt2 = ((p0 != null) ? new tfsbt(p0, this) : new tfsbt(p1, p2, this));
		lock (list)
		{
			list.Add(tfsbt2);
		}
		if (lxnsa && 0 == 0)
		{
			bool flag = false;
			lock (list)
			{
				if (list.Contains(tfsbt2) && 0 == 0)
				{
					list.Remove(tfsbt2);
					flag = true;
				}
			}
			if (flag && 0 == 0)
			{
				tfsbt2.Dispose();
				bfipn(p0, p1, p2);
				return default(tfsbt);
			}
		}
		return tfsbt2;
	}

	private static void bfipn(Action p0, Action<object> p1, object p2)
	{
		if (p0 != null && 0 == 0)
		{
			p0();
		}
		else
		{
			p1(p2);
		}
	}

	private void ocpsb(bool p0)
	{
		List<tfsbt> list = mgysx;
		List<tfsbt> list2;
		do
		{
			list2 = list;
			list = Interlocked.CompareExchange(ref mgysx, dbuoy, list2);
		}
		while (!object.ReferenceEquals(list, list2));
		if (list2 == null || false || object.ReferenceEquals(list2, dbuoy))
		{
			return;
		}
		tfsbt[] array;
		lock (list2)
		{
			if (list2.Count == 0 || 1 == 0)
			{
				return;
			}
			array = list2.ToArray();
			list2.Clear();
		}
		List<Exception> list3 = null;
		tfsbt[] array2 = array;
		int num = 0;
		if (num != 0)
		{
			goto IL_0092;
		}
		goto IL_00dd;
		IL_0092:
		tfsbt tfsbt2 = array2[num];
		try
		{
			tfsbt2.ratek();
		}
		catch (Exception item)
		{
			if (p0 && 0 == 0)
			{
				throw;
			}
			List<Exception> list4 = list3;
			if (list4 == null || 1 == 0)
			{
				list4 = new List<Exception>();
			}
			list3 = list4;
			list3.Add(item);
		}
		num++;
		goto IL_00dd;
		IL_00dd:
		if (num >= array2.Length)
		{
			if (list3 != null && 0 == 0)
			{
				throw new nagsk(list3);
			}
			return;
		}
		goto IL_0092;
	}

	private void pbuai()
	{
		if (fbubv && 0 == 0)
		{
			throw new ObjectDisposedException("CancellationTokenSource");
		}
	}
}
