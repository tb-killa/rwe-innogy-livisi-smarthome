using System.Threading;
using WebSocketLibrary.Helpers;

namespace WebSocketLibrary.Socket;

public class SenderSocket : ISenderSocket
{
	private readonly IBaseSocket socket;

	public SenderSocket(IBaseSocket socket)
	{
		this.socket = socket;
	}

	public void Send(byte[] data)
	{
		Send(data, 0, data.Length, 0u);
	}

	public void Send(byte[] data, uint mask)
	{
		Send(data, 0, data.Length, mask);
	}

	public void Send(byte[] data, int offset, int size)
	{
		Send(data, offset, size, 0u);
	}

	public void Send(byte[] data, int offset, int size, uint mask)
	{
		FrameMaskHelper.ApplyMaskOnBuffer(data, (ulong)offset, (ulong)size, mask);
		int num = size;
		int num2 = offset;
		while (num > 0)
		{
			int num3 = socket.SendBytes(data, num2, num);
			num -= num3;
			num2 += num3;
			if (num3 == 0)
			{
				Thread.Sleep(10);
			}
		}
	}
}
