using System;
using System.Collections.Generic;
using onrkn;

namespace Rebex.IO;

public class DeleteProgressChangedEventArgs : ProgressChangedEventArgs
{
	private DeleteProgressState idana;

	private FileSystemItem offks;

	private int nruxs;

	private int skutz;

	private int wznjd;

	public DeleteProgressState DeleteState => idana;

	public FileSystemItem Item => offks;

	public int FilesTotal => nruxs;

	public int FilesProcessed => skutz;

	public int FilesDeleted => wznjd;

	public new double ProgressPercentage => hrpun.nhiyi(skutz, nruxs);

	protected DeleteProgressChangedEventArgs(object info)
		: base((int)hrpun.nhiyi(hrpun.kgixw<int>(info, tovyl.bzshz), hrpun.kgixw<int>(info, tovyl.vdico)), hrpun.kgixw<object>(info, tovyl.cwufz))
	{
		if (!(info is Dictionary<int, object> dictionary) || 1 == 0)
		{
			throw new ArgumentException("Invalid object specified.", "info");
		}
		idana = (DeleteProgressState)dictionary[2];
		offks = (FileSystemItem)dictionary[3];
		nruxs = (int)dictionary[7];
		skutz = (int)dictionary[8];
		wznjd = (int)dictionary[9];
	}
}
