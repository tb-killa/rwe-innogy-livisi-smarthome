using System;
using System.Collections.Generic;
using Rebex.IO;

namespace onrkn;

internal class hrpun
{
	public static double nhiyi(long p0, long p1)
	{
		if (p1 < 0)
		{
			return 0.0;
		}
		if (p0 >= p1)
		{
			return 100.0;
		}
		double num = 100.0 * (double)p0 / (double)p1;
		if (num >= 100.0)
		{
			return 100.0;
		}
		if (num <= 0.0)
		{
			return 0.0;
		}
		return num;
	}

	public static int vbgnd(TransferAction p0, aqicq p1, double p2)
	{
		if (p0 == TransferAction.Listing)
		{
			switch (p1)
			{
			case aqicq.miduf:
			case aqicq.bnawl:
				return 100;
			default:
				return 0;
			}
		}
		switch (p1)
		{
		case aqicq.hcxlr:
		case aqicq.brcpb:
		case aqicq.nzwwg:
		case aqicq.nwarc:
		case aqicq.cagtv:
			return 0;
		default:
			return (int)p2;
		}
	}

	public static T kgixw<T>(object p0, tovyl p1)
	{
		if (!(p0 is Dictionary<int, object> dictionary) || 1 == 0)
		{
			return default(T);
		}
		return (T)dictionary[(int)p1];
	}

	public static omidq vwzvm(TransferProblemReaction p0, OverwriteCondition p1)
	{
		return p0 switch
		{
			TransferProblemReaction.Overwrite => p1 switch
			{
				OverwriteCondition.None => omidq.nivzg, 
				OverwriteCondition.SizeDiffers => omidq.nwoav, 
				OverwriteCondition.Older => omidq.smbdu, 
				OverwriteCondition.ChecksumDiffers => omidq.xrmda, 
				_ => throw new InvalidOperationException("Invalid OverwriteCondition."), 
			}, 
			TransferProblemReaction.Cancel => omidq.ayvtv, 
			TransferProblemReaction.Fail => omidq.ytvbw, 
			TransferProblemReaction.Skip => omidq.dgkzq, 
			TransferProblemReaction.Retry => omidq.zgjbt, 
			TransferProblemReaction.Rename => omidq.cbxuc, 
			TransferProblemReaction.Resume => omidq.zbfno, 
			TransferProblemReaction.FollowLink => omidq.bnwfa, 
			_ => throw new InvalidOperationException("Invalid reaction."), 
		};
	}

	private static TransferProblemReaction dmino(omidq p0)
	{
		switch (p0)
		{
		case omidq.nivzg:
		case omidq.smbdu:
		case omidq.nwoav:
		case omidq.xrmda:
			return TransferProblemReaction.Overwrite;
		case omidq.ayvtv:
			return TransferProblemReaction.Cancel;
		case omidq.ytvbw:
			return TransferProblemReaction.Fail;
		case omidq.dgkzq:
			return TransferProblemReaction.Skip;
		case omidq.zgjbt:
			return TransferProblemReaction.Retry;
		case omidq.cbxuc:
			return TransferProblemReaction.Rename;
		case omidq.zbfno:
			return TransferProblemReaction.Resume;
		case omidq.bnwfa:
			return TransferProblemReaction.FollowLink;
		default:
			throw new NotImplementedException("Reaction is not implemented.");
		}
	}

