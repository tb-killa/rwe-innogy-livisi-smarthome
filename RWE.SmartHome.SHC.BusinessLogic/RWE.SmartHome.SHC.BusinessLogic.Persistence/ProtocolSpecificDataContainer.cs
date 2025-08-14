using System.Collections.Generic;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;

namespace RWE.SmartHome.SHC.BusinessLogic.Persistence;

public class ProtocolSpecificDataContainer : List<ProtocolSpecificDataEntity>
{
	public ProtocolSpecificDataContainer()
	{
	}

	public ProtocolSpecificDataContainer(int capacity)
		: base(capacity)
	{
	}

	public ProtocolSpecificDataContainer(IEnumerable<ProtocolSpecificDataEntity> collection)
		: base(collection)
	{
	}
}
