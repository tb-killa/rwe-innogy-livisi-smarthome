using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Transformation;
using SmartHome.SHC.API.Protocols.Lemonbeat;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;
using SmartHome.SHC.API.Protocols.Lemonbeat.Gateway;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

internal static class LemonbeatApiConverters
{
	public static PhysicalDeviceDescription ToApiDeviceDescription(DeviceDescription coreDeviceDescription)
	{
		PhysicalDeviceDescription physicalDeviceDescription = new PhysicalDeviceDescription();
		if (coreDeviceDescription != null)
		{
			physicalDeviceDescription.SGTIN = coreDeviceDescription.SGTIN.GetSerialData().ToArray();
			physicalDeviceDescription.DeviceType = coreDeviceDescription.DeviceType;
			physicalDeviceDescription.ManufacturerId = coreDeviceDescription.ManufacturerId;
			physicalDeviceDescription.ManufacturerProductId = coreDeviceDescription.ManufacturerProductId;
			physicalDeviceDescription.Name = coreDeviceDescription.Name;
			physicalDeviceDescription.HardwareVersion = coreDeviceDescription.HardwareVersion;
			physicalDeviceDescription.BootLoaderVersion = coreDeviceDescription.BootloaderVersion;
			physicalDeviceDescription.StackVersion = coreDeviceDescription.StackVersion;
			physicalDeviceDescription.ApplicationVersion = coreDeviceDescription.ApplicationVersion;
			physicalDeviceDescription.ChannelMap = coreDeviceDescription.ChannelMap;
			physicalDeviceDescription.ChannelScanTime = coreDeviceDescription.ChannelScanTime;
			physicalDeviceDescription.WakeupChannel = coreDeviceDescription.WakeupChannel;
			physicalDeviceDescription.WakeupInterval = coreDeviceDescription.WakeupInterval;
			physicalDeviceDescription.WakeupOffset = coreDeviceDescription.WakeupOffset;
			physicalDeviceDescription.RadioMode = RadioModeFromCore(coreDeviceDescription.RadioMode);
			physicalDeviceDescription.MACAddress = coreDeviceDescription.MacAddress;
		}
		return physicalDeviceDescription;
	}

	private static global::SmartHome.SHC.API.Protocols.Lemonbeat.RadioMode RadioModeFromCore(RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.RadioMode mode)
	{
		return mode switch
		{
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.RadioMode.RXonly => global::SmartHome.SHC.API.Protocols.Lemonbeat.RadioMode.RXonly, 
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.RadioMode.TXonly => global::SmartHome.SHC.API.Protocols.Lemonbeat.RadioMode.TXonly, 
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.RadioMode.WakeOnEvent => global::SmartHome.SHC.API.Protocols.Lemonbeat.RadioMode.WakeOnEvent, 
			RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.RadioMode.WakeOnRadio => global::SmartHome.SHC.API.Protocols.Lemonbeat.RadioMode.WakeOnRadio, 
			_ => throw new NotImplementedException("Unsupported radio mode."), 
		};
	}

	public static List<global::SmartHome.SHC.API.Protocols.Lemonbeat.MemoryInformation> ToApiMemoryInformation(List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.MemoryInformation> coreMemoryInformation)
	{
		List<global::SmartHome.SHC.API.Protocols.Lemonbeat.MemoryInformation> apiMemoryInfos = new List<global::SmartHome.SHC.API.Protocols.Lemonbeat.MemoryInformation>();
		coreMemoryInformation?.ForEach(delegate(RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.MemoryInformation mi)
		{
			apiMemoryInfos.Add(new global::SmartHome.SHC.API.Protocols.Lemonbeat.MemoryInformation
			{
				MemoryType = MemoryInfoTypeFromCore(mi.MemoryType),
				Count = mi.Count,
				Free = mi.Free
			});
		});
		return apiMemoryInfos;
	}

