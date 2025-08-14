using System;
using System.Collections.Generic;
using onrkn;

namespace Rebex.IO;

public class ProblemDetectedEventArgs : EventArgs
{
	private Exception afgkp;

	private TransferAction rlpjr;

	private TransferProblemType btpiy;

	private string aauyt;

	private LocalItem ygado;

	private string vdigc;

	private FileSystemItem ezhkl;

	private TransferProblemReaction wgyhr;

	private TransferProblemReaction twpcj;

	private OverwriteCondition obtvl;

	private string dvloo;

	private string qztcc;

	private char[] ktcds;

	private ChecksumAlgorithm xnuww;

	private object jcjfg;

	public TransferAction Action => rlpjr;

	public Exception Exception => afgkp;

	public TransferProblemType ProblemType => btpiy;

	public string LocalPath => aauyt;

	public LocalItem LocalItem => ygado;

	public string RemotePath => vdigc;

	public FileSystemItem RemoteItem => ezhkl;

	public TransferProblemReaction Reaction => twpcj;

	public OverwriteCondition OverwriteCondition => obtvl;

	public string NewName => qztcc;

	public object UserState => jcjfg;

	protected ProblemDetectedEventArgs(object info)
	{
		if (!(info is Dictionary<int, object> dictionary) || 1 == 0)
		{
			throw new ArgumentException("Invalid object specified.", "info");
		}
		rlpjr = (TransferAction)dictionary[1];
		afgkp = (Exception)dictionary[15];
		btpiy = (TransferProblemType)dictionary[16];
		aauyt = (string)dictionary[19];
		ygado = (LocalItem)dictionary[17];
		vdigc = (string)dictionary[20];
		ezhkl = (FileSystemItem)dictionary[18];
		twpcj = (TransferProblemReaction)dictionary[21];
		wgyhr = (TransferProblemReaction)dictionary[22];
		obtvl = (OverwriteCondition)dictionary[26];
		qztcc = (dvloo = (string)dictionary[25]);
		ktcds = (char[])dictionary[23];
		xnuww = (ChecksumAlgorithm)dictionary[27];
		jcjfg = dictionary[14];
	}

	private void sxkso(TransferProblemReaction p0)
	{
		if (!IsReactionPossible(p0) || 1 == 0)
		{
			throw new InvalidOperationException(brgjd.edcru("The specified reaction ({0}) cannot be selected in the current state. Use IsReactionPossible method to determine whether the reaction is possible.", p0));
		}
	}

	public bool IsReactionPossible(TransferProblemReaction reaction)
	{
		return (reaction & wgyhr) == reaction;
	}

	public bool IsOverwriteConditionPossible(OverwriteCondition condition)
	{
		if (!IsReactionPossible(TransferProblemReaction.Overwrite) || 1 == 0)
		{
			return false;
		}
		if (condition == OverwriteCondition.ChecksumDiffers)
		{
			return xnuww != (ChecksumAlgorithm)0;
		}
		return Enum.IsDefined(typeof(OverwriteCondition), condition);
	}

	public void Skip()
	{
		twpcj = TransferProblemReaction.Skip;
	}

	public void Cancel()
	{
		twpcj = TransferProblemReaction.Cancel;
	}

	public void Fail()
	{
		twpcj = TransferProblemReaction.Fail;
	}

	public void Retry()
	{
		sxkso(TransferProblemReaction.Retry);
		twpcj = TransferProblemReaction.Retry;
	}

	public void Resume()
	{
		sxkso(TransferProblemReaction.Resume);
		twpcj = TransferProblemReaction.Resume;
	}

	public void FollowLink()
	{
		sxkso(TransferProblemReaction.FollowLink);
		twpcj = TransferProblemReaction.FollowLink;
	}

	public void Rename()
	{
		Rename(null);
	}

	public void Rename(string newName)
	{
		if (newName == null || 1 == 0)
		{
			newName = dvloo;
		}
		if (newName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "newName");
		}
		if (newName.IndexOfAny(ktcds) >= 0)
		{
			throw new ArgumentException(brgjd.edcru("NewName '{0}' contains one or more invalid characters.", newName), "newName");
		}
		sxkso(TransferProblemReaction.Rename);
		twpcj = TransferProblemReaction.Rename;
		qztcc = newName;
	}

	public void Overwrite()
	{
		Overwrite(OverwriteCondition.None);
	}

	public void Overwrite(OverwriteCondition condition)
	{
		sxkso(TransferProblemReaction.Overwrite);
		switch (condition)
		{
		case OverwriteCondition.ChecksumDiffers:
			if (xnuww == (ChecksumAlgorithm)0 || 1 == 0)
			{
				throw new InvalidOperationException("The server doesn't support any available checksum method.");
			}
			break;
		default:
			throw hifyx.nztrs("condition", condition, "Argument is out of range of valid values.");
		case OverwriteCondition.None:
		case OverwriteCondition.SizeDiffers:
		case OverwriteCondition.Older:
			break;
		}
		twpcj = TransferProblemReaction.Overwrite;
		obtvl = condition;
	}
}
