namespace RWE.SmartHome.SHC.HCI;

public class HciHeader
{
	public int MessageLength => (ushort)(4 + PayloadLength + (HasControlField(ControlField.TimeStampFieldAttached) ? 4 : 0)) + (HasControlField(ControlField.RSSIFieldAttached) ? 1 : 0) + (HasControlField(ControlField.CRC16FieldAttached) ? 2 : 0);

	public ControlField ControlField { get; set; }

	public EndpointIdentifier EndpointId { get; set; }

	public byte MessageId { get; set; }

	public byte PayloadLength { get; set; }

	public HciHeader()
	{
	}

	public HciHeader(byte[] message, byte start)
	{
		ControlField = (ControlField)((message[start] & 0xF0) >> 4);
		EndpointId = (EndpointIdentifier)(message[start] & 0xF);
		MessageId = message[1 + start];
		PayloadLength = message[2 + start];
	}

	public bool HasControlField(ControlField controlField)
	{
		return (ControlField & controlField) == controlField;
	}

	public byte[] ToArray()
	{
		return new byte[3]
		{
			(byte)(((byte)ControlField << 4) | (byte)EndpointId),
			MessageId,
			PayloadLength
		};
	}
}
