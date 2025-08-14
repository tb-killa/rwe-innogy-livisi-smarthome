using System;
using System.Collections.Generic;
using System.Threading;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class awxwg
{
	private class kzuzz : IDisposable
	{
		private ixwxt rvdoc;

		private int gsfba;

		private int yhanb;

		public int ybfsg
		{
			get
			{
				return yhanb;
			}
			private set
			{
				yhanb = value;
			}
		}

		public kzuzz(ixwxt client)
		{
			rvdoc = client;
			gsfba = Environment.TickCount;
			ybfsg = client.hxrxh;
		}

		public void Dispose()
		{
			if (rvdoc != null && 0 == 0)
			{
				rvdoc.Dispose();
				rvdoc = null;
			}
		}

		public ixwxt lsphp(int p0)
		{
			if (rvdoc == null || 1 == 0)
			{
				return null;
			}
			int num = Environment.TickCount - gsfba;
			if (num > p0 || !rvdoc.wugxx() || 1 == 0)
			{
				rvdoc.Dispose();
				rvdoc = null;
			}
			return rvdoc;
		}
	}

	private class eegxl : Queue<kzuzz>
	{
		private sbbtr mxahd;

		private string ceojk;

		private bool otanr;

		public sbbtr byrrb
		{
			get
			{
				return mxahd;
			}
			private set
			{
				mxahd = value;
			}
		}

		public string xvusj
		{
			get
			{
				return ceojk;
			}
			private set
			{
				ceojk = value;
			}
		}

		public bool qlmiv
		{
			get
			{
				return otanr;
			}
			set
			{
				otanr = value;
			}
		}

		public eegxl(sbbtr owner, string key)
		{
			byrrb = owner;
			xvusj = key;
		}
	}

	private class sbbtr : Dictionary<string, eegxl>
	{
		private ISocketFactory lvabr;

		private bool hhmik;

		public ISocketFactory pjucv
		{
			get
			{
				return lvabr;
			}
			private set
			{
				lvabr = value;
			}
		}

		public bool myxza
		{
			get
			{
				return hhmik;
			}
			set
			{
				hhmik = value;
			}
		}

		public sbbtr(ISocketFactory key)
		{
			pjucv = key;
		}
	}

	private readonly object dhmaq;

	private readonly object vnogy;

	private readonly HttpRequestCreator nohdz;

	private Dictionary<ISocketFactory, sbbtr> sckjy;

	private LinkedList<eegxl> ktojr;

	private int axymp;

	public awxwg(HttpRequestCreator owner)
	{
		nohdz = owner;
		dhmaq = new object();
		vnogy = new object();
		sckjy = new Dictionary<ISocketFactory, sbbtr>();
		ktojr = new LinkedList<eegxl>();
	}

	public ixwxt gsuol(ISocketFactory p0, string p1)
	{
		if (p1 != null && 0 == 0)
		{
			sbbtr value;
			lock (dhmaq)
			{
				if (!sckjy.TryGetValue(p0, out value) || 1 == 0)
				{
					return null;
				}
			}
			eegxl value2;
			lock (value)
			{
				if (!value.TryGetValue(p1, out value2) || 1 == 0)
				{
					return null;
				}
			}
			lock (value2)
			{
				while (value2.Count > 0)
				{
					kzuzz kzuzz = value2.Dequeue();
					ixwxt ixwxt2 = kzuzz.lsphp(nohdz.Settings.HttpSessionCacheTimeout);
					if (ixwxt2 != null && 0 == 0)
					{
						return ixwxt2;
					}
					nohdz.tdcir(LogLevel.Debug, "HTTP", "Removed HTTP session ({0}) from cache, because it is expired.", kzuzz.ybfsg);
				}
			}
		}
		return null;
	}

	public bool wpuyo(ISocketFactory p0, string p1, ixwxt p2)
	{
		Func<bool> func = null;
		if (p1 == null || 1 == 0)
		{
			return false;
		}
		while (true)
		{
			sbbtr sbbtr;
			lock (dhmaq)
			{
				sbbtr = izybj(p0);
			}
			eegxl eegxl;
			lock (sbbtr)
			{
				if (sbbtr.myxza && 0 == 0)
				{
					Thread.Sleep(1);
					continue;
				}
				eegxl = virzj(sbbtr, p1);
			}
			lock (eegxl)
			{
				if (eegxl.qlmiv && 0 == 0)
				{
					Thread.Sleep(1);
					continue;
				}
				eegxl.Enqueue(new kzuzz(p2));
				awngk xrpuh = nohdz.xrpuh;
				if (func == null || 1 == 0)
				{
					func = pbmdm;
				}
				p2.poazj(xrpuh, func);
				p2.sesmx(null);
			}
			break;
		}
		nohdz.tdcir(LogLevel.Debug, "HTTP", "Cached HTTP session ({0}).", p2.hxrxh);
		rwbzh();
		return true;
	}

	private sbbtr izybj(ISocketFactory p0)
	{
		if (!sckjy.TryGetValue(p0, out var value) || 1 == 0)
		{
			value = new sbbtr(p0);
			sckjy.Add(p0, value);
		}
		return value;
	}

	private eegxl virzj(sbbtr p0, string p1)
	{
		if (!p0.TryGetValue(p1, out var value) || 1 == 0)
		{
			lock (vnogy)
			{
				value = new eegxl(p0, p1);
				p0.Add(p1, value);
				ktojr.AddLast(value);
			}
		}
		return value;
	}

	private void rwbzh()
	{
		Action<Exception> action = null;
		if (Interlocked.CompareExchange(ref axymp, 1, 0) == 0 || 1 == 0)
		{
			int elmdq = nohdz.Settings.elmdq;
			int hvyjf = nohdz.Settings.hvyjf;
			int num = Math.Max(elmdq, Math.Min(hvyjf, nohdz.Settings.HttpSessionCacheTimeout));
			nohdz.tdcir(LogLevel.Debug, "HTTP", "Clearing of HTTP session cache scheduled ({0} ms).", num);
			Action p = dazey;
			if (action == null || 1 == 0)
			{
				action = rilve;
			}
			dahxy.nqapv(p, action, num);
		}
	}

	private void dazey()
	{
		try
		{
			nohdz.nigxk(LogLevel.Debug, "HTTP", "Clearing of HTTP session cache started.");
			lgqsk();
		}
		catch (Exception ex)
		{
			nohdz.tdcir(LogLevel.Error, "HTTP", "Exception occurred while clearing HTTP session cache. {0}", ex);
			yybwt();
		}
		finally
		{
			Interlocked.CompareExchange(ref axymp, 0, 1);
			bool flag;
			lock (vnogy)
			{
				flag = ktojr.Count == 0;
			}
			if (flag && 0 == 0)
			{
				nohdz.nigxk(LogLevel.Debug, "HTTP", "HTTP session cache is empty.");
			}
			else
			{
				rwbzh();
			}
		}
	}

	private void lgqsk()
	{
		int num = 0;
		LinkedListNode<eegxl> linkedListNode;
		lock (vnogy)
		{
			linkedListNode = ktojr.Last;
		}
		while (linkedListNode != null)
		{
			eegxl value = linkedListNode.Value;
			LinkedListNode<eegxl> node = linkedListNode;
			linkedListNode = linkedListNode.Previous;
			lock (value)
			{
				while (value.Count > 0 && value.Peek().lsphp(nohdz.Settings.HttpSessionCacheTimeout) == null)
				{
					value.Dequeue();
					num++;
				}
				if (value.Count == 0 || 1 == 0)
				{
					value.qlmiv = true;
				}
			}
			if (!value.qlmiv)
			{
				continue;
			}
			sbbtr byrrb = value.byrrb;
			lock (byrrb)
			{
				lock (vnogy)
				{
					byrrb.Remove(value.xvusj);
					ktojr.Remove(node);
				}
				if (byrrb.Count == 0 || 1 == 0)
				{
					byrrb.myxza = true;
				}
			}
			if (byrrb.myxza && 0 == 0)
			{
				lock (dhmaq)
				{
					sckjy.Remove(byrrb.pjucv);
				}
			}
		}
		nohdz.tdcir(LogLevel.Debug, "HTTP", "Discarded {0} HTTP session(s) from cache.", num);
	}

	public void yybwt()
	{
		try
		{
			Dictionary<ISocketFactory, sbbtr> dictionary = sckjy;
			LinkedList<eegxl> linkedList = ktojr;
			lock (dhmaq)
			{
				lock (vnogy)
				{
					sckjy = new Dictionary<ISocketFactory, sbbtr>();
					ktojr = new LinkedList<eegxl>();
				}
			}
			using (LinkedList<eegxl>.Enumerator enumerator = linkedList.GetEnumerator())
			{
				while (enumerator.MoveNext() ? true : false)
				{
					eegxl current = enumerator.Current;
					lock (current)
					{
						using (Queue<kzuzz>.Enumerator enumerator2 = current.GetEnumerator())
						{
							while (enumerator2.MoveNext() ? true : false)
							{
								kzuzz current2 = enumerator2.Current;
								current2.Dispose();
							}
						}
						current.qlmiv = true;
						current.Clear();
					}
				}
			}
			linkedList.Clear();
			dictionary.Clear();
		}
		catch (Exception ex)
		{
			nohdz.tdcir(LogLevel.Error, "HTTP", "Exception occurred while discarding HTTP session cache. {0}", ex);
		}
	}

	private bool pbmdm()
	{
		return nohdz.xrpuh.ngqry;
	}

	private void rilve(Exception p0)
	{
		nohdz.tdcir(LogLevel.Error, "HTTP", "Exception occurred while scheduling HTTP session cache clearing. {0}", p0);
	}
}
