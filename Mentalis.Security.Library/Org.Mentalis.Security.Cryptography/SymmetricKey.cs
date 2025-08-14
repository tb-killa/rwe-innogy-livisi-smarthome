using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Org.Mentalis.Security.Cryptography;

internal sealed class SymmetricKey : IDisposable
{
	private static readonly byte[] ExponentOfOne = new byte[596]
	{
		7, 2, 0, 0, 0, 164, 0, 0, 82, 83,
		65, 50, 0, 4, 0, 0, 1, 0, 0, 0,
		75, 89, 78, 38, 208, 90, 51, 11, 189, 93,
		68, 83, 25, 163, 116, 138, 28, 144, 113, 117,
		8, 65, 25, 244, 188, 146, 35, 4, 38, 103,
		141, 190, 228, 111, 116, 113, 71, 96, 96, 85,
		31, 114, 32, 121, 242, 33, 171, 145, 196, 201,
		92, 180, 137, 103, 82, 16, 156, 113, 82, 123,
		212, 66, 174, 14, 147, 166, 175, 141, 58, 97,
		112, 65, 152, 195, 88, 220, 207, 76, 239, 62,
		198, 243, 224, 180, 205, 251, 236, 129, 11, 122,
		117, 41, 122, 190, 64, 246, 74, 63, 64, 183,
		67, 240, 69, 63, 150, 241, 115, 47, 113, 238,
		167, 112, 77, 249, 99, 184, 82, 76, 241, 24,
		243, 60, 33, 19, 106, 154, 133, 183, 161, 253,
		182, 164, 241, 235, 3, 214, 134, 5, 106, 99,
		147, 178, 231, 249, 42, 119, 9, 228, 12, 144,
		45, 106, 162, 205, 55, 11, 192, 182, 28, 150,
		195, 167, 87, 177, 119, 249, 85, 17, 143, 68,
		141, 119, 49, 167, 69, 224, 142, 66, 13, 228,
		7, 83, 243, 92, 139, 199, 215, 184, 100, 31,
		192, 234, 107, 247, 156, 145, 25, 173, 121, 233,
		222, 195, 69, 102, 237, 62, 30, 144, 64, 38,
		139, 1, 127, 206, 5, 218, 151, 139, 248, 71,
		63, 79, 116, 242, 109, 31, 22, 211, 37, 87,
		45, 48, 111, 60, 226, 65, 134, 193, 199, 51,
		1, 84, 3, 5, 164, 88, 204, 136, 156, 141,
		101, 94, 2, 92, 34, 200, 1, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 186, 246, 143, 42, 154, 76,
		61, 210, 186, 216, 119, 89, 65, 138, 237, 61,
		130, 36, 6, 193, 55, 121, 129, 5, 251, 156,
		108, 21, 190, 68, 92, 181, 22, 4, 196, 78,
		157, 137, 239, 241, 21, 38, 25, 22, 62, 221,
		172, 79, 225, 170, 68, 123, 160, 197, 233, 147,
		193, 52, 21, 103, 105, 45, 195, 131, 1, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0
	};

	private int m_ExponentOfOne;

	private int m_Handle;

	private bool m_OwnsProvider;

	private PaddingMode m_PaddingMode;

	private int m_Provider;

