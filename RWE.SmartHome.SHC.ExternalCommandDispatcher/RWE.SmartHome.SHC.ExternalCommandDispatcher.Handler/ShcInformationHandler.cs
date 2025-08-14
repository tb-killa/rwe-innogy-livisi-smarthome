using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSettings;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.ShcSettings;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.ShcSettings;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.TypeManager;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class ShcInformationHandler : ICommandHandler, IBaseCommandHandler
{
	private IShcTypeManager shcTypeManager;

	internal ShcInformationHandler(IShcTypeManager shcTypeManager)
	{
		this.shcTypeManager = shcTypeManager;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is GetShcInformationRequest)
		{
			return GetShcInformation();
		}
		if (request is SetShcInformationRequest shcInformation)
		{
			return SetShcInformation(shcInformation);
		}
		if (request is GetShcTypeRequest request2)
		{
			return GetShcType(request2);
		}
		return null;
	}

	private static BaseResponse GetShcInformation()
	{
		ShcInformationResponse shcInformationResponse = new ShcInformationResponse();
		ShcInformation shcInformation = new ShcInformation();
		shcInformation.TimeZone = TimeZoneManager.GetShcTimeZoneName().ParseAsShcTimeZone(ignoreCase: false);
		shcInformation.CurrentShcUtcOffset = (int)TimeZoneManager.GetShcUtcOffset().TotalMinutes;
		ShcInformation information = shcInformation;
		shcInformationResponse.Information = information;
		return shcInformationResponse;
	}

	private static BaseResponse SetShcInformation(SetShcInformationRequest request)
	{
		if (request.Information.TimeZone.HasValue)
		{
			string stringValue = request.Information.TimeZone.Value.GetStringValue();
			TimeZoneManager.SetTimeZone(stringValue);
		}
		return GetShcInformation();
	}

	private BaseResponse GetShcType(GetShcTypeRequest request)
	{
		GetShcTypeResponse getShcTypeResponse = new GetShcTypeResponse();
		for (int i = 0; i < 64; i++)
		{
			if (Enum.IsDefined(typeof(ShcRestriction), 1L << i) && ((ulong)(1L << i) & (ulong)request.Restriction) != 0)
			{
				RestrictionState restrictionState = shcTypeManager.GetRestrictionState((ShcRestriction)(1L << i));
				restrictionState.Restriction = (ShcRestriction)(1L << i);
				if (getShcTypeResponse.CurrentLicensingState == null)
				{
					getShcTypeResponse.CurrentLicensingState = new List<RestrictionState>();
				}
				getShcTypeResponse.CurrentLicensingState.Add(restrictionState);
			}
		}
		return getShcTypeResponse;
	}
}
