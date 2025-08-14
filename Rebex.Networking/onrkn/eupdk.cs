using System;
using System.Collections.Generic;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal class eupdk : ofuit
{
	private List<nxtme<byte>> dcxxd;

	private int yflrr;

	public override int nimwj => yflrr + 7;

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		p1 += 4;
		p0[p1] = (byte)((yflrr >> 16) & 0xFF);
		p0[p1 + 1] = (byte)((yflrr >> 8) & 0xFF);
		p0[p1 + 2] = (byte)(yflrr & 0xFF);
		p1 += 3;
		int num = 0;
		if (num != 0)
		{
			goto IL_0054;
		}
		goto IL_00cf;
		IL_0054:
		nxtme<byte> nxtme2 = dcxxd[num];
		p0[p1] = (byte)((nxtme2.tvoem >> 16) & 0xFF);
		p0[p1 + 1] = (byte)((nxtme2.tvoem >> 8) & 0xFF);
		p0[p1 + 2] = (byte)(nxtme2.tvoem & 0xFF);
		p1 += 3;
		nxtme2.rjwrl(new nxtme<byte>(p0, p1));
		p1 += nxtme2.tvoem;
		num++;
		goto IL_00cf;
		IL_00cf:
		if (num >= dcxxd.Count)
		{
			return;
		}
		goto IL_0054;
	}

	public CertificateChain tycix()
	{
		if (dcxxd.Count == 0 || 1 == 0)
		{
			return null;
		}
		CertificateChain certificateChain = new CertificateChain();
		using List<nxtme<byte>>.Enumerator enumerator = dcxxd.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			Certificate certificate = new Certificate(enumerator.Current.trkhv());
			certificateChain.Add(certificate);
		}
		return certificateChain;
	}

	public eupdk(byte[] buffer, int offset, int length)
		: base(nsvut.upgjx)
	{
		int num = offset + 4;
		if (offset + length < num + 3)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "Certificate"));
		}
		int num2 = (yflrr = buffer[num] * 65536 + buffer[num + 1] * 256 + buffer[num + 2]);
		num += 3;
		int num3 = num + num2;
		dcxxd = new List<nxtme<byte>>();
		for (; num < num3; num += 3 + num2)
		{
			if (offset + length < num + 3)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "Certificate"));
			}
			num2 = buffer[num] * 65536 + buffer[num + 1] * 256 + buffer[num + 2];
			if (offset + length < num + 3 + num2)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "Certificate"));
			}
			byte[] array = new byte[num2];
			Array.Copy(buffer, num + 3, array, 0, num2);
			dcxxd.Add(new nxtme<byte>(array));
		}
	}

	public eupdk(CertificateChain chain, TlsParameters parameters)
		: base(nsvut.upgjx)
	{
		dcxxd = new List<nxtme<byte>>();
		if (chain != null && 0 == 0 && chain.LeafCertificate != null && 0 == 0)
		{
			IEnumerator<Certificate> enumerator = chain.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					Certificate current = enumerator.Current;
					dcxxd.Add(current.GetRawCertData());
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
		}
		yflrr = 0;
		int num = 0;
		if (num != 0)
		{
			goto IL_0087;
		}
		goto IL_00ba;
		IL_00ba:
		if (num >= dcxxd.Count)
		{
			return;
		}
		goto IL_0087;
		IL_0087:
		yflrr += 3 + dcxxd[num].tvoem;
		num++;
		goto IL_00ba;
	}
}
