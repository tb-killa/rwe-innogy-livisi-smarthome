using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using SmartHome.Common.API.ModelTransformationService;

namespace WebServerHost.Services;

internal class ConversionContext : IConversionContext
{
	private IRepository repository;

	public List<BaseDevice> Devices => repository.GetOriginalBaseDevices();

	public List<LogicalDevice> LogicalDevices => repository.GetOriginalLogicalDevices();

	public ConversionContext(IRepository repository)
	{
		this.repository = repository;
	}
}
