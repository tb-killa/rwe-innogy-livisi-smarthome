using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketReceivers;

public class UdpPacketReceiver : IPacketReceiver, IMulticastListener
{
	private volatile bool running;

	private Thread thread;

	private UdpClient udpClient;

	private readonly int port;

	private List<IPAddress> multicastAddresses = new List<IPAddress>();

	private Queue<KeyValuePair<IPEndPoint, byte[]>> receivedMessagesQueue = new Queue<KeyValuePair<IPEndPoint, byte[]>>();

	private static ManualResetEvent wait = new ManualResetEvent(initialState: false);

	public Func<IPEndPoint, byte[], byte[]> ProcessIncomingMessage { get; set; }

	public event Action ErrorOccured;

	public UdpPacketReceiver(int port)
	{
		this.port = port;
	}

	public void Start()
	{
		if (!running)
		{
			udpClient = new UdpClient(port, AddressFamily.InterNetworkV6);
			running = true;
			thread = new Thread(ReceiveThread);
			thread.Start();
		}
	}

	public void Stop()
	{
		if (running)
		{
			running = false;
			udpClient.Close();
			thread.Join();
		}
	}

	public void SetAddresses(IList<IPAddress> addresses)
	{
		if (addresses == null)
		{
			addresses = new List<IPAddress>();
		}
		foreach (IPAddress item in addresses.Distinct().Except(multicastAddresses))
		{
			ChangeGroupMembership(item, join: true);
		}
		foreach (IPAddress item2 in multicastAddresses.Distinct().Except(addresses))
		{
			ChangeGroupMembership(item2, join: false);
		}
		multicastAddresses = addresses.ToList();
	}

	private void ReceiveThread()
	{
		try
		{
			ChangeGroupMembership(IPAddress.Parse("ff02::1"), join: true);
			multicastAddresses.ForEach(delegate(IPAddress addr)
			{
				ChangeGroupMembership(addr, join: true);
			});
			do
			{
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.IPv6Any, 0);
				byte[] value = udpClient.Receive(ref remoteEP);
				lock (receivedMessagesQueue)
				{
					if (receivedMessagesQueue.Count > 50)
					{
						Log.Warning(Module.LemonbeatProtocolAdapter, "UdpPacketReceiver: Incomming message queue is full. Ignoring message...");
						continue;
					}
					receivedMessagesQueue.Enqueue(new KeyValuePair<IPEndPoint, byte[]>(remoteEP, value));
					if (receivedMessagesQueue.Count == 1)
					{
						Log.Debug(Module.LemonbeatProtocolAdapter, "UdpPacketReceiver: Starting message processing thread");
						ThreadPool.QueueUserWorkItem(delegate
						{
							ProcessMessage();
						});
					}
				}
			}
			while (running);
		}
		catch (ThreadAbortException)
		{
			running = false;
		}
		catch (SocketException ex2)
		{
			if (ex2.ErrorCode != 10004)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "Error on UdpPacketReceiver. Listening thread will end here:" + ex2);
				this.ErrorOccured?.Invoke();
			}
		}
	}

	private void ProcessMessage()
	{
		try
		{
			bool flag = true;
			while (flag)
			{
				IPEndPoint key;
				byte[] value;
				lock (receivedMessagesQueue)
				{
					key = receivedMessagesQueue.Peek().Key;
					value = receivedMessagesQueue.Peek().Value;
				}
				Func<IPEndPoint, byte[], byte[]> processIncomingMessage = ProcessIncomingMessage;
				if (processIncomingMessage != null)
				{
					byte[] array = processIncomingMessage(key, value);
					if (array != null)
					{
						udpClient.Send(array, array.Length, key);
					}
				}
				lock (receivedMessagesQueue)
				{
					receivedMessagesQueue.Dequeue();
					if (receivedMessagesQueue.Count == 0)
					{
						flag = false;
					}
					else
					{
						Log.Debug(Module.LemonbeatProtocolAdapter, "UdpPacketReceiver: Process next message in the queue. Queue length:" + receivedMessagesQueue.Count);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "UdpPacketReceiver: Unexpected error in the message processing thread " + ex.ToString());
		}
	}

	private void ChangeGroupMembership(IPAddress address, bool join)
	{
		if (address != null)
		{
			int num = 0;
			Exception ex = null;
			if (udpClient == null)
			{
				return;
			}
			while (++num < 10)
			{
				try
				{
					wait.WaitOne(100, exitContext: false);
					if (join)
					{
						udpClient.JoinMulticastGroup(address);
					}
					else
					{
						udpClient.DropMulticastGroup(address);
					}
					ex = null;
				}
				catch (SocketException ex2)
				{
					if (ex2.ErrorCode != 10055)
					{
						throw;
					}
					ex = ex2;
					wait.WaitOne(1000, exitContext: false);
					continue;
				}
				break;
			}
			if (ex != null)
			{
				throw ex;
			}
		}
		else
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "UdpPacketReceiver.ChangeGroupMembership(). IPAddress parameter is null. Ignore call...");
		}
	}
}
