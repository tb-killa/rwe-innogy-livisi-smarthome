using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class bbiry : lnabj
{
	private AlgorithmIdentifier uwqfi;

	private AlgorithmIdentifier ifpki;

	private zjcch atijj;

	private zjcch wnhev;

	public bbiry()
	{
	}

	public bbiry(mrxvh signatureParameters)
	{
		HashingAlgorithmId hashingAlgorithmId = bpkgq.wrqur(signatureParameters.faqqk);
		HashingAlgorithmId hashingAlgorithmId2 = bpkgq.wrqur(signatureParameters.wqjdn);
		if (hashingAlgorithmId != HashingAlgorithmId.SHA1 || hashingAlgorithmId != hashingAlgorithmId2 || signatureParameters.xvcnk != 20)
		{
			uwqfi = AlgorithmIdentifier.heubo(hashingAlgorithmId);
			if (uwqfi == null || 1 == 0)
			{
				throw new InvalidOperationException("Unsupported hash algorithm.");
			}
			if (hashingAlgorithmId2 == (HashingAlgorithmId)0 || 1 == 0)
			{
				hashingAlgorithmId2 = hashingAlgorithmId;
			}
			AlgorithmIdentifier algorithmIdentifier = AlgorithmIdentifier.heubo(hashingAlgorithmId2);
			if (algorithmIdentifier == null || 1 == 0)
			{
				throw new InvalidOperationException("Unsupported hash algorithm.");
			}
			ifpki = new AlgorithmIdentifier("1.2.840.113549.1.1.8", fxakl.kncuz(algorithmIdentifier));
			atijj = new zjcch(signatureParameters.xvcnk);
		}
	}

	private int vwkut()
	{
		if (atijj == null || 1 == 0)
		{
			return 20;
		}
		int num = atijj.kybig();
		if (num < 0 || num > 65535)
		{
			throw new CryptographicException("Unsupported PSS salt length '" + atijj.ToString() + "'.");
		}
		return num;
	}

	private void ifqrv(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ifqrv
		this.ifqrv(p0, p1, p2);
	}

	public SignatureParameters rjpev()
	{
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.HashAlgorithm = fzwux();
		signatureParameters.sqxle = nztzp();
		signatureParameters.Format = SignatureFormat.Pkcs;
		signatureParameters.PaddingScheme = SignaturePaddingScheme.Pss;
		signatureParameters.SaltLength = vwkut();
		return signatureParameters;
	}

	private HashingAlgorithmId nztzp()
	{
		HashingAlgorithmId hashingAlgorithmId;
		if (ifpki != null && 0 == 0)
		{
			if (ifpki.Oid.Value != "1.2.840.113549.1.1.8" && 0 == 0)
			{
				throw new CryptographicException(string.Concat("Unsupported PSS mask generation function  '", ifpki.Oid, "'."));
			}
			AlgorithmIdentifier algorithmIdentifier = new AlgorithmIdentifier();
			hfnnn.qnzgo(algorithmIdentifier, ifpki.Parameters);
			hashingAlgorithmId = algorithmIdentifier.vvmoi(p0: false);
			if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
			{
				throw new CryptographicException(string.Concat("Unsupported PSS mask generation hashing algorithm '", algorithmIdentifier.Oid, "'."));
			}
		}
		else
		{
			hashingAlgorithmId = HashingAlgorithmId.SHA1;
		}
		return hashingAlgorithmId;
	}

	private HashingAlgorithmId fzwux()
	{
		HashingAlgorithmId hashingAlgorithmId;
		if (uwqfi != null && 0 == 0)
		{
			hashingAlgorithmId = uwqfi.vvmoi(p0: false);
			if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
			{
				throw new CryptographicException(string.Concat("Unsupported PSS hashing algorithm '", uwqfi.Oid, "'."));
			}
		}
		else
		{
			hashingAlgorithmId = HashingAlgorithmId.SHA1;
		}
		if (wnhev != null && 0 == 0)
		{
			int num = wnhev.kybig();
			if (num != 1)
			{
				throw new CryptographicException("Unsupported PSS trailer field '" + wnhev.ToString() + "'.");
			}
		}
		return hashingAlgorithmId;
	}

	private lnabj aeomi(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 65536:
			uwqfi = new AlgorithmIdentifier();
			return new rporh(uwqfi, 0);
		case 65537:
			ifpki = new AlgorithmIdentifier();
			return new rporh(ifpki, 1);
		case 65538:
			atijj = new zjcch();
			return new rporh(atijj, 2);
		case 65539:
			wnhev = new zjcch();
			return new rporh(wnhev, 3);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in aeomi
		return this.aeomi(p0, p1, p2);
	}

	private void rpkcw()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in rpkcw
		this.rpkcw();
	}

	private void vrvgl(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vrvgl
		this.vrvgl(p0, p1, p2);
	}

	private void idjzw(fxakl p0)
	{
		List<lnabj> list = new List<lnabj>();
		if (uwqfi != null && 0 == 0)
		{
			list.Add(new rporh(uwqfi, 0));
		}
		if (ifpki != null && 0 == 0)
		{
			list.Add(new rporh(ifpki, 1));
		}
		if (atijj != null && 0 == 0)
		{
			list.Add(new rporh(atijj, 2));
		}
		if (wnhev != null && 0 == 0)
		{
			list.Add(new rporh(wnhev, 3));
		}
		p0.suudj(list.ToArray());
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in idjzw
		this.idjzw(p0);
	}
}
