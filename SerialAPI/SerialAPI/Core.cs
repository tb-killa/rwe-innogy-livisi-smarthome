using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonFunctionality.Encryption;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialApiInterfaces;

namespace SerialAPI;

public sealed class Core : DebugInterface, IDisposable
{
	private const byte HEADER = 170;

	public static readonly byte[] BIDCosDefaultSync = new byte[2] { 233, 202 };

	public byte[] SIPCosDefaultSync = new byte[2] { 154, 125 };

	private crc m_checksum_send = new crc();

	private crc m_checksum_rev = new crc();

	private ISerialPort m_serial;

	private List<SerialHandler> m_handlers = new List<SerialHandler>();

	private List<byte> m_message_raw = new List<byte>();

	private List<byte> m_message_rev = new List<byte>();

	private SendStatus m_sendStatus;

	private MessageType m_status_type = MessageType.SERIAL_TYPE_COMMAND;

	private DateTime m_receive_time = DateTime.Now;

	private int m_watchDogCount;

	private byte m_dutyCycle = 200;

	private byte m_rssi;

	private byte m_last_seq_number;

	private byte m_next_seq_number = 1;

	private int[] m_resends = new int[10];

	private byte[] m_sync_word = new byte[2] { 154, 125 };

	private byte[] m_default_ip = new byte[3];

	private int m_serial_resend_count = 5;

