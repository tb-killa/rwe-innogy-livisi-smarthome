using System;
using System.Globalization;
using System.Text;
using Rebex.Mail;
using Rebex.OutlookMessages;

namespace onrkn;

internal class xrprs
{
	private jfxnb iqkdr;

	private bool ykabf;

	private int lozuy;

	private MsgPropertyId ycamh;

	private byte[] vlfnb;

	private string bvqmo;

	private int lcima;

	private Guid lysmf;

	private MsgPropertySet gezyz = (MsgPropertySet)(-1);

	private int pwasm;

	private long vzqow = -1L;

	private int qciyh = -1;

	public bool jisdp => ykabf;

	public MsgPropertyId hnnrt => ycamh;

	internal byte[] lcvww
	{
		get
		{
			if (vlfnb == null || 1 == 0)
			{
				bdycp();
			}
			return vlfnb;
		}
	}

	public string kmaxh
	{
		get
		{
			if (bvqmo == null || 1 == 0)
			{
				if (ykabf && 0 == 0)
				{
					if (Enum.IsDefined(typeof(MsgPropertyId), ycamh) && 0 == 0)
					{
						bvqmo = ycamh.ToString();
					}
					else
					{
						bvqmo = brgjd.edcru("UNKNOWN 0x{0:X4}", (ushort)ycamh);
					}
				}
				else
				{
					bvqmo = Encoding.Unicode.GetString(lcvww, 0, lcvww.Length);
				}
			}
			return bvqmo;
		}
	}

	public Guid oaixu => lysmf;

	public MsgPropertySet ognto
	{
		get
		{
			if (gezyz < MsgPropertySet.None)
			{
				if (lcima == 1)
				{
					gezyz = MsgPropertySet.Mapi;
				}
				else if (lcima == 2)
				{
					gezyz = MsgPropertySet.PublicStrings;
				}
				else if (!dzwgu.ymgvg.TryGetValue(oaixu, out gezyz) || 1 == 0)
				{
					gezyz = MsgPropertySet.None;
				}
			}
			return gezyz;
		}
	}

	public int zcrap => pwasm;

	public long ljmua
	{
		get
		{
			if (vzqow < 0)
			{
				if (ykabf && 0 == 0)
				{
					throw new InvalidOperationException("Cannot get CRC-32 from numeric property.");
				}
				byte[] bytes = lcvww;
				if (lcima > 2 && kmaxh != kmaxh.ToLower(CultureInfo.InvariantCulture) && 0 == 0 && ognto == MsgPropertySet.InternetHeaders)
				{
					bytes = Encoding.Unicode.GetBytes(kmaxh.ToLower(CultureInfo.InvariantCulture));
				}
				mecsr mecsr2 = new mecsr();
				mecsr2.hxtqz = 4294967295L;
				mecsr2.zeeih(bytes);
				vzqow = ~mecsr2.hxtqz & 0xFFFFFFFFu;
			}
			return vzqow;
		}
	}

	public int bzdcc
	{
		get
		{
			if (qciyh < 0)
			{
				qciyh = qygeo(lcima);
			}
			return qciyh;
		}
	}

	private void bdycp()
	{
		if (ykabf && 0 == 0)
		{
			throw new InvalidOperationException("Property is numeric.");
		}
		if (bvqmo == null || 1 == 0)
		{
			if (iqkdr.eulhi["__nameid_version1.0/__substg1.0_00040102"] == null || 1 == 0)
			{
				throw new MsgMessageException("No string definition for named property {0}.", pwasm);
			}
			xxolr xxolr2 = iqkdr.eulhi.xbrtt("__nameid_version1.0/__substg1.0_00040102");
			xxolr2.Position = lozuy;
			int num = xxolr2.wfljb();
			if (num < 0)
			{
				throw new MsgMessageException("Invalid string length definition for named property {0}.", pwasm);
			}
			vlfnb = new byte[num];
			xxolr2.hvees(vlfnb);
		}
		else
		{
			vlfnb = Encoding.Unicode.GetBytes(bvqmo);
		}
	}

