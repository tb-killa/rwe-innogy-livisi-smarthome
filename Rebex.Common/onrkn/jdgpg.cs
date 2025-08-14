using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using Rebex;
using Rebex.IO;

namespace onrkn;

internal abstract class jdgpg
{
	protected List<lspmt> tlffi = new List<lspmt>();

	protected List<lspmt> sorsy = new List<lspmt>();

	protected oyifz wabpu;

	protected char[] gilnp;

	protected char[] kzmbq;

	private vujad ajgyv;

	private string ivujo;

	private TransferMethod gjboh;

	private MoveMode wppsl;

	private LinkProcessingMode iarky;

	private ActionOnExistingFiles cmzqg;

	private int ataes;

	private char twmml;

	private char mdawq;

	private char dddme;

	private char glryk;

	private static char[] myqmx;

	protected char[] jeduz => eawxe(gilnp, kzmbq);

	protected char[] wqjmm => ohkfa(gilnp, kzmbq);

	protected bool dvdya => wabpu.zzboq;

	protected bool iobse => wabpu.kapcn;

	protected bool eppla
	{
		get
		{
			if (!wabpu.zzboq || 1 == 0)
			{
				return wabpu.kapcn;
			}
			return true;
		}
	}

	protected bool miosc => wabpu.tqzrz;

	protected bool fmmex => wabpu.ruedk;

	protected bool lnfwc => eppla;

	protected bool pugxh => gjboh == TransferMethod.Move;

	protected bool gvbam
	{
		get
		{
			if (gjboh == TransferMethod.Move)
			{
				return wppsl == MoveMode.All;
			}
			return false;
		}
	}

	protected virtual ItemDateTimes rpiab => ItemDateTimes.None;

	protected virtual TimeComparisonGranularity kxtgw => TimeComparisonGranularity.None;

	protected virtual ChecksumAlgorithm ewgpk => (ChecksumAlgorithm)0;

	protected virtual bool mzvyn => false;

	protected virtual bool htdyp => false;

	protected virtual bool uezgy => false;

	protected virtual bool uompv => true;

	protected virtual bool wwddo => true;

	protected virtual string siodr => null;

	protected abstract bool jrhky { get; }

	protected abstract char tfksd { get; }

	protected abstract char tdicj { get; }

	protected abstract char[] gejqh { get; }

	protected static char[] ddpbt
	{
		get
		{
			if (myqmx == null || 1 == 0)
			{
				myqmx = dahxy.cexjz();
			}
			return myqmx;
		}
	}

	protected abstract object cnzmj { get; }

	protected abstract bool jquba { get; }

	protected abstract bool fewso { get; }

	protected abstract bool ncoej { get; }

	protected virtual string psdzz(string p0)
	{
		return brgjd.edcru("Cannot upload file, because it matches exclude path ({0}).", p0);
	}

	private static omidq hpnkx(ActionOnExistingFiles p0, bool p1, bool p2)
	{
		switch (p0)
		{
		case ActionOnExistingFiles.SkipAll:
			return omidq.dgkzq;
		case ActionOnExistingFiles.OverwriteAll:
			return omidq.nivzg;
		case ActionOnExistingFiles.OverwriteOlder:
			return omidq.smbdu;
		case ActionOnExistingFiles.OverwriteDifferentSize:
			return omidq.nwoav;
		case ActionOnExistingFiles.OverwriteDifferentChecksum:
			return omidq.xrmda;
		case ActionOnExistingFiles.ResumeIfPossible:
			if (!p1 || 1 == 0)
			{
				return omidq.ytvbw;
			}
			if (!p2 || 1 == 0)
			{
				return omidq.dgkzq;
			}
			return omidq.zbfno;
		case ActionOnExistingFiles.Rename:
			return omidq.cbxuc;
		case ActionOnExistingFiles.ThrowException:
			return omidq.ytvbw;
		default:
			throw new ArgumentException("Unknown action on existing files.", "existingFileMode");
		}
	}

	private static omidq ckpje(LinkProcessingMode p0)
	{
		return p0 switch
		{
			LinkProcessingMode.FollowLinks => omidq.bnwfa, 
			LinkProcessingMode.SkipLinks => omidq.dgkzq, 
			LinkProcessingMode.ThrowExceptionOnLinks => omidq.ytvbw, 
			_ => throw new ArgumentException("Unknown action on links.", "actionOnLinks"), 
		};
	}

	protected bool mbcdc(bool p0)
	{
		return dvdya == p0;
	}

	protected bool wtjnx(bool p0)
	{
		return dvdya != p0;
	}

	protected T eawxe<T>(T p0, T p1)
	{
		if (!dvdya || 1 == 0)
		{
			return p1;
		}
		return p0;
	}

	protected T ohkfa<T>(T p0, T p1)
	{
		if (!dvdya || 1 == 0)
		{
			return p0;
		}
		return p1;
	}

	protected string clecl(string p0, string p1)
	{
		return eawxe(p0, p1);
	}

	protected string lyyco(string p0, string p1)
	{
		return ohkfa(p0, p1);
	}

	protected ijwiq davms(ijwiq p0, ijwiq p1)
	{
		return eawxe(p0, p1);
	}

	protected ijwiq zrnkw(ijwiq p0, ijwiq p1)
	{
		return ohkfa(p0, p1);
	}

	protected virtual void fcegg()
	{
		nvmgb("Multi-file operation started.");
	}

	protected virtual void mnnve()
	{
		if (wabpu.uaage != TransferAction.Listing)
		{
			nnyll(aqicq.ivywy);
		}
		nvmgb("Multi-file operation done.");
	}

