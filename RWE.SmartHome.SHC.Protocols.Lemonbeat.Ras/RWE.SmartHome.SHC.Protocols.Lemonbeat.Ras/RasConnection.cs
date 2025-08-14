using System;
using System.Net;
using System.Text;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class RasConnection
{
	private class RasConn
	{
		public const int SizeOffset = 0;

		public const int ConnIdOffset = 4;

		public const int ConnNameOffset = 8;

		private byte[] _data;

		public string Name
		{
			get
			{
				string text = null;
				string text2 = Encoding.Unicode.GetString(_data, 8, 20);
				char[] trimChars = new char[1];
				return text2.TrimEnd(trimChars);
			}
		}

		public IntPtr ConnectionId
		{
			get
			{
				IntPtr zero = IntPtr.Zero;
				return (IntPtr)BitConverter.ToInt32(_data, 4);
			}
		}

		public RasConn(byte[] data, int offset)
		{
			_data = new byte[52];
			Buffer.BlockCopy(data, offset, _data, 0, 52);
		}
	}

	private const int RasConnSize = 52;

	private int connectionHandle;

	private RasError result;

	private uint? pppInterfaceIndex;

	public bool IsConnected()
	{
		RasConnectionStatus connectionStatus = GetConnectionStatus();
		return connectionStatus.Error == RasError.Success;
	}

	public RasError Connect(string entryName, string username, string password)
	{
		RecycleLemonbeatConnection(entryName);
		Disconnect();
		AdaptersMonitor adaptersMonitor = new AdaptersMonitor();
		adaptersMonitor.TakeSnapshot();
		RasDialParams rasDialParams = new RasDialParams();
		rasDialParams.EntryName = entryName;
		rasDialParams.UserName = username;
		rasDialParams.Password = password;
		RasDialParams rasDialParams2 = rasDialParams;
		try
		{
			result = NativeRasWrapper.RasDial(rasDialParams2.Data, 0u, IntPtr.Zero, ref connectionHandle);
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"Error in RasDial, message: {ex.Message}, InnerException: {ex.InnerException}");
		}
		if (result != RasError.Success)
		{
			RasError rasError = result;
			Disconnect();
			result = rasError;
		}
		pppInterfaceIndex = adaptersMonitor.GetNewlyCreatedInterface();
		if (pppInterfaceIndex.HasValue)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, "Detected new IP interface: " + pppInterfaceIndex);
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, "Failed to detect the PPP interface.");
		}
		return result;
	}

	public RasError Disconnect()
	{
		if (connectionHandle != 0)
		{
			RasError rasError = result;
			if (rasError == RasError.PortDisconnected)
			{
				connectionHandle = 0;
				result = RasError.Success;
				return result;
			}
			result = NativeRasWrapper.RasHangUp(connectionHandle);
			if (rasError == RasError.Disconnection && result == RasError.Success)
			{
				connectionHandle = 0;
				return result;
			}
			while (result != RasError.InvalidHandle)
			{
				Thread.Sleep(10);
				RasConnectionStatus connectionStatus = GetConnectionStatus();
				result = connectionStatus.Error;
			}
			connectionHandle = 0;
		}
		result = RasError.Success;
		return result;
	}

	public RasConnectionStatus GetConnectionStatus()
	{
		byte[] array = new byte[304];
		array.SetUInt(0, 304u);
		RasError rasError = NativeRasWrapper.RasGetConnectStatus(connectionHandle, array);
		if (rasError == RasError.InvalidHandle)
		{
			RasConnectionStatus rasConnectionStatus = new RasConnectionStatus();
			rasConnectionStatus.Error = rasError;
			return rasConnectionStatus;
		}
		RasConnectionStatus rasConnectionStatus2 = new RasConnectionStatus();
		rasConnectionStatus2.State = (ConnectionState)array.GetUInt(4);
		rasConnectionStatus2.Error = (RasError)array.GetUInt(8);
		rasConnectionStatus2.DeviceType = array.GetString(12, 16);
		rasConnectionStatus2.DeviceName = array.GetString(46, 128);
		return rasConnectionStatus2;
	}

	public RasError GetIpInfo(bool ipv6, out IPAddress localIp, out IPAddress peerIp)
	{
		localIp = null;
		peerIp = null;
		uint bufferSize = (ipv6 ? 28u : 80u);
		byte[] buffer = new byte[bufferSize];
		buffer.SetUInt(0, bufferSize);
		RasError rasError = NativeRasWrapper.RasGetProjectionInfo(connectionHandle, ipv6 ? 32855 : 32801, buffer, ref bufferSize);
		if (rasError != RasError.Success)
		{
			return rasError;
		}
		if (ipv6)
		{
			rasError = (RasError)buffer.GetUInt(4);
			localIp = GetLocalIpv6FromInterfaceIdentifier(buffer, 8);
			peerIp = GetRemoteIpv6FromInterfaceIdentifier(buffer, 16);
		}
		else
		{
			rasError = (RasError)buffer.GetUInt(4);
			localIp = IPAddress.Parse(buffer.GetString(8, 16));
			peerIp = IPAddress.Parse(buffer.GetString(40, 16));
		}
		return rasError;
	}

	private IPAddress GetLocalIpv6FromInterfaceIdentifier(byte[] buffer, int offset)
	{
		byte[] array = new byte[16];
		Array.Copy(buffer, offset, array, 8, 8);
		array[0] = 254;
		array[1] = 128;
		IPAddress iPAddress = new IPAddress(array);
		if (pppInterfaceIndex.HasValue)
		{
			return IPAddress.Parse(iPAddress.ToString() + $"%{pppInterfaceIndex.Value}");
		}
		return iPAddress;
	}

	private IPAddress GetRemoteIpv6FromInterfaceIdentifier(byte[] buffer, int offset)
	{
		byte[] array = new byte[16];
		Array.Copy(buffer, offset, array, 8, 8);
		array[0] = 252;
		array[1] = 0;
		return new IPAddress(array);
	}

	private void RecycleLemonbeatConnection(string connectionName)
	{
		try
		{
			uint bufferByteCount = 0u;
			uint numberOfConnections = 0u;
			byte[] array = new byte[52];
			Buffer.BlockCopy(BitConverter.GetBytes(52), 0, array, 0, 4);
			RasError rasError = NativeRasWrapper.RasEnumConnections(array, ref bufferByteCount, out numberOfConnections);
			if (rasError != RasError.BufferTooSmall)
			{
				return;
			}
			array = new byte[bufferByteCount];
			Buffer.BlockCopy(BitConverter.GetBytes(52), 0, array, 0, 4);
			if (NativeRasWrapper.RasEnumConnections(array, ref bufferByteCount, out numberOfConnections) != RasError.Success || numberOfConnections == 0)
			{
				return;
			}
			RasConn[] array2 = new RasConn[numberOfConnections];
			for (int i = 0; i < numberOfConnections; i++)
			{
				array2[i] = new RasConn(array, 52 * i);
			}
			RasConn[] array3 = array2;
			foreach (RasConn rasConn in array3)
			{
				if (rasConn.Name.CompareTo(connectionName) == 0)
				{
					Log.Debug(Module.LemonbeatProtocolAdapter, "Recycling old connection");
					rasError = NativeRasWrapper.RasHangUp((int)rasConn.ConnectionId);
					if (rasError != RasError.Success)
					{
						Log.Warning(Module.LemonbeatProtocolAdapter, "There was an error terminationg the old Lemonbeat connection. There is a possibility that Lemonbeat communication will not work. Error: " + rasError);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Error recycling connection: " + ex.ToString());
		}
	}
}
