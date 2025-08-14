using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public class MessageId : IHeader
{
	private static string ygjhs;

	private readonly string wbbor;

	public string Id => wbbor.ToString();

	public MessageId(string messageId)
		: this(messageId, check: true)
	{
	}

	public MessageId()
	{
		byte[] array = kgbvh.sjmog();
		StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
		int num = 0;
		if (num != 0)
		{
			goto IL_001c;
		}
		goto IL_0033;
		IL_001c:
		stringBuilder.dlvlk("{0:x2}", array[num]);
		num++;
		goto IL_0033;
		IL_0033:
		if (num >= array.Length)
		{
			if (ygjhs == null || 1 == 0)
			{
				ygjhs = Dns.GetHostName();
				if (ygjhs == null || false || ygjhs.Length == 0 || 1 == 0)
				{
					ygjhs = "@localhost";
				}
				else
				{
					ygjhs = "@" + ygjhs.ToLower(CultureInfo.InvariantCulture);
				}
			}
			stringBuilder.Append(ygjhs);
			wbbor = stringBuilder.ToString();
			return;
		}
		goto IL_001c;
	}

	public static implicit operator MessageId(string messageId)
	{
		return new MessageId(messageId, check: true);
	}

	private MessageId(string messageId, bool check)
	{
		if (check && 0 == 0)
		{
			if (messageId == null || 1 == 0)
			{
				throw new ArgumentNullException("messageId");
			}
			if (brgjd.qwnqu(messageId) && 0 == 0)
			{
				throw new ArgumentException("Message ID is empty.", "messageId");
			}
			mqucj mqucj = mqucj.xdgws(new stzvh(messageId));
			if (mqucj == null || 1 == 0)
			{
				wbbor = "empty@unknown";
			}
			else
			{
				wbbor = mqucj.ToString();
			}
		}
		else
		{
			wbbor = messageId;
		}
	}

	public IHeader Clone()
	{
		return new MessageId(wbbor, check: false);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('<');
		stringBuilder.Append(wbbor);
		stringBuilder.Append('>');
		return stringBuilder.ToString();
	}

	public override bool Equals(object obj)
	{
		if (obj == null || 1 == 0)
		{
			return false;
		}
		if (!(obj is MessageId messageId) || 1 == 0)
		{
			return false;
		}
		return wbbor == messageId.wbbor;
	}

	public override int GetHashCode()
	{
		return wbbor.GetHashCode();
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write('<' + wbbor + '>');
	}

	internal static MessageId nkxgc(string p0)
	{
		return (MessageId)sarvi(new stzvh(p0));
	}

	internal static IHeader sarvi(stzvh p0)
	{
		return hxxov(p0, p1: true);
	}

	internal static IHeader hxxov(stzvh p0, bool p1)
	{
		p0.hdpha();
		StringBuilder stringBuilder = new StringBuilder();
		while (!p0.zsywy)
		{
			char c = p0.pfdcf();
			if (c != ' ' || p1)
			{
				switch (c)
				{
				case '<':
					p1 = true;
					stringBuilder.Length = 0;
					continue;
				default:
					stringBuilder.Append(c);
					continue;
				case '>':
					break;
				}
			}
			break;
		}
		return new MessageId(stringBuilder.ToString(), check: false);
	}
}
