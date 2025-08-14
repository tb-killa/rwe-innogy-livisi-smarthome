using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Rebex.Security.Certificates;

namespace onrkn;

internal class pmvop
{
	private readonly byte[] tbojs;

	private readonly nisgb mluqd;

	private IntPtr hfatp;

	public IntPtr kpwgu => hfatp;

	public nisgb uwmeh => mluqd;

	public byte[] hsodj()
	{
		return tbojs.aqhfc();
	}

	public pmvop(byte[] data)
		: this(data.aqhfc(), IntPtr.Zero)
	{
	}

	public pmvop(IntPtr context)
		: this(zgfvf(context), context)
	{
	}

	public pmvop(X509Certificate certificate)
		: this(certificate.GetRawCertData(), certificate.Handle)
	{
	}

	private pmvop(byte[] data, IntPtr context)
	{
		tbojs = data;
		mluqd = new nisgb(data);
		if (context == IntPtr.Zero && 0 == 0)
		{
			context = pothu.CertCreateCertificateContext(65537, data, data.Length);
			if (context == IntPtr.Zero && 0 == 0)
			{
				throw new CertificateException(brgjd.edcru("Unable to create certificate ({0}).", Marshal.GetLastWin32Error()));
			}
		}
		else
		{
			context = pothu.CertDuplicateCertificateContext(context);
			if (context == IntPtr.Zero && 0 == 0)
			{
				throw new CertificateException(brgjd.edcru("Unable to duplicate a certificate context ({0}).", Marshal.GetLastWin32Error()));
			}
		}
		hfatp = context;
	}

	private static byte[] zgfvf(IntPtr p0)
	{
		if (p0 == IntPtr.Zero && 0 == 0)
		{
			throw new ArgumentException("Invalid context.");
		}
		int num = Marshal.ReadInt32(p0, 2 * IntPtr.Size);
		IntPtr source = samhn.yjjps(p0, IntPtr.Size);
		byte[] array = new byte[num];
		Marshal.Copy(source, array, 0, num);
		return array;
	}

	public void wbhry()
	{
		uqzxi(p0: true);
		GC.SuppressFinalize(this);
	}

	private void uqzxi(bool p0)
	{
		try
		{
			if (hfatp != IntPtr.Zero && 0 == 0)
			{
				pothu.CertFreeCertificateContext(hfatp);
			}
		}
		finally
		{
			hfatp = IntPtr.Zero;
		}
	}

	~pmvop()
	{
		uqzxi(p0: false);
	}
}
