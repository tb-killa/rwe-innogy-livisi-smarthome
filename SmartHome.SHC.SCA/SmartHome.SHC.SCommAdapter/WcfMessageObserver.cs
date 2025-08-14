using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;

namespace SmartHome.SHC.SCommAdapter;

public class WcfMessageObserver : IWcfMessageObserver, IDisposable
{
	private readonly WcfBinding binding;

	public IList<WcfMessageHeader> OutgoingHeaders { get; private set; }

	public IList<WcfMessageHeader> IncomingHeaders { get; private set; }

	public WcfMessageObserver(WcfBinding binding)
	{
		this.binding = binding;
		OutgoingHeaders = new List<WcfMessageHeader>();
		this.binding.MessageObservers.Add(this);
	}

	public void BeforeSendingMessage(Message request)
	{
		foreach (WcfMessageHeader outgoingHeader in OutgoingHeaders)
		{
			request.Headers.Add(outgoingHeader);
		}
	}

	public void AfterReceivedMessage(Message response, IList<WcfMessageHeader> headers)
	{
		IncomingHeaders = headers;
	}

	void IDisposable.Dispose()
	{
		binding.MessageObservers.Remove(this);
	}
}
