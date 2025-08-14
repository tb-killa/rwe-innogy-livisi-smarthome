using System.Collections.Generic;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

namespace RWE.SmartHome.SHC.BusinessLogic.Persistence;

public class AddInConfigurationContainer : List<ConfigurationItem>
{
	public AddInConfigurationContainer()
	{
	}

	public AddInConfigurationContainer(int capacity)
		: base(capacity)
	{
	}

	public AddInConfigurationContainer(IEnumerable<ConfigurationItem> collection)
		: base(collection)
	{
	}
}
