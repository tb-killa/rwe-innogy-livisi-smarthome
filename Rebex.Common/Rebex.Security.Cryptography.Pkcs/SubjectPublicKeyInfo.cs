using System;
using System.ComponentModel;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

[EditorBrowsable(EditorBrowsableState.Never)]
[wptwl(false)]
public class SubjectPublicKeyInfo : PublicKeyInfo
{
	[Obsolete("This class has been deprecated and will be removed. Use PublicKeyInfo class instead.", true)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public SubjectPublicKeyInfo()
	{
	}

	internal SubjectPublicKeyInfo(bool dummy)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This class has been deprecated and will be removed. Use PublicKeyInfo class instead.", true)]
	public SubjectPublicKeyInfo(RSAParameters parameters)
		: base(parameters)
	{
	}

	[wptwl(false)]
	[Obsolete("This class has been deprecated and will be removed. Use PublicKeyInfo class instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public SubjectPublicKeyInfo(DSAParameters parameters)
		: base(parameters)
	{
	}
}
