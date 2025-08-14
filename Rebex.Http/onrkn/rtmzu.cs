using System;
using System.Collections.Generic;
using Rebex;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class rtmzu
{
	private class ygefi
	{
		private string hlpno;

		private int dsxqk;

		private string fkgul;

		private string rwptf;

		public string bddgh
		{
			get
			{
				return hlpno;
			}
			set
			{
				hlpno = value;
			}
		}

		public int nlbyt
		{
			get
			{
				return dsxqk;
			}
			set
			{
				dsxqk = value;
			}
		}

		public string ggrpc
		{
			get
			{
				return fkgul;
			}
			set
			{
				fkgul = value;
			}
		}

		public string fvvqf
		{
			get
			{
				return rwptf;
			}
			set
			{
				rwptf = value;
			}
		}

		public ygefi(string uri, int age, string authMethod, string authMessage)
		{
			bddgh = uri;
			nlbyt = age;
			ggrpc = authMethod;
			fvvqf = authMessage;
		}
	}

	private const int biuxx = 900000;

	private readonly object tcjid;

	private readonly HttpRequestCreator oddin;

	private readonly bbyvy<string, bbyvy<string, Dictionary<string, ygefi>>> vlhbj;

	public rtmzu(HttpRequestCreator owner)
	{
		oddin = owner;
		tcjid = new object();
		vlhbj = new bbyvy<string, bbyvy<string, Dictionary<string, ygefi>>>(900000);
	}

	public bool wuxuf(string p0, string p1, string p2, int p3, string p4, out string p5, out string p6)
	{
		p5 = null;
		p6 = null;
		lock (tcjid)
		{
			string text = nohpg(p0, p2, p3);
			bbyvy<string, Dictionary<string, ygefi>> bbyvy2 = vlhbj.twxbv(text);
			if (bbyvy2 == null || 1 == 0)
			{
				return false;
			}
			string p7 = Convert.ToString(HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, EncodingTools.UTF8.GetBytes(text + p1)));
			Dictionary<string, ygefi> dictionary = bbyvy2.twxbv(p7);
			if (dictionary == null || 1 == 0)
			{
				return false;
			}
			int tickCount = Environment.TickCount;
			using (Dictionary<string, ygefi>.ValueCollection.Enumerator enumerator = dictionary.Values.GetEnumerator())
			{
				while (enumerator.MoveNext() ? true : false)
				{
					ygefi current = enumerator.Current;
					if (current.nlbyt + 900000 >= tickCount && p4.StartsWith(current.bddgh) && 0 == 0)
					{
						p5 = current.ggrpc;
						p6 = current.fvvqf;
						return true;
					}
				}
			}
			return false;
		}
	}

	public void ptnfh(string p0, string p1, string p2, int p3, string p4, string p5, string p6)
	{
		lock (tcjid)
		{
			string text = nohpg(p0, p2, p3);
			bbyvy<string, Dictionary<string, ygefi>> bbyvy2 = vlhbj.twxbv(text);
			if (bbyvy2 == null || 1 == 0)
			{
				bbyvy2 = vlhbj.wzwsv(text, new bbyvy<string, Dictionary<string, ygefi>>(900000));
			}
			string p7 = Convert.ToString(HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, EncodingTools.UTF8.GetBytes(text + p1)));
			Dictionary<string, ygefi> dictionary = bbyvy2.twxbv(p7);
			if (dictionary == null || 1 == 0)
			{
				dictionary = bbyvy2.wzwsv(p7, new Dictionary<string, ygefi>());
			}
			string text2 = usrni(p4);
			dictionary[text2] = new ygefi(text2, Environment.TickCount, p5, p6);
			oddin.tdcir(LogLevel.Debug, "HTTP", "User '{0}' pre-authenticated for '{1}:{2}{3}' (method={4}).", p0, p2, p3, text2, p5);
		}
	}

	private static string nohpg(string p0, string p1, int p2)
	{
		return brgjd.edcru("{0}:{1}:{2}", p1, p2, p0);
	}

	private static string usrni(string p0)
	{
		if (p0.Length == 0 || false || p0 == "/")
		{
			return p0;
		}
		int num = p0.LastIndexOf('/');
		if (num < 0)
		{
			return string.Empty;
		}
		return p0.Substring(0, num + 1);
	}

	public void bpzkr()
	{
		try
		{
			lock (tcjid)
			{
				vlhbj.xpgwc();
			}
		}
		catch (Exception ex)
		{
			oddin.tdcir(LogLevel.Error, "HTTP", "Exception occurred while discarding HTTP session cache. {0}", ex);
		}
	}
}
