using System;

namespace onrkn;

internal sealed class arnic : IDisposable
{
	private static readonly arnic ercff = new arnic(hlonm.lmiul);

	private hlonm aqrau;

	private bool bmbub;

	public bool zzmyt
	{
		get
		{
			fscyi();
			if (ytwtd.maeke && 0 == 0)
			{
				return mmhvd();
			}
			return false;
		}
	}

	internal hlonm ytwtd
	{
		get
		{
			fscyi();
			return aqrau;
		}
		private set
		{
			fscyi();
			aqrau = value;
		}
	}

	private arnic(hlonm connection)
	{
		aqrau = connection;
		bmbub = false;
	}

	private bool mmhvd()
	{
		pxwkt.wfvjt status = pxwkt.wfvjt.rzrie;
		int p = pxwkt.ConnMgrConnectionStatus(ytwtd.rdbhp(), ref status);
		if (pxwkt.awbrj(p) && 0 == 0)
		{
			return false;
		}
		return status == pxwkt.wfvjt.viqkc;
	}

	public void Dispose()
	{
		if (!object.ReferenceEquals(ercff, this) && !bmbub)
		{
			ytwtd.Dispose();
			bmbub = true;
		}
	}

	internal void idjcm()
	{
		fscyi();
		aqrau.sofpj();
	}

	internal static arnic akeqs()
	{
		return ercff;
	}

	internal static arnic upzmu(hlonm p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("connection");
		}
		if (!p0.maeke || 1 == 0)
		{
			return akeqs();
		}
		return new arnic(p0);
	}

	private void fscyi()
	{
		if (bmbub && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
	}
}
