using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class GetEntitiesRequest : BaseRequest, IPreserializedRequest
{
	private const string REQUEST_WITH_PLACEHOLDERS = "<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetEntitiesRequest\" Version=\"{0}\" RequestId=\"{1}\"><EntityType>{2}</EntityType></BaseRequest> ";

	public EntityType EntityType { get; set; }

	public string SerializeToXml()
	{
		return $"<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetEntitiesRequest\" Version=\"{base.Version}\" RequestId=\"{base.RequestId}\"><EntityType>{EntityType}</EntityType></BaseRequest> ";
	}
}
