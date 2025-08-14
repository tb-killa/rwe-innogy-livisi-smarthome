using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Enumerations;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

internal class ConditionConverter : IConditionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ConditionConverter));

	private readonly ParameterConverter parameterConverter = new ParameterConverter();

	public SmartHome.Common.API.Entities.Entities.Condition FromSmartHomeCondition(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition shCondition)
	{
		if (!shCondition.Operator.ToString().TryParse<SmartHome.Common.API.Entities.Enumerations.ConditionOperator>(ignoreCase: true, out var enumValue))
		{
			logger.LogAndThrow<ArgumentException>($"Operator {shCondition.Operator} not supported by API.");
		}
		List<SmartHome.Common.API.Entities.Entities.Property> tags = null;
		if (shCondition.Tags != null && shCondition.Tags.Any())
		{
			tags = shCondition.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		SmartHome.Common.API.Entities.Entities.Condition condition = new SmartHome.Common.API.Entities.Entities.Condition();
		condition.Leqp = parameterConverter.FromSmartHomeDataBinding(shCondition.LeftHandOperand);
		condition.Operator = new SmartHome.Common.API.Entities.Entities.Parameter
		{
			Type = "Constant",
			Name = "Operator",
			Constant = new Constant
			{
				Value = enumValue.ToString()
			}
		};
		condition.Reqp = parameterConverter.FromSmartHomeDataBinding(shCondition.RightHandOperand);
		condition.Tags = tags;
		return condition;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition ToSmartHomeCondition(SmartHome.Common.API.Entities.Entities.Condition apiCondition)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums.ConditionOperator enumValue = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums.ConditionOperator.Equal;
		if (apiCondition.Operator.Constant == null || string.IsNullOrEmpty(apiCondition.Operator.Constant.Value.ToString()))
		{
			logger.LogAndThrow<ArgumentException>($"Operator {apiCondition.Operator} not supported by SHC.");
		}
		else
		{
			string text = apiCondition.Operator.Constant.Value.ToString();
			if (!text.Substring(text.IndexOf("/") + 1).TryParse<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums.ConditionOperator>(ignoreCase: true, out enumValue))
			{
				logger.LogAndThrow<ArgumentException>($"Operator {text} not supported by SHC.");
			}
		}
		List<Tag> tags = null;
		if (apiCondition.Tags != null && apiCondition.Tags.Any())
		{
			tags = apiCondition.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property t) => new Tag
			{
				Name = t.Name,
				Value = t.Value.ToString()
			});
		}
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition condition = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition();
		condition.LeftHandOperand = parameterConverter.ToSmartHomeDataBinding(apiCondition.Leqp);
		condition.Operator = enumValue;
		condition.RightHandOperand = parameterConverter.ToSmartHomeDataBinding(apiCondition.Reqp);
		condition.Tags = tags;
		return condition;
	}
}
