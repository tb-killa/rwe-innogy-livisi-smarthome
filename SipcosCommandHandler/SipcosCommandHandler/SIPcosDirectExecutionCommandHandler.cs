using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosDirectExecutionCommandHandler : SIPcosCommandHandler
{
	public SIPcosDirectExecutionCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.DIRECT_EXECUTION)
	{
	}

	private SIPCOSMessage GenerateSimpleDirectCommand(SIPcosHeader header, SipCosDirectExecutionCommands Command, byte Channel)
	{
		header.FrameType = SIPcosFrameType.DIRECT_EXECUTION;
		List<byte> list = new List<byte>();
		list.Add((byte)Command);
		list.Add(Channel);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateLockCommand(SIPcosHeader header, byte Channel)
	{
		return GenerateSimpleDirectCommand(header, SipCosDirectExecutionCommands.Lock, Channel);
	}

	public SIPCOSMessage GenerateUnlockCommand(SIPcosHeader header, byte Channel)
	{
		return GenerateSimpleDirectCommand(header, SipCosDirectExecutionCommands.Unlock, Channel);
	}

	public SIPCOSMessage GenerateRampStopCommand(SIPcosHeader header, SendMode Mode, byte Channel)
	{
		return GenerateSimpleDirectCommand(header, SipCosDirectExecutionCommands.RampStop, Channel);
	}

	private int calcDEtime(int time)
	{
		int num = 11;
		int num2 = 5;
		int num3 = 0;
		long num4 = time;
		long num5 = (uint)(-1 << num);
		int i;
		for (i = 0; i < 32 - num; i++)
		{
			if ((num4 & num5) == 0)
			{
				break;
			}
			num4 >>= 1;
		}
		return (i << num3) | (int)(num4 << num2);
	}

	public SIPCOSMessage GenerateRampStartCommand(SIPcosHeader header, byte Channel, byte Level, int RampTime)
	{
		SIPCOSMessage sIPCOSMessage = GenerateSimpleDirectCommand(header, SipCosDirectExecutionCommands.RampStart, Channel);
		sIPCOSMessage.Data.Add(Level);
		int num = calcDEtime(RampTime);
		sIPCOSMessage.Data.Add((byte)(num >> 8));
		sIPCOSMessage.Data.Add((byte)(num & 0xFF));
		return sIPCOSMessage;
	}

	public SIPCOSMessage GenerateRampStartCommand(SIPcosHeader header, byte Channel, byte Level, int RampTime, int OnTime)
	{
		SIPCOSMessage sIPCOSMessage = GenerateRampStartCommand(header, Channel, Level, RampTime);
		int num = calcDEtime(OnTime);
		sIPCOSMessage.Data.Add((byte)(num >> 8));
		sIPCOSMessage.Data.Add((byte)(num & 0xFF));
		return sIPCOSMessage;
	}
}
