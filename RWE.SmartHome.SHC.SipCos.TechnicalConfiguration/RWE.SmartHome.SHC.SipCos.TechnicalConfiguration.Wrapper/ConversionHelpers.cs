using System;
using System.Globalization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper;

public static class ConversionHelpers
{
	private const byte MaxExcerciseTime = 143;

	private const short MaxSetpointTime = 287;

	private static readonly int[] Divisors = new int[7] { 10, 5, 2, 6, 5, 2, 6 };

	private static readonly TimeSpan MinTime = new TimeSpan(0, 0, 0, 0);

	private static readonly TimeSpan MaxTime = new TimeSpan(1, 0, 0, 0);

	public static byte TenthSecondsToByte(int durationInTenthSeconds, bool nullIsInfinite, string name)
	{
		if (durationInTenthSeconds == 0)
		{
			return (byte)(nullIsInfinite ? 255u : 0u);
		}
		if (durationInTenthSeconds > 0)
		{
			int num = durationInTenthSeconds;
			int num2 = 0;
			while (num2 < Divisors.Length || num < 32)
			{
				if (num < 32)
				{
					byte b = (byte)((num2 << 5) + num);
					if (nullIsInfinite && b == byte.MaxValue)
					{
						break;
					}
					return b;
				}
				int num3 = Divisors[num2++];
				if (num % num3 != 0)
				{
					break;
				}
				num /= num3;
			}
		}
		throw CreateParameterException(ValidationErrorCode.TimeSpan, name, durationInTenthSeconds.ToString(CultureInfo.InvariantCulture));
	}

	public static byte ConvertCentigrade(decimal temperature)
	{
		if (temperature < 0m)
		{
			return 0;
		}
		if (temperature > 100m)
		{
			return 200;
		}
		return (byte)Math.Round(temperature * 2m);
	}

	public static byte ConvertExcerciseTimeOfDay(TimeSpan timeSpan)
	{
		byte b = (byte)Math.Round(timeSpan.TotalMinutes * 0.1);
		if (b > 143)
		{
			b = 143;
		}
		return b;
	}

	public static short ConvertTimeOfDay(TimeSpan timeSpan)
	{
		short num = (short)Math.Round(timeSpan.TotalMinutes * 0.2);
		if (num > 287)
		{
			num = 287;
		}
		return num;
	}

	public static byte ConvertDayOfWeek(DayOfWeek day)
	{
		return day switch
		{
			DayOfWeek.Monday => 1, 
			DayOfWeek.Tuesday => 2, 
			DayOfWeek.Wednesday => 3, 
			DayOfWeek.Thursday => 4, 
			DayOfWeek.Friday => 5, 
			DayOfWeek.Saturday => 6, 
			DayOfWeek.Sunday => 7, 
			_ => 6, 
		};
	}

	public static byte ConvertDimLevel(int minimum, int maximum, int dimLevel)
	{
		if (dimLevel <= 0)
		{
			return 0;
		}
		int num = Math.Min(100, dimLevel);
		return (byte)Math.Round(2.0 * ((double)(num - 1) * (1.0 / 99.0) * (double)(maximum - minimum) + (double)minimum));
	}

	public static byte GetConditionalOperation(ref SwitchAction action, SwitchAction belowAction, SwitchAction aboveAction)
	{
		byte result = 0;
		if (action == SwitchAction.Default)
		{
			if (belowAction == SwitchAction.Default)
			{
				result = 0;
				action = aboveAction;
			}
			else if (aboveAction == SwitchAction.Default)
			{
				result = 34;
				action = belowAction;
			}
			else if (aboveAction == belowAction)
			{
				result = 85;
				action = aboveAction;
			}
			else
			{
				action = SwitchAction.Toggle;
				result = (byte)((aboveAction == SwitchAction.Off) ? 32u : 2u);
			}
		}
		return result;
	}

