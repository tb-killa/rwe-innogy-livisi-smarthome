using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class uchfa : lnabj
{
	private AlgorithmIdentifier vgune;

	private AlgorithmIdentifier tmwja;

	private AlgorithmIdentifier qrsiz;

	public uchfa()
	{
	}

	public uchfa(jyamo encryptionParameters)
	{
		if (encryptionParameters == null || 1 == 0)
		{
			throw new ArgumentNullException("encryptionParameters");
		}
		if (encryptionParameters.vmeor != xdgzn.bntzq)
		{
			throw new InvalidOperationException("Only OAEP padding scheme is supported.");
		}
		HashingAlgorithmId fbcyx = encryptionParameters.fbcyx;
		HashingAlgorithmId hashingAlgorithmId = encryptionParameters.bablj;
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
		{
			hashingAlgorithmId = fbcyx;
		}
		if (fbcyx != HashingAlgorithmId.SHA1)
		{
			vgune = AlgorithmIdentifier.heubo(fbcyx);
			if (vgune == null || 1 == 0)
			{
				throw new InvalidOperationException("Unsupported hash algorithm.");
			}
		}
		if (hashingAlgorithmId != HashingAlgorithmId.SHA1)
		{
			if (AlgorithmIdentifier.heubo(hashingAlgorithmId) == null || 1 == 0)
			{
				throw new InvalidOperationException("Unsupported hash algorithm.");
			}
			tmwja = new AlgorithmIdentifier("1.2.840.113549.1.1.8", fxakl.kncuz(vgune));
		}
	}

	private void xrozs(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xrozs
		this.xrozs(p0, p1, p2);
	}

	public EncryptionParameters vsuvv()
	{
		EncryptionParameters encryptionParameters = new EncryptionParameters();
		encryptionParameters.Label = oamqc();
		encryptionParameters.HashAlgorithm = wjxop();
		encryptionParameters.mciwu = ygczs();
		encryptionParameters.PaddingScheme = EncryptionPaddingScheme.Oaep;
		return encryptionParameters;
	}

	private byte[] oamqc()
	{
		if (qrsiz != null && 0 == 0)
		{
			if (qrsiz.Oid.Value != "1.2.840.113549.1.1.9" && 0 == 0)
			{
				throw new CryptographicException(string.Concat("Unsupported OAEP encoding parameters source '", qrsiz.Oid, "'."));
			}
			if (qrsiz.Parameters != null && 0 == 0 && qrsiz.Parameters.Length != 0 && 0 == 0)
			{
				byte[] rtrhq = rwolq.tvjgt(qrsiz.Parameters).rtrhq;
				if (rtrhq.Length > 0)
				{
					return rtrhq;
				}
			}
		}
		return null;
	}

	private HashingAlgorithmId ygczs()
	{
		HashingAlgorithmId hashingAlgorithmId;
		if (tmwja != null && 0 == 0)
		{
			if (tmwja.Oid.Value != "1.2.840.113549.1.1.8" && 0 == 0)
			{
				throw new CryptographicException(string.Concat("Unsupported OAEP mask generation function  '", tmwja.Oid, "'."));
			}
			AlgorithmIdentifier algorithmIdentifier = new AlgorithmIdentifier();
			hfnnn.qnzgo(algorithmIdentifier, tmwja.Parameters);
			hashingAlgorithmId = algorithmIdentifier.vvmoi(p0: false);
			if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
			{
				throw new CryptographicException(string.Concat("Unsupported OAEP mask generation hashing algorithm '", algorithmIdentifier.Oid, "'."));
			}
		}
		else
		{
			hashingAlgorithmId = HashingAlgorithmId.SHA1;
		}
		return hashingAlgorithmId;
	}

	private HashingAlgorithmId wjxop()
	{
		HashingAlgorithmId hashingAlgorithmId;
		if (vgune != null && 0 == 0)
		{
			hashingAlgorithmId = vgune.vvmoi(p0: false);
			if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
			{
				throw new CryptographicException(string.Concat("Unsupported OAEP hashing algorithm '", vgune.Oid, "'."));
			}
		}
		else
		{
			hashingAlgorithmId = HashingAlgorithmId.SHA1;
		}
		return hashingAlgorithmId;
	}

	private lnabj kyhuf(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 65536:
			vgune = new AlgorithmIdentifier();
			return new rporh(vgune, 0);
		case 65537:
			tmwja = new AlgorithmIdentifier();
			return new rporh(tmwja, 1);
		case 65538:
			qrsiz = new AlgorithmIdentifier();
			return new rporh(qrsiz, 2);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kyhuf
		return this.kyhuf(p0, p1, p2);
	}

	private void zexrt()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in zexrt
		this.zexrt();
	}

	private void cyutp(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in cyutp
		this.cyutp(p0, p1, p2);
	}

	private void xcmdd(fxakl p0)
	{
		List<lnabj> list = new List<lnabj>();
		if (vgune != null && 0 == 0)
		{
			list.Add(new rporh(vgune, 0));
		}
		if (tmwja != null && 0 == 0)
		{
			list.Add(new rporh(tmwja, 1));
		}
		if (qrsiz != null && 0 == 0)
		{
			list.Add(new rporh(qrsiz, 2));
		}
		p0.suudj(list.ToArray());
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xcmdd
		this.xcmdd(p0);
	}
}
