namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class GetApplicationTokenRequest : BaseRequest, IPreserializedRequest
{
	private const string REQUEST_WITH_PLACEHOLDERS = "<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetApplicationTokenRequest\" Version=\"{0}\" RequestId=\"{1}\" />";

	public string SerializeToXml()
	{
		return $"<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetApplicationTokenRequest\" Version=\"{base.Version}\" RequestId=\"{base.RequestId}\" />";
	}
}
