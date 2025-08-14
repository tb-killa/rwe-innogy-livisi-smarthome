using System;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

internal sealed class ARCFourManagedTransform : ICryptoTransform, IDisposable
{
	private bool m_Disposed;

	private byte m_Index1;

	private byte m_Index2;

	private byte[] m_Key;

	private int m_KeyLen;

	private byte[] m_Permutation;

	public bool CanReuseTransform => true;

	public bool CanTransformMultipleBlocks => true;

	public int InputBlockSize => 1;

	public int OutputBlockSize => 1;

	public ARCFourManagedTransform(byte[] key)
	{
		m_Key = (byte[])key.Clone();
		m_KeyLen = key.Length;
		m_Permutation = new byte[256];
		m_Disposed = false;
		Init();
	}

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (inputBuffer == null || outputBuffer == null)
		{
			throw new ArgumentNullException();
		}
		if (inputOffset < 0 || outputOffset < 0 || inputOffset + inputCount > inputBuffer.Length || outputOffset + inputCount > outputBuffer.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		int num = inputOffset + inputCount;
		while (inputOffset < num)
		{
			m_Index1 = (byte)((m_Index1 + 1) % 256);
			m_Index2 = (byte)((m_Index2 + m_Permutation[m_Index1]) % 256);
			byte b = m_Permutation[m_Index1];
			m_Permutation[m_Index1] = m_Permutation[m_Index2];
			m_Permutation[m_Index2] = b;
			byte b2 = (byte)((m_Permutation[m_Index1] + m_Permutation[m_Index2]) % 256);
			outputBuffer[outputOffset] = (byte)(inputBuffer[inputOffset] ^ m_Permutation[b2]);
			inputOffset++;
			outputOffset++;
		}
		return inputCount;
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		byte[] array = new byte[inputCount];
		TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
		Init();
		return array;
	}

	public void Dispose()
	{
		Array.Clear(m_Key, 0, m_Key.Length);
		Array.Clear(m_Permutation, 0, m_Permutation.Length);
		m_Index1 = 0;
		m_Index2 = 0;
		m_Disposed = true;
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
	}

	private void Init()
	{
		for (int i = 0; i < 256; i++)
		{
			m_Permutation[i] = (byte)i;
		}
		m_Index1 = 0;
		m_Index2 = 0;
		int num = 0;
		for (int j = 0; j < 256; j++)
		{
			num = (num + m_Permutation[j] + m_Key[j % m_KeyLen]) % 256;
			byte b = m_Permutation[j];
			m_Permutation[j] = m_Permutation[num];
			m_Permutation[num] = b;
		}
	}

	~ARCFourManagedTransform()
	{
		Dispose();
	}
}
