using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class aoind : wvwdg
{
	private sealed class mdkja
	{
		public TlsParameters mreex;

		public TlsProtocol rstbb;

		public urofm xfhfj(urofm p0)
		{
			if (!mreex.cfmws(p0, rstbb) || 1 == 0)
			{
				return (urofm)0;
			}
			return p0;
		}
	}

	private sealed class mmqfc
	{
		public urofm ltfnf;

		public bool qskcx(urofm p0)
		{
			return p0 == ltfnf;
		}
	}

	private sealed class iursk
	{
		public TlsCipherSuiteId zzljv;

		public bool dqbrr(TlsCipherSuiteId p0)
		{
			return p0 == zzljv;
		}
	}

	private sealed class iltde
	{
		public List<ushort> resng;

		public bool rgfhq(TlsCipherSuiteId p0)
		{
			resng.Add((ushort)p0);
			return false;
		}
	}

	private int ytvlt;

	private byte[] mumbt;

	private byte[] xjemi;

	private byte[] kjmit;

	private byte[] hntuv;

	private byte[] fbkeo;

	private bool? ejkzt = null;

	public override int nimwj => 42 + xjemi.Length + kjmit.Length + hntuv.Length + ((fbkeo != null || uqsbq != null) ? 2 : 0) + ((fbkeo != null && 0 == 0) ? (fbkeo.Length + 9) : 0) + ((uqsbq != null && 0 == 0) ? uqsbq.Length : 0);

	public int szwbv => ytvlt;

	public byte[] ioriz => mumbt;

	public byte[] kchbb => xjemi;

	public TlsCipher mtlwh(kzdrw p0, TlsVersion p1)
	{
		TlsProtocol hdraz = (TlsProtocol)p0.hdraz;
		if (hdraz != (TlsProtocol)szwbv)
		{
			if (p0.hdraz > szwbv)
			{
				throw new TlsException(rtzwv.iogyt, mjddr.puqjh, "Server is trying to use a higher version of the TLS/SSL protocol that requested by the client.", null);
			}
			bool flag = false;
			switch (hdraz)
			{
			case TlsProtocol.SSL30:
				if ((p1 & TlsVersion.SSL30) == 0)
				{
					break;
				}
				flag = true;
				if (flag)
				{
					break;
				}
				goto case TlsProtocol.TLS10;
			case TlsProtocol.TLS10:
				if ((p1 & TlsVersion.TLS10) == 0)
				{
					break;
				}
				flag = true;
				if (flag)
				{
					break;
				}
				goto case TlsProtocol.TLS11;
			case TlsProtocol.TLS11:
				if ((p1 & TlsVersion.TLS11) == 0)
				{
					break;
				}
				flag = true;
				if (flag)
				{
					break;
				}
				goto case TlsProtocol.TLS12;
			case TlsProtocol.TLS12:
				if ((p1 & TlsVersion.TLS12) == 0)
				{
					break;
				}
				flag = true;
				if (flag)
				{
					break;
				}
				goto default;
			default:
				throw new TlsException(rtzwv.iogyt, mjddr.puqjh, "Server is trying to use a version of TLS/SSL protocol that is not supported by the client.", null);
			}
			if (!flag || 1 == 0)
			{
				throw new TlsException(rtzwv.iogyt, mjddr.puqjh, "Server is trying to use a version of TLS/SSL protocol that is not enabled at the client.", null);
			}
		}
		TlsCipherSuiteId csymh = (TlsCipherSuiteId)p0.csymh;
		if (csymh == TlsCipherSuiteId.EMPTY_RENEGOTIATION_INFO_SCSV || csymh == TlsCipherSuiteId.NULL || 1 == 0)
		{
			throw new TlsException(mjddr.jhrgr, "Unsupported cipher suite.");
		}
		if (!mahuq(csymh) || 1 == 0)
		{
			throw new TlsException(mjddr.jhrgr, "Unexpected cipher suite.");
		}
		TlsCipherSuite tlsCipherSuite = TlsCipher.jnahn(csymh);
		if (tlsCipherSuite == TlsCipherSuite.None)
		{
			throw new TlsException(mjddr.jhrgr, "Unsupported cipher suite.");
		}
		TlsCipher tlsCipher = TlsCipher.lfexb(csymh, (int)hdraz);
		if (tlsCipher == null || 1 == 0)
		{
			throw new TlsException(mjddr.jhrgr, "Unsupported cipher suite.");
		}
		if (hdraz < TlsProtocol.TLS12 && (tlsCipherSuite & (TlsCipherSuite.Secure | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384)) != 0)
		{
			throw new TlsException(mjddr.jhrgr, "Unexpected cipher suite.");
		}
		if (hdraz >= TlsProtocol.TLS11 && ((tlsCipherSuite & (TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.RSA_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.DHE_DSS_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_RSA_WITH_DES_CBC_SHA | TlsCipherSuite.DH_anon_WITH_RC4_128_MD5 | TlsCipherSuite.DH_anon_WITH_DES_CBC_SHA)) != 0 || tlsCipher.Exportable))
		{
			throw new TlsException(mjddr.jhrgr, "Unexpected cipher suite.");
		}
		return tlsCipher;
	}

	public urofm ndqws(TlsParameters p0, TlsProtocol p1)
	{
		mdkja mdkja = new mdkja();
		mdkja.mreex = p0;
		mdkja.rstbb = p1;
		return aqszd(mdkja.xfhfj, (urofm)0);
	}

	public bool yiwjt(urofm p0)
	{
		mmqfc mmqfc = new mmqfc();
		mmqfc.ltfnf = p0;
		return aqszd(mmqfc.qskcx, p1: false);
	}

	private T aqszd<T>(Func<urofm, T> p0, T p1)
	{
		byte[] array = kqmgh(eppge.qlxcs).trkhv();
		if (array.Length < 2)
		{
			return p1;
		}
		int num = 2 + (array[0] * 256 + array[1]);
		if ((((num & 1) != 0) ? true : false) || num > array.Length)
		{
			return p1;
		}
		int num2 = 2;
		if (num2 == 0)
		{
			goto IL_0048;
		}
		goto IL_0086;
		IL_0086:
		if (num2 < num)
		{
			goto IL_0048;
		}
		return p1;
		IL_0048:
		urofm arg = (urofm)(array[num2] * 256 + array[num2 + 1]);
		T val = p0(arg);
		if (!p1.Equals(val) || 1 == 0)
		{
			return val;
		}
		num2 += 2;
		goto IL_0086;
	}

	public bool mahuq(TlsCipherSuiteId p0)
	{
		iursk iursk = new iursk();
		iursk.zzljv = p0;
		return mqpfy(iursk.dqbrr);
	}

	public nxtme<ushort> etiot()
	{
		iltde iltde = new iltde();
		iltde.resng = new List<ushort>();
		mqpfy(iltde.rgfhq);
		return iltde.resng.ToArray().liutv();
	}

	private bool mqpfy(Func<TlsCipherSuiteId, bool> p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0033;
		IL_0006:
		TlsCipherSuiteId arg = (TlsCipherSuiteId)((kjmit[num] << 8) + kjmit[num + 1]);
		if (p0(arg) && 0 == 0)
		{
			return true;
		}
		num += 2;
		goto IL_0033;
		IL_0033:
		if (num < kjmit.Length)
		{
			goto IL_0006;
		}
		return false;
	}

	public byte[] nsqrx()
	{
		nxtme<byte> nxtme2 = kqmgh(eppge.pnxlm);
		if (!nxtme2.hvbtp || 1 == 0)
		{
			return nxtme2.trkhv();
		}
		if (!ejkzt.HasValue || 1 == 0)
		{
			ejkzt = mahuq(TlsCipherSuiteId.EMPTY_RENEGOTIATION_INFO_SCSV);
		}
		if (ejkzt.Value && 0 == 0)
		{
			return new byte[1];
		}
		return null;
	}

	public bool jhgcv()
	{
		nxtme<byte> nxtme2 = kqmgh(eppge.gynad, p1: true);
		if (nxtme2.tvoem == 2 && (nxtme2[0] == 0 || 1 == 0))
		{
			return nxtme2[1] == 0;
		}
		return false;
	}

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		p0[p1 + 4] = (byte)((ytvlt >> 8) & 0xFF);
		p0[p1 + 5] = (byte)(ytvlt & 0xFF);
		p1 += 6;
		mumbt.CopyTo(p0, p1);
		p1 += mumbt.Length;
		p0[p1] = (byte)xjemi.Length;
		xjemi.CopyTo(p0, p1 + 1);
		p1 += 1 + xjemi.Length;
		p0[p1] = (byte)(kjmit.Length >> 8);
		p0[p1 + 1] = (byte)(kjmit.Length & 0xFF);
		kjmit.CopyTo(p0, p1 + 2);
		p1 += 2 + kjmit.Length;
		p0[p1] = (byte)hntuv.Length;
		hntuv.CopyTo(p0, p1 + 1);
		p1 += 1 + hntuv.Length;
		if (fbkeo != null || uqsbq != null)
		{
			int num = ((fbkeo != null && 0 == 0) ? (fbkeo.Length + 9) : 0) + ((uqsbq != null && 0 == 0) ? uqsbq.Length : 0);
			p0[p1] = (byte)(num >> 8);
			p0[p1 + 1] = (byte)(num & 0xFF);
			p1 += 2;
		}
		if (fbkeo != null && 0 == 0)
		{
			int num2 = fbkeo.Length;
			p0[p1] = 0;
			p0[p1 + 1] = 0;
			p0[p1 + 2] = (byte)(num2 + 5 >> 8);
			p0[p1 + 3] = (byte)((num2 + 5) & 0xFF);
			p0[p1 + 4] = (byte)(num2 + 3 >> 8);
			p0[p1 + 5] = (byte)((num2 + 3) & 0xFF);
			p0[p1 + 6] = 0;
			p0[p1 + 7] = (byte)(num2 >> 8);
			p0[p1 + 8] = (byte)(num2 & 0xFF);
			fbkeo.CopyTo(p0, p1 + 9);
			p1 += 9 + fbkeo.Length;
		}
		if (uqsbq != null && 0 == 0)
		{
			uqsbq.CopyTo(p0, p1);
		}
	}

	private void zdiym(byte[] p0, int p1, int p2)
	{
		ytvlt = (p0[p1 + 1] << 8) + p0[p1 + 2];
		int num = (p0[p1 + 3] << 8) + p0[p1 + 4];
		int num2 = (p0[p1 + 5] << 8) + p0[p1 + 6];
		int num3 = (p0[p1 + 7] << 8) + p0[p1 + 8];
		ArrayList arrayList = new ArrayList();
		int num4 = p1 + 9;
		int num5 = 0;
		if (num5 != 0)
		{
			goto IL_0050;
		}
		goto IL_008e;
		IL_0050:
		if (p0[num4] == 0 || 1 == 0)
		{
			arrayList.Add(p0[num4 + 1]);
			arrayList.Add(p0[num4 + 2]);
		}
		num4 += 3;
		num5 += 3;
		goto IL_008e;
		IL_008e:
		if (num5 >= num)
		{
			kjmit = (byte[])arrayList.ToArray(typeof(byte));
			xjemi = new byte[num2];
			Array.Copy(p0, num4, xjemi, 0, num2);
			num4 += num2;
			mumbt = new byte[32];
			if (num3 > 0)
			{
				if (num3 <= 32)
				{
					Array.Copy(p0, num4, mumbt, 32 - num3, num3);
				}
				else
				{
					Array.Copy(p0, num4 + num3 - 32, mumbt, 0, 32);
				}
			}
			num4 += num3;
			hntuv = new byte[0];
			return;
		}
		goto IL_0050;
	}

	public aoind(byte[] buffer, int offset, int length)
		: base(nsvut.lnjcv)
	{
		if (buffer[offset + 1] >= 2)
		{
			zdiym(buffer, offset, length);
			return;
		}
		if (length < 37)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ClientHello"));
		}
		if (buffer[38 + offset] < 0 || buffer[38 + offset] > 32 || length - buffer[38 + offset] - 42 < 0)
		{
			throw new TlsException(mjddr.gkkle, "Message with invalid SessionID.");
		}
		int num = offset + length;
		ytvlt = (buffer[offset + 4] << 8) + buffer[offset + 5];
		mumbt = new byte[32];
		Array.Copy(buffer, offset + 6, mumbt, 0, 32);
		offset += 38;
		int num2 = buffer[offset];
		xjemi = new byte[num2];
		offset++;
		if (num2 > 0)
		{
			Array.Copy(buffer, offset, xjemi, 0, num2);
			offset += num2;
		}
		num2 = (buffer[offset] << 8) + buffer[offset + 1];
		kjmit = new byte[num2];
		offset += 2;
		if (num2 > 0)
		{
			Array.Copy(buffer, offset, kjmit, 0, num2);
			offset += num2;
		}
		num2 = buffer[offset];
		hntuv = new byte[num2];
		offset++;
		if (num2 > 0)
		{
			Array.Copy(buffer, offset, hntuv, 0, num2);
			offset += num2;
		}
		if (num >= offset + 2)
		{
			num2 = (buffer[offset] << 8) + buffer[offset + 1];
			offset += 2;
			if (num < offset + num2)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ClientHello"));
			}
			uqsbq = new byte[num2];
			Array.Copy(buffer, offset, uqsbq, 0, num2);
			offset += num2;
		}
	}

	public aoind(TlsProtocol protocolVersion, TlsCipherSuite allowedSuites, bool announceRenegotiationIndication, TlsParameters parameters, byte[] sessionID, string serverName, byte[] extensions)
		: base(nsvut.lnjcv)
	{
		ytvlt = (int)protocolVersion;
		long num = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
		mumbt = new byte[32];
		mumbt[0] = (byte)((num >> 24) & 0xFF);
		mumbt[1] = (byte)((num >> 16) & 0xFF);
		mumbt[2] = (byte)((num >> 8) & 0xFF);
		mumbt[3] = (byte)(num & 0xFF);
		jtxhe.ubsib(mumbt, 4, 28);
		if (sessionID == null || 1 == 0)
		{
			xjemi = new byte[0];
		}
		else
		{
			xjemi = sessionID;
		}
		ICollection<TlsCipherSuite> preferredSuites = parameters.GetPreferredSuites();
		ICollection<TlsCipherSuite> collection = TlsCipher.xnjzt(allowedSuites, preferredSuites);
		MemoryStream memoryStream = new MemoryStream();
		IEnumerator<TlsCipherSuite> enumerator = collection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				TlsCipherSuite current = enumerator.Current;
				int num2 = (int)TlsCipher.plzju(current);
				memoryStream.WriteByte((byte)(num2 >> 8));
				memoryStream.WriteByte((byte)(num2 & 0xFF));
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (announceRenegotiationIndication && 0 == 0 && protocolVersion >= TlsProtocol.TLS10)
		{
			int num3 = 255;
			memoryStream.WriteByte((byte)(num3 >> 8));
			memoryStream.WriteByte((byte)(num3 & 0xFF));
			ejkzt = true;
		}
		kjmit = memoryStream.ToArray();
		byte[] array = new byte[1];
		hntuv = array;
		if (protocolVersion >= TlsProtocol.TLS10)
		{
			if (!string.IsNullOrEmpty(serverName) || 1 == 0)
			{
				fbkeo = EncodingTools.ASCII.GetBytes(serverName);
			}
			if (extensions != null && 0 == 0 && extensions.Length > 0)
			{
				uqsbq = extensions;
			}
		}
	}

	public nxtme<string> nihvb()
	{
		return nxtme<string>.gihlo;
	}

	public string vfiwv()
	{
		return null;
	}
}
