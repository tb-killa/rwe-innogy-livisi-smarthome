using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

public class SerializationResponse
{
	public BaseResponse ResponseObject { get; set; }

	public StringCollection ResponseBody { get; set; }
}
