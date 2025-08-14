using System;
using onrkn;

namespace Rebex.IO.FileSystem;

public class NodeContentParameters : IEquatable<NodeContentParameters>
{
	public static readonly NodeContentParameters ReadAccess = new NodeContentParameters(NodeContentAccess.Read);

	public static readonly NodeContentParameters WriteAccess = new NodeContentParameters(NodeContentAccess.Write);

	public static readonly NodeContentParameters ReadWriteAccess = new NodeContentParameters(NodeContentAccess.ReadWrite);

	private NodeContentAccess dqsbz;

	public NodeContentAccess AccessType
	{
		get
		{
			return dqsbz;
		}
		private set
		{
			dqsbz = value;
		}
	}

	public NodeContentParameters(NodeContentAccess accessType)
	{
		AccessType = accessType;
	}

	public bool Equals(NodeContentParameters other)
	{
		if (object.ReferenceEquals(null, other) && 0 == 0)
		{
			return false;
		}
		if (object.ReferenceEquals(this, other) && 0 == 0)
		{
			return true;
		}
		return AccessType == other.AccessType;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj) && 0 == 0)
		{
			return true;
		}
		if ((object)obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((NodeContentParameters)obj);
	}

	public override int GetHashCode()
	{
		return (int)AccessType;
	}

	public static bool operator ==(NodeContentParameters left, NodeContentParameters right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(NodeContentParameters left, NodeContentParameters right)
	{
		return !object.Equals(left, right);
	}

	public override string ToString()
	{
		return brgjd.edcru("AccessType: {0}", AccessType);
	}
}
