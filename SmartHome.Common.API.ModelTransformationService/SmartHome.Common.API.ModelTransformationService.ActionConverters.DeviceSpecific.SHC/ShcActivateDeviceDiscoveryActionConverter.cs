using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcActivateDeviceDiscoveryActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ShcActivateDeviceDiscoveryActionConverter));

	public BaseRequest ToBaseRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("ToBaseRequest");
		List<Parameter> list = ((anAction.Data != null) ? anAction.Data.Where((Parameter pp) => pp.Name == "Product").ToList() : new List<Parameter>());
		List<string> list2 = new List<string>();
		foreach (Parameter item in list)
		{
			if (item.Type != "Constant" || item.Constant == null || !RegexExpressions.ProductSpecificTypeRegex.IsMatch(item.Constant.Value.ToString()))
			{
				logger.LogAndThrow<ArgumentException>(string.Format("Invalid {0} parameter. Type {1} with {2} pattern expected.", "Product", "Constant", "/product/<product_name>/<product_version>"));
			}
			MatchCollection matchCollection = RegexExpressions.ProductSpecificTypeRegex.Matches(item.Constant.Value.ToString());
			list2.Add(string.Format("{0}{1}", "sh://", matchCollection[0].Groups[1].Value));
		}
		logger.DebugExitMethod("ToBaseRequest", $"AppIds count: {list2.Count}");
		ActivateDeviceDiscoveryRequest activateDeviceDiscoveryRequest = new ActivateDeviceDiscoveryRequest();
		activateDeviceDiscoveryRequest.AppIds = (list2.Any() ? list2 : null);
		return activateDeviceDiscoveryRequest;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(SmartHome.Common.API.Entities.Entities.Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse.GetActionResponse<AcknowledgeResponse>(anAction);
	}
}
