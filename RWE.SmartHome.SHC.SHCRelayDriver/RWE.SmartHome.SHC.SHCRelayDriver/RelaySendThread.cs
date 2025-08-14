using System;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

internal class RelaySendThread : IDisposable
{
	private readonly Thread sendThread;

	private volatile bool halt;

	private volatile bool disposed;

	private readonly NotificationSendParameters notificationSendParameters;

	private readonly EventWaitHandle threadSync;

	private readonly NotificationQueue notificationQueue;

	public RelaySendThread(NotificationSendParameters param)
	{
		notificationQueue = new NotificationQueue(param);
		threadSync = new EventWaitHandle(initialState: true, EventResetMode.AutoReset);
		sendThread = new Thread(HandleQueues);
		notificationSendParameters = param;
		sendThread.Start();
	}

	private void HandleQueues()
	{
		do
		{
			try
			{
				bool flag = true;
				flag &= notificationQueue.ProcessMessages();
				if (halt)
				{
					break;
				}
				threadSync.WaitOne(flag ? 500 : 10, exitContext: false);
			}
			catch (Exception ex)
			{
				Log.Error(Module.RelayDriver, $"HandleQueues threw exception {ex.Message} with details: {ex}");
			}
		}
		while (!halt);
	}

	public void QueueMessage(BaseNotification notification)
	{
		notificationQueue.QueueNotification(notification);
		threadSync.Set();
	}

	internal void TerminateSession(string clientUri)
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				halt = true;
				sendThread.Join();
			}
			disposed = true;
		}
	}
}
