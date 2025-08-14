using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class zyked : CertificateExtension
{
	public const string rfjqu = "1.3.6.1.5.5.7.48.1.2";

	public const string daxhd = "1.3.6.1.5.5.7.48.1.5";

	internal zyked()
	{
	}

	public zyked(string oid, bool critical, byte[] data)
		: base(new ObjectIdentifier(oid), critical, data)
	{
	}

	public static zyked qzkku(byte[] p0)
	{
		return new zyked("1.3.6.1.5.5.7.48.1.2", critical: false, new rwolq(p0).ionjf());
	}
}
