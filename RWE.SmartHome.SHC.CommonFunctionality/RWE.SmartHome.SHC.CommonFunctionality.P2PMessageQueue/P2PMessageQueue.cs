using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace RWE.SmartHome.SHC.CommonFunctionality.P2PMessageQueue;

public class P2PMessageQueue
{
	private class Win32Api
	{
		public enum EventFlags
		{
			EVENT_PULSE = 1,
			EVENT_RESET,
			EVENT_SET
		}

		public struct MsgQueueInfo
		{
			public int dwSize;

			public int dwFlags;

			public int dwMaxMessages;

			public int cbMaxMessage;

			public int dwCurrentMessages;

			public int dwMaxQueueMessages;

			public short wNumReaders;

			public short wNumWriters;
		}

		public class MsgQueueOptions
		{
			public int dwSize;

			public int dwFlags;

			public int dwMaxMessages;

			public int cbMaxMessage;

			public int bReadAccess;

			public MsgQueueOptions(bool forReading, int maxMessageLength, int maxMessages)
			{
				bReadAccess = (forReading ? 1 : 0);
				dwMaxMessages = maxMessages;
				cbMaxMessage = maxMessageLength;
				dwSize = 20;
			}

			public MsgQueueOptions(bool forReading)
			{
				bReadAccess = (forReading ? 1 : 0);
				dwSize = 20;
			}
		}

		public const int ERROR_ALREADY_EXISTS = 183;

		public const int MSGQUEUE_MSGALERT = 1;

		public const int ERROR_PIPE_NOT_CONNECTED = 233;

		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		public const int MSGQUEUE_NOPRECOMMIT = 1;

		public const int MSGQUEUE_ALLOW_BROKEN = 2;

		public const int ERROR_OUTOFMEMORY = 14;

		public const int ERROR_TIMEOUT = 1460;

		public const int WAIT_TIMEOUT = 258;

		public const int WAIT_OBJECT_0 = 0;

		public const int MAX_PATH = 260;

		public const int INVALID_HANDLE_ERROR = 6;

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern int EventModify(IntPtr hEvent, int function);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool CloseMsgQueue(IntPtr hMsgQ);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern IntPtr CreateMsgQueue(string lpszName, MsgQueueOptions lpOptions);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool GetMsgQueueInfo(IntPtr hMsgQ, ref MsgQueueInfo lpInfo);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern IntPtr OpenMsgQueue(IntPtr hSrcProc, IntPtr hMsgQ, MsgQueueOptions lpOptions);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool ReadMsgQueue(IntPtr hMsgQ, byte[] lpBuffer, int cbBufferSize, out int lpNumberOfBytesRead, int dwTimeout, out int pdwFlags);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool WriteMsgQueue(IntPtr hMsgQ, byte[] lpBuffer, int cbDataSize, int dwTimeout, int dwFlags);
	}

	private const int TimeoutInfinite = -1;

	private const int InfiniteQueueSize = 0;

	private const int InvalidHandle = 0;

	private const int DefaultMaxMsgLength = 4096;

	private readonly bool mIsForReading;

	private readonly string mName;

	private IntPtr hQue = IntPtr.Zero;

	private byte[] mReadBuffer;

	public IntPtr Handle => hQue;

	public bool CanRead => mIsForReading;

	public bool CanWrite => !mIsForReading;

	public string QueueName => mName;

	public int MaxMessagesAllowed => GetInfo().dwMaxMessages;

	public int MaxMessageLength => GetInfo().cbMaxMessage;

	public int MostMessagesSoFar => GetInfo().dwMaxQueueMessages;

	public int MessagesInQueueNow => GetInfo().dwCurrentMessages;

	public short CurrentReaders => GetInfo().wNumReaders;

	public short CurrentWriters => GetInfo().wNumWriters;

	public event EventHandler DataOnQueueChanged;

