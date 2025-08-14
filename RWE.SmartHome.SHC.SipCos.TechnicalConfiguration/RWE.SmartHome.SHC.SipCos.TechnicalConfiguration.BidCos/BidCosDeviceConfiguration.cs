using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Tasks;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos;

public class BidCosDeviceConfiguration
{
	private List<BaseBidCosTask> tasks;

	public Guid DeviceId { get; private set; }

	public bool IsEmpty
	{
		get
		{
			if (tasks != null)
			{
				return tasks.Count <= 0;
			}
			return true;
		}
	}

	public BidCosDeviceConfiguration(Guid deviceId)
	{
		DeviceId = deviceId;
		tasks = new List<BaseBidCosTask>();
	}

	public BidCosDeviceConfiguration(TechnicalConfigurationEntity technicalEntity)
	{
		DeviceId = technicalEntity.Id;
		PopulateTasksFromBytes(technicalEntity.Data);
	}

	public BidCosDeviceConfiguration GetDiff(BidCosDeviceConfiguration previousConfiguration)
	{
		if (previousConfiguration.DeviceId != DeviceId)
		{
			throw new ArgumentException($"Cannot compute the diff for different DeviceIds in BidCosDeviceConfiguarations ({DeviceId} <> {previousConfiguration.DeviceId})");
		}
		BidCosDeviceConfiguration bidCosDeviceConfiguration = new BidCosDeviceConfiguration(DeviceId);
		BaseBidCosTask task;
		foreach (BaseBidCosTask task2 in tasks)
		{
			task = task2;
			BaseBidCosTask baseBidCosTask = previousConfiguration.tasks.FirstOrDefault((BaseBidCosTask m) => m.Type == task.Type && m.Channel == task.Channel);
			if (baseBidCosTask == null || !baseBidCosTask.Equals(task))
			{
				bidCosDeviceConfiguration.AddTask(task);
			}
		}
		return bidCosDeviceConfiguration;
	}

	public void AddTask(BaseBidCosTask task)
	{
		tasks.Add(task);
	}

	public void Clear()
	{
		tasks.Clear();
	}

	public IEnumerable<BaseBidCosTask> GetTasks()
	{
		return tasks;
	}

	public TechnicalConfigurationEntity GetTechnicalConfigurationEntity()
	{
		byte b = (byte)tasks.Count;
		byte[] array = tasks.Select(SerializeTask).SelectMany((byte[] m) => m).ToArray();
		byte[] array2 = new byte[1 + array.Length];
		array2[0] = b;
		Array.Copy(array, 0, array2, 1, array.Length);
		TechnicalConfigurationEntity technicalConfigurationEntity = new TechnicalConfigurationEntity();
		technicalConfigurationEntity.Id = DeviceId;
		technicalConfigurationEntity.Data = array2;
		return technicalConfigurationEntity;
	}

	private void PopulateTasksFromBytes(byte[] data)
	{
		int num = data[0];
		tasks = new List<BaseBidCosTask>(num);
		int pointer = 1;
		for (int i = 0; i < num; i++)
		{
			BaseBidCosTask item = DeserializeTask(data, ref pointer);
			tasks.Add(item);
		}
	}

	private byte[] SerializeTask(BaseBidCosTask task)
	{
		byte[] bytes = task.GetBytes();
		byte b = (byte)bytes.Length;
		byte[] array = new byte[1 + b];
		array[0] = b;
		Array.Copy(bytes, 0, array, 1, b);
		return array;
	}

	private BaseBidCosTask DeserializeTask(byte[] data, ref int pointer)
	{
		int num = data[pointer++];
		byte[] array = new byte[num];
		Array.Copy(data, pointer, array, 0, num);
		pointer += num;
		return BidCosTaskFactory.GetBidCosTask(array);
	}
}
