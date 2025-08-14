using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TypeManager;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class TokenCache : ITokenCache, IService
{
	private ApplicationsToken currentToken;

	private readonly IApplicationsTokenPersistence persistence;

	private readonly IApplicationTokenClient backendServiceClient;

	private readonly ICertificateManager certManager;

	private readonly IShcTypeManager shcTypeManager;

	private readonly IEventManager eventManager;

	private readonly INetworkingMonitor networkMonitor;

	private readonly IRegistrationService registrationService;

	private readonly object tokenCacheLock = new object();

	private readonly ManualResetEvent resetEvent = new ManualResetEvent(initialState: false);

	private Dictionary<string, string> applicationVersionCache = new Dictionary<string, string>();

	public event Action TokenUpdated;

	internal TokenCache(IApplicationsTokenPersistence persistence, IApplicationTokenClient backendServiceClient, ICertificateManager certManager, IShcTypeManager shcTypeManager, IEventManager eventManager, INetworkingMonitor networkMonitor, IRegistrationService registrationService)
	{
		this.persistence = persistence;
		this.backendServiceClient = backendServiceClient;
		this.certManager = certManager;
		this.shcTypeManager = shcTypeManager;
		this.eventManager = eventManager;
		this.networkMonitor = networkMonitor;
		this.registrationService = registrationService;
	}

	public TokenCacheContext GetAndLockCurrentToken(string requestContext)
	{
		Log.DebugFormat(Module.ApplicationsHost, "TokenCache", true, "Trying to lock token cache from {0}", requestContext);
		Monitor.Enter(tokenCacheLock);
		if (currentToken == null)
		{
			currentToken = persistence.LoadApplicationsToken();
		}
		return new TokenCacheContext(this, currentToken.Clone(), requestContext);
	}

	public void ReleaseCurrentToken(TokenCacheContext tokenCacheContext)
	{
		Log.DebugFormat(Module.ApplicationsHost, "TokenCache", true, "Trying to unlock current token cache {0}", (tokenCacheContext != null) ? tokenCacheContext.Context : "NULL Token Cache Context!");
		Monitor.Exit(tokenCacheLock);
		Log.Debug(Module.ApplicationsHost, "TokenCache", "Unlocked token cache");
	}

	public bool IsTokenUpToDate(string requestContext)
	{
		try
		{
			string text = TryGetCertificateThumbprint("SHC certificate is not available, cannot check apps token.");
			if (string.IsNullOrEmpty(text))
			{
				Log.Warning(Module.ApplicationsHost, "Missing certificate thumbprint. Cannot check token validity.");
				return false;
			}
			string applicationTokenHash = backendServiceClient.GetApplicationTokenHash(text);
			Log.DebugFormat(Module.ApplicationsHost, "TokenCache", true, "Locking to test if token cache is up to date {0}", requestContext);
			bool result;
			lock (tokenCacheLock)
			{
				if (currentToken != null && currentToken.Hash != null)
				{
					Log.Debug(Module.ApplicationsHost, "Current hash: " + currentToken.Hash + " [" + currentToken.Hash.Length + "]");
				}
				Log.Debug(Module.ApplicationsHost, "Backend hash: " + applicationTokenHash + " [" + ((!string.IsNullOrEmpty(applicationTokenHash)) ? applicationTokenHash.Length : 0) + "]");
				result = currentToken != null && currentToken.Hash == applicationTokenHash;
			}
			Log.DebugFormat(Module.ApplicationsHost, "TokenCache", true, "Releasing lock for test if token cache is up to date {0}", requestContext);
			return result;
		}
		catch (Exception ex)
		{
			Log.Error(Module.ApplicationsHost, $"Failed to check applications token validity: {ex.Message}. {ex.StackTrace}");
			return false;
		}
	}

	public bool UpdateToken(string requestContext)
	{
		string text = TryGetCertificateThumbprint("SHC certificate is not available, cannot update apps token.");
		if (string.IsNullOrEmpty(text))
		{
			Log.Warning(Module.ApplicationsHost, "Missing certificate thumbprint. Cannot update token.");
			return false;
		}
		try
		{
			Log.DebugFormat(Module.ApplicationsHost, "TokenCache", true, "Locking to update token cache from {0}", requestContext);
			lock (tokenCacheLock)
			{
				if (registrationService.IsShcLocalOnly)
				{
					currentToken = persistence.LoadApplicationsToken();
					if (currentToken == null)
					{
						try
						{
							if (!string.IsNullOrEmpty(text))
							{
								currentToken = backendServiceClient.GetApplicationToken(text);
								if (string.IsNullOrEmpty(currentToken.Hash))
								{
									currentToken = new ApplicationsToken
									{
										Hash = FilePersistence.ApplicationsTokenHash,
										Entries = new List<ApplicationTokenEntry>(),
										ShcType = 0L
									};
								}
							}
						}
						catch (Exception ex)
						{
							Log.Error(Module.ApplicationsHost, $"Could not get the application token from backend, creating a new one. {ex.Message} {ex.StackTrace}");
							currentToken = new ApplicationsToken
							{
								Hash = FilePersistence.ApplicationsTokenHash,
								Entries = new List<ApplicationTokenEntry>(),
								ShcType = 0L
							};
						}
					}
				}
				else
				{
					currentToken = backendServiceClient.GetApplicationToken(text);
				}
				if (currentToken != null)
				{
					currentToken.Entries.ForEach(delegate(ApplicationTokenEntry entry)
					{
						entry.ActiveOnShc = true;
					});
					UpdateTypeManager();
					UpdateApplicationVersionCache();
					persistence.SaveApplicationsToken(currentToken);
					Log.Debug(Module.ApplicationsHost, "TokenCache", "Token update: emitting TokenUpdated internal event");
					this.TokenUpdated?.Invoke();
					Log.Debug(Module.ApplicationsHost, "TokenCache", "Token update: emitting TokenUpdated application-wide event");
					eventManager.GetEvent<TokenCacheUpdateEvent>().Publish(new TokenCacheUpdateEventArgs
					{
						Hash = currentToken.GetHash()
					});
				}
				else
				{
					Log.Error(Module.ApplicationsHost, "Token update failed. Token is null.");
				}
			}
			Log.DebugFormat(Module.ApplicationsHost, "TokenCache", true, "Token cache lock released (update from {0})", requestContext);
		}
		catch (Exception ex2)
		{
			Log.Error(Module.ApplicationsHost, $"Failed to update applications token: {ex2.Message}. {ex2.StackTrace}");
			return false;
		}
		return true;
	}

	public void Initialize()
	{
		try
		{
			currentToken = persistence.LoadApplicationsToken();
			UpdateTypeManager();
			UpdateApplicationVersionCache();
		}
		catch (Exception arg)
		{
			Log.Error(Module.ApplicationsHost, $"Failed to load applications token: {arg}");
			currentToken = null;
		}
	}

	public void Uninitialize()
	{
	}

	public bool IsAvailableApplication(string appId)
	{
		return applicationVersionCache.ContainsKey(appId);
	}

	public string GetApplicationVersionCache(string appId)
	{
		return applicationVersionCache[appId];
	}

	public void UpdateApplicationsTokenPersistence()
	{
		persistence.SaveApplicationsToken(currentToken);
	}

	private string TryGetCertificateThumbprint(string logMessage)
	{
		string personalCertificateThumbprint = certManager.PersonalCertificateThumbprint;
		if (string.IsNullOrEmpty(personalCertificateThumbprint))
		{
			Log.Information(Module.ApplicationsHost, logMessage);
		}
		return personalCertificateThumbprint;
	}

	private void UpdateTypeManager()
	{
		Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
		if (currentToken == null)
		{
			return;
		}
		if (currentToken.Entries != null)
		{
			foreach (ApplicationTokenEntry item in currentToken.Entries.Where((ApplicationTokenEntry e) => e.IsEnabled))
			{
				if (!dictionary.ContainsKey(item.AppId))
				{
					Dictionary<string, string> value = BuildParamDictionary(item);
					dictionary.Add(item.AppId, value);
				}
				else
				{
					Log.Warning(Module.ApplicationsHost, $"TokenCache: Add-in with id: {item.AppId} found multiple times in the token");
				}
			}
		}
		shcTypeManager.UpdateShcTypeData(dictionary, currentToken.ShcType);
	}

	public void UpdateApplicationVersionCache(ApplicationTokenEntry applicationTokenEntry)
	{
		if (!applicationVersionCache.ContainsKey(applicationTokenEntry.AppId))
		{
			applicationVersionCache.Add(applicationTokenEntry.AppId, applicationTokenEntry.Version);
		}
		else
		{
			applicationVersionCache[applicationTokenEntry.AppId] = applicationTokenEntry.Version;
		}
	}

	private void UpdateApplicationVersionCache()
	{
		if (currentToken == null)
		{
			Log.Debug(Module.ApplicationsHost, "UpdateApplicationVersionCache - token is null");
		}
		else if (currentToken.Entries != null)
		{
			Log.Debug(Module.ApplicationsHost, $"UpdateApplicationVersionCache - token entries {currentToken.Entries.Count}");
			foreach (ApplicationTokenEntry item in currentToken.Entries.Where((ApplicationTokenEntry e) => e.IsEnabled))
			{
				string appVersion = GetAppVersion(item.Version);
				if (!applicationVersionCache.ContainsKey(item.AppId))
				{
					applicationVersionCache.Add(item.AppId, appVersion);
				}
				else
				{
					applicationVersionCache[item.AppId] = appVersion;
				}
			}
			RemovedOldKeys(currentToken);
		}
		else
		{
			Log.Debug(Module.ApplicationsHost, "UpdateApplicationVersionCache - no token entries yet");
		}
	}

	private void RemovedOldKeys(ApplicationsToken currentToken)
	{
		List<string> currentTokensIds = currentToken.Entries.Select((ApplicationTokenEntry m) => m.AppId).ToList();
		List<string> list = applicationVersionCache.Keys.Where((string m) => !currentTokensIds.Contains(m)).ToList();
		foreach (string item in list)
		{
			applicationVersionCache.Remove(item);
		}
	}

	private string GetAppVersion(string appVersion)
	{
		Version version = new Version(appVersion);
		return $"{version.Major}.{version.Minor}";
	}

	private static Dictionary<string, string> BuildParamDictionary(ApplicationTokenEntry tokenEntry)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (tokenEntry.Parameters != null)
		{
			foreach (ApplicationParameter parameter in tokenEntry.Parameters)
			{
				if (!dictionary.ContainsKey(parameter.Key))
				{
					dictionary.Add(parameter.Key, parameter.Value);
				}
				else
				{
					Log.Warning(Module.ApplicationsHost, $"TokenCache: Parameter {parameter.Key} of add-in {tokenEntry.AppId} found multiple times in the token");
				}
			}
		}
		return dictionary;
	}

	public string GetCurrentAppTokenHash()
	{
		string value = TryGetCertificateThumbprint("SHC certificate is not available, cannot check apps token.");
		if (string.IsNullOrEmpty(value))
		{
			Log.Warning(Module.ApplicationsHost, "Missing certificate thumbprint. Cannot check token validity.");
			return null;
		}
		if (currentToken == null)
		{
			return null;
		}
		return currentToken.GetHash();
	}
}
