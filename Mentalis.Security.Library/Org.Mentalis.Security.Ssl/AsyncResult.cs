using System;
using System.Threading;

namespace Org.Mentalis.Security.Ssl;

internal class AsyncResult : IAsyncResult
{
	private Exception m_AsyncException;

	private bool m_Completed;

	private object m_Owner;

	private object m_StateObject;

	private ManualResetEvent m_WaitHandle;

	public Exception AsyncException
	{
		get
		{
			return m_AsyncException;
		}
		set
		{
			m_AsyncException = value;
		}
	}

	public bool IsCompleted => m_Completed;

	public bool CompletedSynchronously => false;

	public object AsyncState => m_StateObject;

	public WaitHandle AsyncWaitHandle
	{
		get
		{
			if (m_WaitHandle == null)
			{
				m_WaitHandle = new ManualResetEvent(m_Completed);
			}
			if (m_Completed)
			{
				m_WaitHandle.Set();
			}
			return m_WaitHandle;
		}
	}

	public event AsyncCallback Callback;

	internal AsyncResult(AsyncCallback callback, object stateObject, object owner)
	{
		m_StateObject = stateObject;
		m_Completed = false;
		m_Owner = owner;
		if (callback != null)
		{
			this.Callback = (AsyncCallback)Delegate.Combine(this.Callback, callback);
		}
	}

	public void Notify(Exception e)
	{
		if (m_Completed)
		{
			return;
		}
		m_AsyncException = e;
		m_Completed = true;
		if (this.Callback != null)
		{
			if (m_Owner != null)
			{
				Monitor.Exit(m_Owner);
			}
			try
			{
				this.Callback(this);
			}
			finally
			{
				if (m_Owner != null)
				{
					Monitor.Enter(m_Owner);
				}
			}
		}
		if (m_WaitHandle != null)
		{
			m_WaitHandle.Set();
		}
	}

	public void Notify()
	{
		Notify(AsyncException);
	}
}
