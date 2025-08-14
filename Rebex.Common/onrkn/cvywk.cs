using System;
using System.IO;
using Rebex.IO;

namespace onrkn;

internal static class cvywk
{
	public static TraversingState fkarr(aqicq p0)
	{
		return p0 switch
		{
			(aqicq)0 => (TraversingState)0, 
			aqicq.hcxlr => TraversingState.HierarchyRetrieving, 
			aqicq.miduf => TraversingState.HierarchyRetrieved, 
			aqicq.brcpb => TraversingState.DirectoryRetrieving, 
			aqicq.nzwwg => TraversingState.DirectoryRetrieved, 
			_ => throw new ArgumentException("Invalid BatchOperationStep to convert.", "step"), 
		};
	}

	public static DeleteProgressState fgqns(aqicq p0)
	{
		return p0 switch
		{
			(aqicq)0 => (DeleteProgressState)0, 
			aqicq.yrhed => DeleteProgressState.FileDeleting, 
			aqicq.yynwh => DeleteProgressState.DirectoryProcessing, 
			aqicq.rxwaf => DeleteProgressState.FileDeleted, 
			aqicq.mpwkk => DeleteProgressState.DirectoryDeleted, 
			aqicq.ivywy => DeleteProgressState.DeleteCompleted, 
			_ => throw new ArgumentException("Invalid BatchOperationStep to convert.", "step"), 
		};
	}

	public static TransferProgressState dfsyq(aqicq p0)
	{
		return p0 switch
		{
			(aqicq)0 => (TransferProgressState)0, 
			aqicq.yrhed => TransferProgressState.FileTransferring, 
			aqicq.cecby => TransferProgressState.FileTransferred, 
			aqicq.yynwh => TransferProgressState.DirectoryProcessing, 
			aqicq.vdzjk => TransferProgressState.DataBlockProcessed, 
			aqicq.ivywy => TransferProgressState.TransferCompleted, 
			_ => throw new ArgumentException("Invalid BatchOperationStep to convert.", "step"), 
		};
	}

	public static string rwapr(string p0, string p1, string p2)
	{
		string text = p0.Replace(p1, p2);
		while (text.Length != p0.Length)
		{
			p0 = text;
			text = p0.Replace(p1, p2);
		}
		return p0;
	}

	public static bool panxs(string p0, params char[] p1)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			return false;
		}
		return kwvqr(p0, 0, p1);
	}

	public static bool elcoo(string p0, params char[] p1)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			return false;
		}
		return kwvqr(p0, p0.Length - 1, p1);
	}

	public static bool kwvqr(string p0, int p1, params char[] p2)
	{
		return Array.IndexOf(p2, p0[p1]) >= 0;
	}

	public static string wbzis(string p0, char[] p1, bool p2, out bool p3, out bool p4)
	{
		char c = p1[0];
		int num = 1;
		if (num == 0)
		{
			goto IL_000d;
		}
		goto IL_001f;
		IL_000d:
		p0 = p0.Replace(p1[num], c);
		num++;
		goto IL_001f;
		IL_001f:
		if (num >= p1.Length)
		{
			if (p2 && 0 == 0 && p0.Length > 1 && char.IsLetter(p0[0]) && 0 == 0 && p0[1] == dahxy.njlzv)
			{
				if (p0.Length == 2)
				{
					p0 = vtdxm.smjda(p0);
				}
				else if (p0[2] != c)
				{
					p0 = Path.Combine(vtdxm.smjda(p0.Substring(0, 2)), p0.Remove(0, 2));
				}
			}
			p3 = p0[p0.Length - 1] == c;
			string text = p0.Trim(c);
			switch (text.Length)
			{
			case 0:
				p4 = false;
				break;
			case 1:
				p4 = text[0] == '.';
				break;
			case 2:
				p4 = text.Equals("..", StringComparison.Ordinal);
				break;
			default:
				p4 = (text.EndsWith(c + ".", StringComparison.Ordinal) ? true : false) || text.EndsWith(c + "..", StringComparison.Ordinal);
				break;
			}
			return p0;
		}
		goto IL_000d;
	}

	public static bool qbbjc(string p0, char[] p1)
	{
		wbzis(p0, p1, p2: false, out var p2, out var p3);
		if (!p2 || 1 == 0)
		{
			return p3;
		}
		return true;
	}

	public static bool vykcd(string p0, char[] p1)
	{
		int num = p0.IndexOfAny(dahxy.fmacz);
		int num2 = brgjd.pkosy(p0, p1);
		if (num >= 0)
		{
			return num < num2;
		}
		return false;
	}
}
