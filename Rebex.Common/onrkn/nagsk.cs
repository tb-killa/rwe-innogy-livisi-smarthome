using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace onrkn;

[Serializable]
internal class nagsk : Exception
{
	private readonly ReadOnlyCollection<Exception> jtiku;

	private static Func<Exception, bool> dkojz;

	public ReadOnlyCollection<Exception> mfkfw => jtiku;

	public nagsk()
		: this(null, null, new Exception[0])
	{
	}

	public nagsk(string message, Exception innerException)
		: this(message, (innerException != null) ? new Exception[1] { innerException } : new Exception[0])
	{
	}

	public nagsk(string message, params Exception[] exceptions)
		: this(message, lmazh(exceptions), exceptions)
	{
		jtiku = new ReadOnlyCollection<Exception>(exceptions);
	}

	public nagsk(params Exception[] exceptions)
		: this(null, lmazh(exceptions), exceptions)
	{
	}

	public nagsk(IEnumerable<Exception> exceptions)
		: this(null, lmazh(exceptions), exceptions)
	{
	}

	public override Exception GetBaseException()
	{
		nagsk nagsk2 = this;
		Exception ex = nagsk2;
		while (nagsk2 != null && 0 == 0 && nagsk2.jtiku.Count == 1)
		{
			ex = nagsk2.InnerException;
			nagsk2 = ex as nagsk;
		}
		return ex;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		IEnumerator<Exception> enumerator = jtiku.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Exception current = enumerator.Current;
				stringBuilder.fazck("{0}---> (Inner Exception #{1}) {2}<--- \r\n", dahxy.nxwxy, num, current);
				num++;
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return stringBuilder.ToString();
	}

	private nagsk(string message, Exception innerException, IEnumerable<Exception> exceptions)
	{
		object obj = message;
		if (obj == null || 1 == 0)
		{
			obj = "One or more errors occurred.";
		}
		base._002Ector((string)obj, innerException);
		jtiku = new ReadOnlyCollection<Exception>(exceptions.ToArray());
	}

	private static Exception lmazh(IEnumerable<Exception> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("exceptions");
		}
		if (dkojz == null || 1 == 0)
		{
			dkojz = bckxz;
		}
		if (p0.Any(dkojz) && 0 == 0)
		{
			throw new ArgumentException("The collection contains one or more null value items.", "exceptions");
		}
		return p0.FirstOrDefault();
	}

	private static bool bckxz(Exception p0)
	{
		return p0 == null;
	}
}
