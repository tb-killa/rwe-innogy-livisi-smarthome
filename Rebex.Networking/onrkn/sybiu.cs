using System;

namespace onrkn;

internal class sybiu : mkuxt
{
	private xbrcx pkqez;

	private uint oyuiq;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, (byte)pkqez);
		mkuxt.ebmel(p0, oyuiq);
	}

	public sybiu(xbrcx packet, uint recipientChannel)
	{
		switch (packet)
		{
		default:
			throw new NotSupportedException();
		case xbrcx.punhv:
		case xbrcx.nvcjs:
		case xbrcx.evspj:
		case xbrcx.xgswk:
		case xbrcx.vqsre:
			pkqez = packet;
			oyuiq = recipientChannel;
			break;
		}
	}
}
