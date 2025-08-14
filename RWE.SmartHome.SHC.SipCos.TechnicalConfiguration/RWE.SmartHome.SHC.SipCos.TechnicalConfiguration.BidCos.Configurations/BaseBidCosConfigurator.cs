using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Tasks;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Configurations;

public abstract class BaseBidCosConfigurator
{
	public LogicalDevice LogicalDevice { get; private set; }

	public Guid DeviceId { get; set; }

	public byte[] DeviceAddress { get; set; }

	public byte[] SourceAddress { get; set; }

	public BaseBidCosConfigurator(LogicalDevice logicalDevice, byte[] sourceAdress, byte[] deviceAdress)
	{
		LogicalDevice = logicalDevice;
		DeviceAddress = deviceAdress;
		SourceAddress = sourceAdress;
		if (logicalDevice != null)
		{
			DeviceId = logicalDevice.BaseDeviceId;
		}
	}

	public abstract IEnumerable<BaseBidCosTask> GetTasks();
}
