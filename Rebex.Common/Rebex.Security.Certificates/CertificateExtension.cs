using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Certificates;

public class CertificateExtension : lnabj
{
	private wyjqw wbmti;

	private qlyth qgnex;

	private rwolq lkzdi;

	private byte[] dagyi;

	public string Oid => wbmti.scakm.Value;

	public bool Critical
	{
		get
		{
			if (qgnex == null || 1 == 0)
			{
				return false;
			}
			return qgnex.ogtep();
		}
	}

	public byte[] Value
	{
		get
		{
			if (dagyi == null || 1 == 0)
			{
				dagyi = (byte[])lkzdi.rtrhq.Clone();
			}
			return dagyi;
		}
	}

	public CertificateExtension(string oid, bool critical, byte[] data)
		: this(new ObjectIdentifier(oid), critical, data)
	{
	}

	internal CertificateExtension(ObjectIdentifier oid, bool critical, byte[] data)
	{
		wbmti = new wyjqw(oid);
		if (critical && 0 == 0)
		{
			qgnex = new qlyth(critical);
		}
		lkzdi = new rwolq(data);
	}

	internal CertificateExtension()
	{
	}

	public static CertificateExtension EnhancedKeyUsage(bool critical, params string[] usages)
	{
		motgc motgc = new motgc();
		int num = 0;
		if (num != 0)
		{
			goto IL_0010;
		}
		goto IL_0029;
		IL_0010:
		string oid = usages[num];
		wyjqw item = new wyjqw(oid);
		motgc.Add(item);
		num++;
		goto IL_0029;
		IL_0029:
		if (num < usages.Length)
		{
			goto IL_0010;
		}
		return new CertificateExtension(new ObjectIdentifier("2.5.29.37"), critical, fxakl.kncuz(motgc));
	}

	public static CertificateExtension KeyUsage(KeyUses usage)
	{
		htykq htykq = vieea(usage);
		return new CertificateExtension(new ObjectIdentifier("2.5.29.15"), critical: true, htykq.arcrw());
	}

	private static htykq vieea(KeyUses p0)
	{
		int num = (int)(p0 & (KeyUses)65535);
		int num2 = 0;
		if ((p0 & KeyUses.DigitalSignature) != 0 && 0 == 0)
		{
			num2 = 1;
		}
		if ((p0 & KeyUses.NonRepudiation) != 0 && 0 == 0)
		{
			num2 = 2;
		}
		if ((p0 & KeyUses.KeyEncipherment) != 0 && 0 == 0)
		{
			num2 = 3;
		}
		if ((p0 & KeyUses.DataEncipherment) != 0 && 0 == 0)
		{
			num2 = 4;
		}
		if ((p0 & KeyUses.KeyAgreement) != 0 && 0 == 0)
		{
			num2 = 5;
		}
		if ((p0 & KeyUses.KeyCertSign) != 0 && 0 == 0)
		{
			num2 = 6;
		}
		if ((p0 & KeyUses.CrlSign) != 0 && 0 == 0)
		{
			num2 = 7;
		}
		if ((p0 & KeyUses.EncipherOnly) != 0 && 0 == 0)
		{
			num2 = 8;
		}
		if ((p0 & KeyUses.DecipherOnly) != 0 && 0 == 0)
		{
			num2 = 9;
		}
		if (num2 <= 8)
		{
			return new htykq(new byte[1] { (byte)num }, 8 - num2);
		}
		return new htykq(new byte[2]
		{
			(byte)(num & 0xFF),
			(byte)((num >> 8) & 0xFF)
		}, 16 - num2);
	}

	private void bufrr(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bufrr
		this.bufrr(p0, p1, p2);
	}

	private lnabj jjfkk(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			wbmti = new wyjqw();
			return wbmti;
		case 1:
			if (p0 == rmkkr.pubvj)
			{
				qgnex = new qlyth();
				return qgnex;
			}
			lkzdi = new rwolq();
			return lkzdi;
		case 2:
			if (lkzdi != null && 0 == 0)
			{
				return null;
			}
			lkzdi = new rwolq();
			return lkzdi;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jjfkk
		return this.jjfkk(p0, p1, p2);
	}

	private void gkukr(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gkukr
		this.gkukr(p0, p1, p2);
	}

	private void genio()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in genio
		this.genio();
	}

	private void plcdq(fxakl p0)
	{
		if (qgnex != null && 0 == 0)
		{
			p0.suudj(wbmti, qgnex, lkzdi);
		}
		else
		{
			p0.suudj(wbmti, lkzdi);
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in plcdq
		this.plcdq(p0);
	}
}
