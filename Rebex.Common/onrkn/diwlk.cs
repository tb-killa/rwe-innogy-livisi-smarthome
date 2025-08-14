using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Rebex;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class diwlk
{
	public const string epwhl = "Digest-MD5 credentials cannot be fully encoded using Latin-1 and the server doesn't support any other encoding.";

	public const string gupgc = "Server offers no recognizable Digest-MD5 quality-of-protection value.";

	private string wxdnj = "";

	private Dictionary<string, string> pdqwg;

	private string smvzy = "";

	private string chaxs = "";

	private string jykpp = "";

	private readonly HashingAlgorithm bdydm;

	private readonly bool dvjev;

	public diwlk(string data, string service)
		: this(data, service, base64: true)
	{
	}

	public diwlk(string data, string service, bool base64)
	{
		dvjev = base64;
		if (base64 && 0 == 0)
		{
			byte[] p = Convert.FromBase64String(data);
			wxdnj = jmxsv(p);
		}
		else
		{
			wxdnj = data;
		}
		pdqwg = new Dictionary<string, string>();
		asfpz();
		smvzy = service;
		bdydm = new HashingAlgorithm(HashingAlgorithmId.MD5);
		byte[] array = new byte[8];
		CryptoHelper.CreateRandomNumberGenerator().GetBytes(array);
		chaxs = BitConverter.ToString(array).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
	}

	public string llgzn(string p0, string p1)
	{
		return vhedm(p0, p1, "AUTHENTICATE", null, "digest-uri", p5: false);
	}

	public string vhedm(string p0, string p1, string p2, string p3, string p4, bool p5)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = autzd("charset");
		string text2 = autzd("authzid");
		string text3 = autzd("realm");
		string text4 = autzd("nonce");
		string text5 = chaxs;
		string text6 = ((p5 ? true : false) ? "auth" : jnidm());
		string text7 = ((p3 != null) ? p3 : (smvzy + "/" + text3));
		string text8 = "00000001";
		string text9 = autzd("opaque");
		string text10 = autzd("algorithm");
		string text11 = qoviv(p0, p1, text6, p2, text7, p5);
		if (text.Length > 0)
		{
			stringBuilder.Append("charset=" + text + ", ");
		}
		stringBuilder.Append("username=\"" + p0 + "\"");
		if (text3.Length > 0)
		{
			stringBuilder.Append(", realm=\"" + text3 + "\"");
		}
		if (text4.Length > 0)
		{
			stringBuilder.Append(", nonce=\"" + text4 + "\"");
		}
		if (text7.Length > 0)
		{
			stringBuilder.Append(", " + p4 + "=\"" + text7 + "\"");
		}
		if (text10.Length > 0)
		{
			stringBuilder.Append(", algorithm=\"" + text10 + "\"");
		}
		stringBuilder.Append(", response=\"" + text11 + "\"");
		if (text6.Length > 0)
		{
			stringBuilder.Append(", qop=\"" + text6 + "\"");
		}
		if (text8.Length > 0)
		{
			stringBuilder.Append(", nc=" + text8);
		}
		if (text5.Length > 0)
		{
			stringBuilder.Append(", cnonce=\"" + text5 + "\"");
		}
		if (text2.Length > 0)
		{
			stringBuilder.Append(", authzid=\"" + text2 + "\"");
		}
		if (text9.Length > 0)
		{
			stringBuilder.Append(", opaque=\"" + text9 + "\"");
		}
		if (dvjev && 0 == 0)
		{
			return hneqo(stringBuilder.ToString());
		}
		return stringBuilder.ToString();
	}

	internal string jnidm()
	{
		string result = "auth";
		string text = autzd("qop");
		if (text.Length == 0 || 1 == 0)
		{
			return result;
		}
		char[] separator = new char[5] { ' ', ',', '\t', '\r', '\n' };
		string[] array = text.Split(separator);
		string[] array2 = new string[2] { "auth-int", "auth" };
		int num = 0;
		if (num != 0)
		{
			goto IL_0067;
		}
		goto IL_0083;
		IL_0083:
		if (num >= array2.Length)
		{
			goto IL_0095;
		}
		goto IL_0067;
		IL_0095:
		return result;
		IL_0067:
		if (Array.IndexOf(array, array2[num]) >= 0)
		{
			result = array2[num];
			goto IL_0095;
		}
		num++;
		goto IL_0083;
	}

	private string hneqo(string p0)
	{
		string text = autzd("charset");
		string name = ((text.Length == 0) ? "iso-8859-1" : "utf-8");
		Encoding encoding = EncodingTools.GetEncoding(name);
		byte[] bytes = encoding.GetBytes(p0);
		return Convert.ToBase64String(bytes, 0, bytes.Length);
	}

	private static string bkllm(byte[] p0)
	{
		return BitConverter.ToString(p0).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
	}

	private string qoviv(string p0, string p1, string p2, string p3, string p4, bool p5)
	{
		string text = autzd("authzid");
		string text2 = autzd("realm");
		string text3 = autzd("nonce");
		string text4 = chaxs;
		string text5 = "00000001";
		string strA = autzd("algorithm");
		p0 = vziqr(p0);
		p1 = vziqr(p1);
		string p6 = p0 + ":" + text2 + ":" + p1;
		byte[] p7 = bdydm.ComputeHash(xzxgk(p6));
		if (string.Compare(strA, "MD5-sess", StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
		{
			p6 = ((!p5) ? (jmxsv(p7) + ":" + text3 + ":" + text4) : (bkllm(p7) + ":" + text3 + ":" + text4));
			if (text.Length > 0)
			{
				p6 = p6 + ":" + text;
			}
			p7 = bdydm.ComputeHash(xzxgk(p6));
		}
		p6 = bkllm(p7);
		string text6 = null;
		string text7 = null;
		string text8;
		if ((text8 = p2.ToLower(CultureInfo.InvariantCulture)) == null || false || text8 == "auth" || (!(text8 == "auth-int") && !(text8 == "auth-conf")))
		{
			text6 = p3 + ":" + p4;
			text7 = ":" + p4;
		}
		else
		{
			text6 = p3 + ":" + p4 + ":00000000000000000000000000000000";
			text7 = ":" + p4 + ":00000000000000000000000000000000";
		}
		p7 = bdydm.ComputeHash(xzxgk(text6));
		text6 = bkllm(p7);
		p7 = bdydm.ComputeHash(xzxgk(text7));
		text7 = bkllm(p7);
		string p8 = p6 + ":" + text3 + ":" + text5 + ":" + text4 + ":" + p2 + ":" + text6;
		p7 = bdydm.ComputeHash(xzxgk(p8));
		p8 = bkllm(p7);
		jykpp = p6 + ":" + text3 + ":" + text5 + ":" + text4 + ":" + p2 + ":" + text7;
		p7 = bdydm.ComputeHash(xzxgk(jykpp));
		jykpp = bkllm(p7);
		return p8;
	}

	private string vziqr(string p0)
	{
		Encoding encoding = EncodingTools.GetEncoding("iso-8859-1");
		byte[] bytes = encoding.GetBytes(p0);
		string text = encoding.GetString(bytes, 0, bytes.Length);
		if (text == p0 && 0 == 0)
		{
			return jmxsv(bytes);
		}
		string text2 = autzd("charset");
		if (text2.Length > 0)
		{
			return p0;
		}
		throw new CryptographicException("Digest-MD5 credentials cannot be fully encoded using Latin-1 and the server doesn't support any other encoding.");
	}

	public bool nxuzu(string p0)
	{
		byte[] p1 = Convert.FromBase64String(p0);
		string text = jmxsv(p1);
		int num = text.IndexOf("rspauth");
		if (num < 0)
		{
			return false;
		}
		num = text.IndexOf("=", num + 7);
		if (num < 0)
		{
			return false;
		}
		string text2 = text.Substring(num + 1);
		return text2 == jykpp;
	}

	internal string autzd(string p0)
	{
		p0 = p0.ToUpper(CultureInfo.InvariantCulture);
		if (pdqwg.ContainsKey(p0) && 0 == 0)
		{
			return pdqwg[p0];
		}
		return "";
	}

	private void asfpz()
	{
		int num = 0;
		do
		{
			int num2 = wxdnj.IndexOf('=', num);
			if (num2 <= num)
			{
				break;
			}
			string text = wxdnj.Substring(num, num2 - num).Trim();
			if (num2 + 1 >= wxdnj.Length)
			{
				break;
			}
			int num3;
			string p;
			if (wxdnj[num2 + 1] == '"')
			{
				num3 = xdtvv(wxdnj, num2 + 1, out p);
				if (num3 < 0)
				{
					break;
				}
			}
			else
			{
				int num4 = num2 + 1;
				num3 = wxdnj.IndexOf(",", num4);
				if (num3 < 0)
				{
					num3 = wxdnj.Length;
				}
				p = wxdnj.Substring(num4, num3 - num4).Trim();
			}
			pdqwg.Add(text.ToUpper(CultureInfo.InvariantCulture), p);
			num = wxdnj.IndexOf(",", num3) + 1;
		}
		while (num > 0);
	}

	private int xdtvv(string p0, int p1, out string p2)
	{
		p2 = null;
		if (p0[p1] != '"')
		{
			throw new InvalidOperationException(brgjd.edcru("Character at index {0} is not quote mark ('\"').", p1));
		}
		int num = p1 + 1;
		if (num >= p0.Length)
		{
			return -1;
		}
		int num2 = p0.IndexOf('"', num);
		if (num2 < 0)
		{
			return -1;
		}
		while (wxdnj[num2 - 1] == '\\')
		{
			if (num2 + 1 >= wxdnj.Length)
			{
				return -1;
			}
			num2 = wxdnj.IndexOf('"', num2 + 1);
			if (num2 < 0)
			{
				return -1;
			}
		}
		p2 = wxdnj.Substring(num, num2 - num);
		p2 = qxdbo(p2);
		return num2;
	}

	private string qxdbo(string p0)
	{
		int num = p0.IndexOf('\\');
		if (num >= 0)
		{
			int num2 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (num >= 0)
			{
				stringBuilder.Append(p0.Substring(num2, num - num2));
				num2 = num + 1;
				num = ((num2 + 1 >= p0.Length) ? (-1) : p0.IndexOf('\\', num2 + 1));
			}
			p0 = stringBuilder.ToString();
		}
		return p0;
	}

	private static byte[] xzxgk(string p0)
	{
		byte[] array = new byte[p0.Length];
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_0021;
		IL_0012:
		array[num] = (byte)p0[num];
		num++;
		goto IL_0021;
		IL_0021:
		if (num < p0.Length)
		{
			goto IL_0012;
		}
		return array;
	}

	private static string jmxsv(byte[] p0)
	{
		char[] array = new char[p0.Length];
		int num = 0;
		if (num != 0)
		{
			goto IL_000f;
		}
		goto IL_0019;
		IL_000f:
		array[num] = (char)p0[num];
		num++;
		goto IL_0019;
		IL_0019:
		if (num < p0.Length)
		{
			goto IL_000f;
		}
		return new string(array);
	}
}
