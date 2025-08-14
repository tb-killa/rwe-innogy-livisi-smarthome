using System;
using System.Threading;
using RWE.SmartHome.SHC.HCI.Messages;

namespace RWE.SmartHome.SHC.HCI;

public interface IHciClient : IDisposable
{
	ManualResetEvent Started { get; }

	event Action<HciMessage> HciMessageReceived;

	event Action<string> ConnectionLost;

	void Start(string serialPortName);

	void Pause();

	void Resume();

	void Stop();

	SimpleResponseMessage Send(HciMessage hciMessage);
}
