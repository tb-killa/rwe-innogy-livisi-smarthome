using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core;

public static class ModuleInfos
{
	private static Dictionary<Module, ModuleInfo> moduleInfos = new Dictionary<Module, ModuleInfo>();

	private static Timer loggingLevelExpirationTimer;

	private static int logLevelExpiration;

	public static int RemainingTime => logLevelExpiration;

	private static void expirationCallback(object state)
	{
		if (logLevelExpiration > 0)
		{
			FilePersistence.ElevatedLogLevelTimeRemaining = logLevelExpiration;
			logLevelExpiration--;
			return;
		}
		ResetDefaultLogLevels();
		loggingLevelExpirationTimer.Dispose();
		FilePersistence.ElevatedLogLevelModules.Clear();
		FilePersistence.ElevatedLogLevelTimeRemaining = 0;
	}

	internal static void AddModuleInfo(ModuleInfo moduleInfo)
	{
		moduleInfos.Add(moduleInfo.Identifier, moduleInfo);
	}

	public static void StartLogLevelElevationExpiration(int expireInMinutes)
	{
		if (expireInMinutes < 0)
		{
			expireInMinutes = 0;
		}
		FilePersistence.ElevatedLogLevelTimeRemaining = expireInMinutes;
		logLevelExpiration = expireInMinutes;
		loggingLevelExpirationTimer = new Timer(expirationCallback, null, 0, 60000);
	}

	public static Severity GetLogLevel(Module module)
	{
		if (!moduleInfos.ContainsKey(module))
		{
			return Severity.Information;
		}
		return moduleInfos[module].LogLevel;
	}

	public static void AdjustLogLevel(int expireInMinutes)
	{
		ResetDefaultLogLevels();
		Module[] modules = ModuleList.Modules;
		foreach (Module module in modules)
		{
			AdjustLogLevel(module, Severity.Debug);
		}
		StartLogLevelElevationExpiration(expireInMinutes);
	}

	private static void AdjustLogLevel(Module module, Severity level)
	{
		if (moduleInfos.ContainsKey(module))
		{
			moduleInfos[module].LogLevel = level;
			FilePersistence.ElevatedLogLevelModules.Add(module.ToString());
		}
		else
		{
			Console.WriteLine("AdjustLogLevel: entry for module {0} missing from boot.config!", module.ToString());
		}
	}

	private static void ResetDefaultLogLevels()
	{
		FilePersistence.ElevatedLogLevelModules.Clear();
		FilePersistence.ElevatedLogLevelTimeRemaining = 0;
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
		XElement xElement = XElement.Load(directoryName + "\\boot.config");
		foreach (XElement item in from e in xElement.Elements()
			where e.Name == "module"
			select e)
		{
			try
			{
				XAttribute xAttribute = item.Attribute("name");
				XAttribute xAttribute2 = item.Attribute("logLevel");
				Severity logLevel = ((xAttribute2 != null) ? ((Severity)Enum.Parse(typeof(Severity), xAttribute2.Value, ignoreCase: true)) : Severity.Information);
				Module key = (Module)Enum.Parse(typeof(Module), xAttribute.Value, ignoreCase: true);
				if (moduleInfos.ContainsKey(key))
				{
					moduleInfos[key].LogLevel = logLevel;
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Unable to get valid logging info settings from boot.config");
			}
		}
	}
}
