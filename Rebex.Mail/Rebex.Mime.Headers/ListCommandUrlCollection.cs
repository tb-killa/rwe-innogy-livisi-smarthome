using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public sealed class ListCommandUrlCollection : HeaderValueCollection, IEnumerable<ListCommandUrl>, IEnumerable
{
	internal override Type sxlev => typeof(ListCommandUrl);

	public new ListCommandUrl this[int index]
	{
		get
		{
			return (ListCommandUrl)base[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			base[index] = value;
		}
	}

	public ListCommandUrlCollection()
	{
	}

	public static implicit operator ListCommandUrlCollection(ListCommandUrl url)
	{
		if (url == null || 1 == 0)
		{
			throw new ArgumentNullException("url");
		}
		ListCommandUrlCollection listCommandUrlCollection = new ListCommandUrlCollection();
		listCommandUrlCollection.Add(url);
		return listCommandUrlCollection;
	}

	public static implicit operator ListCommandUrlCollection(string urls)
	{
		if (urls == null || 1 == 0)
		{
			throw new ArgumentNullException("urls");
		}
		return new ListCommandUrlCollection(new stzvh(urls));
	}

	public int Add(ListCommandUrl url)
	{
		if (url == null || 1 == 0)
		{
			throw new ArgumentNullException("url");
		}
		return kkdtb(url);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				ListCommandUrl listCommandUrl = (ListCommandUrl)enumerator.Current;
				if (!flag || 1 == 0)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append(listCommandUrl.ToString());
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
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
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				ListCommandUrl listCommandUrl = (ListCommandUrl)enumerator.Current;
				if (!flag || 1 == 0)
				{
					writer.Write(",");
					rllhn.btprl(writer);
				}
				flag = false;
				listCommandUrl.Encode(writer);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}

	private ListCommandUrlCollection(stzvh reader)
		: this()
	{
		while (true)
		{
			reader.hdpha(',', ';', '<');
			if (reader.zsywy && 0 == 0)
			{
				break;
			}
			Add(ListCommandUrl.jvaot(reader));
		}
	}

	internal static IHeader fjbra(stzvh p0)
	{
		return new ListCommandUrlCollection(p0);
	}

	public void CopyTo(ListCommandUrl[] array, int index)
	{
		CopyTo((Array)array, index);
	}

	private IEnumerator<ListCommandUrl> wrnjl()
	{
		return this.eaqmu<ListCommandUrl>().GetEnumerator();
	}

	IEnumerator<ListCommandUrl> IEnumerable<ListCommandUrl>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in wrnjl
		return this.wrnjl();
	}
}
