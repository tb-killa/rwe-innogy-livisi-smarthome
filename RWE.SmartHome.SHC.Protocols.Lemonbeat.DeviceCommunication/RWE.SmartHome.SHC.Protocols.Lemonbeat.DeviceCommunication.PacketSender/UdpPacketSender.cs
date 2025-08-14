using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketSender;

public class UdpPacketSender : IPacketSender
{
	private struct ClientEntry
	{
		public readonly long Ticks;

		public readonly UdpClient UdpClient;

		public ClientEntry(long ticks, UdpClient udpClient)
		{
			Ticks = ticks;
			UdpClient = udpClient;
		}
	}

	private const int TimeOut = 15000;

	private readonly Dictionary<EndPoint, ClientEntry> openConnections = new Dictionary<EndPoint, ClientEntry>();

	private readonly object syncRoot = new object();

	private readonly Stopwatch sw = new Stopwatch();

	private readonly Thread thread;

	private readonly ManualResetEvent expiredConnectionsThreadWaitHandle = new ManualResetEvent(initialState: false);

	public Predicate<byte[]> IsMessageComplete { get; set; }

	public IPEndPoint LocalEndpoint { get; set; }

	public UdpPacketSender()
	{
		sw.Start();
		thread = new Thread(CheckForExpiredConnections);
		thread.Start();
	}

	public byte[] Send(IPEndPoint remoteEndPoint, byte[] message, bool responseExpected)
	{
		byte[] result = null;
		using (UdpClient udpClient = new UdpClient(AddressFamily.InterNetworkV6))
		{
			udpClient.Connect(remoteEndPoint);
			udpClient.Send(message, message.Length);
			if (responseExpected)
			{
				EndPoint localEndPoint = udpClient.Client.LocalEndPoint;
				lock (syncRoot)
				{
					openConnections.Add(localEndPoint, new ClientEntry(sw.ElapsedMilliseconds, udpClient));
				}
				try
				{
					result = udpClient.Receive(ref remoteEndPoint);
				}
				catch (Exception ex)
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, "No reply from :" + remoteEndPoint.ToString());
					Log.Warning(Module.LemonbeatProtocolAdapter, ex.ToString());
					throw new TimeoutException();
				}
				finally
				{
					lock (syncRoot)
					{
						openConnections.Remove(localEndPoint);
					}
				}
			}
		}
		return result;
	}

	public void Close()
	{
		if (thread == null)
		{
			return;
		}
		expiredConnectionsThreadWaitHandle.Set();
		foreach (KeyValuePair<EndPoint, ClientEntry> openConnection in openConnections)
		{
			openConnection.Value.UdpClient.Close();
		}
	}

	private void CheckForExpiredConnections()
	{
		do
		{
			lock (syncRoot)
			{
				foreach (KeyValuePair<EndPoint, ClientEntry> openConnection in openConnections)
				{
					if (sw.ElapsedMilliseconds - openConnection.Value.Ticks > 15000)
					{
						Log.Warning(Module.LemonbeatProtocolAdapter, "Closing udp client because of timeout...");
						openConnection.Value.UdpClient.Close();
					}
				}
			}
		}
		while (!expiredConnectionsThreadWaitHandle.WaitOne(10000, exitContext: false));
	}
}
