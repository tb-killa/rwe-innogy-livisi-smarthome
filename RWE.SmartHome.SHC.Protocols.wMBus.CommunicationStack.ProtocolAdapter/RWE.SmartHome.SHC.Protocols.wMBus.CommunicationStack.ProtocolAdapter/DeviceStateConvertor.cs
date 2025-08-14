using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.wMBusProtocol;
using SmartHome.SHC.API.Protocols.wMBus;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal static class DeviceStateConvertor
{
	public static WMBusDeviceState Convert(this WMBusFrame wmBusFrame)
	{
		WMBusDeviceState wMBusDeviceState = new WMBusDeviceState();
		wMBusDeviceState.DeviceDescription = new DeviceDescription
		{
			DeviceTypeIdentification = (global::SmartHome.SHC.API.Protocols.wMBus.DeviceTypeIdentification)wmBusFrame.DeviceTypeIdentification,
			Identification = new byte[wmBusFrame.Identification.Length]
		};
		WMBusDeviceState wMBusDeviceState2 = wMBusDeviceState;
		Array.Copy(wmBusFrame.Identification, wMBusDeviceState2.DeviceDescription.Identification, wmBusFrame.Identification.Length);
		wMBusDeviceState2.DeviceDescription.Manufacturer = wmBusFrame.Manufacturer;
		wMBusDeviceState2.DeviceDescription.VersionIdentification = wmBusFrame.VersionIdentification;
		VariableDataStructure variableDataStructure = VariableDataStructure.Create(wmBusFrame.Payload.ToArray(), wmBusFrame.ControlInformation);
		RWE.SmartHome.SHC.wMBusProtocol.FixedDataHeader header = variableDataStructure.Header;
		switch ((int)wmBusFrame.ControlInformation)
		{
		case 114:
			wMBusDeviceState2.DataHeader = new global::SmartHome.SHC.API.Protocols.wMBus.FixedDataHeader(header.ManufacturerId, header.Version, (global::SmartHome.SHC.API.Protocols.wMBus.DeviceTypeIdentification)header.Medium, header.AccessNumber, header.Status, header.Signature, header.IdentNr);
			break;
		case 122:
			wMBusDeviceState2.DataHeader = new BasicDataHeader(header.AccessNumber, header.Status, header.Signature);
			break;
		}
		wMBusDeviceState2.ManufacturerSpecificData = variableDataStructure.ManufacturerSpecificData.ToArray();
		foreach (VariableDataBlock record in variableDataStructure.Records)
		{
			try
			{
				VariableDataEntry variableDataEntry = record.Convert();
				if (variableDataEntry != null && variableDataEntry.HasData)
				{
					wMBusDeviceState2.VariableDataEntries.Add(variableDataEntry);
				}
			}
			catch (Exception ex)
			{
				Log.Warning(Module.wMBusProtocolAdapter, string.Concat("Could not parse data information field with VIF = ", record.Vif, " ", ex.ToString()));
			}
		}
		return wMBusDeviceState2;
	}

	public static VariableDataEntry Convert(this VariableDataBlock variableDataBlock)
	{
		if (variableDataBlock.Dif.DataField == DataFieldCode.NoData)
		{
			return null;
		}
		VariableDataEntry variableDataEntry = null;
		if (variableDataBlock.Dif.DataField != DataFieldCode.VariableLength)
		{
			try
			{
				switch (variableDataBlock.Vif.ValueInformation)
				{
				case ValueInformationFieldCode.TimePointDate:
				{
					DateTime dateTime2 = DateTimeConverter.ToDate(variableDataBlock.Data);
					variableDataEntry = new DateTimeValue(dateTime2);
					break;
				}
				case ValueInformationFieldCode.TimePointDateTime:
				{
					DateTime dateTime = DateTimeConverter.ToDate(variableDataBlock.Data);
					variableDataEntry = new DateTimeValue(dateTime);
					break;
				}
				default:
				{
					decimal? num = DataField.ToNumericalValue(variableDataBlock.Data, variableDataBlock.Dif.DataField);
					if (num.HasValue)
					{
						variableDataEntry = new NumericalValue(num.Value);
						break;
					}
					variableDataEntry = new NumericalValue(0m);
					variableDataEntry.HasData = false;
					break;
				}
				}
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}
		else
		{
			string stringValue = DataField.ToStringValue(variableDataBlock.Data, variableDataBlock.Dif.DataField);
			variableDataEntry = new StringValue(stringValue);
		}
		if (variableDataEntry == null)
		{
			return null;
		}
		variableDataEntry.FunctionCode = (FunctionCode)variableDataBlock.Dif.FunctionField;
		variableDataEntry.StorageNumber = variableDataBlock.StorageNumber;
		variableDataEntry.SubUnit = variableDataBlock.SubUnit;
		variableDataEntry.Tariff = variableDataBlock.Tariff;
		variableDataEntry.ValueInformation = new List<ValueInformationCode>();
		double factor = 0.0;
		variableDataEntry.ValueInformation.Add(variableDataBlock.Vif.ValueInformation.Convert(ref factor));
		variableDataEntry.Factor = factor;
		foreach (ValueInformationFieldBase item in variableDataBlock.Vife)
		{
			if (item is ValueInformationFieldFD valueInformationFieldFD)
			{
				variableDataEntry.ValueInformation.Add(valueInformationFieldFD.ValueInformation.Convert());
			}
			else if (item is ValueInformationField valueInformationField)
			{
				variableDataEntry.ValueInformation.Add(valueInformationField.ValueInformation.Convert(ref factor));
				variableDataEntry.Factor = factor;
			}
		}
		variableDataEntry.ExtensionBit = variableDataBlock.Dif.ExtensionBit;
		return variableDataEntry;
	}

	public static ValueInformationCode Convert(this ValueInformationFieldCodeExtensionFD valueInformationFieldCode)
	{
		return valueInformationFieldCode switch
		{
			ValueInformationFieldCodeExtensionFD.CreditN0 => ValueInformationCode.CreditN0, 
			ValueInformationFieldCodeExtensionFD.CreditN1 => ValueInformationCode.CreditN1, 
			ValueInformationFieldCodeExtensionFD.CreditN2 => ValueInformationCode.CreditN2, 
			ValueInformationFieldCodeExtensionFD.CreditN3 => ValueInformationCode.CreditN3, 
			ValueInformationFieldCodeExtensionFD.DebitN0 => ValueInformationCode.DebitN0, 
			ValueInformationFieldCodeExtensionFD.DebitN1 => ValueInformationCode.DebitN1, 
			ValueInformationFieldCodeExtensionFD.DebitN2 => ValueInformationCode.DebitN2, 
			ValueInformationFieldCodeExtensionFD.DebitN3 => ValueInformationCode.DebitN3, 
			ValueInformationFieldCodeExtensionFD.AccessNumber => ValueInformationCode.AccessNumber, 
			ValueInformationFieldCodeExtensionFD.Medium => ValueInformationCode.Medium, 
			ValueInformationFieldCodeExtensionFD.Manufacturer => ValueInformationCode.Manufacturer, 
			ValueInformationFieldCodeExtensionFD.ParameterSetIdentification => ValueInformationCode.ParameterSetIdentification, 
			ValueInformationFieldCodeExtensionFD.ModelVersion => ValueInformationCode.ModelVersion, 
			ValueInformationFieldCodeExtensionFD.HardwareVersion => ValueInformationCode.HardwareVersion, 
			ValueInformationFieldCodeExtensionFD.FirmwareVersion => ValueInformationCode.FirmwareVersion, 
			ValueInformationFieldCodeExtensionFD.SoftwareVersion => ValueInformationCode.SoftwareVersion, 
			ValueInformationFieldCodeExtensionFD.CustomerLocation => ValueInformationCode.CustomerLocation, 
			ValueInformationFieldCodeExtensionFD.Customer => ValueInformationCode.Customer, 
			ValueInformationFieldCodeExtensionFD.AccessCodeUser => ValueInformationCode.AccessCodeUser, 
			ValueInformationFieldCodeExtensionFD.AccessCodeOperator => ValueInformationCode.AccessCodeOperator, 
			ValueInformationFieldCodeExtensionFD.AceessCodeSystemOperator => ValueInformationCode.AceessCodeSystemOperator, 
			ValueInformationFieldCodeExtensionFD.AccessCodeDeveloper => ValueInformationCode.AccessCodeDeveloper, 
			ValueInformationFieldCodeExtensionFD.Password => ValueInformationCode.Password, 
			ValueInformationFieldCodeExtensionFD.ErrorFlags => ValueInformationCode.ErrorFlags, 
			ValueInformationFieldCodeExtensionFD.ErrorMask => ValueInformationCode.ErrorMask, 
			ValueInformationFieldCodeExtensionFD.DigitalOutput => ValueInformationCode.DigitalOutput, 
			ValueInformationFieldCodeExtensionFD.DigitalInput => ValueInformationCode.DigitalInput, 
			ValueInformationFieldCodeExtensionFD.Baudrate => ValueInformationCode.Baudrate, 
			ValueInformationFieldCodeExtensionFD.ResponseDelayTime => ValueInformationCode.ResponseDelayTime, 
			ValueInformationFieldCodeExtensionFD.Retry => ValueInformationCode.Retry, 
			_ => throw new ArgumentOutOfRangeException("valueInformationFieldCode"), 
		};
	}

	public static ValueInformationCode Convert(this ValueInformationFieldCode valueInformationFieldCode, ref double factor)
	{
		switch (valueInformationFieldCode)
		{
		case ValueInformationFieldCode.Energy_Wh_0_001:
			factor = 0.001;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_Wh_0_010:
			factor = 0.01;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_Wh_0_100:
			factor = 0.1;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_Wh_1:
			factor = 1.0;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_Wh_10:
			factor = 10.0;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_Wh_100:
			factor = 100.0;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_kWh:
			factor = 1000.0;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_kWh_10:
			factor = 10000.0;
			return ValueInformationCode.Energy_Wh;
		case ValueInformationFieldCode.Energy_kJ_0_001:
			factor = 0.001;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Energy_kJ_0_010:
			factor = 0.01;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Energy_kJ_0_100:
			factor = 0.1;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Energy_kJ_1:
			factor = 1.0;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Energy_kJ_10:
			factor = 10.0;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Energy_kJ_100:
			factor = 100.0;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Energy_kJ_1000:
			factor = 1000.0;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Energy_kJ_10000:
			factor = 10000.0;
			return ValueInformationCode.Energy_kJ;
		case ValueInformationFieldCode.Volume_l_0_001:
			factor = 0.001;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Volume_l_0_010:
			factor = 0.01;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Volume_l_0_100:
			factor = 0.1;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Volume_l_1:
			factor = 1.0;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Volume_l_10:
			factor = 10.0;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Volume_l_100:
			factor = 100.0;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Volume_l_1000:
			factor = 1000.0;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Volume_l_10000:
			factor = 10000.0;
			return ValueInformationCode.Volume_l;
		case ValueInformationFieldCode.Mass_kg_0_001:
			factor = 0.001;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.Mass_kg_0_010:
			factor = 0.01;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.Mass_kg_0_100:
			factor = 0.1;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.Mass_kg_1:
			factor = 1.0;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.Mass_kg_10:
			factor = 10.0;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.Mass_kg_100:
			factor = 100.0;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.Mass_kg_1000:
			factor = 1000.0;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.Mass_kg_10000:
			factor = 10000.0;
			return ValueInformationCode.Mass_kg;
		case ValueInformationFieldCode.OnTimeSeonds:
			factor = 1.0;
			return ValueInformationCode.OnTimeSeonds;
		case ValueInformationFieldCode.OnTimeMinutes:
			factor = 60.0;
			return ValueInformationCode.OnTimeSeonds;
		case ValueInformationFieldCode.OnTimeHours:
			factor = 3600.0;
			return ValueInformationCode.OnTimeSeonds;
		case ValueInformationFieldCode.OnTimeDays:
			factor = 86400.0;
			return ValueInformationCode.OnTimeSeonds;
		case ValueInformationFieldCode.OperatingTimeSeconds:
			factor = 1.0;
			return ValueInformationCode.OperatingTimeSeconds;
		case ValueInformationFieldCode.OperatingTimeMinutes:
			factor = 60.0;
			return ValueInformationCode.OperatingTimeSeconds;
		case ValueInformationFieldCode.OperatingTimeHours:
			factor = 3600.0;
			return ValueInformationCode.OperatingTimeSeconds;
		case ValueInformationFieldCode.OperatingTimeDays:
			factor = 86400.0;
			return ValueInformationCode.OperatingTimeSeconds;
		case ValueInformationFieldCode.Power_W_0_001:
			factor = 0.001;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_W_0_010:
			factor = 0.01;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_W_0_100:
			factor = 0.1;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_W_1:
			factor = 1.0;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_W_10:
			factor = 10.0;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_W_100:
			factor = 100.0;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_W_1000:
			factor = 1000.0;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_W_10000:
			factor = 10000.0;
			return ValueInformationCode.Power_W;
		case ValueInformationFieldCode.Power_J_h_1:
			factor = 1.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.Power_J_h_10:
			factor = 10.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.Power_J_h_100:
			factor = 100.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.Power_kJ_h_1:
			factor = 1000.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.Power_kJ_h_10:
			factor = 10000.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.Power_kJ_h_100:
			factor = 100000.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.Power_kJ_h_1000:
			factor = 1000000.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.Power_kJ_h_10000:
			factor = 10000000.0;
			return ValueInformationCode.Power_J_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_0_001:
			factor = 0.001;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_0_010:
			factor = 0.01;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_0_100:
			factor = 0.1;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_1:
			factor = 1.0;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_10:
			factor = 10.0;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_100:
			factor = 100.0;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_1000:
			factor = 1000.0;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlow_l_h_10000:
			factor = 10000.0;
			return ValueInformationCode.VolumeFlow_l_h;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_0_0001:
			factor = 0.0001;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_0_0010:
			factor = 0.001;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_0_0100:
			factor = 0.01;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_0_1000:
			factor = 0.1;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_1:
			factor = 1.0;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_10:
			factor = 10.0;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_100:
			factor = 100.0;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_l_min_1000:
			factor = 1000.0;
			return ValueInformationCode.VolumeFlowExt_l_min;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_0_001:
			factor = 0.001;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_0_010:
			factor = 0.01;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_0_100:
			factor = 0.1;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_1:
			factor = 1.0;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_10:
			factor = 10.0;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_100:
			factor = 100.0;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_1000:
			factor = 1000.0;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.VolumeFlowExt_ml_s_10000:
			factor = 10000.0;
			return ValueInformationCode.VolumeFlowExt_ml_s;
		case ValueInformationFieldCode.MassFlow_kg_h_0_001:
			factor = 0.001;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.MassFlow_kg_h_0_010:
			factor = 0.01;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.MassFlow_kg_h_0_100:
			factor = 0.1;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.MassFlow_kg_h_1:
			factor = 1.0;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.MassFlow_kg_h_10:
			factor = 10.0;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.MassFlow_kg_h_100:
			factor = 100.0;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.MassFlow_kg_h_1000:
			factor = 1000.0;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.MassFlow_kg_h_10000:
			factor = 10000.0;
			return ValueInformationCode.MassFlow_kg_h;
		case ValueInformationFieldCode.FlowTemperature_C_0_001:
			factor = 0.001;
			return ValueInformationCode.FlowTemperature_C;
		case ValueInformationFieldCode.FlowTemperature_C_0_010:
			factor = 0.01;
			return ValueInformationCode.FlowTemperature_C;
		case ValueInformationFieldCode.FlowTemperature_C_0_100:
			factor = 0.1;
			return ValueInformationCode.FlowTemperature_C;
		case ValueInformationFieldCode.FlowTemperature_C_1:
			factor = 1.0;
			return ValueInformationCode.FlowTemperature_C;
		case ValueInformationFieldCode.ReturnTemperature_C_0_001:
			factor = 0.001;
			return ValueInformationCode.ReturnTemperature_C;
		case ValueInformationFieldCode.ReturnTemperature_C_0_010:
			factor = 0.01;
			return ValueInformationCode.ReturnTemperature_C;
		case ValueInformationFieldCode.ReturnTemperature_C_0_100:
			factor = 0.1;
			return ValueInformationCode.ReturnTemperature_C;
		case ValueInformationFieldCode.ReturnTemperature_C_1:
			factor = 1.0;
			return ValueInformationCode.ReturnTemperature_C;
		case ValueInformationFieldCode.TemperatureDifference_mK_1:
			factor = 1.0;
			return ValueInformationCode.TemperatureDifference_mK;
		case ValueInformationFieldCode.TemperatureDifference_mK_10:
			factor = 10.0;
			return ValueInformationCode.TemperatureDifference_mK;
		case ValueInformationFieldCode.TemperatureDifference_mK_100:
			factor = 100.0;
			return ValueInformationCode.TemperatureDifference_mK;
		case ValueInformationFieldCode.TemperatureDifference_mK_1000:
			factor = 1000.0;
			return ValueInformationCode.TemperatureDifference_mK;
		case ValueInformationFieldCode.ExternalTemperature_C_0_001:
			factor = 0.001;
			return ValueInformationCode.ExternalTemperature_C;
		case ValueInformationFieldCode.ExternalTemperature_C_0_010:
			factor = 0.01;
			return ValueInformationCode.ExternalTemperature_C;
		case ValueInformationFieldCode.ExternalTemperature_C_0_100:
			factor = 0.1;
			return ValueInformationCode.ExternalTemperature_C;
		case ValueInformationFieldCode.ExternalTemperature_C_1:
			factor = 1.0;
			return ValueInformationCode.ExternalTemperature_C;
		case ValueInformationFieldCode.Pressure_mbar_1:
			factor = 1.0;
			return ValueInformationCode.Pressure_mbar;
		case ValueInformationFieldCode.TimePointDate:
			factor = 1.0;
			return ValueInformationCode.TimePointDateTime;
		case ValueInformationFieldCode.TimePointDateTime:
			factor = 1.0;
			return ValueInformationCode.TimePointDateTime;
		case ValueInformationFieldCode.UnitsForHCA:
			factor = 1.0;
			return ValueInformationCode.UnitsForHCA;
		case ValueInformationFieldCode.AveragingDurationSeconds:
			factor = 1.0;
			return ValueInformationCode.AveragingDurationSeconds;
		case ValueInformationFieldCode.AveragingDurationMinutes:
			factor = 60.0;
			return ValueInformationCode.AveragingDurationSeconds;
		case ValueInformationFieldCode.AveragingDurationHours:
			factor = 3600.0;
			return ValueInformationCode.AveragingDurationSeconds;
		case ValueInformationFieldCode.AveragingDurationDays:
			factor = 86400.0;
			return ValueInformationCode.AveragingDurationSeconds;
		case ValueInformationFieldCode.ActualityDurationSeconds:
			factor = 1.0;
			return ValueInformationCode.ActualityDurationSeconds;
		case ValueInformationFieldCode.ActualityDurationMinutes:
			factor = 60.0;
			return ValueInformationCode.ActualityDurationSeconds;
		case ValueInformationFieldCode.ActualityDurationHours:
			factor = 3600.0;
			return ValueInformationCode.ActualityDurationSeconds;
		case ValueInformationFieldCode.ActualityDurationDays:
			factor = 86400.0;
			return ValueInformationCode.ActualityDurationSeconds;
		case ValueInformationFieldCode.FabricationNo:
			factor = 1.0;
			return ValueInformationCode.FabricationNo;
		case ValueInformationFieldCode.Enhanced:
			factor = 1.0;
			return ValueInformationCode.Enhanced;
		case ValueInformationFieldCode.BusAddress:
			factor = 1.0;
			return ValueInformationCode.BusAddress;
		case ValueInformationFieldCode.ExtensionFD:
			factor = 1.0;
			return ValueInformationCode.ExtensionFD;
		case ValueInformationFieldCode.ExtensionFB:
			factor = 1.0;
			return ValueInformationCode.ExtensionFB;
		case ValueInformationFieldCode.InformationField:
			factor = 1.0;
			return ValueInformationCode.InformationField;
		default:
			throw new ArgumentOutOfRangeException("valueInformationFieldCode");
		}
	}
}
