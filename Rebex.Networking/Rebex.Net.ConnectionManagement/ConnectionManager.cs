using System;
using onrkn;

namespace Rebex.Net.ConnectionManagement;

public sealed class ConnectionManager
{
	internal const int jdyuk = 120000;

	private static readonly ConnectionManager fjgeh = new ConnectionManager();

	private static readonly object oufxi = new object();

	private static readonly object eyhak = new object();

	private static arnic sjhpo = arnic.akeqs();

	internal static ConnectionManager zrozr => fjgeh;

	internal static arnic wdczk
	{
		get
		{
			lock (oufxi)
			{
				return sjhpo;
			}
		}
	}

	private ConnectionManager()
	{
	}

	public static bool IsConnected()
	{
		lock (oufxi)
		{
			return sjhpo.zzmyt;
		}
	}

	public static bool TryConnect()
	{
		return TryConnect(TimeSpan.FromMilliseconds(120000.0));
	}

	public static bool TryConnect(TimeSpan timeout)
	{
		lock (oufxi)
		{
			sjhpo.Dispose();
			sjhpo = fjgeh.ocyqz(timeout);
			return sjhpo.zzmyt;
		}
	}

	internal bool njldp()
	{
		lock (eyhak)
		{
			try
			{
				pxwkt.vxvhw lpdwFlags = pxwkt.vxvhw.vtpxr;
				bool flag = pxwkt.InternetGetConnectedState(out lpdwFlags, 0);
				brgjd.edcru("IsInternetAvailable: result {0} flags {1}", flag, lpdwFlags);
				return flag;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	internal arnic ocyqz(TimeSpan p0)
	{
		lock (eyhak)
		{
			return keffa(pxwkt.yhvjh, p0);
		}
	}

	internal arnic jppbw()
	{
		return ocyqz(TimeSpan.FromMilliseconds(120000.0));
	}

	internal arnic yayee(Uri p0)
	{
		return azoes(p0, TimeSpan.FromMilliseconds(120000.0));
	}

	internal arnic lchkg(string p0)
	{
		return jmexk(p0, TimeSpan.FromMilliseconds(120000.0));
	}

	internal arnic azoes(Uri p0, TimeSpan p1)
	{
		if (p0 == null && 0 == 0)
		{
			throw new ArgumentNullException("uri");
		}
		return lchkg(p0.OriginalString);
	}

	internal arnic jmexk(string p0, TimeSpan p1)
	{
		lock (eyhak)
		{
			if (string.IsNullOrEmpty(p0) && 0 == 0)
			{
				throw new ArgumentException("uri");
			}
			try
			{
				Guid ppGuid = Guid.Empty;
				int p2 = pxwkt.ConnMgrMapURL(p0, ref ppGuid, 0);
				return (pxwkt.awbrj(p2) ? true : false) ? arnic.akeqs() : keffa(ppGuid, p1);
			}
			catch (Exception)
			{
				return arnic.akeqs();
			}
		}
	}

	private arnic keffa(Guid p0, TimeSpan p1)
	{
		hlonm hlonm = new hlonm(IntPtr.Zero);
		IntPtr phConnection = IntPtr.Zero;
		try
		{
			utewr utewr = new utewr(p0);
			pxwkt.vyqyo pConnInfo = utewr.lkkmg();
			pxwkt.wfvjt pdwStatus = pxwkt.wfvjt.rzrie;
			int num = pxwkt.ConnMgrEstablishConnectionSync(ref pConnInfo, ref phConnection, Convert.ToUInt32(p1.Milliseconds), ref pdwStatus);
			brgjd.edcru("EstablishConnectionResult: {0} - {1}", num, pdwStatus);
			if (phConnection != IntPtr.Zero && 0 == 0)
			{
				hlonm = new hlonm(phConnection);
				if (pdwStatus == pxwkt.wfvjt.viqkc)
				{
					return arnic.upzmu(hlonm);
				}
			}
			hlonm.Dispose();
			return arnic.akeqs();
		}
		catch (Exception)
		{
			if (hlonm.maeke && 0 == 0)
			{
				hlonm.Dispose();
			}
			return arnic.akeqs();
		}
	}
}
