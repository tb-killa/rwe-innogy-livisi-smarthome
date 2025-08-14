using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

internal class CAPIProvider
{
	private static readonly CAPIProvider m_Provider = new CAPIProvider();

	private static readonly int[] m_Providers = new int[2] { 24, 1 };

	private int m_ContainerHandle;

	private bool m_Error;

	private int m_Handle;

	private int m_HandleProviderType;

	public static int Handle
	{
		get
		{
			m_Provider.CreateInternalHandle(ref m_Provider.m_Handle, null);
			return m_Provider.m_Handle;
		}
	}

	public static int HandleProviderType
	{
		get
		{
			m_Provider.CreateInternalHandle(ref m_Provider.m_Handle, null);
			return m_Provider.m_HandleProviderType;
		}
	}

	public static int ContainerHandle
	{
		get
		{
			m_Provider.CreateInternalHandle(ref m_Provider.m_ContainerHandle, "{48959A69-B181-4cdd-B135-7565701307C5}");
			return m_Provider.m_ContainerHandle;
		}
	}

	public void CreateInternalHandle(ref int handle, string container)
	{
		if (handle != 0)
		{
			return;
		}
		lock (this)
		{
			if (handle == 0 && !m_Error)
			{
				int num = 0;
				int num2 = 0;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
				{
					num = 64;
					num2 = 32;
				}
				for (int i = 0; i < m_Providers.Length; i++)
				{
					int num3 = num | num2;
					m_HandleProviderType = m_Providers[i];
					if (SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i], num3) == 0)
					{
						if (Marshal.GetLastWin32Error() == -2146893802)
						{
							SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i], num3 | 8);
						}
						else if (num2 != 0)
						{
							num3 = num;
							if (SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i], num3) == 0 && Marshal.GetLastWin32Error() == -2146893802)
							{
								SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i], num3 | 8);
							}
						}
					}
					if (handle != 0)
					{
						break;
					}
				}
				if (handle == 0)
				{
					m_Error = true;
					m_HandleProviderType = 0;
				}
			}
			if (m_Error)
			{
				throw new CryptographicException("Couldn't acquire crypto service provider context.");
			}
		}
	}

	~CAPIProvider()
	{
		if (m_Handle != 0)
		{
			SspiProvider.CryptReleaseContext(m_Handle, 0);
		}
		if (m_ContainerHandle != 0)
		{
			SspiProvider.CryptReleaseContext(m_ContainerHandle, 0);
		}
	}
}
