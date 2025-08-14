using System;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public class ReturnPath : IHeader
{
	private readonly mqucj qiece;

	public string Address => qiece.ToString();

	public string User
	{
		get
		{
			if (qiece.vffsa == null || 1 == 0)
			{
				return "";
			}
			return qiece.vffsa;
		}
	}

	public string Host
	{
		get
		{
			if (qiece.eeaqt == null || 1 == 0)
			{
				return "";
			}
			return qiece.eeaqt;
		}
	}

	public ReturnPath(string address)
	{
		address = kgbvh.nzgih(address, "address");
		qiece = new mqucj(address);
	}

	private ReturnPath(mqucj address)
	{
		qiece = address;
	}

	public IHeader Clone()
	{
		return new ReturnPath(qiece);
	}

	public override string ToString()
	{
		return qiece.ToString();
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		qiece.cygyr(writer, p1: true);
	}

	private static mqucj gjmzk(stzvh p0)
	{
		MailAddress mailAddress = new MailAddress(p0);
		return mailAddress.qqrmz;
	}

	internal static IHeader ymslu(stzvh p0)
	{
		return new ReturnPath(gjmzk(p0));
	}
}
