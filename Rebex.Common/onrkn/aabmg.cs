using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;

namespace onrkn;

internal class aabmg
{
	private static readonly IDictionary<Type, aabmg> iecdq = new Dictionary<Type, aabmg>();

	public readonly Type jznen;

	public readonly MethodBase dfrqf;

	public readonly MethodBase xlgms;

	public readonly MethodBase eytuw;

	public readonly MethodBase aayhb;

	public readonly MethodBase heyqp;

	public readonly MethodBase euota;

	public readonly MethodBase mrlet;

	public readonly MethodBase nfizw;

	public readonly MethodBase lqlbg;

	public readonly MethodBase gwhjg;

	public readonly MethodBase sjnnz;

	public readonly MethodBase xqyie;

	public readonly bool fjnth;

	public readonly bool nggrf;

	public aabmg(Type type)
	{
		jznen = type;
		dfrqf = ejjgm(type, "Name", typeof(string), p3: false);
		xlgms = poqqs(type, "FromPrivateKey", p2: true);
		eytuw = poqqs(type, "FromSeed", p2: false);
		aayhb = poqqs(type, "FromPublicKey", p2: true);
		heyqp = poqqs(type, "GetPrivateKey", p2: true);
		euota = poqqs(type, "GetPublicKey", p2: true);
		mrlet = poqqs(type, "SignHash", p2: false);
		nfizw = poqqs(type, "VerifyHash", p2: false);
		lqlbg = poqqs(type, "SignMessage", p2: false);
		gwhjg = poqqs(type, "VerifyMessage", p2: false);
		sjnnz = poqqs(type, "GetSharedSecret", p2: false);
		xqyie = poqqs(type, "Dispose", p2: false);
		fjnth = (object)lqlbg != null && 0 == 0 && (object)gwhjg != null;
		nggrf = (object)sjnnz != null;
	}

	public static aabmg grzle(Type p0)
	{
		lock (iecdq)
		{
			if (iecdq.TryGetValue(p0, out var value) && 0 == 0)
			{
				return value;
			}
			return iecdq[p0] = new aabmg(p0);
		}
	}

	private static MethodBase poqqs(Type p0, string p1, bool p2)
	{
		MethodInfo method = p0.GetMethod(p1);
		if (((object)method == null || 1 == 0) && p2 && 0 == 0)
		{
			throw new NotSupportedException("Supplied type does not provide a suitable API.");
		}
		return method;
	}

	private static MethodBase cfkos(Type p0)
	{
		ConstructorInfo constructorInfo = p0.hlbto();
		if ((object)constructorInfo == null || 1 == 0)
		{
			throw new NotSupportedException("Supplied type does not provide a suitable API.");
		}
		return constructorInfo;
	}

	private static MethodBase ejjgm(Type p0, string p1, Type p2, bool p3)
	{
		PropertyInfo property = p0.GetProperty(p1);
		if ((object)property == null || false || (object)property.PropertyType != p2)
		{
			if (p3 && 0 == 0)
			{
				throw new NotSupportedException("Supplied type does not provide a suitable API.");
			}
			return null;
		}
		return property.GetGetMethod();
	}

	public object njjty(object p0, MethodBase p1, params object[] p2)
	{
		try
		{
			if ((p0 == null || 1 == 0) && p1 is ConstructorInfo constructorInfo && 0 == 0)
			{
				return constructorInfo.Invoke(p2);
			}
			return p1.Invoke(p0, p2);
		}
		catch (TargetInvocationException ex)
		{
			Exception innerException = ex.InnerException;
			if (innerException == null || 1 == 0)
			{
				throw new CryptographicException("Unable to invoke algorithm.", ex);
			}
			throw new CryptographicException(innerException.Message, innerException);
		}
	}
}
