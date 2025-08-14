using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class RequiredParameterAttribute : Attribute
{
	public MessageParameterKey ParameterType { get; set; }

	public RequiredParameterAttribute(MessageParameterKey parameter)
	{
		ParameterType = parameter;
	}
}
