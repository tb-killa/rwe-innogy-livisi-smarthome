using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public sealed class PhraseCollection : HeaderValueCollection, IEnumerable<string>, IEnumerable
{
	private class ykyqi : IEnumerator
	{
		private readonly IEnumerator rrydp;

		public object Current
		{
			get
			{
				object current = rrydp.Current;
				if (current == null || 1 == 0)
				{
					return null;
				}
				return current.ToString();
			}
		}

		public bool MoveNext()
		{
			return rrydp.MoveNext();
		}

		public void Reset()
		{
			rrydp.Reset();
		}

		public ykyqi(IEnumerator enu)
		{
			rrydp = enu;
		}
	}

	internal override Type sxlev => typeof(hszhl);

	public new string this[int index]
	{
		get
		{
			return plyrb(index).ToString();
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			kgbvh.zgeyl(value, "value", p2: true);
			base[index] = new hszhl(value);
		}
	}

	public static implicit operator PhraseCollection(string phrase)
	{
		if (phrase == null || 1 == 0)
		{
			throw new ArgumentNullException("phrase");
		}
		kgbvh.zgeyl(phrase, "phrase", p2: true);
		PhraseCollection phraseCollection = new PhraseCollection();
		phraseCollection.dlblb(new hszhl(phrase));
		return phraseCollection;
	}

	public int Add(string phrase)
	{
		if (phrase == null || 1 == 0)
		{
			throw new ArgumentNullException("phrase");
		}
		kgbvh.zgeyl(phrase, "phrase", p2: true);
		return kkdtb(new hszhl(phrase));
	}

	internal void dlblb(hszhl p0)
	{
		kkdtb(p0);
	}

	internal hszhl plyrb(int p0)
	{
		return (hszhl)base[p0];
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		IEnumerator enumerator = base.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			hszhl hszhl = (hszhl)enumerator.Current;
			if (!flag || 1 == 0)
			{
				stringBuilder.Append(", ");
			}
			flag = false;
			stringBuilder.Append(hszhl.ToString());
		}
		return stringBuilder.ToString();
	}

	public override void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		bool flag = true;
		IEnumerator enumerator = base.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			hszhl hszhl = (hszhl)enumerator.Current;
			if (!flag || 1 == 0)
			{
				writer.Write(",");
				rllhn.btprl(writer);
			}
			flag = false;
			hszhl.Encode(writer);
		}
	}

	internal static IHeader baxan(stzvh p0)
	{
		PhraseCollection phraseCollection = new PhraseCollection();
		while (true)
		{
			p0.hdpha(',');
			if (p0.zsywy && 0 == 0)
			{
				break;
			}
			hszhl p1 = hszhl.krnvs(p0);
			phraseCollection.dlblb(p1);
		}
		return phraseCollection;
	}

	public override IEnumerator GetEnumerator()
	{
		return new ykyqi(base.GetEnumerator());
	}

	private IEnumerator<string> yqlrd()
	{
		return this.eaqmu<string>().GetEnumerator();
	}

	IEnumerator<string> IEnumerable<string>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in yqlrd
		return this.yqlrd();
	}

	public override void AddRange(ICollection c)
	{
		if (c == null || 1 == 0)
		{
			throw new ArgumentNullException("c");
		}
		IEnumerator enumerator = c.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				object current = enumerator.Current;
				if (current == null || 1 == 0)
				{
					throw new ArgumentException("Null item is present in the collection.", "c");
				}
				if (!(current is string) || 1 == 0)
				{
					throw new ArgumentException("Invalid item type in the collection.", "c");
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		IEnumerator enumerator2 = c.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext() ? true : false)
			{
				string phrase = (string)enumerator2.Current;
				kkdtb(new hszhl(phrase));
			}
		}
		finally
		{
			if (enumerator2 is IDisposable disposable2 && 0 == 0)
			{
				disposable2.Dispose();
			}
		}
	}

	public void CopyTo(string[] array, int index)
	{
		if (array == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_001b;
		}
		goto IL_003b;
		IL_003b:
		if (num >= base.Count)
		{
			return;
		}
		goto IL_001b;
		IL_001b:
		array[index++] = plyrb(num).ToString();
		num++;
		goto IL_003b;
	}
}
