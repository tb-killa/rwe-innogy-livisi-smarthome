using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

namespace RWE.SmartHome.SHC.Core.TypeManager;

internal class ShcTypeManager : IShcTypeManager, IService
{
	private Dictionary<string, Dictionary<string, string>> registeredApplications = new Dictionary<string, Dictionary<string, string>>();

	private long shcType = long.MaxValue;

	private object sync = new object();

	private Dictionary<ShcRestriction, List<IRestrictionManager>> restrictionManagers = new Dictionary<ShcRestriction, List<IRestrictionManager>>();

	public bool IsUpdated { get; private set; }

	public ShcTypeManager()
	{
		IsUpdated = false;
	}

	public RestrictionData GetRestrictionData(ShcRestriction restriction, string applicationId)
	{
		RestrictionData restrictionData = new RestrictionData();
		lock (sync)
		{
			restrictionData.IsRestrictionActive = ((ulong)shcType & (ulong)restriction) != 0;
			if (restrictionData.IsRestrictionActive && registeredApplications.ContainsKey(applicationId))
			{
				restrictionData.ApplicationParameters = registeredApplications[applicationId];
			}
			else
			{
				restrictionData.ApplicationParameters = new Dictionary<string, string>();
			}
		}
		return restrictionData;
	}

	public void UpdateShcTypeData(Dictionary<string, Dictionary<string, string>> applicationParameters, long shcType)
	{
		lock (sync)
		{
			registeredApplications.Clear();
			if (applicationParameters != null)
			{
				foreach (KeyValuePair<string, Dictionary<string, string>> applicationParameter in applicationParameters)
				{
					Dictionary<string, string> value = applicationParameter.Value.ToDictionary((KeyValuePair<string, string> v) => v.Key, (KeyValuePair<string, string> v) => v.Value);
					registeredApplications.Add(applicationParameter.Key, value);
				}
			}
			this.shcType = shcType;
			IsUpdated = true;
		}
	}

	public void SubscribeRestrictionManager(ShcRestriction restriction, IRestrictionManager restrictionManager)
	{
		for (int i = 0; i < 64; i++)
		{
			if (((ulong)(1L << i) & (ulong)restriction) != 0 && Enum.IsDefined(typeof(ShcRestriction), 1L << i))
			{
				ShcRestriction key = (ShcRestriction)(1L << i);
				if (restrictionManagers.ContainsKey(key))
				{
					restrictionManagers[key].Add(restrictionManager);
					continue;
				}
				restrictionManagers[key] = new List<IRestrictionManager> { restrictionManager };
			}
		}
	}

	public RestrictionState GetRestrictionState(ShcRestriction restriction)
	{
		if (((ulong)shcType & (ulong)restriction) == 0)
		{
			RestrictionState restrictionState = new RestrictionState();
			restrictionState.IsRestrictionEnabled = false;
			restrictionState.Parameters = null;
			restrictionState.Restriction = restriction;
			return restrictionState;
		}
		List<ShcTypeParameterState> list = null;
		if (restrictionManagers.ContainsKey(restriction))
		{
			list = new List<ShcTypeParameterState>();
			foreach (IRestrictionManager item in restrictionManagers[restriction])
			{
				List<ShcTypeParameterState> restrictionState2 = item.GetRestrictionState(restriction);
				if (restrictionState2 != null)
				{
					list.AddRange(restrictionState2);
				}
			}
		}
		RestrictionState restrictionState3 = new RestrictionState();
		restrictionState3.IsRestrictionEnabled = true;
		restrictionState3.Parameters = list;
		restrictionState3.Restriction = restriction;
		return restrictionState3;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}
}
