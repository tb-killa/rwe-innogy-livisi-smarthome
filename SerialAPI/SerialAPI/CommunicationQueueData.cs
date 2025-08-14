using System;
using System.Collections.Generic;

namespace SerialAPI;

internal struct CommunicationQueueData
{
	public SerialHandlerType HandlerType;

	public List<byte> PayloadData;

	public DateTime ReceiveTime;
}
