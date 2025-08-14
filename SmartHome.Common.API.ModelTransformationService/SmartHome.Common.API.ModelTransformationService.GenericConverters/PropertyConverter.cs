using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling.Exceptions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.GenericConverters;

public class PropertyConverter
{
	private static readonly ILogger logger = LogManager.Instance.GetLogger(typeof(PropertyConverter));

	public static SmartHome.Common.API.Entities.Entities.Property ToApiProperty(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property shProperty)
	{
		if (shProperty == null)
		{
			throw new ArgumentNullException("shProperty");
		}
		SmartHome.Common.API.Entities.Entities.Property property = new SmartHome.Common.API.Entities.Entities.Property();
		property.Name = shProperty.Name;
		property.LastChanged = shProperty.UpdateTimestamp;
		property.Value = null;
		SmartHome.Common.API.Entities.Entities.Property property2 = property;
		while (true)
		{
			if (shProperty is StringProperty stringProperty)
			{
				property2.Value = stringProperty.Value;
				break;
			}
			if (shProperty is NumericProperty numericProperty)
			{
				property2.Value = numericProperty.Value;
				break;
			}
			if (shProperty is BooleanProperty booleanProperty)
			{
				property2.Value = booleanProperty.Value;
				break;
			}
			if (shProperty is DateTimeProperty dateTimeProperty)
			{
				property2.Value = dateTimeProperty.Value;
				break;
			}
			logger.LogAndThrow<NotSupportedDataTypeException>($"Unsupported type {shProperty.GetType().Name} for Smarthome property value {shProperty.GetValueAsString()}");
		}
		return property2;
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToSmartHomeProperty(Parameter parameter)
	{
		return GetPropertyForType(parameter.Constant.Value.GetType(), parameter.Name, parameter.Constant.Value, null);
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToSmartHomeProperty(SmartHome.Common.API.Entities.Entities.Property apiProperty)
	{
		if (apiProperty.Value == null)
		{
			throw new MissingFieldException($"The field 'value' is missing for the property {apiProperty.Name}");
		}
		return GetPropertyForType(apiProperty.Value.GetType(), apiProperty.Name, apiProperty.Value, apiProperty.LastChanged);
	}

	private static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property GetPropertyForType(Type type, string name, object value, DateTime? updateTimestamp)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property property = null;
		try
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.String:
			{
				StringProperty stringProperty = new StringProperty();
				stringProperty.Name = name;
				stringProperty.Value = (string)value;
				property = stringProperty;
				break;
			}
			case TypeCode.Boolean:
			{
				BooleanProperty booleanProperty = new BooleanProperty();
				booleanProperty.Name = name;
				booleanProperty.Value = (bool)value;
				property = booleanProperty;
				break;
			}
			case TypeCode.DateTime:
			{
				DateTimeProperty dateTimeProperty = new DateTimeProperty();
				dateTimeProperty.Name = name;
				dateTimeProperty.Value = (DateTime)value;
				property = dateTimeProperty;
				break;
			}
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
			{
				NumericProperty numericProperty = new NumericProperty();
				numericProperty.Name = name;
				numericProperty.Value = Convert.ToDecimal(value);
				property = numericProperty;
				break;
			}
			default:
				throw new NotSupportedException();
			}
			property.UpdateTimestamp = updateTimestamp;
		}
		catch
		{
			string arg = value.ToString();
			logger.LogAndThrow<NotSupportedDataTypeException>($"Unsupported type {type.Name} for property value {arg}");
		}
		return property;
	}
}
