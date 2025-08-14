namespace onrkn;

internal static class ikpkg
{
	private static ivdov svzkb(nbmcv p0)
	{
		return p0 switch
		{
			nbmcv.nntyy => ivdov.yipfs | ivdov.xgath, 
			nbmcv.bnwgo => ivdov.yipfs | ivdov.ldoty, 
			nbmcv.qadtc => ivdov.yipfs | ivdov.iprwf, 
			nbmcv.snzjq => ivdov.yipfs | ivdov.zanwd | ivdov.saddd, 
			nbmcv.ztwat => ivdov.yipfs | ivdov.saddd | ivdov.lrjcl, 
			nbmcv.cqgrp => ivdov.yipfs | ivdov.mcvxy | ivdov.saddd | ivdov.tppgj, 
			nbmcv.gboyb => ivdov.yipfs | ivdov.lcupz | ivdov.mjbij | ivdov.juvah | ivdov.saddd | ivdov.ykecm | ivdov.nhhts | ivdov.mcakg, 
			nbmcv.iqmzq => ivdov.yipfs | ivdov.saddd | ivdov.ivrgb | ivdov.xbgqc | ivdov.utied, 
			nbmcv.mkyzz => ivdov.yipfs | ivdov.saddd | ivdov.ajjzf, 
			nbmcv.nslgs => ivdov.yipfs | ivdov.saddd | ivdov.hpmdc, 
			nbmcv.jyhyf => ivdov.yipfs | ivdov.saddd | ivdov.yxntg, 
			nbmcv.vztdi => ivdov.yipfs | ivdov.saddd | ivdov.mxzsf, 
			nbmcv.bhpke => ivdov.yipfs | ivdov.juvah | ivdov.mcakg, 
			nbmcv.xtpnb => ivdov.yipfs | ivdov.mcvxy | ivdov.saddd | ivdov.lrjcl | ivdov.ivrgb | ivdov.xbgqc | ivdov.utied | ivdov.tppgj, 
			nbmcv.rxsqs => ivdov.yipfs | ivdov.mcvxy | ivdov.pxcxl | ivdov.zanwd | ivdov.saddd | ivdov.lrjcl | ivdov.tppgj, 
			nbmcv.niwnm => (ivdov)2147483647, 
			_ => (ivdov)0, 
		};
	}

	public static ivdov qecvn(nbmcv p0)
	{
		ivdov ivdov2 = svzkb(p0);
		int num;
		if (ivdov2 == (ivdov)0 || 1 == 0)
		{
			num = 1;
			if (num == 0)
			{
				goto IL_0018;
			}
			goto IL_002b;
		}
		goto IL_003a;
		IL_002b:
		if (num > 1073741824)
		{
			goto IL_003a;
		}
		goto IL_0018;
		IL_003a:
		return ivdov2;
		IL_0018:
		if (((uint)num & (uint)p0) == (uint)num)
		{
			ivdov2 |= svzkb((nbmcv)num);
		}
		num <<= 1;
		goto IL_002b;
	}
}
