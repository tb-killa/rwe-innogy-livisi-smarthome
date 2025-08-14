using System;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class vssis : ofuit
{
	private SignatureHashAlgorithm? wgyrq;

	private KeyAlgorithm? dxkfc;

	private byte[] dqqim;

	private SignatureParameters lyiqc;

	public override int nimwj
	{
		get
		{
			int num = dqqim.Length + 6;
			if (wgyrq.HasValue && 0 == 0 && dxkfc.HasValue && 0 == 0)
			{
				num += 2;
			}
			return num;
		}
	}

	public byte[] gksmh => dqqim;

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		p1 += 4;
		if (wgyrq.HasValue && 0 == 0 && dxkfc.HasValue && 0 == 0)
		{
			ofuit.chycn(wgyrq.Value, dxkfc.Value, p0, ref p1);
		}
		p0[p1] = (byte)((dqqim.Length >> 8) & 0xFF);
		p0[p1 + 1] = (byte)(dqqim.Length & 0xFF);
		dqqim.CopyTo(p0, p1 + 2);
	}

	public void hrbim(KeyAlgorithm p0, out HashingAlgorithmId p1, out SignatureHashAlgorithm p2)
	{
		if (p0 == KeyAlgorithm.DSA)
		{
			if ((wgyrq ?? SignatureHashAlgorithm.SHA1) != SignatureHashAlgorithm.SHA1)
			{
				throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
			}
			p1 = HashingAlgorithmId.SHA1;
			p2 = SignatureHashAlgorithm.SHA1;
		}
		else
		{
			p2 = wgyrq ?? SignatureHashAlgorithm.MD5SHA1;
			p1 = bpkgq.wrqur(p2);
		}
	}

	public vssis(SignatureHashAlgorithm hashAlg, KeyAlgorithm keyAlg, byte[] signature, TlsProtocol protocol)
		: base(nsvut.enndd)
	{
		if (protocol >= TlsProtocol.TLS12)
		{
			if (hashAlg == SignatureHashAlgorithm.MD5 || false || hashAlg == SignatureHashAlgorithm.MD4)
			{
				throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
			}
			wgyrq = hashAlg;
			dxkfc = keyAlg;
		}
		else if ((hashAlg != SignatureHashAlgorithm.MD5SHA1 && keyAlg != KeyAlgorithm.DSA) || (hashAlg != SignatureHashAlgorithm.SHA1 && keyAlg == KeyAlgorithm.DSA))
		{
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		}
		dqqim = signature;
	}

	public vssis(byte[] buffer, int offset, int length, TlsProtocol protocol)
		: base(nsvut.enndd)
	{
		int p = offset + 4;
		if (protocol >= TlsProtocol.TLS12)
		{
			if (offset + length < p + 2)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateVerify"));
			}
			ofuit.rhqlp(buffer, ref p, out wgyrq, out dxkfc, out lyiqc);
			KeyAlgorithm? keyAlgorithm = dxkfc;
			if ((keyAlgorithm.GetValueOrDefault() == KeyAlgorithm.RSA || 1 == 0) && keyAlgorithm.HasValue && 0 == 0 && lyiqc != null && 0 == 0 && lyiqc.PaddingScheme != SignaturePaddingScheme.Pkcs1)
			{
				throw new TlsException(mjddr.qssln, "Only PKCS #1 padding scheme is supported for client certificate authentication in TLS 1.2.");
			}
		}
		if (offset + length < p + 2)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateVerify"));
		}
		int num = buffer[p] * 256 + buffer[p + 1];
		p += 2;
		if (offset + length < p + num)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "CertificateVerify"));
		}
		dqqim = new byte[num];
		Array.Copy(buffer, p, dqqim, 0, num);
	}
}
