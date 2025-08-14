namespace RWE.SmartHome.SHC.Core;

public interface ITaskManager : IService
{
	void Register(ITask task);

	void Startup();
}
