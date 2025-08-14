using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using SmartHome.SHC.API.Storage;

namespace RWE.SmartHome.SHC.ApplicationsHost.Storage;

public class ApplicationStorage : IIsolatedStorage
{
	private IApplicationsSettings persistence;

	private string applicationId;

	public ApplicationStorage(IApplicationsSettings persistence, string applicationId)
	{
		this.persistence = persistence;
		this.applicationId = applicationId;
	}

	public string GetValue(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("GetValue: name");
		}
		return persistence.GetSetting(applicationId, name)?.Value;
	}

	public void SetValue(string name, string value)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("SetValue: name");
		}
		if (value == null)
		{
			throw new ArgumentNullException("SetValue: value");
		}
		persistence.SetValue(new ConfigurationItem
		{
			ApplicationId = applicationId,
			Name = name,
			Value = value
		});
	}

	public IDictionary<string, string> GetAllSettings()
	{
		return persistence.GetAllSettings(applicationId).ToDictionary((ConfigurationItem cfg) => cfg.Name, (ConfigurationItem cfg) => cfg.Value);
	}

	public bool Delete(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("Delete: name");
		}
		return persistence.Delete(applicationId, name);
	}

	public void DeleteAllSettings()
	{
		persistence.RemoveAllApplicationSettings(applicationId);
	}

	public IList<string> GetAllNames()
	{
		return persistence.GetAllNames(applicationId);
	}

	internal void Uninstall()
	{
		persistence.RemoveAllApplicationSettings(applicationId);
	}
}
