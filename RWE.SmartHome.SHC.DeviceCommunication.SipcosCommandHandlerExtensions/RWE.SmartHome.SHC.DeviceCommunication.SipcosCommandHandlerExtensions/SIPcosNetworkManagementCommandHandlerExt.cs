using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class SIPcosNetworkManagementCommandHandlerExt : SIPcosNetworkManagementCommandHandler
{
	public SIPcosNetworkManagementCommandHandlerExt(SIPcosHandler handler)
		: base(handler)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		try
		{
			return base.Handle(header, message);
		}
		catch (Exception ex)
		{
			string text = (header.IpSource.SequenceEqual(header.MacSource) ? $"address {header.Source.ToReadable()}" : $"IpSource {header.IpSource.ToReadable()} and MacSource {header.MacSource.ToReadable()}");
			Log.Error(Module.DeviceCommunication, $"Handling of the {header.FrameType} from {text} with sequence number {header.SequenceNumber} failed with the following error: {ex.Message}.\n{ex.StackTrace}.");
			throw new Exception();
		}
	}

	public SIPCOSMessage GenerateForwardNetworkInfoFrame(SIPcosHeader header, NetworkInfoFrameType type, byte[] sgtin, byte[] newDeviceIP)
	{
		SIPCOSMessage sIPCOSMessage = GenerateNetworkInfoFrame(header, type, sgtin);
		sIPCOSMessage.Data[0] = 3;
		sIPCOSMessage.Data.AddRange(newDeviceIP);
		return sIPCOSMessage;
	}
}
