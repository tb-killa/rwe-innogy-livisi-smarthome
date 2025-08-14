using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

internal class FunctionBinder : BaseDataBinder<FunctionBinding>
{
	private List<IDataBinder> operandBinders;

	private Dictionary<FunctionIdentifier, Func<IList<IComparable>, IComparable>> functionMap;

	private Dictionary<FunctionIdentifier, Func<IList<IComparable>, IComparable>> timeFunctionMap;

	private Dictionary<FunctionIdentifier, Func<IList<Parameter>, EventContext, IComparable>> contextBoundFunctionMap;

	private readonly int LastDayOfMonth = int.MinValue;

	private readonly int LastWeekdayofMonth = 16;

	private readonly ILogicalDeviceStateRepository repository;

	private readonly StatePropertyResolver propertyResolver;

	private readonly FunctionIdentifier[] contextBoundFunctions = new FunctionIdentifier[4]
	{
		FunctionIdentifier.GetEntityStateProperty,
		FunctionIdentifier.GetMinutesSinceLastChange,
		FunctionIdentifier.GetEventProperty,
		FunctionIdentifier.HasEventProperty
	};

	private readonly FunctionIdentifier[] booleanFunctions = new FunctionIdentifier[9]
	{
		FunctionIdentifier.Equal,
		FunctionIdentifier.NotEqual,
		FunctionIdentifier.Smaller,
		FunctionIdentifier.Greater,
		FunctionIdentifier.SmallerOrEqual,
		FunctionIdentifier.GreaterOrEqual,
		FunctionIdentifier.And,
		FunctionIdentifier.Or,
		FunctionIdentifier.InBetween
	};

	private readonly FunctionIdentifier[] numericFunctions = new FunctionIdentifier[30]
	{
		FunctionIdentifier.Add,
		FunctionIdentifier.Subtract,
		FunctionIdentifier.Multiply,
		FunctionIdentifier.Divide,
		FunctionIdentifier.Modulo,
		FunctionIdentifier.Min,
		FunctionIdentifier.Max,
		FunctionIdentifier.Pow,
		FunctionIdentifier.Exp,
		FunctionIdentifier.Log,
		FunctionIdentifier.Abs,
		FunctionIdentifier.Round,
		FunctionIdentifier.BitwiseAnd,
		FunctionIdentifier.BitwiseOr,
		FunctionIdentifier.BitwiseXOR,
		FunctionIdentifier.BitwiseNot,
		FunctionIdentifier.BitwiseLeftShift,
		FunctionIdentifier.BitwiseRightShift,
		FunctionIdentifier.GetMinute,
		FunctionIdentifier.GetHour,
		FunctionIdentifier.GetDayOfWeek,
		FunctionIdentifier.GetDayOfMonth,
		FunctionIdentifier.GetWeekdayOfMonth,
		FunctionIdentifier.GetMonth,
		FunctionIdentifier.GetYear,
		FunctionIdentifier.GetDayOfCentury,
		FunctionIdentifier.GetWeekOfCentury,
		FunctionIdentifier.GetMonthOfCentury,
		FunctionIdentifier.Average,
		FunctionIdentifier.GetMinuteOfDay
	};

	private readonly FunctionIdentifier[] dateTimeFunctions = new FunctionIdentifier[1] { FunctionIdentifier.GetCurrentDateTime };

