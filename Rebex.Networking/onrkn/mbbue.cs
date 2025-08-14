using System;
using System.Net.Sockets;
using Rebex.Net;

namespace onrkn;

internal static class mbbue
{
	internal struct lutcu
	{
		[lztdu]
		private bool vdznf;

		[lztdu]
		private apajk<Socket> qeoyz;

		public bool gafhq => vdznf;

		public apajk<Socket> kqgyp => qeoyz;

		public lutcu(bool isKnownSocketOwnerImplementor, apajk<Socket> socket)
		{
			this = default(lutcu);
			vdznf = isKnownSocketOwnerImplementor;
			qeoyz = socket;
		}
	}

	public static lutcu ygaej(this ISocket p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		if (p0 is vrloh vrloh2 && 0 == 0)
		{
			return new lutcu(isKnownSocketOwnerImplementor: true, vrloh2.wbidm);
		}
		return new lutcu(isKnownSocketOwnerImplementor: false, vvchs.apcdm<Socket>());
	}
}
