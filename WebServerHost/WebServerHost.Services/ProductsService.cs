using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Helpers;

namespace WebServerHost.Services;

internal class ProductsService : IProductsService
{
	private const string AppIdPrefix = "sh://";

	private readonly IAddinRepoManager addinRepo;

	private readonly IApplicationsTokenPersistence appTokenPeristence;

	private readonly IShcClient shcClient;

	private readonly INetworkingMonitor netMonitor;

	private readonly IRegistrationService registrationService;

	private readonly IApplicationsHost applicationHost;

	public ProductsService(IAddinRepoManager addinRepo, IApplicationsTokenPersistence appTokenPeristence, IShcClient shcClient, INetworkingMonitor netMonitor, IRegistrationService registrationService, IApplicationsHost applicationHost)
	{
		this.addinRepo = addinRepo;
		this.appTokenPeristence = appTokenPeristence;
		this.shcClient = shcClient;
		this.netMonitor = netMonitor;
		this.registrationService = registrationService;
		this.applicationHost = applicationHost;
	}

	public List<Product> GetAvailableProducts()
	{
		List<ApplicationTokenEntry> allAddinsFromPersistenceFile = addinRepo.GetAllAddinsFromPersistenceFile();
		ApplicationsToken currentAppToken = appTokenPeristence.LoadApplicationsToken();
		if (allAddinsFromPersistenceFile != null && allAddinsFromPersistenceFile.Any())
		{
			return allAddinsFromPersistenceFile.Select((ApplicationTokenEntry appTokenEntry) => new Product
			{
				Type = appTokenEntry.AppId.Remove(0, "sh://".Length),
				Version = "2.0",
				Provisioned = (currentAppToken != null && currentAppToken.Entries.Any((ApplicationTokenEntry e) => e.AppId == appTokenEntry.AppId)),
				Config = new List<Property>
				{
					new Property
					{
						Name = "name",
						Value = appTokenEntry.Name
					},
					new Property
					{
						Name = "productId",
						Value = appTokenEntry.Name
					},
					new Property
					{
						Name = "productKind",
						Value = "Free"
					},
					new Property
					{
						Name = "prerequisitesMet",
						Value = true
					},
					new Property
					{
						Name = "prerequisites",
						Value = string.Empty
					},
					new Property
					{
						Name = "isService",
						Value = appTokenEntry.IsService
					},
					new Property
					{
						Name = "isExtension",
						Value = false
					},
					new Property
					{
						Name = "isInternal",
						Value = false
					}
				},
				State = new List<Property>
				{
					new Property
					{
						Name = "enabledByUser",
						Value = true
					},
					new Property
					{
						Name = "enabledByWebShop",
						Value = true
					},
					new Property
					{
						Name = "active",
						Value = IsActive(appTokenEntry.AppId, currentAppToken)
					},
					new Property
					{
						Name = "updateAvailable",
						Value = false
					},
					new Property
					{
						Name = "fullVersion",
						Value = appTokenEntry.Version
					}
				}
			}).ToList();
		}
		return new List<Product>();
	}

	private bool IsActive(string appId, ApplicationsToken currentAppToken)
	{
		if (currentAppToken != null)
		{
			ApplicationTokenEntry applicationTokenEntry = currentAppToken.Entries.FirstOrDefault((ApplicationTokenEntry e) => e.AppId == appId);
			if (applicationTokenEntry != null)
			{
				return applicationTokenEntry.ActiveOnShc;
			}
		}
		return false;
	}

	public Product GetProduct(string type)
	{
		List<Product> availableProducts = GetAvailableProducts();
		if (availableProducts != null && availableProducts.Any())
		{
			return availableProducts.FirstOrDefault((Product p) => p.Type.EqualsIgnoreCase(type));
		}
		return null;
	}

	public string GetHash()
	{
		ApplicationsToken applicationsToken = appTokenPeristence.LoadApplicationsToken();
		if (applicationsToken != null)
		{
			return applicationsToken.Hash;
		}
		return 0u.ToString();
	}

