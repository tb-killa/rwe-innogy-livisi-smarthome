using System.Collections.Generic;
using RWE.SmartHome.SHC.ChannelInterfaces;

namespace RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;

public interface IChannelMultiplexer : IBaseChannel
{
	void AddCommunicationChannel(ICommunicationChannel communicationCommunicationChannel);

	IEnumerable<ICommunicationChannel> GetChannels();
}
