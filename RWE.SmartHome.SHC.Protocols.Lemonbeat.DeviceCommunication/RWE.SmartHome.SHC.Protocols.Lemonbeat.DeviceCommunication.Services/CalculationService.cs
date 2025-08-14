using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calculation;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class CalculationService : SenderService<network>, ICalculationService
{
	private const string DefaultNamespace = "urn:calculationxsd";

	public CalculationService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Calculation, "urn:calculationxsd")
	{
	}

	public IEnumerable<Calculation> GetAllCalculations(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].ItemsElementName = new ItemsChoiceType[1] { ItemsChoiceType.calculation_get };
		network.device[0].Items = new object[1]
		{
			new calculationGetType
			{
				calculation_idSpecified = false
			}
		};
		network network2 = SendRequest(identifier, network);
		if (network2 != null && network2.device != null)
		{
			networkDevice networkDevice = network2.device.Where((networkDevice d) => d.device_id == identifier.SubDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Items != null)
			{
				calculationReportType calculationReportType = networkDevice.Items.OfType<calculationReportType>().FirstOrDefault();
				if (calculationReportType != null && calculationReportType.calculation != null)
				{
					return calculationReportType.calculation.Select((calculationType calculation) => ToDomainModelCalculation(calculation));
				}
			}
		}
		return new List<Calculation>();
	}

	public void SetAndDeleteCalculations(DeviceIdentifier identifier, IEnumerable<Calculation> toSet, IEnumerable<uint> toDelete)
	{
		network network = CreateNetworkMessage(identifier);
		calculationSetType calculationSetType = new calculationSetType();
		calculationSetType.calculation = toSet.Select((Calculation calculation) => ToWireCalculation(calculation)).ToArray();
		calculationSetType item = calculationSetType;
		List<object> list = toDelete.Select((Func<uint, object>)((uint state) => new calculationDeleteType
		{
			calculation_idSpecified = true,
			calculation_id = state
		})).ToList();
		list.Add(item);
		ItemsChoiceType[] array = new ItemsChoiceType[toDelete.Count() + 1];
		for (int num = 0; num < array.Count() - 2; num++)
		{
			array[num] = ItemsChoiceType.calculation_delete;
		}
		array[array.Count() - 1] = ItemsChoiceType.calculation_set;
		network.device[0].Items = list.ToArray();
		network.device[0].ItemsElementName = array;
		SendMessage(identifier, network, TransportType.Connection);
	}

	private Calculation ToDomainModelCalculation(calculationType calculation)
	{
		Calculation calculation2 = new Calculation();
		calculation2.Id = calculation.calculation_id;
		calculation2.Left = ToDomainModelCalculationOperand(calculation.left);
		calculation2.Right = ToDomainModelCalculationOperand(calculation.right);
		calculation2.Method = ToDomainModelCalculationMethod(calculation.method_id);
		return calculation2;
	}

	private CalculationMethod ToDomainModelCalculationMethod(uint p)
	{
		return (CalculationMethod)p;
	}

	private CalculationOperand ToDomainModelCalculationOperand(calSubType calSubType)
	{
		CalculationOperand calculationOperand = new CalculationOperand();
		calculationOperand.CalculationId = (calSubType.calculation_idSpecified ? new uint?(calSubType.calculation_id) : ((uint?)null));
		calculationOperand.CalendarId = (calSubType.calender_idSpecified ? new uint?(calSubType.calender_id) : ((uint?)null));
		calculationOperand.ConstantBinary = calSubType.constant_hexBinary;
		calculationOperand.ConstantNumber = (calSubType.constant_numberSpecified ? new double?(calSubType.constant_number) : ((double?)null));
		calculationOperand.ConstantString = calSubType.constant_string;
		calculationOperand.PartnerId = (calSubType.partner_idSpecified ? new uint?(calSubType.partner_id) : ((uint?)null));
		calculationOperand.StateMachineId = (calSubType.statemachine_idSpecified ? new uint?(calSubType.statemachine_id) : ((uint?)null));
		calculationOperand.TimerId = (calSubType.timer_idSpecified ? new uint?(calSubType.timer_id) : ((uint?)null));
		calculationOperand.ValueId = (calSubType.value_idSpecified ? new uint?(calSubType.value_id) : ((uint?)null));
		calculationOperand.IsUpdated = (calSubType.is_updatedSpecified ? new bool?(calSubType.is_updated != 0) : ((bool?)null));
		return calculationOperand;
	}

	private calculationType ToWireCalculation(Calculation calculation)
	{
		calculationType calculationType = new calculationType();
		calculationType.calculation_id = calculation.Id;
		calculationType.left = ToWireCalculationOperand(calculation.Left);
		calculationType.right = ToWireCalculationOperand(calculation.Right);
		calculationType.method_id = ToWireMethodId(calculation.Method);
		return calculationType;
	}

	private uint ToWireMethodId(CalculationMethod calculationMethod)
	{
		return (uint)calculationMethod;
	}

	private calSubType ToWireCalculationOperand(CalculationOperand calculationOperand)
	{
		calSubType calSubType = new calSubType();
		calSubType.calculation_id = calculationOperand.CalculationId ?? 0;
		calSubType.calculation_idSpecified = calculationOperand.CalculationId.HasValue;
		calSubType.calender_id = calculationOperand.CalendarId ?? 0;
		calSubType.calender_idSpecified = calculationOperand.CalendarId.HasValue;
		calSubType.constant_hexBinary = calculationOperand.ConstantBinary;
		calSubType.constant_number = calculationOperand.ConstantNumber ?? 0.0;
		calSubType.constant_numberSpecified = calculationOperand.ConstantNumber.HasValue;
		calSubType.constant_string = calculationOperand.ConstantString;
		calSubType.partner_id = calculationOperand.PartnerId ?? 0;
		calSubType.partner_idSpecified = calculationOperand.PartnerId.HasValue;
		calSubType.statemachine_id = calculationOperand.StateMachineId ?? 0;
		calSubType.statemachine_idSpecified = calculationOperand.StateMachineId.HasValue;
		calSubType.timer_id = calculationOperand.TimerId ?? 0;
		calSubType.timer_idSpecified = calculationOperand.TimerId.HasValue;
		calSubType.value_id = calculationOperand.ValueId ?? 0;
		calSubType.value_idSpecified = calculationOperand.ValueId.HasValue;
		calSubType.is_updated = (byte)((calculationOperand.IsUpdated ?? false) ? 1 : 0);
		calSubType.is_updatedSpecified = calculationOperand.IsUpdated.HasValue;
		return calSubType;
	}

	private network CreateNetworkMessage(DeviceIdentifier target)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				device_idSpecified = target.SubDeviceId.HasValue,
				device_id = (target.SubDeviceId ?? 0)
			}
		};
		return network;
	}
}
