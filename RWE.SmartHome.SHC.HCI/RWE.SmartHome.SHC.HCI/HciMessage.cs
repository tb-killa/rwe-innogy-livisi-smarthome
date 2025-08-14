using System;

namespace RWE.SmartHome.SHC.HCI;

public class HciMessage
{
	private const byte sof = 165;

	private byte[] payload;

	public HciHeader Header { get; set; }

	public byte[] Payload
	{
		get
		{
			return payload;
		}
		set
		{
			payload = value;
			if (Header != null)
			{
				Header.PayloadLength = (byte)payload.Length;
			}
		}
	}

	public int TimeStamp { get; set; }

	public byte RSSI { get; set; }

	public ushort FCS { get; set; }

	public HciMessage()
	{
	}

	public HciMessage(byte[] message)
	{
		if (message[0] != 165)
		{
			throw new ArgumentException("HciMessage does not start with HCI start of frame identifier.");
		}
		Header = new HciHeader(message, 1);
		if (message.Length > 4)
		{
			bool flag = Header.HasControlField(ControlField.TimeStampFieldAttached);
			bool flag2 = Header.HasControlField(ControlField.RSSIFieldAttached);
			bool flag3 = Header.HasControlField(ControlField.CRC16FieldAttached);
			Payload = new byte[Header.PayloadLength];
			Array.Copy(message, 4, Payload, 0, Header.PayloadLength);
			ushort num = (ushort)(Header.PayloadLength + 4);
			if (flag)
			{
				TimeStamp = BitConverter.ToInt32(message, num);
				num += 4;
			}
			if (flag2)
			{
				RSSI = message[num];
				num++;
			}
			if (flag3)
			{
				FCS = BitConverter.ToUInt16(message, num);
			}
		}
	}

	public byte[] ToArray()
	{
		bool flag = Header.HasControlField(ControlField.TimeStampFieldAttached);
		bool flag2 = Header.HasControlField(ControlField.RSSIFieldAttached);
		bool flag3 = Header.HasControlField(ControlField.CRC16FieldAttached);
		ushort num = (ushort)(4 + Header.PayloadLength + (flag ? 4 : 0) + (flag2 ? 1 : 0) + (flag3 ? 2 : 0));
		byte[] array = new byte[num];
		array[0] = 165;
		Array.Copy(Header.ToArray(), 0, array, 1, 3);
		Array.Copy(Payload, 0, array, 4, Header.PayloadLength);
		byte b = (byte)(4 + Header.PayloadLength);
		if (flag)
		{
			byte[] bytes = BitConverter.GetBytes(TimeStamp);
			Array.Copy(bytes, 0, array, b, bytes.Length);
			b += 4;
		}
		if (flag2)
		{
			array[b] = RSSI;
			b++;
		}
		if (flag3)
		{
			byte[] bytes2 = BitConverter.GetBytes(FCS);
			Array.Copy(bytes2, 0, array, b, bytes2.Length);
		}
		return array;
	}
}
