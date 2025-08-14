using System;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

internal class RC4UnmanagedTransform : ICryptoTransform, IDisposable
{
	private SymmetricKey m_Key;

	public bool CanReuseTransform => true;

	public bool CanTransformMultipleBlocks => true;

	public int InputBlockSize => 1;

	public int OutputBlockSize => 1;

	public RC4UnmanagedTransform(byte[] key)
	{
		m_Key = new SymmetricKey(CryptoProvider.RsaFull, CryptoAlgorithm.RC4, key);
	}

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		if (m_Key == null)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (inputBuffer == null || outputBuffer == null)
		{
			throw new ArgumentNullException();
		}
		if (inputCount < 0 || inputOffset < 0 || outputOffset < 0 || inputOffset + inputCount > inputBuffer.Length || outputBuffer.Length - outputOffset < inputCount)
		{
			throw new ArgumentOutOfRangeException();
		}
		byte[] array = new byte[inputCount];
		int pdwDataLen = array.Length;
		Array.Copy(inputBuffer, inputOffset, array, 0, pdwDataLen);
		if (SspiProvider.CryptEncrypt(m_Key.Handle, 0, 0, 0, array, ref pdwDataLen, pdwDataLen) == 0)
		{
			throw new CryptographicException("Could not transform data.");
		}
		Array.Copy(array, 0, outputBuffer, outputOffset, pdwDataLen);
		Array.Clear(array, 0, array.Length);
		return pdwDataLen;
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		if (m_Key == null)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (inputBuffer == null)
		{
			throw new ArgumentNullException();
		}
		if (inputCount < 0 || inputOffset < 0 || inputOffset + inputCount > inputBuffer.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		byte[] array = new byte[inputCount];
		int pdwDataLen = array.Length;
		Array.Copy(inputBuffer, inputOffset, array, 0, pdwDataLen);
		if (SspiProvider.CryptEncrypt(m_Key.Handle, 0, 1, 0, array, ref pdwDataLen, pdwDataLen) == 0)
		{
			throw new CryptographicException("Could not transform data.");
		}
		return array;
	}

	public void Dispose()
	{
		if (m_Key != null)
		{
			m_Key.Dispose();
			m_Key = null;
		}
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
	}

	~RC4UnmanagedTransform()
	{
		Dispose();
	}
}
