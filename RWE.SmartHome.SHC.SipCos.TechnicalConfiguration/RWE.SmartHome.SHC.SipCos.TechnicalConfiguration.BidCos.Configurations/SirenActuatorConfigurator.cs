using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Tasks;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Configurations;

public class SirenActuatorConfigurator : BaseBidCosConfigurator
{
	private const byte notificationChannel = 1;

	private const byte feedbackChannel = 2;

	private const byte alarmChannel = 3;

	public SirenActuatorConfigurator(LogicalDevice ld, byte[] sourceAdress, byte[] deviceAdress)
		: base(ld, sourceAdress, deviceAdress)
	{
	}

	public override IEnumerable<BaseBidCosTask> GetTasks()
	{
		string stringValue = base.LogicalDevice.Properties.GetStringValue("AlarmSoundId");
		string stringValue2 = base.LogicalDevice.Properties.GetStringValue("FeedbackSoundId");
		string stringValue3 = base.LogicalDevice.Properties.GetStringValue("NotificationSoundId");
		byte soundValue = GetSoundValue(stringValue);
		byte soundValue2 = GetSoundValue(stringValue2);
		byte soundValue3 = GetSoundValue(stringValue3);
		List<BaseBidCosTask> list = new List<BaseBidCosTask>();
		list.Add(GetSoundTask(3, soundValue));
		list.Add(GetSoundTask(2, soundValue2));
		list.Add(GetSoundTask(1, soundValue3));
		return list;
	}

	private ConfigBidCosTask GetSoundTask(byte channel, byte soundId)
	{
		ConfigBidCosTask configBidCosTask = new ConfigBidCosTask(channel, base.SourceAddress, base.DeviceAddress);
		configBidCosTask.Params = new Dictionary<byte, byte> { { 171, soundId } };
		return configBidCosTask;
	}

	private byte GetSoundValue(string sound)
	{
		if (sound == null || sound.Length != 3)
		{
			throw new TransformationException($"Unknown format for sound value (val:{sound})", new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.BaseDevice,
					Id = base.LogicalDevice.BaseDeviceId
				},
				ErrorCode = ValidationErrorCode.Unknown
			});
		}
		int num = sound[0] - 48 + 1;
		if (num < 1 || num > 9)
		{
			throw new TransformationException($"Unknown signal value ({num - 1})", new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.BaseDevice,
					Id = base.LogicalDevice.BaseDeviceId
				},
				ErrorCode = ValidationErrorCode.Unknown
			});
		}
		int num2 = 0;
		num2 = sound[1] switch
		{
			'L' => 0, 
			'M' => 1, 
			'H' => 2, 
			'V' => 3, 
			_ => -1, 
		};
		if (num2 < 0 || num2 > 3)
		{
			throw new TransformationException($"Unknown tone value ({num2})", new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.BaseDevice,
					Id = base.LogicalDevice.BaseDeviceId
				},
				ErrorCode = ValidationErrorCode.Unknown
			});
		}
		int num3 = 0;
		num3 = sound[2] switch
		{
			'S' => 0, 
			'F' => 1, 
			_ => -1, 
		};
		if (num3 < 0 || num3 > 1)
		{
			throw new TransformationException($"Unknown speed value ({num3})", new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.BaseDevice,
					Id = base.LogicalDevice.BaseDeviceId
				},
				ErrorCode = ValidationErrorCode.Unknown
			});
		}
		return (byte)(num3 * 36 + num2 * 9 + num);
	}
}
