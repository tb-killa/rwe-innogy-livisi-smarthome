using System;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class SecureMimeCapabilityCollection : CryptographicCollection<SecureMimeCapability>, lnabj
{
	public new SecureMimeCapability this[int index]
	{
		get
		{
			return base[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			base[index] = value;
		}
	}

	public SecureMimeCapability this[string oid]
	{
		get
		{
			if (oid == null || 1 == 0)
			{
				throw new ArgumentNullException("oid");
			}
			IEnumerator<SecureMimeCapability> enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					SecureMimeCapability current = enumerator.Current;
					if (current.Oid.Value == oid && 0 == 0)
					{
						return current;
					}
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			return null;
		}
	}

	internal SecureMimeCapabilityCollection()
		: base(rmkkr.osptv)
	{
	}

	private lnabj pmdzz(rmkkr p0, bool p1, int p2)
	{
		SecureMimeCapability secureMimeCapability = new SecureMimeCapability();
		base.lquvo.Add(secureMimeCapability);
		return secureMimeCapability;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in pmdzz
		return this.pmdzz(p0, p1, p2);
	}
}
