using System;
using System.Globalization;
using System.IO;
using System.Net;
using Rebex;

namespace onrkn;

internal class hspjp : xaxit
{
	private const int cewjh = 32768;

	private const yosfy mfpau = (yosfy)99;

	private readonly nymgm kuqwp;

	private readonly bool qgfpz;

	private readonly long tgbbu;

	private readonly eiuaf icfzn;

	private readonly byte[] kgobc;

	private long biqtg;

	private long qblie;

	private long glwqs;

	private bool tvwro;

	private bool zdcnn;

	private bool bzlch;

	private bool rbpyd;

	private bool cvzwt;

	private readonly udlmn nwpzj;

	private readonly thths wkclm;

	private readonly DecompressionMethods dvuaf;

	public DecompressionMethods rkwcn => dvuaf;

	public override bool CanRead
	{
		get
		{
			if (!rbpyd || 1 == 0)
			{
				return cvzwt;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override bool CanWrite => false;

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
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	internal hspjp(nymgm reader, bool chunked, string compression, long? length, bool stayOpen, udlmn client, thths owner)
	{
		Action action = null;
		Action action2 = null;
		base._002Ector();
		kuqwp = reader;
		qgfpz = chunked;
		tgbbu = length ?? (-1);
		glwqs = -1L;
		zdcnn = !stayOpen;
		nwpzj = client;
		wkclm = owner;
		cvzwt = true;
		icfzn = null;
		dvuaf = qwaxv(compression, client.xpfip);
		switch (dvuaf)
		{
		case DecompressionMethods.GZip:
			icfzn = new ayoqm();
			break;
		case DecompressionMethods.Deflate:
		{
			if (action == null || 1 == 0)
			{
				action = ksuen;
			}
			Action onDeflateFallback = action;
			if (action2 == null || 1 == 0)
			{
				action2 = pfkql;
			}
			icfzn = new ogeee(onDeflateFallback, action2);
			break;
		}
		}
		if (icfzn != null && 0 == 0)
		{
			kgobc = new byte[32768];
		}
	}

	private static DecompressionMethods qwaxv(string p0, DecompressionMethods p1)
	{
		brgjd.nyxjq<DecompressionMethods>(p0, p1: true, out var p2);
		if (!p1.qtclt(p2) || 1 == 0)
		{
			return DecompressionMethods.None;
		}
		return p2;
	}

	internal void ncycz()
	{
		byte[] array = new byte[2048];
		while (Read(array, 0, array.Length) > 0)
		{
		}
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		throw new NotSupportedException();
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		throw new NotSupportedException();
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (rbpyd && 0 == 0)
		{
			throw new ObjectDisposedException(null, "Cannot access a closed stream.");
		}
		hybaa();
		if (!cvzwt || 1 == 0)
		{
			throw tfjlb("Response stream is no longer usable.", ezmya.yhvcm);
		}
		dahxy.dionp(buffer, offset, count);
		if (count == 0 || false || bzlch)
		{
			return rxpjc.caxut(0);
		}
		return base.BeginRead(buffer, offset, count, callback, state);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (rbpyd && 0 == 0)
		{
			throw new ObjectDisposedException(null, "Cannot access a closed stream.");
		}
		hybaa();
		if (!cvzwt || 1 == 0)
		{
			throw tfjlb("Response stream is no longer usable.", ezmya.yhvcm);
		}
		dahxy.dionp(buffer, offset, count);
		if (count == 0 || false || bzlch)
		{
			return 0;
		}
		bool flag = false;
		try
		{
			if (icfzn == null || 1 == 0)
			{
				int num = gzxly(buffer, offset, count);
				biqtg += num;
				bool flag2 = tgbbu == biqtg && !qgfpz;
				if (num == 0 || false || flag2)
				{
					if (!tvwro || 1 == 0)
					{
						nwpzj.iptfx(LogLevel.Debug, "HTTP", "Received content ({0} bytes).", biqtg);
						tvwro = true;
					}
					bool flag3 = gnqrc(p0: false);
					if (flag3 && 0 == 0 && (num == 0 || 1 == 0))
					{
						throw tfjlb("Unexpected data appeared after end of response.", ezmya.yhvcm);
					}
					if (!flag3 || 1 == 0)
					{
						pmsrt(p0: true);
					}
				}
				flag = true;
				return num;
			}
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0167;
			}
			goto IL_0301;
			IL_0167:
			switch (icfzn.lotbz)
			{
			case yosfy.drxjq:
			{
				int num3 = gzxly(kgobc, 0, 32768);
				if (num3 == 0 || 1 == 0)
				{
					if (icfzn.wngjx && 0 == 0 && (!gnqrc(p0: false) || 1 == 0))
					{
						nwpzj.iptfx(LogLevel.Info, "HTTP", "Compressed data not finished with final block, but no more data received. Supposing this to be complete response.");
						goto case (yosfy)99;
					}
					throw tfjlb("More compressed data expected.", ezmya.yhvcm);
				}
				icfzn.eanoq(kgobc, 0, num3, dzmpf.iksen);
				break;
			}
			case yosfy.muyhp:
			{
				int num3 = icfzn.zohfz(buffer, offset, count);
				biqtg += num3;
				num2 += num3;
				offset += num3;
				count -= num3;
				break;
			}
			case yosfy.aljno:
				if (icfzn.pmhat() != 0 && 0 == 0)
				{
					throw tfjlb("Unexpected data appeared after end of compressed data.", ezmya.yhvcm);
				}
				if (gnqrc(p0: false) && 0 == 0)
				{
					throw tfjlb("Unexpected data appeared after end of response.", ezmya.yhvcm);
				}
				goto case (yosfy)99;
			case (yosfy)99:
				nwpzj.iptfx(LogLevel.Debug, "HTTP", "Received content ({0} raw bytes, {1} decompressed bytes).", qblie, biqtg);
				pmsrt(p0: true);
				flag = true;
				return num2;
			default:
				throw tfjlb("Unexpected decompressor state.", ezmya.yhvcm);
			}
			goto IL_0301;
			IL_0301:
			if (count <= 0)
			{
				flag = true;
				return num2;
			}
			goto IL_0167;
		}
		catch (Exception p)
		{
			nwpzj.rvhyr("Error while reading response", p);
			hybaa();
			throw;
		}
		finally
		{
			if (!flag || 1 == 0)
			{
				cvzwt = false;
			}
		}
	}

	private int gzxly(byte[] p0, int p1, int p2)
	{
		int num;
		if (qgfpz)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_001e;
			}
			goto IL_0182;
		}
		if (tgbbu >= 0)
		{
			long val = tgbbu - qblie;
			p2 = (int)Math.Min(p2, val);
		}
		if (p2 == 0 || 1 == 0)
		{
			return 0;
		}
		int num2 = huxbg(p0, p1, p2);
		if (num2 == 0 || 1 == 0)
		{
			if (!zdcnn || 1 == 0)
			{
				throw tfjlb("Server closed the connection unexpectedly.", ezmya.yhvcm);
			}
		}
		else
		{
			qblie += num2;
		}
		return num2;
		IL_001e:
		if (glwqs < 0)
		{
			if (glwqs != -1)
			{
				if (glwqs == -3)
				{
					glwqs = -2L;
					throw tfjlb("Connection was closed before the last chunk was received.", ezmya.yhvcm);
				}
				return 0;
			}
			string text = eidvr();
			if (text == null)
			{
				glwqs = -3L;
				if (num > 0)
				{
					return num;
				}
				goto IL_0182;
			}
			glwqs = long.Parse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			if (glwqs == 0)
			{
				string text2 = eidvr();
				if (text2 == null || false || text2.Length != 0)
				{
					throw tfjlb("Invalid chunk.", ezmya.yhvcm);
				}
				glwqs = -2L;
				return num;
			}
		}
		int p3 = (int)Math.Min(p2, glwqs);
		int num3 = huxbg(p0, p1, p3);
		if (num3 == 0 || 1 == 0)
		{
			throw tfjlb("Not enough data.", ezmya.yhvcm);
		}
		qblie += num3;
		num += num3;
		p1 += num3;
		p2 -= num3;
		glwqs -= num3;
		if (glwqs == 0)
		{
			string text3 = eidvr();
			if (text3 == null || false || text3.Length != 0)
			{
				throw tfjlb("Invalid chunk.", ezmya.yhvcm);
			}
			glwqs = -1L;
		}
		goto IL_0182;
		IL_0182:
		if (p2 <= 0)
		{
			return num;
		}
		goto IL_001e;
	}

