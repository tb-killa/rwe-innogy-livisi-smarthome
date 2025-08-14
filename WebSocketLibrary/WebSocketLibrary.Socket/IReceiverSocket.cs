using System;

namespace WebSocketLibrary.Socket;

public interface IReceiverSocket
{
	ushort GetUShortNumber();

	uint GetUIntNumber();

	ulong GetULongNumber();

	void GetBytes(ArraySegment<byte> segmentData, uint mask);

	int ReceiveBytes(ArraySegment<byte> segmentData, uint mask);

	void InsertBufferForReceive(byte[] buffer, int offset, int count);
}
