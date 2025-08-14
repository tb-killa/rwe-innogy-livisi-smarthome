using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

internal class AddinsConfigurationValidator : IConfigurationValidator
{
	private readonly object syncRoot = new object();

	private readonly AddinsConfigurationRepository addinsConfigurationRepository;

	private readonly Dictionary<string, IConfigurationValidator> addinsConfigurationValidators = new Dictionary<string, IConfigurationValidator>();

	public AddinsConfigurationValidator(AddinsConfigurationRepository addinsConfigurationRepository)
	{
		this.addinsConfigurationRepository = addinsConfigurationRepository;
	}

	public void AddAddinValidator(string appId, IConfigurationValidator addinConfigurationValidator)
	{
		lock (syncRoot)
		{
			if (addinConfigurationValidator != null && !addinsConfigurationValidators.Values.Contains(addinConfigurationValidator))
			{
				addinsConfigurationValidators.Add(appId, addinConfigurationValidator);
			}
		}
	}

	public void RemoveAddinValidator(string appId)
	{
		lock (syncRoot)
		{
			addinsConfigurationValidators.Remove(appId);
		}
	}

	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		lock (syncRoot)
		{
			foreach (string key in addinsConfigurationValidators.Keys)
			{
				if (updateContextData.UpdateReport != null && !updateContextData.UpdateReport.IsAppValidationRequired(key))
				{
					continue;
				}
				IEnumerable<ErrorEntry> configurationErrors = addinsConfigurationValidators[key].GetConfigurationErrors(configuration, updateContextData);
				if (configurationErrors != null)
				{
					configurationErrors = configurationErrors.Where((ErrorEntry x) => x != null).ToList();
					list.AddRange(configurationErrors);
				}
			}
			return list;
		}
	}
}
