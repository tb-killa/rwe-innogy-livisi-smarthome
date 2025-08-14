using System;
using System.Collections.Generic;
using onrkn;

namespace Rebex.IO;

public class TransferProgressChangedEventArgs : ProgressChangedEventArgs
{
	private TransferAction yryki;

	private TransferProgressState bbzxs;

	private FileSystemItem mzyzf;

	private string rqabt;

	private long ggtxn;

	private long zclho;

	private int pcwdb;

	private int aiomh;

	private int fdpmt;

	private long pjwuh;

	private long jbpyd;

	private int cbdbb;

	private long frrzc;

	private double gtkap;

	public TransferAction Action => yryki;

	public TransferProgressState TransferState => bbzxs;

	public FileSystemItem SourceItem => mzyzf;

	public string TargetPath => rqabt;

	public long CurrentFileBytesTransferred => zclho;

	public int FilesTotal => pcwdb;

	public int FilesProcessed => aiomh;

	public int FilesTransferred => fdpmt;

	public long BytesTotal => pjwuh;

	public long BytesTransferred => jbpyd;

	public long BytesSinceLastEvent => cbdbb;

	public long BytesPerSecond => frrzc;

	public new double ProgressPercentage => gtkap;

	public double CurrentFileProgressPercentage => hrpun.nhiyi(zclho, ggtxn);

	protected TransferProgressChangedEventArgs(object info)
		: base((int)hrpun.kgixw<double>(info, tovyl.dhafa), hrpun.kgixw<object>(info, tovyl.cwufz))
	{
		if (!(info is Dictionary<int, object> dictionary) || 1 == 0)
		{
			throw new ArgumentException("Invalid object specified.", "info");
		}
		yryki = (TransferAction)dictionary[1];
		bbzxs = (TransferProgressState)dictionary[2];
		mzyzf = (FileSystemItem)dictionary[3];
		rqabt = (string)dictionary[4];
		ggtxn = (long)dictionary[5];
		zclho = (long)dictionary[6];
		pcwdb = (int)dictionary[7];
		aiomh = (int)dictionary[8];
		fdpmt = (int)dictionary[9];
		pjwuh = (long)dictionary[10];
		jbpyd = (long)dictionary[11];
		cbdbb = (int)dictionary[12];
		frrzc = (long)dictionary[13];
		gtkap = (double)dictionary[24];
	}
}
