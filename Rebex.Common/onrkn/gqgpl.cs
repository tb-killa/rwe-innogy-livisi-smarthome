using System.Security.Cryptography;

namespace onrkn;

internal abstract class gqgpl : lnabj
{
	protected class thinf : lnabj
	{
		private readonly gqgpl zyaqg;

		private rmkkr xcsdf;

		private bool hojbj;

		public void zkxnk(rmkkr p0, bool p1, int p2)
		{
			if (p0 != xcsdf)
			{
				throw new CryptographicException("Encountered ASN.1 node with mismatched type.");
			}
			hojbj = p1;
		}

		public thinf(gqgpl outer, rmkkr type)
		{
			xcsdf = type;
			zyaqg = outer;
		}

		public lnabj qaqes(rmkkr p0, bool p1, int p2)
		{
			if (!hojbj || 1 == 0)
			{
				throw new CryptographicException("Encountered constructed ASN.1 node within primitive node.");
			}
			if (xcsdf != p0)
			{
				throw new CryptographicException("Encountered constructed ASN.1 node with mismatched type.");
			}
			return new thinf(zyaqg, xcsdf);
		}

		public void lnxah(byte[] p0, int p1, int p2)
		{
			zyaqg.lnxah(p0, p1, p2);
		}

		public void somzq()
		{
		}

		private void rzdhk(fxakl p0)
		{
		}

		void lnabj.vlfdh(fxakl p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in rzdhk
			this.rzdhk(p0);
		}
	}

	private rmkkr uxuhb;

	private bool peota;

	public rmkkr ccmfu => uxuhb;

	public abstract int qstkb { get; }

	protected gqgpl(rmkkr type)
	{
		uxuhb = type;
	}

	public virtual void zkxnk(rmkkr p0, bool p1, int p2)
	{
		uxuhb = p0;
		peota = p1;
	}

	public virtual lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		if (!peota || 1 == 0)
		{
			return null;
		}
		if (uxuhb != p0)
		{
			throw new CryptographicException("Encountered inner ASN.1 node with mismatched type.");
		}
		return new thinf(this, uxuhb);
	}

	public abstract void lnxah(byte[] p0, int p1, int p2);

	public abstract void somzq();

	public abstract void vlfdh(fxakl p0);

	public int ddoam()
	{
		int num = qstkb;
		if (num < 128)
		{
			return num + 2;
		}
		if (num < 256)
		{
			return num + 3;
		}
		if (num < 65536)
		{
			return num + 4;
		}
		if (num < 16777216)
		{
			return num + 5;
		}
		return num + 6;
	}
}
