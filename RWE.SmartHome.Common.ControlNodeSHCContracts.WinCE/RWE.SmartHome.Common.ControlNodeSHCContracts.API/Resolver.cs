using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API.Interfaces;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.API;

public static class Resolver
{
	private static Dictionary<Guid, IEntityCache> cloneCaches = new Dictionary<Guid, IEntityCache>();

	public static IEntityCache EntityCache { get; set; }

	public static void RegisterCloneCache(Guid id, IEntityCache cache)
	{
		if (!cloneCaches.ContainsKey(id))
		{
			cloneCaches.Add(id, cache);
		}
	}

	public static void UnregisterCloneCache(Guid id)
	{
		if (cloneCaches.ContainsKey(id))
		{
			cloneCaches.Remove(id);
		}
	}

	public static Guid? FromInteraction(Interaction interaction)
	{
		return interaction?.Id;
	}

	public static Interaction ToInteraction(Entity caller, Guid? interactionId)
	{
		if (!interactionId.HasValue)
		{
			return null;
		}
		IEntityCache entitysCloneCache = GetEntitysCloneCache(caller);
		if (entitysCloneCache != null)
		{
			Interaction interaction = entitysCloneCache.GetInteraction(interactionId.Value);
			if (interaction != null)
			{
				return interaction;
			}
		}
		return (EntityCache != null) ? EntityCache.GetInteraction(interactionId.Value) : null;
	}

	public static Guid? FromLocation(Location location)
	{
		return location?.Id;
	}

	public static Location ToLocation(Entity caller, Guid? locationId)
	{
		if (!locationId.HasValue)
		{
			return null;
		}
		IEntityCache entitysCloneCache = GetEntitysCloneCache(caller);
		if (entitysCloneCache != null)
		{
			Location location = entitysCloneCache.GetLocation(locationId.Value);
			if (location != null)
			{
				return location;
			}
		}
		return (EntityCache != null) ? EntityCache.GetLocation(locationId.Value) : null;
	}

	public static Guid FromBaseDevice(BaseDevice baseDevice)
	{
		if (baseDevice == null)
		{
			throw new ArgumentNullException("baseDevice");
		}
		return baseDevice.Id;
	}

	public static BaseDevice ToBaseDevice(Entity caller, Guid baseDeviceId)
	{
		IEntityCache entitysCloneCache = GetEntitysCloneCache(caller);
		if (entitysCloneCache != null)
		{
			BaseDevice baseDevice = entitysCloneCache.GetBaseDevice(baseDeviceId);
			if (baseDevice != null)
			{
				return baseDevice;
			}
		}
		return (EntityCache != null) ? EntityCache.GetBaseDevice(baseDeviceId) : null;
	}

	public static Guid FromHome(Home home)
	{
		if (home == null)
		{
			throw new ArgumentNullException("home");
		}
		return home.Id;
	}

	public static Home ToHome(Entity caller, Guid homeId)
	{
		IEntityCache entitysCloneCache = GetEntitysCloneCache(caller);
		if (entitysCloneCache != null)
		{
			Home home = entitysCloneCache.GetHome(homeId);
			if (home != null)
			{
				return home;
			}
		}
		return (EntityCache != null) ? EntityCache.GetHome(homeId) : null;
	}

	public static Guid FromHomeSetup(HomeSetup homeSetup)
	{
		if (homeSetup == null)
		{
			throw new ArgumentNullException("homeSetup");
		}
		return homeSetup.Id;
	}

	public static HomeSetup ToHomeSetup(Entity caller, Guid homeSetupId)
	{
		IEntityCache entitysCloneCache = GetEntitysCloneCache(caller);
		if (entitysCloneCache != null)
		{
			HomeSetup homeSetup = entitysCloneCache.GetHomeSetup(homeSetupId);
			if (homeSetup != null)
			{
				return homeSetup;
			}
		}
		return (EntityCache != null) ? EntityCache.GetHomeSetup(homeSetupId) : null;
	}

	public static Guid FromLogicalDevice(LogicalDevice logicalDevice)
	{
		if (logicalDevice == null)
		{
			throw new ArgumentNullException("logicalDevice");
		}
		return logicalDevice.Id;
	}

