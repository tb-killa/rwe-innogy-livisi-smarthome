using System;
using System.Globalization;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public class ContentDisposition : IHeader, qmqrp
{
	internal const string jtekz = "filename";

	internal const string ekcgf = "creation-date";

	internal const string lltzn = "modification-date";

	internal const string ieozv = "read-date";

	internal const string oxnvf = "size";

	private string cijsj;

	private MimeParameterCollection yvygs = new MimeParameterCollection();

	private long caglb;

	private bool wlgcn;

	private bool opiwc
	{
		get
		{
			return wlgcn;
		}
		set
		{
			wlgcn = value;
			yvygs.cvjtw = value;
		}
	}

	private long usqfm => caglb + yvygs.xbrdn;

	public string Disposition => cijsj.ToLower(CultureInfo.InvariantCulture);

	public bool Inline
	{
		get
		{
			return string.Compare(cijsj, "inline", StringComparison.OrdinalIgnoreCase) == 0;
		}
		set
		{
			if (wlgcn && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			caglb++;
			if (value && 0 == 0)
			{
				cijsj = "inline";
			}
			else
			{
				cijsj = "attachment";
			}
		}
	}

	public MimeParameterCollection Parameters => yvygs;

	public string FileName
	{
		get
		{
			string text = Parameters["filename"];
			if (text == null || 1 == 0)
			{
				return string.Empty;
			}
			int num = brgjd.pkosy(text, '\\', '/');
			if (num >= 0)
			{
				text = text.Substring(num + 1);
			}
			return text;
		}
		set
		{
			if (wlgcn && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			caglb++;
			Parameters["filename"] = value;
		}
	}

	public MailDateTime ReadDate
	{
		get
		{
			return lunfx("read-date");
		}
		set
		{
			pkzwz("read-date", value);
		}
	}

	public MailDateTime CreationDate
	{
		get
		{
			return lunfx("creation-date");
		}
		set
		{
			pkzwz("creation-date", value);
		}
	}

	public MailDateTime ModificationDate
	{
		get
		{
			return lunfx("modification-date");
		}
		set
		{
			pkzwz("modification-date", value);
		}
	}

	public long? Size
	{
		get
		{
			string text = Parameters["size"];
			if (text == null || 1 == 0)
			{
				return null;
			}
			if (!brgjd.hujyn(text, out var p) || false || p < 0)
			{
				return null;
			}
			return p;
		}
		set
		{
			if (wlgcn && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			caglb++;
			Parameters["size"] = ((!value.HasValue) ? null : value.ToString());
		}
	}

	public ContentDisposition(string dispositionType)
		: this(dispositionType, checkType: true)
	{
	}

	private ContentDisposition(string dispositionType, bool checkType)
	{
		if (dispositionType == null || 1 == 0)
		{
			throw new ArgumentNullException("dispositionType");
		}
		if (checkType && 0 == 0)
		{
			kgbvh.hdlkr(dispositionType, "dispositionType", p2: false);
		}
		cijsj = dispositionType.ToLower(CultureInfo.InvariantCulture);
	}

	public IHeader Clone()
	{
		ContentDisposition contentDisposition = new ContentDisposition(cijsj, checkType: false);
		contentDisposition.yvygs = yvygs.mygie();
		contentDisposition.caglb = caglb;
		return contentDisposition;
	}

	private MailDateTime lunfx(string p0)
	{
		string text = Parameters[p0];
		if (text == null || 1 == 0)
		{
			return null;
		}
		try
		{
			MailDateTime mailDateTime = (MailDateTime)MailDateTime.aohzm(new stzvh(text));
			if (mailDateTime.UniversalTime.Year > DateTime.Now.Year + 10)
			{
				return null;
			}
			return mailDateTime;
		}
		catch (MimeException)
		{
			return null;
		}
	}

	private void pkzwz(string p0, MailDateTime p1)
	{
		if (wlgcn && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		caglb++;
		Parameters[p0] = p1?.ToString();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(cijsj);
		stringBuilder.Append(yvygs.ToString());
		return stringBuilder.ToString();
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(cijsj);
		yvygs.Encode(writer);
	}

	public override int GetHashCode()
	{
		return cijsj.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ContentDisposition contentDisposition) || 1 == 0)
		{
			return false;
		}
		if (string.Compare(cijsj, contentDisposition.cijsj, StringComparison.OrdinalIgnoreCase) != 0 && 0 == 0)
		{
			return false;
		}
		return yvygs.abqlt(contentDisposition.yvygs);
	}

	internal static IHeader ldvap(stzvh p0)
	{
		string dispositionType = p0.aqydg();
		ContentDisposition contentDisposition = new ContentDisposition(dispositionType, checkType: false);
		contentDisposition.Parameters.qlttk(p0);
		return contentDisposition;
	}
}
