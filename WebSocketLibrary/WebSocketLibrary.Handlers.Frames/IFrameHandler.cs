using System;
using WebSocketLibrary.Frames;

namespace WebSocketLibrary.Handlers.Frames;

public interface IFrameHandler
{
	void SendFrameIdentifier(FrameIdentifier frameIdentifier);

	void SendFramePayload(FrameIdentifier frameIdentifier, ArraySegment<byte> data);

	FrameIdentifier ReceiveNextFrameIdentifier();

	int ReceivePayload(FrameIdentifier frameIdentifier, ArraySegment<byte> data);
}
