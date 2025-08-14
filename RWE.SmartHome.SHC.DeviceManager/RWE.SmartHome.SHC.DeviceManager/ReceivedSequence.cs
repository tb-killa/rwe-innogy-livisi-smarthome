using System;

namespace RWE.SmartHome.SHC.DeviceManager;

internal class ReceivedSequence
{
	public byte[] SourceAddress { get; set; }

	public byte SequenceCounter { get; set; }

	public DateTime Timestamp { get; set; }
}
