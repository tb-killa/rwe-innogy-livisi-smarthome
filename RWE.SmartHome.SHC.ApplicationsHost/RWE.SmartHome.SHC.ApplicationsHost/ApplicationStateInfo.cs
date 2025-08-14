using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.SHC.API;

namespace RWE.SmartHome.SHC.ApplicationsHost;

public class ApplicationStateInfo
{
	public IAddIn Application { get; set; }

	public List<ApplicationParameter> Parameters { get; set; }

	public ApplicationState State { get; set; }

	public bool IsActive
	{
		get
		{
			if (State != ApplicationState.Active)
			{
				return State == ApplicationState.ActiveByDefault;
			}
			return true;
		}
	}

	public string Version { get; set; }

	public bool UpdateAvailable { get; set; }

	public DateTime LastDownloadFailure { get; set; }

	public bool EnabledByUser { get; set; }

	public bool EnabledByWebshop { get; set; }

	public ApplicationStateInfo(IAddIn app, ApplicationState state)
	{
		Application = app;
		State = state;
	}
}
