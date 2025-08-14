namespace RWE.SmartHome.SHC.Core;

public interface IDispatcher
{
	void Dispatch(IExecutable executable);
}
