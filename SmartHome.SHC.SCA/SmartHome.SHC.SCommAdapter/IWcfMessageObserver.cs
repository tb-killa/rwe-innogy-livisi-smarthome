using System.Collections.Generic;
using System.ServiceModel.Channels;

namespace SmartHome.SHC.SCommAdapter;

public interface IWcfMessageObserver
{
	void BeforeSendingMessage(Message request);

	void AfterReceivedMessage(Message response, IList<WcfMessageHeader> headers);
}
