using System;

namespace Org.Mentalis.Security.Ssl;

internal class AsyncAcceptResult : AsyncResult
{
	private SecureSocket m_AcceptedSocket;

	public SecureSocket AcceptedSocket
	{
		get
		{
			return m_AcceptedSocket;
		}
		set
		{
			m_AcceptedSocket = value;
		}
	}

	internal AsyncAcceptResult(AsyncCallback callback, object stateObject, object owner)
		: base(callback, stateObject, owner)
	{
	}
}
