using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
internal sealed class LogicalDeviceStateTypeAttribute : Attribute
{
	public Type LogicalDeviceStateType { get; set; }

	public LogicalDeviceStateTypeAttribute(Type logicalDeviceStateType)
	{
		LogicalDeviceStateType = logicalDeviceStateType;
	}
}
