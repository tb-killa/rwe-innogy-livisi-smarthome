namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IServiceLocator
{
	T GetService<T>() where T : class;
}
