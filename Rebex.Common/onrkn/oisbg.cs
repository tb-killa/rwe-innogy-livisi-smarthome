using System;
using Rebex.IO;

namespace onrkn;

internal class oisbg : EventArgs
{
	private Exception nxnww;

	private TransferAction lzfhx;

	private TransferProblemType jphvy;

	private string mgvpm;

	private LocalItem kfwaq;

	private string kvoua;

	private FileSystemItem lfudd;

	private omidq ooawo;

	private char[] tjmzi;

	private ChecksumAlgorithm? oyuxm;

	private object bknpl;

	private omidq whclm;

	private string eeqas;

	public Exception tbbdz
	{
		get
		{
			return nxnww;
		}
		private set
		{
			nxnww = value;
		}
	}

	public TransferAction xacss
	{
		get
		{
			return lzfhx;
		}
		private set
		{
			lzfhx = value;
		}
	}

	public TransferProblemType ardcs
	{
		get
		{
			return jphvy;
		}
		private set
		{
			jphvy = value;
		}
	}

	public string cettx
	{
		get
		{
			return mgvpm;
		}
		private set
		{
			mgvpm = value;
		}
	}

	public LocalItem buymq
	{
		get
		{
			return kfwaq;
		}
		private set
		{
			kfwaq = value;
		}
	}

	public string denym
	{
		get
		{
			return kvoua;
		}
		private set
		{
			kvoua = value;
		}
	}

	public FileSystemItem xckxh
	{
		get
		{
			return lfudd;
		}
		private set
		{
			lfudd = value;
		}
	}

	public omidq chzjg
	{
		get
		{
			return ooawo;
		}
		private set
		{
			ooawo = value;
		}
	}

	public char[] nphrg
	{
		get
		{
			return tjmzi;
		}
		private set
		{
			tjmzi = value;
		}
	}

	public ChecksumAlgorithm? xxbmi
	{
		get
		{
			return oyuxm;
		}
		private set
		{
			oyuxm = value;
		}
	}

	public object euxmu
	{
		get
		{
			return bknpl;
		}
		private set
		{
			bknpl = value;
		}
	}

	public omidq ulaxh
	{
		get
		{
			return whclm;
		}
		set
		{
			whclm = value;
		}
	}

	public string wiqoh
	{
		get
		{
			return eeqas;
		}
		set
		{
			eeqas = value;
		}
	}

	public void kslpy(omidq p0, string p1)
	{
		if (!Enum.IsDefined(typeof(omidq), p0) || 1 == 0)
		{
			throw new ArgumentException(brgjd.edcru("The specified value ({0}) is not a valid action.", p1), "value");
		}
		if (!luhwi(p0) || 1 == 0)
		{
			throw new InvalidOperationException(brgjd.edcru("The specified action ({0}) cannot be selected in the current state.  Use the PossibleActions property or the IsActionPossible method to determine whether an action is possible.", p1));
		}
	}

	public void scigb(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value", "Path cannot be null.");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "value");
		}
		if (p0.IndexOfAny(nphrg) < 0)
		{
			return;
		}
		throw new ArgumentException(brgjd.edcru("NewName '{0}' contains one or more invalid characters.", p0), "value");
	}

	internal oisbg(FileSystemItem source, Exception exception, TransferAction action, TransferProblemType problemType, string localPath, LocalItem localItem, string remotePath, FileSystemItem remoteItem, omidq defaultReaction, omidq possibleReactions, char[] invalidFileNameChars, ChecksumAlgorithm? remoteChecksumType, object userState)
	{
		tbbdz = exception;
		xacss = action;
		ardcs = problemType;
		cettx = localPath;
		buymq = localItem;
		denym = remotePath;
		xckxh = remoteItem;
		ulaxh = defaultReaction;
		chzjg = possibleReactions;
		nphrg = invalidFileNameChars;
		xxbmi = remoteChecksumType;
		euxmu = userState;
		if (luhwi(omidq.cbxuc) && 0 == 0 && source != null && 0 == 0)
		{
			wiqoh = vtdxm.sfvzf(source.Name);
		}
	}

	public bool luhwi(omidq p0)
	{
		return (p0 & chzjg) == p0;
	}
}
