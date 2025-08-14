using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class SIPcosRouteManagementCommandHandlerExt : SIPcosRouteManagementCommandHandler
{
	public SIPcosRouteManagementCommandHandlerExt(SIPcosHandler handler)
		: base(handler)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		byte[] array = message.ToArray();
		try
		{
			if (header.FrameType == SIPcosFrameType.ROUTE_MANAGEMENT && message.Count > 0)
			{
				byte item = message[0];
				while (MessageContainsValidRoutingInfo(message))
				{
					base.Handle(header, message);
					message.Insert(0, item);
				}
				return HandlingResult.Handled;
			}
		}
		catch (Exception ex)
		{
			string text = (header.IpSource.SequenceEqual(header.MacSource) ? $"address {header.Source.ToReadable()}" : $"IpSource {header.IpSource.ToReadable()} and MacSource {header.MacSource.ToReadable()}");
			Log.Error(Module.DeviceCommunication, string.Format("Handling of the {0} from {1} with sequence number {2} failed with the following error: {3}.\nReceived frame data: {4}", header.FrameType, text, header.SequenceNumber, ex, BitConverter.ToString(array).Replace("-", " ")));
		}
		return HandlingResult.NotHandled;
	}

	private bool MessageContainsValidRoutingInfo(List<byte> message)
	{
		if (message.Count >= 10 && message[2] != 0 && message[3] != 0 && message[4] != 0)
		{
			return true;
		}
		return false;
	}
}
