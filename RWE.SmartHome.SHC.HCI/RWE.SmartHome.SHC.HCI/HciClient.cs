using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using RWE.SmartHome.SHC.HCI.Messages;

namespace RWE.SmartHome.SHC.HCI;

public class HciClient : IHciClient, IDisposable
{
	private readonly ManualResetEvent suspendWaitHandle = new ManualResetEvent(initialState: true);

	private Thread receiveThread;

	private SerialPort serialPort;

	private readonly Dictionary<AnswerIdentifier, AnswerContainer> awaitedResponses = new Dictionary<AnswerIdentifier, AnswerContainer>();

	private readonly object awaitedResponsesLock = new object();

	private bool running;

	private readonly Action<string> log;

	private readonly ManualResetEvent started = new ManualResetEvent(initialState: false);

	public ManualResetEvent Started => started;

	public event Action<HciMessage> HciMessageReceived;

	public event Action<string> ConnectionLost;

	public HciClient(Action<string> log)
	{
		this.log = log;
	}

	private void ReceiveHCIMessages(string serialPortName)
	{
		try
		{
			using (serialPort = new SerialPort(serialPortName, 57600, Parity.None, 8, StopBits.One))
			{
				serialPort.Open();
				started.Set();
				log("HciClient started successfully");
				do
				{
					HciMessage hciMessage = HciReceiver.ReceiveHciMessage(serialPort.BaseStream);
					DispatchHCIMessage(hciMessage);
					suspendWaitHandle.WaitOne();
				}
				while (running);
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			log("Exception caught in HciClient. Receiving thread will end here:\n" + ex2.ToString());
			this.ConnectionLost?.Invoke(ex2.Message);
		}
		finally
		{
			started.Reset();
			running = false;
		}
	}

	private void DispatchHCIMessage(HciMessage hciMessage)
	{
		try
		{
			AnswerIdentifier key = new AnswerIdentifier(hciMessage.Header.EndpointId, hciMessage.Header.MessageId);
			AnswerContainer answerContainer = null;
			lock (awaitedResponsesLock)
			{
				if (awaitedResponses.ContainsKey(key))
				{
					answerContainer = awaitedResponses[key];
					answerContainer.Answer = hciMessage.Payload;
				}
			}
			answerContainer?.WaitHandle.Set();
			this.HciMessageReceived?.Invoke(hciMessage);
		}
		catch (Exception ex)
		{
			log($"Failed to process HCI HciMessage: {ex.Message}");
		}
	}

	public void Start(string serialPortName)
	{
		log("HciClient: Start() called.");
		if (!running)
		{
			running = true;
			receiveThread = new Thread((ThreadStart)delegate
			{
				ReceiveHCIMessages(serialPortName);
			});
			receiveThread.Start();
		}
		else
		{
			log("HciClient: No action taken as the processing thread is already running");
		}
	}

	public void Pause()
	{
		suspendWaitHandle.Reset();
		log("HciClient was suspended");
	}

	public void Resume()
	{
		suspendWaitHandle.Set();
		log("HciClient was resumed");
	}

	public void Stop()
	{
		log("HciClient: Stop() was called.");
		if (running)
		{
			receiveThread.Abort();
			log("HciClient successfully stopped");
		}
		else
		{
			log("HciClient: No action taken as the processing thread was already stopped");
		}
		running = false;
	}

	public SimpleResponseMessage Send(HciMessage hciMessage)
	{
		AnswerContainer answerContainer = new AnswerContainer();
		AnswerIdentifier answerIdentifier = GetAnswerIdentifier(hciMessage.Header);
		SimpleResponseMessage result = null;
		try
		{
			lock (awaitedResponsesLock)
			{
				if (awaitedResponses.TryGetValue(answerIdentifier, out var value))
				{
					value.Processed.WaitOne();
				}
				if (answerIdentifier != null)
				{
					awaitedResponses.Add(answerIdentifier, answerContainer);
				}
				byte[] array = hciMessage.ToArray();
				serialPort.BaseStream.Write(array, 0, array.Length);
			}
			if (answerIdentifier != null)
			{
				if (!answerContainer.WaitHandle.WaitOne(4000, exitContext: false))
				{
					log("Failed to send message: Timeout");
					return null;
				}
				result = new SimpleResponseMessage(answerContainer.Answer);
			}
		}
		finally
		{
			if (answerIdentifier != null)
			{
				lock (awaitedResponsesLock)
				{
					awaitedResponses[answerIdentifier].WaitHandle.Close();
					awaitedResponses.Remove(answerIdentifier);
				}
				answerContainer.MarkComplete();
			}
		}
		return result;
	}

	private AnswerIdentifier GetAnswerIdentifier(HciHeader header)
	{
		switch (header.EndpointId)
		{
		case EndpointIdentifier.DEVMGMT_ID:
			return GetAnswerIdentifierDeviceManagement(header.MessageId);
		case EndpointIdentifier.RADIOLINK_ID:
			return GetAnswerIdentifierRadioLink(header.MessageId);
		case EndpointIdentifier.RADIOLINKTEST_ID:
		case EndpointIdentifier.HWTEST_ID:
			throw new NotImplementedException();
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private AnswerIdentifier GetAnswerIdentifierRadioLink(byte messageId)
	{
		return messageId switch
		{
			4 => new AnswerIdentifier(EndpointIdentifier.RADIOLINK_ID, 5), 
			1 => new AnswerIdentifier(EndpointIdentifier.RADIOLINK_ID, 2), 
			_ => null, 
		};
	}

	private AnswerIdentifier GetAnswerIdentifierDeviceManagement(byte messageId)
	{
		return messageId switch
		{
			1 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 2), 
			3 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 4), 
			5 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 6), 
			7 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 8), 
			9 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 10), 
			11 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 12), 
			13 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 14), 
			15 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 16), 
			17 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 18), 
			33 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 34), 
			35 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 36), 
			37 => new AnswerIdentifier(EndpointIdentifier.DEVMGMT_ID, 38), 
			_ => null, 
		};
	}

	public void Dispose()
	{
		if (serialPort != null)
		{
			serialPort.Dispose();
		}
	}
}
