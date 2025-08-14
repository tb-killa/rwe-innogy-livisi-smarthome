using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public sealed class TokenCacheContext : IDisposable
{
	private readonly ITokenCache tokenCache;

	private readonly ApplicationsToken appsToken;

	private readonly string requestContext;

	public ApplicationsToken AppsToken => appsToken;

	public string Context => requestContext;

	public TokenCacheContext(ITokenCache tokenCache, ApplicationsToken appsToken, string requestContext)
	{
		this.tokenCache = tokenCache;
		this.appsToken = appsToken;
		this.requestContext = requestContext;
	}

	public void Dispose()
	{
		Log.InformationFormat(Module.ApplicationsHost, "TokenCacheContext", true, "Releasing TokenCache lock.");
		tokenCache.ReleaseCurrentToken(this);
		GC.SuppressFinalize(this);
	}

	~TokenCacheContext()
	{
		Log.InformationFormat(Module.BusinessLogic, "TokenCacheContext", true, "Releasing TokenCache lock.");
		tokenCache.ReleaseCurrentToken(this);
	}
}
