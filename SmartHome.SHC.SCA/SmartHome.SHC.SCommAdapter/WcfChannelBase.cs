using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SmartHome.SHC.SCommAdapter;

internal abstract class WcfChannelBase : ChannelBase
{
	protected readonly MessageEncoder encoder;

	protected readonly ChannelManagerBase manager;

	public EndpointAddress RemoteAddress { get; private set; }

	public WcfChannelBase(ChannelManagerBase manager, MessageEncoderFactory encoderFactory, EndpointAddress address)
		: base(manager)
	{
		this.manager = manager;
		encoder = encoderFactory.Encoder;
	}

	protected override void OnAbort()
	{
		throw new NotImplementedException();
	}

	protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
	{
		throw new NotImplementedException();
	}

	protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
	{
		throw new NotImplementedException();
	}

	protected override void OnClose(TimeSpan timeout)
	{
	}

	protected override void OnEndClose(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	protected override void OnEndOpen(IAsyncResult result)
	{
		throw new NotImplementedException();
	}

	protected override void OnOpen(TimeSpan timeout)
	{
	}
}
