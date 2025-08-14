using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

public class ApplicationsToken
{
	public string Hash { get; set; }

	public List<ApplicationTokenEntry> Entries { get; set; }

	public long ShcType { get; set; }

	public ApplicationsToken Clone()
	{
		ApplicationsToken applicationsToken = new ApplicationsToken();
		applicationsToken.Entries = new List<ApplicationTokenEntry>();
		applicationsToken.ShcType = ShcType;
		applicationsToken.Hash = Hash;
		ApplicationsToken applicationsToken2 = applicationsToken;
		if (Entries != null)
		{
			foreach (ApplicationTokenEntry entry in Entries)
			{
				applicationsToken2.Entries.Add(entry.Clone());
			}
		}
		return applicationsToken2;
	}

	public string GetHash()
	{
		byte[] array;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			if (Entries != null)
			{
				foreach (ApplicationTokenEntry entry in Entries)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(entry.ToString());
					memoryStream.Write(bytes, 0, bytes.Length);
				}
			}
			memoryStream.Write(BitConverter.GetBytes(ShcType), 0, 8);
			array = memoryStream.ToArray();
		}
		if (array.Length > 0)
		{
			using (MD5 mD = MD5.Create())
			{
				byte[] array2 = mD.ComputeHash(array);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array2.Length; i++)
				{
					stringBuilder.Append(array2[i].ToString("x2"));
				}
				return stringBuilder.ToString();
			}
		}
		return string.Empty;
	}
}
