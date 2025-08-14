namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;

public interface IStopBackendServicesHandler
{
	void ScheduleStoppingBackendServices(string stopBackendRequestsDate);
}
