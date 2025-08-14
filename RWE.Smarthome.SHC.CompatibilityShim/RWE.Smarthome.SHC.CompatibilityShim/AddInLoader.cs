using System.Reflection;
using SmartHome.SHC.API;

namespace RWE.Smarthome.SHC.CompatibilityShim;

public class AddInLoader
{
	public IAddIn LoadApplication(Assembly assembly, string typeName)
	{
		return assembly.CreateInstance(typeName) as IAddIn;
	}
}
