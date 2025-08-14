using System.IO;
using System.Security.Cryptography;
using Rebex;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class elzlx : lnabj
{
	private readonly zjcch amdnu;

	private readonly zjcch uqpkt;

	private readonly zjcch piohi;

	private readonly zjcch lsgrs;

	private readonly zjcch pjcwt;

	private readonly zjcch xleme;

	private readonly zjcch uadqi;

	private readonly zjcch loizs;

	private readonly zjcch nrhwv;

	private nnzwd lidcx;

	private bool mphdh;

	public elzlx()
	{
		amdnu = new zjcch();
		uqpkt = new zjcch();
		piohi = new zjcch();
		lsgrs = new zjcch();
		pjcwt = new zjcch();
		xleme = new zjcch();
		uadqi = new zjcch();
		loizs = new zjcch();
		nrhwv = new zjcch();
	}

	public elzlx(RSAParameters rp)
	{
		amdnu = new zjcch(0);
		uqpkt = new zjcch(rp.Modulus, allowNegative: false);
		piohi = new zjcch(rp.Exponent, allowNegative: false);
		lsgrs = new zjcch(rp.D, allowNegative: false);
		pjcwt = new zjcch(rp.P, allowNegative: false);
		xleme = new zjcch(rp.Q, allowNegative: false);
		if (rp.DP == null || false || rp.DQ == null)
		{
			bdjih p = bdjih.foxoi(rp.P) - 1;
			bdjih p2 = bdjih.foxoi(rp.Q) - 1;
			bdjih bdjih2 = bdjih.foxoi(rp.D);
			bdjih bdjih3 = bdjih2.fvbmy(p);
			bdjih bdjih4 = bdjih2.fvbmy(p2);
			uadqi = new zjcch(bdjih3.kskce(p0: false), allowNegative: false);
			loizs = new zjcch(bdjih4.kskce(p0: false), allowNegative: false);
		}
		else
		{
			uadqi = new zjcch(rp.DP, allowNegative: false);
			loizs = new zjcch(rp.DQ, allowNegative: false);
		}
		nrhwv = new zjcch(rp.InverseQ, allowNegative: false);
	}

	internal elzlx(BinaryReader publicKey, BinaryReader privateKey)
	{
		byte[] array = PrivateKeyInfo.ghzbc(publicKey);
		if (array.Length != 7 && EncodingTools.ASCII.GetString(array, 0, array.Length) != "ssh-rsa" && 0 == 0)
		{
			throw new CryptographicException("Invalid RSA private key.");
		}
		byte[] data = PrivateKeyInfo.ghzbc(publicKey);
		byte[] data2 = PrivateKeyInfo.ghzbc(publicKey);
		byte[] array2 = PrivateKeyInfo.ghzbc(privateKey);
		byte[] array3 = PrivateKeyInfo.ghzbc(privateKey);
		byte[] array4 = PrivateKeyInfo.ghzbc(privateKey);
		byte[] data3 = PrivateKeyInfo.ghzbc(privateKey);
		amdnu = new zjcch(0);
		uqpkt = new zjcch(data2, allowNegative: false);
		piohi = new zjcch(data, allowNegative: false);
		lsgrs = new zjcch(array2, allowNegative: false);
		pjcwt = new zjcch(array3, allowNegative: false);
		xleme = new zjcch(array4, allowNegative: false);
		nrhwv = new zjcch(data3, allowNegative: false);
		bdjih p = bdjih.foxoi(array3) - 1;
		bdjih p2 = bdjih.foxoi(array4) - 1;
		bdjih bdjih2 = bdjih.foxoi(array2);
		bdjih bdjih3 = bdjih2.fvbmy(p);
		bdjih bdjih4 = bdjih2.fvbmy(p2);
		uadqi = new zjcch(bdjih3.kskce(p0: false), allowNegative: false);
		loizs = new zjcch(bdjih4.kskce(p0: false), allowNegative: false);
	}

	public RSAParameters wtkfr()
	{
		RSAParameters result = new RSAParameters
		{
			Modulus = zjcch.euzxs(uqpkt.rtrhq)
		};
		int num = result.Modulus.Length;
		result.Exponent = zjcch.euzxs(piohi.rtrhq);
		result.D = zjcch.bxisb(lsgrs.rtrhq, num, num + 1);
		num /= 2;
		result.P = zjcch.bxisb(pjcwt.rtrhq, num, num + 1);
		result.Q = zjcch.bxisb(xleme.rtrhq, num, num + 1);
		result.DP = zjcch.bxisb(uadqi.rtrhq, num, num + 1);
		result.DQ = zjcch.bxisb(loizs.rtrhq, num, num + 1);
		result.InverseQ = zjcch.bxisb(nrhwv.rtrhq, num, num + 1);
		return result;
	}

	private void ksfka(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ksfka
		this.ksfka(p0, p1, p2);
	}

	private lnabj lyqrw(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return amdnu;
		case 1:
			return uqpkt;
		case 2:
			return piohi;
		case 3:
			return lsgrs;
		case 4:
			return pjcwt;
		case 5:
			return xleme;
		case 6:
			return uadqi;
		case 7:
			return loizs;
		case 8:
			mphdh = true;
			return nrhwv;
		case 9:
			lidcx = new nnzwd();
			return lidcx;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lyqrw
		return this.lyqrw(p0, p1, p2);
	}

	private void bubtr(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bubtr
		this.bubtr(p0, p1, p2);
	}

	private void ksdsr()
	{
		if (!mphdh || 1 == 0)
		{
			throw new CryptographicException("Invalid RsaPrivateKey.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ksdsr
		this.ksdsr();
	}

	public void vlfdh(fxakl p0)
	{
		if (lidcx == null || 1 == 0)
		{
			p0.suudj(amdnu, uqpkt, piohi, lsgrs, pjcwt, xleme, uadqi, loizs, nrhwv);
		}
		else
		{
			p0.suudj(amdnu, uqpkt, piohi, lsgrs, pjcwt, xleme, uadqi, loizs, nrhwv, lidcx);
		}
	}
}
