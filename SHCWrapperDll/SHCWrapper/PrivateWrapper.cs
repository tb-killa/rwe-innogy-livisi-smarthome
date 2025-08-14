using System;
using System.Runtime.InteropServices;
using System.Text;
using SHCWrapper.Drivers;

namespace SHCWrapper;

internal class PrivateWrapper
{
	public const uint CERT_KEY_PROV_INFO_PROP_ID = 2u;

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool GetSGTIN(StringBuilder buffer, int len);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool GetHWVersion(StringBuilder buffer, int len);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool StartSNTPService();

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool StopSNTPService();

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool DhcpRenew(string adapterName);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool RasIsConnected(string connection_name);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool RasConnect(string connection_name, string login, string password);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool RasDisconnect(string connection_name);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern void Reset();

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool IsFactoryReset();

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool EraseRawPartition();

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool WriteRawPartition(string file_to_write);

	[DllImport("spisdk.dll", CharSet = CharSet.Unicode)]
	public static extern IntPtr OpenSPI(string driver_name);

	[DllImport("spisdk.dll", CharSet = CharSet.Unicode)]
	public static extern void CloseSPI(IntPtr hSpi);

	[DllImport("spisdk.dll", CharSet = CharSet.Unicode)]
	public static extern bool TransacSPI(IntPtr hSpi, byte[] pBufIn, byte[] pBufOut, int Len, bool bSplitTransaction);

	[DllImport("gpiosdk.dll", CharSet = CharSet.Unicode)]
	public static extern IntPtr OpenGpio(string driver_name);

	[DllImport("gpiosdk.dll", CharSet = CharSet.Unicode)]
	public static extern void CloseGpio(IntPtr hGpio);

	[DllImport("gpiosdk.dll", CharSet = CharSet.Unicode)]
	public static extern bool ConfigureGpio(IntPtr hGpio, GPIOManager.CONFIGURATION_DESCRIPTOR pio_desciptor);

	[DllImport("gpiosdk.dll", CharSet = CharSet.Unicode)]
	public static extern bool GetGpioState(IntPtr hGpio, ref GPIOManager.LEVEL_DESCRIPTOR pio_state);

	[DllImport("gpiosdk.dll", CharSet = CharSet.Unicode)]
	public static extern bool SetGpioState(IntPtr hGpio, GPIOManager.LEVEL_DESCRIPTOR pio_state);

	[DllImport("gpiosdk.dll", CharSet = CharSet.Unicode)]
	public static extern IntPtr GetGpioIntrEvent(IntPtr hGpio, uint pin_number);

	[DllImport("gpiosdk.dll", CharSet = CharSet.Unicode)]
	public static extern void ReleaseGpioIntrEvent(IntPtr hEvent);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool Cert_ImportCertificateWithPrivateKey(string szFilename, string szSubsystemProtocol, string szPasswordCertificate);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool Cert_ImportCertificate(string szFilename, string szSubsystemProtocol);

	[DllImport("shc_api.dll", CharSet = CharSet.Unicode)]
	public static extern bool LoadPublicKeyFromCertFile(string szFilename);

	[DllImport("crypt32.dll", CharSet = CharSet.Unicode)]
	public static extern bool CertGetCertificateContextProperty(IntPtr handle, uint value, IntPtr data, ref uint uiSize);
}
