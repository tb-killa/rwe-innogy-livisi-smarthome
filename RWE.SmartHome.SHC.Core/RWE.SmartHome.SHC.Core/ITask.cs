using System.Threading;

namespace RWE.SmartHome.SHC.Core;

public interface ITask
{
	int ManagedThreadId { get; }

	WaitHandle WaitHandle { get; }

	string Name { get; set; }

	void Start();

	void Stop();

	void Join();

	bool Join(int milliSecondsTimeout);
}
