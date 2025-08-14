using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class SignerInfoCollection : CryptographicCollection<SignerInfo>, lnabj
{
	private readonly SignedData hkebe;

	public SignerInfoCollection()
		: base(rmkkr.wguaf)
	{
		hksnh();
	}

	internal SignerInfoCollection(SignedData signedData)
		: base(rmkkr.wguaf)
	{
		hkebe = signedData;
	}

	private lnabj avycl(rmkkr p0, bool p1, int p2)
	{
		SignerInfo signerInfo = new SignerInfo();
		base.lquvo.Add(signerInfo);
		return signerInfo;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in avycl
		return this.avycl(p0, p1, p2);
	}

	internal override void ttbls(SignerInfo p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("signerInfo");
		}
		if (p0.ncyct != null && 0 == 0)
		{
			throw new CryptographicException("The signer was already associated with another message.");
		}
		base.ttbls(p0);
		p0.lndip(hkebe);
	}

	internal override void gwubw(int p0, SignerInfo p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("signerInfo");
		}
		if (p1.ncyct != null && 0 == 0)
		{
			throw new CryptographicException("The signer was already associated with another message.");
		}
		base.gwubw(p0, p1);
		p1.lndip(hkebe);
	}

	internal override bool rgnjk(SignerInfo p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("signerInfo");
		}
		if (p0.ncyct != hkebe)
		{
			throw new CryptographicException("The signer info is associated with another message.");
		}
		if (!base.rgnjk(p0) || 1 == 0)
		{
			return false;
		}
		p0.lndip(null);
		return true;
	}

	internal override void prujo(int p0)
	{
		SignerInfo signerInfo = base[p0];
		base.prujo(p0);
		signerInfo.lndip(null);
	}

	internal override void bxkbx()
	{
		List<SignerInfo> list = new List<SignerInfo>(base.lquvo);
		base.bxkbx();
		using List<SignerInfo>.Enumerator enumerator = list.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			SignerInfo current = enumerator.Current;
			current.lndip(null);
		}
	}

	internal override void tqdqu(int p0, SignerInfo p1)
	{
		SignerInfo signerInfo = base[p0];
		if (signerInfo != p1)
		{
			base[p0] = p1;
			signerInfo.lndip(null);
		}
	}
}
