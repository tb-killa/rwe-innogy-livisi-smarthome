using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosLevelCommandHandler : SIPcosCommandHandler
{
	public SIPcosLevelCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.LEVEL_COMMAND)
	{
	}

	public SIPCOSMessage GenerateLevelCommand(SIPcosHeader header, byte Channel, byte Count, byte Trigger, bool LongPress)
	{
		header.FrameType = SIPcosFrameType.LEVEL_COMMAND;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		if (LongPress)
		{
			list[0] += 64;
		}
		list.Add(Count);
		list.Add(Trigger);
		return new SIPCOSMessage(header, list);
	}
}
