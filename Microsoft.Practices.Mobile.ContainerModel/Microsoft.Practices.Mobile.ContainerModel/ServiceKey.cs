using System;

namespace Microsoft.Practices.Mobile.ContainerModel;

internal sealed class ServiceKey
{
	private int hash;

	public Type FactoryType;

	public string Name;

	public ServiceKey(Type factoryType, string serviceName)
	{
		FactoryType = factoryType;
		Name = serviceName;
		hash = factoryType.GetHashCode();
		if (serviceName != null)
		{
			hash ^= serviceName.GetHashCode();
		}
	}

	public bool Equals(ServiceKey other)
	{
		return Equals(this, other);
	}

	public override bool Equals(object obj)
	{
		return Equals(this, obj as ServiceKey);
	}

	public static bool Equals(ServiceKey obj1, ServiceKey obj2)
	{
		if (object.Equals(null, obj1) || object.Equals(null, obj2))
		{
			return false;
		}
		if ((object)obj1.FactoryType == obj2.FactoryType)
		{
			return obj1.Name == obj2.Name;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return hash;
	}
}
