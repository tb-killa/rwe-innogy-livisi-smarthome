using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class tbinn : lnabj
{
	private enum vvtsd
	{
		dymyz,
		qqmii,
		astzq
	}

	private AlgorithmIdentifier mdurw;

	private AlgorithmIdentifier dlwvy;

	private readonly rwolq bumrm;

	private bool hmgte;

	public AlgorithmIdentifier fvcvi
	{
		get
		{
			if (mdurw == null || 1 == 0)
			{
				return null;
			}
			if (dlwvy == null || 1 == 0)
			{
				dlwvy = mdurw.evxkk();
			}
			return dlwvy;
		}
	}

	public tbinn()
	{
		bumrm = new rwolq();
	}

	public tbinn(PrivateKeyInfo privateKey, string password, ObjectIdentifier encryptionAlgorithm)
	{
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(encryptionAlgorithm.Value);
		try
		{
			byte[] randomBytes = CryptoHelper.GetRandomBytes(symmetricKeyAlgorithm.BlockSize / 8);
			int num = 2048;
			Rebex.Security.Cryptography.Rfc2898DeriveBytes generator = new Rebex.Security.Cryptography.Rfc2898DeriveBytes(password, randomBytes, num);
			symmetricKeyAlgorithm.DeriveKey(generator);
			symmetricKeyAlgorithm.GenerateIV();
			isnqv p = new isnqv(randomBytes, num);
			AlgorithmIdentifier keyDerivationAlgorithm = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.5.12"), fxakl.kncuz(p));
			AlgorithmIdentifier encryptionAlgorithm2 = new AlgorithmIdentifier(symmetricKeyAlgorithm);
			fyyhl p2 = new fyyhl(keyDerivationAlgorithm, encryptionAlgorithm2);
			mdurw = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.5.13"), fxakl.kncuz(p2));
			byte[] array = fxakl.kncuz(privateKey);
			ICryptoTransform cryptoTransform = symmetricKeyAlgorithm.CreateEncryptor();
			array = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			bumrm = new rwolq(array);
		}
		finally
		{
			if (symmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)symmetricKeyAlgorithm).Dispose();
			}
		}
	}

	public byte[] nbero(string p0)
	{
		HashingAlgorithmId hashingAlgorithmId = HashingAlgorithmId.SHA1;
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = null;
		vvtsd vvtsd = vvtsd.astzq;
		int num = 64;
		int num2 = 64;
		byte[] parameters = mdurw.Parameters;
		string value;
		if ((value = mdurw.Oid.Value) != null && 0 == 0)
		{
			if (fnfqw.yaryb == null || 1 == 0)
			{
				fnfqw.yaryb = new Dictionary<string, int>(11)
				{
					{ "1.2.840.113549.1.5.10", 0 },
					{ "1.2.840.113549.1.5.13", 1 },
					{ "1.2.840.113549.1.5.3", 2 },
					{ "1.2.840.113549.1.5.6", 3 },
					{ "1.2.840.113549.1.5.11", 4 },
					{ "1.2.840.113549.1.12.1.1", 5 },
					{ "1.2.840.113549.1.12.1.2", 6 },
					{ "1.2.840.113549.1.12.1.5", 7 },
					{ "1.2.840.113549.1.12.1.6", 8 },
					{ "1.2.840.113549.1.12.1.3", 9 },
					{ "1.2.840.113549.1.12.1.4", 10 }
				};
			}
			if (fnfqw.yaryb.TryGetValue(value, out var value2) && 0 == 0)
			{
				string oid;
				isnqv isnqv2;
				byte[] salt;
				int beqkw;
				ICryptoTransform cryptoTransform;
				byte[] rtrhq;
				switch (value2)
				{
				case 0:
					oid = "DES";
					vvtsd = vvtsd.dymyz;
					if (vvtsd != vvtsd.dymyz)
					{
						goto case 1;
					}
					goto IL_0281;
				case 1:
				{
					oid = null;
					fyyhl fyyhl2 = new fyyhl();
					hfnnn.qnzgo(fyyhl2, parameters);
					if (fyyhl2.tqleq.Oid.Value != "1.2.840.113549.1.5.12" && 0 == 0)
					{
						throw new CryptographicException("Unsupported key derivation algorithm.");
					}
					parameters = fyyhl2.tqleq.Parameters;
					symmetricKeyAlgorithm = fyyhl2.zmjnn.jatvn();
					symmetricKeyAlgorithm.Padding = PaddingMode.None;
					vvtsd = vvtsd.qqmii;
					num = symmetricKeyAlgorithm.KeySize;
					goto IL_0281;
				}
				case 2:
					hashingAlgorithmId = HashingAlgorithmId.MD5;
					oid = "DES";
					vvtsd = vvtsd.dymyz;
					if (vvtsd != vvtsd.dymyz)
					{
						goto case 3;
					}
					goto IL_0281;
				case 3:
					hashingAlgorithmId = HashingAlgorithmId.MD5;
					oid = "RC2";
					vvtsd = vvtsd.dymyz;
					if (vvtsd != vvtsd.dymyz)
					{
						goto case 4;
					}
					goto IL_0281;
				case 4:
					oid = "RC2";
					vvtsd = vvtsd.dymyz;
					if (vvtsd != vvtsd.dymyz)
					{
						goto case 5;
					}
					goto IL_0281;
				case 5:
					oid = "RC4";
					num = 128;
					num2 = 8;
					if (num2 == 0)
					{
						goto case 6;
					}
					goto IL_0281;
				case 6:
					oid = "RC4";
					num = 40;
					num2 = 8;
					if (num2 == 0)
					{
						goto case 7;
					}
					goto IL_0281;
				case 7:
					oid = "RC2";
					num = 128;
					if (num == 0)
					{
						goto case 8;
					}
					goto IL_0281;
				case 8:
					oid = "RC2";
					num = 40;
					if (num == 0)
					{
						goto case 9;
					}
					goto IL_0281;
				case 9:
					oid = "3DES";
					num = 192;
					if (num == 0)
					{
						goto case 10;
					}
					goto IL_0281;
				case 10:
					{
						oid = "3DES";
						num = 128;
						if (num == 0)
						{
							break;
						}
						goto IL_0281;
					}
					IL_0281:
					if (symmetricKeyAlgorithm == null || 1 == 0)
					{
						symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(oid);
						symmetricKeyAlgorithm.Mode = CipherMode.CBC;
						symmetricKeyAlgorithm.Padding = PaddingMode.None;
						symmetricKeyAlgorithm.KeySize = num;
						symmetricKeyAlgorithm.BlockSize = num2;
					}
					if (symmetricKeyAlgorithm == null || 1 == 0)
					{
						throw new CryptographicException("Unsupported encryption algorithm.");
					}
					isnqv2 = new isnqv();
					hfnnn.qnzgo(isnqv2, parameters);
					salt = isnqv2.ufvba();
					beqkw = isnqv2.beqkw;
					switch (vvtsd)
					{
					case vvtsd.dymyz:
					{
						rbplv rbplv2 = new rbplv(hashingAlgorithmId, p0, salt, beqkw);
						byte[] bytes = rbplv2.GetBytes(16);
						byte[] array = new byte[8];
						byte[] array2 = new byte[8];
						Array.Copy(bytes, 0, array, 0, 8);
						Array.Copy(bytes, 8, array2, 0, 8);
						symmetricKeyAlgorithm.SetKey(array);
						symmetricKeyAlgorithm.SetIV(array2);
						break;
					}
					case vvtsd.astzq:
					{
						Pkcs12KeyGenerator generator2 = new Pkcs12KeyGenerator(hashingAlgorithmId, p0, salt, beqkw, 1);
						Pkcs12KeyGenerator generator3 = new Pkcs12KeyGenerator(hashingAlgorithmId, p0, salt, beqkw, 2);
						symmetricKeyAlgorithm.DeriveKey(generator2);
						symmetricKeyAlgorithm.DeriveIV(generator3);
						break;
					}
					case vvtsd.qqmii:
					{
						Rebex.Security.Cryptography.Rfc2898DeriveBytes generator = new Rebex.Security.Cryptography.Rfc2898DeriveBytes(isnqv2.swvyl, p0, salt, beqkw);
						symmetricKeyAlgorithm.DeriveKey(generator);
						break;
					}
					}
					rtrhq = bumrm.rtrhq;
					cryptoTransform = symmetricKeyAlgorithm.CreateDecryptor();
					rtrhq = cryptoTransform.TransformFinalBlock(rtrhq, 0, rtrhq.Length);
					return PrivateKeyInfo.dgros(rtrhq, rtrhq.Length, symmetricKeyAlgorithm.BlockSize / 8);
				}
			}
		}
		throw new CryptographicException(brgjd.edcru("Unsupported key encryption algorithm ({0}).", mdurw.Oid.Value));
	}

	private void aavnn(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in aavnn
		this.aavnn(p0, p1, p2);
	}

	private lnabj jbwst(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			mdurw = new AlgorithmIdentifier();
			return mdurw;
		case 1:
			hmgte = true;
			return bumrm;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jbwst
		return this.jbwst(p0, p1, p2);
	}

	private void fwfwq(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fwfwq
		this.fwfwq(p0, p1, p2);
	}

	private void ocbcx()
	{
		if (mdurw == null || 1 == 0)
		{
			throw new CryptographicException("Encryption algorithm not found in EncryptedPrivateKeyInfo.");
		}
		if (!hmgte || 1 == 0)
		{
			throw new CryptographicException("Encrypted data not found in EncryptedPrivateKeyInfo.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ocbcx
		this.ocbcx();
	}

	private void ewoss(fxakl p0)
	{
		p0.suudj(mdurw, bumrm);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ewoss
		this.ewoss(p0);
	}
}