	private EventWaitHandle acknowledgementReceivedEvent = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);

	private Thread m_serialDataHandler;

	private readonly BlockingQueue<CommunicationQueueData> m_serialDataQueue = new BlockingQueue<CommunicationQueueData>();

	private Dictionary<byte[], uint> m_sequence_count = new Dictionary<byte[], uint>(new ByteArrayComparer());

	public byte DutyCycle => m_dutyCycle;

	public byte RSSI => m_rssi;

	public byte[] SyncWord
	{
		get
		{
			return m_sync_word;
		}
		set
		{
			m_sync_word = value;
		}
	}

	public byte[] DefaultSyncWord
	{
		get
		{
			return SIPCosDefaultSync;
		}
		set
		{
			if (value != null && value.Length == 2)
			{
				SIPCosDefaultSync = value;
			}
		}
	}

	public byte[] DefaultIP
	{
		get
		{
			return m_default_ip;
		}
		set
		{
			if (value != null && value.Length == 3)
			{
				m_default_ip = value;
			}
		}
	}

	public int SerialResendCount
	{
		get
		{
			return m_serial_resend_count;
		}
		set
		{
			m_serial_resend_count = value;
		}
	}

	public Dictionary<byte[], uint> SequenceCounts
	{
		get
		{
			return m_sequence_count;
		}
		set
		{
			m_sequence_count.Clear();
			foreach (byte[] key in value.Keys)
			{
				m_sequence_count.Add(key, value[key]);
			}
		}
	}

	public Core(ISerialPort serialPort)
	{
		m_serialDataHandler = new Thread(HandleSerialDataThread);
		m_serialDataHandler.IsBackground = true;
		m_serialDataHandler.Name = "Serial Data Handler Thread";
		m_serial = serialPort;
		m_serial.ReceiveData += OnDataReceived;
		DebugInterface other = m_serial as DebugInterface;
		hookup(other);
	}

	public void Dispose()
	{
		m_serial.Dispose();
	}

	public void RemoveSequenceCount(byte[] ip)
	{
		if (m_sequence_count.ContainsKey(ip))
		{
			m_sequence_count.Remove(ip);
		}
	}

	public void RegisterHandler(SerialHandler handler)
	{
		if (!m_handlers.Contains(handler))
		{
			m_handlers.Add(handler);
		}
	}

	public void UnregisterHandler(SerialHandler handler)
	{
		if (m_handlers.Contains(handler))
		{
			m_handlers.Remove(handler);
		}
	}

	public bool Open(string port)
	{
		bool result = m_serial.Open(port);
		foreach (SerialHandler handler in m_handlers)
		{
			handler.HandleInit();
		}
		return result;
	}

	public bool Open(string port, Baudrate baudrate)
	{
		Log.Debug(Module.SerialCommunication, $"Opening port {port} @ {baudrate}bps");
		bool result = m_serial.Open(port, baudrate);
		foreach (SerialHandler handler in m_handlers)
		{
			handler.HandleInit();
		}
		return result;
	}

	public void Start()
	{
		m_serialDataHandler.Start();
	}

	private MessageSync GetSyncMode(byte[] sync)
	{
		if (sync.SequenceEqual(SIPCosDefaultSync))
		{
			return MessageSync.SIPCOS_DEFAULT;
		}
		if (sync.SequenceEqual(BIDCosDefaultSync))
		{
			return MessageSync.BIDCOS;
		}
		if (sync.SequenceEqual(m_sync_word))
		{
			return MessageSync.SIPCOS_NORMAL;
		}
		return MessageSync.SIPCOS_NORMAL;
	}

	private void GenerateDebugData(string txtraw)
	{
	}

	private SerialHandlerType GetHandlerType(MessageType msgType)
	{
		SerialHandlerType result = SerialHandlerType.RAW_HANDLER;
		switch (msgType)
		{
		case MessageType.SERIAL_TYPE_RAW:
			result = SerialHandlerType.RAW_HANDLER;
			break;
		case MessageType.SERIAL_TYPE_SIPCOS:
			result = SerialHandlerType.SIPCOS_HANDLER;
			break;
		case MessageType.SERIAL_TYPE_COMMAND:
			result = SerialHandlerType.COMMAND_HANDLER;
			break;
		}
		return result;
	}

	private void ProcessMessageDetails(MessageMode msgMode, MessageType msgType, List<byte> data)
	{
		switch (msgMode)
		{
		case MessageMode.SERIAL_MODE_RECEIVED:
			if (msgType != MessageType.SERIAL_TYPE_COMMAND)
			{
				m_dutyCycle = data[2];
				m_rssi = data[3];
			}
			break;
		case MessageMode.SERIAL_MODE_STATUS:
		{
			SerialStatus status = (SerialStatus)data[2];
			m_sendStatus = ConvertSerialToSendStatus(status);
			m_dutyCycle = data[3];
			break;
		}
		}
	}

	private bool IsNewSequence(List<byte> data)
	{
		if (m_last_seq_number != data[0])
		{
			m_last_seq_number = data[0];
			return true;
		}
		return false;
	}

	private bool ParseMessageHeader(List<byte> data, out MessageType msgType, out MessageMode msgMode)
	{
		msgType = (MessageType)(data[1] & 0xF);
		if ((int)msgType > 3)
		{
			m_message_rev.Clear();
			msgMode = MessageMode.SERIAL_MODE_RECEIVED;
			return false;
		}
		msgMode = (MessageMode)(data[1] & 0xF0);
		ProcessMessageDetails(msgMode, msgType, data);
		return true;
	}

	private void OnDataReceived(List<byte> data)
	{
		try
		{
			if (data.Count < 5 || data[3] > data.Count - 4)
			{
				return;
			}
			MessageSync syncMode = GetSyncMode(data.GetRange(1, 2).ToArray());
			m_message_rev = data.GetRange(4, data[3]);
			if (!IsNewSequence(m_message_rev))
			{
				return;
			}
			SerialHandlerType type = SerialHandlerType.RAW_HANDLER;
			int num = 0;
			MessageType msgType = MessageType.SERIAL_TYPE_RES;
			MessageMode msgMode = MessageMode.SERIAL_MODE_RECEIVED;
			if (m_message_rev.Count > 1)
			{
				if (!ParseMessageHeader(m_message_rev, out msgType, out msgMode))
				{
					return;
				}
				if (syncMode == MessageSync.BIDCOS)
				{
					type = SerialHandlerType.BIDCOS_HANDLER;
					if (m_message_rev.Count >= 5 && m_message_rev[4] == 2)
					{
						acknowledgementReceivedEvent.Set();
					}
				}
				else
				{
					type = GetHandlerType(msgType);
				}
				num = ((msgMode != MessageMode.SERIAL_MODE_RECEIVED || msgType == MessageType.SERIAL_TYPE_COMMAND) ? 2 : 4);
			}
			else
			{
				Log.Warning(Module.SipCosProtocolAdapter, "Invalid SipCos frame: " + data.ToArray().ToReadable());
			}
			if (msgMode == MessageMode.SERIAL_MODE_STATUS && msgType != MessageType.SERIAL_TYPE_SERIAL)
			{
				acknowledgementReceivedEvent.Set();
				Log.Debug(Module.SerialCommunication, $"Message mode is {msgMode}, message type is {msgType}, no need to handle the data");
			}
			else
			{
				HandleData(type, m_message_rev.GetRange(num, m_message_rev.Count - num));
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.SipCosProtocolAdapter, $"Unexpected failure processing frame [{data.ToArray().ToReadable()}]. Exception: {arg}");
		}
	}

	private void SendSerialStatus(SerialHandlerType type, SerialStatus status)
	{
		List<byte> list = new List<byte>();
		list.Add((byte)status);
		list.Add(0);
		SendSerialI(MessageMode.SERIAL_MODE_STATUS, type, UseDefaultSyncWord: true, SendMode.Normal, m_last_seq_number, list, expectReply: false);
		Thread.Sleep(40);
	}

	private void HandleData(SerialHandlerType type, List<byte> data)
	{
		m_serialDataQueue.Enqueue(new CommunicationQueueData
		{
			HandlerType = type,
			PayloadData = ((data == null) ? new List<byte>() : new List<byte>(data)),
			ReceiveTime = DateTime.UtcNow
		});
	}

	private void HandleSerialDataThread()
	{
		Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
		while (true)
		{
			CommunicationQueueData newData = m_serialDataQueue.Dequeue();
			CORESTACKHeader cORESTACKHeader = new CORESTACKHeader();
			cORESTACKHeader.RSSI = RSSI;
			bool flag = true;
			if (newData.HandlerType != SerialHandlerType.COMMAND_HANDLER && newData.HandlerType != SerialHandlerType.BIDCOS_HANDLER && newData.PayloadData.Count > 7)
			{
				try
				{
					cORESTACKHeader.parse(newData.PayloadData);
					if (cORESTACKHeader.MacSecurity && cORESTACKHeader.MacSecuritySequenceCount != 0)
					{
						if (m_sequence_count.ContainsKey(cORESTACKHeader.MacSource))
						{
							if (m_sequence_count[cORESTACKHeader.MacSource] < cORESTACKHeader.MacSecuritySequenceCount)
							{
								m_sequence_count[cORESTACKHeader.MacSource] = cORESTACKHeader.MacSecuritySequenceCount;
							}
							else if (m_sequence_count[cORESTACKHeader.MacSource] == cORESTACKHeader.MacSecuritySequenceCount)
							{
								flag = false;
							}
							else
							{
								SequenceProblem(cORESTACKHeader.MacSource, m_sequence_count[cORESTACKHeader.MacSource], cORESTACKHeader.MacSecuritySequenceCount);
								if (m_sequence_count[cORESTACKHeader.MacSource] <= cORESTACKHeader.MacSecuritySequenceCount)
								{
									m_sequence_count[cORESTACKHeader.MacSource] = cORESTACKHeader.MacSecuritySequenceCount;
								}
								else
								{
									flag = false;
								}
							}
						}
						else
						{
							m_sequence_count[cORESTACKHeader.MacSource] = cORESTACKHeader.MacSecuritySequenceCount;
						}
					}
				}
				catch (Exception)
				{
					Log.Warning(Module.DeviceCommunication, "Error parsing frame header. Frame data {0}", BitConverter.ToString(newData.PayloadData.ToArray()));
				}
			}
			foreach (SerialHandler item in m_handlers.Where((SerialHandler h) => h.HandlerType == newData.HandlerType))
			{
				try
				{
					item.HandleFrame(new List<byte>(newData.PayloadData), newData.ReceiveTime);
				}
				catch (Exception ex2)
				{
					Log.Error(Module.DeviceCommunication, string.Format("RAW data: {0}, {1}", BitConverter.ToString(newData.PayloadData.ToArray()).Replace("-", " "), ex2.ToString()));
				}
				if (flag)
				{
					try
					{
						item.Handle(cORESTACKHeader, newData.PayloadData.GetRange(cORESTACKHeader.HeaderSize, newData.PayloadData.Count - cORESTACKHeader.HeaderSize));
					}
					catch (Exception ex3)
					{
						Log.Error(Module.DeviceCommunication, string.Format("RAW data: {0}, {1}", BitConverter.ToString(newData.PayloadData.ToArray()).Replace("-", " "), ex3.ToString()));
					}
				}
			}
		}
	}

	public SendStatus WriteSerial(SerialHandlerType type, SendMode Mode, List<byte> data)
	{
		return SendSerial(MessageMode.SERIAL_MODE_WRITE, type, Mode, data);
	}

	public SendStatus WriteSerial(SerialHandlerType type, bool UseDefaultSyncWord, SendMode Mode, List<byte> data)
	{
		return SendSerial(MessageMode.SERIAL_MODE_WRITE, type, UseDefaultSyncWord, Mode, data);
	}

	public SendStatus WriteSerialWithoutReply(SerialHandlerType type, bool UseDefaultSyncWord, SendMode Mode, List<byte> data)
	{
		return SendSerialWithoutReply(MessageMode.SERIAL_MODE_WRITE, type, UseDefaultSyncWord, Mode, data);
	}

	public SendStatus RegisterSerial(SerialHandlerType type, SendMode Mode, List<byte> data)
	{
		return SendSerial(MessageMode.SERIAL_MODE_REGISTER_READ, type, Mode, data, exp: false);
	}

	public SendStatus UnregisterSerial(SerialHandlerType type, SendMode Mode, List<byte> data)
	{
		return SendSerial(MessageMode.SERIAL_MODE_UNREGISTER_READ, type, Mode, data, exp: false);
	}

	private SendStatus checkStatus(SendStatus stat, ref int count)
	{
		switch (stat)
		{
		case SendStatus.BUSY:
		case SendStatus.TIMEOUT:
		case SendStatus.SERIAL_TIMEOUT:
		case SendStatus.MEDIUM_BUSY:
		case SendStatus.CRC_ERROR:
			Thread.Sleep(300);
			break;
		default:
			count = 0;
			break;
		}
		return stat;
	}

	private SendStatus SendSerial(MessageMode mode, SerialHandlerType type, SendMode sendMode, List<byte> data, bool exp)
	{
		return SendSerial(mode, type, UseDefaultSyncWord: true, sendMode, data, exp);
	}

	private SendStatus SendSerial(MessageMode Mode, SerialHandlerType Type, SendMode SendMode, List<byte> data)
	{
		return SendSerial(Mode, Type, UseDefaultSyncWord: true, SendMode, data, expectReply: true);
	}

	private SendStatus SendSerial(MessageMode Mode, SerialHandlerType Type, bool UseDefaultSyncWord, SendMode SendMode, List<byte> data)
	{
		return SendSerial(Mode, Type, UseDefaultSyncWord, SendMode, data, expectReply: true);
	}

	private SendStatus SendSerialWithoutReply(MessageMode Mode, SerialHandlerType Type, bool UseDefaultSyncWord, SendMode SendMode, List<byte> data)
	{
		return SendSerial(Mode, Type, UseDefaultSyncWord, SendMode, data, expectReply: false);
	}

	private SendStatus SendSerial(MessageMode Mode, SerialHandlerType Type, bool UseDefaultSyncWord, SendMode SendMode, List<byte> data, bool expectReply)
	{
		if ((UseDefaultSyncWord && data.Count > 64) || (!UseDefaultSyncWord && data.Count > 56))
		{
			debug("Message too long");
			return SendStatus.ERROR;
		}
		int count = m_serial_resend_count;
		SendStatus sendStatus = SendStatus.ACK;
		do
		{
			sendStatus = SendSerialI(Mode, Type, UseDefaultSyncWord, SendMode, m_next_seq_number++, data, expectReply);
			checkStatus(sendStatus, ref count);
		}
		while (count-- > 0 && sendStatus != SendStatus.ACK);
		return sendStatus;
	}

	private SendStatus ConvertSerialToSendStatus(SerialStatus Status)
	{
		SendStatus sendStatus = SendStatus.ACK;
		return Status switch
		{
			SerialStatus.SERIAL_STATUS_ACK => SendStatus.ACK, 
			SerialStatus.SERIAL_STATUS_BUSY => SendStatus.BUSY, 
			SerialStatus.SERIAL_STATUS_TIMEOUT => SendStatus.MEDIUM_BUSY, 
			SerialStatus.SERIAL_STATUS_NO_REPLY => SendStatus.NO_REPLY, 
			SerialStatus.SERIAL_STATUS_ERROR => SendStatus.ERROR, 
			SerialStatus.SERIAL_STATUS_CRC_ERROR => SendStatus.CRC_ERROR, 
			SerialStatus.SERIAL_STATUS_DUTU_CYCLE => SendStatus.DUTY_CYCLE, 
			SerialStatus.SERIAL_STATUS_INCOMMING => SendStatus.INCOMMING, 
			SerialStatus.SERIAL_STATUS_MULTI_CAST => SendStatus.MULTI_CAST, 
			_ => SendStatus.ERROR, 
		};
	}

	private SendStatus SendSerialI(MessageMode Mode, SerialHandlerType Type, bool UseDefaultSyncWord, SendMode SendMode, byte seq, List<byte> data, bool expectReply)
	{
		acknowledgementReceivedEvent.Reset();
		m_checksum_send.CRC16_init();
		List<byte> list = new List<byte>();
		list.Add(170);
		if (SendMode != SendMode.Normal)
		{
			list.Add(170);
			if (SendMode == SendMode.TripleBurst)
			{
				list.Add(170);
			}
		}
		if (Type == SerialHandlerType.BIDCOS_HANDLER)
		{
			list.AddRange(BIDCosDefaultSync);
		}
		else if (UseDefaultSyncWord)
		{
			list.AddRange(SIPCosDefaultSync);
		}
		else
		{
			list.AddRange(m_sync_word);
		}
		list.Add((byte)(data.Count + 2));
		m_checksum_send.CRC16_update((byte)(data.Count + 2));
		list.Add(seq);
		m_checksum_send.CRC16_update(seq);
		byte b = (byte)Mode;
		switch (Type)
		{
		case SerialHandlerType.BIDCOS_HANDLER:
			b = b;
			break;
		case SerialHandlerType.RAW_HANDLER:
			b = b;
			break;
		case SerialHandlerType.SIPCOS_HANDLER:
			b |= 1;
			break;
		case SerialHandlerType.COMMAND_HANDLER:
			b |= 2;
			break;
		}
		list.Add(b);
		m_checksum_send.CRC16_update(b);
		for (int i = 0; i < data.Count; i++)
		{
			list.Add(data[i]);
			m_checksum_send.CRC16_update(data[i]);
		}
		list.Add(m_checksum_send.CRC16_High);
		list.Add(m_checksum_send.CRC16_Low);
		m_status_type = MessageType.SERIAL_TYPE_COMMAND;
		m_sendStatus = ConvertSerialToSendStatus((SerialStatus)m_serial.Send(list));
		if (expectReply)
		{
			m_status_type = MessageType.SERIAL_TYPE_SERIAL;
			if (Type != SerialHandlerType.COMMAND_HANDLER && m_sendStatus == SendStatus.ACK)
			{
				int seconds = 4;
				switch (SendMode)
				{
				case SendMode.Burst:
					seconds = 10;
					break;
				case SendMode.TripleBurst:
					seconds = 20;
					break;
				}
				if (m_status_type == MessageType.SERIAL_TYPE_SERIAL && !acknowledgementReceivedEvent.WaitOne((int)new TimeSpan(0, 0, seconds).TotalMilliseconds, exitContext: false))
				{
					CountWatchDog();
					return SendStatus.SERIAL_TIMEOUT;
				}
				if (m_sendStatus == SendStatus.BUSY || m_sendStatus == SendStatus.ERROR)
				{
					CountWatchDog();
				}
				else
				{
					m_watchDogCount = 0;
				}
			}
			return m_sendStatus;
		}
		return m_sendStatus;
	}

	private void m_serial_ReceiveDebugData(string data)
	{
		debug(data);
	}

	private void CountWatchDog()
	{
		m_watchDogCount++;
		if (m_watchDogCount >= 3)
		{
			m_watchDogCount = 0;
			WatchDog();
		}
	}
}
