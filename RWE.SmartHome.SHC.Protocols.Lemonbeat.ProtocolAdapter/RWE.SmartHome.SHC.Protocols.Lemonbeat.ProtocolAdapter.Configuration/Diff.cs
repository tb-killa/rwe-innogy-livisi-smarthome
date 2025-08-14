using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

public static class Diff
{
	public static ConfigurationDifference<T> CalculateDifference<T>(this IEnumerable<T> current, IEnumerable<T> target) where T : ConfigurationItem
	{
		ConfigurationDifference<T> configurationDifference = new ConfigurationDifference<T>();
		T configItem;
		foreach (T item in target)
		{
			configItem = item;
			ConfigurationItem configurationItem = current.Where(delegate(T ci)
			{
				uint id = ci.Id;
				T val2 = configItem;
				return id == val2.Id;
			}).FirstOrDefault();
			if (configurationItem == null || !configurationItem.Equals(configItem))
			{
				configurationDifference.ToSet.Add(configItem);
			}
		}
		T oldItem;
		foreach (T item2 in current)
		{
			oldItem = item2;
			if (!target.Any(delegate(T ci)
			{
				uint id = ci.Id;
				T val2 = oldItem;
				return id == val2.Id;
			}))
			{
				IList<uint> toDelete = configurationDifference.ToDelete;
				T val = oldItem;
				toDelete.Add(val.Id);
			}
		}
		return configurationDifference;
	}

	public static void ApplyDifference<T>(this IList<T> current, ConfigurationDifference<T> diff) where T : ConfigurationItem
	{
		if (diff == null)
		{
			return;
		}
		List<T> list = current.Where((T ci) => diff.ToDelete.Any((uint id) => ci.Id == id)).ToList();
		list.ForEach(delegate(T ci)
		{
			current.Remove(ci);
		});
		T value;
		foreach (T item in diff.ToSet)
		{
			value = item;
			T val = current.Where(delegate(T c)
			{
				uint id = c.Id;
				T val2 = value;
				return id == val2.Id;
			}).FirstOrDefault();
			if (val != null)
			{
				current.Remove(val);
			}
			current.Add(value);
		}
	}
}
