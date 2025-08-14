using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public class StatusConverterService : IStatusConverterService
{
	public Status ToApiModel(ShcStatus shcStatus)
	{
		if (shcStatus != null)
		{
			Status status = new Status();
			status.GatewayStatus = new GatewayStatus
			{
				AppVersion = shcStatus.AppVersion,
				ConfigVersion = shcStatus.ConfigVersion,
				Connected = shcStatus.Connected,
				OsVersion = shcStatus.OsVersion
			};
			return status;
		}
		return null;
	}
}
