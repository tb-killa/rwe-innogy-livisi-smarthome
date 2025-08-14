using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ShcSecurityNotifications;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.DomainModel.Constants;

namespace RWE.SmartHome.SHC.BusinessLogic.ShcSecurityNotifications;

public class RFCommFailureNotificationHandler : IRFCommFailureNotificationHandler
{
	private readonly IRepository repository;

	public RFCommFailureNotificationHandler(IRepository repository)
	{
		this.repository = repository;
	}

	public bool RFCommFailureNotificationEnabled()
	{
		return RFCommFailureNotificationEnabled(repository.GetShcBaseDevice().Properties);
	}

	private bool RFCommFailureNotificationEnabled(IEnumerable<Property> baseDeviceProperties)
	{
		return GetProperty(baseDeviceProperties, ShcBaseDeviceConstants.ConfigurationProperties.RFCommFailureNotification);
	}

	private bool GetProperty(IEnumerable<Property> baseDeviceProperties, string propertyName)
	{
		if (baseDeviceProperties == null)
		{
			return false;
		}
		if (baseDeviceProperties.FirstOrDefault((Property x) => x.Name.Equals(propertyName)) is BooleanProperty { Value: var value })
		{
			return value == true;
		}
		return false;
	}
}
