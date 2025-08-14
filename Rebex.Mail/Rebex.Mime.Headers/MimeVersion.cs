using System;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public class MimeVersion : IHeader
{
	private int iicsz;

	private int ddnob;

	public int Major => iicsz;

	public int Minor => ddnob;

	public MimeVersion(int major, int minor)
	{
		if (major < 1 || major > 9)
		{
			throw new ArgumentOutOfRangeException("major");
		}
		if (minor < 0 || minor > 9)
		{
			throw new ArgumentOutOfRangeException("minor");
		}
		iicsz = major;
		ddnob = minor;
	}

	public IHeader Clone()
	{
		return new MimeVersion(iicsz, ddnob);
	}

	public override string ToString()
	{
		return iicsz + "." + ddnob;
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(ToString());
	}

	internal static IHeader brxqb(stzvh p0)
	{
		hszhl hszhl = hszhl.krnvs(p0);
		string[] array = hszhl.ToString().Split('.');
		if (array.Length != 2)
		{
			throw MimeException.dngxr(0, "Invalid MIME version header.");
		}
		string text = array[0].Trim();
		string text2 = array[1].Trim();
		if (text.Length != 1 || text2.Length < 1)
		{
			throw MimeException.dngxr(0, "Invalid MIME version header.");
		}
		if (text[0] < '1' || text[0] > '9' || text2[0] < '0' || text2[0] > '9')
		{
			throw MimeException.dngxr(0, "Invalid MIME version header.");
		}
		return new MimeVersion(text[0] - 48, text2[0] - 48);
	}
}
