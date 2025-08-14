using System.Collections.Generic;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class CertificateRevocationListCollection : CryptographicCollection<CertificateRevocationList>, lnabj
{
	internal CertificateRevocationListCollection()
		: base(rmkkr.wguaf)
	{
	}

	private lnabj attlg(rmkkr p0, bool p1, int p2)
	{
		CertificateRevocationList certificateRevocationList = new CertificateRevocationList();
		base.lquvo.Add(certificateRevocationList);
		return certificateRevocationList;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in attlg
		return this.attlg(p0, p1, p2);
	}

	internal CertificateRevocationListCollection xizgs()
	{
		CertificateRevocationListCollection certificateRevocationListCollection = new CertificateRevocationListCollection();
		IEnumerator<CertificateRevocationList> enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				CertificateRevocationList current = enumerator.Current;
				certificateRevocationListCollection.Add(current);
			}
			return certificateRevocationListCollection;
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	private void nbgng()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_001d;
		IL_0006:
		CertificateRevocationList certificateRevocationList = base[num];
		certificateRevocationList.ukgmy();
		num++;
		goto IL_001d;
		IL_001d:
		if (num >= base.Count)
		{
			return;
		}
		goto IL_0006;
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in nbgng
		this.nbgng();
	}
}