	public void InstallProduct(string type)
	{
		LocalAddin addinFormUsbToTemporatyLocation = addinRepo.GetAddinFormUsbToTemporatyLocation(type);
		if (addinFormUsbToTemporatyLocation == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, string.Format("Application with appId={0}{1} does not exist.", "sh://", type));
		}
		try
		{
			addinRepo.InstallLocalAddin(addinFormUsbToTemporatyLocation, deleteZipFromTemporarySpace: true);
		}
		catch (AddinNotSignedException ex)
		{
			throw new ApiException(ErrorCode.AddinChecksumFailed, string.Format("Application with appId={0}{1} does not exist {2}", "sh://", type, ex.Message));
		}
		catch (Exception ex2)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"There was a problem installing the {type} add-in, {ex2.Message}, {ex2.StackTrace}");
		}
		ApplicationTokenEntry product = addinRepo.GetAddinTokenEntry(addinFormUsbToTemporatyLocation);
		if (product == null)
		{
			throw NotFound(type);
		}
		product.IsEnabledByUser = true;
		product.IsEnabledByWebshop = true;
		ApplicationsToken applicationsToken = appTokenPeristence.LoadApplicationsToken();
		if (!applicationsToken.Entries.Any((ApplicationTokenEntry e) => e.AppId == product.AppId))
		{
			applicationsToken.Entries.Add(product);
			SaveAndSendRefreshToken(applicationsToken);
		}
		applicationHost.LoadApplication(product, lastAttempt: true, isNewApplication: true);
	}

	private ApiException InvalidOperation(string operation)
	{
		return new ApiException(ErrorCode.TemporaryUnavailable, $"Cannot {operation} products locally while Cloud is available");
	}

	private ApiException NotFound(string type)
	{
		return new ApiException(ErrorCode.EntityDoesNotExist, string.Format("Application with appId={0}{1} does not exist.", "sh://", type));
	}

	private void SaveAndSendRefreshToken(ApplicationsToken token)
	{
		token.Hash = token.GetHash();
		appTokenPeristence.SaveApplicationsToken(token);
		if (!registrationService.IsShcLocalOnly)
		{
			RefreshApplicationTokenRequest request = ShcRequestHelper.NewRequest<RefreshApplicationTokenRequest>();
			shcClient.GetResponse(request);
		}
	}

	public void ActivateProduct(string type)
	{
		ApplicationsToken applicationsToken = appTokenPeristence.LoadApplicationsToken();
		ApplicationTokenEntry applicationTokenEntry = applicationsToken.Entries.FirstOrDefault((ApplicationTokenEntry e) => e.AppId.EqualsIgnoreCase("sh://" + type));
		if (applicationTokenEntry == null)
		{
			throw NotInstalled(type);
		}
		applicationTokenEntry.ActiveOnShc = true;
		SaveAndSendRefreshToken(applicationsToken);
	}

	private ApiException NotInstalled(string type)
	{
		return new ApiException(ErrorCode.PreconditionFailed, string.Format("Application {type} is not installed"));
	}

	public void DeactivateProduct(string type)
	{
		ApplicationsToken applicationsToken = appTokenPeristence.LoadApplicationsToken();
		ApplicationTokenEntry applicationTokenEntry = applicationsToken.Entries.FirstOrDefault((ApplicationTokenEntry e) => e.AppId.EqualsIgnoreCase("sh://" + type));
		if (applicationTokenEntry == null)
		{
			throw NotInstalled(type);
		}
		applicationTokenEntry.ActiveOnShc = false;
		SaveAndSendRefreshToken(applicationsToken);
	}

	public void UninstallProduct(string type)
	{
		ApplicationsToken applicationsToken = appTokenPeristence.LoadApplicationsToken();
		ApplicationTokenEntry app = applicationsToken.Entries.FirstOrDefault((ApplicationTokenEntry e) => e.AppId.EqualsIgnoreCase("sh://" + type));
		if (app == null)
		{
			throw NotInstalled(type);
		}
		applicationsToken.Entries.RemoveAll((ApplicationTokenEntry e) => e.AppId == app.AppId);
		addinRepo.RemoveAddinFromPersistenceFile(app);
		SaveAndSendRefreshToken(applicationsToken);
		applicationHost.CleanupAppRepository(applicationsToken);
	}
}
