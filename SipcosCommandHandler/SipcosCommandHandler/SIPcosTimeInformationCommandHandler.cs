using System;
using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosTimeInformationCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveTimeInformationFrame(SIPcosTimeInformationFrame timeFrame);

	public event ReceiveTimeInformationFrame ReceiveTime;

	public SIPcosTimeInformationCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.TIME_INFORMATION)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		if (header.FrameType == SIPcosFrameType.TIME_INFORMATION && this.ReceiveTime != null)
		{
			SIPcosTimeInformationFrame sIPcosTimeInformationFrame = new SIPcosTimeInformationFrame(header);
			sIPcosTimeInformationFrame.parse(ref message);
			this.ReceiveTime(sIPcosTimeInformationFrame);
			return HandlingResult.Handled;
		}
		return HandlingResult.NotHandled;
	}

	public SendStatus SendTimeInformationRequestCommand(SIPcosHeader header, SIPcosTimeInforamtionMode TimeMode, SendMode Mode)
	{
		SIPCOSMessage sIPCOSMessage = GenerateTimeInformationRequestCommand(header, TimeMode);
		sIPCOSMessage.Mode = Mode;
		return Send(sIPCOSMessage);
	}

	public SendStatus SendTimeInformationCommand(SIPcosHeader header, SIPcosTimeInforamtionMode TimeMode, SendMode Mode)
	{
		return SendTimeInformationCommand(header, TimeMode, DateTime.Now, Mode);
	}

	public SendStatus SendTimeInformationCommand(SIPcosHeader header, SIPcosTimeInforamtionMode TimeMode, DateTime Time, SendMode Mode)
	{
		SIPCOSMessage sIPCOSMessage = GenerateTimeInfomationCommand(header, TimeMode, Time);
		sIPCOSMessage.Mode = Mode;
		return Send(sIPCOSMessage);
	}

	public SIPCOSMessage GenerateTimeInformationRequestCommand(SIPcosHeader header, SIPcosTimeInforamtionMode Mode)
	{
		header.FrameType = SIPcosFrameType.TIME_INFORMATION;
		header.BiDi = true;
		List<byte> list = new List<byte>();
		list.Add((byte)Mode);
		return new SIPCOSMessage(header, list);
	}

	public SIPCOSMessage GenerateTimeInfomationCommand(SIPcosHeader header, SIPcosTimeInforamtionMode Mode)
	{
		return GenerateTimeInformation(header, Mode, DateTime.Now);
	}

	public SIPCOSMessage GenerateTimeInfomationCommand(SIPcosHeader header, SIPcosTimeInforamtionMode Mode, DateTime Time)
	{
		return GenerateTimeInformation(header, Mode, Time);
	}

	private SIPCOSMessage GenerateTimeInformation(SIPcosHeader header, SIPcosTimeInforamtionMode Mode, DateTime time)
	{
		header.FrameType = SIPcosFrameType.TIME_INFORMATION;
		List<byte> list = new List<byte>(7);
		list.Add((byte)Mode);
		list.Insert(1, (byte)(time.Year % 100));
		list.Insert(2, (byte)time.Month);
		list.Insert(3, (byte)time.Day);
		switch (time.DayOfWeek)
		{
		case DayOfWeek.Monday:
			list[3] += 32;
			break;
		case DayOfWeek.Tuesday:
			list[3] += 64;
			break;
		case DayOfWeek.Wednesday:
			list[3] += 96;
			break;
		case DayOfWeek.Thursday:
			list[3] += 128;
			break;
		case DayOfWeek.Friday:
			list[3] += 160;
			break;
		case DayOfWeek.Saturday:
			list[3] += 192;
			break;
		case DayOfWeek.Sunday:
			list[3] += 224;
			break;
		}
		list.Insert(4, (byte)time.Hour);
		list.Insert(5, (byte)time.Minute);
		list.Insert(6, (byte)time.Second);
		return new SIPCOSMessage(header, list);
	}
}
