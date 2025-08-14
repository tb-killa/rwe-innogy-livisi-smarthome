using System;
using Rebex.IO;

namespace onrkn;

internal class vgycx
{
	private xilhs ppwbx;

	private ItemType fzueb;

	private ixkhy otpug;

	private string rpqjw;

	private long gxyah;

	private DateTime slhkl;

	private DateTime ydzvi;

	private DateTime qkiil;

	private bool jimma;

	public xilhs snggt => ppwbx;

	public ItemType rirmp => fzueb;

	public ixkhy zdyzk => otpug;

	public string wyuyy
	{
		get
		{
			return rpqjw;
		}
		set
		{
			rpqjw = value;
		}
	}

	public long vtrzr => gxyah;

	public DateTime kkxpy => slhkl;

	public DateTime shsrc => ydzvi;

	public DateTime xwfgd => qkiil;

	internal bool ahuga
	{
		get
		{
			return jimma;
		}
		set
		{
			jimma = value;
		}
	}

	public vgycx(ItemType? type, ixkhy? flags, string name, long? length, DateTime? created, DateTime? modified, DateTime? accessed)
	{
		if (type.HasValue && 0 == 0)
		{
			ppwbx |= xilhs.kvasa;
			fzueb = type.Value;
		}
		if (flags.HasValue && 0 == 0)
		{
			ppwbx |= xilhs.dzwtu;
			otpug = flags.Value;
		}
		if (name != null && 0 == 0)
		{
			ppwbx |= xilhs.ggiiq;
			rpqjw = name;
		}
		if (length.HasValue && 0 == 0)
		{
			ppwbx |= xilhs.krccs;
			gxyah = length.Value;
		}
		if (created.HasValue && 0 == 0)
		{
			ppwbx |= xilhs.fistc;
			slhkl = created.Value;
		}
		if (modified.HasValue && 0 == 0)
		{
			ppwbx |= xilhs.prxbn;
			ydzvi = modified.Value;
		}
		if (accessed.HasValue && 0 == 0)
		{
			ppwbx |= xilhs.bhwst;
			qkiil = accessed.Value;
		}
		ahuga = false;
	}
}
