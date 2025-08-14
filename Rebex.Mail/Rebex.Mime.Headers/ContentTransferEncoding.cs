using System;
using System.Globalization;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public class ContentTransferEncoding : IHeader
{
	private readonly string rwfig;

	private static Func<FormatException, Exception> sdyjw;

	public bool IsKnown
	{
		get
		{
			string text;
			if ((text = rwfig.ToLower(CultureInfo.InvariantCulture)) == null || false || (!(text == "7bit") && !(text == "8bit") && !(text == "binary") && !(text == "quoted-printable") && !(text == "base64")))
			{
				return false;
			}
			return true;
		}
	}

	public TransferEncoding Encoding
	{
		get
		{
			string text;
			if ((text = rwfig.ToLower(CultureInfo.InvariantCulture)) != null && 0 == 0)
			{
				if (text == "7bit")
				{
					return TransferEncoding.SevenBit;
				}
				if (text == "8bit")
				{
					return TransferEncoding.EightBit;
				}
				if (text == "binary")
				{
					return TransferEncoding.Binary;
				}
				if (text == "quoted-printable")
				{
					return TransferEncoding.QuotedPrintable;
				}
				if (text == "base64")
				{
					return TransferEncoding.Base64;
				}
			}
			return TransferEncoding.Unknown;
		}
	}

	internal bool hqovl
	{
		get
		{
			string text;
			if ((text = rwfig.ToLower(CultureInfo.InvariantCulture)) != null && 0 == 0 && (text == "7bit" || text == "8bit"))
			{
				return false;
			}
			return true;
		}
	}

	internal bool rbzzl => string.Compare(rwfig, "7bit", StringComparison.OrdinalIgnoreCase) != 0;

	private ContentTransferEncoding(string transferEncoding)
	{
		if (transferEncoding == null || 1 == 0)
		{
			throw new ArgumentNullException("transferEncoding");
		}
		rwfig = transferEncoding;
	}

	public IHeader Clone()
	{
		return new ContentTransferEncoding(rwfig);
	}

	public ContentTransferEncoding(TransferEncoding transferEncoding)
	{
		switch (transferEncoding)
		{
		case TransferEncoding.SevenBit:
			rwfig = "7bit";
			break;
		case TransferEncoding.EightBit:
			rwfig = "8bit";
			break;
		case TransferEncoding.Binary:
			rwfig = "binary";
			break;
		case TransferEncoding.QuotedPrintable:
			rwfig = "quoted-printable";
			break;
		case TransferEncoding.Base64:
			rwfig = "base64";
			break;
		default:
			throw hifyx.nztrs("transferEncoding", transferEncoding, "Unknown transfer encoding.");
		}
	}

	public override string ToString()
	{
		return rwfig.ToLower(CultureInfo.InvariantCulture);
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(rwfig);
	}

	internal static IHeader heqkw(stzvh p0)
	{
		string text = p0.aqydg();
		if (text.Length == 0 || 1 == 0)
		{
			text = "7bit";
		}
		return new ContentTransferEncoding(text.Trim());
	}

	public override int GetHashCode()
	{
		return rwfig.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ContentTransferEncoding contentTransferEncoding) || 1 == 0)
		{
			return false;
		}
		return rwfig.Equals(contentTransferEncoding.rwfig);
	}

	public Stream CreateDecodingStream(Stream output)
	{
		switch (Encoding)
		{
		case TransferEncoding.Base64:
			if (sdyjw == null || 1 == 0)
			{
				sdyjw = ootpw;
			}
			return new xvufe(output, ownsInner: false, sdyjw);
		case TransferEncoding.QuotedPrintable:
			return new vmzeg(output, ownsInner: false);
		default:
			return output;
		}
	}

	private static Exception ootpw(FormatException p0)
	{
		return new MimeException("Base64 decoding error.", MimeExceptionStatus.MessageParserError, p0);
	}
}
