using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class MultipleResponse : BaseResponse
{
	public List<BaseResponse> ResponseList { get; set; }

	public MultipleResponse(List<BaseResponse> responseList)
	{
		ResponseList = responseList;
	}

	public MultipleResponse()
	{
		ResponseList = new List<BaseResponse>();
	}
}
