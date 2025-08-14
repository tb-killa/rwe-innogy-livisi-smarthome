using WebSocketLibrary.Exceptions;
using WebSocketLibrary.Frames;
using WebSocketLibrary.Helpers;
using WebSocketLibrary.Socket;

namespace WebSocketLibrary.Handlers.Frames;

public class FrameParser
{
	public FrameHeader GetHeaderFromData(ushort data)
	{
		return FrameHeaderHelper.GetFrameHeader(data);
	}

	public ulong GetPayloadLength(FrameHeader header, IReceiverSocket receiver)
	{
		PayloadLengthType payloadLengthType = FrameLengthHelper.GetPayloadLengthType(header.Length);
		return payloadLengthType switch
		{
			PayloadLengthType.Byte => header.Length, 
			PayloadLengthType.Short => receiver.GetUShortNumber(), 
			PayloadLengthType.Long => receiver.GetULongNumber(), 
			_ => throw new FrameException($"Unknown PayloadLengthType: {payloadLengthType}"), 
		};
	}

	public uint GetMaskIfExist(FrameHeader header, IReceiverSocket receiver)
	{
		if (!header.IsMasked)
		{
			return 0u;
		}
		return receiver.GetUIntNumber();
	}
}