	public static SwitchAction GetConditionalSwitchAction(ProfileAction profileAction)
	{
		return profileAction switch
		{
			ProfileAction.On => SwitchAction.On, 
			ProfileAction.Off => SwitchAction.Off, 
			ProfileAction.NoAction => SwitchAction.Default, 
			_ => throw new ArgumentOutOfRangeException("profileAction"), 
		};
	}

	public static byte? ConvertPercentToByteRange(int? percent, string parameterName)
	{
		if (!percent.HasValue)
		{
			return null;
		}
		CheckPercentageRange(percent.Value, parameterName);
		return (byte)Math.Round((double)percent.Value * 255.0 / 100.0);
	}

	public static void CheckTimeOfDayRange(TimeSpan value, string parameterName)
	{
		if (value < MinTime || value > MaxTime)
		{
			throw CreateParameterException(ValidationErrorCode.TimeSpan, parameterName, value);
		}
	}

	public static void CheckTimeOfDayResolution(TimeSpan value, string parameterName)
	{
		if (value.Seconds > 0 || value.Milliseconds > 0 || value.Minutes % 5 > 0)
		{
			throw CreateParameterException(ValidationErrorCode.TimeSpan, parameterName, value);
		}
	}

	public static void CheckTimeParameter(int minimum, int maximum, int toCheck, string name)
	{
		if (toCheck < minimum || toCheck > maximum)
		{
			throw CreateParameterException(ValidationErrorCode.TimeSpan, name, toCheck);
		}
	}

	public static void CheckTemperatureRange(decimal value, string parameterName)
	{
		if (value < 6m || value > 30m)
		{
			throw CreateParameterException(ValidationErrorCode.TemperatureOutOfRange, parameterName, value);
		}
	}

	public static void CheckPercentageRange(decimal value, string parameterName)
	{
		if (value < 0m || value > 100m)
		{
			throw CreateParameterException(ValidationErrorCode.PercentageOutOfRange, parameterName, value);
		}
	}

	public static SwitchAction FromInternalLinkType(InternalLinkType linkType)
	{
		return linkType switch
		{
			InternalLinkType.UpperButton => SwitchAction.UpButton, 
			InternalLinkType.LowerButton => SwitchAction.DownButton, 
			_ => throw new ArgumentOutOfRangeException("linkType"), 
		};
	}

	public static TransformationException CreateParameterException(ValidationErrorCode code, string parameterName, object value)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.ErrorCode = code;
		ErrorEntry errorEntry2 = errorEntry;
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.ParameterName,
			Value = parameterName
		});
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.ParameterValue,
			Value = value.ToString()
		});
		return new TransformationException(errorEntry2);
	}

	public static TransformationException GetIncompatibleSensorActuatorException(string sensorSettingsId, string actuatorSettingsId)
	{
		return GetProfilesTransformationException(ValidationErrorCode.IncompatibleSensorActuatorCombination, sensorSettingsId, actuatorSettingsId);
	}

	public static TransformationException GetDuplicateSensorActuatorCombinationException(string sensorSettingsId, string actuatorSettingsId)
	{
		return GetProfilesTransformationException(ValidationErrorCode.DuplicateSensorActuatorCombination, sensorSettingsId, actuatorSettingsId);
	}

	public static TransformationException GetIncompatibleButtonActionException(string sensorSettingsId, string actuatorSettingsId)
	{
		return GetProfilesTransformationException(ValidationErrorCode.IncompatibleButtonAction, sensorSettingsId, actuatorSettingsId);
	}

	private static TransformationException GetProfilesTransformationException(ValidationErrorCode errorCode, string sensorSettingsId, string actuatorSettingsId)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = EntityType.Interaction
		};
		errorEntry.ErrorCode = errorCode;
		ErrorEntry errorEntry2 = errorEntry;
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.SensorSettingsId,
			Value = ((!string.IsNullOrEmpty(sensorSettingsId)) ? sensorSettingsId : string.Empty)
		});
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.ActuatorSettingsId,
			Value = ((!string.IsNullOrEmpty(actuatorSettingsId)) ? actuatorSettingsId : string.Empty)
		});
		return new TransformationException(errorEntry2);
	}
}
