using System;
using System.Collections.Generic;

namespace onrkn;

internal class lpmle<T0, T1> : IEquatable<lpmle<T0, T1>>
{
	private const string kfpwb = "Either Left - {0}";

	private const string xfsdl = "Either Right - {0}";

	private bool pblxd;

	private T0 djfdb;

	private T1 phqoj;

	public bool npcau
	{
		get
		{
			return pblxd;
		}
		private set
		{
			pblxd = value;
		}
	}

	public bool aqgkc => !npcau;

	public T0 aqnij
	{
		get
		{
			return djfdb;
		}
		private set
		{
			djfdb = value;
		}
	}

	public T1 obvbo
	{
		get
		{
			return phqoj;
		}
		private set
		{
			phqoj = value;
		}
	}

	public lpmle(T0 left)
	{
		if (left == null || 1 == 0)
		{
			throw new ArgumentNullException("left");
		}
		aqnij = left;
		obvbo = default(T1);
		npcau = true;
	}

	public lpmle(T1 right)
	{
		if (right == null || 1 == 0)
		{
			throw new ArgumentNullException("right");
		}
		obvbo = right;
		aqnij = default(T0);
		npcau = false;
	}

	public bool Equals(lpmle<T0, T1> other)
	{
		if (object.ReferenceEquals(null, other) && 0 == 0)
		{
			return false;
		}
		if (object.ReferenceEquals(this, other) && 0 == 0)
		{
			return true;
		}
		if (!npcau || 1 == 0)
		{
			if (other.aqgkc && 0 == 0)
			{
				return EqualityComparer<T1>.Default.Equals(obvbo, other.obvbo);
			}
			return false;
		}
		if (other.npcau && 0 == 0)
		{
			return EqualityComparer<T0>.Default.Equals(aqnij, other.aqnij);
		}
		return false;
	}

	public TR kmgai<TR>(Func<T0, TR> p0, Func<T1, TR> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("leftFunc");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("rightFunc");
		}
		if (!npcau || 1 == 0)
		{
			return p1(obvbo);
		}
		return p0(aqnij);
	}

	public void dzzno(Action<T0> p0, Action<T1> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("leftAction");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("rightAction");
		}
		kmgai(p0.kbiid(), p1.kbiid());
	}

	public static implicit operator lpmle<T0, T1>(T0 left)
	{
		return new lpmle<T0, T1>(left);
	}

	public static implicit operator lpmle<T0, T1>(T1 right)
	{
		return new lpmle<T0, T1>(right);
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
		return Equals((lpmle<T0, T1>)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = GetType().GetHashCode();
		return (npcau ? true : false) ? ((hashCode * 397) ^ EqualityComparer<T0>.Default.GetHashCode(aqnij)) : ((hashCode * 397) ^ EqualityComparer<T1>.Default.GetHashCode(obvbo));
	}

	public static bool operator ==(lpmle<T0, T1> first, lpmle<T0, T1> second)
	{
		return object.Equals(first, second);
	}

	public static bool operator !=(lpmle<T0, T1> first, lpmle<T0, T1> second)
	{
		return !object.Equals(first, second);
	}

	public override string ToString()
	{
		return kmgai(mnits, gumvx);
	}

	private string mnits(T0 p0)
	{
		return brgjd.edcru("Either Left - {0}", aqnij);
	}

	private string gumvx(T1 p0)
	{
		return brgjd.edcru("Either Right - {0}", obvbo);
	}
}
