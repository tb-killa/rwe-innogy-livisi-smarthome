using System.IO;
using Rebex.IO;

namespace onrkn;

internal class oyifz
{
	private bool wiuhc;

	private bool tkrmd;

	private TransferAction rjfvq;

	private FileSystemItem qlewt;

	private string ujlun;

	private string xpzsj;

	private long aealv;

	private long vmpfp;

	private int szusa;

	private int efkhz;

	private int abimm;

	private long bwrqg;

	private long qypre;

	private long vvvsd;

	private long itfdn;

	private object yynti;

	public bool wkbhi
	{
		get
		{
			return wiuhc;
		}
		private set
		{
			wiuhc = value;
		}
	}

	public bool lbgim
	{
		get
		{
			return tkrmd;
		}
		private set
		{
			tkrmd = value;
		}
	}

	public TransferAction uaage
	{
		get
		{
			return rjfvq;
		}
		private set
		{
			rjfvq = value;
		}
	}

	public FileSystemItem Item
	{
		get
		{
			return qlewt;
		}
		private set
		{
			qlewt = value;
		}
	}

	public string hlrzw
	{
		get
		{
			if (uaage != TransferAction.Uploading)
			{
				return vlzcc;
			}
			return unstq;
		}
	}

	public string vlzcc
	{
		get
		{
			return ujlun;
		}
		set
		{
			ujlun = value;
		}
	}

	public string unstq
	{
		get
		{
			return xpzsj;
		}
		set
		{
			xpzsj = value;
		}
	}

	public long ffdop
	{
		get
		{
			return aealv;
		}
		set
		{
			aealv = value;
		}
	}

	public long aqpch
	{
		get
		{
			return vmpfp;
		}
		set
		{
			vmpfp = value;
		}
	}

	public int mudko
	{
		get
		{
			return szusa;
		}
		set
		{
			szusa = value;
		}
	}

	public int mbsca
	{
		get
		{
			return efkhz;
		}
		set
		{
			efkhz = value;
		}
	}

	public int btlui
	{
		get
		{
			return abimm;
		}
		set
		{
			abimm = value;
		}
	}

	public long jcakx
	{
		get
		{
			return bwrqg;
		}
		set
		{
			bwrqg = value;
		}
	}

	public long naawe
	{
		get
		{
			return qypre;
		}
		set
		{
			qypre = value;
		}
	}

	public long cwtrc
	{
		get
		{
			return vvvsd;
		}
		set
		{
			vvvsd = value;
		}
	}

	public long unzmu
	{
		get
		{
			return itfdn;
		}
		set
		{
			itfdn = value;
		}
	}

	public object kqzlx
	{
		get
		{
			return yynti;
		}
		set
		{
			yynti = value;
		}
	}

	public bool zzboq => uaage == TransferAction.Uploading;

	public bool kapcn => uaage == TransferAction.Downloading;

	public bool tqzrz => uaage == TransferAction.Deleting;

	public bool ruedk => uaage == TransferAction.Listing;

	public oyifz(TransferAction action, bool isForSingleOperation, bool isLocking)
	{
		uaage = action;
		wkbhi = isForSingleOperation;
		lbgim = isLocking;
	}

	public oyifz mjzyz()
	{
		return (oyifz)MemberwiseClone();
	}

	public void ueijm(FileSystemItem p0, string p1, string p2)
	{
		Item = p0;
		unstq = p1;
		vlzcc = p2;
	}

	public void zrnyg(bool p0)
	{
		lbgim = p0;
	}

	public static oyifz tqvby(TransferAction p0, string p1, string p2, char[] p3)
	{
		oyifz oyifz2 = new oyifz(p0, isForSingleOperation: true, isLocking: true);
		oyifz2.mudko = 1;
		if (p0 == TransferAction.Uploading)
		{
			if (p2 == null || 1 == 0)
			{
				oyifz2.ueijm(sjgua.nbvqk(null, null, -1L), p1, p2);
			}
			else
			{
				oyifz2.ueijm(new LocalItem(p2), p1, p2);
			}
		}
		else
		{
			oyifz2.ueijm(sjgua.nbvqk(p1, qzxrw(p1, p3), -1L), p1, p2);
		}
		oyifz2.ffdop = oyifz2.Item.Length;
		return oyifz2;
	}

	public static oyifz hpczh(TransferAction p0, string p1, Stream p2, bool p3, char[] p4)
	{
		if (p2 == null || 1 == 0)
		{
			return null;
		}
		string text = vtdxm.kilhc(p2);
		oyifz oyifz2 = tqvby(p0, p1, text, p4);
		if (p0 == TransferAction.Uploading && (text == null || 1 == 0) && (!p3 || 1 == 0) && p2.CanSeek && 0 == 0)
		{
			((sjgua)oyifz2.Item).qofsg(p2.Length);
			oyifz2.ffdop = oyifz2.Item.Length;
		}
		return oyifz2;
	}

	public static oyifz rsydy(string p0, char[] p1)
	{
		oyifz oyifz2 = new oyifz(TransferAction.Listing, isForSingleOperation: true, isLocking: true);
		oyifz2.ueijm(sjgua.tzjir(p0, qzxrw(p0, p1)), p0, null);
		oyifz2.mudko = 1;
		return oyifz2;
	}

	public static oyifz lyrhs(string p0, char[] p1)
	{
		oyifz oyifz2 = new oyifz(TransferAction.Listing, isForSingleOperation: true, isLocking: true);
		oyifz2.ueijm(sjgua.tzjir(p0, qzxrw(p0, p1)), p0, null);
		return oyifz2;
	}

	public static oyifz evnig(bool p0)
	{
		return new oyifz((TransferAction)0, isForSingleOperation: true, !p0);
	}

	private static string qzxrw(string p0, char[] p1)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		int num = brgjd.pkosy(p0, p1);
		if (num < 0)
		{
			return p0;
		}
		return p0.Substring(num + 1);
	}
}
