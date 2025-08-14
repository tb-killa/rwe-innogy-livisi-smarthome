using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TLSDetector;
using Rebex;
using SmartHome.SHC.SCommAdapter;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients;

internal class ApplicationTokenClient : ClientBase<ApplicationTokenServiceClient, IApplicationTokenService>, IApplicationTokenClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("ApplicationTokenClient");

	private ApplicationTokenServiceClient appTokenClient;

	public ApplicationTokenClient(INetworkingMonitor networkingMonitor, Configuration configuration, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.ApplicationTokenServiceUrl, registrationService)
	{
	}

	private void TryCreateServiceClient(string certificateThumbprint)
	{
		if (appTokenClient == null)
		{
			if (string.IsNullOrEmpty(certificateThumbprint))
			{
				Log.Error(Module.BackendCommunication, "Certificate is null, cannot create ApplicationTokenServiceClient");
			}
			else
			{
				appTokenClient = CreateClient(certificateThumbprint);
			}
		}
		else
		{
			appTokenClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, certificateThumbprint);
		}
	}

	private ApplicationTokenServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new ApplicationTokenServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationsToken GetApplicationToken(string certificateThumbprint)
	{
		TryCreateServiceClient(certificateThumbprint);
		if (appTokenClient != null)
		{
			return GetContractTokenFromBackendToken(appTokenClient.GetApplicationToken());
		}
		return null;
	}

	public string GetApplicationTokenHash(string certificateThumbprint)
	{
		TryCreateServiceClient(certificateThumbprint);
		if (appTokenClient != null)
		{
			return appTokenClient.GetApplicationTokenHash();
		}
		return string.Empty;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationsToken GetContractTokenFromBackendToken(RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationsToken token)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationsToken applicationsToken = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationsToken();
		applicationsToken.ShcType = token.ShcType;
		applicationsToken.Hash = token.Hash;
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationsToken applicationsToken2 = applicationsToken;
		if (token.Entries != null)
		{
			applicationsToken2.Entries = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationTokenEntry>(token.Entries.Length);
			RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationTokenEntry[] entries = token.Entries;
			foreach (RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationTokenEntry applicationTokenEntry in entries)
			{
				applicationsToken2.Entries.Add(new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationTokenEntry
				{
					AppId = applicationTokenEntry.AppId,
					ApplicationUrl = applicationTokenEntry.ApplicationUrl,
					ExpirationDate = applicationTokenEntry.ExpirationDate,
					IsEnabledByUser = applicationTokenEntry.IsEnabledByUser,
					IsEnabledByWebshop = applicationTokenEntry.IsEnabledByWebshop,
					IsService = applicationTokenEntry.IsService,
					Parameters = applicationTokenEntry.Parameters.ToList().ConvertAll((RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationParameter p) => new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.ApplicationParameter
					{
						Key = p.Key,
						Value = p.Value
					}),
					UpdateAvailable = applicationTokenEntry.UpdateAvailable,
					Version = applicationTokenEntry.Version,
					FullyQualifiedTypeName = applicationTokenEntry.FullyQualifiedAssemblyName,
					ApplicationKind = ConvertApplicationKind(applicationTokenEntry.ApplicationKind),
					AppManifest = applicationTokenEntry.AppManifest,
					SHCPackageChecksum = applicationTokenEntry.SHCPackageChecksum,
					SlPackageChecksum = applicationTokenEntry.SlPackageChecksum,
					Name = applicationTokenEntry.Name
				});
			}
		}
		return applicationsToken2;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums.ApplicationKind ConvertApplicationKind(RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationKind beValue)
	{
		return beValue switch
		{
			RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationKind.SHCAndSilverlight => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums.ApplicationKind.SHCAndSilverlight, 
			RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationKind.SHCOnly => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums.ApplicationKind.SHCOnly, 
			RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope.ApplicationKind.SilverlightOnly => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums.ApplicationKind.SilverlightOnly, 
			_ => throw new ArgumentException("Invalid application kind."), 
		};
	}
}
