using SmartHome.SHC.API.Configuration;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public interface IConfigurationTransformer
{
	TransformationResult TransformLogicalConfiguration(TargetLogicalConfiguration logicalConfiguration);
}
