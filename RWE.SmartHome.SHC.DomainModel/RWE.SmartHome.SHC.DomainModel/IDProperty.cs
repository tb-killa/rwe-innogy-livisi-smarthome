using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel;

public interface IDProperty
{
	string Name { get; }

	DPropertyType Type { get; }

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToContracts();

	global::SmartHome.SHC.API.PropertyDefinition.Property ToDeviceSDK();

	string ValueToString();
}
