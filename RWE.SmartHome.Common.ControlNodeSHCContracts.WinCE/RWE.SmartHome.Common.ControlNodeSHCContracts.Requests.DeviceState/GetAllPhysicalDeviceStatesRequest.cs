namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.DeviceState;

public class GetAllPhysicalDeviceStatesRequest : BaseRequest, IPreserializedRequest
{
	private const string REQUEST_WITH_PLACEHOLDERS = "<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetAllPhysicalDeviceStatesRequest\" Version=\"{0}\" RequestId=\"{1}\" /> ";

	public string SerializeToXml()
	{
		return $"<BaseRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"GetAllPhysicalDeviceStatesRequest\" Version=\"{base.Version}\" RequestId=\"{base.RequestId}\" /> ";
	}
}
