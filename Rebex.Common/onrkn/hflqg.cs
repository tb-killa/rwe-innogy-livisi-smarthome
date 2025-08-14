using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rebex.Security.Certificates;

namespace onrkn;

[rbjhl("windows")]
internal abstract class hflqg : jayrg
{
	private sealed class upvqr
	{
		public Certificate bvljg;

		public CspParameters ozoea()
		{
			return xbcyx(bvljg, p1: true);
		}
	}

	private readonly Func<CspParameters> gqmqs;

	protected hflqg(Func<CspParameters> getInfo)
	{
		gqmqs = getInfo;
	}

	public override CspParameters iqqfj()
	{
		CspParameters cspParameters = gqmqs();
		if (cspParameters.KeyContainerName == null || 1 == 0)
		{
			return null;
		}
		CspParameters cspParameters2 = new CspParameters(cspParameters.ProviderType, cspParameters.ProviderName, cspParameters.KeyContainerName);
		cspParameters2.KeyNumber = cspParameters.KeyNumber;
		cspParameters2.Flags = cspParameters.Flags;
		return cspParameters2;
	}

	public static CspParameters xbcyx(Certificate p0, bool p1)
	{
		IntPtr handle = p0.Handle;
		int pcbData = 0;
		if (pothu.CertGetCertificateContextProperty(handle, 2, IntPtr.Zero, out pcbData) == 0 || 1 == 0)
		{
			if (!p1 || 1 == 0)
			{
				return null;
			}
			throw new CryptographicException(brgjd.edcru("Unable to access the private key (0x{0:X8}).", Marshal.GetLastWin32Error()));
		}
		samhn samhn2 = new samhn(pcbData);
		try
		{
			if (pothu.CertGetCertificateContextProperty(handle, 2, samhn2.inyna(), out pcbData) == 0 || 1 == 0)
			{
				if (!p1 || 1 == 0)
				{
					return null;
				}
				throw new CryptographicException(brgjd.edcru("Unable to access the private key (0x{0:X8}).", Marshal.GetLastWin32Error()));
			}
			pothu.rpyum rpyum = (pothu.rpyum)Marshal.PtrToStructure(samhn2.inyna(), typeof(pothu.rpyum));
			CspParameters cspParameters = new CspParameters();
			cspParameters.ProviderName = Marshal.PtrToStringUni(rpyum.uxwcu);
			cspParameters.KeyContainerName = Marshal.PtrToStringUni(rpyum.pvqfg);
			cspParameters.ProviderType = rpyum.hctow;
			cspParameters.KeyNumber = rpyum.lbski;
			cspParameters.Flags = CspProviderFlags.UseExistingKey;
			if ((rpyum.wgppb & 0x20) != 0 && 0 == 0)
			{
				cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
			}
			return cspParameters;
		}
		finally
		{
			if (samhn2 != null && 0 == 0)
			{
				((IDisposable)samhn2).Dispose();
			}
		}
	}

	public static void pkpvn(CspParameters p0, bool p1)
	{
		uint num = 80u;
		if ((p0.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags && 0 == 0)
		{
			num |= 0x20;
		}
		if (pothu.qfori(out var _, p0.KeyContainerName, p0.ProviderName, p0.ProviderType, num) == 0 || 1 == 0)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error != -2146893802 && p1)
			{
				throw new CryptographicException(brgjd.edcru("Unable to delete private key (0x{0:X8}).", lastWin32Error));
			}
		}
	}

	public static hflqg ahfzl(Certificate p0, bool p1, bool p2, out int p3)
	{
		upvqr upvqr = new upvqr();
		upvqr.bvljg = p0;
		p3 = 0;
		uint num = 0u;
		if (p1 && 0 == 0)
		{
			num |= 0x40;
		}
		if (pothu.CryptAcquireCertificatePrivateKey(upvqr.bvljg.Handle, num, IntPtr.Zero, out var phCryptProv, out var pdwKeySpec, out var pfCallerFreeProv) == 0 || 1 == 0)
		{
			p3 = Marshal.GetLastWin32Error();
			if (!p2 || 1 == 0)
			{
				return null;
			}
			switch (p3)
			{
			case -2146893790:
				throw new CryptographicException("CSP needs to display UI to operate.");
			case -2146885621:
				throw new CryptographicException("Certificate does not have a private key.");
			default:
				throw new CryptographicException(brgjd.edcru("Unable to acquire private key handle for this certificate ({0:X8}).", p3));
			}
		}
		Func<CspParameters> getInfo = upvqr.ozoea;
		bool ownsHandle = pfCallerFreeProv != 0;
		return new xgwba(phCryptProv, getInfo, upvqr.bvljg.KeyAlgorithm, pdwKeySpec, ownsHandle);
	}

	public static void jqcpo(Certificate p0, CspParameters p1)
	{
		pothu.rpyum pvData = default(pothu.rpyum);
		samhn samhn2 = samhn.yccdb(p1.ProviderName);
		samhn samhn3 = samhn.yccdb(p1.KeyContainerName);
		pvData.uxwcu = samhn2.inyna();
		pvData.pvqfg = samhn3.inyna();
		pvData.hctow = p1.ProviderType;
		pvData.lbski = p1.KeyNumber;
		pvData.wgppb = (((p1.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags && 0 == 0) ? 32u : 0u);
		if (pothu.CertSetCertificateContextProperty(p0.Handle, 2, 0, ref pvData) == 0 || 1 == 0)
		{
			throw new CryptographicException(brgjd.edcru("Unable to associate private key (0x{0:X8}).", Marshal.GetLastWin32Error()));
		}
	}
}
