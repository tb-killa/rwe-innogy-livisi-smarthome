using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsSeeker
{
	private const int MaxMDnsFrameSize = 9000;

	private const int MulticastPort = 5353;

	private const int OnErrorRetryLimit = 10;

	private readonly IPAddress MulticastIP = IPAddress.Parse("224.0.0.251");

	private readonly IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 5353);

	private volatile bool ForceDiscoveryFinishedFlag;

	private ManualResetEvent PackageReceivedEvent = new ManualResetEvent(initialState: false);

	private MDnsPacket receivedPacket;

	private byte[] buffer = new byte[9000];

	private IAsyncResult asyncResult;

	public bool ForceDiscoveryFinished
	{
		set
		{
			ForceDiscoveryFinishedFlag = value;
			if (ForceDiscoveryFinishedFlag)
			{
				PackageReceivedEvent.Set();
			}
		}
	}

	public event Action<MDnsPacket> MDNSPacketReceived;

	public void Resolve(IEnumerable<MDnsQuestion> questions, TimeSpan timeout)
	{
		SendDiscoveryMulticast(questions);
		Start(timeout);
	}

	private void SendDiscoveryMulticast(IEnumerable<MDnsQuestion> questions)
	{
		ForceDiscoveryFinishedFlag = false;
		MDnsPacket mDnsPacket = new MDnsPacket();
		mDnsPacket.AsQuery().AddQuestions(questions);
		byte[] array = mDnsPacket.ToByteArray();
		using UdpClient udpClient = new UdpClient();
		udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
		udpClient.Client.Bind(endpoint);
		udpClient.JoinMulticastGroup(MulticastIP);
		udpClient.Connect(MulticastIP, 5353);
		udpClient.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 255);
		udpClient.Send(array, array.Length);
	}

	private Socket CreateListener()
	{
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
		socket.Bind(endpoint);
		socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(MulticastIP));
		return socket;
	}

	private void StartListening(Socket receiverSocket)
	{
		receivedPacket = null;
		PackageReceivedEvent.Reset();
		asyncResult = receiverSocket.BeginReceive(buffer, 0, 9000, SocketFlags.None, delegate(IAsyncResult ar)
		{
			if (ForceDiscoveryFinishedFlag)
			{
				return;
			}
			try
			{
				int num = receiverSocket.EndReceive(ar);
				if (num > 0)
				{
					Log.Debug(Module.ApplicationsHost, $"MDNS Packet: [{MDnsParserHelpers.ToReadable(buffer, num)}]");
					receivedPacket = new MDnsPacket(buffer);
					if (receivedPacket != null)
					{
						Log.Debug(Module.ApplicationsHost, $"Received {receivedPacket.Header.QueryCount} questions, {receivedPacket.Header.AnswerCount} answers, {receivedPacket.Header.AuthorityCount} auth, and {receivedPacket.Header.AdditionalResourcesCount} additional");
					}
					else
					{
						Log.Debug(Module.ApplicationsHost, "Packet was incorrect?");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Debug(Module.ApplicationsHost, $"Exception on MDNS listener thread: {ex.Message}");
			}
			finally
			{
				PackageReceivedEvent.Set();
			}
		}, null);
	}

	private void CloseListener(Socket receiverSocket)
	{
		try
		{
			ForceDiscoveryFinishedFlag = true;
			receiverSocket.Close();
		}
		catch (Exception ex)
		{
			Log.Debug(Module.ApplicationsHost, string.Format("Failure closing the receiving socket: ", ex.Message));
		}
		try
		{
			receiverSocket.EndReceive(asyncResult);
		}
		catch
		{
		}
	}

	private void Start(TimeSpan timeout)
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime dateTime = utcNow + timeout;
		Action<MDnsPacket> mDNSPacketReceived = this.MDNSPacketReceived;
		if (mDNSPacketReceived == null)
		{
			Log.Debug(Module.ApplicationsHost, $"No handler subscribed to the MDNSPacketReceived event");
			return;
		}
		PackageReceivedEvent.Reset();
		ForceDiscoveryFinished = false;
		Socket receiverSocket = CreateListener();
		while (!ForceDiscoveryFinishedFlag && dateTime > utcNow)
		{
			StartListening(receiverSocket);
			if (PackageReceivedEvent.WaitOne((int)(dateTime - utcNow).TotalMilliseconds, exitContext: false) && !ForceDiscoveryFinishedFlag)
			{
				if (receivedPacket == null)
				{
					Thread.Sleep(100);
				}
				else if (receivedPacket.Header.RCode == 0)
				{
					mDNSPacketReceived(receivedPacket);
				}
			}
			utcNow = DateTime.UtcNow;
		}
		CloseListener(receiverSocket);
	}
}
