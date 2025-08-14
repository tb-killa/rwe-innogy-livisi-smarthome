namespace SmartHome.SHC.API.PropertyDefinition;

public interface Property
{
	string Name { get; }

	string GetValueAsString();
}
