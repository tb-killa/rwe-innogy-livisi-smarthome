using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class qpehr : imfrk
{
	private readonly AsymmetricKeyAlgorithmId qfdmb;

	private readonly int cznbp;

	private static readonly object dijcr = new object();

	private static bool? jbynz;

	private static bool hfkhi;

	private static readonly byte[] llisl = new byte[607]
	{
		48, 130, 2, 91, 2, 1, 0, 2, 129, 129,
		0, 191, 117, 33, 86, 52, 250, 209, 126, 126,
		229, 255, 68, 17, 93, 135, 54, 214, 202, 94,
		205, 86, 43, 169, 59, 244, 165, 90, 126, 45,
		100, 23, 39, 4, 212, 116, 126, 49, 220, 255,
		121, 205, 193, 250, 142, 182, 77, 118, 138, 163,
		198, 179, 46, 103, 247, 232, 203, 32, 112, 181,
		70, 117, 102, 103, 155, 24, 196, 101, 196, 244,
		14, 84, 179, 93, 81, 189, 175, 88, 109, 82,
		134, 10, 148, 9, 231, 229, 201, 25, 171, 155,
		91, 179, 236, 192, 242, 241, 19, 250, 98, 157,
		248, 241, 213, 65, 0, 120, 245, 49, 29, 168,
		29, 4, 111, 13, 91, 168, 176, 115, 100, 125,
		11, 162, 241, 93, 53, 46, 25, 139, 249, 2,
		3, 1, 0, 1, 2, 129, 128, 6, 248, 106,
		184, 74, 193, 126, 243, 14, 7, 173, 157, 122,
		204, 94, 233, 222, 52, 11, 243, 137, 217, 153,
		21, 183, 184, 117, 108, 246, 150, 24, 73, 177,
		97, 82, 196, 109, 104, 80, 92, 204, 226, 237,
		14, 89, 16, 196, 234, 19, 64, 94, 177, 167,
		211, 92, 196, 88, 112, 2, 9, 136, 168, 171,
		200, 231, 152, 150, 72, 37, 65, 190, 120, 75,
		11, 219, 22, 4, 4, 175, 114, 236, 71, 246,
		61, 55, 153, 49, 3, 53, 159, 217, 70, 167,
		36, 238, 22, 164, 113, 105, 129, 6, 195, 52,
		42, 102, 232, 67, 138, 152, 132, 184, 130, 93,
		211, 120, 4, 22, 42, 229, 18, 30, 195, 35,
		20, 114, 216, 68, 41, 2, 65, 0, 204, 206,
		68, 88, 228, 229, 195, 23, 199, 138, 255, 232,
		184, 59, 223, 10, 16, 224, 247, 121, 24, 139,
		117, 188, 245, 66, 35, 220, 227, 236, 87, 195,
		198, 38, 243, 212, 154, 147, 110, 234, 229, 154,
		197, 48, 27, 37, 216, 176, 209, 201, 75, 33,
		150, 86, 250, 186, 170, 208, 54, 89, 157, 177,
		150, 71, 2, 65, 0, 239, 80, 178, 213, 10,
		115, 15, 34, 211, 88, 165, 28, 197, 61, 189,
		87, 244, 61, 108, 110, 154, 196, 250, 93, 243,
		60, 168, 120, 137, 42, 242, 50, 136, 198, 19,
		226, 166, 51, 33, 24, 9, 22, 199, 118, 170,
		2, 1, 35, 123, 27, 73, 166, 59, 203, 72,
		178, 214, 132, 243, 102, 214, 127, 171, 191, 2,
		64, 37, 50, 133, 105, 91, 209, 123, 56, 147,
		110, 100, 130, 97, 11, 198, 187, 174, 75, 29,
		199, 105, 180, 210, 162, 138, 45, 4, 20, 119,
		117, 18, 143, 165, 42, 167, 248, 130, 70, 170,
		203, 144, 254, 38, 56, 81, 133, 243, 48, 82,
		57, 236, 34, 98, 138, 211, 169, 25, 163, 13,
		108, 3, 95, 32, 187, 2, 64, 70, 68, 144,
		173, 227, 42, 147, 152, 43, 44, 77, 22, 220,
		135, 91, 80, 55, 3, 206, 17, 207, 217, 228,
		149, 175, 116, 241, 22, 171, 87, 243, 211, 136,
		187, 120, 93, 69, 101, 159, 226, 249, 208, 57,
		115, 11, 74, 25, 97, 124, 165, 47, 131, 226,
		236, 182, 132, 228, 94, 23, 69, 235, 215, 130,
		235, 2, 64, 107, 183, 52, 232, 105, 127, 149,
		82, 99, 181, 246, 233, 231, 217, 13, 92, 250,
		11, 134, 127, 220, 117, 237, 72, 42, 142, 152,
		144, 143, 75, 46, 236, 153, 12, 245, 2, 164,
		143, 81, 199, 88, 233, 195, 86, 95, 188, 64,
		118, 122, 254, 4, 65, 175, 41, 115, 115, 182,
		94, 73, 89, 141, 212, 113, 135
	};

	private static bool? zxvjl;

	private static bool pqimp;

	private static readonly byte[] orcjj = new byte[245]
	{
		48, 129, 242, 48, 129, 169, 6, 7, 42, 134,
		72, 206, 56, 4, 1, 48, 129, 157, 2, 65,
		0, 241, 27, 64, 170, 247, 74, 181, 159, 0,
		157, 221, 46, 202, 91, 128, 1, 30, 136, 76,
		192, 114, 22, 67, 207, 128, 20, 213, 195, 78,
		164, 165, 181, 177, 91, 84, 180, 99, 62, 170,
		85, 130, 211, 207, 209, 217, 227, 67, 91, 121,
		48, 99, 246, 13, 254, 119, 205, 187, 21, 121,
		69, 149, 156, 99, 83, 2, 21, 0, 202, 30,
		220, 109, 113, 167, 250, 226, 235, 179, 53, 26,
		133, 222, 141, 212, 64, 147, 202, 17, 2, 65,
		0, 188, 3, 48, 137, 139, 191, 134, 72, 224,
		51, 72, 106, 13, 123, 89, 192, 158, 127, 18,
		65, 27, 213, 242, 151, 112, 47, 108, 158, 2,
		233, 181, 106, 251, 59, 2, 60, 48, 238, 164,
		69, 247, 71, 231, 212, 217, 174, 202, 55, 16,
		9, 205, 227, 12, 42, 196, 211, 244, 142, 77,
		5, 165, 210, 112, 79, 3, 68, 0, 2, 65,
		0, 211, 164, 192, 85, 70, 35, 103, 52, 209,
		40, 218, 67, 176, 165, 69, 208, 239, 80, 178,
		78, 29, 206, 26, 13, 149, 67, 191, 237, 32,
		58, 48, 34, 20, 31, 0, 227, 113, 253, 42,
		185, 113, 25, 180, 129, 130, 13, 112, 176, 23,
		213, 171, 56, 148, 39, 104, 175, 15, 220, 249,
		197, 219, 76, 195, 59
	};

	private qpehr(AsymmetricKeyAlgorithmId algorithmId, int keySize)
	{
		qfdmb = algorithmId;
		cznbp = keySize;
	}

	private static bool oggnh()
	{
		lock (dijcr)
		{
			bool flag;
			try
			{
				flag = RSACryptoServiceProvider.UseMachineKeyStore;
			}
			catch (PlatformNotSupportedException)
			{
				flag = false;
			}
			if (jbynz.HasValue && 0 == 0 && hfkhi != flag)
			{
				jbynz = null;
			}
			if (!jbynz.HasValue || 1 == 0)
			{
				elzlx elzlx2 = new elzlx();
				hfnnn.qnzgo(elzlx2, llisl);
				try
				{
					RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
					try
					{
						rSACryptoServiceProvider.ImportParameters(elzlx2.wtkfr());
					}
					finally
					{
						if (rSACryptoServiceProvider != null && 0 == 0)
						{
							((IDisposable)rSACryptoServiceProvider).Dispose();
						}
					}
					jbynz = true;
				}
				catch
				{
					jbynz = false;
				}
				finally
				{
					hfkhi = flag;
				}
			}
			return jbynz.Value;
		}
	}

	private static bool exupr()
	{
		lock (dijcr)
		{
			if ((!dahxy.hdfhq || 1 == 0) && dahxy.gtgwr && 0 == 0)
			{
				zxvjl = false;
				return false;
			}
			bool useMachineKeyStore = DSACryptoServiceProvider.UseMachineKeyStore;
			if (zxvjl.HasValue && 0 == 0 && pqimp != useMachineKeyStore)
			{
				zxvjl = null;
			}
			if (!zxvjl.HasValue || 1 == 0)
			{
				PublicKeyInfo publicKeyInfo = new PublicKeyInfo();
				hfnnn.qnzgo(publicKeyInfo, orcjj);
				try
				{
					DSACryptoServiceProvider dSACryptoServiceProvider = new DSACryptoServiceProvider();
					try
					{
						dSACryptoServiceProvider.ImportParameters(publicKeyInfo.GetDSAParameters());
					}
					finally
					{
						if (dSACryptoServiceProvider != null && 0 == 0)
						{
							((IDisposable)dSACryptoServiceProvider).Dispose();
						}
					}
					zxvjl = true;
				}
				catch
				{
					zxvjl = false;
				}
				finally
				{
					pqimp = useMachineKeyStore;
				}
			}
			return zxvjl.Value;
		}
	}

	public static qpehr jsbgt(string p0)
	{
		if (!bpkgq.guhie(p0, out var p1, out var p2) || 1 == 0)
		{
			return null;
		}
		switch (p1)
		{
		case AsymmetricKeyAlgorithmId.RSA:
			if (!oggnh() || 1 == 0)
			{
				return null;
			}
			break;
		case AsymmetricKeyAlgorithmId.DSA:
			if (!exupr() || 1 == 0)
			{
				return null;
			}
			break;
		default:
			return null;
		}
		if (!apkxp(p1, p2) || 1 == 0)
		{
			return null;
		}
		return new qpehr(p1, p2);
	}

	private static bool apkxp(AsymmetricKeyAlgorithmId p0, int p1)
	{
		switch (p0)
		{
		case AsymmetricKeyAlgorithmId.RSA:
		{
			int num = ((dahxy.kfygb() ? true : false) ? 1024 : 512);
			if (p1 >= num && p1 <= 4096 && ((p1 & 0x3F) == 0 || 1 == 0))
			{
				return true;
			}
			break;
		}
		case AsymmetricKeyAlgorithmId.DSA:
			if (dahxy.lehmf && 0 == 0 && (!dahxy.hdfhq || 1 == 0) && p1 != 1024)
			{
				return false;
			}
			if (p1 >= 512 && p1 <= 1024 && ((p1 & 0x3F) == 0 || 1 == 0))
			{
				return true;
			}
			break;
		}
		return false;
	}

	private eatps ukswj(PrivateKeyInfo p0)
	{
		return kdlln(p0);
	}

	eatps imfrk.xaunu(PrivateKeyInfo p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ukswj
		return this.ukswj(p0);
	}

	internal mdxtm kdlln(PrivateKeyInfo p0)
	{
		switch (qfdmb)
		{
		case AsymmetricKeyAlgorithmId.RSA:
		{
			RSAParameters rSAParameters = p0.GetRSAParameters();
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(rSAParameters);
			return new mdxtm(rSACryptoServiceProvider, ownsAlgorithm: true);
		}
		case AsymmetricKeyAlgorithmId.DSA:
		{
			DSAParameters dSAParameters = p0.GetDSAParameters();
			DSACryptoServiceProvider dSACryptoServiceProvider = new DSACryptoServiceProvider();
			dSACryptoServiceProvider.ImportParameters(dSAParameters);
			return new mdxtm(dSACryptoServiceProvider, ownsAlgorithm: true);
		}
		default:
			throw new CryptographicException("Unsupported key algorithm.");
		}
	}

	private eatps xqyeh(PublicKeyInfo p0)
	{
		return orngu(p0);
	}

	eatps imfrk.neqkn(PublicKeyInfo p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xqyeh
		return this.xqyeh(p0);
	}

	internal mdxtm orngu(PublicKeyInfo p0)
	{
		switch (qfdmb)
		{
		case AsymmetricKeyAlgorithmId.RSA:
		{
			RSAParameters rSAParameters = p0.GetRSAParameters();
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(rSAParameters);
			return new mdxtm(rSACryptoServiceProvider, ownsAlgorithm: true);
		}
		case AsymmetricKeyAlgorithmId.DSA:
		{
			DSAParameters dSAParameters = p0.GetDSAParameters();
			DSACryptoServiceProvider dSACryptoServiceProvider = new DSACryptoServiceProvider();
			dSACryptoServiceProvider.ImportParameters(dSAParameters);
			return new mdxtm(dSACryptoServiceProvider, ownsAlgorithm: true);
		}
		default:
			throw new CryptographicException("Unsupported key algorithm.");
		}
	}

	private eatps cirpc()
	{
		return jkeyc();
	}

	eatps imfrk.poerm()
	{
		//ILSpy generated this explicit interface implementation from .override directive in cirpc
		return this.cirpc();
	}

	internal mdxtm jkeyc()
	{
		switch (qfdmb)
		{
		case AsymmetricKeyAlgorithmId.RSA:
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(cznbp);
			rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
			return new mdxtm(rSACryptoServiceProvider, ownsAlgorithm: true);
		}
		case AsymmetricKeyAlgorithmId.DSA:
		{
			DSACryptoServiceProvider dSACryptoServiceProvider = new DSACryptoServiceProvider(cznbp);
			dSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
			return new mdxtm(dSACryptoServiceProvider, ownsAlgorithm: true);
		}
		default:
			throw new CryptographicException("Unsupported key algorithm.");
		}
	}
}
