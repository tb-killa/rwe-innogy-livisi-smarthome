using System;
using Rebex.IO;

namespace Rebex.Net;

public class ListItemReceivedEventArgs : EventArgs
{
	private readonly string vhocg;

	private readonly FileSystemItem nrnnv;

	private bool iznys;

	private bool ztdvz;

	private object xtbdl;

	public object UserState => xtbdl;

	public string RawLine => vhocg;

	public FileSystemItem Item => nrnnv;

	protected bool Ignored => ztdvz;

	protected bool Aborted => iznys;

	public void Ignore()
	{
		ztdvz = true;
	}

	public void Abort()
	{
		iznys = true;
	}

	public ListItemReceivedEventArgs(string rawLine, FileSystemItem item)
	{
		vhocg = rawLine;
		nrnnv = item;
	}

	internal ListItemReceivedEventArgs(string rawLine, FileSystemItem item, object userState)
	{
		vhocg = rawLine;
		nrnnv = item;
		xtbdl = userState;
	}
}
