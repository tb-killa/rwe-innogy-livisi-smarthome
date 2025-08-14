using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public interface ITokenCache : IService
{
	event Action TokenUpdated;

	string GetCurrentAppTokenHash();

	TokenCacheContext GetAndLockCurrentToken(string requestContext);

	void ReleaseCurrentToken(TokenCacheContext tokenCacheContext);

	bool IsTokenUpToDate(string requestContext);

	bool UpdateToken(string requestContext);

	bool IsAvailableApplication(string appId);

	string GetApplicationVersionCache(string appId);

	void UpdateApplicationVersionCache(ApplicationTokenEntry applicationTokenEntry);

	void UpdateApplicationsTokenPersistence();
}
