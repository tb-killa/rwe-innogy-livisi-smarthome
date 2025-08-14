using System;
using Rebex.Mime.Headers;

namespace Rebex.Mime;

public class MimeUnparsableHeaderEventArgs : EventArgs
{
	private readonly string fhlqi;

	private readonly string rsyvz;

	private readonly MimeException mwmfy;

	private bool adcun;

	private IHeader ezkdp;

	private readonly MimeUnparsableHeaderSeverity dqetk;

	private readonly MimeUnparsableHeaderStatus pduio;

	public string Name => fhlqi;

	public string Raw => rsyvz;

	public MimeException Error => mwmfy;

	public bool Ignore
	{
		get
		{
			return adcun;
		}
		set
		{
			adcun = value;
		}
	}

	internal IHeader wfcxz => ezkdp;

	public MimeUnparsableHeaderSeverity Severity => dqetk;

	public MimeUnparsableHeaderStatus Status => pduio;

	public void SetHeaderValue(IHeader value)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		ezkdp = value.Clone();
		adcun = true;
	}

	internal MimeUnparsableHeaderEventArgs(string name, string raw, MimeException error, MimeUnparsableHeaderSeverity severity, MimeUnparsableHeaderStatus status)
	{
		fhlqi = name;
		rsyvz = raw;
		mwmfy = error;
		dqetk = severity;
		pduio = status;
	}
}