	private static MemoryInfoType MemoryInfoTypeFromCore(MemoryType infoType)
	{
		return infoType switch
		{
			MemoryType.Action => MemoryInfoType.ActionItem, 
			MemoryType.Calculation => MemoryInfoType.Calculation, 
			MemoryType.Calendar => MemoryInfoType.Calendar, 
			MemoryType.PartnerInformation => MemoryInfoType.PartnerInformation, 
			MemoryType.StateMachine => MemoryInfoType.StateMachine, 
			MemoryType.StateMachineState => MemoryInfoType.StateMachineTransaction, 
			MemoryType.Timer => MemoryInfoType.Timer, 
			MemoryType.Value => MemoryInfoType.Value, 
			_ => throw new NotImplementedException("Unknown memory information type " + infoType), 
		};
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.ValueDescription ToCoreValueDescription(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.ValueDescription apiDescription)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.ValueDescription valueDescription = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.ValueDescription();
		valueDescription.Id = apiDescription.ValueId;
		valueDescription.Type = apiDescription.Type;
		valueDescription.Writeable = apiDescription.Writeable;
		valueDescription.IsVirtual = apiDescription.IsVirtual;
		valueDescription.MaxLogValues = apiDescription.MaxLogValues;
		valueDescription.MinLogInterval = apiDescription.MinLogInterval;
		valueDescription.Name = apiDescription.Name;
		valueDescription.Persistent = apiDescription.Persistent;
		valueDescription.Readable = apiDescription.Readable;
		valueDescription.HexBinaryFormat = ((apiDescription.HexBinaryFormat == null) ? null : new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.HexBinaryFormat
		{
			MaximumLength = apiDescription.HexBinaryFormat.MaximumLength
		});
		valueDescription.NumberFormat = ((apiDescription.NumberFormat == null) ? null : new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.NumberFormat
		{
			Max = apiDescription.NumberFormat.Max,
			Min = apiDescription.NumberFormat.Min,
			Step = apiDescription.NumberFormat.Step,
			Unit = apiDescription.NumberFormat.Unit
		});
		valueDescription.StringFormat = ((apiDescription.StringFormat == null) ? null : new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StringFormat
		{
			MaximumLength = apiDescription.StringFormat.MaximumLength,
			ValidValues = apiDescription.StringFormat.ValidValues
		});
		return valueDescription;
	}

	public static List<global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.ValueDescription> ToApiValueDescription(List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.ValueDescription> list)
	{
		if (list == null)
		{
			return new List<global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.ValueDescription>();
		}
		return list.Select((RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.ValueDescription corevd) => new global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.ValueDescription
		{
			HexBinaryFormat = ((corevd.HexBinaryFormat == null) ? null : new global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.HexBinaryFormat(corevd.HexBinaryFormat.MaximumLength)),
			IsVirtual = corevd.IsVirtual,
			MaxLogValues = corevd.MaxLogValues,
			MinLogInterval = corevd.MinLogInterval,
			Name = corevd.Name,
			NumberFormat = ((corevd.NumberFormat == null) ? null : new global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.NumberFormat(corevd.NumberFormat.Unit, corevd.NumberFormat.Min, corevd.NumberFormat.Max, corevd.NumberFormat.Step)),
			Persistent = corevd.Persistent,
			Readable = corevd.Readable,
			StringFormat = ((corevd.StringFormat == null) ? null : new global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions.StringFormat(corevd.StringFormat.MaximumLength, corevd.StringFormat.ValidValues)),
			Type = corevd.Type,
			ValueId = corevd.Id,
			Writeable = corevd.Writeable
		}).ToList();
	}

	public static NumberValue ToApiNumberValue(this CoreNumberValue value)
	{
		return new NumberValue(value.Id, value.Value);
	}

	public static StringValue ToApiStringValue(this CoreStringValue value)
	{
		return new StringValue(value.Id, value.Value);
	}

	public static HexBinaryValue ToApiHexBinaryValue(this CoreHexBinaryValue value)
	{
		return new HexBinaryValue(value.Id, value.Value);
	}

	public static CoreNumberValue ToCoreNumberValue(this NumberValue value)
	{
		return new CoreNumberValue(value.ValueId, value.Value);
	}

	public static CoreStringValue ToCoreStringValue(this StringValue value)
	{
		return new CoreStringValue(value.ValueId, value.Value);
	}

