using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SHCWrapper.Crypto;

public class Certificates
{
	public static bool ImportCertificate(string filename, string subsystem_protocol, string password_certificate)
	{
		return PrivateWrapper.Cert_ImportCertificateWithPrivateKey(filename, subsystem_protocol, password_certificate);
	}

	public static bool ImportCertificate(string filename, string subsystem_protocol)
	{
		return PrivateWrapper.Cert_ImportCertificate(filename, subsystem_protocol);
	}

	public static bool ImportPublicKey(string filename)
	{
		return PrivateWrapper.LoadPublicKeyFromCertFile(filename);
	}

	public static RSACryptoServiceProvider GetRSACSPPrivateKeyFromCert(X509Certificate2 cert)
	{
		RSACryptoServiceProvider result = null;
		if (cert == null)
		{
			return null;
		}
		uint uiSize = 0u;
		if (PrivateWrapper.CertGetCertificateContextProperty(cert.Handle, 2u, IntPtr.Zero, ref uiSize) && uiSize != 0)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = Marshal.AllocHGlobal((int)uiSize);
				if (PrivateWrapper.CertGetCertificateContextProperty(cert.Handle, 2u, intPtr, ref uiSize))
				{
					string strContainerNameIn = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(intPtr));
					string strProviderNameIn = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32((IntPtr)((int)intPtr + 4)));
					int dwTypeIn = Marshal.ReadInt32((IntPtr)((int)intPtr + 8));
					CspParameters parameters = new CspParameters(dwTypeIn, strProviderNameIn, strContainerNameIn);
					result = new RSACryptoServiceProvider(parameters);
				}
			}
			catch
			{
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}
		return result;
	}
}
