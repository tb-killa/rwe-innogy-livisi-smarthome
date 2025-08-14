using System;
using System.IO;
using System.Security.Cryptography;
using Rebex;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal static class pltec
{
	public static CertificateRevocationList qujyk(mnzit p0, string p1, bool p2, awngk p3)
	{
		bool p4;
		CertificateRevocationList result = iftyy(p0, p1, p2, p3, out p4);
		if (p4 && 0 == 0)
		{
			p0.raovz(p1, p3);
		}
		return result;
	}

	private static CertificateRevocationList iftyy(mnzit p0, string p1, bool p2, awngk p3, out bool p4)
	{
		CertificateRevocationList certificateRevocationList = null;
		p4 = false;
		p3.byfnx(LogLevel.Debug, "CRL", "Trying to load CRL from cache '{0}'.", p1);
		Stream stream = p0.xwedh(p1, p3);
		if (stream != null && 0 == 0)
		{
			try
			{
				certificateRevocationList = itqrz(stream, p3);
			}
			finally
			{
				stream.Dispose();
			}
			if (certificateRevocationList != null && 0 == 0)
			{
				if (dahxy.qsrcs(certificateRevocationList.NextUpdate) > DateTime.UtcNow && 0 == 0)
				{
					p3.byfnx(LogLevel.Debug, "CRL", "CRL loaded from cache '{0}'.", p1);
					return certificateRevocationList;
				}
				p3.byfnx(LogLevel.Debug, "CRL", "CRL found in cache has expired '{0}'.", p1);
				p4 = true;
			}
			else
			{
				p3.byfnx(LogLevel.Error, "CRL", "CRL found in cache is unparsable '{0}'.", p1);
			}
		}
		if (p2 && 0 == 0)
		{
			p3.byfnx(LogLevel.Debug, "CRL", "CacheOnly = true, returning null for '{0}'.", p1);
			return null;
		}
		p3.byfnx(LogLevel.Debug, "CRL", "Downloading CRL from '{0}'.", p1);
		certificateRevocationList = null;
		stream = agbih.tqzhi(p1, p3, "CRL");
		if (stream != null && 0 == 0)
		{
			p3.byfnx(LogLevel.Debug, "CRL", "Parsing CRL '{0}'.", p1);
			certificateRevocationList = itqrz(stream, p3);
			if (certificateRevocationList != null && 0 == 0)
			{
				if (dahxy.qsrcs(certificateRevocationList.NextUpdate) < DateTime.UtcNow && 0 == 0)
				{
					p3.byfnx(LogLevel.Debug, "CRL", "Downloaded CRL is expired '{0}'.", p1);
				}
				else
				{
					p3.byfnx(LogLevel.Debug, "CRL", "Saving CRL to cache '{0}'.", p1);
					stream.Position = 0L;
					p0.vrgug(p1, stream, p3);
					p4 = false;
				}
			}
			else
			{
				p3.byfnx(LogLevel.Error, "CRL", "Unable to parse CRL downloaded from '{0}'.", p1);
			}
		}
		else
		{
			p3.byfnx(LogLevel.Error, "CRL", "Unable to download CRL from '{0}'.", p1);
		}
		return certificateRevocationList;
	}

	public static CertificateRevocationList itqrz(Stream p0, awngk p1)
	{
		try
		{
			return CertificateRevocationList.Load(new npohs(p0, leaveOpen: true));
		}
		catch (CryptographicException ex)
		{
			p1.byfnx(LogLevel.Error, "CRL", "Unable to parse CRL: {0}", ex);
			return null;
		}
	}
}
