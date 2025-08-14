using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Ssl.Shared;

internal class RSAKeyTransform
{
	private static bool m_FillInfoDone;

	private static FieldInfo m_KeyField;

	private static PropertyInfo m_KeyProp;

	private static bool m_NeedsHack = false;

	private readonly bool m_Disposed;

	private readonly RSACryptoServiceProvider m_Key;

	public RSAKeyTransform(RSACryptoServiceProvider key)
	{
		FillStaticInfo();
		m_Key = key;
		m_Disposed = false;
	}

	public byte[] CreateKeyExchange(byte[] data)
	{
		if (m_Disposed)
		{
			throw new CryptographicException("The key has been disposed");
		}
		if (m_NeedsHack)
		{
			IntPtr handle = GetHandle(m_Key);
			int pdwDataLen = data.Length;
			SspiProvider.CryptEncrypt(handle, 0, 1, 0, null, ref pdwDataLen, 0);
			byte[] array = new byte[pdwDataLen];
			Array.Copy(data, 0, array, 0, data.Length);
			pdwDataLen = data.Length;
			if (SspiProvider.CryptEncrypt(handle, 0, 1, 0, array, ref pdwDataLen, array.Length) == 0)
			{
				throw new CryptographicException("Unable to decrypt the key exchange.");
			}
			Array.Reverse(array);
			return array;
		}
		return m_Key.Encrypt(data, fOAEP: false);
	}

	public byte[] DecryptKeyExchange(byte[] data)
	{
		if (m_Disposed)
		{
			throw new CryptographicException("The key has been disposed");
		}
		if (m_NeedsHack)
		{
			IntPtr handle = GetHandle(m_Key);
			byte[] array = (byte[])data.Clone();
			Array.Reverse(array);
			int pdwDataLen = data.Length;
			SspiProvider.CryptDecrypt(handle, 0, 1, 0, array, ref pdwDataLen);
			Marshal.GetLastWin32Error();
			if (SspiProvider.CryptDecrypt(handle, 0, 1, 0, array, ref pdwDataLen) == 0)
			{
				throw new CryptographicException("Unable to decrypt the key exchange [" + Marshal.GetLastWin32Error() + "].");
			}
			byte[] array2 = new byte[pdwDataLen];
			Array.Copy(array, 0, array2, 0, pdwDataLen);
			return array2;
		}
		return m_Key.Decrypt(data, fOAEP: false);
	}

	private static IntPtr GetHandle(RSACryptoServiceProvider rsa)
	{
		if ((object)m_KeyProp != null)
		{
			return (IntPtr)m_KeyProp.GetValue(rsa, null);
		}
		object value = m_KeyField.GetValue(rsa);
		FieldInfo field = value.GetType().GetField("handle", BindingFlags.Instance | BindingFlags.NonPublic);
		object value2 = field.GetValue(value);
		return (IntPtr)value2;
	}

	private static void FillStaticInfo()
	{
		if (m_FillInfoDone)
		{
			return;
		}
		m_FillInfoDone = true;
		if (m_NeedsHack)
		{
			Type typeFromHandle = typeof(RSACryptoServiceProvider);
			m_KeyProp = typeFromHandle.GetProperty("_safeKeyHandle", BindingFlags.Instance | BindingFlags.NonPublic);
			if ((object)m_KeyProp == null)
			{
				m_KeyField = typeFromHandle.GetField("_safeKeyHandle", BindingFlags.Instance | BindingFlags.NonPublic);
			}
		}
	}
}
