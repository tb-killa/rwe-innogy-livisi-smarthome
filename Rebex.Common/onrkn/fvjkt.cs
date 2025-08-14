using System;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal sealed class fvjkt : eatps, dzjkq, ntowq, lncnv, IDisposable
{
	private readonly eatps rguav;

	private readonly AsymmetricKeyAlgorithmId eynux;

	private readonly jayrg yswwg;

	private eatps jcact;

	private eatps humgz;

	private eatps zqpgn;

	private eatps smxnw;

	private eatps uwdrn;

	private bool ijwap;

	public string janem => rguav.janem;

	public int KeySize => rguav.KeySize;

	public AsymmetricKeyAlgorithmId bptsq => eynux;

	public fvjkt(eatps publicKeyAlgorithm, jayrg privateKey)
	{
		if (publicKeyAlgorithm == null || 1 == 0)
		{
			throw new ArgumentNullException("publicKeyAlgorithm");
		}
		rguav = publicKeyAlgorithm;
		eynux = publicKeyAlgorithm.bptsq;
		yswwg = privateKey;
	}

	private void gfmtv()
	{
		if (yswwg == null || 1 == 0)
		{
			throw new CryptographicException("Private key not available.");
		}
	}

	private bool kzvdm(jyamo p0)
	{
		return true;
	}

	bool ntowq.knvjq(jyamo p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kzvdm
		return this.kzvdm(p0);
	}

	private byte[] tecrp(byte[] p0, jyamo p1)
	{
		ntowq ntowq2 = ykzgt(p1);
		if (ntowq2 == null || 1 == 0)
		{
			ntowq2 = rguav as ntowq;
			if (ntowq2 == null || 1 == 0)
			{
				throw new CryptographicException("Not supported for this key algorithm.");
			}
		}
		return ntowq2.sfbms(p0, p1);
	}

	byte[] ntowq.sfbms(byte[] p0, jyamo p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tecrp
		return this.tecrp(p0, p1);
	}

	private byte[] ngpdq(byte[] p0, jyamo p1)
	{
		gfmtv();
		ntowq ntowq2 = mnnfs(p1);
		if (ntowq2 != null && 0 == 0)
		{
			return ntowq2.lhhds(p0, p1);
		}
		return yswwg.lhhds(p0, p1);
	}

	byte[] ntowq.lhhds(byte[] p0, jyamo p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ngpdq
		return this.ngpdq(p0, p1);
	}

	private bool ykmze(mrxvh p0)
	{
		return true;
	}

	bool dzjkq.vmedb(mrxvh p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ykmze
		return this.ykmze(p0);
	}

	private byte[] ihynq(byte[] p0, mrxvh p1)
	{
		gfmtv();
		dzjkq dzjkq2 = naoxx(p1);
		if (dzjkq2 != null && 0 == 0)
		{
			return dzjkq2.rypyi(p0, p1);
		}
		return yswwg.rypyi(p0, p1);
	}

	byte[] dzjkq.rypyi(byte[] p0, mrxvh p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ihynq
		return this.ihynq(p0, p1);
	}

	private bool yoxek(byte[] p0, byte[] p1, mrxvh p2)
	{
		dzjkq dzjkq2 = jbwtg(p2);
		if (dzjkq2 == null || 1 == 0)
		{
			dzjkq2 = rguav as dzjkq;
			if (dzjkq2 == null || 1 == 0)
			{
				throw new CryptographicException("Not supported for this key algorithm.");
			}
		}
		return dzjkq2.cbzmp(p0, p1, p2);
	}

	bool dzjkq.cbzmp(byte[] p0, byte[] p1, mrxvh p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in yoxek
		return this.yoxek(p0, p1, p2);
	}

	private PublicKeyInfo fbcwf()
	{
		if (rguav is lncnv lncnv2 && 0 == 0)
		{
			return lncnv2.kptoi();
		}
		throw new CryptographicException("Not supported for this key algorithm.");
	}

	PublicKeyInfo lncnv.kptoi()
	{
		//ILSpy generated this explicit interface implementation from .override directive in fbcwf
		return this.fbcwf();
	}

	private PrivateKeyInfo idzoo(bool p0)
	{
		gfmtv();
		return yswwg.jbbgs(p0);
	}

	PrivateKeyInfo lncnv.jbbgs(bool p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in idzoo
		return this.idzoo(p0);
	}

	private CspParameters lgyam()
	{
		gfmtv();
		return yswwg.iqqfj();
	}

	CspParameters lncnv.iqqfj()
	{
		//ILSpy generated this explicit interface implementation from .override directive in lgyam
		return this.lgyam();
	}

	private ntowq mnnfs(jyamo p0)
	{
		if (eynux != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			return null;
		}
		if (yswwg.knvjq(p0) && 0 == 0)
		{
			return null;
		}
		return ptxtj(p0) as ntowq;
	}

	private dzjkq naoxx(mrxvh p0)
	{
		if (eynux != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			return null;
		}
		if (yswwg.vmedb(p0) && 0 == 0)
		{
			return null;
		}
		if (p0.faqqk == SignatureHashAlgorithm.MD5SHA1)
		{
			if (p0.hqtwc != goies.lfkki)
			{
				return null;
			}
			return breyb() as dzjkq;
		}
		return ptxtj(p0) as dzjkq;
	}

	private static eatps ksavi(jayrg p0, Func<string, imfrk> p1, string p2)
	{
		PrivateKeyInfo privateKeyInfo;
		try
		{
			bool p3 = !CryptoHelper.ebikb;
			privateKeyInfo = p0.jbbgs(p3);
		}
		catch (Exception inner)
		{
			throw new CryptographicException("Unable to export private key in order to use a " + p2 + " algorithm.", inner);
		}
		string arg = privateKeyInfo.bdgxx();
		imfrk imfrk2 = p1(arg);
		if (imfrk2 == null || 1 == 0)
		{
			return null;
		}
		return imfrk2.xaunu(privateKeyInfo);
	}

	private eatps ptxtj(fivsd p0)
	{
		eatps eatps2 = humgz;
		if (p0.izbcg && 0 == 0)
		{
			if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
			{
				return null;
			}
			if (eatps2 is RSAManaged && 0 == 0)
			{
				return eatps2;
			}
			eatps2 = ksavi(yswwg, RSAManaged.icbbo, "managed");
			if (humgz is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
			humgz = eatps2;
		}
		else if (eatps2 == null || 1 == 0)
		{
			eatps2 = (humgz = ksavi(yswwg, rmnyn.aregb, "more capable"));
		}
		return eatps2;
	}

	private eatps breyb()
	{
		eatps eatps2 = smxnw;
		if (eatps2 == null || 1 == 0)
		{
			eatps2 = (smxnw = ksavi(yswwg, rmnyn.fnqth, "legacy"));
		}
		return eatps2;
	}

	private dzjkq jbwtg(mrxvh p0)
	{
		if (eynux != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			return null;
		}
		if (p0.hqtwc == goies.lfkki && (!CryptoHelper.wyrkl || 1 == 0))
		{
			switch (p0.faqqk)
			{
			case SignatureHashAlgorithm.SHA256:
			case SignatureHashAlgorithm.SHA384:
			case SignatureHashAlgorithm.SHA512:
				if (umczq() is dzjkq result && 0 == 0)
				{
					return result;
				}
				break;
			}
		}
		if (rguav is dzjkq dzjkq2 && 0 == 0 && dzjkq2.vmedb(p0) && 0 == 0)
		{
			return null;
		}
		if (p0.faqqk == SignatureHashAlgorithm.MD5SHA1)
		{
			if (p0.hqtwc != goies.lfkki)
			{
				return null;
			}
			return mwena() as dzjkq;
		}
		return eejos(p0) as dzjkq;
	}

	private ntowq ykzgt(jyamo p0)
	{
		if (eynux != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			return null;
		}
		if (rguav is ntowq ntowq2 && 0 == 0 && ntowq2.knvjq(p0) && 0 == 0)
		{
			return null;
		}
		if (p0.vmeor != xdgzn.bntzq)
		{
			return null;
		}
		return eejos(p0) as ntowq;
	}

	private static eatps kihon(eatps p0, Func<string, imfrk> p1, string p2)
	{
		PublicKeyInfo publicKeyInfo = null;
		Exception inner = null;
		try
		{
			if (p0 is lncnv lncnv2 && 0 == 0)
			{
				publicKeyInfo = lncnv2.kptoi();
			}
		}
		catch (Exception ex)
		{
			inner = ex;
		}
		if (publicKeyInfo == null || 1 == 0)
		{
			throw new CryptographicException("Unable to export public key in order to use a " + p2 + " algorithm.", inner);
		}
		string arg = publicKeyInfo.dtrjf();
		imfrk imfrk2 = p1(arg);
		if (imfrk2 == null || 1 == 0)
		{
			return null;
		}
		return imfrk2.neqkn(publicKeyInfo);
	}

	private eatps eejos(fivsd p0)
	{
		eatps eatps2 = jcact;
		if (p0.izbcg && 0 == 0)
		{
			if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
			{
				return null;
			}
			if (eatps2 is RSAManaged && 0 == 0)
			{
				return eatps2;
			}
			eatps2 = kihon(rguav, RSAManaged.icbbo, "managed");
			if (jcact is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
			jcact = eatps2;
		}
		else if (eatps2 == null || 1 == 0)
		{
			eatps2 = (jcact = kihon(rguav, rmnyn.aregb, "more capable"));
		}
		return eatps2;
	}

	private eatps mwena()
	{
		eatps eatps2 = zqpgn;
		if (eatps2 == null || 1 == 0)
		{
			eatps2 = (zqpgn = kihon(rguav, rmnyn.fnqth, "legacy"));
		}
		return eatps2;
	}

	private eatps umczq()
	{
		eatps eatps2 = uwdrn;
		if (eatps2 == null || 1 == 0)
		{
			eatps2 = (uwdrn = kihon(rguav, zogos.ojezw, "native"));
		}
		return eatps2;
	}

	public void Dispose()
	{
		if (!ijwap)
		{
			ijwap = true;
			dahxy.wahav(yswwg);
			dahxy.wahav(rguav);
			dahxy.wahav(humgz);
			dahxy.wahav(jcact);
			dahxy.wahav(smxnw);
			dahxy.wahav(zqpgn);
			dahxy.wahav(uwdrn);
		}
	}
}
