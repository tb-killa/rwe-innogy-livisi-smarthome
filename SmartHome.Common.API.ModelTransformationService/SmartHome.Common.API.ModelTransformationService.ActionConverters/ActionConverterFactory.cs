using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.ActionConverters.CapabilitySpecific.RollerShutterActuator;
using SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;
using SmartHome.Common.API.ModelTransformationService.ActionConverters.Generic;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters;

internal class ActionConverterFactory : IActionConverterFactory
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ActionConverterFactory));

	private Dictionary<string, IActionConverter> converters;

	public ActionConverterFactory()
	{
		RegisterCustomActionConverters();
	}

	public IActionConverter GetActionConverter(Action action)
	{
		logger.DebugEnterMethod("GetActionConverter");
		IActionConverter actionConverter = null;
		if (converters.ContainsKey(GetActionType(action.Type)))
		{
			actionConverter = converters[GetActionType(action.Type)];
		}
		if (actionConverter == null)
		{
			actionConverter = new GenericActionConverter();
		}
		logger.DebugExitMethod("GetActionConverter");
		return actionConverter;
	}

	private void RegisterCustomActionConverters()
	{
		converters = new Dictionary<string, IActionConverter>(new InvariantEqualityComparer())
		{
			{
				GetActionType("UploadLog"),
				new ShcUploadLogActionConverter()
			},
			{
				GetActionType("UploadSysInfo"),
				new ShcUploadSysInfoActionConverter()
			},
			{
				GetActionType("Restart"),
				new ShcRestartActionConverter()
			},
			{
				GetActionType("FactoryReset"),
				new ShcFactoryResetActionConverter()
			},
			{
				GetActionType("SetTimeZone"),
				new ShcSetTimeZoneActionConverter()
			},
			{
				GetActionType("GetSHCTypeInfo"),
				new ShcGetShcTypeInfoActionConverter()
			},
			{
				GetActionType("SetLoggingConfig"),
				new ShcSetLoggingConfigActionConverter()
			},
			{
				GetActionType("GetLoggingConfig"),
				new ShcGetLoggingConfigActionConverter()
			},
			{
				GetActionType("MarkMessageAsRead"),
				new ShcMarkMessageAsReadActionConverter()
			},
			{
				GetActionType("SetState"),
				new GenericActionConverter()
			},
			{
				GetActionType("ReincludeDevice"),
				new GenericActionConverter()
			},
			{
				GetActionType("ActivateDeviceDiscovery"),
				new ShcActivateDeviceDiscoveryActionConverter()
			},
			{
				GetActionType("DeactivateDeviceDiscovery"),
				new ShcDeactivateDeviceDiscoveryActionConverter()
			},
			{
				GetActionType("StartCalibration"),
				new StartCalibrationActionConverter()
			},
			{
				GetActionType("StopCalibration"),
				new StopCalibrationActionConverter()
			},
			{
				GetActionType("ResetCalibration"),
				new ResetCalibrationActionConverter()
			},
			{
				GetActionType("Generic"),
				new GenericActionConverter()
			}
		};
	}

	private string GetActionType(string actionType)
	{
		return "core.RWE/" + actionType;
	}
}
