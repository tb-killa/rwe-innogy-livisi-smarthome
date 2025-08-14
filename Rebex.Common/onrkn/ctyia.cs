using System.Security.Cryptography;

namespace onrkn;

internal class ctyia : lnabj
{
	private readonly zjcch wmvbu;

	private readonly rwolq kbkuk;

	public int pgahg => wmvbu.kybig() switch
	{
		160 => 40, 
		120 => 64, 
		58 => 128, 
		_ => throw new CryptographicException("Invalid RC2 key length encountered."), 
	};

	public byte[] wpttm => kbkuk.rtrhq;

	public ctyia()
	{
		wmvbu = new zjcch();
		kbkuk = new rwolq();
	}

	public ctyia(int keyLength, byte[] iv)
	{
		keyLength = keyLength switch
		{
			40 => 160, 
			64 => 120, 
			128 => 58, 
			_ => throw new CryptographicException("Invalid RC2 key length encountered."), 
		};
		wmvbu = new zjcch(keyLength);
		kbkuk = new rwolq(iv, clone: true);
	}

	private void ecakk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ecakk
		this.ecakk(p0, p1, p2);
	}

	private lnabj dtgzc(rmkkr p0, bool p1, int p2)
	{
		return p2 switch
		{
			0 => wmvbu, 
			1 => kbkuk, 
			_ => null, 
		};
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dtgzc
		return this.dtgzc(p0, p1, p2);
	}

	private void jdqmb(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jdqmb
		this.jdqmb(p0, p1, p2);
	}

	private void elhmz()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in elhmz
		this.elhmz();
	}

	private void kkrlx(fxakl p0)
	{
		p0.afwyb();
		p0.suudj(wmvbu, kbkuk);
		p0.xljze();
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kkrlx
		this.kkrlx(p0);
	}
}