	private string fdqwo(string p0, bool p1)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			p0 = ".";
		}
		if (mbcdc(p1) && 0 == 0)
		{
			p0 = vtdxm.smjda(p0);
			return dahxy.wysxc(p0, jeduz, p2: true, htdyp);
		}
		return poizd(p0);
	}

	protected static string lkibq(string p0, char[] p1, char p2, bool p3)
	{
		string text = p0.TrimEnd(p1);
		bool flag = p0.Length > text.Length;
		int num = brgjd.pkosy(text, p1);
		p0 = ((num >= 0) ? text.Substring(num + 1) : text);
		if (((p3 ? true : false) || text.Length == 0 || 1 == 0) && flag && 0 == 0)
		{
			return p0 + p2;
		}
		return p0;
	}

	private static string typsk(string p0, char[] p1, string p2)
	{
		p0 = p0.TrimEnd(p1);
		int num = brgjd.pkosy(p0, p1);
		if (num >= 0)
		{
			return p0.Substring(0, num + 1) + p2;
		}
		return p2;
	}

	private string tqqlb(lspmt p0)
	{
		string rvimb = p0.rvimb;
		hyygw migcp = p0.migcp;
		string text;
		if (migcp.mkziv.Flatten && 0 == 0)
		{
			if (p0.Item.IsDirectory && 0 == 0)
			{
				return migcp.hlzcy;
			}
			text = lkibq(rvimb, gilnp, dddme, p3: false);
		}
		else
		{
			if (rvimb.Length <= migcp.dfgud.Length)
			{
				return migcp.hlzcy;
			}
			if (!rvimb.StartsWith(migcp.dfgud, StringComparison.OrdinalIgnoreCase) || 1 == 0)
			{
				throw new InvalidOperationException(brgjd.edcru("Unexpected path encountered ({0} should starts with {1}).", rvimb, migcp.dfgud));
			}
			text = rvimb.Substring(migcp.dfgud.Length);
		}
		if (text.Length == 0 || 1 == 0)
		{
			return migcp.hlzcy;
		}
		bool flag = text[0] == twmml || text[0] == mdawq;
		if ((text[0] == dddme || text[0] == glryk) && 0 == 0 && (!flag || 1 == 0))
		{
			return migcp.hlzcy.TrimEnd(kzmbq) + dddme + aemed(text);
		}
		return ozfub(migcp.hlzcy, aemed(text), p2: false);
	}

	private string aemed(string p0)
	{
		if (twmml != dddme)
		{
			p0 = p0.Replace(twmml, dddme);
		}
		if (mdawq != dddme && twmml != mdawq)
		{
			p0 = p0.Replace(mdawq, dddme);
		}
		return p0;
	}

	protected string ozfub(string p0, string p1, bool p2)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			return p1;
		}
		char c = ((p2 ? true : false) ? twmml : dddme);
		char c2 = ((p2 ? true : false) ? mdawq : glryk);
		bool flag = p0[p0.Length - 1] == c || p0[p0.Length - 1] == c2;
		bool flag2 = p1.Length > 0 && (p1[0] == c || p1[0] == c2);
		if ((!flag2 || 1 == 0) && (!flag || 1 == 0))
		{
			return p0 + c + p1;
		}
		if (flag2 && 0 == 0 && flag && 0 == 0)
		{
			return p0.TrimEnd(c, c2) + p1;
		}
		return p0 + p1;
	}

	protected bool enofc(string p0)
	{
		if (p0.Length > 0)
		{
			return p0.IndexOfAny((dvdya ? true : false) ? gejqh : ddpbt) < 0;
		}
		return false;
	}

	protected string cjqpc(ijwiq p0, string p1)
	{
		if (p1.IndexOfAny(wqjmm) < 0)
		{
			return p1;
		}
		if (p0 != null && 0 == 0 && p0.Path.Trim(wqjmm) == p1.Trim(wqjmm) && 0 == 0)
		{
			return p0.Name;
		}
		return lkibq(p1, wqjmm, tfksd, p3: false);
	}

	protected void ocrqf(string p0, string p1, out string p2, out string p3)
	{
		if (p1.IndexOfAny(gilnp) < 0)
		{
			p3 = p1;
			p2 = ozfub(p0, p3, p2: true);
			return;
		}
		int num = p1.IndexOf(p0, StringComparison.OrdinalIgnoreCase);
		if (num >= 0 && p1.Length > num + p0.Length)
		{
			p2 = p1;
			p3 = p2.Substring(num + p0.Length + 1);
		}
		else
		{
			p3 = p1;
			p2 = ozfub(p0, p3, p2: true);
		}
	}

	protected void qqjhq(lspmt p0, ijwiq p1)
	{
		string text = p0.ovfjk;
		if (text != null && 0 == 0)
		{
			text = text + twmml + p1.Name;
		}
		else if (p1.IsLink && 0 == 0)
		{
			text = p1.Path;
		}
		tlffi.Add(new lspmt(p1, p0.migcp, text));
	}

	private void vkbrc(ijwiq p0)
	{
		using List<hyygw>.Enumerator enumerator = ajgyv.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			hyygw current = enumerator.Current;
			nvmgb("Normalizing source path ('{0}').", current.dfgud);
			current.dfgud = fdqwo(current.dfgud, p1: true);
			nvmgb("Checking source path ('{0}').", current.dfgud);
			if (dvdya && 0 == 0 && siodr != null && 0 == 0 && siodr.Equals(current.dfgud, StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				throw new InvalidOperationException(psdzz(current.dfgud));
			}
			ijwiq ijwiq2 = qvwqe(current.dfgud, p1: true, p2: false, p0);
			if (ijwiq2 == null || 1 == 0)
			{
				throw uvmln(brgjd.edcru("Could not find source path ('{0}').", current.dfgud));
			}
			if ((ijwiq2.IsDirectory ? true : false) || (ijwiq2.IsLink ? true : false) || osywg(ijwiq2.Item, p1: true))
			{
				tlffi.Add(new lspmt(ijwiq2, current, (ijwiq2.IsLink ? true : false) ? current.dfgud : null));
				continue;
			}
			throw mppwr(brgjd.edcru("Source path is not a file or directory ('{0}').", current.dfgud));
		}
	}

	private void cybzb()
	{
		nvmgb("Normalizing target path ('{0}').", ivujo);
		ivujo = fdqwo(ivujo, p1: false);
		nvmgb("Checking target path ('{0}').", ivujo);
		string text = ivujo.TrimEnd(kzmbq);
		if (text.Length == 0 || false || text.Equals(".", StringComparison.Ordinal))
		{
			return;
		}
		ijwiq ijwiq2 = feknh(ivujo, p1: false, p2: true);
		if (ijwiq2 == null || 1 == 0)
		{
			if ((dvdya && 0 == 0 && (uompv ? true : false)) || ((!dvdya || 1 == 0) && wwddo))
			{
				throw mppwr(brgjd.edcru("Could not find target directory ('{0}').", ivujo));
			}
		}
		else if (!ijwiq2.IsDirectory || 1 == 0)
		{
			throw mppwr(brgjd.edcru("Target path is not a directory ('{0}').", ivujo));
		}
	}

	private void zusjs()
	{
		string rvimb = sorsy[ataes].rvimb;
		nvmgb("Skipping directory ('{0}').", rvimb);
		ataes--;
		while (ataes >= 0)
		{
			lspmt lspmt2 = sorsy[ataes];
			if (lspmt2.Item.IsFile && 0 == 0)
			{
				wabpu.mbsca++;
				wabpu.cwtrc += lspmt2.Item.Length;
			}
			else if (!qxepk(lspmt2.rvimb, rvimb))
			{
				break;
			}
			ataes--;
		}
		ataes++;
	}

	private bool qxepk(string p0, string p1)
	{
		return p0.StartsWith(p1, StringComparison.OrdinalIgnoreCase);
	}

	protected lspmt puxbp()
	{
		if (ataes == 0 || 1 == 0)
		{
			return null;
		}
		return sorsy[ataes - 1];
	}

	private void dxyyj()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_00d6;
		IL_000c:
		ijwiq ijwiq2 = sorsy[num].Item;
		if (ijwiq2.vvhki && 0 == 0)
		{
			ijwiq2 = fjtch(sorsy[num].Item.Path, null);
		}
		if (ijwiq2 == null || 1 == 0)
		{
			throw mppwr(brgjd.edcru("Cannot determine whether a {1} that was previously found exists ('{0}').", sorsy[num].Item.Path, (sorsy[num].Item.IsFile ? true : false) ? "file" : "directory"));
		}
		sorsy[num].pgbfk(ijwiq2);
		num++;
		goto IL_00d6;
		IL_00d6:
		if (num >= sorsy.Count)
		{
			return;
		}
		goto IL_000c;
	}

	protected void tgnen(TransferAction p0, vujad p1, string p2, TransferMethod p3, MoveMode p4, LinkProcessingMode p5, ActionOnExistingFiles p6, ijwiq p7)
	{
		nvmgb("Executing multi-file operation: {0}, source = {1}, target = '{2}', TransferMethod.{3}, MoveMode.{4}, LinkProcessingMode.{5}, ActionOnExistingFiles.{6}.", p0, (p1.Count != 1) ? brgjd.edcru("[{0} paths]", p1.Count) : brgjd.edcru("'{0}'", p1[0].dfgud), p2, p3, p4, p5, p6);
		wabpu = new oyifz(p0, isForSingleOperation: false, isLocking: false);
		wabpu.kqzlx = cnzmj;
		ajgyv = p1;
		ivujo = p2;
		gjboh = p3;
		wppsl = p4;
		iarky = p5;
		cmzqg = p6;
		if (dvdya && 0 == 0)
		{
			twmml = dahxy.ymzqe;
			mdawq = dahxy.nmnrp;
			dddme = tfksd;
			glryk = tdicj;
		}
		else
		{
			twmml = tfksd;
			mdawq = tdicj;
			dddme = dahxy.ymzqe;
			glryk = dahxy.nmnrp;
		}
		if (twmml == mdawq)
		{
			gilnp = new char[1] { twmml };
		}
		else
		{
			gilnp = new char[2] { twmml, mdawq };
		}
		if (dddme == glryk)
		{
			kzmbq = new char[1] { dddme };
		}
		else
		{
			kzmbq = new char[2] { dddme, glryk };
		}
		if (lnfwc && 0 == 0)
		{
			if (dvdya && 0 == 0)
			{
				vkbrc(p7);
				cybzb();
			}
			else
			{
				cybzb();
				vkbrc(p7);
			}
			using List<hyygw>.Enumerator enumerator = ajgyv.GetEnumerator();
			while (enumerator.MoveNext() ? true : false)
			{
				hyygw current = enumerator.Current;
				if (current.zgxzt && 0 == 0)
				{
					string text = lkibq(current.dfgud, gilnp, dddme, p3: true);
					if (!enofc(text) || 1 == 0)
					{
						current.hlzcy = ivujo + dddme + text;
					}
					else
					{
						current.hlzcy = ozfub(ivujo, text, p2: false);
					}
				}
				else
				{
					current.hlzcy = ivujo;
				}
			}
		}
		else
		{
			vkbrc(p7);
		}
		fcegg();
		ungxg();
		switch (p0)
		{
		case TransferAction.Uploading:
		case TransferAction.Downloading:
		case TransferAction.Deleting:
			rfrjj();
			break;
		case TransferAction.Listing:
			dxyyj();
			break;
		default:
			throw new ArgumentException("Invalid MultiFileOperation.", "method");
		}
		mnnve();
	}

	private void ungxg()
	{
		if (tlffi.Count == 0 || 1 == 0)
		{
			return;
		}
		wabpu.ueijm(tlffi[0].Item.Item, lyyco(ajgyv[0].dfgud, ajgyv[0].hlzcy), clecl(ajgyv[0].dfgud, ajgyv[0].hlzcy));
		oyifz oyifz2 = wabpu.mjzyz();
		nnyll(aqicq.hcxlr);
		aqicq p = aqicq.bnawl;
		try
		{
			while (tlffi.Count > 0)
			{
				hicrt();
				lspmt lspmt2 = tlffi[tlffi.Count - 1];
				if (lspmt2.nqhex && 0 == 0)
				{
					if (lspmt2.Item.IsDirectory && 0 == 0 && (!lspmt2.blvgv || 1 == 0))
					{
						if (!lspmt2.iakqs || 1 == 0)
						{
							wabpu.ueijm(lspmt2.Item.Item, lyyco(lspmt2.rvimb, lspmt2.nuttj), clecl(lspmt2.rvimb, lspmt2.nuttj));
							nnyll(aqicq.nzwwg);
						}
						if (yxple(lspmt2) && 0 == 0)
						{
							sorsy.Add(lspmt2);
						}
					}
					tlffi.RemoveAt(tlffi.Count - 1);
					continue;
				}
				lspmt2.nqhex = true;
				lspmt2.tnfhc = sorsy.Count;
				if (lnfwc && 0 == 0)
				{
					lspmt2.nuttj = tqqlb(lspmt2);
				}
				if (!lspmt2.Item.IsLink || 1 == 0)
				{
					if ((!lspmt2.Item.IsFile || 1 == 0) && lspmt2.Item.IsDirectory && 0 == 0 && (!lspmt2.lcogx(gilnp, FileSetMatchMode.TraverseDirectory) || 1 == 0))
					{
						lspmt2.iakqs = true;
						continue;
					}
				}
				else if (!lspmt2.Item.IsFile || 1 == 0)
				{
					if (lspmt2.Item.IsDirectory && 0 == 0)
					{
						if ((!lspmt2.lcogx(gilnp, FileSetMatchMode.TraverseDirectory) || 1 == 0) && (!yxple(lspmt2) || 1 == 0))
						{
							lspmt2.blvgv = true;
							continue;
						}
					}
					else if ((!lspmt2.lcogx(gilnp, FileSetMatchMode.MatchFile) || 1 == 0) && (!lspmt2.lcogx(gilnp, FileSetMatchMode.TraverseDirectory) || 1 == 0) && (!yxple(lspmt2) || 1 == 0))
					{
						lspmt2.blvgv = true;
						continue;
					}
				}
				wkihn(lspmt2);
			}
			p = aqicq.miduf;
		}
		finally
		{
			wabpu.ueijm(oyifz2.Item, oyifz2.unstq, oyifz2.vlzcc);
			nnyll(p);
		}
	}

	private bool yxple(lspmt p0)
	{
		if (p0.rvimb.Length <= p0.migcp.dfgud.Length)
		{
			return p0.migcp.zgxzt;
		}
		if (miosc && 0 == 0)
		{
			return p0.lcogx(gilnp, FileSetMatchMode.MatchDirectory);
		}
		bool flag = p0.tnfhc != sorsy.Count;
		if ((p0.migcp.mkziv.ContainingDirectoriesIncluded ? true : false) || eppla)
		{
			if (!flag || 1 == 0)
			{
				if (p0.migcp.mkziv.EmptyDirectoriesIncluded && 0 == 0)
				{
					return p0.lcogx(gilnp, FileSetMatchMode.MatchDirectory);
				}
				return false;
			}
			return true;
		}
		if ((flag ? true : false) || p0.migcp.mkziv.EmptyDirectoriesIncluded)
		{
			return p0.lcogx(gilnp, FileSetMatchMode.MatchDirectory);
		}
		return false;
	}

	private void wkihn(lspmt p0)
	{
		wabpu.ueijm(p0.Item.Item, lyyco(p0.rvimb, p0.nuttj), clecl(p0.rvimb, p0.nuttj));
		if ((dvdya ? true : false) || mzvyn)
		{
			try
			{
				ijwiq ijwiq2 = qvwqe(p0.rvimb, p1: true, p2: false, p0.Item);
				if (ijwiq2 == null || 1 == 0)
				{
					throw new pwzds(null, null);
				}
				p0.pgbfk(ijwiq2);
				wabpu.ueijm(ijwiq2.Item, wabpu.unstq, wabpu.vlzcc);
			}
			catch (pwzds pwzds2)
			{
				string p1 = brgjd.edcru("Cannot determine whether a {1} that was previously found exists ('{0}').", p0.rvimb, (p0.Item.IsLink ? true : false) ? "link" : ((p0.Item.IsDirectory ? true : false) ? "directory" : "file"));
				wllpt(pwzds2.InnerException, p1, (p0.Item.IsLink ? true : false) ? TransferProblemType.CannotFindLink : ((p0.Item.IsDirectory ? true : false) ? TransferProblemType.CannotFindDirectory : TransferProblemType.CannotFindFile), zrnkw(p0.Item, null), davms(p0.Item, null), omidq.nvxgc, out var p2);
				if (p2 == omidq.dgkzq)
				{
					p0.blvgv = true;
				}
				if (p2 == omidq.zgjbt)
				{
					p0.nqhex = false;
				}
				return;
			}
		}
		if (p0.Item.IsLink && 0 == 0)
		{
			dospj(p0, omidq.ohxrr, out var p3);
			switch (p3)
			{
			case omidq.dgkzq:
				p0.blvgv = true;
				return;
			case omidq.zgjbt:
				p0.nqhex = false;
				return;
			}
			if (p0.Item.IsDirectory && 0 == 0 && (!p0.lcogx(gilnp, FileSetMatchMode.TraverseDirectory) || 1 == 0))
			{
				p0.iakqs = true;
				return;
			}
		}
		if (p0.Item.IsDirectory && 0 == 0)
		{
			if (p0.migcp.mkziv.Flatten && 0 == 0)
			{
				p0.nuttj = p0.migcp.hlzcy;
				wabpu.ueijm(wabpu.Item, lyyco(p0.rvimb, p0.nuttj), clecl(p0.rvimb, p0.nuttj));
			}
			if (!miosc || false || !p0.Item.IsLink)
			{
				bool flag = false;
				int count = tlffi.Count;
				try
				{
					nvmgb("Retrieving items of directory ('{0}').", p0.rvimb);
					nnyll(aqicq.brcpb);
					if (dvdya && 0 == 0)
					{
						umxig(p0, p1: true);
					}
					else
					{
						shnkg(p0, p1: true);
					}
					flag = true;
					return;
				}
				catch (pwzds pwzds3)
				{
					wllpt(pwzds3.InnerException, pwzds3.Message, TransferProblemType.CannotReadFromDirectory, zrnkw(p0.Item, null), davms(p0.Item, null), omidq.nvxgc, out var p4);
					if (count != tlffi.Count)
					{
						tlffi.RemoveRange(count, tlffi.Count - count);
					}
					if (p4 == omidq.zgjbt)
					{
						p0.nqhex = false;
					}
					else
					{
						nnyll(aqicq.nwarc);
					}
					return;
				}
				finally
				{
					if (!flag || 1 == 0)
					{
						nnyll(aqicq.cagtv);
					}
				}
			}
			wabpu.mudko++;
		}
		else if (osywg(p0.Item.Item, p1: true) && 0 == 0)
		{
			if (p0.rvimb.Length <= p0.migcp.dfgud.Length && (!p0.migcp.begld || 1 == 0))
			{
				throw mppwr(brgjd.edcru("Source path is a file, not a directory ('{0}').", p0.rvimb));
			}
			sorsy.Add(p0);
			wabpu.jcakx += p0.Item.Length;
			wabpu.mudko++;
		}
		else
		{
			Exception p5 = tpelv(brgjd.edcru("'{0}' is not a file or directory.", p0.rvimb), null, TransferProblemType.NotFileOrDirectory);
			bksrc(p5, TransferProblemType.NotFileOrDirectory, zrnkw(p0.Item, null), davms(p0.Item, null), omidq.ytvbw, omidq.dgkzq, out var _);
		}
	}

	private void dospj(lspmt p0, omidq p1, out omidq p2)
	{
		nvmgb("Resolving link ('{0}').", p0.rvimb);
		omidq omidq2 = ckpje(iarky);
		if (iobse && 0 == 0 && (!jrhky || 1 == 0))
		{
			p1 &= ~omidq.bnwfa;
			if (omidq2 == omidq.bnwfa)
			{
				omidq2 = omidq.ytvbw;
			}
		}
		Exception p3 = tpelv(brgjd.edcru("Link detected ('{0}').", p0.rvimb), null, TransferProblemType.LinkDetected);
		bksrc(p3, TransferProblemType.LinkDetected, zrnkw(p0.Item, null), davms(p0.Item, null), omidq2, p1, out p2);
		if (p2 != omidq.bnwfa)
		{
			return;
		}
		p0.okqrv(gilnp);
		if (p0.sviua(gilnp) && 0 == 0)
		{
			ocqxj(p0, p1, out p2);
			if (p2 != omidq.bnwfa)
			{
				return;
			}
		}
		ijwiq ijwiq2 = p0.Item;
		if ((!dvdya || 1 == 0) && jrhky && 0 == 0 && (!ijwiq2.IsFile || 1 == 0) && (!ijwiq2.IsDirectory || 1 == 0))
		{
			try
			{
				ijwiq2 = gczcr(p0.rvimb, p1: true, ijwiq2);
			}
			catch (pwzds pwzds2)
			{
				if (!miosc)
				{
					string p4 = brgjd.edcru("Cannot resolve link ('{0}'). {1}", p0.rvimb, pwzds2.Message);
					wllpt(pwzds2.InnerException, p4, TransferProblemType.CannotResolveLink, zrnkw(p0.Item, null), davms(p0.Item, null), p1 & omidq.nvxgc, out p2);
					return;
				}
				ijwiq2 = new ijwiq(sjgua.nbvqk(ijwiq2.Path, ijwiq2.Name, 0L), ijwiq2.Name, ijwiq2.Path);
			}
		}
		ijwiq2.nnyfv = p0.Item.Item;
		p0.pgbfk(ijwiq2);
		wabpu.ueijm(ijwiq2.Item, wabpu.unstq, wabpu.vlzcc);
		if (ijwiq2.IsDirectory && 0 == 0)
		{
			nvmgb("Link is directory ('{0}').", p0.rvimb);
			return;
		}
		if (ijwiq2.IsFile && 0 == 0)
		{
			nvmgb("Link is file ('{0}').", p0.rvimb);
			if (!p0.lcogx(gilnp, FileSetMatchMode.MatchFile) || 1 == 0)
			{
				p2 = omidq.dgkzq;
			}
			return;
		}
		if (uezgy && 0 == 0 && (!dvdya || 1 == 0))
		{
			nvmgb("Link is not resolved - considering it as file ('{0}').", p0.rvimb);
			if (!p0.lcogx(gilnp, FileSetMatchMode.MatchFile) || 1 == 0)
			{
				p2 = omidq.dgkzq;
			}
			return;
		}
		if (ijwiq2.IsLink && 0 == 0)
		{
			nvmgb("Link resolved to another link ('{0}').", p0.rvimb);
		}
		else
		{
			nvmgb("Link is not a file or directory ('{0}').", p0.rvimb);
		}
		p3 = tpelv(brgjd.edcru("Target of the link is not a file or directory ('{0}').", p0.rvimb), null, TransferProblemType.NotFileOrDirectory);
		bksrc(p3, TransferProblemType.NotFileOrDirectory, zrnkw(p0.Item, null), davms(p0.Item, null), omidq.ytvbw, p1 & omidq.nvxgc, out p2);
	}

	protected bool osywg(FileSystemItem p0, bool p1)
	{
		if (p0.IsFile && 0 == 0)
		{
			return true;
		}
		if (p0.IsDirectory && 0 == 0)
		{
			return false;
		}
		if (wtjnx(p1) && 0 == 0)
		{
			if (jrhky && 0 == 0 && p0.IsLink && 0 == 0)
			{
				return false;
			}
			return uezgy;
		}
		return false;
	}

	private void rfrjj()
	{
		nvmgb("{0} hierarchy started.", (miosc ? true : false) ? "Deleting" : "Transferring");
		oyifz oyifz2 = wabpu.mjzyz();
		try
		{
			List<lspmt> list = new List<lspmt>();
			for (ataes = sorsy.Count - 1; ataes >= 0; ataes--)
			{
				hicrt();
				lspmt lspmt2 = sorsy[ataes];
				int num = list.Count - 1;
				while (num >= 0 && (!qxepk(lspmt2.rvimb, list[num].rvimb) || 1 == 0))
				{
					zazxj(list[num]);
					list.RemoveAt(num);
					num--;
				}
				bool flag = true;
				if (lspmt2.Item.IsDirectory && 0 == 0)
				{
					tljfy(lspmt2);
					if ((!miosc || 1 == 0) && (!lspmt2.migcp.mkziv.Flatten || 1 == 0))
					{
						flag = oqkeo(lspmt2);
					}
					if ((miosc ? true : false) || (flag && 0 == 0 && gvbam))
					{
						list.Add(lspmt2);
					}
				}
				else
				{
					if (!miosc || 1 == 0)
					{
						flag = urpwz(lspmt2);
					}
					if ((miosc ? true : false) || (flag && 0 == 0 && pugxh))
					{
						zazxj(lspmt2);
					}
				}
			}
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				zazxj(list[num2]);
			}
		}
		finally
		{
			wabpu.ueijm(oyifz2.Item, oyifz2.unstq, oyifz2.vlzcc);
		}
	}

	private void tljfy(lspmt p0)
	{
		string rvimb = p0.rvimb;
		string nuttj = p0.nuttj;
		string p1 = clecl(rvimb, nuttj);
		string p2 = lyyco(rvimb, nuttj);
		wabpu.ueijm(p0.Item.Item, p2, p1);
		nnyll(aqicq.yynwh);
	}

	private bool oqkeo(lspmt p0)
	{
		bool flag = true;
		aqicq p1 = aqicq.hxwkq;
		try
		{
			kjted(p0: false, p0, ref p1);
			if (p1 == aqicq.lovez)
			{
				zusjs();
				return false;
			}
			while (true)
			{
				hicrt();
				try
				{
					ijwiq ijwiq2;
					try
					{
						ijwiq2 = feknh(wabpu.hlrzw, p1: false, p2: true);
					}
					catch (pwzds pwzds2)
					{
						throw new pwzds(brgjd.edcru("Cannot retrieve information of target path ('{0}').", wabpu.hlrzw), pwzds2.InnerException);
					}
					if (ijwiq2 == null || 1 == 0)
					{
						if (dvdya && 0 == 0)
						{
							cfjap(wabpu.unstq);
						}
						else
						{
							jgayu(wabpu.vlzcc);
						}
						p1 = aqicq.axynn;
						if (p1 != 0)
						{
							goto IL_012d;
						}
					}
					if (ijwiq2.IsDirectory && 0 == 0)
					{
						flag = false;
						if (!flag)
						{
							goto IL_012d;
						}
					}
					Exception inner = tpelv(brgjd.edcru("File already exists ('{0}').", wabpu.hlrzw), null, TransferProblemType.FileExists);
					string message = brgjd.edcru("Cannot create a directory because a file with the same name already exists ('{0}').", wabpu.hlrzw);
					throw new pwzds(message, inner);
					IL_012d:
					return true;
				}
				catch (pwzds pwzds3)
				{
					omidq p2;
					oisbg oisbg2 = wllpt(pwzds3.InnerException, pwzds3.Message, TransferProblemType.CannotCreateDirectory, zrnkw(p0.Item, null), davms(p0.Item, null), omidq.ufghs, out p2);
					switch (p2)
					{
					case omidq.zgjbt:
						break;
					case omidq.cbxuc:
					{
						string text = typsk(wabpu.hlrzw, kzmbq, oisbg2.wiqoh);
						if (dvdya && 0 == 0)
						{
							wabpu.unstq = text;
						}
						else
						{
							wabpu.vlzcc = text;
						}
						break;
					}
					default:
						p1 = aqicq.lovez;
						zusjs();
						return false;
					}
				}
			}
		}
		finally
		{
			if (flag && 0 == 0)
			{
				nnyll(p1);
			}
		}
	}

	private bool urpwz(lspmt p0)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		wabpu.ueijm(p0.Item.Item, lyyco(p0.rvimb, p0.nuttj), clecl(p0.rvimb, p0.nuttj));
		nvmgb("Processing file to transfer ('{0}').", p0.rvimb);
		nnyll(aqicq.yrhed);
		aqicq p1 = aqicq.zarny;
		try
		{
			kjted(p0: true, p0, ref p1);
			if (p1 == aqicq.qjuok)
			{
				return false;
			}
			ijwiq ijwiq2;
			omidq p2;
			while (true)
			{
				hicrt();
				if ((dvdya ? true : false) || mzvyn)
				{
					try
					{
						ijwiq2 = qvwqe(p0.rvimb, p1: true, p2: true, p0.Item);
						if (ijwiq2 == null || 1 == 0)
						{
							throw new pwzds(null, null);
						}
						if (ijwiq2.IsDirectory && 0 == 0)
						{
							throw tpelv("File previously found is a directory now.", null, TransferProblemType.CannotFindFile);
						}
						if (ijwiq2.Length != p0.Item.Length)
						{
							wabpu.jcakx += ijwiq2.Length - p0.Item.Length;
						}
						p0.pgbfk(ijwiq2);
						wabpu.ueijm(ijwiq2.Item, wabpu.unstq, wabpu.vlzcc);
					}
					catch (pwzds pwzds2)
					{
						wllpt(p1: (pwzds2.InnerException != null) ? brgjd.edcru("Cannot retrieve information of source file ('{0}').", p0.rvimb) : brgjd.edcru("Cannot determine whether a file that was previously found exists ('{0}').", p0.rvimb), p0: pwzds2.InnerException, p2: TransferProblemType.CannotFindFile, p3: zrnkw(p0.Item, null), p4: davms(p0.Item, null), p5: omidq.nvxgc, p6: out p2);
						if (p2 == omidq.zgjbt)
						{
							continue;
						}
						p1 = aqicq.qjuok;
						return false;
					}
				}
				else
				{
					ijwiq2 = p0.Item;
				}
				break;
			}
			ijwiq ijwiq3;
			ijwiq ijwiq4;
			ijwiq ijwiq5;
			while (true)
			{
				hicrt();
				nvmgb("Checking existence of target item ('{0}').", wabpu.hlrzw);
				try
				{
					ijwiq3 = feknh(wabpu.hlrzw, p1: false, p2: true);
				}
				catch (pwzds pwzds3)
				{
					wllpt(pwzds3.InnerException, brgjd.edcru("Cannot retrieve information of target path ('{0}').", wabpu.hlrzw), TransferProblemType.CannotTransferFile, zrnkw(p0.Item, null), davms(p0.Item, null), omidq.nvxgc, out p2);
					if (p2 == omidq.zgjbt)
					{
						continue;
					}
					p1 = aqicq.qjuok;
					return false;
				}
				ijwiq4 = davms(ijwiq2, ijwiq3);
				ijwiq5 = zrnkw(ijwiq2, ijwiq3);
				if (ijwiq3 == null)
				{
					break;
				}
				Exception ex = tpelv(brgjd.edcru("{1} with the same name already exists ('{0}').", wabpu.hlrzw, (ijwiq3.IsDirectory ? true : false) ? "Directory" : "File"), null, TransferProblemType.FileExists);
				bool isFile = ijwiq3.IsFile;
				bool flag4 = isFile && 0 == 0 && ijwiq2.Length > ijwiq3.Length;
				omidq omidq2 = omidq.ufghs;
				if (flag4 && 0 == 0)
				{
					omidq2 |= omidq.zbfno;
				}
				if (ijwiq3.IsFile && 0 == 0)
				{
					omidq2 |= omidq.btzph | omidq.masrh;
				}
				omidq omidq3 = hpnkx(cmzqg, isFile, flag4);
				if ((omidq2 & omidq3) == 0 || 1 == 0)
				{
					omidq3 = omidq.ytvbw;
				}
				oisbg oisbg2 = bksrc(ex, TransferProblemType.FileExists, ijwiq5, ijwiq4, omidq3, omidq2, out p2);
				while (true)
				{
					switch (p2)
					{
					case omidq.zgjbt:
						break;
					case omidq.dgkzq:
						p1 = aqicq.qjuok;
						return false;
					case omidq.masrh:
						flag = true;
						if (flag)
						{
							goto end_IL_0207;
						}
						goto case omidq.nivzg;
					case omidq.nivzg:
						flag3 = true;
						if (flag3)
						{
							goto end_IL_0207;
						}
						goto case omidq.smbdu;
					case omidq.smbdu:
					{
						int? num = dahxy.movxk(ijwiq2.LastWriteTime, ijwiq3.LastWriteTime, kxtgw);
						if (num.GetValueOrDefault() <= 0 && num.HasValue && 0 == 0)
						{
							p1 = aqicq.qjuok;
							return false;
						}
						flag3 = true;
						if (flag3)
						{
							goto end_IL_0207;
						}
						goto case omidq.nwoav;
					}
					case omidq.nwoav:
						if (ijwiq2.Length == ijwiq3.Length)
						{
							p1 = aqicq.qjuok;
							return false;
						}
						flag3 = true;
						if (flag3)
						{
							goto end_IL_0207;
						}
						goto case omidq.xrmda;
					case omidq.xrmda:
						try
						{
							if (ijwiq2.Length == ijwiq3.Length && dgxbt(ijwiq4, ijwiq5) && 0 == 0)
							{
								p1 = aqicq.qjuok;
								return false;
							}
							flag3 = true;
						}
						catch (pwzds pwzds4)
						{
							Exception ex2 = pwzds4.InnerException;
							if (ex2 == null || 1 == 0)
							{
								ex2 = ex;
							}
							wllpt(ex2, pwzds4.Message, TransferProblemType.CannotCalculateChecksum, ijwiq5, ijwiq4, omidq2, out p2);
							continue;
						}
						goto end_IL_0207;
					case omidq.cbxuc:
						goto IL_0522;
					case omidq.zbfno:
						flag2 = true;
						if (flag2)
						{
							goto end_IL_0207;
						}
						goto default;
					default:
						throw new InvalidOperationException(brgjd.edcru("Selected action is not allowed ('{0}').", p2));
					}
					break;
				}
				continue;
				IL_0522:
				string text = typsk(wabpu.hlrzw, kzmbq, oisbg2.wiqoh);
				if (dvdya && 0 == 0)
				{
					wabpu.unstq = text;
				}
				else
				{
					wabpu.vlzcc = text;
				}
				continue;
				end_IL_0207:
				break;
			}
			p1 = gtlan(p0.rvimb, wabpu.hlrzw, ijwiq2, ijwiq3, ijwiq5, ijwiq4, flag, flag2, flag3);
		}
		finally
		{
			wabpu.cwtrc += p0.Item.Length;
			wabpu.mbsca++;
			if (p1 == aqicq.cecby)
			{
				wabpu.btlui++;
			}
			nnyll(p1);
		}
		return p1 == aqicq.cecby;
	}

	private aqicq gtlan(string p0, string p1, ijwiq p2, ijwiq p3, ijwiq p4, ijwiq p5, bool p6, bool p7, bool p8)
	{
		nvmgb("Transferring file ('{0}' ---> '{1}').", p0, p1);
		nnyll(aqicq.brerj);
		while (true)
		{
			hicrt();
			try
			{
				if (p6 && 0 == 0)
				{
					if (dvdya && 0 == 0)
					{
						xhzmk(p2, p3, p0, p1, 0L, p3.Length);
					}
					else
					{
						lebzd(p2, p3, p0, p1, 0L, p3.Length);
					}
				}
				else if (p7 && 0 == 0)
				{
					if (dvdya && 0 == 0)
					{
						xhzmk(p2, p3, p0, p1, p3.Length, p3.Length);
					}
					else
					{
						lebzd(p2, p3, p0, p1, p3.Length, p3.Length);
					}
				}
				else if (dvdya && 0 == 0)
				{
					rxome(p2, p3, p0, p1, p8);
				}
				else
				{
					ojdmd(p2, p3, p0, p1, p8);
				}
				if (rpiab != ItemDateTimes.None && 0 == 0)
				{
					if (dvdya && 0 == 0)
					{
						ozuvm(p1, p2);
					}
					else
					{
						nmfnd(p1, p2);
					}
				}
				return aqicq.cecby;
			}
			catch (pwzds pwzds2)
			{
				wllpt(pwzds2.InnerException, pwzds2.Message, TransferProblemType.CannotTransferFile, p4, p5, omidq.nvxgc, out var p9);
				if (p9 == omidq.zgjbt)
				{
					continue;
				}
				return aqicq.qjuok;
			}
		}
	}

	private ijwiq feknh(string p0, bool p1, bool p2)
	{
		return qvwqe(p0, p1, p2, null);
	}

	private ijwiq qvwqe(string p0, bool p1, bool p2, ijwiq p3)
	{
		if (mbcdc(p1) && 0 == 0)
		{
			return okpct(p0);
		}
		string text = p0.TrimEnd(wqjmm);
		if (text.Length == 0 || 1 == 0)
		{
			return ijwiq.eiuvm(p0);
		}
		if (text.Equals(".", StringComparison.Ordinal) && 0 == 0)
		{
			return ijwiq.eiuvm(p0);
		}
		if (p2 && 0 == 0 && jrhky && 0 == 0)
		{
			return gczcr(p0, p1: false, p3);
		}
		return fjtch(p0, p3);
	}

	private void zazxj(lspmt p0)
	{
		bool flag = true;
		string rvimb = p0.rvimb;
		string p1 = clecl(rvimb, null);
		string p2 = lyyco(rvimb, null);
		wabpu.ueijm(p0.Item.Item, p2, p1);
		nvmgb("Deleting file ('{0}').", rvimb);
		if (miosc && 0 == 0 && p0.Item.IsFile && 0 == 0)
		{
			nnyll(aqicq.yrhed);
		}
		aqicq aqicq2 = ((p0.Item.IsFile ? true : false) ? aqicq.zarny : aqicq.hxwkq);
		try
		{
			if (miosc && 0 == 0)
			{
				while (true)
				{
					hicrt();
					if (!mzvyn)
					{
						break;
					}
					try
					{
						ijwiq ijwiq2 = qvwqe(rvimb, p1: true, p2: true, p0.Item);
						if (ijwiq2 == null || 1 == 0)
						{
							throw new pwzds(null, null);
						}
						if (p0.Item.IsFile == ijwiq2.IsFile)
						{
							if (ijwiq2.IsFile && 0 == 0 && ijwiq2.Length != p0.Item.Length)
							{
								wabpu.jcakx += ijwiq2.Length - p0.Item.Length;
							}
						}
						else if (ijwiq2.IsFile && 0 == 0)
						{
							wabpu.jcakx += ijwiq2.Length;
						}
						else
						{
							wabpu.jcakx -= p0.Item.Length;
						}
						p0.pgbfk(ijwiq2);
						wabpu.ueijm(ijwiq2.Item, p2, null);
					}
					catch (pwzds pwzds2)
					{
						wllpt(p1: (pwzds2.InnerException != null) ? brgjd.edcru("Cannot retrieve information of source {1} ('{0}').", rvimb, (p0.Item.IsFile ? true : false) ? "file" : "directory") : brgjd.edcru("Cannot determine whether a {1} that was previously found exists ('{0}').", rvimb, (p0.Item.IsFile ? true : false) ? "file" : "directory"), p0: pwzds2.InnerException, p2: (p0.Item.IsFile ? true : false) ? TransferProblemType.CannotFindFile : TransferProblemType.CannotFindDirectory, p3: p0.Item, p4: null, p5: omidq.nvxgc, p6: out var p3);
						if (p3 == omidq.zgjbt)
						{
							continue;
						}
						return;
					}
					break;
				}
			}
			else if (p0.Item.IsDirectory && 0 == 0)
			{
				flag = ((!dvdya) ? (shnkg(p0, p1: false) == 0) : (umxig(p0, p1: false) == 0));
			}
			if (flag && 0 == 0)
			{
				aqicq2 = wpilg(rvimb, p0.Item);
			}
		}
		finally
		{
			if (miosc && 0 == 0 && ((p0.Item.IsFile ? true : false) || p0.Item.IsLink))
			{
				wabpu.mbsca++;
				if (p0.Item.IsFile && 0 == 0)
				{
					wabpu.cwtrc += p0.Item.Length;
				}
				if (aqicq2 == aqicq.rxwaf)
				{
					wabpu.btlui++;
				}
				if (aqicq2 == aqicq.mpwkk && p0.Item.IsLink && 0 == 0)
				{
					wabpu.btlui++;
				}
			}
			if (flag && 0 == 0)
			{
				nnyll(aqicq2);
			}
		}
	}

	private aqicq wpilg(string p0, ijwiq p1)
	{
		while (true)
		{
			hicrt();
			try
			{
				if (dvdya && 0 == 0)
				{
					jabxt(p1);
				}
				else
				{
					ocazz(p1, p0);
				}
				return (p1.IsFile ? true : false) ? aqicq.rxwaf : aqicq.mpwkk;
			}
			catch (pwzds pwzds2)
			{
				wllpt(pwzds2.InnerException, pwzds2.Message, (p1.IsFile ? true : false) ? TransferProblemType.CannotDeleteFile : TransferProblemType.CannotDeleteDirectory, p1, null, omidq.nvxgc, out var p2);
				if (p2 == omidq.zgjbt)
				{
					continue;
				}
				return (p1.IsFile ? true : false) ? aqicq.qjuok : aqicq.lovez;
			}
		}
	}

	private void ocqxj(lspmt p0, omidq p1, out omidq p2)
	{
		Exception p3 = tpelv(brgjd.edcru("Possible infinite loop detected ('{0}').", p0.rvimb), null, TransferProblemType.InfiniteLoopDetected);
		bksrc(p3, TransferProblemType.InfiniteLoopDetected, zrnkw(p0.Item, null), davms(p0.Item, null), omidq.ytvbw, p1, out p2);
	}

	private void kjted(bool p0, lspmt p1, ref aqicq p2)
	{
		string name = p1.Item.Name;
		if (enofc(name))
		{
			return;
		}
		TransferProblemType transferProblemType = ((p0 ? true : false) ? TransferProblemType.FileNameIsInvalidOnTargetFileSystem : TransferProblemType.DirectoryNameIsInvalidOnTargetFileSystem);
		Exception p3 = ((!p0) ? tpelv(brgjd.edcru("Directory name is invalid on target destination ('{0}').", name), null, transferProblemType) : tpelv(brgjd.edcru("File name is invalid on target destination ('{0}').", name), null, transferProblemType));
		omidq p4;
		oisbg oisbg2 = bksrc(p3, transferProblemType, zrnkw(p1.Item, null), davms(p1.Item, null), omidq.ytvbw, omidq.edeyx, out p4);
		switch (p4)
		{
		case omidq.dgkzq:
			p2 = ((p0 ? true : false) ? aqicq.qjuok : aqicq.lovez);
			break;
		case omidq.cbxuc:
		{
			string text = wabpu.hlrzw.Remove(wabpu.hlrzw.Length - name.Length, name.Length) + oisbg2.wiqoh;
			if (dvdya && 0 == 0)
			{
				wabpu.unstq = text;
			}
			else
			{
				wabpu.vlzcc = text;
			}
			break;
		}
		}
	}

	protected oisbg wllpt(Exception p0, string p1, TransferProblemType p2, ijwiq p3, ijwiq p4, omidq p5, out omidq p6)
	{
		Exception ex = tpelv(p1, p0, p2);
		if (!lxsff() || 1 == 0)
		{
			throw ex;
		}
		return bksrc(ex, p2, p3, p4, omidq.ytvbw, p5, out p6);
	}

	protected oisbg bksrc(Exception p0, TransferProblemType p1, ijwiq p2, ijwiq p3, omidq p4, omidq p5, out omidq p6)
	{
		Exception ex = tirwt(p0);
		if (ex != null && 0 == 0)
		{
			throw ex;
		}
		p5 |= omidq.ayvtv | omidq.ytvbw;
		oisbg oisbg2 = new oisbg((dvdya ? true : false) ? p2 : p3, p0, wabpu.uaage, p1, wabpu.vlzcc, (p3 == null) ? null : ((LocalItem)p3.Item), wabpu.unstq, p2?.Item, p4, p5, (dvdya ? true : false) ? gejqh : ddpbt, ewgpk, wabpu.kqzlx);
		fngaz(oisbg2, p2, p3);
		switch (oisbg2.ulaxh)
		{
		case omidq.dgkzq:
		case omidq.masrh:
		case omidq.nivzg:
		case omidq.smbdu:
		case omidq.nwoav:
		case omidq.zgjbt:
		case omidq.bnwfa:
		case omidq.zbfno:
		case omidq.xrmda:
			if ((p5 & oisbg2.ulaxh) != 0 && 0 == 0)
			{
				p6 = oisbg2.ulaxh;
				return oisbg2;
			}
			break;
		case omidq.cbxuc:
			if ((p5 & omidq.cbxuc) != 0 && 0 == 0)
			{
				if (oisbg2.wiqoh == null || 1 == 0)
				{
					throw new InvalidOperationException("The NewName property has to be set when Action equals to Rename.", p0);
				}
				p6 = omidq.cbxuc;
				return oisbg2;
			}
			break;
		case omidq.ayvtv:
			throw tpelv("Operation was canceled.", p0, TransferProblemType.OperationCanceled);
		case omidq.ytvbw:
			throw p0;
		default:
			throw new InvalidOperationException(brgjd.edcru("Unknown Reaction was specified ('{0}').", oisbg2.ulaxh), p0);
		}
		throw new InvalidOperationException(brgjd.edcru("Reaction ('{0}') is not an allowed response for the {1} exception.", oisbg2.ulaxh, p1), p0);
	}

	private ijwiq okpct(string p0)
	{
		try
		{
			LocalItem localItem = new LocalItem(p0);
			if (!localItem.Exists || 1 == 0)
			{
				return null;
			}
			return new ijwiq(localItem);
		}
		catch (SecurityException inner)
		{
			throw new pwzds(inner);
		}
		catch (UnauthorizedAccessException inner2)
		{
			throw new pwzds(inner2);
		}
		catch (IOException inner3)
		{
			throw new pwzds(inner3);
		}
	}

	private void jabxt(ijwiq p0)
	{
		try
		{
			((LocalItem)p0.Item).Delete();
		}
		catch (SecurityException ex)
		{
			string message = brgjd.edcru("Cannot delete local {1} ('{0}'). " + ex.Message, p0.Path, (p0.IsFile ? true : false) ? "file" : "directory");
			throw new pwzds(message, ex);
		}
		catch (UnauthorizedAccessException ex2)
		{
			string message2 = brgjd.edcru("Cannot delete local {1} ('{0}'). " + ex2.Message, p0.Path, (p0.IsFile ? true : false) ? "file" : "directory");
			throw new pwzds(message2, ex2);
		}
		catch (IOException ex3)
		{
			string message3 = brgjd.edcru("Cannot delete local {1} ('{0}'). " + ex3.Message, p0.Path, (p0.IsFile ? true : false) ? "file" : "directory");
			throw new pwzds(message3, ex3);
		}
	}

	private int umxig(lspmt p0, bool p1)
	{
		LocalItem localItem = (LocalItem)p0.Item.Item;
		try
		{
			LocalItem[] directories;
			LocalItem[] files;
			LocalItem[] array;
			int num;
			if (p1)
			{
				directories = localItem.GetDirectories();
				files = localItem.GetFiles();
				array = files;
				num = 0;
				if (num != 0)
				{
					goto IL_003a;
				}
				goto IL_00c0;
			}
			return Directory.GetFiles(localItem.FullPath).Length + Directory.GetDirectories(localItem.FullPath).Length;
			IL_00c0:
			if (num < array.Length)
			{
				goto IL_003a;
			}
			LocalItem[] array2 = directories;
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_00d8;
			}
			goto IL_0140;
			IL_00d8:
			LocalItem localItem2 = array2[num2];
			if (siodr == null || false || string.Compare(siodr, localItem2.FullPath, StringComparison.OrdinalIgnoreCase) != 0)
			{
				qqjhq(p0, new ijwiq(localItem2));
				nvmgb(" - Directory added for processing ('{0}').", localItem2.FullPath);
			}
			num2++;
			goto IL_0140;
			IL_0140:
			if (num2 >= array2.Length)
			{
				return files.Length + directories.Length;
			}
			goto IL_00d8;
			IL_003a:
			LocalItem localItem3 = array[num];
			if ((siodr == null || false || string.Compare(siodr, localItem3.FullPath, StringComparison.OrdinalIgnoreCase) != 0) && p0.migcp.omemp(localItem3.FullPath, gilnp, FileSetMatchMode.MatchFile) && 0 == 0)
			{
				qqjhq(p0, new ijwiq(localItem3));
				nvmgb(" - File added for processing ('{0}').", localItem3.FullPath);
			}
			num++;
			goto IL_00c0;
		}
		catch (SecurityException ex)
		{
			string message = brgjd.edcru("Error when retrieving items from a directory ('{0}'). " + ex.Message, p0.rvimb);
			throw new pwzds(message, ex);
		}
		catch (UnauthorizedAccessException ex2)
		{
			string message2 = brgjd.edcru("Error when retrieving items from a directory ('{0}'). " + ex2.Message, p0.rvimb);
			throw new pwzds(message2, ex2);
		}
		catch (IOException ex3)
		{
			string message3 = ((!p0.Item.IsLink || false || !vtdxm.qjbvo(p0.rvimb)) ? brgjd.edcru("Error when retrieving items from a directory ('{0}'). " + ex3.Message, p0.rvimb) : brgjd.edcru("Cannot read from a directory reparse point ('{0}'). " + ex3.Message, p0.rvimb));
			throw new pwzds(message3, ex3);
		}
	}

	private void jgayu(string p0)
	{
		try
		{
			string p1 = dahxy.tfhfm(p0);
			vtdxm.ceona(p1);
		}
		catch (UnauthorizedAccessException ex)
		{
			throw new pwzds(brgjd.edcru("Cannot create directory ('{0}'). " + ex.Message, p0), ex);
		}
		catch (IOException ex2)
		{
			throw new pwzds(brgjd.edcru("Cannot create directory ('{0}'). " + ex2.Message, p0), ex2);
		}
	}

	private void nmfnd(string p0, ijwiq p1)
	{
		try
		{
			vtdxm.tjuce(p0, p1, rpiab);
		}
		catch (UnauthorizedAccessException ex)
		{
			throw new pwzds(brgjd.edcru("Cannot restore file dates ('{0}'). " + ex.Message, p0), ex);
		}
		catch (IOException ex2)
		{
			throw new pwzds(brgjd.edcru("Cannot restore file dates ('{0}'). " + ex2.Message, p0), ex2);
		}
	}

	protected abstract string poizd(string p0);

	protected abstract ijwiq gczcr(string p0, bool p1, ijwiq p2);

	protected abstract ijwiq fjtch(string p0, ijwiq p1);

	protected virtual void ozuvm(string p0, ijwiq p1)
	{
		throw new NotImplementedException("Restore remote date time is not implemented.");
	}

	protected abstract int shnkg(lspmt p0, bool p1);

	protected abstract bool lxsff();

	protected abstract Exception mppwr(string p0);

	protected virtual Exception uvmln(string p0)
	{
		return mppwr(p0);
	}

	protected abstract Exception tpelv(string p0, Exception p1, TransferProblemType p2);

	protected abstract void cfjap(string p0);

	protected abstract long rxome(ijwiq p0, ijwiq p1, string p2, string p3, bool p4);

	protected abstract long xhzmk(ijwiq p0, ijwiq p1, string p2, string p3, long p4, long p5);

	protected abstract long ojdmd(ijwiq p0, ijwiq p1, string p2, string p3, bool p4);

	protected abstract long lebzd(ijwiq p0, ijwiq p1, string p2, string p3, long p4, long p5);

	protected abstract void ocazz(ijwiq p0, string p1);

	protected abstract void hicrt();

	protected abstract Exception tirwt(Exception p0);

	protected virtual bool dgxbt(ijwiq p0, ijwiq p1)
	{
		throw new NotSupportedException("Checksums are not supported by this class.");
	}

	protected virtual void nvmgb(string p0, params object[] p1)
	{
	}

	protected abstract void fngaz(oisbg p0, ijwiq p1, ijwiq p2);

	protected abstract void botrz(object p0);

	protected abstract void gfqam(object p0);

	protected abstract void vqxew(object p0);

	protected virtual void nnyll(aqicq p0)
	{
		switch (p0)
		{
		case aqicq.hcxlr:
		case aqicq.miduf:
		case aqicq.brcpb:
		case aqicq.nzwwg:
			if (jquba && 0 == 0)
			{
				botrz(hrpun.begfh(cvywk.fkarr(p0), wabpu));
			}
			return;
		case aqicq.bnawl:
		case aqicq.brerj:
		case aqicq.qjuok:
		case aqicq.zarny:
		case aqicq.axynn:
		case aqicq.lovez:
		case aqicq.hxwkq:
		case aqicq.nwarc:
		case aqicq.cagtv:
			return;
		}
		if (wabpu.uaage == TransferAction.Deleting)
		{
			if (ncoej && 0 == 0)
			{
				switch (p0)
				{
				case aqicq.yrhed:
				case aqicq.yynwh:
				case aqicq.rxwaf:
				case aqicq.mpwkk:
				case aqicq.ivywy:
					vqxew(hrpun.delkr(cvywk.fgqns(p0), wabpu));
					break;
				}
			}
		}
		else if (fewso && 0 == 0)
		{
			switch (p0)
			{
			case aqicq.yrhed:
				wabpu.ffdop = wabpu.Item.Length;
				wabpu.aqpch = 0L;
				goto case aqicq.cecby;
			case aqicq.yynwh:
			case aqicq.ivywy:
				wabpu.ffdop = -1L;
				wabpu.aqpch = 0L;
				goto case aqicq.cecby;
			case aqicq.cecby:
				gfqam(hrpun.zkztw(cvywk.dfsyq(p0), wabpu, 0L, 0));
				break;
			case aqicq.vdzjk:
				throw new InvalidOperationException("Invalid call of the method.");
			}
		}
	}
}
