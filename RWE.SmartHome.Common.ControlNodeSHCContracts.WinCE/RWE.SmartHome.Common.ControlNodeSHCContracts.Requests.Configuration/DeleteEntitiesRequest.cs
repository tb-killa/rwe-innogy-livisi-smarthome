using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class DeleteEntitiesRequest : BaseRequest
{
	public List<EntityMetadata> Entities { get; set; }

	public DeleteEntitiesRequest()
	{
		Entities = new List<EntityMetadata>();
	}
}
