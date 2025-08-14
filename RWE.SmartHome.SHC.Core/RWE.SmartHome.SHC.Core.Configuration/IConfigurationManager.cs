namespace RWE.SmartHome.SHC.Core.Configuration;

public interface IConfigurationManager : IService
{
	ConfigurationSection this[string sectionName] { get; }
}
