using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.RuleEngine.Actions;
using RWE.SmartHome.SHC.RuleEngine.Repository;
using RWE.SmartHome.SHC.RuleEngineInterfaces;

namespace RWE.SmartHome.SHC.RuleEngine;

public class RuleEngineModule : IModule
{
	public void Configure(Container container)
	{
		RulesRepository rulesRepository = new RulesRepository();
		container.Register((Func<Container, IRulesRepository>)((Container c) => rulesRepository)).ReusedWithin(ReuseScope.Container);
		IConfigurationProcessor configurationProcessor = container.Resolve<IConfigurationProcessor>();
		configurationProcessor.RulesRepository = rulesRepository;
		IEventManager eventManager = container.Resolve<IEventManager>();
		ILogicalDeviceStateRepository stateRepository = container.Resolve<ILogicalDeviceStateRepository>();
		IProtocolMultiplexer protocolMultiplexer = container.Resolve<IProtocolMultiplexer>();
		IRepository configurationRepository = container.Resolve<IRepository>();
		IApplicationsHost applicationsHost = container.Resolve<IApplicationsHost>();
		IScheduler scheduler = container.Resolve<IScheduler>();
		RuleEngineObjectsFactory objectsFactory = new RuleEngineObjectsFactory();
		DynamicSettingsResolver dynamicSettingsResolver = new DynamicSettingsResolver(objectsFactory.CreateDataBinders(stateRepository, configurationRepository, protocolMultiplexer));
		container.Register((IDynamicSettingsResolver)dynamicSettingsResolver);
		ITokenCache tokenCache = container.Resolve<ITokenCache>();
		GenericActionExecuter actionExecuter = new GenericActionExecuter(protocolMultiplexer, dynamicSettingsResolver, applicationsHost, configurationRepository, tokenCache);
		container.Register((IActionExecuter)actionExecuter);
		container.Register((Func<Container, IRuleEngine>)((Container c) => new RuleEngine(eventManager, rulesRepository, protocolMultiplexer, stateRepository, objectsFactory, configurationRepository, actionExecuter, scheduler))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IRuleEngine>();
	}
}
