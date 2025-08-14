using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

public class CcSchedulerChannel : ActuatorChannel<ActuatorLink>
{
	public TimeSpan ExerciseTime { get; set; }

	public DayOfWeek ExerciseWeekDay { get; set; }

	public List<SchedulerSetpoint>[] Setpoints { get; private set; }

	public CcSchedulerChannel(byte channelIndex, byte maxLinkCount)
		: base(channelIndex, maxLinkCount)
	{
		ExerciseTime = new TimeSpan(11, 0, 0);
		ExerciseWeekDay = DayOfWeek.Saturday;
		Setpoints = new List<SchedulerSetpoint>[7];
		for (int i = 0; i < 7; i++)
		{
			Setpoints[i] = new List<SchedulerSetpoint>();
		}
	}

	protected override ConfigurationLink CreateLinkWithGlobalSettings()
	{
		ConversionHelpers.CheckTimeOfDayRange(ExerciseTime, "ExerciseTime");
		ConfigurationLink configurationLink = new ConfigurationLink();
		ParameterList parameterList = configurationLink[1];
		parameterList[10] = ConversionHelpers.ConvertDayOfWeek(ExerciseWeekDay);
		parameterList[11] = ConversionHelpers.ConvertExcerciseTimeOfDay(ExerciseTime);
		ParameterList link = configurationLink[5];
		for (int i = 0; i < 7; i++)
		{
			byte b = (byte)(i * 16 + 1);
			List<SchedulerSetpoint> list = Setpoints[i];
			list.Sort();
			RemoveEarlierSetpointsWithSameRoundedTime(list);
			foreach (SchedulerSetpoint item in list)
			{
				item.SaveToLink(link, b);
				b += 2;
			}
			for (int j = list.Count; j < 8; j++)
			{
				SchedulerSetpoint.SaveDefaultToLink(link, b);
				b += 2;
			}
		}
		return configurationLink;
	}

	private static void RemoveEarlierSetpointsWithSameRoundedTime(List<SchedulerSetpoint> setpoints)
	{
		short num = -1;
		SchedulerSetpoint item = null;
		List<SchedulerSetpoint> list = new List<SchedulerSetpoint>();
		foreach (SchedulerSetpoint setpoint in setpoints)
		{
			short num2 = ConversionHelpers.ConvertTimeOfDay(setpoint.Time);
			if (num2 == num)
			{
				list.Add(item);
			}
			item = setpoint;
			num = num2;
		}
		foreach (SchedulerSetpoint item2 in list)
		{
			setpoints.Remove(item2);
		}
	}

	public bool AddLink(LinkPartner sensor, ActionDescription action, SwitchAction switchAction, SwitchAction aboveAction, SwitchAction belowAction)
	{
		Parameter parameter = action.Data.SingleOrDefault((Parameter p) => p.Name == "OperationMode");
		if (parameter == null || !(parameter.Value is ConstantStringBinding))
		{
			return false;
		}
		if (switchAction != SwitchAction.Default || (aboveAction == SwitchAction.Default && belowAction == SwitchAction.Default))
		{
			throw new IncompatibleLinkTypeException();
		}
		return false;
	}
}
