using System;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public interface IBusinessLogic : IService
{
	bool SyncUsersAndRoles();

	void PerformBackendCommunicationWithRetries(string actionText, Func<bool, bool> backendCommunicationMethod);

	void BegingStopBackendServicesScheduler(string stopBackendRequestsDate);
}
