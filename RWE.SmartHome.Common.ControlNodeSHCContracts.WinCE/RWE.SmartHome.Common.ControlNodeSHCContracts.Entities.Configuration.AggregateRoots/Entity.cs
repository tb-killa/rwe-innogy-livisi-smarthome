using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

[XmlInclude(typeof(Interaction))]
[XmlInclude(typeof(Rule))]
[XmlInclude(typeof(Location))]
[XmlInclude(typeof(BaseDevice))]
[XmlInclude(typeof(ActionDescription))]
[XmlInclude(typeof(LogicalDevice))]
public abstract class Entity
{
	public Guid Id { get; set; }

	[XmlIgnore]
	public Guid CloneTag { get; set; }

	[XmlIgnore]
	public bool IsClone => CloneTag != Guid.Empty;

	[XmlIgnore]
	public int Version { get; set; }

	public List<Tag> Tags { get; set; }

	protected Entity()
	{
		Id = Guid.NewGuid();
		Version = 0;
		Tags = new List<Tag>();
	}

	protected abstract Entity CreateClone();

	protected virtual void TransferProperties(Entity clone)
	{
		clone.Id = Id;
		clone.Version = Version;
		clone.Tags.AddRange(Tags);
	}

	protected T GetClassAttribute<T>(Type type) where T : Attribute
	{
		T val = (T)type.GetCustomAttributes(typeof(T), inherit: false).SingleOrDefault((object attr) => attr is T);
		if (val == null)
		{
			Type[] interfaces = GetType().GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				val = (T)type2.GetCustomAttributes(typeof(T), inherit: false).SingleOrDefault((object attr) => attr is T);
				if (val != null)
				{
					break;
				}
			}
		}
		return val;
	}

	protected List<T> GetClassAttributes<T>(Type type) where T : Attribute
	{
		IEnumerable<T> enumerable = (from attr in type.GetCustomAttributes(typeof(T), inherit: false)
			where attr is T
			select attr).Cast<T>();
		if (enumerable != null)
		{
			return enumerable.ToList();
		}
		List<T> list = new List<T>();
		Type[] interfaces = GetType().GetInterfaces();
		foreach (Type type2 in interfaces)
		{
			IEnumerable<T> enumerable2 = (from attr in type2.GetCustomAttributes(typeof(T), inherit: false)
				where attr is T
				select attr).Cast<T>();
			if (enumerable2 != null)
			{
				list.AddRange(enumerable2);
			}
		}
		return list;
	}

	public virtual Entity Clone()
	{
		return Clone(Guid.Empty);
	}

	public virtual Entity Clone(Guid tag)
	{
		Entity entity = CreateClone();
		entity.CloneTag = tag;
		TransferProperties(entity);
		return entity;
	}
}
