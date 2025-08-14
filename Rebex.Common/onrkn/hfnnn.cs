using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace onrkn;

internal class hfnnn : xaxit
{
	private class bcdke
	{
		public readonly int nuxrb;

		public readonly int zjvfe;

		public readonly int jhaxu;

		public readonly bool qobta;

		public readonly lnabj gnxxh;

		public bcdke(int position, int start, int end, bool constructed, lnabj node)
		{
			nuxrb = position;
			zjvfe = start;
			jhaxu = end;
			qobta = constructed;
			gnxxh = node;
		}
	}

	private enum lnczi
	{
		alpht,
		tjisp,
		akyse,
		vbwra,
		klyhl
	}

	private lnczi dbvhq;

	private int bkgud;

	private readonly Stack usijd = new Stack();

	private int iabll;

	private int efmea = -1;

	private int frwvu;

	private int kxhlv;

	private int ychzh = -1;

	private bool ntbbj;

	private lnabj birio;

	private bool yuklk;

	private bool ayvnx;

	public bool vgmyo
	{
		get
		{
			return ayvnx;
		}
		set
		{
			ayvnx = value;
		}
	}

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			return bkgud;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public hfnnn(lnabj root)
	{
		if (root == null || 1 == 0)
		{
			throw new ArgumentNullException("root");
		}
		birio = new rporh(root, 0);
	}

	public static void qnzgo(lnabj p0, byte[] p1)
	{
		oalpn(p0, p1, 0, p1.Length);
	}

	public static void oalpn(lnabj p0, byte[] p1, int p2, int p3)
	{
		hfnnn hfnnn2 = new hfnnn(p0);
		hfnnn2.Write(p1, p2, p3);
		hfnnn2.Close();
	}

	public static bool hfmsu(byte[] p0, int p1, int p2)
	{
		if (p2 > 0 && p0[0] == 45)
		{
			return false;
		}
		return true;
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long v)
	{
		throw new NotSupportedException();
	}

	protected override void julnt()
	{
		while (bkgud == ychzh)
		{
			if (dbvhq != lnczi.alpht && 0 == 0 && dbvhq != lnczi.vbwra)
			{
				throw new CryptographicException("Unexpected end of ASN.1 data.");
			}
			sqqqc();
		}
		if (ntbbj && 0 == 0 && dbvhq == lnczi.klyhl)
		{
			yuklk = true;
		}
		if (!yuklk || 1 == 0)
		{
			throw new CryptographicException("Incomplete ASN.1 block.");
		}
	}