	public FunctionBinder(List<IDataBinder> operandBinders, ILogicalDeviceStateRepository repository, IRepository configurationRepository, IProtocolMultiplexer protocolMultiplexer)
	{
		this.operandBinders = operandBinders;
		this.repository = repository;
		propertyResolver = new StatePropertyResolver(operandBinders, protocolMultiplexer.PhysicalState, repository, configurationRepository);
		functionMap = new Dictionary<FunctionIdentifier, Func<IList<IComparable>, IComparable>>
		{
			{
				FunctionIdentifier.Add,
				PerformAddOperation
			},
			{
				FunctionIdentifier.Subtract,
				PerformSubtractOperation
			},
			{
				FunctionIdentifier.Multiply,
				PerformMultiplyOperation
			},
			{
				FunctionIdentifier.Divide,
				PerformDivideOperation
			},
			{
				FunctionIdentifier.Modulo,
				PerformModuloOperation
			},
			{
				FunctionIdentifier.Equal,
				PerformEqualOperation
			},
			{
				FunctionIdentifier.NotEqual,
				PerformNotEqualOperation
			},
			{
				FunctionIdentifier.Smaller,
				PerformSmallerOperation
			},
			{
				FunctionIdentifier.Greater,
				PerformGreaterOperation
			},
			{
				FunctionIdentifier.SmallerOrEqual,
				PerformSmallerOrEqualOperation
			},
			{
				FunctionIdentifier.GreaterOrEqual,
				PerformGreaterOrEqualOperation
			},
			{
				FunctionIdentifier.And,
				PerformAndOperation
			},
			{
				FunctionIdentifier.Or,
				PerformOrOperation
			},
			{
				FunctionIdentifier.Min,
				PerformMinOperation
			},
			{
				FunctionIdentifier.Max,
				PerformMaxOperation
			},
			{
				FunctionIdentifier.Pow,
				PerformPowOperation
			},
			{
				FunctionIdentifier.Exp,
				PerformExpOperation
			},
			{
				FunctionIdentifier.Log,
				PerformLogOperation
			},
			{
				FunctionIdentifier.Abs,
				PerformAbsOperation
			},
			{
				FunctionIdentifier.Round,
				PerformRoundOperation
			},
			{
				FunctionIdentifier.BitwiseAnd,
				PerformBitwiseAndOperation
			},
			{
				FunctionIdentifier.BitwiseOr,
				PerformBitwiseOrOperation
			},
			{
				FunctionIdentifier.BitwiseXOR,
				PerformBitwiseXOROperation
			},
			{
				FunctionIdentifier.BitwiseLeftShift,
				PerformBitwiseLeftShiftOperation
			},
			{
				FunctionIdentifier.BitwiseRightShift,
				PerformBitwiseRightShiftOperation
			},
			{
				FunctionIdentifier.BitwiseNot,
				PerformBitwiseNotOperation
			},
			{
				FunctionIdentifier.Average,
				PerformAverageOperation
			},
			{
				FunctionIdentifier.InBetween,
				PerformInBetweenOperation
			}
		};
		contextBoundFunctionMap = new Dictionary<FunctionIdentifier, Func<IList<Parameter>, EventContext, IComparable>>
		{
			{
				FunctionIdentifier.GetEntityStateProperty,
				propertyResolver.GetStateProperty
			},
			{
				FunctionIdentifier.GetMinutesSinceLastChange,
				propertyResolver.GetMinutesSinceLastChangeProperty
			},
			{
				FunctionIdentifier.GetEventProperty,
				PerformGetContextPropertyOperation
			},
			{
				FunctionIdentifier.HasEventProperty,
				PerformHasEventPropertyOperation
			}
		};
		timeFunctionMap = new Dictionary<FunctionIdentifier, Func<IList<IComparable>, IComparable>>
		{
			{
				FunctionIdentifier.GetMinute,
				PerformGetMinuteOperation
			},
			{
				FunctionIdentifier.GetHour,
				PerformGetHourOperation
			},
			{
				FunctionIdentifier.GetDayOfWeek,
				PerformGetDayOfWeekOperation
			},
			{
				FunctionIdentifier.GetDayOfMonth,
				PerformGetDayOfMonthOperation
			},
			{
				FunctionIdentifier.GetWeekdayOfMonth,
				PerformGetWeekdayOfMonthOperation
			},
			{
				FunctionIdentifier.GetMonth,
				PerformGetMonthOperation
			},
			{
				FunctionIdentifier.GetYear,
				PerformGetYearOperation
			},
			{
				FunctionIdentifier.GetDayOfCentury,
				PerformGetDayOfCenturyOperation
			},
			{
				FunctionIdentifier.GetWeekOfCentury,
				PerformGetWeekOfCenturyOperation
			},
			{
				FunctionIdentifier.GetMonthOfCentury,
				PerformGetMonthOfCenturyOperation
			},
			{
				FunctionIdentifier.GetCurrentDateTime,
				PerformGetCurrentDateTimeOperation
			},
			{
				FunctionIdentifier.GetMinuteOfDay,
				PerformGetMinuteOfDayOperation
			}
		};
	}

