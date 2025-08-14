using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public abstract class SensorConfiguration : TechnicalConfigurationCreator
{
	protected SensorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
	}

	public abstract bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule);

	public virtual void CreateInternalLinks(IEnumerable<ActuatorConfiguration> actuatorConfigurations)
	{
	}

	public abstract IEnumerable<int> GetUsedChannels(Trigger trigger);

	protected IEnumerable<LinkPartner> CreateActuatorLinks(byte channelIndex, ActuatorConfiguration actuator, Guid sensorId, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		LinkPartner sensor = new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, channelIndex);
		try
		{
			return actuator.CreateLinks(sensor, action, switchEvent, aboveEvent, belowEvent, comparisonValuePercent, rule);
		}
		catch (IncompatibleLinkTypeException)
		{
			throw ConversionHelpers.GetIncompatibleSensorActuatorException(sensorId.ToString("N"), action.Target.EntityIdAsGuid().ToString("N"));
		}
		catch (LinkAlreadyExistsException)
		{
			throw ConversionHelpers.GetDuplicateSensorActuatorCombinationException(sensorId.ToString("N"), action.Target.EntityIdAsGuid().ToString("N"));
		}
		catch (IncompatibleButtonActionException)
		{
			throw ConversionHelpers.GetIncompatibleButtonActionException(sensorId.ToString("N"), action.Target.EntityIdAsGuid().ToString("N"));
		}
	}

	protected IEnumerable<LinkPartner> CreateInternalActuatorLinks(byte channelIndex, ActuatorConfiguration actuator, InternalLinkType linkType)
	{
		LinkPartner sensor = new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, channelIndex);
		try
		{
			return actuator.CreateInternalLinks(sensor, linkType);
		}
		catch (IncompatibleLinkTypeException)
		{
			throw GetTransformationException(ValidationErrorCode.IncompatibleSensorActuatorCombination);
		}
		catch (LinkAlreadyExistsException)
		{
			throw GetTransformationException(ValidationErrorCode.DuplicateSensorActuatorCombination);
		}
	}

	protected int GetOperandIntegerValue(DataBinding operand)
	{
		if (operand is ConstantNumericBinding)
		{
			try
			{
				return (int)(operand as ConstantNumericBinding).Value;
			}
			catch
			{
				return -1;
			}
		}
		return -1;
	}

	private TransformationException GetTransformationException(ValidationErrorCode errorCode)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = EntityType.LogicalDevice
		};
		errorEntry.ErrorCode = errorCode;
		ErrorEntry errorEntry2 = errorEntry;
		errorEntry2.ErrorParameters.Add(new ErrorParameter
		{
			Key = ErrorParameterKey.LogicalDeviceId,
			Value = base.LogicalDeviceContract.Id.ToString()
		});
		return new TransformationException(errorEntry2);
	}
}
