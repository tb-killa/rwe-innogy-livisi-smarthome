using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using onrkn;

namespace Rebex.Security.Cryptography;

public class Pkcs12KeyGenerator : DeriveBytes
{
	public const int KeyMaterial = 1;

	public const int IVMaterial = 2;

	public const int MacMaterial = 3;

	private IHashTransform zvxia;

	private byte[] exnrj;

	private int dzjuo;

	private byte[] radjy;

	private int ddcke;

	private byte[] hojcm;

	private byte[] fiarf;

	private byte hqjzu;

	private int zbhiz;

	private int usyjo;

	private byte[] fdttg;

	[Obsolete("This constructor has been deprecated. Please use Pkcs12KeyGenerator(HashingAlgorithmId, string, byte[], int, int) instead.", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Pkcs12KeyGenerator(Type alg, string password, byte[] salt, int iterations, int id)
	{
		if ((object)alg == null || 1 == 0)
		{
			throw new ArgumentNullException("alg");
		}
		if (!alg.IsSubclassOf(typeof(HashAlgorithm)) || 1 == 0)
		{
			throw new CryptographicException("The specified type is not a HashAlgorithm.");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		if (salt == null || 1 == 0)
		{
			throw new ArgumentNullException("salt");
		}
		zvxia = HashingAlgorithm.ytacd((HashAlgorithm)Activator.CreateInstance(alg));
		ktlgg(password, salt, iterations, id);
	}

	public Pkcs12KeyGenerator(HashingAlgorithmId algorithm, string password, byte[] salt, int iterations, int id)
	{
		zvxia = new HashingAlgorithm(algorithm).CreateTransform();
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		if (salt == null || 1 == 0)
		{
			throw new ArgumentNullException("salt");
		}
		ktlgg(password, salt, iterations, id);
	}

	private void ktlgg(string p0, byte[] p1, int p2, int p3)
	{
		ddcke = 64;
		hqjzu = (byte)p3;
		radjy = Encoding.BigEndianUnicode.GetBytes(p0 + "\0");
		exnrj = p1;
		dzjuo = p2;
		zwvbd();
	}

	private void zwvbd()
	{
		hojcm = new byte[ddcke];
		int num = 0;
		if (num != 0)
		{
			goto IL_0016;
		}
		goto IL_0028;
		IL_0016:
		hojcm[num] = hqjzu;
		num++;
		goto IL_0028;
		IL_0028:
		if (num >= hojcm.Length)
		{
			byte[] array = aulfa(exnrj);
			byte[] array2 = aulfa(radjy);
			fiarf = new byte[array.Length + array2.Length];
			array.CopyTo(fiarf, 0);
			array2.CopyTo(fiarf, array.Length);
			if (fdttg != null && 0 == 0)
			{
				Array.Clear(fdttg, 0, fdttg.Length);
			}
			fdttg = new byte[hojcm.Length + fiarf.Length];
			zbhiz = 0;
			usyjo = 0;
			return;
		}
		goto IL_0016;
	}

	private byte[] aulfa(byte[] p0)
	{
		if (p0 == null || false || p0.Length == 0 || 1 == 0)
		{
			return new byte[0];
		}
		byte[] array = new byte[ddcke + ddcke * ((p0.Length - 1) / ddcke)];
		int num = 0;
		if (num != 0)
		{
			goto IL_0047;
		}
		goto IL_0063;
		IL_0063:
		if (num < array.Length)
		{
			goto IL_0047;
		}
		return array;
		IL_0047:
		Array.Copy(p0, 0, array, num, Math.Min(p0.Length, array.Length - num));
		num += p0.Length;
		goto IL_0063;
	}

	private void gjdkj(byte[] p0, int p1, byte[] p2)
	{
		int num = p2.Length - 1;
		int num2 = p2[num] + p0[p1 + num] + 1;
		p0[p1 + num] = (byte)(num2 & 0xFF);
		while (num > 0)
		{
			num--;
			num2 >>= 8;
			num2 += p2[num] + p0[p1 + num];
			p0[p1 + num] = (byte)(num2 & 0xFF);
		}
	}

	private byte[] qumev()
	{
		byte[] array = new byte[hojcm.Length + fiarf.Length];
		hojcm.CopyTo(array, 0);
		fiarf.CopyTo(array, hojcm.Length);
		int num = 0;
		if (num != 0)
		{
			goto IL_0041;
		}
		goto IL_006c;
		IL_0041:
		zvxia.Process(array, 0, array.Length);
		array = zvxia.GetHash();
		zvxia.Reset();
		num++;
		goto IL_006c;
		IL_006c:
		if (num < dzjuo)
		{
			goto IL_0041;
		}
		byte[] p = aulfa(array);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0082;
		}
		goto IL_0099;
		IL_0099:
		if (num2 < fiarf.Length)
		{
			goto IL_0082;
		}
		return array;
		IL_0082:
		gjdkj(fiarf, num2, p);
		num2 += ddcke;
		goto IL_0099;
	}

	public override void Reset()
	{
		zwvbd();
	}

	public override byte[] GetBytes(int cb)
	{
		if (cb <= 0)
		{
			throw new ArgumentOutOfRangeException("cb");
		}
		byte[] array = new byte[cb];
		int num = usyjo - zbhiz;
		int i = 0;
		if (num > 0)
		{
			if (cb < num)
			{
				Array.Copy(fdttg, zbhiz, array, 0, cb);
				zbhiz += cb;
				return array;
			}
			Array.Copy(fdttg, zbhiz, array, 0, num);
			i += num;
			zbhiz = 0;
			usyjo = 0;
		}
		for (; i < cb; i += 20)
		{
			byte[] sourceArray = qumev();
			num = cb - i;
			if (num <= 20)
			{
				Array.Copy(sourceArray, 0, array, i, num);
				i += num;
				Array.Copy(sourceArray, num, fdttg, zbhiz, 20 - num);
				usyjo = usyjo + 20 - num;
				return array;
			}
			Array.Copy(sourceArray, 0, array, i, 20);
		}
		return array;
	}
}
