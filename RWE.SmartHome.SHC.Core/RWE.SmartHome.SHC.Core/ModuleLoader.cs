using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core;

internal static class ModuleLoader
{
	public static void LoadModules(Container container)
	{
		PerformanceMonitoring.PrintMemoryUsage("Load modules...");
		List<ModuleInfo> list = new List<ModuleInfo>();
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
		XElement xElement = XElement.Load(directoryName + "\\boot.config");
		int elevatedLogLevelTimeRemaining = FilePersistence.ElevatedLogLevelTimeRemaining;
		List<string> elevatedLogLevelModules = FilePersistence.ElevatedLogLevelModules;
		if (elevatedLogLevelTimeRemaining <= 0)
		{
			elevatedLogLevelModules.Clear();
		}
		foreach (XElement item in from e in xElement.Elements()
			where e.Name == "module"
			select e)
		{
			try
			{
				XAttribute xAttribute = item.Attribute("name");
				XAttribute xAttribute2 = item.Attribute("assembly");
				XAttribute xAttribute3 = item.Attribute("class");
				XAttribute xAttribute4 = item.Attribute("logLevel");
				Module module = (Module)Enum.Parse(typeof(Module), xAttribute.Value, ignoreCase: true);
				object obj = null;
				if (xAttribute2 == null)
				{
					if (module == Module.Logging)
					{
						obj = new LoggingModule();
					}
				}
				else
				{
					string assemblyFile = $"{directoryName}\\{xAttribute2.Value}.dll";
					Assembly assembly = Assembly.LoadFrom(assemblyFile);
					Type type = assembly.GetType(xAttribute3.Value);
					obj = Activator.CreateInstance(type);
				}
				Severity logLevel = ((xAttribute4 != null) ? ((Severity)Enum.Parse(typeof(Severity), xAttribute4.Value, ignoreCase: true)) : Severity.Information);
				if (elevatedLogLevelModules.Contains(module.ToString()) && elevatedLogLevelTimeRemaining > 0)
				{
					logLevel = Severity.Debug;
				}
				list.Add(new ModuleInfo
				{
					Identifier = module,
					Instance = (IModule)obj,
					LogLevel = logLevel
				});
				PerformanceMonitoring.PrintMemoryUsage($"Module {xAttribute} loaded");
			}
			catch (Exception)
			{
				XAttribute xAttribute5 = item.Attribute("name");
				Console.WriteLine("Unable to initialize module {0}", (xAttribute5 == null) ? "unknown" : xAttribute5.Value);
				throw;
			}
		}
		PerformanceMonitoring.PrintMemoryUsage("Configure modules...");
		list.ForEach(delegate(ModuleInfo moduleInfo)
		{
			ModuleInfos.AddModuleInfo(moduleInfo);
			if (moduleInfo.Instance != null)
			{
				moduleInfo.Instance.Configure(container);
			}
			PerformanceMonitoring.PrintMemoryUsage($"Module {moduleInfo.Identifier} configured");
		});
		ModuleInfos.StartLogLevelElevationExpiration(elevatedLogLevelTimeRemaining);
	}
}
