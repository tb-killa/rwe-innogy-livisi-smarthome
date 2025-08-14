namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class GetApplicationTokenHashRequest : BaseRequest, IPreserializedRequest
{
	private const string REQUEST_WITH_PLACEHOLDERS = "<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetApplicationTokenHashRequest\" Version=\"{0}\" RequestId=\"{1}\" />";

	public string SerializeToXml()
	{
		return $"<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetApplicationTokenHashRequest\" Version=\"{base.Version}\" RequestId=\"{base.RequestId}\" />";
	}
}
