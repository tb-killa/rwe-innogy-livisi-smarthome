using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Tasks;

public static class BidCosTaskFactory
{
	public static BaseBidCosTask GetBidCosTask(byte[] data)
	{
		if (data == null || data.Length < 1)
		{
			throw new ArgumentException("Invalid data for BidCosTask");
		}
		byte b = data[0];
		if (b == 1)
		{
			return new ConfigBidCosTask(data);
		}
		throw new ArgumentException($"Unknown BidCosTaskType value: {data[0]}");
	}

	public static Guid ExecuteTask(BaseBidCosTask task, IBidCosConfigurator bidCosConfigurator)
	{
		BidCosTaskType type = task.Type;
		if (type == BidCosTaskType.Configuration)
		{
			ConfigBidCosTask configBidCosTask = task as ConfigBidCosTask;
			return bidCosConfigurator.Configure(configBidCosTask.SourceAddress, configBidCosTask.DestinationAddress, configBidCosTask.Channel, configBidCosTask.GetParams());
		}
		throw new ArgumentException($"Unknown task type {task.Type.ToString()}");
	}
}
