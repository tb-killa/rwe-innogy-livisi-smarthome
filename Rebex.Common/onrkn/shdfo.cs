using System;

namespace onrkn;

internal abstract class shdfo : wcrlo
{
	public abstract xwtwn ellsg(string p0, string p1);

	public abstract eyqzi nxqay(string p0, rdvij p1, hqxly p2);

	public abstract void kosgg(string p0);

	public abstract void tkipb(string p0);

	public abstract void xptlv(string p0);

	public abstract vgycx wxxmm(string p0);

	public abstract void jcpei(string p0, string p1);

	public abstract void idbxb(string p0, string p1);

	public abstract void bwzap(string p0, vgycx p1);

	public abstract evdac tunjy();

	internal static void amrzo(rdvij p0, hqxly p1)
	{
		switch (p0)
		{
		default:
			throw new nfcev(fvjcl.cdaaf, "Invalid open mode.");
		case rdvij.mgjux:
		case rdvij.gucpw:
		case rdvij.hujjg:
		case rdvij.zsxxj:
		case rdvij.pcxdt:
			switch (p1)
			{
			default:
				throw new nfcev(fvjcl.cdaaf, "Invalid access mode.");
			case hqxly.xpmbl:
			case hqxly.oigov:
			case hqxly.qfdhh:
				break;
			}
			break;
		}
	}

	internal static Exception xdaty(fvjcl p0)
	{
		return new nfcev(p0);
	}

	internal static Exception ebteu(fvjcl p0, Exception p1)
	{
		return new nfcev(p0, p1);
	}

	internal static string dprqm(fvjcl p0)
	{
		return p0 switch
		{
			fvjcl.ockud => "File not found.", 
			fvjcl.nzuue => "Path not found.", 
			fvjcl.jlxwg => "Access denied.", 
			fvjcl.latcf => "Invalid handle.", 
			fvjcl.psrbn => "Seek fault.", 
			fvjcl.hznby => "Write fault.", 
			fvjcl.ovavv => "Read fault.", 
			fvjcl.xjlzv => "File is already exclusively open.", 
			fvjcl.xwbjx => "Not enough space.", 
			fvjcl.nadim => "Already exists.", 
			fvjcl.cdaaf => "Open failed.", 
			fvjcl.vmbsn => "Invalid path.", 
			fvjcl.ywelb => "Directory is not empty.", 
			fvjcl.sqidy => "Operation was canceled.", 
			_ => "Unspecified error.", 
		};
	}
}
