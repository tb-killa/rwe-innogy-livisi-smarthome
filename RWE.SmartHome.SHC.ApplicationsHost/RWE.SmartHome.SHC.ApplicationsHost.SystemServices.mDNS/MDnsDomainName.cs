using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsDomainName
{
	private string[] names;

	public string Name { get; private set; }

	public MDnsDomainName(IEnumerable<string> labels)
	{
		InitializeDomainName(labels);
	}

	public MDnsDomainName(string domainName)
	{
		InitializeDomainName(from s in domainName.Split('.')
			where !string.IsNullOrEmpty(s)
			select s);
	}

	public byte[] ToByteArray()
	{
		int num = names.Sum((string s) => s.Length + 1) + 1;
		byte[] array = new byte[num];
		int num2 = 0;
		string[] array2 = names;
		foreach (string text in array2)
		{
			if (text.Length == 0)
			{
				break;
			}
			array[num2] = (byte)text.Length;
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			Array.Copy(bytes, 0, array, num2 + 1, bytes.Length);
			num2 += text.Length + 1;
		}
		array[array.Length - 1] = 0;
		return array;
	}

	public override string ToString()
	{
		return Name;
	}

	private void InitializeDomainName(IEnumerable<string> labels)
	{
		names = labels.ToArray();
		Name = string.Join(".", names) + (names.Any() ? "." : "");
	}
}
