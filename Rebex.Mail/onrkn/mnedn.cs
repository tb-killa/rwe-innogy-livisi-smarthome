using System.Collections.Generic;

namespace onrkn;

internal class mnedn
{
	private static string wbkfi = "\r\n";

	private venkc yhmmh;

	private fpnng nhpnw;

	private int zydet;

	internal static string zybru => wbkfi;

	public mnedn()
	{
		nhpnw = new fpnng();
	}

	public string ydglw()
	{
		string text = nhpnw.wdtbt();
		if (text == string.Empty && 0 == 0)
		{
			if (yhmmh.pgqva && 0 == 0)
			{
				return string.Empty;
			}
			text = "<body></body>";
		}
		return brgjd.edcru("<html>{1}{0}{1}</html>", text, zybru);
	}

	public void chjez(venkc p0)
	{
		yhmmh = p0;
		sbnrz gttba = yhmmh.gttba;
		if (gttba.khhoe.Count > 0)
		{
			if (gttba.khhoe[0].jprco.Equals(ajuaj.icyep) && 0 == 0)
			{
				drqpt(gttba.khhoe, 0, gttba.khhoe[0], p3: true);
			}
			if (nhpnw.swkfa is cmjgd cmjgd2 && 0 == 0)
			{
				cmjgd2.akccy(cmjgd2.suqdl - 1);
			}
		}
	}

	private void drqpt(List<mmgqv> p0, int p1, mmgqv p2, bool p3)
	{
		if (p2.iyvve == null || 1 == 0)
		{
			return;
		}
		List<mmgqv> khhoe = p2.iyvve.khhoe;
		if (khhoe.Count < 1)
		{
			return;
		}
		string iaqgi = khhoe[0].iaqgi;
		paeay swkfa;
		int num;
		if (iaqgi != null && 0 == 0)
		{
			if (iaqgi.Equals("fonttbl") && 0 == 0)
			{
				excna(khhoe);
				return;
			}
			if (iaqgi.Equals("colortbl") && 0 == 0)
			{
				ghldb(khhoe);
				return;
			}
			if ((iaqgi.Equals("stylesheet") && 0 == 0) || (iaqgi.Equals("info") ? true : false) || (iaqgi.Equals("operator") ? true : false) || iaqgi.Equals("pict"))
			{
				return;
			}
			if (iaqgi.Equals("field") && 0 == 0 && khhoe.Count >= 3 && codsj.hxkbl(khhoe) && 0 == 0)
			{
				swkfa = nhpnw.swkfa;
				if (nhpnw.swkfa is zbmst zbmst2 && 0 == 0)
				{
					nhpnw.swkfa = zbmst2.nysqr();
				}
				mfths mfths2 = new codsj(nhpnw, nhpnw.swkfa, new jzsjg(nhpnw));
				nhpnw.swkfa.ziwoy(mfths2);
				nhpnw.swkfa = mfths2;
				num = 1;
				if (num == 0)
				{
					goto IL_0190;
				}
				goto IL_01cf;
			}
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_01f0;
		}
		goto IL_040d;
		IL_01cf:
		if (num >= khhoe.Count)
		{
			nhpnw.swkfa = swkfa;
			return;
		}
		goto IL_0190;
		IL_040d:
		if (num2 >= khhoe.Count)
		{
			return;
		}
		goto IL_01f0;
		IL_01f0:
		switch (khhoe[num2].jprco)
		{
		case ajuaj.gdmuq:
		{
			if ((num2 == 0 || 1 == 0) && ((khhoe[0].iaqgi.StartsWith("*") ? true : false) || khhoe[0].iaqgi.Equals("info")))
			{
				if ((p3 ? true : false) || (nhpnw.swkfa is codsj && 0 == 0 && khhoe.Count > 1 && khhoe[1].jprco == ajuaj.gdmuq && khhoe[1].iaqgi == "datafield"))
				{
					return;
				}
				break;
			}
			string iaqgi3 = khhoe[num2].iaqgi;
			if (iaqgi3 == "objattph" && 0 == 0 && num2 + 1 < khhoe.Count && khhoe[num2 + 1].jprco == ajuaj.mbsvn && khhoe[num2 + 1].iaqgi == " " && 0 == 0)
			{
				string text = brgjd.edcru("embedded-rtf-attachment-id{0}@rebex.net", zydet);
				string p4 = brgjd.edcru("<img border=0 src=\"cid:{0}\">", text);
				zydet++;
				nhpnw.tqiqh(p4, p1: true);
			}
			scuat(khhoe, num2, khhoe[num2]);
			break;
		}
		case ajuaj.mbsvn:
		{
			string iaqgi2 = khhoe[num2].iaqgi;
			nhpnw.tqiqh(iaqgi2, p1: false);
			break;
		}
		case ajuaj.icyep:
			nhpnw.trlza();
			drqpt(khhoe, num2, khhoe[num2], p3);
			nhpnw.cafwu();
			break;
		}
		num2++;
		goto IL_040d;
		IL_0190:
		if (khhoe[num].jprco.Equals(ajuaj.icyep) && 0 == 0)
		{
			drqpt(khhoe, num, khhoe[num], p3: false);
		}
		num++;
		goto IL_01cf;
	}