	public override DataBinderType GetBinderType(DataBinding binding)
	{
		if (binding is FunctionBinding functionBinding)
		{
			if (dateTimeFunctions.Contains(functionBinding.Function))
			{
				return DataBinderType.DateTime;
			}
			if (numericFunctions.Contains(functionBinding.Function))
			{
				return DataBinderType.Numeric;
			}
			if (booleanFunctions.Contains(functionBinding.Function))
			{
				return DataBinderType.Boolean;
			}
		}
		return DataBinderType.Runtime;
	}

	protected override IComparable GetValue(FunctionBinding binding, EventContext context)
	{
		if (contextBoundFunctions.Contains(binding.Function))
		{
			return PerformContextBoundOperation(binding.Function, binding.Parameters, context);
		}
		try
		{
			IList<IComparable> operands = binding.Parameters.Select(delegate(Parameter param)
			{
				IDataBinder dataBinder = operandBinders.Where((IDataBinder b) => b.CanEvaluate(param.Value)).FirstOrDefault();
				if (dataBinder == null)
				{
					Log.Information(Module.RuleEngine, "No binder found for DataBinding " + param);
					throw new NotSupportedException();
				}
				return dataBinder.GetValue(param.Value, context);
			}).ToList();
			if (timeFunctionMap.ContainsKey(binding.Function))
			{
				return PerformTimeOperations(binding.Function, operands);
			}
			return PerformOperation(binding.Function, operands);
		}
		catch (ArgumentNullException ex)
		{
			throw ex;
		}
		catch (NotSupportedException)
		{
			return null;
		}
	}

	protected override bool IsAffectedByTrigger(FunctionBinding binding, DeviceStateTriggerData stateTriggerData)
	{
		List<bool> list = binding.Parameters.Select((Parameter param) => operandBinders.Where((IDataBinder b) => b.CanEvaluate(param.Value)).First().IsAffectedByTrigger(param.Value, stateTriggerData)).ToList();
		if (list.Contains(item: true))
		{
			return true;
		}
		return false;
	}

