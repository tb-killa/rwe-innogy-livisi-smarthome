using System;
using System.ComponentModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

public class RequestStatusCompletedEventArgs : AsyncCompletedEventArgs
{
	public ValueCollection Result { get; private set; }

	public RequestStatusCompletedEventArgs(ValueCollection result, Exception error, bool cancelled, object userState)
		: base(error, cancelled, userState)
	{
		Result = result;
	}
}
