using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class MultipleRequest : BaseRequest
{
	public List<BaseRequest> RequestList { get; set; }

	public MultipleRequest(List<BaseRequest> requestList)
	{
		RequestList = requestList;
	}

	public MultipleRequest()
	{
		RequestList = new List<BaseRequest>();
	}
}
