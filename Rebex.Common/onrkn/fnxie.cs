using System;
using System.IO;
using System.Security.Cryptography;
using Rebex;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class fnxie : lnabj
{
	private readonly zjcch crnbq = new zjcch();

	private readonly zjcch xpolx = new zjcch();

	private readonly zjcch qjrkk = new zjcch();

	private readonly zjcch adhzn = new zjcch();

	private readonly zjcch piwrz = new zjcch();

	private readonly zjcch idvmg = new zjcch();

	private bool ijuga;

	public fnxie()
	{
	}

	private static byte[] icidj(BinaryReader p0)
	{
		byte[] array = p0.ReadBytes(4);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(array, 0, array.Length);
		}
		int count = BitConverter.ToInt32(array, 0);
		return p0.ReadBytes(count);
	}

	internal fnxie(BinaryReader publicKey, BinaryReader privateKey)
	{
		byte[] array = icidj(publicKey);
		if (array.Length != 7 && EncodingTools.ASCII.GetString(array, 0, array.Length) != "ssh-dss" && 0 == 0)
		{
			throw new CryptographicException("Invalid DSA private key.");
		}
		byte[] data = icidj(publicKey);
		byte[] data2 = icidj(publicKey);
		byte[] data3 = icidj(publicKey);
		byte[] data4 = icidj(publicKey);
		byte[] data5 = icidj(privateKey);
		crnbq = new zjcch(0);
		xpolx = new zjcch(data, allowNegative: false);
		qjrkk = new zjcch(data2, allowNegative: false);
		adhzn = new zjcch(data3, allowNegative: false);
		piwrz = new zjcch(data4, allowNegative: false);
		idvmg = new zjcch(data5, allowNegative: false);
	}

	internal fnxie(DSAParameters dp)
	{
		crnbq = new zjcch(0);
		xpolx = new zjcch(dp.P, allowNegative: false);
		qjrkk = new zjcch(dp.Q, allowNegative: false);
		adhzn = new zjcch(dp.G, allowNegative: false);
		piwrz = new zjcch(dp.Y, allowNegative: false);
		idvmg = new zjcch(dp.X, allowNegative: false);
	}

	public rwolq oziuq()
	{
		return new rwolq(idvmg.ionjf());
	}

	public AlgorithmIdentifier rswkn()
	{
		return new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.10040.4.1"), fxakl.kncuz(new ocawh(xpolx, qjrkk, adhzn)));
	}

	public DSAParameters hywbw()
	{
		return new DSAParameters
		{
			P = zjcch.euzxs(xpolx.rtrhq),
			Q = zjcch.euzxs(qjrkk.rtrhq),
			G = zjcch.euzxs(adhzn.rtrhq),
			Y = zjcch.euzxs(piwrz.rtrhq),
			X = zjcch.euzxs(idvmg.rtrhq)
		};
	}

	private void djmey(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in djmey
		this.djmey(p0, p1, p2);
	}

	private lnabj bdglm(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return crnbq;
		case 1:
			return xpolx;
		case 2:
			return qjrkk;
		case 3:
			return adhzn;
		case 4:
			return piwrz;
		case 5:
			ijuga = true;
			return idvmg;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bdglm
		return this.bdglm(p0, p1, p2);
	}

	private void acsjo(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in acsjo
		this.acsjo(p0, p1, p2);
	}

	private void uwija()
	{
		if (!ijuga || 1 == 0)
		{
			throw new CryptographicException("Invalid DssPrivateKey.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in uwija
		this.uwija();
	}

	public void vlfdh(fxakl p0)
	{
		p0.suudj(crnbq, xpolx, qjrkk, adhzn, piwrz, idvmg);
	}
}