	private P2PMessageQueue(IntPtr hwnd, bool forReading)
	{
		if (hwnd.Equals(0))
		{
			throw new ApplicationException("Could not create queue, NativeError: " + Marshal.GetLastWin32Error());
		}
		hQue = hwnd;
		mIsForReading = forReading;
		StartEventThread();
	}

	public P2PMessageQueue(bool forReading)
		: this(forReading, null, 4096, 0)
	{
	}

	public P2PMessageQueue(bool forReading, string name)
		: this(forReading, name, 4096, 0)
	{
	}

	public P2PMessageQueue(bool forReading, string name, int maxMessageLength, int maxMessages)
	{
		if (name != null && name.Length > 260)
		{
			throw new ApplicationException("name too long");
		}
		if (maxMessageLength < 0)
		{
			throw new ApplicationException("maxMessageLength must be positive");
		}
		if (maxMessages < 0)
		{
			throw new ApplicationException("maxMessages must be positive");
		}
		Win32Api.MsgQueueOptions lpOptions = new Win32Api.MsgQueueOptions(forReading, maxMessageLength, maxMessages)
		{
			dwFlags = GetBehaviourFlag()
		};
		try
		{
			hQue = Win32Api.CreateMsgQueue(name, lpOptions);
		}
		catch (MissingMethodException innerException)
		{
			throw new ApplicationException("P2P Queues are only supported by WinCE 4.x\r\n PPC2002 or other CE 3-based devices are not supported.", innerException);
		}
		if (hQue.Equals(0))
		{
			throw new ApplicationException("Could not create queue " + name + ", NativeError: " + Marshal.GetLastWin32Error());
		}
		mIsForReading = forReading;
		mName = name;
		if (forReading)
		{
			mReadBuffer = new byte[maxMessageLength];
		}
		StartEventThread();
	}

	public P2PMessageQueue(bool forReading, string name, int maxMessageLength, int maxMessages, out bool createdNew)
		: this(forReading, name, maxMessageLength, maxMessages)
	{
		createdNew = Marshal.GetLastWin32Error() != 183;
	}

	public void Close()
	{
		Dispose(finalizing: false);
	}

	private void Dispose(bool finalizing)
	{
		if (!hQue.Equals(0))
		{
			IntPtr hMsgQ = hQue;
			hQue = IntPtr.Zero;
			Win32Api.CloseMsgQueue(hMsgQ);
			if (!finalizing)
			{
				GC.SuppressFinalize(this);
			}
		}
	}

	~P2PMessageQueue()
	{
		try
		{
			Dispose(finalizing: true);
		}
		catch
		{
		}
	}

	public static P2PMessageQueue OpenExisting(bool forReading, IntPtr processHandle, IntPtr queueHandle)
	{
		IntPtr hwnd = Win32Api.OpenMsgQueue(processHandle, queueHandle, new Win32Api.MsgQueueOptions(forReading));
		if (hwnd.Equals(0))
		{
			return null;
		}
		P2PMessageQueue p2PMessageQueue = new P2PMessageQueue(hwnd, forReading);
		if (forReading)
		{
			p2PMessageQueue.mReadBuffer = new byte[p2PMessageQueue.MaxMessageLength];
		}
		return p2PMessageQueue;
	}

	public ReadWriteResult Receive(Message msg)
	{
		return Receive(msg, -1);
	}

	public ReadWriteResult Receive(Message msg, TimeSpan ts)
	{
		return Receive(msg, ts.Milliseconds);
	}

