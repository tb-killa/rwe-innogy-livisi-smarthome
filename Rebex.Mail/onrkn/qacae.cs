using System;
using System.IO;
using System.Text;
using Rebex;
using Rebex.Mail;
using Rebex.OutlookMessages;

namespace onrkn;

internal class qacae
{
	private readonly howhn fgrqq;

	private readonly string mrxro;

	private readonly int ijmlw;

	private readonly bool dhbij;

	private readonly uint vpnyv;

	private readonly MsgPropertyTag yfrku;

	private xrprs kpfzg;

	private mshrw qoddj;

	private readonly xcrar oxbgv;

	private readonly kerad vnlvx;

	private int okugo;

	private object fkfgu;

	private byte[] kbeuk;

	private bool ejxtt;

	private object eqgzk;

	internal bool fehzf => dhbij;

	internal jfxnb tinxh => fgrqq.bpotr;

	internal howhn eybpm => fgrqq;

	internal object xgigm
	{
		get
		{
			return eqgzk;
		}
		set
		{
			eqgzk = value;
		}
	}

	internal xrprs ryfdm => kpfzg;

	private uint xshzh
	{
		get
		{
			if (tinxh.kfurm && 0 == 0)
			{
				if (oxbgv == xcrar.bkapb)
				{
					return (uint)(((int)yfrku << 16) | 0x1F);
				}
				if (oxbgv == xcrar.kkdjd)
				{
					return (uint)(((int)yfrku << 16) | 0x101F);
				}
			}
			return vpnyv;
		}
	}

	public mshrw hmrmu => qoddj;

	public MsgPropertyTag xablt => yfrku;

	public MsgPropertyId ifnad
	{
		get
		{
			if (kpfzg == null || false || !kpfzg.jisdp || 1 == 0)
			{
				throw new InvalidOperationException("Property is not Numerical named.");
			}
			return kpfzg.hnnrt;
		}
	}

	public string abdox
	{
		get
		{
			if (kpfzg == null || false || kpfzg.jisdp)
			{
				throw new InvalidOperationException("Property is not String named.");
			}
			return kpfzg.kmaxh;
		}
	}

	public xcrar pzpvc
	{
		get
		{
			if (tinxh.kfurm && 0 == 0)
			{
				if (oxbgv == xcrar.bkapb)
				{
					return xcrar.xmyux;
				}
				if (oxbgv == xcrar.kkdjd)
				{
					return xcrar.wymks;
				}
			}
			return oxbgv;
		}
	}

