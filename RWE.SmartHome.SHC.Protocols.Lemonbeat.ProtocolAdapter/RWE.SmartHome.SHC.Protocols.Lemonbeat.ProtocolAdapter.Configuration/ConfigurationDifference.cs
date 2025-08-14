using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

public class ConfigurationDifference<T> where T : ConfigurationItem
{
	private readonly IList<T> toSet = new List<T>();

	private readonly IList<uint> toDelete = new List<uint>();

	public IList<T> ToSet => toSet;

	public IList<uint> ToDelete => toDelete;

	public bool IsEmpty
	{
		get
		{
			if (ToSet.Count == 0)
			{
				return ToDelete.Count == 0;
			}
			return false;
		}
	}
}
