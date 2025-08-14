using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;

public class ApplicationLoadStateChangedEventArgs : EventArgs
{
	public IAddIn Application { get; set; }

	public string ApplicationVersion { get; set; }

	public ApplicationStates ApplicationState { get; set; }

	public List<Property> Parameters { get; set; }
}
