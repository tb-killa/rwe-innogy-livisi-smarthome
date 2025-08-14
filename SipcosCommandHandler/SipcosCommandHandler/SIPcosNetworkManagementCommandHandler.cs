using System.Collections.Generic;
using CommonFunctionality.Encryption;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosNetworkManagementCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveDeviceInfoData(SIPcosDeviceInfoFrame deviceInfo);

	public event ReceiveDeviceInfoData ReceiveDeviceInfo;

	public SIPcosNetworkManagementCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.NETWORK_MANAGEMENT_FRAME)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		if (message.Count == 0 || header.FrameType != SIPcosFrameType.NETWORK_MANAGEMENT_FRAME)
		{
			return HandlingResult.NotHandled;
		}
		switch ((SIPcosNetworkManagementFrameType)message[0])
		{
		case SIPcosNetworkManagementFrameType.DeviceInfoFrame:
			handleDeviceInfo(header, message);
			break;
		case SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame:
			handleDeviceInfo(header, message);
			break;
		}
		return HandlingResult.Handled;
	}

	private void handleDeviceInfo(SIPcosHeader header, List<byte> message)
	{
		ReceiveDeviceInfoData receiveDeviceInfo = this.ReceiveDeviceInfo;
		if (receiveDeviceInfo != null && message.Count != 0)
		{
			SIPcosDeviceInfoFrame sIPcosDeviceInfoFrame = new SIPcosDeviceInfoFrame(header);
			if (sIPcosDeviceInfoFrame.parse(ref message))
			{
				receiveDeviceInfo(sIPcosDeviceInfoFrame);
			}
		}
	}

	public SendStatus SendNetworkInfoFrame(SIPcosHeader header, SendMode Mode, NetworkInfoFrameType type, byte[] sgtin)
	{
		SIPCOSMessage sIPCOSMessage = GenerateNetworkInfoFrame(header, type, sgtin);
		sIPCOSMessage.Mode = Mode;
		return SendDefaultSync(sIPCOSMessage);
	}

	public SendStatus SendNetworkAcceptFrame(SIPcosHeader header, SendMode Mode, NetworkAcceptFrame frame)
	{
		SIPCOSMessage sIPCOSMessage = GenerateNetworkAcceptFrame(header, frame);
		sIPCOSMessage.Mode = Mode;
		return SendDefaultSync(sIPCOSMessage);
	}

	public SendStatus SendForwardNetworkAcceptFrame(SIPcosHeader header, SendMode Mode, NetworkAcceptFrame frame, byte[] NewDeviceIP)
	{
		SIPCOSMessage sIPCOSMessage = GenerateForwardNetworkAcceptFrame(header, frame, NewDeviceIP);
		sIPCOSMessage.Mode = Mode;
		return SendDefaultSync(sIPCOSMessage);
	}

	public SendStatus SendNetworkExclusionFrame(SIPcosHeader header, SendMode Mode)
	{
		SIPCOSMessage sIPCOSMessage = GenerateNetworkExclusionFrame(header);
		sIPCOSMessage.Mode = Mode;
		return Send(sIPCOSMessage);
	}

	public void EncryptNetworkKey(SIPcosDeviceInfoFrame info, ref byte[] NetworkKey, byte[] OneTimeKey)
	{
		byte[] array = new byte[16];
		byte[] sGTIN = info.SGTIN;
		for (int i = 0; i < sGTIN.Length; i++)
		{
			array[i] = sGTIN[i];
		}
		ulong num = (ulong)(info.SecuritySequenceNumber[0] << 24);
		num += (ulong)(info.SecuritySequenceNumber[1] << 16);
		num += (ulong)(info.SecuritySequenceNumber[2] << 8);
		num += (ulong)(info.SecuritySequenceNumber[3] + 3);
		array[12] = (byte)((num >> 24) & 0xFF);
		array[13] = (byte)((num >> 16) & 0xFF);
		array[14] = (byte)((num >> 8) & 0xFF);
		array[15] = (byte)(num & 0xFF);
		Aes aes = new Aes(KeySize.Bits128, OneTimeKey);
		aes.Cipher(array, out var output);
		for (int j = 0; j < output.Length; j++)
		{
			NetworkKey[j] ^= output[j];
		}
	}

	public void EncryptNetworkKey(SIPcosDeviceInfoFrame info, ref byte[] NetworkKey, byte[] OneTimeKey, byte[] DefaultKey)
	{
		EncryptNetworkKey(info, ref NetworkKey, OneTimeKey);
		byte[] array = new byte[16];
		byte[] sGTIN = info.SGTIN;
		for (int i = 0; i < sGTIN.Length; i++)
		{
			array[i] = sGTIN[i];
		}
		ulong num = (ulong)(info.SecuritySequenceNumber[0] << 24);
		num += (ulong)(info.SecuritySequenceNumber[1] << 16);
		num += (ulong)(info.SecuritySequenceNumber[2] << 8);
		num += (ulong)(info.SecuritySequenceNumber[3] + 2);
		array[12] = (byte)((num >> 24) & 0xFF);
		array[13] = (byte)((num >> 16) & 0xFF);
		array[14] = (byte)((num >> 8) & 0xFF);
		array[15] = (byte)(num & 0xFF);
		Aes aes = new Aes(KeySize.Bits128, DefaultKey);
		aes.Cipher(array, out var output);
		for (int j = 0; j < output.Length; j++)
		{
			NetworkKey[j] ^= output[j];
		}
	}

	public void GenerateMIC(SIPcosDeviceInfoFrame info, out byte[] Mic32, byte[] NetworkKey, byte[] DefaultKey)
	{
		byte[] array = new byte[16];
		for (int i = 0; i < 12; i++)
		{
			array[i] = info.SGTIN[i];
		}
		ulong num = (ulong)(info.SecuritySequenceNumber[0] << 24);
		num += (ulong)(info.SecuritySequenceNumber[1] << 16);
		num += (ulong)(info.SecuritySequenceNumber[2] << 8);
		num += (ulong)(info.SecuritySequenceNumber[3] + 1);
		array[12] = (byte)((num >> 24) & 0xFF);
		array[13] = (byte)((num >> 16) & 0xFF);
		array[14] = (byte)((num >> 8) & 0xFF);
		array[15] = (byte)(num & 0xFF);
		Aes aes = new Aes(KeySize.Bits128, DefaultKey);
		aes.Cipher(array, out var output);
		Mic32 = new byte[4];
		for (int j = 0; j < 4; j++)
		{
			Mic32[j] = (byte)(output[j] ^ NetworkKey[j]);
		}
	}

	public SIPCOSMessage GenerateNetworkInfoFrame(SIPcosHeader header, NetworkInfoFrameType type, byte[] sgtin)
	{
		header.BiDi = false;
		header.MacSecurity = false;
		header.FrameType = SIPcosFrameType.NETWORK_MANAGEMENT_FRAME;
		List<byte> list = new List<byte>();
		list.Add(2);
		list.Add((byte)type);
		list.AddRange(sgtin);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateNetworkAcceptFrame(SIPcosHeader header, NetworkAcceptFrame frame)
	{
		header.CorestackFrameType = CorestackFrameType.SIPCOS_APPLICATION;
		header.MacSecurity = false;
		header.QoS = false;
		header.LocalHopLimit = 7;
		header.Fragmentation = false;
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.NETWORK_MANAGEMENT_FRAME;
		List<byte> list = new List<byte>();
		list.Add(4);
		list.AddRange(frame.getSerialData());
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateForwardNetworkAcceptFrame(SIPcosHeader header, NetworkAcceptFrame frame, byte[] newDeviceIP)
	{
		SIPCOSMessage sIPCOSMessage = GenerateNetworkAcceptFrame(header, frame);
		sIPCOSMessage.Data[0] = 5;
		sIPCOSMessage.Data.AddRange(newDeviceIP);
		return sIPCOSMessage;
	}

	public SIPCOSMessage GenerateNetworkExclusionFrame(SIPcosHeader header)
	{
		m_hander.RemoveSequenseNumber(header.Destination);
		header.BiDi = false;
		header.StayAwake = false;
		header.MacSecurity = true;
		header.FrameType = SIPcosFrameType.NETWORK_MANAGEMENT_FRAME;
		List<byte> list = new List<byte>();
		list.Add(6);
		return new SIPCOSMessage(header, list);
	}
}
