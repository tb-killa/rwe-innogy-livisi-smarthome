using System;
using System.Collections.Generic;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Certificates;

public class CertificateExtensionCollection : CryptographicCollection<CertificateExtension>, lnabj
{
	public CertificateExtension this[string oid]
	{
		get
		{
			if (oid == null || 1 == 0)
			{
				throw new ArgumentNullException("oid");
			}
			ObjectIdentifier objectIdentifier = new ObjectIdentifier(oid);
			IEnumerator<CertificateExtension> enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					CertificateExtension current = enumerator.Current;
					if (current.Oid == objectIdentifier.Value && 0 == 0)
					{
						return current;
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
			return null;
		}
	}

	internal CertificateExtensionCollection()
		: base(rmkkr.osptv)
	{
	}

	public CertificateExtensionCollection(byte[] data)
		: this()
	{
		hfnnn.qnzgo(this, data);
	}

	private lnabj stxot(rmkkr p0, bool p1, int p2)
	{
		CertificateExtension certificateExtension = new CertificateExtension();
		base.lquvo.Add(certificateExtension);
		return certificateExtension;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in stxot
		return this.stxot(p0, p1, p2);
	}
}
