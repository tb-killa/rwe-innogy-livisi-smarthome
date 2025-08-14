using System;

namespace onrkn;

internal class mmgqv
{
	public readonly sbnrz iyvve;

	public ajuaj jprco;

	public string iaqgi;

	public mmgqv(sbnrz child)
	{
		jprco = ajuaj.icyep;
		iyvve = child;
	}

	public mmgqv(ajuaj type, string value)
	{
		if (type == ajuaj.icyep || 1 == 0)
		{
			throw new ArgumentException("Invalid type.", "type");
		}
		jprco = type;
		iaqgi = value;
	}

	public override string ToString()
	{
		return jprco switch
		{
			ajuaj.icyep => brgjd.edcru("{0}{1}...{2}", '{', iyvve.ToString(), '}'), 
			ajuaj.gdmuq => '\\' + iaqgi, 
			ajuaj.lmtqx => "\\'" + iaqgi, 
			ajuaj.exzyo => brgjd.edcru("\\x{0:X2}", (int)iaqgi[0]), 
			_ => iaqgi, 
		};
	}
}
