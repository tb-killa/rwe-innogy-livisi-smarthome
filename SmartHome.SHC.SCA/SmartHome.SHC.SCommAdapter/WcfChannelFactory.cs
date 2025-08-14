using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SmartHome.SHC.SCommAdapter;

internal class WcfChannelFactory : ChannelFactoryBase<IRequestChannel>
{
	private readonly MessageEncoderFactory encoderFactory;

	private readonly WcfBinding binding;

	private readonly BindingContext context;

	public WcfChannelFactory(WcfBinding binding, BindingContext context)
		: base((IDefaultCommunicationTimeouts)context.Binding)
	{
		this.binding = binding;
		this.context = context;
		MessageEncodingBindingElement messageEncodingBindingElement = context.BindingParameters.Remove<MessageEncodingBindingElement>();
		encoderFactory = messageEncodingBindingElement.CreateMessageEncoderFactory();
	}

	protected override IRequestChannel OnCreateChannel(EndpointAddress address, Uri via)
	{
		return new WcfRequestChannel(this, binding, encoderFactory, address, via, context);
	}

	protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
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