	public static CoreHexBinaryValue ToCoreHexBinaryValue(this HexBinaryValue value)
	{
		return new CoreHexBinaryValue(value.ValueId, value.Value);
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation ToCoreCalculation(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Calculation calculation)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation calculation2 = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.Calculation();
		calculation2.Id = calculation.CalculationId;
		calculation2.Left = calculation.Left.ToCoreCalculationOperand();
		calculation2.Method = calculation.Method.ToCoreCalculationMethod();
		calculation2.Right = calculation.Right.ToCoreCalculationOperand();
		return calculation2;
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand ToCoreCalculationOperand(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationOperand calcOp)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand calculationOperand = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationOperand();
		calculationOperand.CalculationId = calcOp.CalculationId;
		calculationOperand.CalendarId = calcOp.CalendarId;
		calculationOperand.ConstantBinary = calcOp.ConstantBinary;
		calculationOperand.ConstantNumber = (calcOp.ConstantNumber.HasValue ? new double?(Convert.ToDouble(calcOp.ConstantNumber)) : ((double?)null));
		calculationOperand.ConstantString = calcOp.ConstantString;
		calculationOperand.IsUpdated = calcOp.IsUpdated;
		calculationOperand.StateMachineId = calcOp.StateMachineId;
		calculationOperand.TimerId = calcOp.TimerId;
		calculationOperand.ValueId = calcOp.ValueId;
		calculationOperand.PartnerId = (calcOp.IsShcValue ? new uint?(PhysicalConfigurationBuilder.SHC_PARTNER_ID) : ((uint?)null));
		return calculationOperand;
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod ToCoreCalculationMethod(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod apiMethod)
	{
		return apiMethod switch
		{
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Add => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Add, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.And => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.And, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Divide => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Divide, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Equal => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Equal, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Greater => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Greater, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.GreaterOrEqual => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.GreaterOrEqual, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Modulo => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Modulo, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Multiply => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Multiply, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.NotEqual => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.NotEqual, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Or => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Or, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Smaller => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Smaller, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.SmallerOrEqual => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.SmallerOrEqual, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalculationMethod.Subtract => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations.CalculationMethod.Subtract, 
			_ => throw new ArgumentException("API Calculation method not supported: " + apiMethod), 
		};
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.LemonbeatTimer ToCoreTimer(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.LemonbeatTimer apiTimer)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.LemonbeatTimer lemonbeatTimer = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.LemonbeatTimer();
		lemonbeatTimer.ActionId = apiTimer.ActionId;
		lemonbeatTimer.CalculationId = apiTimer.CalculationId;
		lemonbeatTimer.Delay = apiTimer.Delay;
		lemonbeatTimer.Id = apiTimer.TimerId;
		return lemonbeatTimer;
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.CalendarTask ToCoreCalendar(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.CalendarTask apiCalendar)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.CalendarTask calendarTask = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.CalendarTask();
		calendarTask.ActionId = apiCalendar.ActionId;
		calendarTask.End = apiCalendar.End;
		calendarTask.Id = apiCalendar.TaskId;
		calendarTask.Repeat = apiCalendar.Repeat.ToCoreCalendarRepeat();
		calendarTask.Start = apiCalendar.Start;
		calendarTask.WeekDays = apiCalendar.WeekDays;
		return calendarTask;
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.Repeat? ToCoreCalendarRepeat(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Repeat? repeat)
	{
		if (!repeat.HasValue)
		{
			return null;
		}
		return repeat.Value switch
		{
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Repeat.Daily => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.Repeat.Daily, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Repeat.TimeInSeconds => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.Repeat.TimeInSeconds, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Repeat.Weekday => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.Repeat.Weekday, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Repeat.Weekly => RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar.Repeat.Weekly, 
			_ => throw new ArgumentException("API calendar repeat value not supported: " + repeat.ToString()), 
		};
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.StateMachine ToCoreStateMachine(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.StateMachine apiMachine)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.StateMachine result = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.StateMachine();
		result.Id = apiMachine.StateMachineId;
		if (apiMachine.States != null)
		{
			result.States = new List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.State>();
			apiMachine.States.ForEach(delegate(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.State state)
			{
				result.States.Add(state.ToCoreState());
			});
		}
		return result;
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.State ToCoreState(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.State apiState)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.State result = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.State
		{
			StateId = apiState.StateId
		};
		if (apiState.Transactions != null)
		{
			result.Transactions = new List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.Transaction>();
			apiState.Transactions.ForEach(delegate(global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Transaction tr)
			{
				result.Transactions.Add(tr.ToCoreTransaction());
			});
		}
		return result;
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.Transaction ToCoreTransaction(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Transaction apiTransaction)
	{
		RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.Transaction transaction = new RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.Transaction();
		transaction.ActionId = apiTransaction.ActionId;
		transaction.CalculationId = apiTransaction.CalculationId;
		transaction.NextStateId = apiTransaction.GotoStateId;
		return transaction;
	}

	public static RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Transport ToCoreTransport(this global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport transport)
	{
		return transport switch
		{
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport.Tcp => RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Transport.Tcp, 
			global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport.Udp => RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Transport.Udp, 
			_ => throw new NotImplementedException("Unknown API transport protocol."), 
		};
	}

	public static global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport ToApiTransport(this TransportType preferredTransportType)
	{
		if (preferredTransportType == TransportType.Connection)
		{
			return global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport.Tcp;
		}
		return global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport.Udp;
	}

	public static ServiceType ToServiceType(this LemonbeatServiceId serviceId)
	{
		return (ServiceType)serviceId;
	}

	public static LemonbeatServiceId ToLemonbeatServiceId(this ServiceType serviceId)
	{
		return (LemonbeatServiceId)serviceId;
	}
}
