using System;
using System.Collections;
using System.Collections.Generic;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public class TlsSession
{
	internal static long ketlr = 1800000L;

	internal static long normv = 300000L;

	internal static long ewyum = 86400000L;

	internal static long bychy = 60000L;

	internal static int vtpbb = 100;

	private readonly string bmuwc;

	private long khoai;

	private readonly long yueql;

	private static Hashtable lmyoh = new Hashtable();

	private static long janjr = dahxy.ntngc;

	private readonly byte[] srajj;

	private readonly byte[] kanwm;

	private readonly CertificateChain hduhj;

	private readonly CertificateChain gtpzp;

	private readonly bool ipskn;

	private bool opznt;

	internal string rivvr => bmuwc;

	internal static object ykwqv => lmyoh.SyncRoot;

	internal byte[] wqpxu => srajj;

	internal byte[] tydvs => kanwm;

	internal CertificateChain xalxj => hduhj;

	internal CertificateChain twwdt => gtpzp;

	internal bool zqpiy => ipskn;

	internal bool cteqc => tydvs == null;

	public override string ToString()
	{
		return bmuwc;
	}

	internal void lmipo()
	{
		opznt = true;
		khoai = dahxy.ntngc;
	}

	internal static void mcihc(string p0, TlsSession p1)
	{
		lock (ykwqv)
		{
			if (p1 == null || 1 == 0)
			{
				if (!lmyoh.ContainsKey(p0) || 1 == 0)
				{
					lmyoh.Add(p0, new TlsSession(p0, null, null, null, null, extendedMasterSecretEnabled: false));
				}
				return;
			}
			if (lmyoh[p0] is TlsSession tlsSession && 0 == 0 && (!tlsSession.cteqc || 1 == 0))
			{
				throw new ArgumentException("This session was already added into session cache.");
			}
			lmyoh[p0] = p1;
		}
	}

	internal static TlsSession relpg(string p0)
	{
		TlsSession tlsSession;
		lock (ykwqv)
		{
			tlsSession = lmyoh[p0] as TlsSession;
		}
		if (tlsSession != null && 0 == 0)
		{
			if ((tlsSession.cteqc ? true : false) || ilwro(tlsSession, dahxy.ntngc))
			{
				return null;
			}
			tlsSession.lmipo();
		}
		return tlsSession;
	}

	internal static string pwkvr(byte[] p0)
	{
		if (p0 == null || false || p0.Length == 0 || 1 == 0)
		{
			return null;
		}
		return Convert.ToBase64String(p0, 0, p0.Length).TrimEnd('=');
	}

	internal static void atnfa()
	{
		lock (ykwqv)
		{
			long ntngc = dahxy.ntngc;
			if (ntngc - janjr < bychy || lmyoh.Count < vtpbb)
			{
				return;
			}
			List<string> list = new List<string>();
			IEnumerator enumerator = lmyoh.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					object current = enumerator.Current;
					TlsSession tlsSession = (TlsSession)current;
					if (ilwro(tlsSession, ntngc) && 0 == 0)
					{
						list.Add(tlsSession.rivvr);
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
			using (List<string>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext() ? true : false)
				{
					string current2 = enumerator2.Current;
					lmyoh.Remove(current2);
				}
			}
			janjr = ntngc;
		}
	}

	private static bool ilwro(TlsSession p0, long p1)
	{
		if (!p0.opznt || 1 == 0)
		{
			if (p1 - p0.yueql > normv)
			{
				return true;
			}
		}
		else if (p1 - p0.khoai > ketlr)
		{
			return true;
		}
		if (p1 - p0.yueql > ewyum)
		{
			return true;
		}
		return false;
	}

	internal TlsSession(byte[] ID, byte[] masterSecret, CertificateChain serverCertificateChain, CertificateChain clientCertificateChain, bool extendedMasterSecretEnabled)
		: this(pwkvr(ID), ID, masterSecret, serverCertificateChain, clientCertificateChain, extendedMasterSecretEnabled)
	{
	}

	private TlsSession(string id, byte[] ID, byte[] masterSecret, CertificateChain serverCertificateChain, CertificateChain clientCertificateChain, bool extendedMasterSecretEnabled)
	{
		bmuwc = id;
		khoai = (yueql = dahxy.ntngc);
		srajj = ID;
		kanwm = masterSecret;
		gtpzp = serverCertificateChain;
		hduhj = clientCertificateChain;
		ipskn = extendedMasterSecretEnabled;
	}
}
