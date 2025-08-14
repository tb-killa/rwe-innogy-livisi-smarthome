using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosRouteManagementCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveRouteManagementFrame(SIPcosRouteManagementFrame Frame);

	public event ReceiveRouteManagementFrame ReceiveRouteManagement;

	public SIPcosRouteManagementCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.ROUTE_MANAGEMENT)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		if (header.FrameType == SIPcosFrameType.ROUTE_MANAGEMENT && this.ReceiveRouteManagement != null)
		{
			SIPcosRouteManagementFrame sIPcosRouteManagementFrame = new SIPcosRouteManagementFrame(header);
			sIPcosRouteManagementFrame.parse(ref message);
			this.ReceiveRouteManagement(sIPcosRouteManagementFrame);
			return HandlingResult.Handled;
		}
		return HandlingResult.NotHandled;
	}

	public SendStatus RequestRouterInfo(SIPcosHeader Header, RouteManagementAddressFamilyIdentifier addressFamily, byte[] address, SendMode Mode)
	{
		SIPCOSMessage sIPCOSMessage = GenerateRequestRouterInfo(Header, addressFamily, address);
		sIPCOSMessage.Mode = Mode;
		return Send(sIPCOSMessage);
	}

	public SIPCOSMessage GenerateRequestRouterInfo(SIPcosHeader header, RouteManagementAddressFamilyIdentifier addressFamily, byte[] address)
	{
		header.FrameType = SIPcosFrameType.ROUTE_MANAGEMENT;
		header.BiDi = true;
		List<byte> list = new List<byte>();
		list.Add(0);
		list.Add((byte)addressFamily);
		list.AddRange(address);
		return new SIPCOSMessage(header, list);
	}
}