	private IComparable PerformOperation(FunctionIdentifier id, IList<IComparable> operands)
	{
		try
		{
			if (functionMap.TryGetValue(id, out var value))
			{
				return value(operands);
			}
			Log.Error(Module.RuleEngine, $"Invalid operand {id}");
		}
		catch (ArgumentNullException arg)
		{
			throw new ArgumentNullException($"Could not execute \"{id}\" function {arg}");
		}
		catch (Exception arg2)
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{id}\" function {arg2}");
		}
		return null;
	}

	private IComparable PerformContextBoundOperation(FunctionIdentifier id, IList<Parameter> operands, EventContext context)
	{
		try
		{
			if (contextBoundFunctionMap.TryGetValue(id, out var value))
			{
				return value(operands, context);
			}
			Log.Error(Module.RuleEngine, $"Invalid operand {id}");
		}
		catch (Exception arg)
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{id}\" context bound function {arg}");
		}
		return null;
	}

	private IComparable PerformTimeOperations(FunctionIdentifier id, IList<IComparable> operands)
	{
		try
		{
			if (timeFunctionMap.TryGetValue(id, out var value))
			{
				return value(operands);
			}
			Log.Error(Module.RuleEngine, $"Invalid operand {id}");
		}
		catch (Exception arg)
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{id}\" time function {arg}");
		}
		return null;
	}

	private IComparable PerformAddOperation(IList<IComparable> operands)
	{
		if (operands == null || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Add}\" function");
			return null;
		}
		decimal num = operands.Sum((IComparable opnd) => Convert.ToDecimal(opnd));
		LogOperation(FunctionIdentifier.Add, operands, num.ToString());
		return num;
	}

	private IComparable PerformSubtractOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Subtract}\" function");
			return null;
		}
		decimal num = Convert.ToDecimal(operands[0]) - Convert.ToDecimal(operands[1]);
		LogOperation(FunctionIdentifier.Subtract, operands, num.ToString());
		return num;
	}

	private IComparable PerformMultiplyOperation(IList<IComparable> operands)
	{
		if (operands == null || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Multiply}\" function");
			return null;
		}
		decimal num = 1m;
		foreach (IComparable operand in operands)
		{
			num *= Convert.ToDecimal(operand);
		}
		LogOperation(FunctionIdentifier.Multiply, operands, num.ToString());
		return num;
	}

	private IComparable PerformDivideOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Divide}\" function");
			return null;
		}
		decimal num = Convert.ToDecimal(operands[0]) / Convert.ToDecimal(operands[1]);
		LogOperation(FunctionIdentifier.Divide, operands, num.ToString());
		return num;
	}

	private IComparable PerformModuloOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Modulo}\" function");
			return null;
		}
		decimal num = Convert.ToDecimal(operands[0]) % Convert.ToDecimal(operands[1]);
		LogOperation(FunctionIdentifier.Modulo, operands, num.ToString());
		return num;
	}

	private IComparable PerformEqualOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2)
		{
			return null;
		}
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute Equal operation. Received null operand");
		}
		bool flag = operands[0].CompareTo(operands[1]) == 0;
		LogOperation(FunctionIdentifier.Equal, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformNotEqualOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2)
		{
			return null;
		}
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute \"NotEqual\" operation. Received null operand");
		}
		bool flag = operands[0].CompareTo(operands[1]) != 0;
		LogOperation(FunctionIdentifier.NotEqual, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformSmallerOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2)
		{
			return null;
		}
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute \"Smaller\" operation. Received null operand");
		}
		bool flag = operands[0].CompareTo(operands[1]) < 0;
		LogOperation(FunctionIdentifier.Smaller, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformGreaterOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2)
		{
			return null;
		}
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute \"Greater\" operation. Received null operand");
		}
		bool flag = operands[0].CompareTo(operands[1]) > 0;
		LogOperation(FunctionIdentifier.Greater, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformSmallerOrEqualOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2)
		{
			return null;
		}
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute \"SmallerOrEqual\" operation. Received null operand");
		}
		bool flag = operands[0].CompareTo(operands[1]) <= 0;
		LogOperation(FunctionIdentifier.SmallerOrEqual, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformGreaterOrEqualOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2)
		{
			return null;
		}
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute \"GreaterOrEqual\" operation. Received null operand");
		}
		bool flag = operands[0].CompareTo(operands[1]) >= 0;
		LogOperation(FunctionIdentifier.GreaterOrEqual, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformAndOperation(IList<IComparable> operands)
	{
		bool flag = true;
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute \"And\" operation. Received null operand");
		}
		foreach (IComparable operand in operands)
		{
			flag = flag && (bool)(object)operand;
		}
		LogOperation(FunctionIdentifier.And, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformOrOperation(IList<IComparable> operands)
	{
		bool flag = false;
		if (operands.Any((IComparable op) => op == null))
		{
			throw new ArgumentNullException("Could not execute \"Or\" operation. Received null operand");
		}
		foreach (IComparable operand in operands)
		{
			flag = flag || (bool)(object)operand;
		}
		LogOperation(FunctionIdentifier.Or, operands, flag.ToString());
		return flag;
	}

	private IComparable PerformMinOperation(IList<IComparable> operands)
	{
		if (AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Min}\" function");
			return null;
		}
		IComparable comparable = operands.Min();
		LogOperation(FunctionIdentifier.Min, operands, comparable.ToString());
		return comparable;
	}

	private IComparable PerformMaxOperation(IList<IComparable> operands)
	{
		if (AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Max}\" function");
			return null;
		}
		IComparable comparable = operands.Max();
		LogOperation(FunctionIdentifier.Max, operands, comparable.ToString());
		return comparable;
	}

	private IComparable PerformPowOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Pow}\" function");
			return null;
		}
		double value = Math.Pow(Convert.ToDouble(operands[0]), Convert.ToDouble(operands[1]));
		LogOperation(FunctionIdentifier.Pow, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformExpOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 1 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Exp}\" function");
			return null;
		}
		double value = Math.Exp(Convert.ToDouble(operands[0]));
		LogOperation(FunctionIdentifier.Exp, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformLogOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 1 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Log}\" function");
			return null;
		}
		double value = Math.Log(Convert.ToDouble(operands[0]));
		LogOperation(FunctionIdentifier.Log, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformAbsOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 1 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Abs}\" function");
			return null;
		}
		decimal num = Math.Abs(Convert.ToDecimal(operands[0]));
		LogOperation(FunctionIdentifier.Abs, operands, num.ToString());
		return num;
	}

	private IComparable PerformRoundOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 1 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Round}\" function");
			return null;
		}
		decimal num = Math.Round(Convert.ToDecimal(operands[0]));
		LogOperation(FunctionIdentifier.Round, operands, num.ToString());
		return num;
	}

	private IComparable PerformBitwiseAndOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.BitwiseAnd}\" function");
			return null;
		}
		long value = ConvertHexToInt(operands[0]) & ConvertHexToInt(operands[1]);
		LogOperation(FunctionIdentifier.BitwiseAnd, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformBitwiseOrOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.BitwiseOr}\" function");
			return null;
		}
		long value = ConvertHexToInt(operands[0]) | ConvertHexToInt(operands[1]);
		LogOperation(FunctionIdentifier.BitwiseAnd, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformBitwiseXOROperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.BitwiseXOR}\" function");
			return null;
		}
		long value = ConvertHexToInt(operands[0]) ^ ConvertHexToInt(operands[1]);
		LogOperation(FunctionIdentifier.BitwiseXOR, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformBitwiseLeftShiftOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.BitwiseLeftShift}\" function");
			return null;
		}
		long value = ConvertHexToInt(operands[0]) << Convert.ToInt32(ConvertHexToInt(operands[1]));
		LogOperation(FunctionIdentifier.BitwiseLeftShift, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformBitwiseRightShiftOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() != 2 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.BitwiseRightShift}\" function");
			return null;
		}
		long value = ConvertHexToInt(operands[0]) >> Convert.ToInt32(ConvertHexToInt(operands[1]));
		LogOperation(FunctionIdentifier.BitwiseRightShift, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformBitwiseNotOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() != 1 || AreBooleanOperands(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.BitwiseNot}\" function");
			return null;
		}
		long value = ~ConvertHexToInt(operands[0]);
		LogOperation(FunctionIdentifier.BitwiseNot, operands, value.ToString());
		return Convert.ToDecimal(value);
	}

	private IComparable PerformGetContextPropertyOperation(IList<Parameter> operands, EventContext context)
	{
		return TryToGetOperandPropertyFromContext(operands, context)?.GetValueAsComparable();
	}

	private IComparable PerformHasEventPropertyOperation(IList<Parameter> operands, EventContext context)
	{
		Property property = TryToGetOperandPropertyFromContext(operands, context);
		if (property == null || ContextDoesNotHaveOldState(context))
		{
			return false;
		}
		Property propertyByName = GetPropertyByName(context.OldStateProperties, property.Name);
		if (propertyByName == null)
		{
			return false;
		}
		IComparable valueAsComparable = property.GetValueAsComparable();
		IComparable valueAsComparable2 = propertyByName.GetValueAsComparable();
		return !object.Equals(valueAsComparable, valueAsComparable2);
	}

	private static bool ContextDoesNotHaveOldState(EventContext context)
	{
		if (context != null)
		{
			return context.OldStateProperties == null;
		}
		return true;
	}

	private Property TryToGetOperandPropertyFromContext(IList<Parameter> operands, EventContext context)
	{
		if (context == null || context.CurrentStateProperties == null)
		{
			return null;
		}
		Parameter eventPropertyName = operands.FirstOrDefault((Parameter p) => p.Name == "EventPropertyName");
		if (eventPropertyName == null || eventPropertyName.Value == null)
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetEventProperty}\" function. Missing event property name.");
			return null;
		}
		string propertyName = operandBinders.Where((IDataBinder b) => b.CanEvaluate(eventPropertyName.Value)).First().GetValue(eventPropertyName.Value, context)
			.ToString();
		return GetPropertyByName(context.CurrentStateProperties, propertyName);
	}

	private static Property GetPropertyByName(List<Property> properties, string propertyName)
	{
		return properties.SingleOrDefault((Property p) => p.Name.ToLower() == propertyName.ToLower());
	}

	private IComparable PerformGetMinuteOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetMinute}\" function.");
			return null;
		}
		return Convert.ToDecimal(Convert.ToDateTime(operands[0]).Minute);
	}

	private IComparable PerformGetHourOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetHour}\" function.");
			return null;
		}
		return Convert.ToDecimal(Convert.ToDateTime(operands[0]).Hour);
	}

	private IComparable PerformGetDayOfWeekOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetDayOfWeek}\" function.");
			return null;
		}
		DateTime dateTime = Convert.ToDateTime(operands[0]);
		if (dateTime.DayOfWeek == DayOfWeek.Sunday)
		{
			return 64;
		}
		return Convert.ToDecimal(1 << (int)(dateTime.DayOfWeek - 1));
	}

	private IComparable PerformGetDayOfMonthOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetDayOfMonth}\" function.");
			return null;
		}
		DateTime dateTime = Convert.ToDateTime(operands[0]);
		if (dateTime.AddDays(1.0).Month != dateTime.Month)
		{
			return Convert.ToDecimal(LastDayOfMonth);
		}
		return Convert.ToDecimal(1 << dateTime.Day - 1);
	}

	private IComparable PerformGetWeekdayOfMonthOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetWeekdayOfMonth}\" function.");
			return null;
		}
		int num = 0;
		DateTime dateTime = Convert.ToDateTime(operands[0]);
		DateTime dateTime2 = dateTime;
		do
		{
			dateTime2 = dateTime2.AddDays(-7.0);
			num++;
		}
		while (dateTime2.Month == dateTime.Month);
		if (dateTime.AddDays(7.0).Month != dateTime.Month)
		{
			return Convert.ToDecimal(LastWeekdayofMonth);
		}
		return Convert.ToDecimal(1 << num - 1);
	}

	private IComparable PerformGetMonthOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetMonth}\" function.");
			return null;
		}
		int month = Convert.ToDateTime(operands[0]).Month;
		return Convert.ToDecimal(1 << month - 1);
	}

	private IComparable PerformGetYearOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetYear}\" function.");
			return null;
		}
		return Convert.ToDecimal(Convert.ToDateTime(operands[0]).Year);
	}

	private IComparable PerformGetDayOfCenturyOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetDayOfCentury}\" function.");
			return null;
		}
		DateTime dateTime = Convert.ToDateTime(operands[0]);
		int num = dateTime.Year / 100 + 1;
		DateTime dateTime2 = new DateTime((num - 1) * 100 + 1, 1, 1);
		return Convert.ToDecimal((dateTime - dateTime2).Days + 1);
	}

	private IComparable PerformGetWeekOfCenturyOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetWeekOfCentury}\" function.");
			return null;
		}
		DateTime dateTime = Convert.ToDateTime(operands[0]);
		int num = dateTime.Year / 100 + 1;
		DateTime dateTime2 = new DateTime((num - 1) * 100 + 1, 1, 1);
		return Convert.ToDecimal((dateTime - dateTime2).Days / 7 + 1);
	}

	private IComparable PerformGetMonthOfCenturyOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetMonthOfCentury}\" function.");
			return null;
		}
		DateTime dateTime = Convert.ToDateTime(operands[0]);
		int num = dateTime.Year / 100 + 1;
		DateTime dateTime2 = new DateTime((num - 1) * 100 + 1, 1, 1);
		_ = dateTime - dateTime2;
		int value = (dateTime.Year - dateTime2.Year) * 12 + dateTime.Month - dateTime2.Month + 1;
		return Convert.ToDecimal(value);
	}

	private IComparable PerformGetCurrentDateTimeOperation(IList<IComparable> operands)
	{
		return DateTime.Now;
	}

	private IComparable PerformGetMinuteOfDayOperation(IList<IComparable> operands)
	{
		if (!IsSingleOperand(operands))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.GetMinuteOfDay}\" function.");
			return null;
		}
		DateTime dateTime = Convert.ToDateTime(operands[0]);
		return Convert.ToDecimal(dateTime.Hour * 60 + dateTime.Minute);
	}

	private IComparable PerformAverageOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() == 0 || AreBooleanOperands(operands) || operands.All((IComparable opnd) => opnd == null))
		{
			Log.Debug(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.Average}\" function. All operands are null.");
			return null;
		}
		decimal num = operands.Where((IComparable opnd) => opnd != null).Average((IComparable opnd) => Convert.ToDecimal(opnd));
		LogOperation(FunctionIdentifier.Average, operands, num.ToString());
		return num;
	}

	private IComparable PerformInBetweenOperation(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() != 3 || operands.Any((IComparable x) => x == null))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{FunctionIdentifier.InBetween}\" function");
			return null;
		}
		int num = operands[0].CompareTo(operands[1]);
		int num2 = operands[0].CompareTo(operands[2]);
		bool flag = (num >= 0 || num == 0) && (num2 <= 0 || num2 == 0);
		LogOperation(FunctionIdentifier.InBetween, operands, flag.ToString());
		return flag;
	}

	private void LogOperation(FunctionIdentifier operation, IList<IComparable> operands, string result)
	{
		StringBuilder stringBuilder = new StringBuilder("(");
		stringBuilder.Append(operation);
		stringBuilder.Append(" ");
		foreach (IComparable operand in operands)
		{
			if (operand == null)
			{
				stringBuilder.Append("null, ");
			}
			else
			{
				stringBuilder.Append(operand.ToString()).Append(", ");
			}
		}
		stringBuilder.Append(")=>");
		stringBuilder.Append(result);
		Log.Debug(Module.RuleEngine, stringBuilder.ToString());
	}

	private bool AreBooleanOperands(IList<IComparable> operands)
	{
		foreach (IComparable operand in operands)
		{
			if (operand is bool)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsSingleOperand(IList<IComparable> operands)
	{
		if (operands == null || operands.Count() != 1)
		{
			return false;
		}
		return true;
	}

	private long ConvertHexToInt(IComparable number)
	{
		if (number is string)
		{
			string text = number.ToString();
			if (text.IndexOf("x", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				text = number.ToString().Substring(2, number.ToString().Length - 2);
				return long.Parse(text, NumberStyles.HexNumber);
			}
		}
		return Convert.ToInt64(number);
	}
}
