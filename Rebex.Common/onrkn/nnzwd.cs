using System;
using System.IO;

namespace onrkn;

internal class nnzwd : ltutw, lnabj, IComparable<nnzwd>
{
	private class rackf : ltutw, lnabj
	{
		private readonly nnzwd rfsav;

		public void zkxnk(rmkkr p0, bool p1, int p2)
		{
		}

		public rackf(nnzwd outer)
		{
			rfsav = outer;
		}

		public lnabj qaqes(rmkkr p0, bool p1, int p2)
		{
			return this;
		}

		public void lnxah(byte[] p0, int p1, int p2)
		{
			rfsav.lnxah(p0, p1, p2);
		}

		public void somzq()
		{
		}

		private void vughb(fxakl p0)
		{
		}

		void lnabj.vlfdh(fxakl p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in vughb
			this.vughb(p0);
		}
	}

	private byte[] yhfra;

	private MemoryStream xvkes;

	private ltutw ispyd;

	public byte[] lktyp
	{
		get
		{
			if (xvkes != null && 0 == 0)
			{
				return xvkes.ToArray();
			}
			return yhfra;
		}
	}

	public nnzwd(byte[] data)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		yhfra = data;
	}

	internal nnzwd()
	{
	}

	public nnzwd ltfsl()
	{
		nnzwd nnzwd2 = new nnzwd();
		if (yhfra != null && 0 == 0)
		{
			nnzwd2.yhfra = (byte[])yhfra.Clone();
		}
		return nnzwd2;
	}

	public void zkxnk(rmkkr p0, bool p1, int p2)
	{
		xvkes = new MemoryStream();
	}

	public lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		if (ispyd == null || 1 == 0)
		{
			ispyd = new rackf(this);
		}
		return ispyd;
	}

	public void lnxah(byte[] p0, int p1, int p2)
	{
		xvkes.Write(p0, p1, p2);
	}

	public virtual void somzq()
	{
		yhfra = xvkes.ToArray();
		xvkes.Close();
		xvkes = null;
		ispyd = null;
	}

	private void zbtoi(fxakl p0)
	{
		p0.enfzf(yhfra);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zbtoi
		this.zbtoi(p0);
	}

	private int ypdtd(nnzwd p0)
	{
		byte[] array = lktyp;
		byte[] array2 = p0?.lktyp;
		if (array == null || 1 == 0)
		{
			if (array2 != null && 0 == 0)
			{
				return -1;
			}
			return 0;
		}
		if (array2 == null || 1 == 0)
		{
			return 1;
		}
		int num = Math.Min(array.Length, array2.Length);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0058;
		}
		goto IL_007c;
		IL_007c:
		if (num2 >= num)
		{
			return array.Length.CompareTo(array2.Length);
		}
		goto IL_0058;
		IL_0058:
		int num3 = array[num2].CompareTo(array2[num2]);
		if (num3 != 0 && 0 == 0)
		{
			return num3;
		}
		num2++;
		goto IL_007c;
	}

	int IComparable<nnzwd>.CompareTo(nnzwd p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ypdtd
		return this.ypdtd(p0);
	}
}