	private void scuat(List<mmgqv> p0, int p1, mmgqv p2)
	{
		nhpnw.swkfa.taklk(p2.iaqgi);
		nhpnw.jahzk = p2.iaqgi;
	}

	private void ghldb(List<mmgqv> p0)
	{
		int red = 0;
		int green = 0;
		int blue = 0;
		int tint = 255;
		int shade = 255;
		int num = 0;
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_0024;
		}
		goto IL_0250;
		IL_0024:
		if (p0[num2].jprco == ajuaj.gdmuq || p0[num2].jprco == ajuaj.mbsvn)
		{
			int p1;
			if (p0[num2].iaqgi.IndexOf(";") >= 0)
			{
				ooyms value = new ooyms("c" + num, red, green, blue, tint, shade);
				if (!nhpnw.jwtwk.ContainsKey(num) || 1 == 0)
				{
					nhpnw.jwtwk.Add(num, value);
				}
				red = 0;
				green = 0;
				blue = 0;
				tint = 255;
				shade = 255;
				num++;
			}
			else if (p0[num2].iaqgi.StartsWith("red") && 0 == 0 && rqdck(p0[num2].iaqgi, "red", out p1))
			{
				red = p1;
			}
			else if (p0[num2].iaqgi.StartsWith("green") && 0 == 0 && rqdck(p0[num2].iaqgi, "green", out p1))
			{
				green = p1;
			}
			else if (p0[num2].iaqgi.StartsWith("blue") && 0 == 0 && rqdck(p0[num2].iaqgi, "blue", out p1))
			{
				blue = p1;
			}
			else if (p0[num2].iaqgi.StartsWith("tint") && 0 == 0 && rqdck(p0[num2].iaqgi, "tint", out p1))
			{
				tint = p1;
			}
			else if (p0[num2].iaqgi.StartsWith("shade") && 0 == 0 && rqdck(p0[num2].iaqgi, "shade", out p1) && 0 == 0)
			{
				shade = p1;
			}
		}
		num2++;
		goto IL_0250;
		IL_0250:
		if (num2 >= p0.Count)
		{
			return;
		}
		goto IL_0024;
	}

	private void excna(List<mmgqv> p0)
	{
		int num = 1;
		if (num == 0)
		{
			goto IL_000c;
		}
		goto IL_0469;
		IL_000c:
		List<mmgqv> khhoe;
		brnhb brnhb2;
		int num2;
		if (p0[num].iyvve != null && 0 == 0)
		{
			khhoe = p0[num].iyvve.khhoe;
			if (khhoe.Count > 1)
			{
				brnhb2 = new brnhb();
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_0051;
				}
				goto IL_041a;
			}
		}
		goto IL_045d;
		IL_02ea:
		brnhb2.kunwh = brnhb.aimxq.dyunt;
		goto IL_0416;
		IL_0469:
		if (num >= p0.Count)
		{
			return;
		}
		goto IL_000c;
		IL_031a:
		brnhb2.kunwh = brnhb.aimxq.tmasq;
		goto IL_0416;
		IL_0051:
		string iaqgi = khhoe[num2].iaqgi;
		ajuaj jprco = khhoe[num2].jprco;
		if ((iaqgi == null || 1 == 0) && (jprco == ajuaj.icyep || 1 == 0))
		{
			mmgqv mmgqv2 = khhoe[num2];
			if (mmgqv2.iyvve.khhoe.Count == 3 && mmgqv2.iyvve.khhoe[0].iaqgi != null && 0 == 0 && mmgqv2.iyvve.khhoe[0].iaqgi == "*" && 0 == 0 && mmgqv2.iyvve.khhoe[1].iaqgi != null && 0 == 0 && mmgqv2.iyvve.khhoe[1].iaqgi == "falt" && 0 == 0 && mmgqv2.iyvve.khhoe[2].iaqgi != null && 0 == 0)
			{
				brnhb2.ekgcj = mmgqv2.iyvve.khhoe[2].iaqgi;
			}
		}
		else if (iaqgi != null && 0 == 0)
		{
			if (jprco == ajuaj.mbsvn)
			{
				if (iaqgi.EndsWith(";") && 0 == 0)
				{
					if (iaqgi.Length > 1)
					{
						brnhb2.hmtqe = iaqgi.Substring(0, iaqgi.Length - 1);
					}
				}
				else if (iaqgi.Length >= 3)
				{
					brnhb2.hmtqe = iaqgi;
				}
			}
			if (jprco == ajuaj.gdmuq)
			{
				string key;
				if ((key = iaqgi) != null && 0 == 0)
				{
					if (czzgh.vnhgl == null || 1 == 0)
					{
						czzgh.vnhgl = new Dictionary<string, int>(8)
						{
							{ "fnil", 0 },
							{ "froman", 1 },
							{ "fswiss", 2 },
							{ "fmodern", 3 },
							{ "fscript", 4 },
							{ "fdecor", 5 },
							{ "ftech", 6 },
							{ "fbidi", 7 }
						};
					}
					if (czzgh.vnhgl.TryGetValue(key, out var value))
					{
						switch (value)
						{
						case 0:
							break;
						case 1:
							goto IL_02de;
						case 2:
							goto IL_02ea;
						case 3:
							goto IL_02f6;
						case 4:
							goto IL_0302;
						case 5:
							goto IL_030e;
						case 6:
							goto IL_031a;
						case 7:
							goto IL_0326;
						default:
							goto IL_0332;
						}
						brnhb2.kunwh = brnhb.aimxq.ptazv;
						goto IL_0416;
					}
				}
				goto IL_0332;
			}
		}
		goto IL_0416;
		IL_02f6:
		brnhb2.kunwh = brnhb.aimxq.xrpup;
		goto IL_0416;
		IL_0302:
		brnhb2.kunwh = brnhb.aimxq.gayhi;
		goto IL_0416;
		IL_030e:
		brnhb2.kunwh = brnhb.aimxq.iktvx;
		goto IL_0416;
		IL_02de:
		brnhb2.kunwh = brnhb.aimxq.jzpmm;
		goto IL_0416;
		IL_0416:
		num2++;
		goto IL_041a;
		IL_041a:
		if (num2 < khhoe.Count)
		{
			goto IL_0051;
		}
		if (!nhpnw.ivqtq.ContainsKey(brnhb2.thzuv) || 1 == 0)
		{
			nhpnw.ivqtq.Add(brnhb2.thzuv, brnhb2);
		}
		goto IL_045d;
		IL_0326:
		brnhb2.kunwh = brnhb.aimxq.gwvzv;
		goto IL_0416;
		IL_045d:
		num++;
		goto IL_0469;
		IL_0332:
		if (iaqgi.StartsWith("fcharset") && 0 == 0 && rqdck(iaqgi, "fcharset", out var p1))
		{
			brnhb2.uhkfj = p1;
		}
		else if (iaqgi.StartsWith("fprq") && 0 == 0 && rqdck(iaqgi, "fprq", out p1))
		{
			brnhb2.aiofa = p1;
		}
		else if (iaqgi.StartsWith("f") && 0 == 0 && rqdck(iaqgi, "f", out p1))
		{
			brnhb2.thzuv = p1;
		}
		else if (iaqgi.StartsWith("cpg") && 0 == 0 && rqdck(iaqgi, "cpg", out p1) && 0 == 0)
		{
			brnhb2.coxhl = p1;
		}
		goto IL_0416;
	}

	internal static string miajk(string p0)
	{
		return p0.Replace("\r", "").Replace("\n", "");
	}

	internal static string rgxzy(string p0)
	{
		return p0.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
	}

	internal static bool rqdck(string p0, string p1, out int p2)
	{
		if (p0.StartsWith(p1) && 0 == 0)
		{
			string p3 = p0.Substring(p1.Length);
			return brgjd.bnrqx(p3, out p2);
		}
		p2 = 0;
		return false;
	}

	internal void anypa()
	{
		nhpnw.vejtn();
	}
}
