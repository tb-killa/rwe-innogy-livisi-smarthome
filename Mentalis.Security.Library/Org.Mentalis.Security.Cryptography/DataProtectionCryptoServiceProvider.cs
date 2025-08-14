using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public sealed class DataProtectionCryptoServiceProvider : IDisposable
{
	private bool m_Disposed;

	private byte[] m_OptionalEntropy;

	public byte[] Entropy
	{
		get
		{
			return m_OptionalEntropy;
		}
		set
		{
			m_OptionalEntropy = value;
		}
	}

	public DataProtectionCryptoServiceProvider()
		: this(null)
	{
	}

	public DataProtectionCryptoServiceProvider(byte[] optionalEntropy)
	{
		if (optionalEntropy != null)
		{
			m_OptionalEntropy = (byte[])optionalEntropy.Clone();
		}
		m_Disposed = false;
	}

	public void Dispose()
	{
		if (m_OptionalEntropy != null)
		{
			Array.Clear(m_OptionalEntropy, 0, m_OptionalEntropy.Length);
		}
		m_Disposed = true;
	}

	public byte[] ProtectData(ProtectionType type, byte[] data)
	{
		return ProtectData(type, data, Entropy);
	}

	public byte[] ProtectData(ProtectionType type, byte[] data, byte[] entropy)
	{
		if (data == null)
		{
			throw new ArgumentNullException();
		}
		return ProtectData(type, data, 0, data.Length, entropy);
	}

	public byte[] ProtectData(ProtectionType type, byte[] data, int offset, int size, byte[] entropy)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (data == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset + size > data.Length || !Enum.IsDefined(typeof(ProtectionType), type))
		{
			throw new ArgumentException();
		}
		DataBlob pDataIn = default(DataBlob);
		DataBlob pOptionalEntropy = default(DataBlob);
		DataBlob pDataOut = default(DataBlob);
		try
		{
			pDataIn.cbData = size;
			pDataIn.pbData = Marshal.AllocHGlobal(size);
			Marshal.Copy(data, offset, pDataIn.pbData, size);
			if (entropy == null)
			{
				pOptionalEntropy.cbData = 0;
				pOptionalEntropy.pbData = IntPtr.Zero;
			}
			else
			{
				pOptionalEntropy.cbData = entropy.Length;
				pOptionalEntropy.pbData = Marshal.AllocHGlobal(pOptionalEntropy.cbData);
				Marshal.Copy(entropy, 0, pOptionalEntropy.pbData, pOptionalEntropy.cbData);
			}
			pDataOut.cbData = 0;
			pDataOut.pbData = IntPtr.Zero;
			int num = 0;
			if (type == ProtectionType.LocalMachine)
			{
				num |= 4;
			}
			if (SspiProvider.CryptProtectData(ref pDataIn, "", ref pOptionalEntropy, IntPtr.Zero, IntPtr.Zero, num, ref pDataOut) == 0 || pDataOut.pbData == IntPtr.Zero)
			{
				throw new CryptographicException("The data could not be protected.");
			}
			byte[] array = new byte[pDataOut.cbData];
			Marshal.Copy(pDataOut.pbData, array, 0, pDataOut.cbData);
			return array;
		}
		finally
		{
			if (pDataIn.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pDataIn.pbData);
			}
			if (pOptionalEntropy.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pOptionalEntropy.pbData);
			}
			if (pDataOut.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pDataOut.pbData);
			}
		}
	}

	public byte[] UnprotectData(byte[] data)
	{
		return UnprotectData(data, Entropy);
	}

	public byte[] UnprotectData(byte[] data, byte[] entropy)
	{
		if (data == null)
		{
			throw new ArgumentNullException();
		}
		return UnprotectData(data, 0, data.Length, entropy);
	}

	public byte[] UnprotectData(byte[] data, int offset, int size, byte[] entropy)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (data == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset + size > data.Length)
		{
			throw new ArgumentException();
		}
		DataBlob pDataIn = default(DataBlob);
		DataBlob pOptionalEntropy = default(DataBlob);
		DataBlob pDataOut = default(DataBlob);
		try
		{
			pDataIn.cbData = size;
			pDataIn.pbData = Marshal.AllocHGlobal(size);
			Marshal.Copy(data, offset, pDataIn.pbData, size);
			if (entropy == null)
			{
				pOptionalEntropy.cbData = 0;
				pOptionalEntropy.pbData = IntPtr.Zero;
			}
			else
			{
				pOptionalEntropy.cbData = entropy.Length;
				pOptionalEntropy.pbData = Marshal.AllocHGlobal(pOptionalEntropy.cbData);
				Marshal.Copy(entropy, 0, pOptionalEntropy.pbData, pOptionalEntropy.cbData);
			}
			pDataOut.cbData = 0;
			pDataOut.pbData = IntPtr.Zero;
			int num = 0;
			num |= 1;
			if (SspiProvider.CryptUnprotectData(ref pDataIn, IntPtr.Zero, ref pOptionalEntropy, IntPtr.Zero, IntPtr.Zero, num, ref pDataOut) == 0 || pDataOut.pbData == IntPtr.Zero)
			{
				throw new CryptographicException("The data could not be unprotected.");
			}
			byte[] array = new byte[pDataOut.cbData];
			Marshal.Copy(pDataOut.pbData, array, 0, pDataOut.cbData);
			return array;
		}
		finally
		{
			if (pDataIn.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pDataIn.pbData);
			}
			if (pOptionalEntropy.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pOptionalEntropy.pbData);
			}
			if (pDataOut.pbData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pDataOut.pbData);
			}
		}
	}

	~DataProtectionCryptoServiceProvider()
	{
		Dispose();
	}
}
