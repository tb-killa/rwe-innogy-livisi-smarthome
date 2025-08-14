using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class ICMPHandlerExt : SerialHandler, IICMPHandler
{
	public delegate void ReceiveRawData(ICMPMessage Message);

	public event ReceiveRawData ReceiveData;

	public ICMPHandlerExt(SerialAPI.Core core)
		: base(SerialHandlerType.SIPCOS_HANDLER, core)
	{
	}

	protected override void HandleHeader(CORESTACKHeader header, List<byte> data)
	{
		if (header.CorestackFrameType == CorestackFrameType.COMPRESSED_ICMP && this.ReceiveData != null)
		{
			try
			{
				ICMPMessage iCMPMessage = new ICMPMessage(header);
				iCMPMessage.Parse(ref data);
				this.ReceiveData(iCMPMessage);
			}
			catch (Exception ex)
			{
				string arg = (header.IpSource.SequenceEqual(header.MacSource) ? $"address {header.IpSource.ToReadable()}" : $"IpSource {header.IpSource.ToReadable()} and MacSource {header.MacSource.ToReadable()}");
				Log.Error(Module.DeviceCommunication, $"Handling of the ICMP frame from {arg} failed with the following error: {ex.Message}.\n{ex}");
			}
		}
	}

	public SendStatus SendICMPMessage(CORESTACKMessage message)
	{
		List<byte> headerAsSerial = message.Header.getHeaderAsSerial();
		headerAsSerial.AddRange(message.Data);
		return BroadcastFrameToAir(headerAsSerial, message.Mode);
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
