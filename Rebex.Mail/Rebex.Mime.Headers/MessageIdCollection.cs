using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public sealed class MessageIdCollection : HeaderValueCollection, IEnumerable<MessageId>, IEnumerable
{
	internal override Type sxlev => typeof(MessageId);

	public new MessageId this[int index]
	{
		get
		{
			return (MessageId)base[index];
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

	public MessageIdCollection()
	{
	}

	public static implicit operator MessageIdCollection(MessageId messageId)
	{
		if (messageId == null || 1 == 0)
		{
			throw new ArgumentNullException("messageId");
		}
		MessageIdCollection messageIdCollection = new MessageIdCollection();
		messageIdCollection.Add(messageId);
		return messageIdCollection;
	}

	public static implicit operator MessageIdCollection(string messageIDs)
	{
		if (messageIDs == null || 1 == 0)
		{
			throw new ArgumentNullException("messageIDs");
		}
		return new MessageIdCollection(new stzvh(messageIDs));
	}

	public int Add(MessageId messageId)
	{
		if (messageId == null || 1 == 0)
		{
			throw new ArgumentNullException("messageId");
		}
		return kkdtb(messageId);
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
				MessageId messageId = (MessageId)enumerator.Current;
				if (!flag || 1 == 0)
				{
					stringBuilder.Append(' ');
				}
				flag = false;
				stringBuilder.Append(messageId.ToString());
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
				MessageId messageId = (MessageId)enumerator.Current;
				if (!flag || 1 == 0)
				{
					rllhn.btprl(writer);
				}
				flag = false;
				messageId.Encode(writer);
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

	private MessageIdCollection(stzvh reader)
		: this()
	{
		while (true)
		{
			reader.hdpha(',', ';');
			if (reader.zsywy && 0 == 0)
			{
				break;
			}
			Add((MessageId)MessageId.hxxov(reader, p1: false));
		}
	}

	internal static IHeader iogdx(stzvh p0)
	{
		return new MessageIdCollection(p0);
	}

	public void CopyTo(MessageId[] array, int index)
	{
		CopyTo((Array)array, index);
	}

	private IEnumerator<MessageId> ckauk()
	{
		return this.eaqmu<MessageId>().GetEnumerator();
	}

	IEnumerator<MessageId> IEnumerable<MessageId>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ckauk
		return this.ckauk();
	}
}
