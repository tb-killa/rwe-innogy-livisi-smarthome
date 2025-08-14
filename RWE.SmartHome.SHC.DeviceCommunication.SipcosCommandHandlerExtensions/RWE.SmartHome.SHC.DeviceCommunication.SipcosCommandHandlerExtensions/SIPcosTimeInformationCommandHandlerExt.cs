using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class SIPcosTimeInformationCommandHandlerExt : SIPcosTimeInformationCommandHandler
{
	public SIPcosTimeInformationCommandHandlerExt(SIPcosHandler handler)
		: base(handler)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		byte[] array = message.ToArray();
		try
		{
			return base.Handle(header, message);
		}
		catch (Exception ex)
		{
			string text = (header.IpSource.SequenceEqual(header.MacSource) ? $"address {header.Source.ToReadable()}" : $"IpSource {header.IpSource.ToReadable()} and MacSource {header.MacSource.ToReadable()}");
			Log.Error(Module.DeviceCommunication, string.Format("Handling of the {0} frame from {1} with sequence number {2} failed with the following error: {3}.\nReceived fram data: {4}", header.FrameType, text, header.SequenceNumber, ex, BitConverter.ToString(array).Replace("-", " ")));
		}
		return HandlingResult.NotHandled;
	}
}
