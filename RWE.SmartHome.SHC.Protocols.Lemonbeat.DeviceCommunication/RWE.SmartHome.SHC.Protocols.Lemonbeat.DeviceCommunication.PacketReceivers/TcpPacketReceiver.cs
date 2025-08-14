using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketSender;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketReceivers;

public class TcpPacketReceiver : IPacketReceiver
{
	private volatile bool running;

	private readonly Thread thread;

	private TcpListener tcpListener;

	private readonly Encoding encoding = Encoding.ASCII;

	public Func<IPEndPoint, byte[], byte[]> ProcessIncomingMessage { get; set; }

	public event Action ErrorOccured;

	public TcpPacketReceiver(int port)
	{
		tcpListener = new TcpListener(IPAddress.IPv6Any, port);
		thread = new Thread(ReceiveThread);
	}

	public void Start()
	{
		if (!running)
		{
			tcpListener.Start();
			running = true;
			thread.Start();
		}
	}

	public void Stop()
	{
		if (running)
		{
			running = false;
			tcpListener.Stop();
			thread.Join();
		}
	}

	private void ReceiveThread()
	{
		try
		{
			do
			{
				TcpClient acceptedTcpClient = tcpListener.AcceptTcpClient();
				ThreadPool.QueueUserWorkItem(delegate
				{
					ProcessConnection(acceptedTcpClient);
				});
			}
			while (running);
			tcpListener.Stop();
		}
		catch (ThreadAbortException)
		{
			running = false;
		}
		catch (SocketException ex2)
		{
			if (ex2.ErrorCode != 10004)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "Error on TcpPacketReceiver. Listening thread will end here:" + ex2);
				this.ErrorOccured?.Invoke();
			}
		}
	}

	private void ProcessConnection(TcpClient acceptedTcpClient)
	{
		byte[] buffer = BufferManager.GetBuffer();
		try
		{
			NetworkStream stream = acceptedTcpClient.GetStream();
			stream.Read(buffer, 0, buffer.Length);
			Func<IPEndPoint, byte[], byte[]> processIncomingMessage = ProcessIncomingMessage;
			if (processIncomingMessage != null)
			{
				byte[] array = processIncomingMessage((IPEndPoint)acceptedTcpClient.Client.RemoteEndPoint, buffer);
				if (array != null)
				{
					stream.Write(array, 0, array.Length);
				}
			}
		}
		finally
		{
			BufferManager.ReleaseBuffer(buffer);
		}
	}
}
