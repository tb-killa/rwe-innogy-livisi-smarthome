using System.IO;
using System.Security.Cryptography;

namespace onrkn;

internal class htykq : lnabj
{
	private class kpfro : lnabj
	{
		private readonly htykq xobxu;

		private bool tqxjo;

		private MemoryStream jiwln;

		public void zkxnk(rmkkr p0, bool p1, int p2)
		{
			if (xobxu.oyqgc != 0 && 0 == 0)
			{
				throw new CryptographicException("Invalid constructed bit string.");
			}
			if (p0 != rmkkr.ysphu)
			{
				throw new CryptographicException("Encountered ASN.1 node with mismatched type.");
			}
			tqxjo = p1;
			jiwln = new MemoryStream();
		}

		public kpfro(htykq outer)
		{
			xobxu = outer;
		}

		public lnabj qaqes(rmkkr p0, bool p1, int p2)
		{
			if (!tqxjo || 1 == 0)
			{
				throw new CryptographicException("Encountered constructed ASN.1 node within primitive node.");
			}
			if (p0 != rmkkr.ysphu)
			{
				throw new CryptographicException("Encountered constructed ASN.1 node with mismatched type.");
			}
			return new kpfro(xobxu);
		}

		public void lnxah(byte[] p0, int p1, int p2)
		{
			jiwln.Write(p0, p1, p2);
		}

		public void somzq()
		{
			if (jiwln.Length == 0)
			{
				throw new CryptographicException("Invalid constructed bit string.");
			}
			byte[] buffer = jiwln.GetBuffer();
			xobxu.oyqgc = buffer[0];
			xobxu.lnxah(buffer, 0, (int)jiwln.Length);
			jiwln.Close();
		}

		private void ppbqq(fxakl p0)
		{
		}

		void lnabj.vlfdh(fxakl p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ppbqq
			this.ppbqq(p0);
		}
	}

	private byte[] xhxwz;

	private int copqv;

	private bool jmcve;

	private MemoryStream dqloz;

	public byte[] lssxa => xhxwz;

	public int oyqgc
	{
		get
		{
			return copqv;
		}
		set
		{
			copqv = value;
		}
	}

	public int astri
	{
		get
		{
			int num = xhxwz.Length + 1;
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

	public htykq()
	{
	}

	public htykq(byte[] data, int padding)
	{
		xhxwz = data;
		copqv = padding;
	}

	public virtual void zkxnk(rmkkr p0, bool p1, int p2)
	{
		jmcve = p1;
		dqloz = new MemoryStream();
	}

	public virtual lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		if (!jmcve || 1 == 0)
		{
			return null;
		}
		if (p0 != rmkkr.ysphu)
		{
			throw new CryptographicException("Encountered inner ASN.1 node with mismatched type.");
		}
		return new kpfro(this);
	}

	public virtual void lnxah(byte[] p0, int p1, int p2)
	{
		if (p2 > 0 && dqloz.Length == 0)
		{
			copqv = p0[p1];
			p1++;
			p2--;
		}
		dqloz.Write(p0, p1, p2);
	}

	public virtual void somzq()
	{
		xhxwz = dqloz.ToArray();
		dqloz.Close();
		dqloz = null;
	}

	protected virtual void trxqw(fxakl p0, byte p1)
	{
	}

	private void ayppi(fxakl p0)
	{
		byte[] array = new byte[xhxwz.Length + 1];
		array[0] = (byte)copqv;
		xhxwz.CopyTo(array, 1);
		p0.loggt(rmkkr.ysphu, array);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ayppi
		this.ayppi(p0);
	}

	public byte[] arcrw()
	{
		return fxakl.kncuz(this);
	}

	public ushort xmojg()
	{
		if (xhxwz.Length == 0 || 1 == 0)
		{
			return 0;
		}
		int num = 255;
		if (copqv > 0)
		{
			num = (num << copqv) & 0xFF;
		}
		if (xhxwz.Length == 1)
		{
			return (ushort)(xhxwz[0] & num);
		}
		if (xhxwz.Length == 2)
		{
			return (ushort)(((xhxwz[1] & num) << 8) + xhxwz[0]);
		}
		return (ushort)((xhxwz[1] << 8) + xhxwz[0]);
	}
}
