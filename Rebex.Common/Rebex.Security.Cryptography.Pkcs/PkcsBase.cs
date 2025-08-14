using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public abstract class PkcsBase
{
	private class eenwg : rhegb
	{
		private readonly ICertificateFinder alfer;

		private readonly bool jbqcl;

		private PkcsBase dtglb;

		public PkcsBase xzdcz => dtglb;

		public eenwg(ICertificateFinder finder, bool silent)
			: base(detectOnly: false)
		{
			alfer = finder;
			jbqcl = silent;
		}

		protected override lnabj qqsit()
		{
			string value;
			if ((value = base.nsfih.Value) != null && 0 == 0)
			{
				if (value == "1.2.840.113549.1.7.2")
				{
					SignedData signedData = new SignedData();
					signedData.CertificateFinder = alfer;
					signedData.Silent = jbqcl;
					signedData.ixwfs(this);
					dtglb = signedData;
					return signedData;
				}
				if (value == "1.2.840.113549.1.7.3")
				{
					EnvelopedData envelopedData = new EnvelopedData();
					envelopedData.CertificateFinder = alfer;
					envelopedData.Silent = jbqcl;
					envelopedData.epeku(this);
					dtglb = envelopedData;
					return envelopedData;
				}
			}
			return null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method has been deprecated and will be removed. Use PkcsBase.LoadSignedOrEnvelopedData instead.", true)]
	[wptwl(false)]
	public static PkcsBase Load(Stream input, ICertificateFinder finder, bool silent)
	{
		return LoadSignedOrEnvelopedData(input, finder, silent);
	}

	public static PkcsBase LoadSignedOrEnvelopedData(Stream input, ICertificateFinder finder, bool silent)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		eenwg eenwg = new eenwg(finder, silent);
		Stream stream = new hfnnn(eenwg);
		input.alskc(stream);
		stream.Close();
		input.Close();
		return eenwg.xzdcz;
	}

	internal static Stream ghxwz(Stream p0, Stream p1, string p2)
	{
		int num = p0.ReadByte();
		if (num < 0)
		{
			throw new CryptographicException("Input stream is empty.");
		}
		if (num != 48)
		{
			p1 = new xvufe(p1, ownsInner: true, null);
			p1 = new tewxl(p1, p2);
		}
		p1.WriteByte((byte)num);
		return p1;
	}
}
