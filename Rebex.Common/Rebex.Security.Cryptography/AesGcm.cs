using System;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class AesGcm : IDisposable
{
	private const int cthdb = 16;

	private const int iqjac = 12;

	private const int urcuv = 16;

	private const int btrqn = 12;

	private const int wkvzy = 12;

	private const string fmpij = "The tag size is invalid.";

	private static readonly byte[] hmdyo = new byte[0];

	private readonly byte[] dmkpc;

	private bool nnfdv;

	private fhryo dcvpv;

	private int xfawh;

	private gajry opvnh;

	private int gofwo;

	public static readonly KeySizes KeyByteSizes = new KeySizes(16, 32, 8);

	public static readonly KeySizes TagByteSizes = new KeySizes(12, 16, 1);

	public static readonly KeySizes NonceByteSizes = new KeySizes(12, 12, 1);

	public AesGcm(byte[] key)
	{
		if (key == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (!CryptoHelper.egtui(KeyByteSizes, key.Length) || 1 == 0)
		{
			throw new ArgumentException("The key size is invalid.", "key");
		}
		dmkpc = new byte[key.Length];
		Array.Copy(key, 0, dmkpc, 0, key.Length);
		xfawh = -1;
		gofwo = -1;
	}

	public void Encrypt(byte[] nonce, byte[] plaintext, byte[] ciphertext, byte[] authTag, byte[] additionalAuthData = null)
	{
		zqovf();
		vetlv(nonce, plaintext, ciphertext);
		if (authTag == null || 1 == 0)
		{
			throw new ArgumentNullException("authTag");
		}
		fhryo fhryo = dcvpv;
		int num = authTag.Length;
		if (num != xfawh)
		{
			if (num < 12 || num > 16)
			{
				throw new ArgumentException("The tag size is invalid.", "authTag");
			}
			if (fhryo != null && 0 == 0)
			{
				fhryo.Dispose();
			}
			fhryo = (dcvpv = wfcez.stfbw(dmkpc, 16, authTag.Length));
			xfawh = num;
		}
		fhryo.yirig(nonce);
		fhryo fhryo2 = fhryo;
		byte[] array = additionalAuthData;
		if (array == null || 1 == 0)
		{
			array = hmdyo;
		}
		fhryo2.seoke(array);
		int num2 = fhryo.yajzn(plaintext, 0, plaintext.Length, ciphertext, 0);
		if (num2 != plaintext.Length)
		{
			throw new CryptographicException("Encryption failed.");
		}
		fhryo.mdexb(authTag, 0);
	}

	public void Decrypt(byte[] nonce, byte[] ciphertext, byte[] authTag, byte[] plaintext, byte[] additionalAuthData = null)
	{
		zqovf();
		vetlv(nonce, plaintext, ciphertext);
		if (authTag == null || 1 == 0)
		{
			throw new ArgumentNullException("authTag");
		}
		gajry gajry = opvnh;
		int num = authTag.Length;
		if (num != gofwo)
		{
			if (num < 12 || num > 16)
			{
				throw new ArgumentException("The tag size is invalid.", "authTag");
			}
			if (gajry != null && 0 == 0)
			{
				gajry.Dispose();
			}
			gajry = (opvnh = wfcez.usflo(dmkpc, 16, authTag.Length));
			gofwo = num;
		}
		gajry.yirig(nonce);
		gajry.tglzc(authTag, 0, authTag.Length);
		gajry gajry2 = gajry;
		byte[] array = additionalAuthData;
		if (array == null || 1 == 0)
		{
			array = hmdyo;
		}
		gajry2.seoke(array);
		try
		{
			int num2 = gajry.yajzn(ciphertext, 0, ciphertext.Length, plaintext, 0);
			if (num2 != ciphertext.Length)
			{
				throw new CryptographicException("Decryption failed.");
			}
		}
		catch (CryptographicException ex)
		{
			if (ex.Message.aptsd("Authentication tag check failed.", StringComparison.Ordinal) && 0 == 0)
			{
				Array.Clear(plaintext, 0, plaintext.Length);
			}
			throw;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (nnfdv)
		{
			return;
		}
		nnfdv = true;
		if (disposing && 0 == 0)
		{
			Array.Clear(dmkpc, 0, dmkpc.Length);
			fhryo fhryo = dcvpv;
			if (fhryo != null && 0 == 0)
			{
				fhryo.Dispose();
			}
			gajry gajry = opvnh;
			if (gajry != null && 0 == 0)
			{
				gajry.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	private void zqovf()
	{
		if (nnfdv && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
	}

	private static void vetlv(byte[] p0, byte[] p1, byte[] p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("nonce");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("plaintext");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("ciphertext");
		}
		int num = p0.Length;
		if (num < 12 || num > 12)
		{
			throw new ArgumentException("The nonce size is invalid.", "nonce");
		}
		if (p1.Length == p2.Length)
		{
			return;
		}
		throw new ArgumentException("The plaintext and ciphertext arrays have to be of the same length.");
	}
}
