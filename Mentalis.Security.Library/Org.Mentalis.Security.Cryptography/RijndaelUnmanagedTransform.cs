using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

internal class RijndaelUnmanagedTransform : ICryptoTransform, IDisposable
{
	private int m_BlockSize;

	private SymmetricKey m_Key;

	private CryptoMethod m_Method;

	public bool CanReuseTransform
	{
		get
		{
			if (m_Key == null)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return true;
		}
	}

	public bool CanTransformMultipleBlocks
	{
		get
		{
			if (m_Key == null)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return true;
		}
	}

	public int InputBlockSize
	{
		get
		{
			if (m_Key == null)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return m_BlockSize / 8;
		}
	}

	public int OutputBlockSize
	{
		get
		{
			if (m_Key == null)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return m_BlockSize / 8;
		}
	}

	public RijndaelUnmanagedTransform(CryptoAlgorithm algorithm, CryptoMethod method, byte[] key, byte[] iv, CipherMode mode, int feedback, PaddingMode padding)
	{
		m_Key = new SymmetricKey(CryptoProvider.RsaAes, algorithm, key);
		m_Key.IV = iv;
		m_Key.Mode = mode;
		if (mode == CipherMode.CFB)
		{
			m_Key.FeedbackSize = feedback;
		}
		m_Key.Padding = padding;
		m_BlockSize = 128;
		m_Method = method;
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
		if (inputCount < 0 || inputOffset < 0 || outputOffset < 0 || inputOffset + inputCount > inputBuffer.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		int pdwDataLen = inputCount;
		if (m_Method == CryptoMethod.Encrypt)
		{
			if (SspiProvider.CryptEncrypt(m_Key.Handle, 0, 0, 0, null, ref pdwDataLen, 0) == 0)
			{
				throw new CryptographicException("Could not encrypt data.");
			}
			if (outputBuffer.Length - outputOffset < pdwDataLen)
			{
				throw new ArgumentOutOfRangeException();
			}
			Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			pdwDataLen = inputCount;
			GCHandle gCHandle = GCHandle.Alloc(outputBuffer, GCHandleType.Pinned);
			try
			{
				if (SspiProvider.CryptEncrypt(m_Key.Handle, 0, 0, 0, new IntPtr(gCHandle.AddrOfPinnedObject().ToInt64() + outputOffset), ref pdwDataLen, outputBuffer.Length - outputOffset) == 0)
				{
					throw new CryptographicException("Could not encrypt data.");
				}
			}
			finally
			{
				gCHandle.Free();
			}
		}
		else
		{
			byte[] array = new byte[inputCount];
			Array.Copy(inputBuffer, inputOffset, array, 0, inputCount);
			if (SspiProvider.CryptDecrypt(m_Key.Handle, 0, 0, 0, array, ref pdwDataLen) == 0)
			{
				throw new CryptographicException("Could not decrypt data.");
			}
			if (pdwDataLen > outputBuffer.Length - outputOffset)
			{
				throw new ArgumentOutOfRangeException();
			}
			Array.Copy(array, 0, outputBuffer, outputOffset, pdwDataLen);
		}
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
		int pdwDataLen = inputCount;
		byte[] array;
		if (m_Method == CryptoMethod.Encrypt)
		{
			if (SspiProvider.CryptEncrypt(m_Key.Handle, 0, 1, 0, null, ref pdwDataLen, 0) == 0)
			{
				throw new CryptographicException("Could not encrypt data.");
			}
			array = new byte[pdwDataLen];
			Array.Copy(inputBuffer, inputOffset, array, 0, inputCount);
			pdwDataLen = inputCount;
			if (SspiProvider.CryptEncrypt(m_Key.Handle, 0, 1, 0, array, ref pdwDataLen, array.Length) == 0)
			{
				throw new CryptographicException("Could not encrypt data.");
			}
		}
		else
		{
			byte[] array2 = new byte[inputCount];
			Array.Copy(inputBuffer, inputOffset, array2, 0, inputCount);
			if (SspiProvider.CryptDecrypt(m_Key.Handle, 0, 1, 0, array2, ref pdwDataLen) == 0)
			{
				throw new CryptographicException("Could not decrypt data.");
			}
			array = new byte[pdwDataLen];
			Array.Copy(array2, 0, array, 0, pdwDataLen);
		}
		return array;
	}

	~RijndaelUnmanagedTransform()
	{
		Dispose();
	}
}
