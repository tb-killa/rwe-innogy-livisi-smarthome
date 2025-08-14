using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos;

public class BidCosConfiguration
{
	private readonly object sync = new object();

	private List<BidCosDeviceConfiguration> deviceConfigs;

	public bool IsEmpty
	{
		get
		{
			if (deviceConfigs != null && deviceConfigs.Count >= 0)
			{
				return deviceConfigs.All((BidCosDeviceConfiguration m) => m.IsEmpty);
			}
			return true;
		}
	}

	public BidCosConfiguration()
		: this(new List<BidCosDeviceConfiguration>())
	{
	}

	public BidCosConfiguration(IEnumerable<BidCosDeviceConfiguration> deviceConfigs)
	{
		this.deviceConfigs = deviceConfigs.ToList();
	}

	public BidCosConfiguration(IEnumerable<TechnicalConfigurationEntity> entities)
	{
		PopulateFromEntites(entities);
	}

	public BidCosConfiguration GetDiff(BidCosConfiguration previousConfig)
	{
		BidCosConfiguration bidCosConfiguration = new BidCosConfiguration();
		lock (sync)
		{
			BidCosDeviceConfiguration deviceConfig;
			foreach (BidCosDeviceConfiguration deviceConfig2 in deviceConfigs)
			{
				deviceConfig = deviceConfig2;
				BidCosDeviceConfiguration bidCosDeviceConfiguration = previousConfig.deviceConfigs.FirstOrDefault((BidCosDeviceConfiguration m) => m.DeviceId == deviceConfig.DeviceId);
				BidCosDeviceConfiguration item = ((bidCosDeviceConfiguration != null) ? deviceConfig.GetDiff(bidCosDeviceConfiguration) : deviceConfig);
				bidCosConfiguration.deviceConfigs.Add(item);
			}
			return bidCosConfiguration;
		}
	}

	public IEnumerable<BidCosDeviceConfiguration> GetDeviceConfigs()
	{
		return deviceConfigs;
	}

	public void AddDeviceConfig(BidCosDeviceConfiguration deviceConfig)
	{
		lock (sync)
		{
			deviceConfigs.Add(deviceConfig);
		}
	}

	public void ClearConfig()
	{
		lock (sync)
		{
			deviceConfigs.Clear();
		}
	}

	public IEnumerable<TechnicalConfigurationEntity> GetTechEntities()
	{
		lock (sync)
		{
			return deviceConfigs.Select((BidCosDeviceConfiguration m) => m.GetTechnicalConfigurationEntity());
		}
	}

	public void RemoveDevice(Guid deviceId)
	{
		lock (sync)
		{
			deviceConfigs.RemoveAll((BidCosDeviceConfiguration m) => m.DeviceId == deviceId);
		}
	}

	private void PopulateFromEntites(IEnumerable<TechnicalConfigurationEntity> entites)
	{
		lock (sync)
		{
			deviceConfigs = entites.Select((TechnicalConfigurationEntity m) => new BidCosDeviceConfiguration(m)).ToList();
		}
	}
}
