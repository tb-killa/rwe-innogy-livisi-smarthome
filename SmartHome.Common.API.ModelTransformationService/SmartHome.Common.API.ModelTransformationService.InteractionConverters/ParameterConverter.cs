using System;
using ModelTransformations.Helpers;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling.Exceptions;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

internal class ParameterConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ParameterConverter));

	private readonly LinkConverter linkConverter = new LinkConverter();

	public SmartHome.Common.API.Entities.Entities.Parameter FromSmartHomeParameter(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter shParameter)
	{
		SmartHome.Common.API.Entities.Entities.Parameter parameter = FromSmartHomeDataBinding(shParameter.Value);
		parameter.Name = shParameter.Name;
		return parameter;
	}

	public SmartHome.Common.API.Entities.Entities.Parameter FromSmartHomeDataBinding(DataBinding shDataBinding)
	{
		SmartHome.Common.API.Entities.Entities.Parameter parameter = new SmartHome.Common.API.Entities.Entities.Parameter();
		if (shDataBinding is ConstantBooleanBinding)
		{
			ConstantBooleanBinding constantBooleanBinding = shDataBinding as ConstantBooleanBinding;
			parameter.Type = "Constant";
			parameter.Constant = new Constant
			{
				Value = constantBooleanBinding.Value
			};
		}
		else if (shDataBinding is ConstantDateTimeBinding)
		{
			ConstantDateTimeBinding constantDateTimeBinding = shDataBinding as ConstantDateTimeBinding;
			parameter.Type = "Constant";
			parameter.Constant = new Constant
			{
				Value = constantDateTimeBinding.Value
			};
		}
		else if (shDataBinding is ConstantNumericBinding)
		{
			ConstantNumericBinding constantNumericBinding = shDataBinding as ConstantNumericBinding;
			parameter.Type = "Constant";
			parameter.Constant = new Constant
			{
				Value = constantNumericBinding.Value
			};
		}
		else if (shDataBinding is ConstantStringBinding)
		{
			ConstantStringBinding constantStringBinding = shDataBinding as ConstantStringBinding;
			parameter.Type = "Constant";
			parameter.Constant = new Constant
			{
				Value = constantStringBinding.Value
			};
		}
		else if (shDataBinding is FunctionBinding)
		{
			FunctionBinding functionBinding = shDataBinding as FunctionBinding;
			parameter.Type = "function/";
			parameter.Function = new Function
			{
				Type = functionBinding.Function.ToString(),
				Parameters = ((functionBinding.Parameters != null) ? functionBinding.Parameters.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter p) => FromSmartHomeParameter(p)) : null)
			};
		}
		else if (shDataBinding is LinkBinding)
		{
			LinkBinding shLink = shDataBinding as LinkBinding;
			parameter.Type = "Constant";
			parameter.Constant = new Constant
			{
				Value = linkConverter.FromSmartHomeLinkBinding(shLink)
			};
		}
		return parameter;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter ToSmartHomeParameter(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter parameter = null;
		if (string.Equals(apiParameter.Type, "Constant", StringComparison.InvariantCultureIgnoreCase))
		{
			return ToConstantParameter(apiParameter);
		}
		return ToFunctionParameter(apiParameter);
	}

	public DataBinding ToSmartHomeDataBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		DataBinding dataBinding = null;
		if (string.Equals(apiParameter.Type, "Constant", StringComparison.InvariantCultureIgnoreCase))
		{
			return ToConstantDataBinding(apiParameter);
		}
		return ToFunctionDataBinding(apiParameter);
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter ToFunctionParameter(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter parameter = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter();
		parameter.Name = apiParameter.Name;
		parameter.Value = ToFunctionDataBinding(apiParameter);
		return parameter;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter ToConstantParameter(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		if (apiParameter.Constant == null || apiParameter.Constant.Value == null)
		{
			logger.LogAndThrow<ArgumentException>("Constant value mandatory");
		}
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter result = null;
		Type type = apiParameter.Constant.Value.GetType();
		string arg = apiParameter.Constant.Value.ToString();
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.String:
			result = ToConstantStringParameter(apiParameter);
			break;
		case TypeCode.Boolean:
			result = ToConstantBooleanParameter(apiParameter);
			break;
		case TypeCode.DateTime:
			result = ToConstantDateTimeParameter(apiParameter);
			break;
		case TypeCode.SByte:
		case TypeCode.Byte:
		case TypeCode.Int16:
		case TypeCode.UInt16:
		case TypeCode.Int32:
		case TypeCode.UInt32:
		case TypeCode.Int64:
		case TypeCode.UInt64:
		case TypeCode.Single:
		case TypeCode.Double:
		case TypeCode.Decimal:
			result = ToConstantNumericParameter(apiParameter);
			break;
		default:
			logger.LogAndThrow<NotSupportedDataTypeException>($"Unsupported type {type.Name} for constant value {arg}");
			break;
		}
		return result;
	}

	private DataBinding ToFunctionDataBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		FunctionIdentifier enumValue = FunctionIdentifier.Equal;
		if (apiParameter.Function == null || !apiParameter.Function.Type.Substring(apiParameter.Function.Type.IndexOf("/") + 1).TryParse<FunctionIdentifier>(ignoreCase: true, out enumValue))
		{
			logger.LogAndThrow<ArgumentException>("No function parameters provided or function type not supported");
		}
		FunctionBinding functionBinding = new FunctionBinding();
		functionBinding.Function = enumValue;
		functionBinding.Parameters = ((apiParameter.Function.Parameters != null) ? apiParameter.Function.Parameters.ConvertAll((SmartHome.Common.API.Entities.Entities.Parameter param) => ToSmartHomeParameter(param)) : null);
		return functionBinding;
	}

	private DataBinding ToConstantDataBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		if (apiParameter.Constant == null || apiParameter.Constant.Value == null)
		{
			logger.LogAndThrow<ArgumentException>("Constant value mandatory");
		}
		DataBinding result = null;
		Type type = apiParameter.Constant.Value.GetType();
		string arg = apiParameter.Constant.Value.ToString();
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.String:
			result = ToConstantStringBinding(apiParameter);
			break;
		case TypeCode.Boolean:
			result = ToConstantBooleanBinding(apiParameter);
			break;
		case TypeCode.DateTime:
			result = ToConstantDateTimeBinding(apiParameter);
			break;
		case TypeCode.SByte:
		case TypeCode.Byte:
		case TypeCode.Int16:
		case TypeCode.UInt16:
		case TypeCode.Int32:
		case TypeCode.UInt32:
		case TypeCode.Int64:
		case TypeCode.UInt64:
		case TypeCode.Single:
		case TypeCode.Double:
		case TypeCode.Decimal:
			result = ToConstantNumericBinding(apiParameter);
			break;
		default:
			logger.LogAndThrow<NotSupportedDataTypeException>($"Unsupported type {type.Name} for constant value {arg}");
			break;
		}
		return result;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter ToConstantBooleanParameter(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter parameter = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter();
		parameter.Name = apiParameter.Name;
		parameter.Value = ToConstantBooleanBinding(apiParameter);
		return parameter;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter ToConstantDateTimeParameter(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter parameter = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter();
		parameter.Name = apiParameter.Name;
		parameter.Value = ToConstantDateTimeBinding(apiParameter);
		return parameter;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter ToConstantNumericParameter(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter parameter = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter();
		parameter.Name = apiParameter.Name;
		parameter.Value = ToConstantNumericBinding(apiParameter);
		return parameter;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter ToConstantStringParameter(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter parameter = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter();
		parameter.Name = apiParameter.Name;
		parameter.Value = ToConstantStringBinding(apiParameter);
		return parameter;
	}

	private DataBinding ToConstantBooleanBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		bool value = TypeConverter.To<bool>((IConvertible)apiParameter.Constant.Value);
		ConstantBooleanBinding constantBooleanBinding = new ConstantBooleanBinding();
		constantBooleanBinding.Value = value;
		return constantBooleanBinding;
	}

	private DataBinding ToConstantDateTimeBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		DateTime value = TypeConverter.To<DateTime>((IConvertible)apiParameter.Constant.Value);
		ConstantDateTimeBinding constantDateTimeBinding = new ConstantDateTimeBinding();
		constantDateTimeBinding.Value = value;
		return constantDateTimeBinding;
	}

	private DataBinding ToConstantNumericBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		decimal value = TypeConverter.To<decimal>((IConvertible)apiParameter.Constant.Value);
		ConstantNumericBinding constantNumericBinding = new ConstantNumericBinding();
		constantNumericBinding.Value = value;
		return constantNumericBinding;
	}

	private DataBinding ToConstantStringBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		if (RegexExpressions.LinkSpecificTypeRegex.IsMatch(apiParameter.Constant.Value.ToString()))
		{
			return ToLinkDataBinding(apiParameter);
		}
		ConstantStringBinding constantStringBinding = new ConstantStringBinding();
		constantStringBinding.Value = apiParameter.Constant.Value.ToString();
		return constantStringBinding;
	}

	private DataBinding ToLinkDataBinding(SmartHome.Common.API.Entities.Entities.Parameter apiParameter)
	{
		if (string.IsNullOrEmpty(apiParameter.Constant.Value.ToString()))
		{
			logger.LogAndThrow<ArgumentException>("No string parameter provided");
		}
		return linkConverter.ToSmartHomeLinkBinding(apiParameter.Constant.Value.ToString());
	}
}
