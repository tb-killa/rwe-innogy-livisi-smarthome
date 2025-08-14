using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class RecipientInfoCollection : CryptographicCollection<RecipientInfo>, lnabj
{
	private readonly EnvelopedData spfql;

	private readonly ArrayList sogni = new ArrayList();

	private bool iylia;

	internal bool qgzsb => iylia;

	public RecipientInfoCollection()
		: base(rmkkr.wguaf)
	{
		hksnh();
	}

	internal RecipientInfoCollection(EnvelopedData envelopedData)
		: base(rmkkr.wguaf)
	{
		spfql = envelopedData;
	}

	internal override void ttbls(RecipientInfo p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("recipientInfo");
		}
		if (p0.boltq != null && 0 == 0)
		{
			throw new CryptographicException("The recipient was already associated with another message.");
		}
		vgeou();
		base.ttbls(p0);
		p0.bvglb(spfql);
	}

	internal override void gwubw(int p0, RecipientInfo p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("recipientInfo");
		}
		if (p1.boltq != null && 0 == 0)
		{
			throw new CryptographicException("The recipient was already associated with another message.");
		}
		base.gwubw(p0, p1);
		p1.bvglb(spfql);
	}

	internal override bool rgnjk(RecipientInfo p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("recipientInfo");
		}
		if (p0.boltq != spfql)
		{
			throw new CryptographicException("The recipient info is associated with another message.");
		}
		if (!base.rgnjk(p0) || 1 == 0)
		{
			return false;
		}
		p0.bvglb(null);
		return true;
	}

	internal override void prujo(int p0)
	{
		RecipientInfo recipientInfo = base[p0];
		base.prujo(p0);
		recipientInfo.bvglb(null);
	}

	internal override void bxkbx()
	{
		List<RecipientInfo> list = new List<RecipientInfo>(base.lquvo);
		base.bxkbx();
		using List<RecipientInfo>.Enumerator enumerator = list.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			RecipientInfo current = enumerator.Current;
			current.bvglb(null);
		}
	}

	internal override void tqdqu(int p0, RecipientInfo p1)
	{
		RecipientInfo recipientInfo = base[p0];
		if (recipientInfo != p1)
		{
			base[p0] = p1;
			recipientInfo.bvglb(null);
		}
	}

	private lnabj kaclm(rmkkr p0, bool p1, int p2)
	{
		if (p2 < 65536)
		{
			KeyTransRecipientInfo keyTransRecipientInfo = new KeyTransRecipientInfo();
			base.lquvo.Add(keyTransRecipientInfo);
			return keyTransRecipientInfo;
		}
		switch (p2)
		{
		case 65537:
		{
			KeyAgreeRecipientInfo keyAgreeRecipientInfo = new KeyAgreeRecipientInfo();
			base.lquvo.Add(keyAgreeRecipientInfo);
			return keyAgreeRecipientInfo;
		}
		case 65539:
		case 65540:
			iylia = true;
			break;
		}
		nnzwd nnzwd = new nnzwd();
		sogni.Add(nnzwd);
		return nnzwd;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kaclm
		return this.kaclm(p0, p1, p2);
	}

	private void iyopr(fxakl p0)
	{
		p0.afwyb();
		lnabj[] array = new lnabj[base.Count + sogni.Count];
		CopyTo(array, 0);
		int num = 0;
		if (num != 0)
		{
			goto IL_002b;
		}
		goto IL_004f;
		IL_002b:
		if (array[num] is KeyAgreeRecipientInfo && 0 == 0)
		{
			array[num] = new rwknq(array[num], 1, rmkkr.osptv);
		}
		num++;
		goto IL_004f;
		IL_004f:
		if (num >= base.Count)
		{
			sogni.CopyTo(array, base.Count);
			p0.aiflg(rmkkr.wguaf, array);
			p0.xljze();
			return;
		}
		goto IL_002b;
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in iyopr
		this.iyopr(p0);
	}
}
