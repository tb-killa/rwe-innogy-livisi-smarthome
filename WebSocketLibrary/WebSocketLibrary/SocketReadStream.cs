using System;
using System.IO;
using WebSocketLibrary.Managers.Frames;

namespace WebSocketLibrary;

public class SocketReadStream : Stream
{
	private readonly IWebSocket socket;

	private bool readCompleted;

	public override bool CanRead => true;

	public override bool CanWrite => false;

	public override bool CanSeek => false;

	public override bool CanTimeout => true;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public SocketReadStream(IWebSocket aSocket)
	{
		socket = aSocket;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (readCompleted)
		{
			return 0;
		}
		ReceivedResult receivedResult = socket.ReceiveData(new ArraySegment<byte>(buffer, offset, count));
		readCompleted = receivedResult.IsFinalData;
		return receivedResult.ReadBytes;
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}
}
