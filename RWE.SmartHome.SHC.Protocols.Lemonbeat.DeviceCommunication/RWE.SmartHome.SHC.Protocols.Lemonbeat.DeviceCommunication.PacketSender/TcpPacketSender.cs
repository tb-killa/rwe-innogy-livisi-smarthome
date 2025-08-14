using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketSender;

public class TcpPacketSender : IPacketSender
{
	private class ReadState
	{
		public ManualResetEvent CompletedEvent;

		public NetworkStream Stream;

		public byte[] Buffer;

		public byte[] Response;

		public Exception Exception;
	}

	private const string LoggingSource = "TcpPacketSender";

	private const int ConnectTimeout = 15000;

	private const int WriteTimeout = 20000;

	private const int ReadTimeout = 25000;

	private readonly Encoding encoding = Encoding.ASCII;

	public Predicate<byte[]> IsMessageComplete { get; set; }

	public IPEndPoint LocalEndpoint { get; set; }

	public byte[] Send(IPEndPoint remoteEndPoint, byte[] message, bool responseExpected)
	{
		byte[] result = null;
		TcpClient tcpClient = OpenConnection(remoteEndPoint);
		try
		{
			SendMessage(message, tcpClient);
			if (responseExpected)
			{
				result = GetResponse(tcpClient);
			}
		}
		finally
		{
			CloseConnection(tcpClient);
		}
		return result;
	}

	public void Close()
	{
	}

	private TcpClient OpenConnection(IPEndPoint remoteEndPoint)
	{
		TcpClient tcpClient = null;
		Exception connectError = null;
		int num = 0;
		IAsyncResult asyncResult = null;
		while (num++ < 3)
		{
			try
			{
				tcpClient = new TcpClient(LocalEndpoint);
			}
			catch (Exception ex)
			{
				connectError = ex;
				Thread.Sleep(1000);
				continue;
			}
			asyncResult = tcpClient.Client.BeginConnect(remoteEndPoint, delegate
			{
				try
				{
					if (tcpClient.Client != null)
					{
						tcpClient.Client.EndConnect(asyncResult);
						connectError = null;
					}
				}
				catch (Exception ex2)
				{
					connectError = ex2;
				}
			}, null);
			if (asyncResult.AsyncWaitHandle.WaitOne(15000, exitContext: false))
			{
				break;
			}
			CloseConnection(tcpClient);
		}
		if (connectError != null)
		{
			throw connectError;
		}
		if (!asyncResult.AsyncWaitHandle.WaitOne(0, exitContext: false))
		{
			Log.Information(Module.LemonbeatProtocolAdapter, "OpenConnection time out..trying to clean up...");
			CloseConnection(tcpClient);
			throw new TimeoutException("Timeout for connect, aborted.");
		}
		if (connectError != null)
		{
			throw connectError;
		}
		tcpClient.NoDelay = true;
		tcpClient.LingerState = new LingerOption(enable: true, 60);
		Log.Debug(Module.LemonbeatProtocolAdapter, "TCP Connection opened");
		return tcpClient;
	}

	private void SendMessage(byte[] message, TcpClient tcpClient)
	{
		NetworkStream networkStream = tcpClient.GetStream();
		Exception sendError = null;
		IAsyncResult asyncResult = networkStream.BeginWrite(message, 0, message.Length, delegate(IAsyncResult ar)
		{
			try
			{
				networkStream.EndWrite(ar);
			}
			catch (Exception ex)
			{
				sendError = ex;
			}
		}, null);
		if (!asyncResult.AsyncWaitHandle.WaitOne(20000, exitContext: false))
		{
			Log.Information(Module.LemonbeatProtocolAdapter, "SendMessage EXIT with Exception");
			throw new TimeoutException("Timeout for write, aborted.");
		}
		if (sendError != null)
		{
			throw sendError;
		}
	}

	private byte[] GetResponse(TcpClient tcpClient)
	{
		NetworkStream stream = tcpClient.GetStream();
		new StringBuilder();
		ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		byte[] buffer = BufferManager.GetBuffer();
		try
		{
			ReadState readState = new ReadState();
			readState.CompletedEvent = manualResetEvent;
			readState.Buffer = buffer;
			readState.Stream = stream;
			readState.Response = null;
			ReadState readState2 = readState;
			stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, readState2);
			if (!manualResetEvent.WaitOne(25000, exitContext: false))
			{
				throw new TimeoutException("Read timeout.");
			}
			if (readState2.Exception != null)
			{
				throw readState2.Exception;
			}
			return readState2.Response;
		}
		finally
		{
			BufferManager.ReleaseBuffer(buffer);
		}
	}

	private void ReadCallback(IAsyncResult asyncResult)
	{
		ReadState readState = (ReadState)asyncResult.AsyncState;
		try
		{
			if (!readState.Stream.CanRead)
			{
				readState.CompletedEvent.Set();
				Log.Information(Module.LemonbeatProtocolAdapter, "Exit read callback because stream cannot be read");
				return;
			}
			int num = readState.Stream.EndRead(asyncResult);
			if (num == 0)
			{
				readState.CompletedEvent.Set();
				Log.Information(Module.LemonbeatProtocolAdapter, "Done reading response asynchronously.");
				return;
			}
			int num2 = 0;
			if (readState.Response == null)
			{
				readState.Response = new byte[num];
			}
			else
			{
				num2 = readState.Response.Length;
				Array.Resize(ref readState.Response, num2 + num);
			}
			Buffer.BlockCopy(readState.Buffer, 0, readState.Response, num2, num);
			if (!IsMessageComplete(readState.Response))
			{
				Log.Information(Module.LemonbeatProtocolAdapter, "More data available on the stream? Fun. Let's read once more");
				readState.Stream.BeginRead(readState.Buffer, 0, readState.Buffer.Length, ReadCallback, readState);
			}
			else
			{
				Log.Debug(Module.LemonbeatProtocolAdapter, $"Finished reading the response: {readState.Response.Length} bytes");
				readState.CompletedEvent.Set();
			}
		}
		catch (Exception innerException)
		{
			if (innerException.InnerException is SocketException)
			{
				innerException = innerException.InnerException;
			}
			Log.Information(Module.LemonbeatProtocolAdapter, $"Read failed with exception {((object)innerException).GetType()}: {innerException.Message}");
			readState.Exception = innerException;
			readState.CompletedEvent.Set();
		}
	}

	private void CloseConnection(TcpClient tcpClient)
	{
		try
		{
			if (tcpClient.Client != null && tcpClient.Client.Connected)
			{
				tcpClient.GetStream().Close();
				tcpClient.Close();
				Log.Debug(Module.LemonbeatProtocolAdapter, "TCP Connection closed");
			}
			((IDisposable)tcpClient).Dispose();
		}
		catch (Exception ex)
		{
			Log.Information(Module.LemonbeatProtocolAdapter, $"Close failed with exception {ex.Message}");
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Details: {ex.ToString()}");
		}
	}
}