	public static LogicalDevice ToLogicalDevice(Entity caller, Guid? logicalDeviceId)
	{
		if (!logicalDeviceId.HasValue)
		{
			return null;
		}
		IEntityCache entitysCloneCache = GetEntitysCloneCache(caller);
		if (entitysCloneCache != null)
		{
			LogicalDevice logicalDevice = entitysCloneCache.GetLogicalDevice(logicalDeviceId.Value);
			if (logicalDevice != null)
			{
				return logicalDevice;
			}
		}
		return (EntityCache != null) ? EntityCache.GetLogicalDevice(logicalDeviceId.Value) : null;
	}

	public static List<Guid> FromLogicalDevices(IEnumerable<LogicalDevice> logicalDevices)
	{
		List<Guid> list = new List<Guid>();
		if (logicalDevices != null)
		{
			list.AddRange(from logicalDevice in logicalDevices
				select FromLogicalDevice(logicalDevice) into id
				where true
				select id);
		}
		return list;
	}

	public static List<LogicalDevice> ToLogicalDevices(Entity caller, List<Guid> logicalDeviceIds)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		List<Guid> unresolvedIds = new List<Guid>();
		if (logicalDeviceIds == null)
		{
			return list;
		}
		IEntityCache cloneCache = GetEntitysCloneCache(caller);
		if (cloneCache != null)
		{
			list.AddRange(from ld in logicalDeviceIds.Select(delegate(Guid logicalDeviceId)
				{
					LogicalDevice logicalDevice = cloneCache.GetLogicalDevice(logicalDeviceId);
					if (logicalDevice == null)
					{
						unresolvedIds.Add(logicalDeviceId);
					}
					return logicalDevice;
				})
				where ld != null
				select ld);
		}
		else
		{
			unresolvedIds = logicalDeviceIds;
		}
		if (EntityCache != null)
		{
			list.AddRange(from logicalDeviceId in unresolvedIds
				select EntityCache.GetLogicalDevice(logicalDeviceId) into ld
				where ld != null
				select ld);
		}
		return list;
	}

	public static Guid FromMember(Member member)
	{
		if (member == null)
		{
			throw new ArgumentNullException("member");
		}
		return member.Id;
	}

	public static Member ToMember(Entity caller, Guid? memberId)
	{
		if (!memberId.HasValue)
		{
			return null;
		}
		IEntityCache entitysCloneCache = GetEntitysCloneCache(caller);
		if (entitysCloneCache != null)
		{
			Member member = entitysCloneCache.GetMember(memberId.Value);
			if (member != null)
			{
				return member;
			}
		}
		return (EntityCache != null) ? EntityCache.GetMember(memberId.Value) : null;
	}

	public static List<Guid> FromMembers(IEnumerable<Member> members)
	{
		List<Guid> list = new List<Guid>();
		if (members != null)
		{
			list.AddRange(from member in members
				select FromMember(member) into id
				where true
				select id);
		}
		return list;
	}

	public static List<Member> ToMembers(Entity caller, List<Guid> memberIds)
	{
		List<Member> list = new List<Member>();
		List<Guid> unresolvedIds = new List<Guid>();
		if (memberIds == null)
		{
			return list;
		}
		IEntityCache cloneCache = GetEntitysCloneCache(caller);
		if (cloneCache != null)
		{
			list.AddRange(from mb in memberIds.Select(delegate(Guid memberId)
				{
					Member member = cloneCache.GetMember(memberId);
					if (member == null)
					{
						unresolvedIds.Add(memberId);
					}
					return member;
				})
				where mb != null
				select mb);
		}
		else
		{
			unresolvedIds = memberIds;
		}
		if (EntityCache != null)
		{
			list.AddRange(from mId in unresolvedIds
				select EntityCache.GetMember(mId) into mm
				where mm != null
				select mm);
		}
		return list;
	}

	private static IEntityCache GetEntitysCloneCache(Entity caller)
	{
		IEntityCache result = null;
		if (caller != null && caller.IsClone)
		{
			Guid cloneTag = caller.CloneTag;
			result = (cloneCaches.ContainsKey(cloneTag) ? cloneCaches[cloneTag] : null);
		}
		return result;
	}
}
