using System;
using Rebex.Net;

namespace onrkn;

internal class euwhd : ofuit
{
	private byte[] ftpbr;

	public override int nimwj => 4 + ftpbr.Length;

	public byte[] lkwbh => ftpbr;

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		ftpbr.CopyTo(p0, p1 + 4);
	}

	public euwhd(byte[] buffer, int offset, int length)
		: base(nsvut.oaufz)
	{
		if (length < 5)
		{
			throw new TlsException(mjddr.gkkle, "Invalid Finished verify data.");
		}
		ftpbr = new byte[length - 4];
		Array.Copy(buffer, offset + 4, ftpbr, 0, length - 4);
	}

	public euwhd(byte[] verifyData)
		: base(nsvut.oaufz)
	{
		ftpbr = verifyData;
	}
}
