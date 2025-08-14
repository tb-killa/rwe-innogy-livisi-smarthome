using System;
using System.Collections.Generic;
using System.Threading;

namespace SerialAPI;

public class CommandHandler : SerialHandler
{
	private EventWaitHandle m_ewh = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);

	private byte[] m_commandData = new byte[255];

	public CommandHandler(Core core)
		: base(SerialHandlerType.COMMAND_HANDLER, core)
	{
	}

	protected override void HandleData(List<byte> data, DateTime receiveTime)
	{
		data.CopyTo(m_commandData);
		m_ewh.Set();
	}

	internal override void HandleInit()
	{
		GetSyncWord(out var _);
		GetDefaultIP(out var _);
	}

	private SendStatus SendGetCommand(SerialCommands command)
	{
		return SendGetCommand(command, null);
	}

	private SendStatus SendGetCommand(SerialCommands command, List<byte> cmd_data)
	{
		List<byte> list = new List<byte>();
		list.Add((byte)command);
		if (cmd_data != null)
		{
			list.AddRange(cmd_data);
		}
		SendStatus sendStatus = SendStatus.TIMEOUT;
		m_ewh.Reset();
		sendStatus = register(list);
		if (sendStatus == SendStatus.ACK)
		{
			TimeSpan timeSpan = new TimeSpan(0, 0, 2);
			if (!m_ewh.WaitOne((int)timeSpan.TotalMilliseconds, exitContext: false))
			{
				return SendStatus.TIMEOUT;
			}
		}
		return sendStatus;
	}

	public SendStatus GetDefaultIP(out byte[] ip)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.DefaultIP);
		ip = new byte[3];
		if (sendStatus == SendStatus.ACK)
		{
			ip[0] = m_commandData[0];
			ip[1] = m_commandData[1];
			ip[2] = m_commandData[2];
			base.DefaultIP = ip;
		}
		return sendStatus;
	}

	public SendStatus SetDefaultIP(byte[] ip)
	{
		List<byte> list = new List<byte>();
		list.Add(0);
		list.AddRange(ip);
		SendStatus sendStatus = writeDefaultSync(list);
		if (sendStatus == SendStatus.ACK)
		{
			base.DefaultIP = new byte[3]
			{
				ip[0],
				ip[1],
				ip[2]
			};
		}
		return sendStatus;
	}

	public SendStatus FactoryReset()
	{
		List<byte> list = new List<byte>();
		list.Add(1);
		return writeDefaultSync(list);
	}

	public SendStatus GetDefaultNumberOfCSMACAAttempts(out byte number)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.CSMA_CA_Attempts);
		if (sendStatus == SendStatus.ACK)
		{
			number = m_commandData[0];
		}
		else
		{
			number = 0;
		}
		return sendStatus;
	}

	public SendStatus SetDefaultNumberOfCSMACAAttempts(byte number)
	{
		List<byte> list = new List<byte>();
		list.Add(2);
		list.Add(number);
		return writeDefaultSync(list);
	}

	public SendStatus GetDefaultNumberOfSendAttempts(out byte number)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.Send_Attempts);
		if (sendStatus == SendStatus.ACK)
		{
			number = m_commandData[0];
		}
		else
		{
			number = 0;
		}
		return sendStatus;
	}

	public SendStatus SetDefaultNumberOfSendAttempts(byte number)
	{
		List<byte> list = new List<byte>();
		list.Add(3);
		list.Add(number);
		return writeDefaultSync(list);
	}

	public SendStatus GetNetworkKey(NetworkKeyEncryptionType key, out byte[] networkKey)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.NetworkKey);
		networkKey = new byte[16];
		if (sendStatus == SendStatus.ACK)
		{
			for (int i = 0; i < 16; i++)
			{
				networkKey[i] = m_commandData[i];
			}
		}
		return sendStatus;
	}

	public SendStatus SetNetworkKey(byte[] networkKey)
	{
		List<byte> list = new List<byte>();
		list.Add(4);
		list.AddRange(networkKey);
		return writeDefaultSync(list);
	}

	public SendStatus GetDutyCycleLimit(out byte limit)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.DutyCycle);
		if (sendStatus == SendStatus.ACK)
		{
			limit = m_commandData[0];
		}
		else
		{
			limit = 0;
		}
		return sendStatus;
	}

	public SendStatus SetDutyCycleLimit(byte limit)
	{
		List<byte> list = new List<byte>();
		list.Add(5);
		list.Add(limit);
		return writeDefaultSync(list);
	}

	public SendStatus GetSyncWord(out byte[] SyncWord)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.SyncWord);
		SyncWord = new byte[2];
		if (sendStatus == SendStatus.ACK)
		{
			SyncWord[0] = m_commandData[0];
			SyncWord[1] = m_commandData[1];
			m_core.SyncWord = SyncWord;
		}
		return sendStatus;
	}

	public SendStatus SetSyncWord(byte[] NewSyncWord)
	{
		List<byte> list = new List<byte>();
		list.Add(6);
		list.Add(NewSyncWord[0]);
		list.Add(NewSyncWord[1]);
		base.SyncWord = NewSyncWord;
		return writeDefaultSync(list);
	}

	public SendStatus GetTransmitpower(out TransmitPower TransmitPower)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.TransmitPower);
		if (sendStatus == SendStatus.ACK)
		{
			TransmitPower = (TransmitPower)m_commandData[0];
		}
		else
		{
			TransmitPower = TransmitPower.Power_0;
		}
		return sendStatus;
	}

	public SendStatus SetTransmitPower(TransmitPower TransmitPower)
	{
		List<byte> list = new List<byte>();
		list.Add(7);
		list.Add((byte)TransmitPower);
		return writeDefaultSync(list);
	}

	public SendStatus GetVersion(out byte[] Version)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.Version);
		Version = new byte[5];
		if (sendStatus == SendStatus.ACK)
		{
			for (int i = 0; i < Version.Length; i++)
			{
				Version[i] = m_commandData[i];
			}
		}
		return sendStatus;
	}

	public SendStatus GetSequenceCount(out uint SequenceCount)
	{
		byte[] SequenceCount2;
		SendStatus sequenceCount = GetSequenceCount(out SequenceCount2);
		SequenceCount = 0u;
		for (int i = 0; i < SequenceCount2.Length; i++)
		{
			SequenceCount += SequenceCount2[i];
			SequenceCount <<= 8;
		}
		return sequenceCount;
	}

	public SendStatus GetSequenceCount(out byte[] SequenceCount)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.SequenceCount);
		SequenceCount = new byte[4];
		if (sendStatus == SendStatus.ACK)
		{
			for (int i = 0; i < SequenceCount.Length; i++)
			{
				SequenceCount[i] = m_commandData[i];
			}
		}
		return sendStatus;
	}

	public SendStatus SetSequenceCount(uint NewSequenceCount)
	{
		byte[] array = new byte[4];
		for (int num = array.Length - 1; num >= 0; num--)
		{
			array[num] = (byte)(NewSequenceCount & 0xFF);
			NewSequenceCount >>= 8;
		}
		return SetSequenceCount(array);
	}

	public SendStatus SetSequenceCount(byte[] Count)
	{
		List<byte> list = new List<byte>();
		list.Add(16);
		list.AddRange(Count);
		return writeDefaultSync(list);
	}

	public SendStatus GetFlashCRC(out byte[] CheckSum)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.FlashCRC);
		CheckSum = new byte[2];
		if (sendStatus == SendStatus.ACK)
		{
			for (int i = 0; i < CheckSum.Length; i++)
			{
				CheckSum[i] = m_commandData[i];
			}
		}
		return sendStatus;
	}

	public SendStatus GetPartner(byte Id, out SIPCosSerialPartner Partner)
	{
		List<byte> list = new List<byte>();
		list.Add(Id);
		return GetPartnerData(list, out Partner);
	}

	public SendStatus GetPartner(byte[] Ip, out SIPCosSerialPartner Partner)
	{
		List<byte> list = new List<byte>();
		list.AddRange(Ip);
		return GetPartnerData(list, out Partner);
	}

	private SendStatus GetPartnerData(List<byte> data, out SIPCosSerialPartner Partner)
	{
		SendStatus sendStatus = SendGetCommand(SerialCommands.Partners, data);
		Partner = default(SIPCosSerialPartner);
		if (sendStatus == SendStatus.ACK)
		{
			Partner.ip = new byte[3];
			Partner.router = new byte[3];
			Partner.OperationMode = m_commandData[3];
			for (int i = 0; i < 3; i++)
			{
				Partner.ip[i] = m_commandData[i];
				Partner.router[i] = m_commandData[4 + i];
			}
			if (Partner.ip[0] == byte.MaxValue)
			{
				Partner.ip[1] = byte.MaxValue;
				Partner.ip[2] = byte.MaxValue;
			}
		}
		return sendStatus;
	}

	public SendStatus SetPartner(SIPCosSerialPartner partner)
	{
		List<byte> list = new List<byte>();
		list.Add(9);
		list.AddRange(partner.ip);
		list.Add(partner.OperationMode);
		list.AddRange(partner.router);
		return writeDefaultSync(list);
	}

	public SendStatus RemovePartner(SIPCosSerialPartner partner)
	{
		return RemovePartner(partner.ip);
	}

	public SendStatus RemovePartner(byte[] ip)
	{
		List<byte> list = new List<byte>();
		list.Add(9);
		list.AddRange(ip);
		return unregister(list);
	}

	public SendStatus RemovePartner(byte id)
	{
		List<byte> list = new List<byte>();
		list.Add(9);
		list.Add(id);
		return unregister(list);
	}

	public SendStatus RegisterIP(byte[] ip)
	{
		List<byte> list = new List<byte>();
		list.Add(18);
		list.AddRange(ip);
		return writeDefaultSync(list);
	}

	public SendStatus UnregisterIP(byte[] ip)
	{
		List<byte> list = new List<byte>();
		list.Add(18);
		list.AddRange(ip);
		return unregister(list);
	}

	public SendStatus GetRegisteredIPS(ref List<byte[]> ips)
	{
		for (int i = 0; i < m_commandData.Length; i++)
		{
			m_commandData[i] = 0;
		}
		SendStatus sendStatus = SendGetCommand(SerialCommands.RegisteredIP);
		ips = new List<byte[]>();
		if (sendStatus == SendStatus.ACK)
		{
			for (int j = 0; j < m_commandData.Length; j += 3)
			{
				if (m_commandData[j] != 0 && m_commandData[j + 1] != 0 && m_commandData[j + 2] != 0)
				{
					byte[] item = new byte[3]
					{
						m_commandData[j],
						m_commandData[j + 1],
						m_commandData[j + 2]
					};
					ips.Add(item);
				}
			}
		}
		return sendStatus;
	}
}