	public object tgbhs
	{
		get
		{
			if (!ejxtt || 1 == 0)
			{
				if (ijmlw != tinxh.meqdt)
				{
					throw new InvalidOperationException("Object is no longer valid because the owner message was reloaded.");
				}
				jvnbe();
			}
			return fkfgu;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value", "Value cannot be null.");
			}
			switch (oxbgv)
			{
			case xcrar.plwre:
			case xcrar.rjogj:
			case xcrar.xrlus:
			case xcrar.ysatw:
			case xcrar.dddyf:
			case xcrar.xnjos:
			case xcrar.yzzqc:
			case xcrar.bkapb:
			case xcrar.xmyux:
			case xcrar.cjxwq:
			case xcrar.qfwqv:
			case xcrar.yesjh:
			case xcrar.dazgb:
			case xcrar.qkgmc:
			case xcrar.zfjkc:
			case xcrar.gibpf:
			case xcrar.rkomo:
			case xcrar.kxwum:
			case xcrar.kkdjd:
			case xcrar.wymks:
			case xcrar.mctvt:
			case xcrar.fawvf:
			case xcrar.vbevq:
				if (!dzwgu.gesog(value.GetType(), oxbgv) || 1 == 0)
				{
					throw new ArgumentException("Property data type does not match the type of the value.", "value");
				}
				break;
			case xcrar.devcm:
				if (!(value is int) || 1 == 0)
				{
					throw new ArgumentException("Property data type does not match the type of the value.", "value");
				}
				break;
			default:
				throw new NotSupportedException(brgjd.edcru("Setting a value to property of data type '{0}' is not supported.", oxbgv));
			}
			vldpd(value);
		}
	}

	internal qacae(howhn owner, string storagePath, byte[] data, bool readVariableLengthData)
	{
		if (owner == null || 1 == 0)
		{
			throw new ArgumentNullException("owner", "Owner cannot be null.");
		}
		if (storagePath == null || 1 == 0)
		{
			throw new ArgumentNullException("storagePath", "Path cannot be null.");
		}
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data", "Buffer cannot be null.");
		}
		if (data.Length < 16)
		{
			throw new ArgumentException("Invalid data length.", "data");
		}
		fgrqq = owner;
		mrxro = storagePath;
		ijmlw = tinxh.meqdt;
		vpnyv = jlfbq.ubcts(data, 0);
		yfrku = (MsgPropertyTag)jlfbq.sunaw(data, 2);
		oxbgv = (xcrar)jlfbq.sunaw(data, 0);
		vnlvx = (kerad)jlfbq.nlfaq(data, 4);
		dhbij = bibpw(oxbgv);
		jabhu();
		if (dhbij && 0 == 0)
		{
			zwdcw(data);
		}
		else if (oxbgv == xcrar.xmyux)
		{
			okugo = jlfbq.nlfaq(data, 8) - 2;
		}
		else if (oxbgv == xcrar.bkapb)
		{
			okugo = jlfbq.nlfaq(data, 8) - 1;
		}
		else
		{
			if (!Enum.IsDefined(typeof(xcrar), oxbgv) || 1 == 0)
			{
				throw new MsgMessageException("MSG file specifies invalid data type for the {0} property ({1:X8}).", jpkwk(), vpnyv);
			}
			okugo = jlfbq.nlfaq(data, 8);
		}
		if ((!dhbij || 1 == 0) && readVariableLengthData && 0 == 0)
		{
			jvnbe();
		}
	}

	internal qacae(howhn owner, MsgPropertyTag tag, xcrar dataType, object value)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value", "Value cannot be null.");
		}
		fgrqq = owner;
		yfrku = tag;
		oxbgv = dataType;
		vnlvx = kerad.scqzc | kerad.omaxj;
		vpnyv = (uint)((int)yfrku << 16) | (uint)pzpvc;
		dhbij = bibpw(dataType);
		jabhu();
		vldpd(value);
	}

	private void jabhu()
	{
		if (vpnyv < 2147483648u)
		{
			return;
		}
		int num = (int)((vpnyv >> 16) - 32768);
		if (num < tinxh.ccfhn.fnqqt)
		{
			kpfzg = tinxh.ccfhn[num];
			if (tinxh.ccfhn[num].jisdp && 0 == 0)
			{
				qoddj = mshrw.dtkaq;
			}
			else
			{
				qoddj = mshrw.tkmbe;
			}
		}
	}

	private void vldpd(object p0)
	{
		fkfgu = p0;
		ejxtt = true;
		kbeuk = null;
		switch (oxbgv)
		{
		case xcrar.yesjh:
			okugo = ((byte[])p0).Length;
			break;
		case xcrar.qfwqv:
			okugo = 16;
			break;
		case xcrar.xkvvt:
			okugo = -1;
			break;
		case xcrar.dazgb:
			okugo = ((short[])p0).Length << 1;
			break;
		case xcrar.qkgmc:
			okugo = ((int[])p0).Length << 2;
			break;
		case xcrar.zfjkc:
			okugo = ((float[])p0).Length << 2;
			break;
		case xcrar.gibpf:
			okugo = ((double[])p0).Length << 3;
			break;
		case xcrar.yiqqk:
			okugo = ((decimal[])p0).Length << 3;
			break;
		case xcrar.rkomo:
			okugo = ((DateTime[])p0).Length << 3;
			break;
		case xcrar.kxwum:
			okugo = ((long[])p0).Length << 3;
			break;
		case xcrar.mctvt:
			okugo = ((DateTime[])p0).Length << 3;
			break;
		case xcrar.fawvf:
			okugo = ((Guid[])p0).Length << 4;
			break;
		case xcrar.kkdjd:
		case xcrar.wymks:
			okugo = ((string[])p0).Length << 2;
			break;
		case xcrar.vbevq:
			okugo = ((byte[][])p0).Length << 3;
			break;
		}
	}

	private bool bibpw(xcrar p0)
	{
		switch (p0)
		{
		case xcrar.plwre:
		case xcrar.rjogj:
		case xcrar.xrlus:
		case xcrar.ysatw:
		case xcrar.uvojk:
		case xcrar.dddyf:
		case xcrar.devcm:
		case xcrar.xnjos:
		case xcrar.yzzqc:
		case xcrar.cjxwq:
			return true;
		default:
			return false;
		}
	}

	private bool vwiyh()
	{
		if (pzpvc != xcrar.xmyux)
		{
			return pzpvc == xcrar.wymks;
		}
		return true;
	}

	private int gnryc(string p0)
	{
		if (tinxh.xewra.rcvvh && 0 == 0 && p0.Length > 0 && (yfrku == MsgPropertyTag.DisplayTo || yfrku == MsgPropertyTag.DisplayCc || yfrku == MsgPropertyTag.DisplayBcc))
		{
			return hlysf(p0) + ((!vwiyh() || 1 == 0) ? 1 : 2);
		}
		return hlysf(p0);
	}

	private int hlysf(string p0)
	{
		if ((tinxh.kfurm ? true : false) || oxbgv == xcrar.xmyux || oxbgv == xcrar.wymks)
		{
			return Encoding.Unicode.GetByteCount(p0);
		}
		if (yfrku == MsgPropertyTag.Body || yfrku == MsgPropertyTag.BodyHtml)
		{
			return tinxh.haost.GetByteCount(p0);
		}
		return tinxh.mxgar.GetByteCount(p0);
	}

	private void zwdcw(byte[] p0)
	{
		ejxtt = true;
		switch (oxbgv)
		{
		case xcrar.plwre:
			fkfgu = jlfbq.sunaw(p0, 8);
			break;
		case xcrar.rjogj:
			fkfgu = jlfbq.nlfaq(p0, 8);
			break;
		case xcrar.xrlus:
			fkfgu = jlfbq.tcekw(p0, 8);
			break;
		case xcrar.ysatw:
			fkfgu = jlfbq.iydrg(p0, 8);
			break;
		case xcrar.uvojk:
			fkfgu = kjncd.gndni(jlfbq.epwpf(p0, 8));
			break;
		case xcrar.dddyf:
			fkfgu = kjncd.xpkid(jlfbq.iydrg(p0, 8));
			break;
		case xcrar.devcm:
			fkfgu = jlfbq.nlfaq(p0, 8);
			break;
		case xcrar.xnjos:
			fkfgu = jlfbq.sunaw(p0, 8) != 0;
			break;
		case xcrar.yzzqc:
			fkfgu = jlfbq.epwpf(p0, 8);
			break;
		case xcrar.cjxwq:
			try
			{
				fkfgu = DateTime.FromFileTime(jlfbq.epwpf(p0, 8)).ToUniversalTime();
				break;
			}
			catch (ArgumentOutOfRangeException)
			{
				fkfgu = DateTime.MinValue;
				break;
			}
		default:
			throw new InvalidOperationException("Property has no fixed data.");
		}
	}

	internal void jvnbe()
	{
		ejxtt = true;
		if (oxbgv == xcrar.xomkp || false || oxbgv == xcrar.crqsh)
		{
			return;
		}
		string text = jxtqv.motnn(mrxro, brgjd.edcru("{0}{1:X8}", "__substg1.0_", vpnyv));
		byte[] p;
		jnkze jnkze2;
		if (yfrku == MsgPropertyTag.AttachDataBinary && oxbgv == xcrar.xkvvt)
		{
			qacae qacae2 = eybpm[MsgPropertyTag.AttachMethod];
			if (qacae2 != null && 0 == 0)
			{
				switch ((ulypf)qacae2.tgbhs)
				{
				case ulypf.goblg:
				{
					jfxnb jfxnb2 = new jfxnb(tinxh);
					jfxnb2.wvcnp(text);
					fkfgu = jfxnb2;
					return;
				}
				case ulypf.uvcfl:
					jnkze2 = tinxh.eulhi.vmjtu(jxtqv.motnn(text, "CONTENTS"));
					if (jnkze2 != null && 0 == 0)
					{
						p = new byte[jnkze2.obvof];
						jnkze2.mzhde().hvees(p);
						fkfgu = p;
					}
					return;
				}
			}
		}
		jnkze2 = tinxh.eulhi.vmjtu(text);
		rnsvi rnsvi2;
		if (jnkze2 == null || 1 == 0)
		{
			rnsvi2 = new chhth();
		}
		else
		{
			if (!jnkze2.qrqby || 1 == 0)
			{
				throw new MsgMessageException("Unknown structure of the {0} property (ID:0x{1:X8}).", jpkwk(), vpnyv);
			}
			rnsvi2 = jnkze2.mzhde();
		}
		if (okugo <= 0 || okugo > rnsvi2.Length)
		{
			okugo = (int)Math.Min(rnsvi2.Length, 2147483647L);
		}
		else if (dzwgu.dovit(oxbgv) && 0 == 0)
		{
			if (rnsvi2.Length != okugo && rnsvi2.Length != okugo + ((!tinxh.kfurm || 1 == 0) ? 1 : 2))
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
		}
		else if (rnsvi2.Length != okugo)
		{
			throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
		}
		short[] array6;
		int num7;
		int[] array3;
		int num3;
		float[] array9;
		int num10;
		double[] array7;
		int num8;
		decimal[] array4;
		int num4;
		DateTime[] array10;
		int num11;
		long[] array5;
		int num6;
		DateTime[] array2;
		int num2;
		Guid[] array11;
		int num12;
		string[] array8;
		int num9;
		byte[][] array;
		int num;
		ref decimal reference;
		int num5;
		xxolr xxolr2;
		xxolr xxolr3;
		ref DateTime reference3;
		ref Guid reference4;
		switch (oxbgv)
		{
		case xcrar.xmyux:
			if (yfrku == MsgPropertyTag.Body || yfrku == MsgPropertyTag.BodyHtml)
			{
				fkfgu = jkhfe(rnsvi2, Encoding.Unicode, tinxh.chnlh(p0: false));
			}
			else
			{
				fkfgu = jkhfe(rnsvi2, Encoding.Unicode, tinxh.bucji(p0: false));
			}
			break;
		case xcrar.bkapb:
			if (yfrku == MsgPropertyTag.Body || yfrku == MsgPropertyTag.BodyHtml)
			{
				fkfgu = jkhfe(rnsvi2, tinxh.bzoqu, EncodingTools.Default);
			}
			else
			{
				fkfgu = jkhfe(rnsvi2, tinxh.ifzxe, EncodingTools.Default);
			}
			break;
		case xcrar.yesjh:
			kbeuk = new byte[okugo];
			rnsvi2.hvees(kbeuk);
			fkfgu = kbeuk;
			break;
		case xcrar.qfwqv:
			if (okugo == 0 || 1 == 0)
			{
				fkfgu = Guid.Empty;
				break;
			}
			if (okugo != 16)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			kbeuk = new byte[16];
			rnsvi2.hvees(kbeuk);
			fkfgu = new Guid(kbeuk);
			break;
		case xcrar.dazgb:
			if ((okugo & 1) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array6 = new short[okugo >> 1];
			num7 = 0;
			if (num7 != 0)
			{
				goto IL_051a;
			}
			goto IL_052b;
		case xcrar.qkgmc:
			if ((okugo & 3) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array3 = new int[okugo >> 2];
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_0592;
			}
			goto IL_05a3;
		case xcrar.zfjkc:
			p = new byte[4];
			if ((okugo & 3) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array9 = new float[okugo >> 2];
			num10 = 0;
			if (num10 != 0)
			{
				goto IL_0611;
			}
			goto IL_062a;
		case xcrar.gibpf:
			p = new byte[8];
			if ((okugo & 7) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array7 = new double[okugo >> 3];
			num8 = 0;
			if (num8 != 0)
			{
				goto IL_0698;
			}
			goto IL_06b1;
		case xcrar.yiqqk:
			p = new byte[8];
			if ((okugo & 7) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array4 = new decimal[okugo >> 3];
			num4 = 0;
			if (num4 != 0)
			{
				goto IL_071f;
			}
			goto IL_0746;
		case xcrar.rkomo:
			p = new byte[8];
			if ((okugo & 7) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array10 = new DateTime[okugo >> 3];
			num11 = 0;
			if (num11 != 0)
			{
				goto IL_07b4;
			}
			goto IL_07db;
		case xcrar.kxwum:
			if ((okugo & 7) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array5 = new long[okugo >> 3];
			num6 = 0;
			if (num6 != 0)
			{
				goto IL_0842;
			}
			goto IL_0853;
		case xcrar.mctvt:
			if ((okugo & 7) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array2 = new DateTime[okugo >> 3];
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_08ba;
			}
			goto IL_08f7;
		case xcrar.fawvf:
			if ((okugo & 0xF) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			p = new byte[16];
			array11 = new Guid[okugo >> 4];
			num12 = 0;
			if (num12 != 0)
			{
				goto IL_0967;
			}
			goto IL_0988;
		case xcrar.kkdjd:
		case xcrar.wymks:
			if ((okugo & 3) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array8 = new string[okugo >> 2];
			num9 = 0;
			if (num9 != 0)
			{
				goto IL_09f2;
			}
			goto IL_0aed;
		case xcrar.vbevq:
			if ((okugo & 7) != 0 && 0 == 0)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:0x{1:X8}) is invalid.", jpkwk(), vpnyv);
			}
			array = new byte[okugo >> 3][];
			num = 0;
			if (num != 0)
			{
				goto IL_0b5a;
			}
			goto IL_0c2f;
		default:
			throw new InvalidOperationException("Property has no variable length data.");
		case xcrar.xkvvt:
		case xcrar.nvjck:
		case xcrar.lzzsa:
		case xcrar.oihjp:
			break;
			IL_08f7:
			if (num2 >= array2.Length)
			{
				fkfgu = array2;
				break;
			}
			goto IL_08ba;
			IL_05a3:
			if (num3 >= array3.Length)
			{
				fkfgu = array3;
				break;
			}
			goto IL_0592;
			IL_071f:
			rnsvi2.hvees(p);
			reference = ref array4[num4];
			reference = kjncd.gndni(jlfbq.epwpf(p, 8));
			num4++;
			goto IL_0746;
			IL_0c2f:
			if (num < array.Length)
			{
				goto IL_0b5a;
			}
			fkfgu = array;
			break;
			IL_08ba:
			try
			{
				ref DateTime reference2 = ref array2[num2];
				reference2 = DateTime.FromFileTime(rnsvi2.sftyl()).ToUniversalTime();
			}
			catch (ArgumentOutOfRangeException)
			{
				fkfgu = DateTime.MinValue;
			}
			num2++;
			goto IL_08f7;
			IL_0b5a:
			num5 = rnsvi2.wfljb();
			rnsvi2.Seek(4L, SeekOrigin.Current);
			jnkze2 = tinxh.eulhi.vmjtu(text + brgjd.edcru("-{0:X8}", num));
			if (jnkze2 == null || 1 == 0)
			{
				throw new MsgMessageException("MSG file doesn't contain data stream for the {0} property (ID:{1}).", jpkwk(), jnkze2.ucwew);
			}
			xxolr2 = jnkze2.mzhde();
			if (xxolr2.Length != num5)
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:{1}) is invalid.", jpkwk(), jnkze2.ucwew);
			}
			p = new byte[num5];
			xxolr2.hvees(p);
			array[num] = p;
			num++;
			goto IL_0c2f;
			IL_051a:
			array6[num7] = rnsvi2.ecwzy();
			num7++;
			goto IL_052b;
			IL_0698:
			rnsvi2.hvees(p);
			array7[num8] = jlfbq.iydrg(p, 0);
			num8++;
			goto IL_06b1;
			IL_06b1:
			if (num8 >= array7.Length)
			{
				fkfgu = array7;
				break;
			}
			goto IL_0698;
			IL_0842:
			array5[num6] = rnsvi2.sftyl();
			num6++;
			goto IL_0853;
			IL_0853:
			if (num6 >= array5.Length)
			{
				fkfgu = array5;
				break;
			}
			goto IL_0842;
			IL_0aed:
			if (num9 >= array8.Length)
			{
				fkfgu = array8;
				break;
			}
			goto IL_09f2;
			IL_09f2:
			jnkze2 = tinxh.eulhi.vmjtu(text + brgjd.edcru("-{0:X8}", num9));
			if (jnkze2 == null || 1 == 0)
			{
				throw new MsgMessageException("MSG file doesn't contain data stream for the {0} property (ID:{1}).", jpkwk(), jnkze2.ucwew);
			}
			xxolr3 = jnkze2.mzhde();
			if (xxolr3.Length != rnsvi2.wfljb())
			{
				throw new MsgMessageException("Stream length for the {0} property (ID:{1}) is invalid.", jpkwk(), jnkze2.ucwew);
			}
			if (oxbgv == xcrar.wymks)
			{
				array8[num9] = jkhfe(xxolr3, Encoding.Unicode, tinxh.bucji(p0: false));
			}
			else
			{
				array8[num9] = jkhfe(xxolr3, tinxh.ifzxe, EncodingTools.Default);
			}
			num9++;
			goto IL_0aed;
			IL_0592:
			array3[num3] = rnsvi2.wfljb();
			num3++;
			goto IL_05a3;
			IL_07db:
			if (num11 >= array10.Length)
			{
				fkfgu = array10;
				break;
			}
			goto IL_07b4;
			IL_062a:
			if (num10 >= array9.Length)
			{
				fkfgu = array9;
				break;
			}
			goto IL_0611;
			IL_0611:
			rnsvi2.hvees(p);
			array9[num10] = jlfbq.tcekw(p, 0);
			num10++;
			goto IL_062a;
			IL_07b4:
			rnsvi2.hvees(p);
			reference3 = ref array10[num11];
			reference3 = kjncd.xpkid(jlfbq.iydrg(p, 8));
			num11++;
			goto IL_07db;
			IL_0746:
			if (num4 >= array4.Length)
			{
				fkfgu = array4;
				break;
			}
			goto IL_071f;
			IL_0988:
			if (num12 >= array11.Length)
			{
				fkfgu = array11;
				break;
			}
			goto IL_0967;
			IL_052b:
			if (num7 >= array6.Length)
			{
				fkfgu = array6;
				break;
			}
			goto IL_051a;
			IL_0967:
			rnsvi2.hvees(p);
			reference4 = ref array11[num12];
			reference4 = new Guid(p);
			num12++;
			goto IL_0988;
		}
	}

	private string jkhfe(rnsvi p0, Encoding p1, Encoding p2)
	{
		kbeuk = p0.uxseg();
		if (kbeuk.Length == 0 || 1 == 0)
		{
			return string.Empty;
		}
		string text;
		if ((!tinxh.xewra.ryjny || 1 == 0) && p1.WebName.Equals("utf-16", StringComparison.OrdinalIgnoreCase) && 0 == 0 && (!EncodingTools.azzem(kbeuk) || 1 == 0))
		{
			rltew rltew2;
			switch (oxbgv)
			{
			case xcrar.bkapb:
			case xcrar.kkdjd:
				rltew2 = tinxh.xewra.ltbiy;
				break;
			case xcrar.xmyux:
			case xcrar.wymks:
				rltew2 = tinxh.xewra.rhkxd;
				break;
			default:
				throw new InvalidOperationException("Unsupported value.");
			}
			bool flag;
			switch (rltew2)
			{
			case rltew.rvtdm:
				flag = false;
				if (!flag)
				{
					break;
				}
				goto case rltew.craxt;
			case rltew.craxt:
				flag = true;
				if (flag)
				{
					break;
				}
				goto case rltew.vxeuu;
			case rltew.vxeuu:
				flag = rtqlm();
				break;
			case rltew.ewygk:
				flag = yfrku == MsgPropertyTag.Body || yfrku == MsgPropertyTag.BodyHtml;
				break;
			default:
				throw new InvalidOperationException("Unsupported value.");
			}
			if (flag && 0 == 0)
			{
				text = p2.GetString(kbeuk, 0, kbeuk.Length);
				string text2 = text;
				char[] trimChars = new char[1];
				text = text2.TrimEnd(trimChars);
				if (aoyqp(text) && 0 == 0)
				{
					return text;
				}
			}
		}
		text = p1.GetString(kbeuk, 0, kbeuk.Length);
		string text3 = text;
		char[] trimChars2 = new char[1];
		return text3.TrimEnd(trimChars2);
	}

	private bool aoyqp(string p0)
	{
		int num;
		if (rtqlm())
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0015;
			}
			goto IL_0047;
		}
		return p0.IndexOf('\0') < 0;
		IL_0015:
		int num2 = p0[num];
		if (num2 < 32)
		{
			switch (num2)
			{
			default:
				return false;
			case 9:
			case 10:
			case 13:
				break;
			}
		}
		num++;
		goto IL_0047;
		IL_0047:
		if (num < p0.Length)
		{
			goto IL_0015;
		}
		return true;
	}

	private bool rtqlm()
	{
		switch (yfrku)
		{
		case MsgPropertyTag.Subject:
		case MsgPropertyTag.SentRepresentingName:
		case MsgPropertyTag.ConversationTopic:
		case MsgPropertyTag.SenderName:
		case MsgPropertyTag.DisplayBcc:
		case MsgPropertyTag.DisplayCc:
		case MsgPropertyTag.DisplayTo:
		case MsgPropertyTag.NormalizedSubject:
		case MsgPropertyTag.Body:
		case MsgPropertyTag.BodyHtml:
		case MsgPropertyTag.DisplayName:
		case MsgPropertyTag.GivenName:
		case MsgPropertyTag.Initials:
		case MsgPropertyTag.Surname:
		case MsgPropertyTag.PostalAddress:
		case MsgPropertyTag.CompanyName:
		case MsgPropertyTag.Title:
		case MsgPropertyTag.DepartmentName:
		case MsgPropertyTag.Country:
		case MsgPropertyTag.Locality:
		case MsgPropertyTag.StateOrProvince:
		case MsgPropertyTag.StreetAddress:
		case MsgPropertyTag.PostalCode:
		case MsgPropertyTag.MiddleName:
		case MsgPropertyTag.HomeAddressCity:
		case MsgPropertyTag.LastModifierName:
			return true;
		default:
			if (qoddj == mshrw.dtkaq && kpfzg != null && 0 == 0)
			{
				switch (kpfzg.hnnrt)
				{
				case MsgPropertyId.FileUnder:
				case MsgPropertyId.WorkAddress:
				case MsgPropertyId.WorkAddressStreet:
				case MsgPropertyId.WorkAddressCity:
				case MsgPropertyId.WorkAddressState:
				case MsgPropertyId.WorkAddressCountry:
				case MsgPropertyId.InternetAccountName:
					return true;
				}
			}
			return false;
		}
	}

	internal void jagfb(BinaryWriter p0, byte[] p1)
	{
		p0.Write(xshzh);
		p0.Write((int)vnlvx);
		if (yfrku == MsgPropertyTag.AttachDataBinary && oxbgv == xcrar.xkvvt)
		{
			qacae qacae2 = eybpm[MsgPropertyTag.AttachMethod];
			if (qacae2 != null && 0 == 0 && (int)qacae2.tgbhs == 5)
			{
				p0.Write(-1);
				p0.Write(1);
				return;
			}
		}
		if (dhbij && 0 == 0)
		{
			nnkwh(p0, p1);
		}
		else if (pzpvc == xcrar.xmyux)
		{
			p0.Write((long)gnryc((string)fkfgu) + 2L);
		}
		else if (pzpvc == xcrar.bkapb)
		{
			p0.Write((long)gnryc((string)fkfgu) + 1L);
		}
		else
		{
			p0.Write((long)okugo);
		}
	}

	private void nnkwh(BinaryWriter p0, byte[] p1)
	{
		switch (oxbgv)
		{
		case xcrar.plwre:
			p0.Write((short)fkfgu);
			p0.Write(p1, 0, 6);
			break;
		case xcrar.rjogj:
			p0.Write((int)fkfgu);
			p0.Write(p1, 0, 4);
			break;
		case xcrar.xrlus:
			p0.Write((float)fkfgu);
			p0.Write(p1, 0, 4);
			break;
		case xcrar.ysatw:
			p0.Write((double)fkfgu);
			break;
		case xcrar.uvojk:
			p0.Write(kjncd.wdexs((decimal)fkfgu));
			break;
		case xcrar.dddyf:
			p0.Write(kjncd.hlvpo((DateTime)fkfgu));
			break;
		case xcrar.devcm:
			p0.Write((int)fkfgu);
			p0.Write(p1, 0, 4);
			break;
		case xcrar.xnjos:
			p0.Write((short)((((bool)fkfgu) ? true : false) ? 1 : 0));
			p0.Write(p1, 0, 6);
			break;
		case xcrar.yzzqc:
			p0.Write((long)fkfgu);
			break;
		case xcrar.cjxwq:
			p0.Write(((DateTime)fkfgu).ToUniversalTime().ToFileTime());
			break;
		default:
			throw new InvalidOperationException("Property has no fixed length data.");
		}
	}

	internal void nazyn(duqmg p0, uuipu p1, string p2)
	{
		if (yfrku == MsgPropertyTag.AttachDataBinary && oxbgv == xcrar.xkvvt)
		{
			qacae qacae2 = eybpm[MsgPropertyTag.AttachMethod];
			if (qacae2 != null && 0 == 0 && (ulypf)qacae2.tgbhs == ulypf.goblg)
			{
				return;
			}
		}
		string text = jxtqv.motnn(p2, brgjd.edcru("{0}{1:X8}", "__substg1.0_", xshzh));
		if (oxbgv == xcrar.xomkp || false || oxbgv == xcrar.crqsh)
		{
			p0.unyyx(text, p1.BaseStream, 0L);
			return;
		}
		int num = 0;
		short[] array12;
		int num12;
		int[] array11;
		int num11;
		float[] array10;
		int num10;
		double[] array9;
		int num9;
		decimal[] array8;
		int num8;
		DateTime[] array7;
		int num7;
		long[] array6;
		int num6;
		DateTime[] array5;
		int num5;
		Guid[] array4;
		int num4;
		string[] array3;
		int num3;
		byte[][] array;
		int num2;
		byte[] array2;
		string text2;
		Stream stream;
		Guid guid;
		DateTime dateTime;
		long value;
		DateTime p3;
		decimal p4;
		double value2;
		float value3;
		int value4;
		short value5;
		switch (pzpvc)
		{
		case xcrar.nvjck:
			throw new NotSupportedException("Data type ServerId is not supported yet.");
		case xcrar.xkvvt:
			throw new NotSupportedException("Data type ComObject is not supported yet.");
		case xcrar.lzzsa:
			throw new NotSupportedException("Data type Restriction is not supported yet.");
		case xcrar.oihjp:
			throw new NotSupportedException("Data type RuleAction is not supported yet.");
		case xcrar.xmyux:
			p1.Write(Encoding.Unicode.GetBytes((string)fkfgu));
			if (tinxh.xewra.rcvvh && 0 == 0 && ((string)fkfgu).Length > 0 && (yfrku == MsgPropertyTag.DisplayTo || yfrku == MsgPropertyTag.DisplayCc || yfrku == MsgPropertyTag.DisplayBcc))
			{
				p1.Write((short)0);
			}
			break;
		case xcrar.bkapb:
			if (yfrku == MsgPropertyTag.Body || yfrku == MsgPropertyTag.BodyHtml)
			{
				p1.Write(tinxh.haost.GetBytes((string)fkfgu));
			}
			else
			{
				p1.Write(tinxh.mxgar.GetBytes((string)fkfgu));
			}
			if (tinxh.xewra.rcvvh && 0 == 0 && ((string)fkfgu).Length > 0 && (yfrku == MsgPropertyTag.DisplayTo || yfrku == MsgPropertyTag.DisplayCc || yfrku == MsgPropertyTag.DisplayBcc))
			{
				p1.Write((byte)0);
			}
			break;
		case xcrar.yesjh:
			p1.Write((byte[])fkfgu);
			break;
		case xcrar.qfwqv:
			p1.Write(((Guid)fkfgu).ToByteArray());
			break;
		case xcrar.dazgb:
			array12 = (short[])fkfgu;
			num12 = 0;
			if (num12 != 0)
			{
				goto IL_034e;
			}
			goto IL_0361;
		case xcrar.qkgmc:
			array11 = (int[])fkfgu;
			num11 = 0;
			if (num11 != 0)
			{
				goto IL_0382;
			}
			goto IL_0397;
		case xcrar.zfjkc:
			array10 = (float[])fkfgu;
			num10 = 0;
			if (num10 != 0)
			{
				goto IL_03b8;
			}
			goto IL_03ce;
		case xcrar.gibpf:
			array9 = (double[])fkfgu;
			num9 = 0;
			if (num9 != 0)
			{
				goto IL_03ef;
			}
			goto IL_0405;
		case xcrar.yiqqk:
			array8 = (decimal[])fkfgu;
			num8 = 0;
			if (num8 != 0)
			{
				goto IL_0426;
			}
			goto IL_0449;
		case xcrar.rkomo:
			array7 = (DateTime[])fkfgu;
			num7 = 0;
			if (num7 != 0)
			{
				goto IL_046a;
			}
			goto IL_048d;
		case xcrar.kxwum:
			array6 = (long[])fkfgu;
			num6 = 0;
			if (num6 != 0)
			{
				goto IL_04ae;
			}
			goto IL_04c3;
		case xcrar.mctvt:
			array5 = (DateTime[])fkfgu;
			num5 = 0;
			if (num5 != 0)
			{
				goto IL_04e4;
			}
			goto IL_0510;
		case xcrar.fawvf:
			array4 = (Guid[])fkfgu;
			num4 = 0;
			if (num4 != 0)
			{
				goto IL_0531;
			}
			goto IL_0554;
		case xcrar.kkdjd:
		case xcrar.wymks:
			array3 = (string[])fkfgu;
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_0578;
			}
			goto IL_0641;
		case xcrar.vbevq:
			array = (byte[][])fkfgu;
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_0665;
			}
			goto IL_06bf;
		default:
			{
				throw new InvalidOperationException("Property has no variable length data.");
			}
			IL_06bf:
			if (num2 >= array.Length)
			{
				break;
			}
			goto IL_0665;
			IL_0665:
			array2 = array[num2];
			p1.Write(array2.Length);
			p1.Write(0);
			p0.bwhtj(text + brgjd.edcru("-{0:X8}", num++), array2);
			num2++;
			goto IL_06bf;
			IL_0641:
			if (num3 >= array3.Length)
			{
				break;
			}
			goto IL_0578;
			IL_0578:
			text2 = array3[num3];
			if (text2.Length == 0 || 1 == 0)
			{
				stream = new MemoryStream();
			}
			else if (pzpvc == xcrar.wymks)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(text2);
				stream = new jiqyi(bytes, new MemoryStream(new byte[2]), 0L);
			}
			else
			{
				byte[] bytes = tinxh.mxgar.GetBytes(text2);
				stream = new jiqyi(bytes, new MemoryStream(new byte[1]), 0L);
			}
			p1.Write((int)stream.Length);
			p0.unyyx(text + brgjd.edcru("-{0:X8}", num++), stream, stream.Length);
			num3++;
			goto IL_0641;
			IL_0554:
			if (num4 >= array4.Length)
			{
				break;
			}
			goto IL_0531;
			IL_0531:
			guid = array4[num4];
			p1.Write(guid.ToByteArray());
			num4++;
			goto IL_0554;
			IL_0510:
			if (num5 >= array5.Length)
			{
				break;
			}
			goto IL_04e4;
			IL_04e4:
			dateTime = array5[num5];
			p1.Write(dateTime.ToUniversalTime().ToFileTime());
			num5++;
			goto IL_0510;
			IL_04c3:
			if (num6 >= array6.Length)
			{
				break;
			}
			goto IL_04ae;
			IL_04ae:
			value = array6[num6];
			p1.Write(value);
			num6++;
			goto IL_04c3;
			IL_048d:
			if (num7 >= array7.Length)
			{
				break;
			}
			goto IL_046a;
			IL_046a:
			p3 = array7[num7];
			p1.Write(kjncd.hlvpo(p3));
			num7++;
			goto IL_048d;
			IL_0449:
			if (num8 >= array8.Length)
			{
				break;
			}
			goto IL_0426;
			IL_0426:
			p4 = array8[num8];
			p1.Write(kjncd.wdexs(p4));
			num8++;
			goto IL_0449;
			IL_0405:
			if (num9 >= array9.Length)
			{
				break;
			}
			goto IL_03ef;
			IL_03ef:
			value2 = array9[num9];
			p1.Write(value2);
			num9++;
			goto IL_0405;
			IL_03ce:
			if (num10 >= array10.Length)
			{
				break;
			}
			goto IL_03b8;
			IL_03b8:
			value3 = array10[num10];
			p1.Write(value3);
			num10++;
			goto IL_03ce;
			IL_0397:
			if (num11 >= array11.Length)
			{
				break;
			}
			goto IL_0382;
			IL_0382:
			value4 = array11[num11];
			p1.Write(value4);
			num11++;
			goto IL_0397;
			IL_0361:
			if (num12 >= array12.Length)
			{
				break;
			}
			goto IL_034e;
			IL_034e:
			value5 = array12[num12];
			p1.Write(value5);
			num12++;
			goto IL_0361;
		}
		p0.unyyx(text, p1.BaseStream, p1.fyrnh);
	}

	public byte[] vvwfy()
	{
		if (!ejxtt || 1 == 0)
		{
			if (ijmlw != tinxh.meqdt)
			{
				throw new InvalidOperationException("Object is no longer valid because the owner message was reloaded.");
			}
			jvnbe();
		}
		if (kbeuk != null && 0 == 0)
		{
			return kbeuk.aqhfc();
		}
		return null;
	}

	public override string ToString()
	{
		return brgjd.edcru("{0}{1:X8} ({2} {3}) [{4}]: {5}", "__substg1.0_", vpnyv, jpkwk(), oxbgv, (dhbij ? true : false) ? "FIX" : okugo.ToString(), bujmq());
	}

	public string kuroo()
	{
		return brgjd.edcru("{0}{1:X8} ({2} {3}) [{4}]", "__substg1.0_", vpnyv, jpkwk(), oxbgv, (dhbij ? true : false) ? "FIX" : okugo.ToString());
	}

	public string jpkwk()
	{
		switch (hmrmu)
		{
		case mshrw.isojd:
			if (Enum.IsDefined(typeof(MsgPropertyTag), xablt) && 0 == 0)
			{
				return xablt.ToString();
			}
			return brgjd.edcru("UNKNOWN(0x{0:X4})", (ushort)xablt);
		case mshrw.dtkaq:
			if (Enum.IsDefined(typeof(MsgPropertyId), ifnad) && 0 == 0)
			{
				return ifnad.ToString();
			}
			return brgjd.edcru("UNKNOWN(0x{0:X4})", (ushort)ifnad);
		case mshrw.tkmbe:
			if (!string.IsNullOrEmpty(abdox) || 1 == 0)
			{
				return abdox;
			}
			return brgjd.edcru("<EMPTY>");
		default:
			return "<UNKNOWN-KIND>.";
		}
	}

	public string bujmq()
	{
		if (oxbgv == xcrar.yesjh)
		{
			return BitConverter.ToString((byte[])tgbhs);
		}
		if (oxbgv == xcrar.rjogj)
		{
			return brgjd.edcru("0x{0:X8} ({1})", tgbhs, tgbhs);
		}
		StringBuilder stringBuilder;
		int num;
		int num2;
		if (tgbhs is Array array)
		{
			stringBuilder = new StringBuilder();
			if (oxbgv == xcrar.vbevq)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_0085;
				}
				goto IL_00b1;
			}
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_00c1;
			}
			goto IL_00e2;
		}
		if (tgbhs != null && 0 == 0)
		{
			return tgbhs.ToString();
		}
		return string.Empty;
		IL_00eb:
		return stringBuilder.ToString();
		IL_00e2:
		if (num2 < array.Length)
		{
			goto IL_00c1;
		}
		goto IL_00eb;
		IL_00b1:
		if (num < array.Length)
		{
			goto IL_0085;
		}
		goto IL_00eb;
		IL_00c1:
		if (num2 > 0)
		{
			stringBuilder.Append("\r\n");
		}
		stringBuilder.arumx(array.GetValue(num2));
		num2++;
		goto IL_00e2;
		IL_0085:
		if (num > 0)
		{
			stringBuilder.Append("\r\n");
		}
		stringBuilder.Append(BitConverter.ToString((byte[])array.GetValue(num)));
		num++;
		goto IL_00b1;
	}
}
