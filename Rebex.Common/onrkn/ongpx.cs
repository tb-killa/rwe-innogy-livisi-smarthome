using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Rebex;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal static class ongpx
{
	private const byte dyoqx = 65;

	private const int uoson = 32;

	private const string pgibj = "Your trial version is too old. Please contact support@rebex.net to get a suitable trial key.";

	private static Dictionary<string, byte[]> zkuhz = new Dictionary<string, byte[]>();

	private static List<byte[]> yrqmg = new List<byte[]>();

	private static readonly Dictionary<string, byte[]> ofbcd = new Dictionary<string, byte[]>(StringComparer.Ordinal);

	internal static fwwdw plgbf(string p0, string p1)
	{
		lock (zkuhz)
		{
			byte[] p2;
			fwwdw fwwdw2 = iuvuf(p1, out p2);
			if (fwwdw2 != null && 0 == 0)
			{
				return fwwdw2;
			}
			if (zkuhz.TryGetValue(p0, out var value) && 0 == 0)
			{
				zkuhz[p0] = p2;
				yrqmg.Remove(value);
			}
			else
			{
				zkuhz.Add(p0, p2);
			}
			yrqmg.Add(p2);
			return null;
		}
	}

	internal static void foqfm(string p0)
	{
		lock (zkuhz)
		{
			if (zkuhz.TryGetValue(p0, out var value) && 0 == 0)
			{
				zkuhz.Remove(p0);
				yrqmg.Remove(value);
			}
		}
	}

	private static string vvvpc(Assembly p0)
	{
		if ((object)p0 == null || 1 == 0)
		{
			return null;
		}
		AssemblyName name = p0.GetName();
		if (name == null || 1 == 0)
		{
			return null;
		}
		return name.CodeBase;
	}

	private static byte[] xbbww(string p0)
	{
		try
		{
			FileStream fileStream = vtdxm.vswch(p0, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				byte[] array = new byte[33];
				array[0] = 65;
				fileStream.Seek(-32L, SeekOrigin.End);
				fileStream.Read(array, 1, 32);
				return array;
			}
			finally
			{
				if (fileStream != null && 0 == 0)
				{
					((IDisposable)fileStream).Dispose();
				}
			}
		}
		catch (ArgumentException)
		{
			return null;
		}
		catch (IOException)
		{
			return null;
		}
		catch (UnauthorizedAccessException)
		{
			return null;
		}
	}

	private static fwwdw kyqfn(string p0, string p1, int p2)
	{
		if ((!string.IsNullOrEmpty(p0) || 1 == 0) && File.Exists(p0) && 0 == 0)
		{
			byte[] array = xbbww(p0);
			if (array != null && 0 == 0)
			{
				return lhypl(array, p1, p2);
			}
		}
		return fwwdw.exkbn(p1);
	}

	private static fwwdw iuvuf(string p0, out byte[] p1)
	{
		byte[] array = new byte[1] { 35 };
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			p1 = null;
			return new fwwdw("License key is empty.");
		}
		p0 = p0.Trim('=').Trim();
		if (p0.Length == 0 || 1 == 0)
		{
			p1 = array;
			return null;
		}
		byte b = (byte)Math.Max(0, Math.Min(255, (int)p0[0]));
		switch (b)
		{
		default:
			p1 = null;
			return new fwwdw("License key is unknown.");
		case 80:
			p1 = array;
			return null;
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
		case 78:
		case 79:
		case 81:
		case 82:
		case 83:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 89:
		case 90:
		{
			p0 = p0.Substring(1);
			int totalWidth = ((p0.Length + 3) | 3) - 3;
			p0 = p0.PadRight(totalWidth, '=');
			byte[] array2;
			try
			{
				array2 = Convert.FromBase64String(p0);
			}
			catch (FormatException)
			{
				p1 = null;
				return new fwwdw("License key format is invalid.");
			}
			p1 = new byte[array2.Length + 1];
			p1[0] = b;
			array2.CopyTo(p1, 1);
			return null;
		}
		}
	}

	private static byte[] ryyvf(byte[] p0)
	{
		string key = Convert.ToBase64String(p0);
		byte[] value;
		lock (ofbcd)
		{
			if (ofbcd.TryGetValue(key, out value) && 0 == 0)
			{
				return value;
			}
		}
		RSAParameters parameters = new RSAParameters
		{
			Modulus = new byte[32]
			{
				182, 28, 156, 84, 172, 217, 205, 220, 65, 15,
				164, 101, 241, 26, 18, 244, 193, 44, 214, 186,
				203, 99, 154, 131, 134, 199, 190, 107, 233, 99,
				126, 147
			},
			Exponent = new byte[3] { 1, 0, 1 }
		};
		RSAManaged rSAManaged = new RSAManaged(skipFipsCheck: true);
		rSAManaged.ImportParameters(parameters);
		byte[] array = new byte[p0.Length - 1];
		Array.Copy(p0, 1, array, 0, array.Length);
		try
		{
			value = rSAManaged.EncryptValue(array);
			value = RSAManaged.ffitw(value, 7, p2: true);
		}
		catch (CryptographicException)
		{
			value = null;
		}
		lock (ofbcd)
		{
			ofbcd[key] = value;
			return value;
		}
	}

	private static void udndm(byte[] p0, out DateTime p1, out DateTime p2, out int p3, out int p4)
	{
		int num = p0[0] * 256 + p0[1];
		p3 = p0[2] * 256 + p0[3];
		p4 = p0[4];
		p2 = new DateTime(2010, 12, 31).AddDays(num);
		p1 = p2.AddDays(-64.0);
	}

	private static fwwdw lhypl(byte[] p0, string p1, int p2)
	{
		byte[] array = ryyvf(p0);
		if (array == null || 1 == 0)
		{
			return new fwwdw("Invalid license key.");
		}
		udndm(array, out var p3, out var p4, out var p5, out var p6);
		if ((p6 & 1) != 0 && 0 == 0)
		{
			return fwwdw.exkbn(p1);
		}
		if (p5 < 65535 && p2 > 0 && p2 < p5)
		{
			return new fwwdw("Your trial version is too old. Please contact support@rebex.net to get a suitable trial key.");
		}
		DateTime now = DateTime.Now;
		if (now < p3 && 0 == 0)
		{
			return new fwwdw("System time is wrong, unable to check the trial license key.");
		}
		if (now > p4 && 0 == 0)
		{
			return fwwdw.qgara(p1);
		}
		return null;
	}

	private static fwwdw ysycu(byte[] p0, ivdov p1, string p2, int p3)
	{
		if (p0.Length < 45)
		{
			return new fwwdw("License key is corrupted.");
		}
		byte[] array = new byte[40];
		Array.Copy(p0, 1, array, 0, 40);
		byte[] array2 = new byte[p0.Length - 41];
		int num = 0;
		if (num != 0)
		{
			goto IL_0039;
		}
		goto IL_0050;
		IL_0169:
		wmbjj wmbjj2;
		int p4 = wmbjj2.nnram();
		int num2 = wmbjj2.fhuaz();
		int num3 = wmbjj2.mytfp();
		bool flag;
		DateTime utcNow;
		bool flag2;
		if (p1 != 0 && 0 == 0)
		{
			ivdov ivdov2 = ikpkg.qecvn((nbmcv)p4);
			if ((ivdov2 & p1) == p1)
			{
				switch (num2)
				{
				case 1:
					if (p3 <= num3)
					{
						return null;
					}
					flag = true;
					if (flag)
					{
						break;
					}
					goto case 2;
				case 2:
				{
					DateTime dateTime = new DateTime(2010, 12, 31, 0, 0, 0, DateTimeKind.Utc).AddDays(num3);
					if (utcNow <= dateTime && 0 == 0)
					{
						return null;
					}
					flag2 = true;
					break;
				}
				}
			}
		}
		goto IL_01fe;
		IL_0039:
		array2[num] = (byte)(p0[num + 41] ^ p0[num % 40 + 1]);
		num++;
		goto IL_0050;
		IL_0050:
		if (num < array2.Length)
		{
			goto IL_0039;
		}
		utcNow = DateTime.UtcNow;
		string s = "-----BEGIN PUBLIC KEY-----\r\nMIIBtzCCASsGByqGSM44BAEwggEeAoGBAMyn0CrBOcXnBfDqgbxcZaXMhwmiYvbv\r\ndyAlFmtjVIZMMHLotW7GX7rhC+iApKb22N0STewEpng+nvuZ/36gaAhT49RJ9zmM\r\nJ4Pqucw8QKzwYeqgloUkpwMRV/2TEsw6cJ7LGed9FwomvO/POdLNO2GBUk0/iol5\r\n/IdRRlWIZD2NAhUAwIyx9VZV0hnBnW20bMghBAz3WnkCgYAjiwdms1J3QQnm/kbn\r\nkv9gEtQGfc3EWIPqla8NrH5L3Uy+HkxU4YqjlN6bTDcmfUmQqqq7AT2LO2FswGiV\r\nIyH0mKQA8MKbV3JCN44zgn1kccuo07UPVEdy+c1sf4yhvqYj2AgNoojONcgjHUP8\r\nsft+gisdwmXsX9ExeujZoJF0CgOBhQACgYEAk0eKBhWKTbKUaXzNti+qdw/BK/jv\r\nvs1syvuXH4m7BaqynR2TRL/zwlKATdOaZ0qxlS/kMuw+do3PzUehJQn9Fl57mQsl\r\n7f+2D9T4sWAg90WHZ36WrQWcffIUl60z9FdSoLRoH2IrbpQdF0AYbHy5w7gUcwhe\r\n5nbqzXAnQQgsYz4=\r\n-----END PUBLIC KEY-----\r\n";
		PublicKeyInfo publicKeyInfo = new PublicKeyInfo();
		MemoryStream memoryStream = new MemoryStream(EncodingTools.ASCII.GetBytes(s));
		try
		{
			publicKeyInfo.Load(memoryStream);
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
		IDisposable disposable = null;
		dzjkq dzjkq2 = null;
		imfrk imfrk2 = rmnyn.aregb("dsa1024");
		if (imfrk2 != null && 0 == 0)
		{
			eatps eatps2 = imfrk2.neqkn(publicKeyInfo);
			dzjkq2 = eatps2 as dzjkq;
			disposable = eatps2 as IDisposable;
		}
		try
		{
			if (dzjkq2 == null || 1 == 0)
			{
				DSAManaged dSAManaged = new DSAManaged(skipFipsCheck: true);
				DSAParameters dSAParameters = publicKeyInfo.GetDSAParameters();
				dSAManaged.ImportParameters(dSAParameters);
				dzjkq2 = dSAManaged;
			}
			byte[] p5 = HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, array2);
			if (!dzjkq2.cbzmp(p5, array, mrxvh.vtcca(SignatureHashAlgorithm.SHA1, AsymmetricKeyAlgorithmId.DSA)) || 1 == 0)
			{
				return new fwwdw("License key is not valid.");
			}
		}
		finally
		{
			if (disposable != null && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		wmbjj2 = new wmbjj(array2);
		wmbjj2.jvrae();
		flag = false;
		flag2 = false;
		if (flag2)
		{
			goto IL_0169;
		}
		goto IL_01fe;
		IL_01fe:
		if (wmbjj2.hpxkw >= wmbjj2.tdjyr)
		{
			if (flag && 0 == 0)
			{
				return new fwwdw("License key for " + p2 + " has not been renewed. See https://www.rebex.net/kb/license-keys/ for more information.");
			}
			if (flag2 && 0 == 0)
			{
				return new fwwdw("License key for " + p2 + " has expired. See https://www.rebex.net/kb/license-keys/ for more information.");
			}
			return new fwwdw("License key for " + p2 + " not available. See https://www.rebex.net/kb/license-keys/ for more information.");
		}
		goto IL_0169;
	}

	internal static qacxy bjvdq(rsljk p0)
	{
		_ = p0.hfdnx;
		string xarpt = p0.xarpt;
		int cilda = p0.cilda;
		if (p0.vfpmi == cltxh.azefz || 1 == 0)
		{
			return qacxy.dhiwg;
		}
		lock (zkuhz)
		{
			fwwdw fwwdw2 = null;
			fwwdw fwwdw3 = null;
			using (List<byte[]>.Enumerator enumerator = yrqmg.GetEnumerator())
			{
				while (enumerator.MoveNext() ? true : false)
				{
					byte[] current = enumerator.Current;
					switch ((char)current[0])
					{
					case '#':
						if (fwwdw3 == null || 1 == 0)
						{
							fwwdw3 = fwwdw.exkbn(xarpt);
						}
						break;
					case 'A':
					case 'T':
						fwwdw3 = lhypl(current, xarpt, cilda);
						if (fwwdw3 == null || 1 == 0)
						{
							return qacxy.nabbw;
						}
						break;
					case 'F':
						throw new fwwdw("Full license key is not applicable to " + xarpt + " legacy trial binaries. Contact support@rebex.net for asistance.");
					}
				}
			}
			if (p0.vfpmi != cltxh.nvuyk && (!dahxy.hdfhq || 1 == 0))
			{
				Assembly dmses = p0.dmses;
				if ((object)dmses != null && 0 == 0)
				{
					AssemblyName name = dmses.GetName();
					if (name != null && 0 == 0 && name.Name == xarpt && 0 == 0)
					{
						string p1 = vvvpc(dmses);
						fwwdw fwwdw4 = kyqfn(p1, xarpt, cilda);
						if (fwwdw4 == null || 1 == 0)
						{
							return qacxy.nabbw;
						}
						fwwdw obj = fwwdw3;
						if (obj == null || 1 == 0)
						{
							obj = fwwdw4;
						}
						fwwdw3 = obj;
					}
				}
			}
			fwwdw obj2 = fwwdw2;
			if (obj2 == null || 1 == 0)
			{
				obj2 = fwwdw3;
			}
			fwwdw fwwdw5 = obj2;
			if (fwwdw5 == null || 1 == 0)
			{
				fwwdw5 = fwwdw.exkbn(xarpt);
			}
			throw fwwdw5;
		}
	}
}