	private static TransferProblemReaction tztba(omidq p0)
	{
		TransferProblemReaction transferProblemReaction = (TransferProblemReaction)0;
		if ((p0 & omidq.btzph) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.Overwrite;
			p0 &= ~omidq.btzph;
		}
		if ((p0 & omidq.ayvtv) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.Cancel;
			p0 &= ~omidq.ayvtv;
		}
		if ((p0 & omidq.ytvbw) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.Fail;
			p0 &= ~omidq.ytvbw;
		}
		if ((p0 & omidq.dgkzq) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.Skip;
			p0 &= ~omidq.dgkzq;
		}
		if ((p0 & omidq.zgjbt) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.Retry;
			p0 &= ~omidq.zgjbt;
		}
		if ((p0 & omidq.cbxuc) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.Rename;
			p0 &= ~omidq.cbxuc;
		}
		if ((p0 & omidq.zbfno) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.Resume;
			p0 &= ~omidq.zbfno;
		}
		if ((p0 & omidq.bnwfa) != 0 && 0 == 0)
		{
			transferProblemReaction |= TransferProblemReaction.FollowLink;
			p0 &= ~omidq.bnwfa;
		}
		if ((p0 & omidq.masrh) != 0 && 0 == 0)
		{
			transferProblemReaction |= (TransferProblemReaction)32768;
			p0 &= ~omidq.masrh;
		}
		if (p0 != 0 && 0 == 0)
		{
			throw new NotImplementedException("A reaction is not implemented.");
		}
		return transferProblemReaction;
	}

	private static OverwriteCondition aubjb(omidq p0)
	{
		return p0 switch
		{
			omidq.smbdu => OverwriteCondition.Older, 
			omidq.nwoav => OverwriteCondition.SizeDiffers, 
			omidq.xrmda => OverwriteCondition.ChecksumDiffers, 
			_ => OverwriteCondition.None, 
		};
	}

	public static Dictionary<int, object> hqytk(oisbg p0)
	{
		Dictionary<int, object> dictionary = new Dictionary<int, object>();
		dictionary.Add(1, p0.xacss);
		dictionary.Add(15, p0.tbbdz);
		dictionary.Add(16, p0.ardcs);
		dictionary.Add(19, p0.cettx);
		dictionary.Add(17, p0.buymq);
		dictionary.Add(20, p0.denym);
		dictionary.Add(18, p0.xckxh);
		dictionary.Add(21, dmino(p0.ulaxh));
		dictionary.Add(22, tztba(p0.chzjg));
		dictionary.Add(26, aubjb(p0.ulaxh));
		dictionary.Add(25, p0.wiqoh);
		dictionary.Add(23, p0.nphrg);
		dictionary.Add(27, p0.xxbmi);
		dictionary.Add(14, p0.euxmu);
		return dictionary;
	}

	public static Dictionary<int, object> begfh(TraversingState p0, oyifz p1)
	{
		Dictionary<int, object> dictionary = new Dictionary<int, object>();
		dictionary.Add(2, p0);
		dictionary.Add(1, p1.uaage);
		dictionary.Add(3, p1.Item);
		dictionary.Add(7, p1.mudko);
		dictionary.Add(10, p1.jcakx);
		dictionary.Add(14, p1.kqzlx);
		return dictionary;
	}

	public static Dictionary<int, object> zkztw(TransferProgressState p0, oyifz p1, long p2, int p3)
	{
		Dictionary<int, object> dictionary = new Dictionary<int, object>();
		dictionary.Add(2, p0);
		dictionary.Add(1, p1.uaage);
		dictionary.Add(3, p1.Item);
		dictionary.Add(4, p1.hlrzw);
		dictionary.Add(5, p1.ffdop);
		dictionary.Add(6, p1.aqpch);
		dictionary.Add(7, p1.mudko);
		dictionary.Add(8, p1.mbsca);
		dictionary.Add(9, p1.btlui);
		dictionary.Add(10, p1.jcakx);
		dictionary.Add(11, p1.naawe);
		dictionary.Add(12, p3);
		dictionary.Add(13, p1.unzmu);
		dictionary.Add(24, nhiyi(p1.cwtrc + p2, p1.jcakx));
		dictionary.Add(14, p1.kqzlx);
		return dictionary;
	}

	public static Dictionary<int, object> delkr(DeleteProgressState p0, oyifz p1)
	{
		Dictionary<int, object> dictionary = new Dictionary<int, object>();
		dictionary.Add(2, p0);
		dictionary.Add(3, p1.Item);
		dictionary.Add(7, p1.mudko);
		dictionary.Add(8, p1.mbsca);
		dictionary.Add(9, p1.btlui);
		dictionary.Add(14, p1.kqzlx);
		return dictionary;
	}
}
