using System;
using System.ServiceModel.Channels;

namespace SmartHome.SHC.SCommAdapter;

internal class WcfChannelBindingElement : TransportBindingElement
{
	private readonly WcfBinding binding;

	public override string Scheme => "https";

	public WcfChannelBindingElement(WcfBinding owner)
	{
		binding = owner;
	}

	public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
	{
		return (object)typeof(TChannel) == typeof(IRequestChannel);
	}

	public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (!CanBuildChannelFactory<TChannel>(context))
		{
			throw new ArgumentException($"Unsupported channel type: {typeof(TChannel).Name}.");
		}
		return (IChannelFactory<TChannel>)new WcfChannelFactory(binding, context);
	}

	public override BindingElement Clone()
	{
		return new WcfChannelBindingElement(binding);
	}
}
