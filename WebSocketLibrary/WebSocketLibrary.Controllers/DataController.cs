using System;
using WebSocketLibrary.Managers.Frames;

namespace WebSocketLibrary.Controllers;

public class DataController
{
	private readonly IFramesManager framesManager;

	public DataController(IFramesManager framesManager)
	{
		this.framesManager = framesManager;
	}

	public void SendText(string message)
	{
		framesManager.SendText(message);
	}

	public void SendData(ArraySegment<byte> data)
	{
		framesManager.SendData(data);
	}

	public ReceivedResult ReceiveData(ArraySegment<byte> data)
	{
		return framesManager.ReceiveData(data);
	}
}
