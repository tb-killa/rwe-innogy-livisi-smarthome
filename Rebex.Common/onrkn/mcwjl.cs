using System;
using System.Linq;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class mcwjl : CryptographicCollection<zyked>, lnabj
{
	private sealed class qzcor
	{
		public string ukorg;

		public bool sjivh(zyked p0)
		{
			return p0.Oid == ukorg;
		}
	}

	public zyked this[string oid]
	{
		get
		{
			qzcor qzcor = new qzcor();
			qzcor.ukorg = oid;
			if (qzcor.ukorg == null || 1 == 0)
			{
				throw new ArgumentNullException("oid");
			}
			return base.lquvo.FirstOrDefault(qzcor.sjivh);
		}
	}

	internal mcwjl()
		: base(rmkkr.osptv)
	{
	}

	internal static mcwjl mfjid(params zyked[] p0)
	{
		mcwjl mcwjl2 = new mcwjl();
		int num;
		if (p0 != null && 0 == 0)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0018;
			}
			goto IL_0027;
		}
		goto IL_002d;
		IL_0027:
		if (num < p0.Length)
		{
			goto IL_0018;
		}
		goto IL_002d;
		IL_002d:
		mcwjl2.hksnh();
		return mcwjl2;
		IL_0018:
		zyked item = p0[num];
		mcwjl2.Add(item);
		num++;
		goto IL_0027;
	}

	private lnabj zfpoy(rmkkr p0, bool p1, int p2)
	{
		zyked zyked2 = new zyked();
		base.lquvo.Add(zyked2);
		return zyked2;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zfpoy
		return this.zfpoy(p0, p1, p2);
	}
}
