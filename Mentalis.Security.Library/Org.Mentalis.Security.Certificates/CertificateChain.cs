using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

namespace Org.Mentalis.Security.Certificates;

public class CertificateChain
{
	private Certificate m_Certificate;

	private IntPtr m_Handle;

	protected Certificate Certificate => m_Certificate;

	public CertificateChain(Certificate cert)
		: this(cert, null)
	{
	}

	public CertificateChain(Certificate cert, CertificateStore additional)
		: this(cert, additional, CertificateChainOptions.Default)
	{
	}

	public CertificateChain(Certificate cert, CertificateStore additional, CertificateChainOptions options)
	{
		if (cert == null)
		{
			throw new ArgumentNullException();
		}
		IntPtr hAdditionalStore = additional?.Handle ?? IntPtr.Zero;
		ChainParameters pChainPara = new ChainParameters
		{
			cbSize = Marshal.SizeOf(typeof(ChainParameters)),
			RequestedUsagecUsageIdentifier = 0,
			RequestedUsagedwType = 0,
			RequestedUsagergpszUsageIdentifier = IntPtr.Zero
		};
		if (SspiProvider.CertGetCertificateChain(IntPtr.Zero, cert.Handle, IntPtr.Zero, hAdditionalStore, ref pChainPara, (int)options, IntPtr.Zero, ref m_Handle) == 0)
		{
			throw new CertificateException("Unable to find the certificate chain.");
		}
		m_Certificate = cert;
	}

	~CertificateChain()
	{
		if (m_Handle != IntPtr.Zero)
		{
			SspiProvider.CertFreeCertificateChain(m_Handle);
			m_Handle = IntPtr.Zero;
		}
	}

	public virtual Certificate[] GetCertificates()
	{
		ArrayList arrayList = new ArrayList();
		CertificateStoreCollection certificateStoreCollection;
		if (Certificate.Store is CertificateStoreCollection collection)
		{
			certificateStoreCollection = new CertificateStoreCollection(collection);
		}
		else
		{
			certificateStoreCollection = new CertificateStoreCollection(new CertificateStore[0]);
			if (Certificate.Store == null)
			{
				certificateStoreCollection.AddStore(new CertificateStore(Certificate.m_Context.hCertStore, duplicate: true));
			}
			else
			{
				certificateStoreCollection.AddStore(Certificate.Store);
			}
		}
		certificateStoreCollection.AddStore(CertificateStore.GetCachedStore("Root"));
		certificateStoreCollection.AddStore(CertificateStore.GetCachedStore("CA"));
		IntPtr handle = certificateStoreCollection.Handle;
		IntPtr intPtr = Certificate.DuplicateHandle();
		while (intPtr != IntPtr.Zero)
		{
			arrayList.Add(new Certificate(intPtr, duplicate: false));
			int pdwFlags = 0;
			intPtr = SspiProvider.CertGetIssuerCertificateFromStore(handle, intPtr, IntPtr.Zero, ref pdwFlags);
		}
		certificateStoreCollection.Dispose();
		return (Certificate[])arrayList.ToArray(typeof(Certificate));
	}

	public virtual CertificateStatus VerifyChain(string server, AuthType type)
	{
		return VerifyChain(server, type, VerificationFlags.None);
	}

	public virtual CertificateStatus VerifyChain(string server, AuthType type, VerificationFlags flags)
	{
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		try
		{
			intPtr = ((server != null) ? CeMarshal.StringToHGlobalUni(server) : IntPtr.Zero);
			SslPolicyParameters sslPolicyParameters = new SslPolicyParameters
			{
				cbSize = Marshal.SizeOf(typeof(SslPolicyParameters)),
				dwAuthType = (int)type,
				pwszServerName = intPtr,
				fdwChecks = (int)flags
			};
			intPtr2 = Marshal.AllocHGlobal(sslPolicyParameters.cbSize);
			Marshal.StructureToPtr((object)sslPolicyParameters, intPtr2, fDeleteOld: false);
			ChainPolicyParameters chainPolicyParameters = new ChainPolicyParameters
			{
				cbSize = Marshal.SizeOf(typeof(ChainPolicyParameters)),
				dwFlags = (int)flags,
				pvExtraPolicyPara = intPtr2
			};
			ChainPolicyStatus chainPolicyStatus = new ChainPolicyStatus
			{
				cbSize = Marshal.SizeOf(typeof(ChainPolicyStatus))
			};
			return CertificateStatus.ValidCertificate;
		}
		finally
		{
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public virtual CertificateStatus VerifyChain(string server, AuthType type, VerificationFlags flags, byte[] crl)
	{
		CertificateStatus certificateStatus = VerifyChain(server, type, flags);
		if (certificateStatus != CertificateStatus.ValidCertificate || crl == null)
		{
			return certificateStatus;
		}
		try
		{
			if (!m_Certificate.VerifyRevocation(crl))
			{
				return CertificateStatus.Revoked;
			}
			return certificateStatus;
		}
		catch
		{
			return CertificateStatus.RevocationFailure;
		}
	}

	public virtual IAsyncResult BeginVerifyChain(string server, AuthType type, VerificationFlags flags, AsyncCallback callback, object asyncState)
	{
		CertificateVerificationResult certificateVerificationResult = new CertificateVerificationResult(this, server, type, flags, callback, asyncState);
		if (!ThreadPool.QueueUserWorkItem(StartVerification, certificateVerificationResult))
		{
			throw new CertificateException("Could not schedule the certificate chain for verification.");
		}
		return certificateVerificationResult;
	}

	public virtual CertificateStatus EndVerifyChain(IAsyncResult ar)
	{
		if (ar == null)
		{
			throw new ArgumentNullException();
		}
		CertificateVerificationResult certificateVerificationResult;
		try
		{
			certificateVerificationResult = (CertificateVerificationResult)ar;
		}
		catch
		{
			throw new ArgumentException();
		}
		if (certificateVerificationResult.Chain != this)
		{
			throw new ArgumentException();
		}
		if (certificateVerificationResult.HasEnded)
		{
			throw new InvalidOperationException();
		}
		if (certificateVerificationResult.ThrowException != null)
		{
			throw certificateVerificationResult.ThrowException;
		}
		certificateVerificationResult.HasEnded = true;
		return certificateVerificationResult.Status;
	}

	protected void StartVerification(object state)
	{
		if (state != null)
		{
			CertificateVerificationResult certificateVerificationResult;
			try
			{
				certificateVerificationResult = (CertificateVerificationResult)state;
			}
			catch
			{
				return;
			}
			CertificateStatus status;
			try
			{
				status = VerifyChain(certificateVerificationResult.Server, certificateVerificationResult.Type, certificateVerificationResult.Flags);
			}
			catch (CertificateException error)
			{
				certificateVerificationResult.VerificationCompleted(error, CertificateStatus.OtherError);
				return;
			}
			catch (Exception inner)
			{
				certificateVerificationResult.VerificationCompleted(new CertificateException("Could not verify the certificate chain.", inner), CertificateStatus.OtherError);
				return;
			}
			certificateVerificationResult.VerificationCompleted(null, status);
		}
	}
}
