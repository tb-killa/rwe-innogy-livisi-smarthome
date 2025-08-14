using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.Core;
using SerialAPI;
using SerialApiInterfaces;

namespace RWE.SmartHome.SHC.DeviceCommunication.SerialCommunicationModule;

public class SerialCommunicationModule : IModule
{
	public void Configure(Container container)
	{
		container.Register((Func<Container, ISerialPort>)((Container c) => new SerialDLL())).ReusedWithin(ReuseScope.Container);
		container.Resolve<ISerialPort>();
	}
}
