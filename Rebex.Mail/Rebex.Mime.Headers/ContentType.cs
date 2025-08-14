using System;
using System.Globalization;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public class ContentType : IHeader, qmqrp
{
	internal const string cydvj = "text/plain";

	internal const string kqpxm = "multipart/digest";

	internal const string quoam = "message/rfc822";

	internal const string uazuz = "application/octet-stream";

	internal const string llrze = "multipart/signed";

	internal const string toorb = "application/x-pkcs7-signature";

	internal const string sjlde = "application/pkcs7-signature";

	internal const string njsah = "application/pkcs7-mime";

	internal const string tqcld = "application/x-pkcs7-mime";

	internal const string ducpt = "multipart/relative";

	private string kxaci;

	private string bdyuj;

	private MimeParameterCollection vfvgb = new MimeParameterCollection();

	private bool bbyzf
	{
		get
		{
			return vfvgb.cvjtw;
		}
		set
		{
			vfvgb.cvjtw = value;
		}
	}

	private long odqeh => vfvgb.xbrdn;

	public string MediaType => msrzy + "/" + mniut;

	internal string msrzy => kxaci.ToLower(CultureInfo.InvariantCulture);

	internal string mniut => bdyuj.ToLower(CultureInfo.InvariantCulture);

	public MimeParameterCollection Parameters => vfvgb;

	public string Boundary
	{
		get
		{
			return vfvgb["boundary"];
		}
		set
		{
			vfvgb["boundary"] = value;
		}
	}

	public string CharSet
	{
		get
		{
			return vfvgb["charset"];
		}
		set
		{
			vfvgb["charset"] = value;
		}
	}

	public Encoding Encoding
	{
		get
		{
			string text = vfvgb["charset"];
			if (text == null || 1 == 0)
			{
				if (string.Compare(kxaci, "text", StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
				{
					return EncodingTools.ASCII;
				}
				return null;
			}
			return kgbvh.qirxf(text);
		}
	}

	public ContentType(string mediaType)
	{
		mediaType = kgbvh.nzgih(mediaType, "mediaType");
		if (!eumcm(mediaType, out var p) || 1 == 0)
		{
			throw new ArgumentException("Media type is not valid.", "mediaType");
		}
		kxaci = mediaType.Substring(0, p);
		bdyuj = mediaType.Substring(p + 1);
	}

	internal static bool vgubl(string p0)
	{
		int p1;
		return eumcm(p0, out p1);
	}

	private static bool eumcm(string p0, out int p1)
	{
		p1 = p0.IndexOf('/');
		if (p1 <= 0 || p1 >= p0.Length - 1 || p1 != p0.LastIndexOf('/'))
		{
			return false;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0032;
		}
		goto IL_0072;
		IL_0032:
		char c = p0[num];
		if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '/' && c != '-' && c != '.' && c != '+')
		{
			return false;
		}
		num++;
		goto IL_0072;
		IL_0072:
		if (num < p0.Length)
		{
			goto IL_0032;
		}
		return true;
	}

	private ContentType(string type, string subtype)
	{
		kxaci = type;
		bdyuj = subtype;
	}

	public IHeader Clone()
	{
		ContentType contentType = new ContentType(kxaci, bdyuj);
		contentType.vfvgb = vfvgb.mygie();
		return contentType;
	}

	internal void dczab(string p0, string p1)
	{
		kxaci = p0;
		bdyuj = p1;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(kxaci.ToLower(CultureInfo.InvariantCulture));
		stringBuilder.Append("/");
		stringBuilder.Append(bdyuj.ToLower(CultureInfo.InvariantCulture));
		stringBuilder.Append(vfvgb.ToString());
		return stringBuilder.ToString();
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(kxaci + "/" + bdyuj);
		vfvgb.Encode(writer);
	}

	public override int GetHashCode()
	{
		return kxaci.GetHashCode() ^ bdyuj.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ContentType contentType) || 1 == 0)
		{
			return false;
		}
		if (((string.Compare(kxaci, contentType.kxaci, StringComparison.OrdinalIgnoreCase) != 0) ? true : false) || string.Compare(bdyuj, contentType.bdyuj, StringComparison.OrdinalIgnoreCase) != 0)
		{
			return false;
		}
		return vfvgb.abqlt(contentType.vfvgb);
	}

	internal static IHeader tcamm(stzvh p0)
	{
		string text = p0.aqydg();
		int num = text.IndexOf('/');
		ContentType contentType = ((num > 0 && num < text.Length - 1) ? new ContentType(text.Substring(0, num), text.Substring(num + 1)) : new ContentType("application", "octet-stream"));
		contentType.vfvgb.qlttk(p0);
		return contentType;
	}
}
