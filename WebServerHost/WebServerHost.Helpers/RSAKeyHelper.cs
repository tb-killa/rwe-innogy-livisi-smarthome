using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Helpers;

internal class RSAKeyHelper
{
	public static void SaveToFile(string filePath, RSAParameters key)
	{
		using StreamWriter streamWriter = new StreamWriter(filePath);
		string s = key.ToJson();
		string value = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		streamWriter.Write(value);
		streamWriter.Flush();
		streamWriter.Close();
	}

	public static RSAParameters LoadFromFile(string filePath)
	{
		string s = string.Empty;
		using (StreamReader streamReader = new StreamReader(filePath))
		{
			s = streamReader.ReadToEnd();
		}
		byte[] array = Convert.FromBase64String(s);
		string json = Encoding.UTF8.GetString(array, 0, array.Length);
		return json.FromJson<RSAParameters>();
	}
}
