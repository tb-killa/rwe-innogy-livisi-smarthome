using System;
using System.Collections.Generic;

namespace Rebex.IO;

public class TraversingEventArgs : EventArgs
{
	private TransferAction hixey;

	private TraversingState qcqge;

	private FileSystemItem lwgbp;

	private int btytd;

	private long czifm;

	private object cjjtm;

	public TransferAction Action => hixey;

	public TraversingState TraversingState => qcqge;

	public FileSystemItem Item => lwgbp;

	public int FilesTotal => btytd;

	public long BytesTotal => czifm;

	public object UserState => cjjtm;

	protected TraversingEventArgs(object info)
	{
		if (!(info is Dictionary<int, object> dictionary) || 1 == 0)
		{
			throw new ArgumentException("Invalid object specified.", "info");
		}
		hixey = (TransferAction)dictionary[1];
		qcqge = (TraversingState)dictionary[2];
		lwgbp = (FileSystemItem)dictionary[3];
		btytd = (int)dictionary[7];
		czifm = (long)dictionary[10];
		cjjtm = dictionary[14];
	}
}
