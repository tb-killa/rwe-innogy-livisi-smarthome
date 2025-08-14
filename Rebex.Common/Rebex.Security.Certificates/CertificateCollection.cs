using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Certificates;

public class CertificateCollection : CryptographicCollection<Certificate>, lnabj
{
	public CertificateCollection()
		: base(rmkkr.wguaf)
	{
	}

	internal CertificateCollection(bool isSet)
		: base((isSet ? true : false) ? rmkkr.wguaf : rmkkr.osptv)
	{
	}

	private lnabj mdexq(rmkkr p0, bool p1, int p2)
	{
		Certificate certificate = new Certificate();
		base.lquvo.Add(certificate);
		return certificate;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in mdexq
		return this.mdexq(p0, p1, p2);
	}

	internal new bool Contains(Certificate certificate)
	{
		if (certificate == null || 1 == 0)
		{
			return false;
		}
		byte[] rawCertData = certificate.GetRawCertData();
		int num = 0;
		if (num != 0)
		{
			goto IL_0019;
		}
		goto IL_003b;
		IL_0019:
		if (zjcch.wduyr(rawCertData, base[num].GetRawCertData()) && 0 == 0)
		{
			return true;
		}
		num++;
		goto IL_003b;
		IL_003b:
		if (num < base.Count)
		{
			goto IL_0019;
		}
		return false;
	}
}
