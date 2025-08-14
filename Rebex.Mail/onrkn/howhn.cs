using System;
using System.Collections.Generic;
using Rebex.Mail;
using Rebex.OutlookMessages;

namespace onrkn;

internal class howhn : upend<qacae>
{
	private readonly Dictionary<MsgPropertyTag, qacae> uyegz = new Dictionary<MsgPropertyTag, qacae>();

	private readonly Dictionary<MsgPropertyId, upend<qacae>> eehqa = new Dictionary<MsgPropertyId, upend<qacae>>();

	private readonly Dictionary<string, upend<qacae>> vaerr = new Dictionary<string, upend<qacae>>(StringComparer.OrdinalIgnoreCase);

	private bool ujfdf;

	internal bool gixkz
	{
		get
		{
			return ujfdf;
		}
		set
		{
			ujfdf = value;
		}
	}

	public qacae this[MsgPropertyTag tag]
	{
		get
		{
			if (uyegz.TryGetValue(tag, out var value) && 0 == 0)
			{
				return value;
			}
			return null;
		}
	}

	public upend<qacae> this[MsgPropertyId lid]
	{
		get
		{
			if (eehqa.TryGetValue(lid, out var value) && 0 == 0)
			{
				return value;
			}
			return null;
		}
	}

	public upend<qacae> this[string name]
	{
		get
		{
			if (vaerr.TryGetValue(name, out var value) && 0 == 0)
			{
				return value;
			}
			return null;
		}
	}

	internal howhn(jfxnb owner)
		: this(owner, allowMultipleTags: false)
	{
	}

	internal howhn(jfxnb owner, bool allowMultipleTags)
		: base(owner)
	{
		gixkz = allowMultipleTags;
	}

	public bool ptrsn(MsgPropertyTag p0)
	{
		return uyegz.ContainsKey(p0);
	}

	public bool njwgp(MsgPropertyId p0)
	{
		return eehqa.ContainsKey(p0);
	}

	internal override void vmjwa()
	{
		base.vmjwa();
		uyegz.Clear();
		eehqa.Clear();
		vaerr.Clear();
	}

	internal override qacae rckpv(int p0)
	{
		qacae qacae2 = base.rckpv(p0);
		aivoi(qacae2);
		return qacae2;
	}

	internal override bool plpge(qacae p0)
	{
		bool flag = base.plpge(p0);
		if (flag && 0 == 0)
		{
			aivoi(p0);
		}
		return flag;
	}

	private void aivoi(qacae p0)
	{
		upend<qacae> value;
		switch (p0.hmrmu)
		{
		case mshrw.isojd:
			uyegz.Remove(p0.xablt);
			break;
		case mshrw.dtkaq:
			if (eehqa.TryGetValue(p0.ifnad, out value) && 0 == 0)
			{
				value.plpge(p0);
			}
			break;
		case mshrw.tkmbe:
			if (vaerr.TryGetValue(p0.abdox, out value) && 0 == 0)
			{
				value.plpge(p0);
			}
			break;
		}
	}

	public void gdqdd(MsgPropertyTag p0)
	{
		if (uyegz.TryGetValue(p0, out var value) && 0 == 0)
		{
			base.plpge(value);
			uyegz.Remove(p0);
		}
	}

