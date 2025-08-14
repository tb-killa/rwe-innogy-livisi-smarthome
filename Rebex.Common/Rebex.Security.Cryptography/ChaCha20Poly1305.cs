using System;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public sealed class ChaCha20Poly1305 : IDisposable
{
	public const int KeySize = 32;

	public const int NonceSize = 12;

	public const int AuthenticationTagSize = 16;

	private const string zgnyk = "The key size is invalid. Must be exactly 32 bytes.";

	private const string godyd = "The nonce size is invalid. Must be exactly 12 bytes.";

	private const string gtdby = "The authTag size is invalid. Must be exactly 16 bytes.";

	private const string fxazk = "The plaintext and ciphertext arrays have to be of the same length.";

	private byte[] arulw;

	private nvqxb dumma;

	private fmjea iwslm;

	private bool vcvmn;

	public ChaCha20Poly1305(byte[] key)
	{
		if (key == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (key.Length != 32)
		{
			throw new ArgumentException("The key size is invalid. Must be exactly 32 bytes.", "key");
		}
		arulw = new byte[32];
		Buffer.BlockCopy(key, 0, arulw, 0, 32);
		iwslm = null;
		dumma = null;
		vcvmn = false;
	}

	public void Encrypt(byte[] nonce, byte[] plaintext, byte[] ciphertext, byte[] authTag, byte[] additionalAuthData = null)
	{
		yxwej();
		mnpxr(nonce, plaintext, ciphertext, authTag);
		nvqxb nvqxb = dumma;
		if (nvqxb == null || 1 == 0)
		{
			nvqxb = new nvqxb(arulw);
		}
		dumma = nvqxb;
		dumma.yirig(nonce);
		if (additionalAuthData != null && 0 == 0)
		{
			dumma.seoke(additionalAuthData);
		}
		dumma.yajzn(plaintext, 0, plaintext.Length, ciphertext, 0);
		dumma.mdexb(authTag, 0);
	}

	public void Decrypt(byte[] nonce, byte[] ciphertext, byte[] authTag, byte[] plaintext, byte[] additionalAuthData = null)
	{
		yxwej();
		mnpxr(nonce, plaintext, ciphertext, authTag);
		fmjea fmjea = iwslm;
		if (fmjea == null || 1 == 0)
		{
			fmjea = new fmjea(arulw);
		}
		iwslm = fmjea;
		iwslm.yirig(nonce);
		iwslm.tglzc(authTag, 0, authTag.Length);
		if (additionalAuthData != null && 0 == 0)
		{
			iwslm.seoke(additionalAuthData);
		}
		try
		{
			iwslm.yajzn(ciphertext, 0, ciphertext.Length, plaintext, 0);
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

	public void Dispose()
	{
		nvqxb nvqxb = dumma;
		fmjea fmjea = iwslm;
		byte[] array = arulw;
		if (nvqxb != null && 0 == 0)
		{
			nvqxb.Dispose();
			dumma = null;
		}
		if (fmjea != null && 0 == 0)
		{
			fmjea.Dispose();
			iwslm = null;
		}
		if (array != null && 0 == 0)
		{
			Array.Clear(array, 0, array.Length);
			arulw = null;
		}
		vcvmn = true;
	}

	private void yxwej()
	{
		if (vcvmn && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
	}

	private static void mnpxr(byte[] p0, byte[] p1, byte[] p2, byte[] p3)
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
		if (p3 == null || 1 == 0)
		{
			throw new ArgumentNullException("authTag");
		}
		if (p0.Length != 12)
		{
			throw new ArgumentException("The nonce size is invalid. Must be exactly 12 bytes.", "nonce");
		}
		if (p1.Length != p2.Length)
		{
			throw new ArgumentException("The plaintext and ciphertext arrays have to be of the same length.");
		}
		if (p3.Length == 16)
		{
			return;
		}
		throw new ArgumentException("The authTag size is invalid. Must be exactly 16 bytes.", "authTag");
	}
}
