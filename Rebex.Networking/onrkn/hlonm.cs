using System;

namespace onrkn;

internal sealed class hlonm : IDisposable
{
	private static readonly hlonm dtqrr = new hlonm(IntPtr.Zero);

	private IntPtr fqbxm;

	private int pnlnf;

	private bool mgxlg;

	public static hlonm lmiul => dtqrr;

	public bool maeke
	{
		get
		{
			hwisq();
			return fqbxm != IntPtr.Zero;
		}
	}

	public hlonm(IntPtr connection)
	{
		fqbxm = connection;
		mgxlg = false;
		pnlnf = 1;
	}

	public void Dispose()
	{
		if (!object.ReferenceEquals(dtqrr, this) && !mgxlg)
		{
			nysvy();
			GC.SuppressFinalize(this);
			mgxlg = true;
		}
	}

	public IntPtr rdbhp()
	{
		hwisq();
		return fqbxm;
	}

	public void sofpj()
	{
		hwisq();
		pnlnf = 0;
	}

	private void hwisq()
	{
		if (mgxlg && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
	}

	private void nysvy()
	{
		if (!(fqbxm == IntPtr.Zero))
		{
			pxwkt.ConnMgrReleaseConnection(fqbxm, pnlnf);
			fqbxm = IntPtr.Zero;
		}
	}

	~hlonm()
	{
		nysvy();
	}
}
