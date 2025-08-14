using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using SmartHome.SHC.API.DeviceActivityLogging;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity;

internal class DalTypeResolver : IDalTypeResolver
{
	private readonly Dictionary<string, IActivityLoggingControl> apps = new Dictionary<string, IActivityLoggingControl>();

	public DalTypeResolver(IApplicationsHost appHost)
	{
		Action<ApplicationLoadStateChangedEventArgs> value = delegate(ApplicationLoadStateChangedEventArgs args)
		{
			if (args != null && args.Application != null && args.ApplicationState == ApplicationStates.ApplicationActivated && args.Application is IActivityLoggingControl app)
			{
				RegisterLoggingControl(args.Application.ApplicationId, app);
			}
		};
		appHost.ApplicationStateChanged += value;
		appHost.ApplicationStateChanged += delegate(ApplicationLoadStateChangedEventArgs args)
		{
			if (args != null && args.Application != null && args.ApplicationState == ApplicationStates.ApplicationDeactivated)
			{
				UnregisterLoggingControl(args.Application.ApplicationId);
			}
		};
	}

	private void RegisterLoggingControl(string appId, IActivityLoggingControl app)
	{
		UnregisterLoggingControl(appId);
		lock (apps)
		{
			apps.Add(appId, app);
		}
	}

	private void UnregisterLoggingControl(string appId)
	{
		lock (apps)
		{
			apps.Remove(appId);
		}
	}

	public DeviceActivityLoggingType GetDeviceLoggingType(LogicalDevice capability)
	{
		DeviceActivityLoggingType result = DeviceActivityLoggingType.Core;
		if (capability != null)
		{
			BaseDevice baseDevice = capability.BaseDevice;
			if (baseDevice == null)
			{
				throw new ArgumentNullException(string.Concat("Inconsistent repository: capability ", capability.Id, " has null device"));
			}
			IActivityLoggingControl value = null;
			if (baseDevice != null && baseDevice.AppId != CoreConstants.CoreAppId)
			{
				lock (apps)
				{
					apps.TryGetValue(baseDevice.AppId, out value);
				}
			}
			if (value != null)
			{
				result = value.GetDeviceLoggingType(capability.Id);
			}
		}
		return result;
	}
}