	public void eqafh(MsgPropertyId p0, MsgPropertySet p1)
	{
		int num;
		if (eehqa.TryGetValue(p0, out var value))
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0020;
			}
			goto IL_004e;
		}
		return;
		IL_0020:
		qacae qacae2 = value[num];
		if (qacae2.ryfdm.ognto == p1)
		{
			value.rckpv(num--);
			base.plpge(qacae2);
		}
		num++;
		goto IL_004e;
		IL_004e:
		if (num >= value.fnqqt)
		{
			if (value.fnqqt == 0 || 1 == 0)
			{
				eehqa.Remove(p0);
			}
			return;
		}
		goto IL_0020;
	}

	public void qcrsw(string p0, MsgPropertySet p1)
	{
		int num;
		if (vaerr.TryGetValue(p0, out var value))
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0020;
			}
			goto IL_004e;
		}
		return;
		IL_0020:
		qacae qacae2 = value[num];
		if (qacae2.ryfdm.ognto == p1)
		{
			value.rckpv(num--);
			base.plpge(qacae2);
		}
		num++;
		goto IL_004e;
		IL_004e:
		if (num >= value.fnqqt)
		{
			if (value.fnqqt == 0 || 1 == 0)
			{
				vaerr.Remove(p0);
			}
			return;
		}
		goto IL_0020;
	}

	public T ocszm<T>(MsgPropertyId p0)
	{
		return axybu(this[p0], default(T), p2: true);
	}

	public T hlyhv<T>(string p0)
	{
		return axybu(this[p0], default(T), p2: true);
	}

	public T jljfr<T>(MsgPropertyId p0)
	{
		return axybu(this[p0], default(T), p2: false);
	}

	public T wcmmv<T>(MsgPropertyId p0, T p1)
	{
		return axybu(this[p0], p1, p2: false);
	}

	public T oijdq<T>(string p0)
	{
		return axybu(this[p0], default(T), p2: false);
	}

	public T qpukf<T>(string p0, T p1)
	{
		return axybu(this[p0], p1, p2: false);
	}

	private T axybu<T>(upend<qacae> p0, T p1, bool p2)
	{
		if (p0 == null || false || p0.fnqqt == 0 || 1 == 0)
		{
			return p1;
		}
		IEnumerator<qacae> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				qacae current = enumerator.Current;
				if (dzwgu.pmrst<T>(current.pzpvc) && 0 == 0)
				{
					return (T)current.tgbhs;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (p2 && 0 == 0)
		{
			return p1;
		}
		return (T)p0[0].tgbhs;
	}

	public T unzoh<T>(MsgPropertyTag p0)
	{
		return vvzzv(p0, default(T));
	}

	public T vvzzv<T>(MsgPropertyTag p0, T p1)
	{
		qacae qacae2 = this[p0];
		if (qacae2 == null || 1 == 0)
		{
			return p1;
		}
		return (T)qacae2.tgbhs;
	}

	public qacae qrqdp<T>(MsgPropertyTag p0, T p1, bool p2 = false)
	{
		if ((p2 ? true : false) || p1 == null)
		{
			gdqdd(p0);
		}
		if (p1 == null || 1 == 0)
		{
			return null;
		}
		qacae qacae2 = ((p2 ? true : false) ? null : this[p0]);
		if (qacae2 != null && 0 == 0)
		{
			if (!dzwgu.pmrst<T>(qacae2.pzpvc) || 1 == 0)
			{
				throw new ArgumentException("The property already exists and uses different data type. Remove the original property first if changing data type is desired.");
			}
			qacae2.tgbhs = p1;
			return qacae2;
		}
		string key;
		if ((key = dzwgu.njsop<T>()) != null && 0 == 0)
		{
			if (czzgh.grrhv == null || 1 == 0)
			{
				czzgh.grrhv = new Dictionary<string, int>(19)
				{
					{ "String", 0 },
					{ "String[]", 1 },
					{ "Boolean", 2 },
					{ "Int16", 3 },
					{ "Int16[]", 4 },
					{ "Int32", 5 },
					{ "Int32[]", 6 },
					{ "Int64", 7 },
					{ "Int64[]", 8 },
					{ "DateTime", 9 },
					{ "DateTime[]", 10 },
					{ "Byte[]", 11 },
					{ "Byte[][]", 12 },
					{ "Guid", 13 },
					{ "Guid[]", 14 },
					{ "Single", 15 },
					{ "Single[]", 16 },
					{ "Double", 17 },
					{ "Double[]", 18 }
				};
			}
			if (czzgh.grrhv.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					return duwaw(p0, (string)(object)p1);
				case 1:
					return owvuz(p0, (string[])(object)p1);
				case 2:
					return dossp(p0, (bool)(object)p1);
				case 3:
					return mnfnx(p0, (short)(object)p1);
				case 4:
					return sqnvz(p0, (short[])(object)p1);
				case 5:
					return mycww(p0, (int)(object)p1);
				case 6:
					return awzyi(p0, (int[])(object)p1);
				case 7:
					return xtczv(p0, (long)(object)p1);
				case 8:
					return ubdne(p0, (long[])(object)p1);
				case 9:
					return bvloj(p0, (DateTime)(object)p1);
				case 10:
					return nrjbu(p0, (DateTime[])(object)p1);
				case 11:
					return onqsx(p0, (byte[])(object)p1);
				case 12:
					return fhszr(p0, (byte[][])(object)p1);
				case 13:
					return ezdgm(p0, (Guid)(object)p1);
				case 14:
					return vtrrr(p0, (Guid[])(object)p1);
				case 15:
					return fsllm(p0, (float)(object)p1);
				case 16:
					return dgjlx(p0, (float[])(object)p1);
				case 17:
					return xwaiw(p0, (double)(object)p1);
				case 18:
					return gelek(p0, (double[])(object)p1);
				}
			}
		}
		throw new ArgumentException("Unsupported MSG data type.");
	}

	public qacae frvyd<T>(MsgPropertyId p0, MsgPropertySet p1, T p2)
	{
		eqafh(p0, p1);
		if (p2 == null || 1 == 0)
		{
			return null;
		}
		string key;
		if ((key = dzwgu.njsop<T>()) != null && 0 == 0)
		{
			if (czzgh.capxq == null || 1 == 0)
			{
				czzgh.capxq = new Dictionary<string, int>(19)
				{
					{ "String", 0 },
					{ "String[]", 1 },
					{ "Boolean", 2 },
					{ "Int16", 3 },
					{ "Int16[]", 4 },
					{ "Int32", 5 },
					{ "Int32[]", 6 },
					{ "Int64", 7 },
					{ "Int64[]", 8 },
					{ "DateTime", 9 },
					{ "DateTime[]", 10 },
					{ "Byte[]", 11 },
					{ "Byte[][]", 12 },
					{ "Guid", 13 },
					{ "Guid[]", 14 },
					{ "Single", 15 },
					{ "Single[]", 16 },
					{ "Double", 17 },
					{ "Double[]", 18 }
				};
			}
			if (czzgh.capxq.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					return tfexu(base.bpotr.ccfhn.ccmvk(p0, p1), (string)(object)p2);
				case 1:
					return oomjq(base.bpotr.ccfhn.ccmvk(p0, p1), (string[])(object)p2);
				case 2:
					return aayit(base.bpotr.ccfhn.ccmvk(p0, p1), (bool)(object)p2);
				case 3:
					return oxkdd(base.bpotr.ccfhn.ccmvk(p0, p1), (short)(object)p2);
				case 4:
					return lcudw(base.bpotr.ccfhn.ccmvk(p0, p1), (short[])(object)p2);
				case 5:
					return cghwx(base.bpotr.ccfhn.ccmvk(p0, p1), (int)(object)p2);
				case 6:
					return phthc(base.bpotr.ccfhn.ccmvk(p0, p1), (int[])(object)p2);
				case 7:
					return qqkwh(base.bpotr.ccfhn.ccmvk(p0, p1), (long)(object)p2);
				case 8:
					return bccrx(base.bpotr.ccfhn.ccmvk(p0, p1), (long[])(object)p2);
				case 9:
					return yxoxf(base.bpotr.ccfhn.ccmvk(p0, p1), (DateTime)(object)p2);
				case 10:
					return abymy(base.bpotr.ccfhn.ccmvk(p0, p1), (DateTime[])(object)p2);
				case 11:
					return hxrjy(base.bpotr.ccfhn.ccmvk(p0, p1), (byte[])(object)p2);
				case 12:
					return bjhdt(base.bpotr.ccfhn.ccmvk(p0, p1), (byte[][])(object)p2);
				case 13:
					return cyqbr(base.bpotr.ccfhn.ccmvk(p0, p1), (Guid)(object)p2);
				case 14:
					return tykji(base.bpotr.ccfhn.ccmvk(p0, p1), (Guid[])(object)p2);
				case 15:
					return nuitf(base.bpotr.ccfhn.ccmvk(p0, p1), (float)(object)p2);
				case 16:
					return iazlc(base.bpotr.ccfhn.ccmvk(p0, p1), (float[])(object)p2);
				case 17:
					return nuitf(base.bpotr.ccfhn.ccmvk(p0, p1), (double)(object)p2);
				case 18:
					return klwcq(base.bpotr.ccfhn.ccmvk(p0, p1), (double[])(object)p2);
				}
			}
		}
		throw new ArgumentException("Unsupported MSG data type.");
	}

	internal qacae tfexu(xrprs p0, string p1)
	{
		return duwaw((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae aayit(xrprs p0, bool p1)
	{
		return dossp((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae oxkdd(xrprs p0, short p1)
	{
		return mnfnx((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae cghwx(xrprs p0, int p1)
	{
		return mycww((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae qqkwh(xrprs p0, long p1)
	{
		return xtczv((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae nuitf(xrprs p0, double p1)
	{
		return xwaiw((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae cyqbr(xrprs p0, Guid p1)
	{
		return ezdgm((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae ivjju(xrprs p0, decimal p1)
	{
		return ibizk((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae yxoxf(xrprs p0, DateTime p1)
	{
		return bvloj((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae hxrjy(xrprs p0, byte[] p1)
	{
		return onqsx((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae oomjq(xrprs p0, string[] p1)
	{
		return owvuz((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae lcudw(xrprs p0, short[] p1)
	{
		return sqnvz((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae phthc(xrprs p0, int[] p1)
	{
		return awzyi((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae bccrx(xrprs p0, long[] p1)
	{
		return ubdne((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae iazlc(xrprs p0, float[] p1)
	{
		return dgjlx((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae klwcq(xrprs p0, double[] p1)
	{
		return gelek((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae tykji(xrprs p0, Guid[] p1)
	{
		return vtrrr((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae biawa(xrprs p0, decimal[] p1)
	{
		return zlvmb((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae abymy(xrprs p0, DateTime[] p1)
	{
		return nrjbu((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal qacae bjhdt(xrprs p0, byte[][] p1)
	{
		return fhszr((MsgPropertyTag)(32768 + p0.zcrap), p1);
	}

	internal override qacae oaesx(qacae p0)
	{
		if (uyegz.ContainsKey(p0.xablt) && 0 == 0)
		{
			if (!gixkz || 1 == 0)
			{
				throw new MsgMessageException("Collection already contains specified property (Tag:{0}).", p0.xablt);
			}
		}
		else
		{
			uyegz.Add(p0.xablt, p0);
		}
		switch (p0.hmrmu)
		{
		case mshrw.dtkaq:
			if (!eehqa.ContainsKey(p0.ifnad) || 1 == 0)
			{
				eehqa.Add(p0.ifnad, new upend<qacae>(base.bpotr));
			}
			eehqa[p0.ifnad].oaesx(p0);
			break;
		case mshrw.tkmbe:
			if (!vaerr.ContainsKey(p0.abdox) || 1 == 0)
			{
				vaerr.Add(p0.abdox, new upend<qacae>(base.bpotr));
			}
			vaerr[p0.abdox].oaesx(p0);
			break;
		default:
			throw new InvalidOperationException("Unknown kind.");
		case mshrw.isojd:
			break;
		}
		return base.oaesx(p0);
	}

	public qacae mnfnx(MsgPropertyTag p0, short p1)
	{
		return oaesx(new qacae(this, p0, xcrar.plwre, p1));
	}

	public qacae mycww(MsgPropertyTag p0, int p1)
	{
		return oaesx(new qacae(this, p0, xcrar.rjogj, p1));
	}

	public qacae xtczv(MsgPropertyTag p0, long p1)
	{
		return oaesx(new qacae(this, p0, xcrar.yzzqc, p1));
	}

	public qacae fsllm(MsgPropertyTag p0, float p1)
	{
		return oaesx(new qacae(this, p0, xcrar.xrlus, p1));
	}

	public qacae xwaiw(MsgPropertyTag p0, double p1)
	{
		return oaesx(new qacae(this, p0, xcrar.ysatw, p1));
	}

	public qacae ibizk(MsgPropertyTag p0, decimal p1)
	{
		return oaesx(new qacae(this, p0, xcrar.uvojk, p1));
	}

	public qacae udvfn(MsgPropertyTag p0, DateTime p1)
	{
		return oaesx(new qacae(this, p0, xcrar.dddyf, p1));
	}

	public qacae bvloj(MsgPropertyTag p0, DateTime p1)
	{
		return oaesx(new qacae(this, p0, xcrar.cjxwq, p1));
	}

	public qacae lkvow(MsgPropertyTag p0, int p1)
	{
		return oaesx(new qacae(this, p0, xcrar.devcm, p1));
	}

	public qacae dossp(MsgPropertyTag p0, bool p1)
	{
		return oaesx(new qacae(this, p0, xcrar.xnjos, p1));
	}

	public qacae duwaw(MsgPropertyTag p0, string p1)
	{
		return oaesx(new qacae(this, p0, (base.bpotr.kfurm ? true : false) ? xcrar.xmyux : xcrar.bkapb, p1));
	}

	public qacae onqsx(MsgPropertyTag p0, byte[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.yesjh, p1));
	}

	public qacae ezdgm(MsgPropertyTag p0, Guid p1)
	{
		return oaesx(new qacae(this, p0, xcrar.qfwqv, p1));
	}

	public qacae sqnvz(MsgPropertyTag p0, short[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.dazgb, p1));
	}

	public qacae awzyi(MsgPropertyTag p0, int[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.qkgmc, p1));
	}

	public qacae ubdne(MsgPropertyTag p0, long[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.kxwum, p1));
	}

	public qacae dgjlx(MsgPropertyTag p0, float[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.zfjkc, p1));
	}

	public qacae gelek(MsgPropertyTag p0, double[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.gibpf, p1));
	}

	public qacae zlvmb(MsgPropertyTag p0, decimal[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.yiqqk, p1));
	}

	public qacae ffhhn(MsgPropertyTag p0, DateTime[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.rkomo, p1));
	}

	public qacae nrjbu(MsgPropertyTag p0, DateTime[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.mctvt, p1));
	}

	public qacae vtrrr(MsgPropertyTag p0, Guid[] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.fawvf, p1));
	}

	public qacae owvuz(MsgPropertyTag p0, string[] p1)
	{
		return oaesx(new qacae(this, p0, (base.bpotr.kfurm ? true : false) ? xcrar.wymks : xcrar.kkdjd, p1));
	}

	public qacae fhszr(MsgPropertyTag p0, byte[][] p1)
	{
		return oaesx(new qacae(this, p0, xcrar.vbevq, p1));
	}
}