	public ReadWriteResult Receive(Message msg, int timeoutMillis)
	{
		if (hQue.Equals(0))
		{
			throw new ApplicationException("Invalid Handle. Please use new queue");
		}
		if (Win32Api.ReadMsgQueue(hQue, mReadBuffer, mReadBuffer.GetLength(0), out var lpNumberOfBytesRead, timeoutMillis, out var pdwFlags))
		{
			byte[] array = new byte[lpNumberOfBytesRead];
			Buffer.BlockCopy(mReadBuffer, 0, array, 0, lpNumberOfBytesRead);
			msg.MessageBytes = array;
			msg.IsAlert = pdwFlags == 1;
			return ReadWriteResult.OK;
		}
		int lastWin32Error = Marshal.GetLastWin32Error();
		switch (lastWin32Error)
		{
		case 122:
			return ReadWriteResult.BufferFail;
		case 233:
			return ReadWriteResult.Disconnected;
		case 1460:
			return ReadWriteResult.Timeout;
		case 6:
			Close();
			return ReadWriteResult.InvalidHandle;
		default:
			throw new ApplicationException("Failed to read: " + lastWin32Error);
		}
	}

	public ReadWriteResult Send(Message msg)
	{
		return Send(msg, -1);
	}

	public ReadWriteResult Send(Message msg, TimeSpan ts)
	{
		return Send(msg, ts.Milliseconds);
	}

	public ReadWriteResult Send(Message msg, int timeoutMillis)
	{
		if (hQue.Equals(0))
		{
			throw new ApplicationException("Invalid Handle. Please use new queue");
		}
		byte[] messageBytes = msg.MessageBytes;
		if (messageBytes == null)
		{
			throw new ApplicationException("Message must contain bytes");
		}
		if (Win32Api.WriteMsgQueue(hQue, messageBytes, messageBytes.GetLength(0), timeoutMillis, msg.IsAlert ? 1 : 0))
		{
			return ReadWriteResult.OK;
		}
		msg = null;
		int lastWin32Error = Marshal.GetLastWin32Error();
		switch (lastWin32Error)
		{
		case 122:
			return ReadWriteResult.BufferFail;
		case 233:
			return ReadWriteResult.Disconnected;
		case 1460:
			return ReadWriteResult.Timeout;
		case 14:
			return ReadWriteResult.Timeout;
		case 6:
			Close();
			return ReadWriteResult.InvalidHandle;
		default:
			throw new ApplicationException("Failed to write: " + lastWin32Error);
		}
	}

	public void Purge()
	{
		if (CanWrite)
		{
			throw new ApplicationException("Queue is write only. Purge not applicable");
		}
		if (hQue.Equals(0))
		{
			throw new ApplicationException("Invalid Handle. Please use new queue");
		}
		ReadWriteResult readWriteResult = ReadWriteResult.OK;
		while (readWriteResult == ReadWriteResult.OK)
		{
			Message msg = new Message();
			readWriteResult = Receive(msg, 0);
		}
	}

	private void MonitorHandle()
	{
		while (!hQue.Equals(0))
		{
			int num = Win32Api.WaitForSingleObject(hQue, -1);
			if (hQue.Equals(0) || num != 0 || Win32Api.EventModify(hQue, 2) == 0)
			{
				break;
			}
			if (this.DataOnQueueChanged != null)
			{
				if (CanRead && MessagesInQueueNow > 0)
				{
					this.DataOnQueueChanged(this, EventArgs.Empty);
				}
				else if (CanWrite)
				{
					this.DataOnQueueChanged(this, EventArgs.Empty);
				}
			}
		}
	}

	private Win32Api.MsgQueueInfo GetInfo()
	{
		if (hQue.Equals(0))
		{
			throw new ApplicationException("Invalid Handle. Please use new queue");
		}
		Win32Api.MsgQueueInfo lpInfo = new Win32Api.MsgQueueInfo
		{
			dwSize = 28
		};
		if (Win32Api.GetMsgQueueInfo(hQue, ref lpInfo))
		{
			return lpInfo;
		}
		throw new ApplicationException("Failed to get queue info. NativeError = " + Marshal.GetLastWin32Error());
	}

	protected virtual void StartEventThread()
	{
		Thread thread = new Thread(MonitorHandle);
		thread.Start();
	}

	protected virtual int GetBehaviourFlag()
	{
		return 2;
	}
}
