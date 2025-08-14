using System;

namespace Rebex.Security.Cryptography.Pkcs;

public class RevocationCheckResult
{
	private RevokedCertificate knnxt;

	private RevocationCheckStatus ltyug;

	public RevocationCheckStatus Status
	{
		get
		{
			return ltyug;
		}
		private set
		{
			ltyug = value;
		}
	}

	public DateTime? RevocationDate
	{
		get
		{
			if (knnxt != null && 0 == 0)
			{
				return knnxt.RevocationDate;
			}
			return null;
		}
	}

	public RevocationReason? RevocationReason
	{
		get
		{
			if (knnxt != null && 0 == 0)
			{
				return knnxt.GetRevocationReason();
			}
			return null;
		}
	}

	internal RevocationCheckResult(RevocationCheckStatus status)
	{
		Status = status;
	}

	internal RevocationCheckResult(RevokedCertificate info)
	{
		Status = RevocationCheckStatus.Revoked;
		knnxt = info;
	}
}
