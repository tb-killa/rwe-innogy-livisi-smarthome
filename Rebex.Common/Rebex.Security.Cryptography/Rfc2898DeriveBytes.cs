using System;
using System.Text;

namespace Rebex.Security.Cryptography;

public class Rfc2898DeriveBytes : DeriveBytes
{
	private KeyedHashAlgorithm fdhdr;

	private int bghir;

	private int cjyup;

	private byte[] gmppe;

	private uint momzd;

	private int kziix;

	private int upyzw;

	private byte[] ogaol;

	public int IterationCount
	{
		get
		{
			return cjyup;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			cjyup = value;
			yfqdt();
		}
	}

	public byte[] Salt
	{
		get
		{
			return (byte[])gmppe.Clone();
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length < 8)
			{
				throw new ArgumentException("Not enough data.", "value");
			}
			gmppe = (byte[])value.Clone();
			yfqdt();
		}
	}

	public Rfc2898DeriveBytes(string password, int saltSize)
		: this(password, saltSize, 1000)
	{
	}

	public Rfc2898DeriveBytes(string password, byte[] salt)
		: this(password, salt, 1000)
	{
	}

	public Rfc2898DeriveBytes(string password, int saltSize, int iterations)
		: this(HashingAlgorithmId.SHA1, password, saltSize, iterations)
	{
	}

	internal Rfc2898DeriveBytes(HashingAlgorithmId hashingAlgorithmId, string password, int saltSize, int iterations)
	{
		if (saltSize < 0)
		{
			throw new ArgumentOutOfRangeException("saltSize");
		}
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		byte[] array = new byte[saltSize];
		CryptoHelper.CreateRandomNumberGenerator().GetBytes(array);
		Salt = array;
		IterationCount = iterations;
		IHashTransform hashTransform = new HashingAlgorithm(hashingAlgorithmId).CreateTransform();
		fdhdr = new HMAC(hashTransform, Encoding.UTF8.GetBytes(password));
		bghir = hashTransform.HashSize / 8;
		yfqdt();
	}

	public Rfc2898DeriveBytes(string password, byte[] salt, int iterations)
		: this(HashingAlgorithmId.SHA1, Encoding.UTF8.GetBytes(password), salt, iterations)
	{
	}

	internal Rfc2898DeriveBytes(HashingAlgorithmId hashingAlgorithmId, string password, byte[] salt, int iterations)
		: this(hashingAlgorithmId, Encoding.UTF8.GetBytes(password), salt, iterations)
	{
	}

	public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations)
		: this(HashingAlgorithmId.SHA1, password, salt, iterations)
	{
	}

	internal Rfc2898DeriveBytes(HashingAlgorithmId hashingAlgorithmId, byte[] password, byte[] salt, int iterations)
	{
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		Salt = salt;
		IterationCount = iterations;
		IHashTransform hashTransform = new HashingAlgorithm(hashingAlgorithmId).CreateTransform();
		fdhdr = new HMAC(hashTransform, password);
		bghir = hashTransform.HashSize / 8;
		yfqdt();
	}

	private byte[] mkmmd()
	{
		byte[] bytes = BitConverter.GetBytes(momzd);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(bytes, 0, bytes.Length);
		}
		fdhdr.TransformBlock(gmppe, 0, gmppe.Length, gmppe, 0);
		fdhdr.TransformFinalBlock(bytes, 0, bytes.Length);
		byte[] array = fdhdr.Hash;
		fdhdr.Initialize();
		byte[] array2 = array;
		int num = 2;
		if (num == 0)
		{
			goto IL_007a;
		}
		goto IL_00b0;
		IL_00a2:
		int num2;
		if (num2 < bghir)
		{
			goto IL_008e;
		}
		num++;
		goto IL_00b0;
		IL_00b0:
		if (num > cjyup)
		{
			momzd++;
			return array2;
		}
		goto IL_007a;
		IL_008e:
		array2[num2] ^= array[num2];
		num2++;
		goto IL_00a2;
		IL_007a:
		array = fdhdr.ComputeHash(array);
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_008e;
		}
		goto IL_00a2;
	}

	private void yfqdt()
	{
		if (ogaol != null && 0 == 0)
		{
			Array.Clear(ogaol, 0, ogaol.Length);
		}
		ogaol = new byte[bghir];
		momzd = 1u;
		kziix = 0;
		upyzw = 0;
	}

	public override void Reset()
	{
		yfqdt();
	}

	public override byte[] GetBytes(int cb)
	{
		if (cb <= 0)
		{
			throw new ArgumentOutOfRangeException("cb");
		}
		byte[] array = new byte[cb];
		int num = upyzw - kziix;
		int i = 0;
		if (num > 0)
		{
			if (cb < num)
			{
				Array.Copy(ogaol, kziix, array, 0, cb);
				kziix += cb;
				return array;
			}
			Array.Copy(ogaol, kziix, array, 0, num);
			i += num;
			kziix = 0;
			upyzw = 0;
		}
		for (; i < cb; i += bghir)
		{
			byte[] sourceArray = mkmmd();
			num = cb - i;
			if (num <= bghir)
			{
				Array.Copy(sourceArray, 0, array, i, num);
				Array.Copy(sourceArray, num, ogaol, kziix, bghir - num);
				upyzw = upyzw + bghir - num;
				return array;
			}
			Array.Copy(sourceArray, 0, array, i, bghir);
		}
		return array;
	}
}
