using System;
using System.IO;
using System.Text;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using WebSocketLibrary.Managers.Frames;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class SocketReadStream : Stream
{
	private readonly WebSocketManager socket;

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

	public SocketReadStream(WebSocketManager aSocket)
	{
		socket = aSocket;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (readCompleted)
		{
			return 0;
		}
		Log.Debug(Module.RelayDriver, "Reading from socket.");
		ReceivedResult receivedResult = socket.ReceiveData(new ArraySegment<byte>(buffer, offset, count));
		Log.Debug(Module.RelayDriver, "Read from socket " + Encoding.UTF8.GetString(buffer, offset, receivedResult.ReadBytes));
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
