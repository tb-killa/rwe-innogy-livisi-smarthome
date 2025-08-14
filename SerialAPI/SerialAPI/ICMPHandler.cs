using System.Collections.Generic;

namespace SerialAPI;

public class ICMPHandler : SerialHandler
{
	public delegate void ReceiveICMPData(ICMPMessage Message);

	public event ReceiveICMPData ReceiveData;

	public ICMPHandler(Core core)
		: base(SerialHandlerType.SIPCOS_HANDLER, core)
	{
	}

	protected override void HandleHeader(CORESTACKHeader header, List<byte> data)
	{
		if (header.CorestackFrameType == CorestackFrameType.COMPRESSED_ICMP && this.ReceiveData != null)
		{
			ICMPMessage iCMPMessage = new ICMPMessage(header);
			iCMPMessage.Parse(ref data);
			this.ReceiveData(iCMPMessage);
		}
	}

	public SendStatus SendICMPMessage(CORESTACKHeader header, SendMode Mode, byte OperationMode, byte Rssi, ICMP_type Type)
	{
		CORESTACKMessage cORESTACKMessage = GenerateICMPMessage(header, OperationMode, Rssi, Type);
		List<byte> headerAsSerial = header.getHeaderAsSerial();
		headerAsSerial.AddRange(cORESTACKMessage.Data);
		return BroadcastFrameToAir(headerAsSerial, Mode);
	}

	public CORESTACKMessage GenerateICMPMessage(CORESTACKHeader header, byte OperationMode, byte Rssi, ICMP_type Type)
	{
		header.CorestackFrameType = CorestackFrameType.COMPRESSED_ICMP;
		List<byte> list = new List<byte>();
		list.Add(OperationMode);
		list.Add(Rssi);
		list.Add((byte)Type);
		return new CORESTACKMessage(header, list);
	}
}
