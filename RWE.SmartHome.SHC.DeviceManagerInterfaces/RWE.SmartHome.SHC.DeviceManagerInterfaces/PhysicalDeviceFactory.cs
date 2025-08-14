using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.SerialNumber;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public static class PhysicalDeviceFactory
{
	private const string InnogyManufacturer = "Innogy";

	public static BaseDevice Create(IDeviceInformation deviceInformation, IDeviceDefinitionsProvider deviceDefinitionsProvider)
	{
		List<Property> properties = new List<Property>();
		BuiltinPhysicalDeviceType deviceType = GetDeviceType(deviceInformation);
		string manufacturer = GetManufacturer(deviceType);
		BaseDevice baseDevice = new BaseDevice();
		baseDevice.Id = deviceInformation.DeviceId;
		baseDevice.Properties = properties;
		baseDevice.AppId = CoreConstants.CoreAppId;
		baseDevice.Manufacturer = manufacturer;
		baseDevice.DeviceVersion = "1.0";
		baseDevice.DeviceType = deviceType.ToString();
		baseDevice.ProtocolId = ProtocolIdentifier.Cosip;
		baseDevice.SerialNumber = CreateSerialNumber(deviceInformation);
		baseDevice.TimeOfDiscovery = ShcDateTime.UtcNow;
		baseDevice.Name = CreateDefaultNameForDeviceType(deviceType);
		BaseDevice baseDevice2 = baseDevice;
		BaseDeviceDefinition deviceDefinition = deviceDefinitionsProvider.GetDeviceDefinition(null, deviceType.ToString(), new SipcosFirmwareVersion(deviceInformation.ManufacturerDeviceAndFirmware));
		if (deviceDefinition != null)
		{
			baseDevice2.DeviceVersion = deviceDefinition.GetAttribute<StringPropertyDefinition>("Version").DefaultValue;
			List<Property> collection = deviceDefinition.ConfigurationProperties.GenerateMissingProperties(baseDevice2.Properties);
			baseDevice2.Properties.AddRange(collection);
		}
		return baseDevice2;
	}

	public static BuiltinPhysicalDeviceType GetDeviceType(IDeviceInformation deviceInformation)
	{
		short manufacturerCode = deviceInformation.ManufacturerCode;
		uint manufacturerDeviceType = deviceInformation.ManufacturerDeviceType;
		BuiltinPhysicalDeviceType result = BuiltinPhysicalDeviceType.Unknown;
		if (manufacturerCode == 1)
		{
			switch (manufacturerDeviceType)
			{
			case 1u:
				result = BuiltinPhysicalDeviceType.RST;
				break;
			case 2u:
				result = BuiltinPhysicalDeviceType.PSS;
				break;
			case 3u:
				result = BuiltinPhysicalDeviceType.WSC2;
				break;
			case 4u:
				result = BuiltinPhysicalDeviceType.BRC8;
				break;
			case 5u:
				result = BuiltinPhysicalDeviceType.WMD;
				break;
			case 6u:
				result = BuiltinPhysicalDeviceType.WMDO;
				break;
			case 7u:
				result = BuiltinPhysicalDeviceType.PSSO;
				break;
			case 8u:
				result = BuiltinPhysicalDeviceType.WDS;
				break;
			case 9u:
				result = BuiltinPhysicalDeviceType.PSD;
				break;
			case 10u:
				result = BuiltinPhysicalDeviceType.PSR;
				break;
			case 11u:
				result = BuiltinPhysicalDeviceType.ISS2;
				break;
			case 12u:
				result = BuiltinPhysicalDeviceType.ISD2;
				break;
			case 13u:
				result = BuiltinPhysicalDeviceType.ISC2;
				break;
			case 14u:
				result = BuiltinPhysicalDeviceType.ISR2;
				break;
			case 15u:
				result = BuiltinPhysicalDeviceType.WRT;
				break;
			case 16u:
				result = BuiltinPhysicalDeviceType.FSC8;
				break;
			case 17u:
				result = BuiltinPhysicalDeviceType.RVA;
				break;
			case 18u:
				result = (deviceInformation.SupportsEncryption ? BuiltinPhysicalDeviceType.WSD2 : BuiltinPhysicalDeviceType.WSD);
				break;
			case 20u:
				result = BuiltinPhysicalDeviceType.ChargingStation;
				break;
			case 21u:
				result = BuiltinPhysicalDeviceType.RST2;
				break;
			case 22u:
				result = BuiltinPhysicalDeviceType.SIR;
				break;
			}
		}
		return result;
	}

	private static string CreateSerialNumber(IDeviceInformation deviceInformation)
	{
		SGTIN96 sGTIN = SGTIN96.Create(deviceInformation.Sgtin);
		if (deviceInformation.ProtocolType == ProtocolType.SipCos)
		{
			return RweSerialNumberStrategy.CreateSerialNumberFromSgtin(sGTIN);
		}
		if (deviceInformation.ProtocolType == ProtocolType.BidCos)
		{
			return BidCosSerialNumberStrategy.CreateSerialNumberFromSgtin(sGTIN);
		}
		return "Failed to determine serial number.";
	}

	private static string CreateDefaultNameForDeviceType(BuiltinPhysicalDeviceType deviceType)
	{
		string result = string.Empty;
		switch (deviceType)
		{
		case BuiltinPhysicalDeviceType.RST:
		case BuiltinPhysicalDeviceType.RST2:
			result = "Radiator mounted Smart Thermostat";
			break;
		case BuiltinPhysicalDeviceType.PSS:
			result = "Pluggable Smart Switch";
			break;
		case BuiltinPhysicalDeviceType.WSC2:
			result = "Wall mounted Smart Controller";
			break;
		case BuiltinPhysicalDeviceType.BRC8:
			result = "Basic Remote Control";
			break;
		case BuiltinPhysicalDeviceType.WMD:
			result = "Wall mounted Motion Detector Indoor";
			break;
		case BuiltinPhysicalDeviceType.WMDO:
			result = "Wall mounted Motion Detector Outdoor";
			break;
		case BuiltinPhysicalDeviceType.PSSO:
			result = "Pluggable Smart Switch Outdoor";
			break;
		case BuiltinPhysicalDeviceType.WDS:
			result = "Wall mounted Door/Window Sensor";
			break;
		case BuiltinPhysicalDeviceType.PSD:
			result = "Pluggable Smart Dimmer";
			break;
		case BuiltinPhysicalDeviceType.PSR:
			result = "Pluggable Smart Router";
			break;
		case BuiltinPhysicalDeviceType.ISS2:
			result = "In wall Smart Switch";
			break;
		case BuiltinPhysicalDeviceType.ISD2:
			result = "In wall Smart Dimmer";
			break;
		case BuiltinPhysicalDeviceType.ISC2:
			result = "In wall Smart Controller";
			break;
		case BuiltinPhysicalDeviceType.ISR2:
			result = "In wall Smart Roller Shutter";
			break;
		case BuiltinPhysicalDeviceType.WRT:
			result = "Wall mounted Room Thermostat";
			break;
		case BuiltinPhysicalDeviceType.FSC8:
			result = "Floor heating Smart Control";
			break;
		case BuiltinPhysicalDeviceType.RVA:
			result = "RVA";
			break;
		case BuiltinPhysicalDeviceType.WSD:
		case BuiltinPhysicalDeviceType.WSD2:
			result = "Wall mounted Smoke Detector";
			break;
		case BuiltinPhysicalDeviceType.ChargingStation:
			result = "Charging Station";
			break;
		case BuiltinPhysicalDeviceType.SIR:
			result = "Wall mounted Sirene";
			break;
		}
		return result;
	}

	private static string GetManufacturer(BuiltinPhysicalDeviceType deviceType)
	{
		if (deviceType != BuiltinPhysicalDeviceType.SIR)
		{
			return "RWE";
		}
		return "Innogy";
	}
}