	public int qygeo(int p0)
	{
		if (ykabf && 0 == 0)
		{
			return 4096 + (lozuy ^ (p0 << 1)) % 31;
		}
		return 4096 + (int)((ljmua ^ ((p0 << 1) | 1)) % 31);
	}

	public string xmtnb(int p0)
	{
		return brgjd.edcru("{0}{1:X8}", "__substg1.0_", (qygeo(p0) << 16) | 0x102);
	}

	internal xrprs(jfxnb owner, byte[] data)
	{
		if (owner == null || 1 == 0)
		{
			throw new ArgumentNullException("owner", "Message cannot be null.");
		}
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data", "Buffer cannot be null.");
		}
		if (data.Length < 8)
		{
			throw new ArgumentException("Invalid data length.", "data");
		}
		iqkdr = owner;
		lozuy = jlfbq.nlfaq(data, 0);
		pwasm = jlfbq.izulq(data, 6);
		lcima = jlfbq.izulq(data, 4) >> 1;
		ykabf = (data[4] & 1) == 0;
		if (ykabf && 0 == 0)
		{
			ycamh = (MsgPropertyId)lozuy;
		}
		if (lcima == 1)
		{
			lysmf = dzwgu.sazkf[MsgPropertySet.Mapi];
		}
		else if (lcima == 2)
		{
			lysmf = dzwgu.sazkf[MsgPropertySet.PublicStrings];
		}
		else
		{
			if (lcima <= 0 || lcima - 3 >= iqkdr.pwleq.Count)
			{
				throw new MsgMessageException("Invalid GUID index of the named property {0}.", kmaxh);
			}
			lysmf = iqkdr.pwleq[lcima - 3];
		}
		if ((!ykabf || 1 == 0) && (!iqkdr.xewra.dqdgz || 1 == 0))
		{
			bdycp();
		}
	}

	internal xrprs(jfxnb owner, int index, MsgPropertyId lid, MsgPropertySet set)
	{
		ykabf = true;
		ycamh = lid;
		lozuy = (int)lid;
		skzcd(owner, index, set);
	}

	internal xrprs(jfxnb owner, int index, string name, MsgPropertySet set)
	{
		bvqmo = name;
		skzcd(owner, index, set);
	}

	private void skzcd(jfxnb p0, int p1, MsgPropertySet p2)
	{
		if (!dzwgu.sazkf.TryGetValue(p2, out lysmf) || 1 == 0)
		{
			throw new ArgumentException("Property set is unknown.", "set");
		}
		iqkdr = p0;
		pwasm = p1;
		gezyz = p2;
		switch (p2)
		{
		case MsgPropertySet.Mapi:
			lcima = 1;
			return;
		case MsgPropertySet.PublicStrings:
			lcima = 2;
			return;
		}
		if (p0.fenpp.TryGetValue(lysmf, out lcima) && 0 == 0)
		{
			lcima += 3;
			return;
		}
		lcima = p0.pwleq.Count + 3;
		p0.fenpp.Add(lysmf, p0.fenpp.Count);
		p0.pwleq.Add(lysmf);
	}

	public override string ToString()
	{
		if (ykabf && 0 == 0)
		{
			return brgjd.edcru("NUMERIC: property index: 8{0:X3}, Stream ID: 0x{1:X4}, GUID: {2}, LID: {3}", zcrap, bzdcc, (ognto == MsgPropertySet.None) ? oaixu.ToString() : ognto.ToString(), kmaxh);
		}
		return brgjd.edcru("STRING: property index: 8{0:X3}, Stream ID: 0x{1:X4}, GUID: {2}, Name: {3}, CRC-32: {4:X4}", zcrap, bzdcc, (ognto == MsgPropertySet.None) ? oaixu.ToString() : ognto.ToString(), kmaxh, ljmua);
	}
}