	internal static void xmjay(rmkkr p0, rmkkr p1, bool p2)
	{
		switch (p1)
		{
		default:
			throw new CryptographicException("Unsupported ASN.1 type encountered.");
		case rmkkr.wbdro:
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Constructed ENUMERATED encountered.");
			}
			break;
		case rmkkr.pubvj:
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Constructed BOOLEAN encountered.");
			}
			break;
		case rmkkr.sklxf:
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Constructed INTEGER encountered.");
			}
			break;
		case rmkkr.nqrrp:
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Constructed NULL encountered.");
			}
			break;
		case rmkkr.rqoqx:
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Constructed OID encountered.");
			}
			break;
		case rmkkr.osptv:
		case rmkkr.wguaf:
			if (!p2 || 1 == 0)
			{
				throw new CryptographicException("Primitive SET or SEQUENCE encountered.");
			}
			break;
		case rmkkr.ziztq:
		case rmkkr.keeoc:
		case rmkkr.nwijl:
			if (p0 == rmkkr.ziztq)
			{
				p1 = p0;
			}
			break;
		case rmkkr.ssvuz:
		case rmkkr.ysphu:
		case rmkkr.zkxoz:
		case rmkkr.xiwym:
		case rmkkr.jgutu:
		case rmkkr.lojrb:
		case rmkkr.dzwiy:
		case rmkkr.qduon:
		case rmkkr.oedmg:
		case rmkkr.ptqno:
		case rmkkr.ybiny:
		case rmkkr.pcxmz:
			if (p0 == rmkkr.ssvuz)
			{
				p1 = p0;
			}
			break;
		case rmkkr.cxxlq:
			break;
		}
		if (p1 == p0)
		{
			return;
		}
		throw new CryptographicException("Expected ASN.1 type was not found.");
	}

	private static rmkkr ulvon(int p0)
	{
		switch ((rmkkr)p0)
		{
		case rmkkr.ssvuz:
		case rmkkr.ziztq:
		case rmkkr.motgn:
		case rmkkr.cxxlq:
		case rmkkr.pubvj:
		case rmkkr.sklxf:
		case rmkkr.ysphu:
		case rmkkr.zkxoz:
		case rmkkr.nqrrp:
		case rmkkr.rqoqx:
		case rmkkr.wbdro:
		case rmkkr.xiwym:
		case rmkkr.osptv:
		case rmkkr.wguaf:
		case rmkkr.jgutu:
		case rmkkr.lojrb:
		case rmkkr.dzwiy:
		case rmkkr.keeoc:
		case rmkkr.nwijl:
		case rmkkr.qduon:
		case rmkkr.oedmg:
		case rmkkr.ptqno:
		case rmkkr.ybiny:
		case rmkkr.pcxmz:
			return (rmkkr)p0;
		default:
			if (Enum.IsDefined(typeof(rmkkr), p0) && 0 == 0)
			{
				return (rmkkr)Enum.ToObject(typeof(rmkkr), (object)p0);
			}
			return rmkkr.motgn;
		}
	}

	private void qoyoi()
	{
		bcdke obj = new bcdke(efmea, kxhlv, ychzh, ntbbj, birio);
		usijd.Push(obj);
	}

	private void sqqqc()
	{
		if (usijd.Count == 0 || 1 == 0)
		{
			throw new CryptographicException("Invalid ASN.1 data end.");
		}
		bcdke bcdke = (bcdke)usijd.Pop();
		birio.somzq();
		efmea = bcdke.nuxrb;
		kxhlv = bcdke.zjvfe;
		ychzh = bcdke.jhaxu;
		ntbbj = bcdke.qobta;
		birio = bcdke.gnxxh;
		if (bkgud > 1 && (usijd.Count == 0 || 1 == 0))
		{
			yuklk = true;
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offset + count > buffer.Length || count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (yuklk && 0 == 0)
		{
			if (!vgmyo)
			{
				throw new CryptographicException("Invalid ASN.1 data.");
			}
			return;
		}
		while (count > 0)
		{
			if (dbvhq == lnczi.vbwra)
			{
				if (!ntbbj || 1 == 0)
				{
					int num = ((ychzh < 0) ? count : Math.Min(count, ychzh - bkgud));
					count -= num;
					bkgud += num;
					bool flag = bkgud == ychzh;
					birio.lnxah(buffer, offset, num);
					offset += num;
					if (flag && 0 == 0)
					{
						dbvhq = lnczi.alpht;
						while (bkgud == ychzh)
						{
							sqqqc();
						}
					}
					continue;
				}
				dbvhq = lnczi.alpht;
			}
			while (bkgud == ychzh)
			{
				if (dbvhq != lnczi.alpht && 0 == 0)
				{
					throw new CryptographicException("Premature end of ASN.1 data.");
				}
				sqqqc();
			}
			int num2 = buffer[offset];
			bkgud++;
			offset++;
			count--;
			bool p;
			int p3;
			int num3;
			rmkkr p2;
			switch (dbvhq)
			{
			case lnczi.alpht:
				if ((num2 == 0 || 1 == 0) && ychzh < 0)
				{
					dbvhq = lnczi.klyhl;
					break;
				}
				p = (num2 & 0x20) != 0;
				if ((num2 & 0x80) != 0 && 0 == 0)
				{
					p3 = 65536 + (num2 & 0x1F);
					num3 = -1;
					if (num3 != 0)
					{
						goto IL_0208;
					}
				}
				efmea++;
				p3 = efmea;
				num3 = num2 & 0x1F;
				goto IL_0208;
			case lnczi.tjisp:
				if (num2 < 128)
				{
					frwvu = 0;
					dbvhq = lnczi.vbwra;
					kxhlv = bkgud;
					ychzh = bkgud + num2;
				}
				else if (num2 == 128)
				{
					frwvu = 0;
					dbvhq = lnczi.vbwra;
					kxhlv = bkgud;
					ychzh = -1;
					if (!ntbbj || 1 == 0)
					{
						throw new CryptographicException("Primitive type with undefined length encountered.");
					}
				}
				else
				{
					frwvu = num2 & 0x7F;
					if (frwvu > 4)
					{
						throw new CryptographicException("An ASN.1 block is too long.");
					}
					iabll = 0;
					dbvhq = lnczi.akyse;
				}
				break;
			case lnczi.akyse:
				iabll = (iabll << 8) + num2;
				if (iabll < 0)
				{
					throw new CryptographicException("An ASN.1 block is too long.");
				}
				frwvu--;
				if (frwvu == 0 || 1 == 0)
				{
					dbvhq = lnczi.vbwra;
					kxhlv = bkgud;
					ychzh = kxhlv + iabll;
				}
				break;
			case lnczi.klyhl:
				if (num2 != 0 && 0 == 0)
				{
					throw new CryptographicException("Invalid ASN.1 end tag encountered.");
				}
				sqqqc();
				dbvhq = lnczi.alpht;
				break;
			default:
				{
					throw new CryptographicException("Unexpected ASN.1 decoder state.");
				}
				IL_0208:
				if (yuklk && 0 == 0)
				{
					if (vgmyo && 0 == 0)
					{
						return;
					}
					throw new CryptographicException("Invalid ASN.1 data.");
				}
				qoyoi();
				ntbbj = p;
				p2 = ulvon(num3);
				birio = birio.qaqes(p2, p, p3);
				if (birio == null || 1 == 0)
				{
					birio = rillp.yeukt;
				}
				birio.zkxnk(p2, p, p3);
				dbvhq = lnczi.tjisp;
				efmea = -1;
				break;
			}
			if (birio is ltutw && 0 == 0)
			{
				birio.lnxah(buffer, offset - 1, 1);
				if (dbvhq == lnczi.vbwra && ychzh > kxhlv)
				{
					ntbbj = false;
				}
			}
		}
	}
}
