using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;

public class SIPCosNetworkParameter
{
	public const string SYNCWORD_COLUMN = "SyncWord";

	public const byte SYNCWORD_SIZE = 2;

	public const string NETWORKKEY_COLUMN = "NetworkKey";

	public const byte NETWORKKEY_SIZE = 16;

	public const string SEQUENCE_COUNTER_COLUMN = "SequenceCounter";

	public const string SEQUENCE_COUNTER_SAVE_TIME_COLUMN = "SequenceCounterSaveTime";

	public const string TABLE = "SipCosNetworkParameter";

	public const string ADDRESS_COLUMN = "Address";

	public const string TABLE_ADDRESSES = "ShcAddresses";

	public const byte ADDRESS_SIZE = 3;

	public byte[] SyncWord { get; set; }

	public byte[] NetworkKey { get; set; }

	public uint? SequenceCounter { get; set; }

	public DateTime? SequenceCounterSaveTime { get; set; }

	public List<byte[]> ShcAddresses { get; set; }
}
