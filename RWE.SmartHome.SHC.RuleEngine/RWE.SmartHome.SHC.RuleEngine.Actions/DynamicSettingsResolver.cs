using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.RuleEngine.DataBinders;
using RWE.SmartHome.SHC.RuleEngineInterfaces;

namespace RWE.SmartHome.SHC.RuleEngine.Actions;

internal class DynamicSettingsResolver : IDynamicSettingsResolver
{
	private readonly IEnumerable<IDataBinder> dataBinders;

	public DynamicSettingsResolver(IEnumerable<IDataBinder> dataBinders)
	{
		this.dataBinders = dataBinders;
	}

	public IEnumerable<Property> GetTargetState(IEnumerable<Parameter> action)
	{
		return action.Select((Parameter param) => EvaluateToProperty(param));
	}

	private Property EvaluateToProperty(Parameter param)
	{
		FunctionBinding fb = param.Value as FunctionBinding;
		if (fb != null)
		{
			IComparable value = dataBinders.Where((IDataBinder db) => db.CanEvaluate(fb)).First().GetValue(fb, null);
			if (value is bool)
			{
				BooleanProperty booleanProperty = new BooleanProperty();
				booleanProperty.Name = param.Name;
				booleanProperty.Value = (bool)(object)value;
				return booleanProperty;
			}
			if (value is decimal || value is double)
			{
				NumericProperty numericProperty = new NumericProperty();
				numericProperty.Name = param.Name;
				numericProperty.Value = Convert.ToDecimal(value);
				return numericProperty;
			}
			if (value is DateTime)
			{
				DateTimeProperty dateTimeProperty = new DateTimeProperty();
				dateTimeProperty.Name = param.Name;
				dateTimeProperty.Value = (DateTime)(object)value;
				return dateTimeProperty;
			}
			StringProperty stringProperty = new StringProperty();
			stringProperty.Name = param.Name;
			stringProperty.Value = (string)value;
			return stringProperty;
		}
		if (param.Value is ConstantBooleanBinding)
		{
			BooleanProperty booleanProperty2 = new BooleanProperty();
			booleanProperty2.Name = param.Name;
			booleanProperty2.Value = (param.Value as ConstantBooleanBinding).Value;
			return booleanProperty2;
		}
		if (param.Value is ConstantDateTimeBinding)
		{
			DateTimeProperty dateTimeProperty2 = new DateTimeProperty();
			dateTimeProperty2.Name = param.Name;
			dateTimeProperty2.Value = (param.Value as ConstantDateTimeBinding).Value;
			return dateTimeProperty2;
		}
		if (param.Value is ConstantNumericBinding)
		{
			NumericProperty numericProperty2 = new NumericProperty();
			numericProperty2.Name = param.Name;
			numericProperty2.Value = (param.Value as ConstantNumericBinding).Value;
			return numericProperty2;
		}
		if (param.Value is ConstantStringBinding)
		{
			StringProperty stringProperty2 = new StringProperty();
			stringProperty2.Name = param.Name;
			stringProperty2.Value = (param.Value as ConstantStringBinding).Value;
			return stringProperty2;
		}
		throw new ArgumentException("Unknown parameter type: " + param.GetType().Name);
	}

	public Parameter EvaluateToConstantParameter(Parameter param)
	{
		FunctionBinding fb = param.Value as FunctionBinding;
		if (fb != null)
		{
			IComparable value = dataBinders.Where((IDataBinder db) => db.CanEvaluate(fb)).First().GetValue(fb, null);
			if (value is bool)
			{
				Parameter parameter = new Parameter();
				parameter.Name = param.Name;
				parameter.Value = new ConstantBooleanBinding
				{
					Value = (bool)(object)value
				};
				return parameter;
			}
			if (value is decimal || value is double)
			{
				Parameter parameter2 = new Parameter();
				parameter2.Name = param.Name;
				parameter2.Value = new ConstantNumericBinding
				{
					Value = Convert.ToDecimal(value)
				};
				return parameter2;
			}
			if (value is DateTime)
			{
				Parameter parameter3 = new Parameter();
				parameter3.Name = param.Name;
				parameter3.Value = new ConstantDateTimeBinding
				{
					Value = (DateTime)(object)value
				};
				return parameter3;
			}
			Parameter parameter4 = new Parameter();
			parameter4.Name = param.Name;
			parameter4.Value = new ConstantStringBinding
			{
				Value = (string)value
			};
			return parameter4;
		}
		return param;
	}

	public IEnumerable<Property> GetTargetTypes(IEnumerable<Parameter> action)
	{
		return action.Select((Parameter param) => EvaluateToType(param));
	}

	private Property EvaluateToType(Parameter param)
	{
		DataBinderType dataBinderType = DataBinderType.Runtime;
		FunctionBinding fb = param.Value as FunctionBinding;
		if (fb != null)
		{
			dataBinderType = dataBinders.Where((IDataBinder db) => db.CanEvaluate(fb)).First().GetBinderType(fb);
		}
		else if (param.Value is ConstantBooleanBinding)
		{
			dataBinderType = DataBinderType.Boolean;
		}
		else if (param.Value is ConstantDateTimeBinding)
		{
			dataBinderType = DataBinderType.DateTime;
		}
		else if (param.Value is ConstantNumericBinding)
		{
			dataBinderType = DataBinderType.Numeric;
		}
		else
		{
			if (!(param.Value is ConstantStringBinding))
			{
				throw new ArgumentException("Unknown parameter type: " + param.GetType().Name);
			}
			dataBinderType = DataBinderType.String;
		}
		return new StringProperty(param.Name, dataBinderType.ToString());
	}
}
