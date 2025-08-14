using System;
using System.IO;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class ccgaj : ofuit
{
	private byte[] qucia;

	private byte[] jvltn;

	private byte[] oxfgt;

	private byte[] edstf;

	private SignatureHashAlgorithm? euelf;

	private KeyAlgorithm? czjiv;

	private urofm? ulprq;

	private bool arqxl;

	private SignatureParameters mmydv;

	public override int nimwj
	{
		get
		{
			int num = 4;
			if (ulprq.HasValue && 0 == 0)
			{
				num += jvltn.Length;
			}
			else
			{
				num += 2 + qucia.Length;
				num += 2 + jvltn.Length;
				num += 2 + oxfgt.Length;
			}
			if ((euelf.HasValue ? true : false) || czjiv.HasValue)
			{
				num += 2;
			}
			if (edstf != null && 0 == 0)
			{
				num += 2 + edstf.Length;
			}
			return num;
		}
	}

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		p1 += 4;
		if (ulprq.HasValue && 0 == 0)
		{
			jvltn.CopyTo(p0, p1);
			p1 += jvltn.Length;
		}
		else
		{
			p0[p1] = (byte)((qucia.Length >> 8) & 0xFF);
			p0[p1 + 1] = (byte)(qucia.Length & 0xFF);
			qucia.CopyTo(p0, p1 + 2);
			p1 += qucia.Length + 2;
			p0[p1] = (byte)((jvltn.Length >> 8) & 0xFF);
			p0[p1 + 1] = (byte)(jvltn.Length & 0xFF);
			jvltn.CopyTo(p0, p1 + 2);
			p1 += jvltn.Length + 2;
			p0[p1] = (byte)((oxfgt.Length >> 8) & 0xFF);
			p0[p1 + 1] = (byte)(oxfgt.Length & 0xFF);
			oxfgt.CopyTo(p0, p1 + 2);
			p1 += oxfgt.Length + 2;
		}
		if ((euelf.HasValue ? true : false) || czjiv.HasValue)
		{
			ofuit.chycn(euelf.Value, czjiv.Value, p0, ref p1);
		}
		if (edstf != null && 0 == 0)
		{
			p0[p1] = (byte)((edstf.Length >> 8) & 0xFF);
			p0[p1 + 1] = (byte)(edstf.Length & 0xFF);
			edstf.CopyTo(p0, p1 + 2);
			p1 += edstf.Length + 2;
		}
	}

	private void aqyoo(IHashTransform p0, byte[] p1, byte[] p2)
	{
		byte[] array = efyao(p1, p2);
		p0.Process(array, 0, array.Length);
	}

	private byte[] efyao(byte[] p0, byte[] p1)
	{
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			memoryStream.Write(p0, 0, p0.Length);
			memoryStream.Write(p1, 0, p1.Length);
			if (ulprq.HasValue && 0 == 0)
			{
				memoryStream.Write(jvltn, 0, jvltn.Length);
			}
			else
			{
				memoryStream.WriteByte((byte)(qucia.Length >> 8));
				memoryStream.WriteByte((byte)(qucia.Length & 0xFF));
				memoryStream.Write(qucia, 0, qucia.Length);
				memoryStream.WriteByte((byte)(jvltn.Length >> 8));
				memoryStream.WriteByte((byte)(jvltn.Length & 0xFF));
				memoryStream.Write(jvltn, 0, jvltn.Length);
				if (oxfgt != null && 0 == 0 && (!arqxl || 1 == 0))
				{
					memoryStream.WriteByte((byte)(oxfgt.Length >> 8));
					memoryStream.WriteByte((byte)(oxfgt.Length & 0xFF));
					memoryStream.Write(oxfgt, 0, oxfgt.Length);
				}
			}
			return memoryStream.ToArray();
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	public bool ynbct(Certificate p0, nxtme<byte> p1, byte[] p2, byte[] p3)
	{
		KeyAlgorithm keyAlgorithm = p0.KeyAlgorithm;
		if (czjiv.HasValue && 0 == 0 && keyAlgorithm != czjiv.Value)
		{
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		}
		if (mmydv != null && 0 == 0)
		{
			SignatureHashAlgorithm p4 = bpkgq.vfyof(mmydv.HashAlgorithm);
			if (!ofuit.kplxz(p1, keyAlgorithm, p4, mmydv.PaddingScheme) || 1 == 0)
			{
				throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
			}
			byte[] signature = ((edstf == null) ? oxfgt : edstf);
			byte[] message = efyao(p2, p3);
			return p0.VerifyMessage(message, signature, mmydv);
		}
		if (euelf.HasValue && 0 == 0 && (!ofuit.mhozp(p1, keyAlgorithm, euelf.Value) || 1 == 0))
		{
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		}
		if (keyAlgorithm == KeyAlgorithm.RSA || 1 == 0)
		{
			byte[] signature2 = ((edstf == null) ? oxfgt : edstf);
			SignatureHashAlgorithm alg = euelf ?? SignatureHashAlgorithm.MD5SHA1;
			eojik eojik2 = new eojik(alg);
			try
			{
				aqyoo(eojik2, p2, p3);
				return p0.VerifyHash(eojik2.GetHash(), alg, signature2);
			}
			finally
			{
				if (eojik2 != null && 0 == 0)
				{
					((IDisposable)eojik2).Dispose();
				}
			}
		}
		if (keyAlgorithm == KeyAlgorithm.ECDsa)
		{
			byte[] signature3 = ((edstf == null) ? oxfgt : edstf);
			SignatureParameters signatureParameters = new SignatureParameters();
			signatureParameters.Format = SignatureFormat.Pkcs;
			signatureParameters.HashAlgorithm = bpkgq.wrqur(euelf ?? SignatureHashAlgorithm.SHA1);
			byte[] message2 = efyao(p2, p3);
			return p0.VerifyMessage(message2, signature3, signatureParameters);
		}
		if (edstf == null || 1 == 0)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ServerKeyExchange"));
		}
		byte[] signature4;
		try
		{
			signature4 = CryptoHelper.DecodeSignature(edstf, KeyAlgorithm.DSA);
		}
		catch (CryptographicException inner)
		{
			throw new TlsException(rtzwv.iogyt, mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ServerKeyExchange"), inner);
		}
		if (euelf.HasValue && 0 == 0 && euelf.Value != SignatureHashAlgorithm.SHA1)
		{
			throw new TlsException(mjddr.qssln, "Unsupported algorithm.");
		}
		IHashTransform hashTransform = new HashingAlgorithm(HashingAlgorithmId.SHA1).CreateTransform();
		aqyoo(hashTransform, p2, p3);
		return p0.VerifyHash(hashTransform.GetHash(), SignatureHashAlgorithm.SHA1, signature4);
	}

	public urofm anrsz()
	{
		return ulprq.GetValueOrDefault();
	}

	public byte[] afugf()
	{
		return qucia;
	}

	public DiffieHellmanParameters acery()
	{
		return new DiffieHellmanParameters
		{
			P = qucia,
			G = jvltn,
			Y = oxfgt
		};
	}

	public RSAParameters dtcci()
	{
		return new RSAParameters
		{
			Modulus = qucia,
			Exponent = jvltn
		};
	}

	public ccgaj(DiffieHellmanParameters param)
		: base(nsvut.iysge)
	{
		qucia = param.P;
		jvltn = param.G;
		oxfgt = param.Y;
	}

	public ccgaj(Certificate cert, nxtme<byte> signatureAlgorithms, DiffieHellmanParameters param, byte[] clientRandom, byte[] serverRandom, TlsProtocol protocol)
		: this(param)
	{
		if (cert.KeyAlgorithm == KeyAlgorithm.RSA || 1 == 0)
		{
			SignatureHashAlgorithm signatureHashAlgorithm;
			if (protocol >= TlsProtocol.TLS12)
			{
				czjiv = KeyAlgorithm.RSA;
				euelf = (signatureHashAlgorithm = ofuit.uojyj(signatureAlgorithms, cert.KeyAlgorithm, SignatureHashAlgorithm.SHA256));
				if (signatureHashAlgorithm == SignatureHashAlgorithm.Unsupported)
				{
					throw new TlsException(mjddr.qssln, "All usable signature algorithms have been disabled.");
				}
			}
			else
			{
				signatureHashAlgorithm = SignatureHashAlgorithm.MD5SHA1;
			}
			eojik eojik2 = new eojik(signatureHashAlgorithm);
			try
			{
				aqyoo(eojik2, clientRandom, serverRandom);
				edstf = cert.SignHash(eojik2.GetHash(), signatureHashAlgorithm, silent: true);
				return;
			}
			finally
			{
				if (eojik2 != null && 0 == 0)
				{
					((IDisposable)eojik2).Dispose();
				}
			}
		}
		if (protocol >= TlsProtocol.TLS12)
		{
			czjiv = KeyAlgorithm.DSA;
			euelf = SignatureHashAlgorithm.SHA1;
			if (!ofuit.mhozp(signatureAlgorithms, KeyAlgorithm.DSA, SignatureHashAlgorithm.SHA1) || 1 == 0)
			{
				throw new TlsException(mjddr.qssln, "All usable signature algorithms have been disabled.");
			}
		}
		IHashTransform hashTransform = new HashingAlgorithm(HashingAlgorithmId.SHA1).CreateTransform();
		aqyoo(hashTransform, clientRandom, serverRandom);
		edstf = CryptoHelper.EncodeSignature(cert.SignHash(hashTransform.GetHash(), SignatureHashAlgorithm.SHA1, silent: true), KeyAlgorithm.DSA);
	}

	public ccgaj(Certificate cert, RSAParameters param, byte[] clientRandom, byte[] serverRandom, TlsProtocol protocol)
		: base(nsvut.iysge)
	{
		if (protocol >= TlsProtocol.TLS11)
		{
			throw new TlsException(mjddr.qssln, "Unsupported cipher suite.");
		}
		qucia = param.Modulus;
		jvltn = param.Exponent;
		eojik eojik2 = new eojik(SignatureHashAlgorithm.MD5SHA1);
		try
		{
			aqyoo(eojik2, clientRandom, serverRandom);
			oxfgt = cert.SignHash(eojik2.GetHash(), SignatureHashAlgorithm.MD5SHA1, silent: true);
			arqxl = true;
		}
		finally
		{
			if (eojik2 != null && 0 == 0)
			{
				((IDisposable)eojik2).Dispose();
			}
		}
	}

	public ccgaj(Certificate cert, nxtme<byte> signatureAlgorithms, urofm curveId, byte[] publicKey, byte[] clientRandom, byte[] serverRandom, TlsProtocol protocol)
		: base(nsvut.iysge)
	{
		ulprq = curveId;
		qucia = publicKey;
		KeyAlgorithm keyAlgorithm = cert.KeyAlgorithm;
		if (keyAlgorithm != KeyAlgorithm.RSA && 0 == 0 && keyAlgorithm != KeyAlgorithm.ECDsa)
		{
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		}
		if (publicKey.Length > 255)
		{
			throw new TlsException(mjddr.qssln, "Unsupported key.");
		}
		wmbjj wmbjj2 = new wmbjj();
		wmbjj2.ywmoe(3);
		wmbjj2.mmgwn((ushort)curveId);
		wmbjj2.ywmoe((byte)publicKey.Length);
		wmbjj2.udtyl(publicKey);
		jvltn = wmbjj2.ihelo();
		SignatureHashAlgorithm signatureHashAlgorithm;
		if (protocol >= TlsProtocol.TLS12)
		{
			signatureHashAlgorithm = ((keyAlgorithm != KeyAlgorithm.ECDsa) ? ofuit.uojyj(signatureAlgorithms, keyAlgorithm, SignatureHashAlgorithm.SHA256) : ofuit.uojyj(signatureAlgorithms, keyAlgorithm, bpkgq.gjkao(cert.awyrh())));
			if (signatureHashAlgorithm == SignatureHashAlgorithm.Unsupported)
			{
				throw new TlsException(mjddr.qssln, "All usable signature algorithms have been disabled.");
			}
			czjiv = keyAlgorithm;
			euelf = signatureHashAlgorithm;
		}
		else
		{
			if (keyAlgorithm == KeyAlgorithm.ECDsa)
			{
				signatureHashAlgorithm = SignatureHashAlgorithm.SHA1;
				if (signatureHashAlgorithm != SignatureHashAlgorithm.MD5)
				{
					goto IL_00ee;
				}
			}
			signatureHashAlgorithm = SignatureHashAlgorithm.MD5SHA1;
		}
		goto IL_00ee;
		IL_00ee:
		if (keyAlgorithm == KeyAlgorithm.ECDsa)
		{
			edstf = cert.SignMessage(parameters: new SignatureParameters
			{
				Format = SignatureFormat.Pkcs,
				HashAlgorithm = bpkgq.wrqur(signatureHashAlgorithm)
			}, message: efyao(clientRandom, serverRandom));
			return;
		}
		eojik eojik2 = new eojik(signatureHashAlgorithm);
		try
		{
			aqyoo(eojik2, clientRandom, serverRandom);
			edstf = cert.SignHash(eojik2.GetHash(), signatureHashAlgorithm, silent: true);
		}
		finally
		{
			if (eojik2 != null && 0 == 0)
			{
				((IDisposable)eojik2).Dispose();
			}
		}
	}

	public ccgaj(byte[] buffer, int offset, int length, TlsProtocol protocol, TlsCipher cipher)
		: base(nsvut.iysge)
	{
		int num = offset + 4;
		TlsKeyExchangeAlgorithm keyExchangeAlgorithm = cipher.KeyExchangeAlgorithm;
		if (keyExchangeAlgorithm == TlsKeyExchangeAlgorithm.ECDHE_RSA || keyExchangeAlgorithm == TlsKeyExchangeAlgorithm.ECDHE_ECDSA)
		{
			wmbjj wmbjj2 = new wmbjj(buffer, length);
			int num2 = num;
			wmbjj2.xqsga(num);
			if (wmbjj2.fhuaz() != 3)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ServerKeyExchange"));
			}
			urofm value = (urofm)wmbjj2.mytfp();
			byte[] array = wmbjj2.dliku(wmbjj2.fhuaz());
			int num3 = (num = wmbjj2.hpxkw);
			if (protocol >= TlsProtocol.TLS12)
			{
				ofuit.rhqlp(buffer, ref num, out euelf, out czjiv, out mmydv);
			}
			wmbjj2.hpxkw = num;
			byte[] array2 = wmbjj2.dliku(wmbjj2.mytfp());
			ulprq = value;
			qucia = array;
			jvltn = new byte[num3 - num2];
			Array.Copy(buffer, num2, jvltn, 0, jvltn.Length);
			edstf = array2;
			return;
		}
		if (offset + length >= num + 2)
		{
			int num4 = buffer[num] * 256 + buffer[num + 1];
			if (offset + length >= num + 2 + num4)
			{
				qucia = new byte[num4];
				Array.Copy(buffer, num + 2, qucia, 0, num4);
				num += 2 + num4;
				if (offset + length >= num + 2)
				{
					num4 = buffer[num] * 256 + buffer[num + 1];
					if (offset + length >= num + 2 + num4)
					{
						jvltn = new byte[num4];
						Array.Copy(buffer, num + 2, jvltn, 0, num4);
						num += 2 + num4;
						if (offset + length >= num + 2)
						{
							num4 = buffer[num] * 256 + buffer[num + 1];
							if (offset + length >= num + 2 + num4)
							{
								oxfgt = new byte[num4];
								Array.Copy(buffer, num + 2, oxfgt, 0, num4);
								num += 2 + num4;
								if (cipher.Exportable && 0 == 0 && cipher.KeyExchangeAlgorithm == TlsKeyExchangeAlgorithm.RSA)
								{
									arqxl = true;
								}
								if (offset + length < num + 2)
								{
									return;
								}
								if (protocol >= TlsProtocol.TLS12)
								{
									ofuit.rhqlp(buffer, ref num, out euelf, out czjiv, out mmydv);
									if (offset + length < num + 2)
									{
										goto IL_0289;
									}
								}
								num4 = buffer[num] * 256 + buffer[num + 1];
								if (offset + length >= num + 2 + num4)
								{
									edstf = new byte[num4];
									Array.Copy(buffer, num + 2, edstf, 0, num4);
									num += 2 + num4;
									return;
								}
							}
						}
					}
				}
			}
		}
		goto IL_0289;
		IL_0289:
		throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ServerKeyExchange"));
	}
}
