using System.Collections;
using System.Collections.Generic;

namespace onrkn;

internal class cogva<T0> : IEnumerable<T0>, IEnumerable
{
	private readonly List<T0> afgqu;

	private readonly object ekeqo;

	public int erlfn
	{
		get
		{
			lock (ekeqo)
			{
				return afgqu.Count;
			}
		}
	}

	public T0 this[int index]
	{
		get
		{
			lock (ekeqo)
			{
				return afgqu[index];
			}
		}
	}

	public cogva()
	{
		afgqu = new List<T0>();
		ekeqo = new object();
	}

	public void ifjux(T0 p0)
	{
		lock (ekeqo)
		{
			afgqu.Add(p0);
		}
	}

	public void vhtoe(IEnumerable<T0> p0)
	{
		lock (ekeqo)
		{
			afgqu.AddRange(p0);
		}
	}

	public void hurkf(T0 p0)
	{
		lock (ekeqo)
		{
			afgqu.Remove(p0);
		}
	}

	public bool beelj(T0 p0)
	{
		lock (ekeqo)
		{
			return afgqu.Contains(p0);
		}
	}

	public List<T0> vpejr()
	{
		lock (ekeqo)
		{
			return new List<T0>(afgqu);
		}
	}

	public apajk<T0> ynizz()
	{
		lock (ekeqo)
		{
			if (afgqu.Count == 0 || 1 == 0)
			{
				return vvchs.apcdm<T0>();
			}
			apajk<T0> result = vvchs.zyyfq(afgqu[0]);
			afgqu.RemoveAt(0);
			return result;
		}
	}

	public T0[] ftqmn()
	{
		lock (ekeqo)
		{
			return afgqu.ToArray();
		}
	}

	public IEnumerator<T0> GetEnumerator()
	{
		return vpejr().GetEnumerator();
	}

	private IEnumerator vdldv()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in vdldv
		return this.vdldv();
	}

	public void zhkfw()
	{
		lock (ekeqo)
		{
			afgqu.Clear();
		}
	}
}
