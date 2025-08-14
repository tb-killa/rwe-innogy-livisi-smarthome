using System;
using System.Collections.Generic;
using System.Threading;
using CommonFunctionality.Encryption;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosFirmwareUpdateCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveFirmwareAnswerFrame(FirmwareStatusFrame Status);

	public const byte MaxFrameSize = 32;

	private EventWaitHandle m_ewh = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);

	private List<byte> m_data = new List<byte>();

	public event ReceiveFirmwareAnswerFrame ReceiveFirmwareAnswer;

	public SIPcosFirmwareUpdateCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.FIRMWARE_UPDATE)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		if (header.FrameType == SIPcosFrameType.FIRMWARE_UPDATE)
		{
			m_data.Clear();
			m_data.AddRange(message);
			m_ewh.Set();
			if (this.ReceiveFirmwareAnswer != null)
			{
				FirmwareStatusFrame firmwareStatusFrame = new FirmwareStatusFrame(header);
				firmwareStatusFrame.parse(message);
				this.ReceiveFirmwareAnswer(firmwareStatusFrame);
				return HandlingResult.Handled;
			}
		}
		return HandlingResult.NotHandled;
	}

	private SendStatus SendGetCommand(SIPCOSMessage Message)
	{
		SendStatus sendStatus = SendStatus.TIMEOUT;
		m_ewh.Reset();
		sendStatus = Send(Message);
		if (sendStatus == SendStatus.ACK && !m_ewh.WaitOne((int)new TimeSpan(0, 0, 3).TotalMilliseconds, exitContext: false))
		{
			return SendStatus.TIMEOUT;
		}
		return sendStatus;
	}

	public SendStatus ForceDeviceToRestart(SIPcosHeader header, SendMode mode, byte[] ManufacturerCode, byte[] ManufacturerDeviceType, ref FirmwareStatusFrame status)
	{
		header.StayAwake = true;
		SendStatus sendStatus = SendStart(header, mode, ManufacturerCode, ManufacturerDeviceType, 0, new byte[4] { 0, 0, 0, 1 }, ref status);
		if (sendStatus == SendStatus.ACK && status.Status == FirmwareReplyStatus.ACK)
		{
			byte[] nextSequence = status.NextSequence;
			byte[] firmwareData = new byte[1];
			sendStatus = SendUpdateData(header, SendMode.Normal, nextSequence, firmwareData, ref status);
			if (sendStatus == SendStatus.ACK && status.Status == FirmwareReplyStatus.ACK)
			{
				sendStatus = SendDoUpdate(header, SendMode.Normal, ref status);
			}
		}
		return sendStatus;
	}

	public SendStatus SendGetInfo(SIPcosHeader header, SendMode Mode, ref FirmwareInfoFrame info)
	{
		SIPCOSMessage sIPCOSMessage = GenerateGetInfo(header);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			info.parse(m_data);
		}
		return sendStatus;
	}

	public SendStatus SendStart(SIPcosHeader header, SendMode Mode, byte[] ManufacturerCode, byte[] ManufacturerDeviceType, byte Version, byte[] size, ref FirmwareStatusFrame status)
	{
		SIPCOSMessage sIPCOSMessage = GenerateStart(header, ManufacturerCode, ManufacturerDeviceType, Version, size);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			status.parse(m_data);
		}
		return sendStatus;
	}

	public SendStatus SendUpdateData(SIPcosHeader header, SendMode Mode, byte[] SequenceNumber, byte[] firmwareData, ref FirmwareStatusFrame status)
	{
		SIPCOSMessage sIPCOSMessage = GenerateUpdateData(header, SequenceNumber, firmwareData);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			status.parse(m_data);
		}
		return sendStatus;
	}

	public SendStatus SendEnd(SIPcosHeader header, SendMode Mode, ref FirmwareStatusFrame status)
	{
		SIPCOSMessage sIPCOSMessage = GenerateEnd(header);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			status.parse(m_data);
		}
		return sendStatus;
	}

	public SendStatus SendDoUpdate(SIPcosHeader header, SendMode Mode, ref FirmwareStatusFrame status)
	{
		SIPCOSMessage sIPCOSMessage = GenerateDoUpdate(header);
		sIPCOSMessage.Mode = Mode;
		SendStatus sendStatus = SendGetCommand(sIPCOSMessage);
		if (sendStatus == SendStatus.ACK)
		{
			status.parse(m_data);
		}
		return sendStatus;
	}

	public SIPCOSMessage GenerateGetInfo(SIPcosHeader header)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.FIRMWARE_UPDATE;
		List<byte> list = new List<byte>();
		list.Add(4);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateStart(SIPcosHeader header, byte[] ManufacturerCode, byte[] ManufacturerDeviceType, byte Version, byte[] size)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.FIRMWARE_UPDATE;
		List<byte> list = new List<byte>();
		list.Add(0);
		list.AddRange(ManufacturerCode);
		list.AddRange(ManufacturerDeviceType);
		list.Add(Version);
		list.AddRange(size);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateUpdateData(SIPcosHeader header, byte[] SequenceNumber, byte[] firmwareData)
	{
		if (firmwareData.Length > 32)
		{
			throw new OverflowException("The max firmware packet size is " + (byte)32 + " and the size you are trying is " + firmwareData.Length);
		}
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.FIRMWARE_UPDATE;
		List<byte> list = new List<byte>();
		list.Add(1);
		list.AddRange(SequenceNumber);
		list.AddRange(firmwareData);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateEnd(SIPcosHeader header)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.FIRMWARE_UPDATE;
		List<byte> list = new List<byte>();
		list.Add(2);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateDoUpdate(SIPcosHeader header)
	{
		header.BiDi = true;
		header.FrameType = SIPcosFrameType.FIRMWARE_UPDATE;
		List<byte> list = new List<byte>();
		list.Add(3);
		return new SIPCOSMessage(header, list);
	}

	public uint GenerateFirmwareCRC(byte[] firmware)
	{
		crc crc = new crc();
		crc.CRC16_init();
		for (int i = 0; i < firmware.Length; i++)
		{
			crc.CRC16_update(firmware[i]);
		}
		return (uint)((crc.CRC16_High << 8) | crc.CRC16_Low);
	}

	public bool ValidateFirmware(byte[] firmware, uint Crc)
	{
		if (firmware != null)
		{
			uint num = GenerateFirmwareCRC(firmware);
			if (num == Crc)
			{
				return true;
			}
		}
		return false;
	}
}
