using System;

namespace Rebex.Mime;

public class MimeParsingHeaderEventArgs : EventArgs
{
	private readonly string dlgrg;

	private string jvhkm;

	public string Name => dlgrg;

	public string Raw
	{
		get
		{
			return jvhkm;
		}
		set
		{
			jvhkm = value;
		}
	}

	internal MimeParsingHeaderEventArgs(string name, string raw)
	{
		dlgrg = name;
		Raw = raw;
	}
}