	public int Provider
	{
		get
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return m_Provider;
		}
	}

	public int Handle
	{
		get
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return m_Handle;
		}
	}

	public byte[] IV
	{
		get
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			int pdwDataLen = 0;
			if (SspiProvider.CryptGetKeyParam(m_Handle, 1, null, ref pdwDataLen, 0) == 0)
			{
				throw new CryptographicException("Could not get the IV.");
			}
			byte[] array = new byte[pdwDataLen];
			if (SspiProvider.CryptGetKeyParam(m_Handle, 1, array, ref pdwDataLen, 0) == 0)
			{
				throw new CryptographicException("Could not get the IV.");
			}
			return array;
		}
		set
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			if (SspiProvider.CryptSetKeyParam(m_Handle, 1, value, 0) == 0)
			{
				throw new CryptographicException("Unable to set the initialization vector.");
			}
		}
	}

	public CipherMode Mode
	{
		get
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			int pbData = 0;
			int pdwDataLen = 4;
			if (SspiProvider.CryptGetKeyParam(m_Handle, 4, ref pbData, ref pdwDataLen, 0) == 0)
			{
				throw new CryptographicException("Could not get the cipher mode.");
			}
			return (CipherMode)pbData;
		}
		set
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			int pbData = (int)value;
			if (SspiProvider.CryptSetKeyParam(m_Handle, 4, ref pbData, 0) == 0)
			{
				throw new CryptographicException("Unable to set the cipher mode.");
			}
		}
	}

	public int FeedbackSize
	{
		get
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			int pbData = 0;
			int pdwDataLen = 4;
			if (SspiProvider.CryptGetKeyParam(m_Handle, 5, ref pbData, ref pdwDataLen, 0) == 0)
			{
				throw new CryptographicException("Could not get the feedback size.");
			}
			return pbData;
		}
		set
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (SspiProvider.CryptSetKeyParam(m_Handle, 5, ref value, 0) == 0)
			{
				throw new CryptographicException("Unable to set the feedback size.");
			}
		}
	}

	public PaddingMode Padding
	{
		get
		{
			return m_PaddingMode;
		}
		set
		{
			if (m_Handle == 0)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			int pbData = GetPaddingMode(value);
			if (SspiProvider.CryptSetKeyParam(m_Handle, 3, ref pbData, 0) == 0)
			{
				throw new CryptographicException("Unable to set the padding type.");
			}
			m_PaddingMode = value;
		}
	}

	private SymmetricKey(bool ownsProvider)
	{
		m_OwnsProvider = ownsProvider;
	}

	private SymmetricKey(CryptoProvider provider)
		: this(ownsProvider: false)
	{
		m_Provider = CAPIProvider.Handle;
		m_ExponentOfOne = CreateExponentOfOneKey();
		m_PaddingMode = PaddingMode.None;
	}

	internal SymmetricKey(int provider, int key, bool ownsProvider)
		: this(ownsProvider)
	{
		if (key == 0 || provider == 0)
		{
			throw new ArgumentNullException();
		}
		m_Provider = provider;
		m_Handle = key;
		m_PaddingMode = PaddingMode.None;
	}

	public SymmetricKey(CryptoProvider provider, CryptoAlgorithm algorithm)
		: this(provider)
	{
		m_Handle = 0;
		if (SspiProvider.CryptGenKey(m_Provider, new IntPtr((int)algorithm), 1, ref m_Handle) == 0)
		{
			throw new SecurityException("Cannot generate session key.");
		}
	}

	public SymmetricKey(CryptoProvider provider, CryptoAlgorithm algorithm, byte[] buffer)
		: this(provider)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		m_Handle = KeyFromBytes(m_Provider, algorithm, buffer);
	}

	public void Dispose()
	{
		if (m_Handle != 0)
		{
			SspiProvider.CryptDestroyKey(m_Handle);
		}
		if (m_ExponentOfOne != 0)
		{
			SspiProvider.CryptDestroyKey(m_ExponentOfOne);
		}
		m_Handle = (m_ExponentOfOne = (m_Provider = 0));
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
	}

	private int KeyFromBytes(int provider, CryptoAlgorithm algorithm, byte[] key)
	{
		int dwFlags = 1;
		int pbData = 0;
		int pbData2 = 0;
		int num = 0;
		int phKey = 0;
		int phKey2 = 0;
		IntPtr intPtr = new IntPtr((int)algorithm);
		IntPtr pbData3 = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		IntPtr intPtr3 = IntPtr.Zero;
		try
		{
			bool flag = false;
			intPtr3 = Marshal.AllocHGlobal(84 + IntPtr.Size);
			int pdwDataLen;
			do
			{
				pdwDataLen = 84 + IntPtr.Size;
				if (SspiProvider.CryptGetProvParam(provider, 22, intPtr3, ref pdwDataLen, dwFlags) == 0)
				{
					break;
				}
				dwFlags = 0;
				if (Marshal.ReadIntPtr(intPtr3) == intPtr)
				{
					flag = true;
				}
			}
			while (!flag);
			if (!flag)
			{
				throw new SecurityException("CSP does not support selected algorithm.");
			}
			if (SspiProvider.CryptGenKey(provider, intPtr, 0, ref phKey) == 0)
			{
				throw new SecurityException("Cannot generate temporary key.");
			}
			pdwDataLen = 4;
			if (SspiProvider.CryptGetKeyParam(phKey, 9, ref pbData, ref pdwDataLen, 0) == 0)
			{
				throw new SecurityException("Cannot retrieve key parameters.");
			}
			if (key.Length * 8 > pbData)
			{
				throw new SecurityException("Key too big.");
			}
			pdwDataLen = IntPtr.Size;
			if (SspiProvider.CryptGetKeyParam(m_ExponentOfOne, 7, ref pbData3, ref pdwDataLen, 0) == 0)
			{
				throw new SecurityException("Unable to get the private key's algorithm.");
			}
			pdwDataLen = 4;
			if (SspiProvider.CryptGetKeyParam(m_ExponentOfOne, 9, ref pbData2, ref pdwDataLen, 0) == 0)
			{
				throw new SecurityException("Unable to get the key length.");
			}
			int num2 = pbData2 / 8 + IntPtr.Size + 4 + IntPtr.Size;
			intPtr2 = Marshal.AllocHGlobal(num2);
			PUBLICKEYSTRUC pUBLICKEYSTRUC = new PUBLICKEYSTRUC
			{
				bType = 1,
				bVersion = 2,
				reserved = 0,
				aiKeyAlg = intPtr
			};
			Marshal.StructureToPtr((object)pUBLICKEYSTRUC, intPtr2, fDeleteOld: false);
			Marshal.WriteInt32(intPtr2, num = Marshal.SizeOf((object)pUBLICKEYSTRUC), pbData3.ToInt32());
			num += IntPtr.Size;
			for (int i = 0; i < key.Length; i++)
			{
				Marshal.WriteByte(intPtr2, num + key.Length - i - 1, key[i]);
			}
			pdwDataLen = num2 - (IntPtr.Size + IntPtr.Size + 4 + key.Length + 3);
			num += key.Length + 1;
			if (SspiProvider.CryptGenRandom(provider, pdwDataLen, new IntPtr(intPtr2.ToInt64() + num)) == 0)
			{
				throw new SecurityException("Could not generate random data.");
			}
			for (int j = 0; j < pdwDataLen; j++)
			{
				if (Marshal.ReadByte(intPtr2, num) == 0)
				{
					Marshal.WriteByte(intPtr2, num, 1);
				}
				num++;
			}
			Marshal.WriteByte(intPtr2, num2 - 2, 2);
			if (SspiProvider.CryptImportKey(provider, intPtr2, num2, m_ExponentOfOne, 1, ref phKey2) == 0)
			{
				throw new SecurityException("Cannot import key [key has right size?].");
			}
			return phKey2;
		}
		finally
		{
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (phKey != 0)
			{
				SspiProvider.CryptDestroyKey(phKey);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
		}
	}

	public byte[] ToBytes()
	{
		if (m_Handle == 0)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			int pdwDataLen = 0;
			if (SspiProvider.CryptExportKey(m_Handle, m_ExponentOfOne, 1, 0, IntPtr.Zero, ref pdwDataLen) == 0)
			{
				throw new SecurityException("Cannot export key.");
			}
			intPtr = Marshal.AllocHGlobal(pdwDataLen);
			if (SspiProvider.CryptExportKey(m_Handle, m_ExponentOfOne, 1, 0, intPtr, ref pdwDataLen) == 0)
			{
				throw new SecurityException("Cannot export key.");
			}
			int pdwDataLen2 = 4;
			int pbData = 0;
			if (SspiProvider.CryptGetKeyParam(m_Handle, 9, ref pbData, ref pdwDataLen2, 0) == 0)
			{
				throw new SecurityException("Cannot retrieve key parameters.");
			}
			pbData /= 8;
			byte[] array = new byte[pbData];
			int num = 4 + IntPtr.Size;
			num += IntPtr.Size;
			Marshal.Copy(new IntPtr(intPtr.ToInt64() + num), array, 0, array.Length);
			Array.Reverse(array);
			return array;
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public override string ToString()
	{
		if (m_Handle == 0)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		byte[] array = ToBytes();
		StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	~SymmetricKey()
	{
		Dispose();
	}

	private int CreateExponentOfOneKey()
	{
		try
		{
			return CreateStaticExponentOfOneKey();
		}
		catch
		{
			return CreateDynamicExponentOfOneKey();
		}
	}

	private int CreateStaticExponentOfOneKey()
	{
		int phKey = 0;
		if (SspiProvider.CryptImportKey(m_Provider, ExponentOfOne, ExponentOfOne.Length, 0, 1, ref phKey) == 0)
		{
			throw new SecurityException("Could not import modified key.");
		}
		return phKey;
	}

	private int CreateDynamicExponentOfOneKey()
	{
		int phKey = 0;
		int pdwDataLen = 0;
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			if (SspiProvider.CryptGenKey(m_Provider, new IntPtr(1), 1, ref phKey) == 0)
			{
				throw new SecurityException("Cannot generate key pair.");
			}
			if (SspiProvider.CryptExportKey(phKey, 0, 7, 0, IntPtr.Zero, ref pdwDataLen) == 0)
			{
				throw new SecurityException("Cannot export generated key.");
			}
			intPtr = Marshal.AllocHGlobal(pdwDataLen);
			if (SspiProvider.CryptExportKey(phKey, 0, 7, 0, intPtr, ref pdwDataLen) == 0)
			{
				throw new SecurityException("Cannot export generated key.");
			}
			SspiProvider.CryptDestroyKey(phKey);
			phKey = 0;
			int num = Marshal.ReadInt32(intPtr, 12);
			int num2 = 16;
			for (int i = 0; i < 4; i++)
			{
				if (i == 0)
				{
					Marshal.WriteByte(intPtr, num2, 1);
				}
				else
				{
					Marshal.WriteByte(intPtr, num2 + i, 0);
				}
			}
			num2 += 4;
			num2 += num / 8;
			num2 += num / 16;
			num2 += num / 16;
			for (int j = 0; j < num / 16; j++)
			{
				if (j == 0)
				{
					Marshal.WriteByte(intPtr, num2, 1);
				}
				else
				{
					Marshal.WriteByte(intPtr, num2 + j, 0);
				}
			}
			num2 += num / 16;
			for (int k = 0; k < num / 16; k++)
			{
				if (k == 0)
				{
					Marshal.WriteByte(intPtr, num2, 1);
				}
				else
				{
					Marshal.WriteByte(intPtr, num2 + k, 0);
				}
			}
			num2 += num / 16;
			num2 += num / 16;
			for (int l = 0; l < num / 8; l++)
			{
				if (l == 0)
				{
					Marshal.WriteByte(intPtr, num2, 1);
				}
				else
				{
					Marshal.WriteByte(intPtr, num2 + l, 0);
				}
			}
			if (SspiProvider.CryptImportKey(m_Provider, intPtr, pdwDataLen, 0, 1, ref phKey) == 0)
			{
				throw new SecurityException("Could not import modified key.");
			}
			return phKey;
		}
		catch (Exception ex)
		{
			if (phKey != 0)
			{
				SspiProvider.CryptDestroyKey(phKey);
			}
			throw ex;
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	private int GetPaddingMode(PaddingMode mode)
	{
		if (mode == PaddingMode.PKCS7)
		{
			return 1;
		}
		return 3;
	}
}
