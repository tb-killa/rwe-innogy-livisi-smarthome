using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.RepositoryOperations;

public static class InteractionChecker
{
	public static bool FixFunctionValuesIfNecessary(Interaction interaction)
	{
		if (interaction == null)
		{
			throw new ArgumentException("Interaction is null");
		}
		try
		{
			IEnumerable<FunctionBinding> allFunctionsFromInteraction = GetAllFunctionsFromInteraction(interaction);
			return CheckFunctionsValues(allFunctionsFromInteraction);
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Error occurred when trying to update function parameter value casing: {ex.Message}");
		}
		return false;
	}

	private static IEnumerable<FunctionBinding> GetAllFunctionsFromInteraction(Interaction interaction)
	{
		IEnumerable<FunctionBinding> first = (from t in interaction.Rules.Where((Rule r) => r.Triggers != null).SelectMany((Rule r) => r.Triggers)
			where t != null && t.TriggerConditions != null
			select t).SelectMany((Trigger t) => t.TriggerConditions).SelectMany((Condition tc) => new List<DataBinding> { tc.LeftHandOperand, tc.RightHandOperand }).OfType<FunctionBinding>();
		IEnumerable<FunctionBinding> second = (from t in interaction.Rules.Where((Rule r) => r.Conditions != null).SelectMany((Rule r) => r.Conditions)
			where t != null
			select t).SelectMany((Condition tc) => new List<DataBinding> { tc.LeftHandOperand, tc.RightHandOperand }).OfType<FunctionBinding>();
		return first.Union(second);
	}

	private static bool CheckFunctionsValues(IEnumerable<FunctionBinding> functions)
	{
		bool result = false;
		Queue<FunctionBinding> queue = new Queue<FunctionBinding>(functions);
		while (queue.Count > 0)
		{
			FunctionBinding functionBinding = queue.Dequeue();
			if (FixParamValues(functionBinding))
			{
				result = true;
			}
			IEnumerable<FunctionBinding> enumerable = functionBinding.Parameters.Select((Parameter m) => m.Value).OfType<FunctionBinding>();
			foreach (FunctionBinding item in enumerable)
			{
				queue.Enqueue(item);
			}
		}
		return result;
	}

	private static bool FixParamValues(FunctionBinding function)
	{
		return function.Function switch
		{
			FunctionIdentifier.GetEntityStateProperty => FixValue(function, "TargetPropertyName"), 
			FunctionIdentifier.GetEventProperty => FixValue(function, "EventPropertyName"), 
			_ => false, 
		};
	}

	private static bool FixValue(FunctionBinding function, string parameterToFix)
	{
		bool result = false;
		Parameter parameter = function.Parameters.FirstOrDefault((Parameter prm) => prm.Name == parameterToFix);
		if (parameter != null)
		{
			ConstantStringBinding constantStringBinding = parameter.Value as ConstantStringBinding;
			if (ModifyToCamelCaseString(constantStringBinding.Value, out var destString))
			{
				parameter.Value = new ConstantStringBinding
				{
					CloneTag = constantStringBinding.CloneTag,
					Tags = constantStringBinding.Tags,
					Value = destString,
					Version = constantStringBinding.Version
				};
				result = true;
			}
		}
		return result;
	}

	private static bool ModifyToCamelCaseString(string sourceString, out string destString)
	{
		if ((sourceString.Length >= 2 && char.IsUpper(sourceString[0]) && char.IsUpper(sourceString[1])) || char.IsLower(sourceString[0]))
		{
			destString = sourceString;
			return false;
		}
		char[] array = sourceString.ToCharArray();
		array[0] = char.ToLowerInvariant(array[0]);
		destString = new string(array);
		return true;
	}
}
