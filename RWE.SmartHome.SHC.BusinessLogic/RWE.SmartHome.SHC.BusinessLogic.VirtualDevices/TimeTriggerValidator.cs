using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class TimeTriggerValidator : IConfigurationValidator
{
	internal static readonly uint DayOfWeekOccurrenceInMonthMaxValue = 31u;

	internal static readonly uint MonthMaxValue = 4095u;

	internal static readonly uint DayOfMonthMaxValue = uint.MaxValue;

	internal static readonly uint DayOfWeekMaxValue = 127u;

	public TimeTriggerValidator(IConfigurationProcessor configProcessor)
	{
		configProcessor?.RegisterConfigurationValidator(this);
	}

	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> errors = new List<ErrorEntry>();
		configuration.GetInteractions().ForEach(delegate(Interaction interaction)
		{
			interaction.Rules.ForEach(delegate(Rule rule)
			{
				if (rule.CustomTriggers != null)
				{
					rule.CustomTriggers.ForEach(delegate(CustomTrigger customTrigger)
					{
						if (customTrigger.IsCustomTimeTrigger() && !IsValidTimeTrigger(customTrigger))
						{
							AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.InvalidTimeTrigger, errors);
						}
					});
				}
			});
		});
		return errors;
	}

	private bool IsValidTimeTrigger(CustomTrigger ct)
	{
		bool result = true;
		switch (ct.Type)
		{
		case "BySecondTrigger":
			result = IsBySecondTriggerValid(ct);
			break;
		case "HourlyTrigger":
			result = IsHourlyTriggerValid(ct);
			break;
		case "DailyTrigger":
			result = IsDailyTriggerValid(ct);
			break;
		case "WeeklyTrigger":
			result = IsWeeklyTriggerValid(ct);
			break;
		case "DayOfWeekMonthlyTrigger":
			result = IsDayOfWeekMonthlyTriggerValid(ct);
			break;
		case "DayOfMonthTrigger":
			result = IsDayOfMonthTriggerValid(ct);
			break;
		default:
			Log.Debug(Module.BusinessLogic, $"Unknown custom time trigger: {ct.Type}");
			break;
		}
		return result;
	}

	private bool IsBySecondTriggerValid(CustomTrigger customTrigger)
	{
		if (!HasProperty(customTrigger, "RecurrenceInterval") || IsNumericPropertyValid(customTrigger, "RecurrenceInterval", 30, 2147483647u, 10))
		{
			return true;
		}
		return false;
	}

	private bool IsHourlyTriggerValid(CustomTrigger customTrigger)
	{
		if (HasProperty(customTrigger, "StartTime") && AreHourPropertiesValid(customTrigger, 1) && (!HasProperty(customTrigger, "RecurrenceInterval") || (HasProperty(customTrigger, "RecurrenceInterval") && IsReccurrenceIntervalValid(customTrigger))))
		{
			return true;
		}
		return false;
	}

	private bool IsDailyTriggerValid(CustomTrigger customTrigger)
	{
		if (HasProperty(customTrigger, "StartTime") && AreHourPropertiesValid(customTrigger, null) && (!HasProperty(customTrigger, "RecurrenceInterval") || (HasProperty(customTrigger, "RecurrenceInterval") && IsReccurrenceIntervalValid(customTrigger))))
		{
			return true;
		}
		return false;
	}

	private bool IsWeeklyTriggerValid(CustomTrigger customTrigger)
	{
		if (IsDailyTriggerValid(customTrigger) && (!HasProperty(customTrigger, "DayOfWeek") || (HasProperty(customTrigger, "DayOfWeek") && IsDayOfWeekValid(customTrigger))))
		{
			return true;
		}
		return false;
	}

	private bool IsDayOfWeekMonthlyTriggerValid(CustomTrigger customTrigger)
	{
		if (HasProperty(customTrigger, "StartTime") && AreHourPropertiesValid(customTrigger, null) && (!HasProperty(customTrigger, "DayOfWeek") || (HasProperty(customTrigger, "DayOfWeek") && IsDayOfWeekValid(customTrigger))) && (!HasProperty(customTrigger, "DayOfWeekOccurrenceInMonth") || (HasProperty(customTrigger, "DayOfWeekOccurrenceInMonth") && IsDayOfWeekOccurrenceInMonthValid(customTrigger))) && (!HasProperty(customTrigger, "Month") || (HasProperty(customTrigger, "Month") && IsMonthValid(customTrigger))))
		{
			return true;
		}
		return false;
	}

	private bool IsDayOfMonthTriggerValid(CustomTrigger customTrigger)
	{
		if (HasProperty(customTrigger, "StartTime") && AreHourPropertiesValid(customTrigger, null) && (!HasProperty(customTrigger, "DayOfMonth") || (HasProperty(customTrigger, "DayOfMonth") && IsDayOfMonthValid(customTrigger))) && (!HasProperty(customTrigger, "Month") || (HasProperty(customTrigger, "Month") && IsMonthValid(customTrigger))))
		{
			return true;
		}
		return false;
	}

	private bool AreHourPropertiesValid(CustomTrigger customTrigger, int? maxPropCount)
	{
		List<StringProperty> list = (from prop in customTrigger.Properties.OfType<StringProperty>()
			where prop.Name == "StartTime" && prop.Value.Length > 0
			select prop).ToList();
		if (maxPropCount.HasValue && maxPropCount != list.Count)
		{
			return false;
		}
		foreach (StringProperty item in list)
		{
			Regex regex = new Regex("\\d{1,2}\\:\\d{1,2}\\:\\d{1,2}");
			Match match = regex.Match(item.Value);
			if (!match.Success)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsReccurrenceIntervalValid(CustomTrigger customTrigger)
	{
		return IsNumericPropertyValid(customTrigger, "RecurrenceInterval", 0, DayOfWeekMaxValue, 1);
	}

	private bool IsDayOfWeekValid(CustomTrigger customTrigger)
	{
		return IsNumericPropertyValid(customTrigger, "DayOfWeek", 1, DayOfWeekMaxValue, 1);
	}

	private bool IsDayOfWeekOccurrenceInMonthValid(CustomTrigger customTrigger)
	{
		return IsNumericPropertyValid(customTrigger, "DayOfWeekOccurrenceInMonth", 1, DayOfWeekOccurrenceInMonthMaxValue, 1);
	}

	private bool IsMonthValid(CustomTrigger customTrigger)
	{
		return IsNumericPropertyValid(customTrigger, "Month", 1, MonthMaxValue, 1);
	}

	private bool IsDayOfMonthValid(CustomTrigger customTrigger)
	{
		return IsNumericPropertyValid(customTrigger, "DayOfMonth", 1, DayOfMonthMaxValue, 1);
	}

	private bool HasProperty(CustomTrigger customTrigger, string propertyName)
	{
		if (customTrigger.Properties.Any((Property prop) => prop.Name == propertyName))
		{
			return true;
		}
		return false;
	}

	private bool IsNumericPropertyValid(CustomTrigger customTrigger, string propertyName, int minValue, uint maxValue, int granularity)
	{
		if (customTrigger.Properties.FirstOrDefault((Property prop) => prop.Name == propertyName) is NumericProperty { Value: not null } numericProperty)
		{
			try
			{
				decimal value = numericProperty.Value.Value;
				if (value % (decimal)granularity == 0m && value >= (decimal)minValue && value <= (decimal)maxValue)
				{
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		return false;
	}

	private void AddValidationError(Guid entityId, EntityType entityType, ValidationErrorCode validationError, List<ErrorEntry> errors)
	{
		errors.Add(new ErrorEntry
		{
			AffectedEntity = new EntityMetadata
			{
				EntityType = entityType,
				Id = entityId
			},
			ErrorCode = validationError,
			ErrorParameters = new List<ErrorParameter>()
		});
	}
}
