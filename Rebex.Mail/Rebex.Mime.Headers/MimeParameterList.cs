using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public class MimeParameterList
{
	private class hgcqo : IComparer<string>
	{
		private static readonly string[] iacge = new string[3] { "method", "type", "name" };

		public int Compare(string a, string b)
		{
			if (a == null || false || b == null)
			{
				return 0;
			}
			int num = string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
			if (num == 0 || 1 == 0)
			{
				return 0;
			}
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0036;
			}
			goto IL_006e;
			IL_0036:
			if (string.Compare(a, iacge[num2], StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
			{
				return -1;
			}
			if (string.Compare(b, iacge[num2], StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
			{
				return 1;
			}
			num2++;
			goto IL_006e;
			IL_006e:
			if (num2 < iacge.Length)
			{
				goto IL_0036;
			}
			return num;
		}
	}

	private readonly Dictionary<string, string> smeog;

	private long dfuyd;

	private bool vsdes;

	internal bool cvjtw
	{
		get
		{
			return vsdes;
		}
		set
		{
			vsdes = value;
		}
	}

	internal long xbrdn => dfuyd;

	public int Count => smeog.Count;

	public string this[string name]
	{
		get
		{
			if (name == null || 1 == 0)
			{
				throw new ArgumentNullException("name");
			}
			smeog.TryGetValue(name, out var value);
			return value;
		}
		set
		{
			if (name == null || 1 == 0)
			{
				throw new ArgumentNullException("name");
			}
			kgbvh.hdlkr(name, "name", p2: false);
			if (vsdes && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			if (value == null || 1 == 0)
			{
				if (smeog.ContainsKey(name) && 0 == 0)
				{
					smeog.Remove(name);
					dfuyd++;
				}
			}
			else
			{
				kgbvh.zgeyl(value, "value", p2: true);
				smeog[name] = value;
				dfuyd++;
			}
		}
	}

	public ICollection Names => smeog.Keys;

	internal MimeParameterList()
	{
		smeog = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}

	internal MimeParameterCollection mygie()
	{
		MimeParameterCollection mimeParameterCollection = new MimeParameterCollection();
		using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = smeog.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				string current = enumerator.Current;
				mimeParameterCollection.smeog.Add(current, smeog[current]);
			}
		}
		mimeParameterCollection.dfuyd = dfuyd;
		return mimeParameterCollection;
	}

	public void Add(string name, string value)
	{
		if (name == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Parameter name is empty.", "name");
		}
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		kgbvh.hdlkr(name, "name", p2: false);
		if (vsdes && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (smeog.ContainsKey(name) && 0 == 0)
		{
			throw new ArgumentException(brgjd.edcru("Parameter '{0}' already exists.", name), "name");
		}
		kgbvh.zgeyl(value, "value", p2: true);
		smeog[name] = value;
		dfuyd++;
	}

	public void Remove(string name)
	{
		if (name == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Parameter name is empty.", "name");
		}
		if (vsdes && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (smeog.ContainsKey(name) && 0 == 0)
		{
			smeog.Remove(name);
			dfuyd++;
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>(smeog.Keys);
		list.Sort(new hgcqo());
		using (List<string>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				string current = enumerator.Current;
				string text = this[current];
				if (text != null && 0 == 0)
				{
					stringBuilder.mwigd("; {0}=\"{1}\"", current, text);
				}
			}
		}
		return stringBuilder.ToString();
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		List<string> list = new List<string>(smeog.Keys);
		list.Sort(new hgcqo());
		using List<string>.Enumerator enumerator = list.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			string current = enumerator.Current;
			string text = this[current];
			if (text != null && 0 == 0)
			{
				writer.Write(";");
				if (writer is zncis && 0 == 0)
				{
					writer.Write("\r\n\t");
				}
				else
				{
					writer.Write(' ');
				}
				writer.Write(current);
				writer.Write("=");
				ujfpn(writer, text);
			}
		}
	}

	private static void ujfpn(TextWriter p0, string p1)
	{
		if (p1.Length == 0 || 1 == 0)
		{
			p0.Write("\"\"");
			return;
		}
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_0028;
		}
		goto IL_004d;
		IL_0056:
		flag = true;
		if (flag2 && 0 == 0)
		{
			p0.Write("\"");
			rllhn.bvfoi(p0, p1, 108);
			p0.Write("\"");
			return;
		}
		StringBuilder stringBuilder;
		int num2;
		if (flag)
		{
			stringBuilder = new StringBuilder();
			stringBuilder.Append('"');
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_00a7;
			}
			goto IL_00db;
		}
		p0.Write(p1);
		return;
		IL_004d:
		if (num < p1.Length)
		{
			goto IL_0028;
		}
		goto IL_0056;
		IL_0028:
		if (p1[num] == '\0' || false || p1[num] > '~')
		{
			flag2 = true;
			if (flag2)
			{
				goto IL_0056;
			}
		}
		num++;
		goto IL_004d;
		IL_00db:
		if (num2 >= p1.Length)
		{
			stringBuilder.Append('"');
			p0.Write(stringBuilder.ToString());
			return;
		}
		goto IL_00a7;
		IL_00a7:
		char c = p1[num2];
		if (c == '\\' || c == '"')
		{
			stringBuilder.Append('\\');
		}
		stringBuilder.Append(p1[num2]);
		num2++;
		goto IL_00db;
	}

	internal bool abqlt(MimeParameterCollection p0)
	{
		if (p0 == null || 1 == 0)
		{
			return false;
		}
		int count = smeog.Count;
		if (p0.smeog.Count != count)
		{
			return false;
		}
		using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = smeog.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				string current = enumerator.Current;
				if (smeog[current] != p0.smeog[current] && 0 == 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	private static bool falgr(int p0, string p1, out byte[] p2, out int p3)
	{
		int num = 0;
		int length = p1.Length;
		p2 = new byte[length];
		p3 = 0;
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0022;
		}
		goto IL_00a0;
		IL_0022:
		char c = p1[num + num3];
		if (c == '%')
		{
			if (num + num3 + 2 >= p1.Length)
			{
				return false;
			}
			int num4 = kgbvh.bvuap(p1[num + num3 + 1]);
			int num5 = kgbvh.bvuap(p1[num + num3 + 2]);
			if (num4 < 0 || num5 < 0)
			{
				return false;
			}
			p2[num2++] = (byte)(16 * num4 + num5);
			num3 += 3;
		}
		else
		{
			if (c > '~')
			{
				return false;
			}
			p2[num2++] = (byte)c;
			num3++;
		}
		goto IL_00a0;
		IL_00a0:
		if (num3 >= length)
		{
			p3 = num2;
			return true;
		}
		goto IL_0022;
	}

	private void lqkkd(string p0, string p1)
	{
		if (p0 == "smime-type" && 0 == 0)
		{
			p1 = p1.ToLower(CultureInfo.InvariantCulture);
			if (smeog.TryGetValue(p0, out var value) && 0 == 0 && value != p1 && 0 == 0)
			{
				p0 = "x-smime-type";
			}
		}
		smeog[p0] = p1;
	}

	internal void qlttk(stzvh p0)
	{
		string text = null;
		bool flag = false;
		kgbvh.oemou oemou = null;
		Encoding p1 = EncodingTools.ASCII;
		while (!p0.zsywy)
		{
			p0.hdpha(';');
			if (p0.zsywy ? true : false)
			{
				break;
			}
			string text2 = p0.wzlcv();
			if (text2 == null || 1 == 0)
			{
				return;
			}
			p0.hdpha('=');
			if (p0.zsywy && 0 == 0)
			{
				return;
			}
			int num;
			string text3;
			if (p0.havrs == '"')
			{
				num = p0.bauax + 1;
				text3 = p0.oypkh().ToString();
			}
			else
			{
				num = p0.bauax;
				text3 = p0.wzlcv();
				if (text3 == null || 1 == 0)
				{
					return;
				}
				while (!p0.zsywy || 1 == 0)
				{
					char c;
					if (p0.havrs == ':')
					{
						p0.pfdcf();
						c = '_';
						if (c != 0)
						{
							goto IL_0108;
						}
					}
					if (!kgbvh.zhxzu(p0.havrs))
					{
						break;
					}
					p0.bwnkb();
					c = ' ';
					goto IL_0108;
					IL_0108:
					string text4 = p0.wzlcv();
					if (text4 == null)
					{
						break;
					}
					text3 += c;
					text3 += text4;
				}
			}
			bool flag2 = false;
			int num2 = text2.IndexOf('*');
			if (num2 >= 0)
			{
				flag2 = text2.EndsWith("*");
				text2 = text2.Substring(0, num2);
			}
			if (text != null && 0 == 0 && string.Compare(text2, text, StringComparison.OrdinalIgnoreCase) != 0 && 0 == 0)
			{
				if (oemou != null && 0 == 0)
				{
					oemou.clkna();
					string text5 = oemou.ToString();
					if (flag && 0 == 0)
					{
						text5 = MimeHeader.DecodeMimeHeader(text5);
					}
					lqkkd(text, text5);
				}
				oemou = null;
				text = null;
				flag = false;
			}
			if (num2 < 0)
			{
				lqkkd(text2, text3);
				continue;
			}
			text = text2;
			flag = true;
			if (oemou == null || 1 == 0)
			{
				p1 = EncodingTools.ASCII;
				if (flag2 && 0 == 0)
				{
					int num3 = text3.IndexOf('\'');
					if (num3 > 0)
					{
						int num4 = text3.LastIndexOf('\'') + 1;
						string p2 = text3.Substring(0, num3);
						text3 = text3.Substring(num4);
						num += num4;
						Encoding encoding = kgbvh.qirxf(p2);
						if (encoding != null && 0 == 0)
						{
							p1 = encoding;
						}
					}
				}
			}
			if (oemou == null || 1 == 0)
			{
				oemou = new kgbvh.oemou();
			}
			if (flag2 && 0 == 0 && falgr(num, text3, out var p3, out var p4) && 0 == 0)
			{
				oemou.qftve(p1, p3, 0, p4);
			}
			else
			{
				oemou.jdpgz(text3, 0, text3.Length);
			}
		}
		if (text != null && 0 == 0 && oemou != null && 0 == 0)
		{
			oemou.clkna();
			string text6 = oemou.ToString();
			if (flag && 0 == 0)
			{
				text6 = MimeHeader.DecodeMimeHeader(text6);
			}
			lqkkd(text, text6);
		}
	}
}
