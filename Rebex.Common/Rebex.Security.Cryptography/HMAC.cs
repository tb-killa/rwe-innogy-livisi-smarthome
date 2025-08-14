using System;
using System.ComponentModel;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class HMAC : KeyedHashAlgorithm, IHashTransform, IDisposable
{
	private IHashTransform eseyb;

	private bool oefye;

	private int kvjhs;

	public override byte[] Key
	{
		get
		{
			return KeyValue;
		}
		set
		{
			if (oefye && 0 == 0)
			{
				throw new CryptographicException("An attempt to change Key after hashing has begun.");
			}
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length > 64)
			{
				eseyb.Process(value, 0, value.Length);
				value = eseyb.GetHash();
				eseyb.Reset();
			}
			KeyValue = (byte[])value.Clone();
		}
	}

	private int uvphx => HashSizeValue;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This constructor has been deprecated. Please use HMAC(HashingAlgorithmId, byte[]) instead.", false)]
	public HMAC(Type alg, byte[] rgbKey)
	{
		if ((object)alg == null || 1 == 0)
		{
			throw new ArgumentNullException("alg");
		}
		if (!alg.IsSubclassOf(typeof(HashAlgorithm)) || 1 == 0)
		{
			throw new CryptographicException("The specified type is not a HashAlgorithm.");
		}
		eseyb = HashingAlgorithm.ytacd((HashAlgorithm)Activator.CreateInstance(alg));
		amevc(rgbKey);
	}

	[Obsolete("This constructor has been deprecated. Please use HMAC(HashingAlgorithmId, byte[]) instead.", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public HMAC(string alg, byte[] rgbKey)
		: this(bpkgq.pdzpf(alg), rgbKey)
	{
	}

	internal HMAC(HashAlgorithm algorithm, byte[] rgbKey)
		: this(HashingAlgorithm.ytacd(algorithm), rgbKey)
	{
	}

	public HMAC(HashingAlgorithmId algorithm, byte[] rgbKey)
	{
		eseyb = new HashingAlgorithm(algorithm).CreateTransform();
		amevc(rgbKey);
	}

	public HMAC(HashingAlgorithmId algorithm)
		: this(algorithm, null)
	{
	}

	internal HMAC(IHashTransform algorithm, byte[] rgbKey)
	{
		eseyb = algorithm;
		amevc(rgbKey);
	}

	private void amevc(byte[] p0)
	{
		HashSizeValue = eseyb.HashSize;
		kvjhs = ((HashSizeValue <= 256) ? 64 : 128);
		if (p0 == null || 1 == 0)
		{
			RandomNumberGenerator randomNumberGenerator = CryptoHelper.CreateRandomNumberGenerator();
			KeyValue = new byte[64];
			randomNumberGenerator.GetBytes(KeyValue);
			return;
		}
		if (p0.Length > kvjhs)
		{
			eseyb.Process(p0, 0, p0.Length);
			p0 = eseyb.GetHash();
			eseyb.Reset();
		}
		KeyValue = (byte[])p0.Clone();
	}

	[Obsolete("This constructor has been deprecated. Please use HMAC(HashingAlgorithmId) instead.", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public HMAC(Type alg)
		: this(alg, null)
	{
	}

	[Obsolete("This constructor has been deprecated. Please use HMAC(HashingAlgorithmId) instead.", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public HMAC(string alg)
		: this(alg, null)
	{
	}

	public override void Initialize()
	{
		if (eseyb == null || 1 == 0)
		{
			throw new ObjectDisposedException("HMAC");
		}
		eseyb.Reset();
		oefye = false;
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		byte[] array2;
		int num;
		if (!oefye || 1 == 0)
		{
			oefye = true;
			array2 = new byte[kvjhs];
			Array.Copy(KeyValue, 0, array2, 0, KeyValue.Length);
			num = 0;
			if (num != 0)
			{
				goto IL_0041;
			}
			goto IL_005b;
		}
		goto IL_007d;
		IL_007d:
		eseyb.Process(array, ibStart, cbSize);
		return;
		IL_005b:
		if (num < kvjhs)
		{
			goto IL_0041;
		}
		eseyb.Process(array2, 0, kvjhs);
		goto IL_007d;
		IL_0041:
		array2[num] ^= 54;
		num++;
		goto IL_005b;
	}

	protected override byte[] HashFinal()
	{
		byte[] hash = eseyb.GetHash();
		eseyb.Reset();
		byte[] array = new byte[kvjhs];
		Array.Copy(KeyValue, 0, array, 0, KeyValue.Length);
		int num = 0;
		if (num != 0)
		{
			goto IL_0042;
		}
		goto IL_005c;
		IL_0042:
		array[num] ^= 92;
		num++;
		goto IL_005c;
		IL_005c:
		if (num >= kvjhs)
		{
			eseyb.Process(array, 0, kvjhs);
			eseyb.Process(hash, 0, hash.Length);
			hash = eseyb.GetHash();
			Array.Clear(array, 0, kvjhs);
			oefye = false;
			return hash;
		}
		goto IL_0042;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && 0 == 0)
		{
			eseyb.Dispose();
		}
		base.Dispose(disposing);
	}

	private void iasus(byte[] p0, int p1, int p2)
	{
		HashCore(p0, p1, p2);
	}

	void IHashTransform.Process(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in iasus
		this.iasus(p0, p1, p2);
	}

	private byte[] pgjwe()
	{
		return HashFinal();
	}

	byte[] IHashTransform.GetHash()
	{
		//ILSpy generated this explicit interface implementation from .override directive in pgjwe
		return this.pgjwe();
	}

	private void amxrl()
	{
		Initialize();
	}

	void IHashTransform.Reset()
	{
		//ILSpy generated this explicit interface implementation from .override directive in amxrl
		this.amxrl();
	}
}
