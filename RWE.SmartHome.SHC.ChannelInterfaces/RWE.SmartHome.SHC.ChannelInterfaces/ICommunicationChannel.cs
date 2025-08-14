namespace RWE.SmartHome.SHC.ChannelInterfaces;

public interface ICommunicationChannel : IBaseChannel
{
	string ChannelId { get; }

	ChannelType ChannelType { get; }

	bool Connected { get; }
}