	private void hybaa()
	{
		try
		{
			nwpzj.maskh();
		}
		catch (ujepc p)
		{
			throw wkclm.phrhc(p);
		}
	}

	private string eidvr()
	{
		try
		{
			nwpzj.maskh();
			return kuqwp.ReadLine();
		}
		catch (ujepc p)
		{
			throw wkclm.phrhc(p);
		}
	}

	private bool gnqrc(bool p0)
	{
		try
		{
			nwpzj.maskh();
			return kuqwp.ooaym(p0);
		}
		catch (ujepc p1)
		{
			throw wkclm.phrhc(p1);
		}
	}

	private int huxbg(byte[] p0, int p1, int p2)
	{
		try
		{
			nwpzj.maskh();
			return kuqwp.Receive(p0, p1, p2);
		}
		catch (ujepc p3)
		{
			throw wkclm.phrhc(p3);
		}
	}

	private Exception tfjlb(string p0, ezmya p1)
	{
		ujepc p2 = new ujepc(p0, p1);
		return wkclm.phrhc(p2);
	}

	protected override void julnt()
	{
		if (!rbpyd)
		{
			rbpyd = true;
			pmsrt(p0: false);
		}
	}

	private void pmsrt(bool p0)
	{
		if (!bzlch)
		{
			nwpzj.iptfx(LogLevel.Debug, "HTTP", "Closing response stream.");
			bzlch = true;
			if ((zdcnn ? true : false) || !p0 || 1 == 0)
			{
				kuqwp.fvtwr();
			}
			wkclm.cqvrm(kuqwp);
		}
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	private void ksuen()
	{
		nwpzj.iptfx(LogLevel.Info, "HTTP", "ZLIB header check failed. Using DEFLATE fallback.");
	}

	private void pfkql()
	{
		nwpzj.iptfx(LogLevel.Info, "HTTP", "ZLIB uses invalid compression method. Using DEFLATE fallback.");
	}
}
