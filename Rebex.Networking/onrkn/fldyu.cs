using System;
using System.IO;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class fldyu
{
	private class vkqvn
	{
		private readonly HashingAlgorithmId glmig;

		private readonly byte[] nfoaa;

		public vkqvn(TlsMacAlgorithm mac, byte[] key)
		{
			switch (mac)
			{
			case TlsMacAlgorithm.MD5:
				glmig = HashingAlgorithmId.MD5;
				break;
			case TlsMacAlgorithm.SHA1:
				glmig = HashingAlgorithmId.SHA1;
				break;
			case TlsMacAlgorithm.SHA256:
				glmig = HashingAlgorithmId.SHA256;
				break;
			case TlsMacAlgorithm.SHA384:
				glmig = HashingAlgorithmId.SHA384;
				break;
			default:
				throw new NotSupportedException("Unsupported MAC algorithm.");
			}
			nfoaa = key;
		}

		public IHashTransform xxowj(TlsProtocol p0)
		{
			if (p0 == TlsProtocol.SSL30)
			{
				return new ecjay(glmig, nfoaa, oayqn.bsnif);
			}
			HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(glmig);
			hashingAlgorithm.KeyMode = HashingAlgorithmKeyMode.HMAC;
			hashingAlgorithm.SetKey(nfoaa);
			return hashingAlgorithm.CreateTransform();
		}
	}

	private TlsCipher ufkgk;

	private byte[] eawmo;

	private byte[] uyfrm;

	private byte[] tkycd;

	private TlsConnectionEnd izpqh;

	private TlsProtocol hndco;

	private bpnki lagzk;

	private bpnki sezzx;

	public bpnki pbohw => lagzk;

	public bpnki juoso => sezzx;

	public byte[] qqffv
	{
		get
		{
			return eawmo;
		}
		set
		{
			eawmo = value;
		}
	}

	public fldyu(TlsCipher cipher, TlsConnectionEnd connectionEnd, byte[] clientRandom, byte[] serverRandom)
	{
		ufkgk = cipher;
		hndco = ufkgk.Protocol;
		izpqh = connectionEnd;
		uyfrm = new byte[64];
		Array.Copy(clientRandom, 0, uyfrm, 0, 32);
		Array.Copy(serverRandom, 0, uyfrm, 32, 32);
		tkycd = new byte[64];
		Array.Copy(serverRandom, 0, tkycd, 0, 32);
		Array.Copy(clientRandom, 0, tkycd, 32, 32);
	}

	public void opzff(sgjhx p0, MemoryStream p1)
	{
		byte[] p2;
		string p3;
		if (p1 == null || 1 == 0)
		{
			p2 = uyfrm;
			p3 = "master secret";
		}
		else
		{
			p2 = wfzao(p1);
			p3 = "extended master secret";
		}
		switch (hndco)
		{
		case TlsProtocol.TLS10:
		case TlsProtocol.TLS11:
		case TlsProtocol.TLS12:
			eawmo = p0.ffrgi(hndco, p3, ufkgk.mabiw, p2);
			break;
		case TlsProtocol.SSL30:
		{
			onash onash2 = (onash)p0;
			betck betck2 = new betck(onash2.sxggy, uyfrm);
			eawmo = betck2.GetBytes(48);
			break;
		}
		default:
			throw new NotSupportedException("Unsupported TLS/SSL protocol version.");
		}
		p0.Dispose();
	}

	public void ybvgj()
	{
		switch (hndco)
		{
		case TlsProtocol.TLS12:
			czpif(ufkgk.mabiw);
			break;
		case TlsProtocol.TLS10:
		case TlsProtocol.TLS11:
			czpif(SignatureHashAlgorithm.MD5SHA1);
			break;
		case TlsProtocol.SSL30:
			ndxja();
			break;
		default:
			throw new NotSupportedException("Unsupported TLS/SSL protocol version.");
		}
	}

	private void czpif(SignatureHashAlgorithm p0)
	{
		abwyb abwyb2 = new abwyb(p0, eawmo, "key expansion", tkycd);
		byte[] key = null;
		byte[] key2 = null;
		if (ufkgk.CipherMode != TlsBulkCipherMode.GCM)
		{
			key = abwyb2.GetBytes(ufkgk.MacSize);
			key2 = abwyb2.GetBytes(ufkgk.MacSize);
		}
		byte[] bytes = abwyb2.GetBytes(ufkgk.KeyMaterialSize);
		byte[] bytes2 = abwyb2.GetBytes(ufkgk.KeyMaterialSize);
		byte[] array = null;
		byte[] array2 = null;
		if (!ufkgk.Exportable || 1 == 0)
		{
			if (ufkgk.CipherMode != TlsBulkCipherMode.Stream)
			{
				array = abwyb2.GetBytes(ufkgk.iclvv);
				array2 = abwyb2.GetBytes(ufkgk.iclvv);
			}
		}
		else
		{
			abwyb abwyb3 = new abwyb(p0, bytes, "client write key", uyfrm);
			bytes = abwyb3.GetBytes(ufkgk.KeySize);
			abwyb3.jfzmp();
			abwyb abwyb4 = new abwyb(p0, bytes2, "server write key", uyfrm);
			bytes2 = abwyb4.GetBytes(ufkgk.KeySize);
			abwyb4.jfzmp();
			if (ufkgk.Cbc && 0 == 0)
			{
				abwyb abwyb5 = new abwyb(p0, new byte[0], "IV block", uyfrm);
				array = abwyb5.GetBytes(ufkgk.iclvv);
				array2 = abwyb5.GetBytes(ufkgk.iclvv);
				abwyb5.jfzmp();
			}
		}
		abwyb2.jfzmp();
		vkqvn p1 = null;
		vkqvn p2 = null;
		if (ufkgk.CipherMode != TlsBulkCipherMode.GCM)
		{
			p1 = new vkqvn(ufkgk.MacAlgorithm, key);
			p2 = new vkqvn(ufkgk.MacAlgorithm, key2);
		}
		switch (ufkgk.CipherMode)
		{
		case TlsBulkCipherMode.CBC:
			qlhim(p1, p2, bytes, bytes2, array, array2);
			break;
		case TlsBulkCipherMode.Stream:
			lqfkz(p1, p2, bytes, bytes2);
			break;
		case TlsBulkCipherMode.GCM:
			uztkl(bytes, bytes2, array, array2);
			break;
		default:
			throw new NotSupportedException("Unsupported cipher suite.");
		}
	}

	private void ndxja()
	{
		betck betck2 = new betck(eawmo, tkycd);
		byte[] bytes = betck2.GetBytes(ufkgk.MacSize);
		byte[] bytes2 = betck2.GetBytes(ufkgk.MacSize);
		byte[] array = betck2.GetBytes(ufkgk.KeyMaterialSize);
		byte[] array2 = betck2.GetBytes(ufkgk.KeyMaterialSize);
		byte[] array3 = null;
		byte[] array4 = null;
		if (!ufkgk.Exportable || 1 == 0)
		{
			if (ufkgk.Cbc && 0 == 0)
			{
				array3 = betck2.GetBytes(ufkgk.BlockSize);
				array4 = betck2.GetBytes(ufkgk.BlockSize);
			}
		}
		else
		{
			IHashTransform hashTransform = new HashingAlgorithm(HashingAlgorithmId.MD5).CreateTransform();
			hashTransform.Reset();
			hashTransform.Process(array, 0, array.Length);
			hashTransform.Process(uyfrm, 0, uyfrm.Length);
			array = new byte[ufkgk.KeySize];
			Array.Copy(hashTransform.GetHash(), 0, array, 0, ufkgk.KeySize);
			hashTransform.Reset();
			hashTransform.Process(array2, 0, array2.Length);
			hashTransform.Process(tkycd, 0, tkycd.Length);
			array2 = new byte[ufkgk.KeySize];
			Array.Copy(hashTransform.GetHash(), 0, array2, 0, ufkgk.KeySize);
			if (ufkgk.Cbc && 0 == 0)
			{
				hashTransform.Reset();
				hashTransform.Process(uyfrm, 0, uyfrm.Length);
				array3 = new byte[ufkgk.BlockSize];
				Array.Copy(hashTransform.GetHash(), 0, array3, 0, ufkgk.BlockSize);
				hashTransform.Reset();
				hashTransform.Process(tkycd, 0, tkycd.Length);
				array4 = new byte[ufkgk.BlockSize];
				Array.Copy(hashTransform.GetHash(), 0, array4, 0, ufkgk.BlockSize);
			}
		}
		vkqvn p = new vkqvn(ufkgk.MacAlgorithm, bytes);
		vkqvn p2 = new vkqvn(ufkgk.MacAlgorithm, bytes2);
		switch (ufkgk.CipherMode)
		{
		case TlsBulkCipherMode.CBC:
			qlhim(p, p2, array, array2, array3, array4);
			break;
		case TlsBulkCipherMode.Stream:
			lqfkz(p, p2, array, array2);
			break;
		default:
			throw new NotSupportedException("Unsupported cipher suite.");
		}
	}

	private void qlhim(vkqvn p0, vkqvn p1, byte[] p2, byte[] p3, byte[] p4, byte[] p5)
	{
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = ufkgk.CipherAlgorithm switch
		{
			TlsBulkCipherAlgorithm.RC2 => new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.ArcTwo), 
			TlsBulkCipherAlgorithm.DES => new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.DES), 
			TlsBulkCipherAlgorithm.TripleDES => new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.TripleDES), 
			TlsBulkCipherAlgorithm.AES => new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.AES), 
			TlsBulkCipherAlgorithm.Twofish => new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.Twofish), 
			_ => throw new NotSupportedException("Unsupported cipher suite."), 
		};
		symmetricKeyAlgorithm.BlockSize = ufkgk.BlockSize * 8;
		symmetricKeyAlgorithm.Padding = PaddingMode.None;
		switch (izpqh)
		{
		case TlsConnectionEnd.Client:
			symmetricKeyAlgorithm.SetKey(p2);
			symmetricKeyAlgorithm.SetIV(p4);
			lagzk = new srrkl(p0.xxowj(hndco), symmetricKeyAlgorithm.CreateEncryptor(), ufkgk.Cbc, hndco);
			symmetricKeyAlgorithm.SetKey(p3);
			symmetricKeyAlgorithm.SetIV(p5);
			sezzx = new kbyou(p1.xxowj(hndco), symmetricKeyAlgorithm.CreateDecryptor(), ufkgk.Cbc, hndco);
			break;
		case TlsConnectionEnd.Server:
			symmetricKeyAlgorithm.SetKey(p3);
			symmetricKeyAlgorithm.SetIV(p5);
			lagzk = new srrkl(p1.xxowj(hndco), symmetricKeyAlgorithm.CreateEncryptor(), ufkgk.Cbc, hndco);
			symmetricKeyAlgorithm.SetKey(p2);
			symmetricKeyAlgorithm.SetIV(p4);
			sezzx = new kbyou(p0.xxowj(hndco), symmetricKeyAlgorithm.CreateDecryptor(), ufkgk.Cbc, hndco);
			break;
		default:
			throw new NotSupportedException("Unsupported entity.");
		}
	}

	private void uztkl(byte[] p0, byte[] p1, byte[] p2, byte[] p3)
	{
		byte[] array;
		byte[] salt;
		byte[] array2;
		byte[] salt2;
		switch (izpqh)
		{
		case TlsConnectionEnd.Client:
			array = p0;
			salt = p2;
			array2 = p1;
			salt2 = p3;
			break;
		case TlsConnectionEnd.Server:
			array = p1;
			salt = p3;
			array2 = p0;
			salt2 = p2;
			break;
		default:
			throw new NotSupportedException("Unsupported entity.");
		}
		switch (ufkgk.CipherAlgorithm)
		{
		case TlsBulkCipherAlgorithm.AES:
		{
			int blockSize = ufkgk.BlockSize;
			int mxsqt = ufkgk.mxsqt;
			fhryo encryptor = wfcez.stfbw(array, blockSize, mxsqt);
			gajry decryptor = wfcez.usflo(array2, blockSize, mxsqt);
			lagzk = new qeldc(encryptor, salt);
			sezzx = new skzlx(decryptor, salt2);
			break;
		}
		case TlsBulkCipherAlgorithm.Chacha20Poly1305:
		{
			fhryo encryptor = new nvqxb(array);
			gajry decryptor = new fmjea(array2);
			lagzk = new yebnx(encryptor, salt);
			sezzx = new spqck(decryptor, salt2);
			break;
		}
		default:
			throw new NotSupportedException("Unsupported cipher suite.");
		}
	}

	private void lqfkz(vkqvn p0, vkqvn p1, byte[] p2, byte[] p3)
	{
		if (ufkgk.CipherAlgorithm != TlsBulkCipherAlgorithm.RC4)
		{
			throw new NotSupportedException("Unsupported cipher suite.");
		}
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.ArcFour);
		switch (izpqh)
		{
		case TlsConnectionEnd.Client:
			symmetricKeyAlgorithm.SetKey(p2);
			lagzk = new srrkl(p0.xxowj(hndco), symmetricKeyAlgorithm.CreateEncryptor(), ufkgk.Cbc, hndco);
			symmetricKeyAlgorithm.SetKey(p3);
			sezzx = new kbyou(p1.xxowj(hndco), symmetricKeyAlgorithm.CreateDecryptor(), ufkgk.Cbc, hndco);
			break;
		case TlsConnectionEnd.Server:
			symmetricKeyAlgorithm.SetKey(p3);
			lagzk = new srrkl(p1.xxowj(hndco), symmetricKeyAlgorithm.CreateEncryptor(), ufkgk.Cbc, hndco);
			symmetricKeyAlgorithm.SetKey(p2);
			sezzx = new kbyou(p0.xxowj(hndco), symmetricKeyAlgorithm.CreateDecryptor(), ufkgk.Cbc, hndco);
			break;
		default:
			throw new NotSupportedException("Unsupported entity.");
		}
	}

	public void llosq(KeyAlgorithm p0, SignatureHashAlgorithm p1, nxtme<byte> p2, out SignatureHashAlgorithm p3, out HashingAlgorithmId p4)
	{
		if (p0 == KeyAlgorithm.DSA)
		{
			if (hndco < TlsProtocol.TLS12 || ofuit.mhozp(p2, p0, SignatureHashAlgorithm.SHA1))
			{
				p4 = HashingAlgorithmId.SHA1;
				p3 = SignatureHashAlgorithm.SHA1;
				return;
			}
			throw new TlsException(mjddr.jhrgr, "All usable signature algorithms have been disabled.");
		}
		switch (hndco)
		{
		case TlsProtocol.TLS12:
			p3 = ofuit.uojyj(p2, p0, p1);
			if (p3 == SignatureHashAlgorithm.Unsupported)
			{
				throw new TlsException(mjddr.jhrgr, "All usable signature algorithms have been disabled.");
			}
			p4 = bpkgq.wrqur(p3);
			break;
		case TlsProtocol.SSL30:
		case TlsProtocol.TLS10:
		case TlsProtocol.TLS11:
			p4 = (HashingAlgorithmId)0;
			p3 = SignatureHashAlgorithm.MD5SHA1;
			break;
		default:
			throw new NotSupportedException("Unsupported TLS/SSL protocol version.");
		}
	}

	public byte[] wfrbn(MemoryStream p0, HashingAlgorithmId p1)
	{
		if (hndco == TlsProtocol.SSL30)
		{
			eojik eojik2 = new eojik(SignatureHashAlgorithm.MD5SHA1);
			try
			{
				eojik2.Process(p0.GetBuffer(), 0, (int)p0.Length);
				return eojik2.qgbqt(eawmo);
			}
			finally
			{
				if (eojik2 != null && 0 == 0)
				{
					((IDisposable)eojik2).Dispose();
				}
			}
		}
		if (p1 == (HashingAlgorithmId)0 || 1 == 0)
		{
			byte[] buffer = p0.GetBuffer();
			HashingAlgorithm hashingAlgorithm = new HashingAlgorithm((HashingAlgorithmId)65543);
			byte[] p2;
			try
			{
				p2 = hashingAlgorithm.ComputeHash(buffer, 0, (int)p0.Length);
			}
			finally
			{
				if (hashingAlgorithm != null && 0 == 0)
				{
					((IDisposable)hashingAlgorithm).Dispose();
				}
			}
			HashingAlgorithm hashingAlgorithm2 = new HashingAlgorithm(HashingAlgorithmId.SHA1);
			byte[] p3;
			try
			{
				p3 = hashingAlgorithm2.ComputeHash(buffer, 0, (int)p0.Length);
			}
			finally
			{
				if (hashingAlgorithm2 != null && 0 == 0)
				{
					((IDisposable)hashingAlgorithm2).Dispose();
				}
			}
			return jlfbq.usqov(p2, p3);
		}
		IHashTransform hashTransform = new HashingAlgorithm(p1).CreateTransform();
		try
		{
			hashTransform.Process(p0.GetBuffer(), 0, (int)p0.Length);
			return hashTransform.GetHash();
		}
		finally
		{
			if (hashTransform != null && 0 == 0)
			{
				hashTransform.Dispose();
			}
		}
	}

	public byte[] wgqsm(MemoryStream p0, TlsConnectionEnd p1)
	{
		SignatureHashAlgorithm signatureHashAlgorithm;
		switch (hndco)
		{
		case TlsProtocol.TLS12:
			signatureHashAlgorithm = ufkgk.mabiw;
			break;
		case TlsProtocol.TLS10:
		case TlsProtocol.TLS11:
			signatureHashAlgorithm = SignatureHashAlgorithm.MD5SHA1;
			if (signatureHashAlgorithm != SignatureHashAlgorithm.MD5)
			{
				break;
			}
			goto case TlsProtocol.SSL30;
		case TlsProtocol.SSL30:
		{
			byte[] array = ((p1 != TlsConnectionEnd.Client) ? new byte[4] { 83, 82, 86, 82 } : new byte[4] { 67, 76, 78, 84 });
			eojik eojik2 = new eojik(SignatureHashAlgorithm.MD5SHA1);
			try
			{
				eojik2.Process(p0.GetBuffer(), 0, (int)p0.Length);
				eojik2.Process(array, 0, array.Length);
				return eojik2.qgbqt(eawmo);
			}
			finally
			{
				if (eojik2 != null && 0 == 0)
				{
					((IDisposable)eojik2).Dispose();
				}
			}
		}
		default:
			throw new NotSupportedException("Unsupported TLS/SSL protocol version.");
		}
		string label = ((p1 != TlsConnectionEnd.Client) ? "server finished" : "client finished");
		eojik eojik3 = new eojik(signatureHashAlgorithm);
		byte[] hash;
		try
		{
			eojik3.Process(p0.GetBuffer(), 0, (int)p0.Length);
			hash = eojik3.GetHash();
		}
		finally
		{
			if (eojik3 != null && 0 == 0)
			{
				((IDisposable)eojik3).Dispose();
			}
		}
		Rebex.Security.Cryptography.DeriveBytes deriveBytes = new abwyb(signatureHashAlgorithm, eawmo, label, hash);
		return deriveBytes.GetBytes(12);
	}

	private byte[] wfzao(MemoryStream p0)
	{
		SignatureHashAlgorithm signatureHashAlgorithm;
		switch (hndco)
		{
		case TlsProtocol.TLS12:
			signatureHashAlgorithm = ufkgk.mabiw;
			break;
		case TlsProtocol.TLS10:
		case TlsProtocol.TLS11:
			signatureHashAlgorithm = SignatureHashAlgorithm.MD5SHA1;
			if (signatureHashAlgorithm == SignatureHashAlgorithm.MD5)
			{
				goto default;
			}
			break;
		default:
			throw new NotSupportedException("Unsupported TLS/SSL protocol version.");
		}
		eojik eojik2 = new eojik(signatureHashAlgorithm);
		try
		{
			eojik2.Process(p0.GetBuffer(), 0, (int)p0.Length);
			return eojik2.GetHash();
		}
		finally
		{
			if (eojik2 != null && 0 == 0)
			{
				((IDisposable)eojik2).Dispose();
			}
		}
	}
}
