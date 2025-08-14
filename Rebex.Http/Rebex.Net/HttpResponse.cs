using System;
using System.IO;
using System.Net;
using onrkn;

namespace Rebex.Net;

public class HttpResponse : WebResponse
{
	private readonly thths nreyi;

	private readonly Uri rjlbw;

	private readonly WebHeaderCollection ifert;

	private readonly TlsCipher jizck;

	private readonly DateTime uuzfh;

	private DateTime? xubrr;

	private string fpoqj;

	private string tqvsv;

	public override long ContentLength => nreyi.belbk;

	public override string ContentType => nreyi.zgbyk;

	public override WebHeaderCollection Headers => ifert;

	public TlsCipher Cipher => jizck;

	public Version ProtocolVersion => nreyi.fiqxe;

	public override Uri ResponseUri => rjlbw;

	public string Server => nreyi.virwn["Server"];

	public HttpStatusCode StatusCode => nreyi.xgkmt;

	public string StatusDescription => nreyi.vbeuo;

	public string CharacterSet
	{
		get
		{
			string[] array2;
			int num;
			if (fpoqj == null || 1 == 0)
			{
				string text = Headers["Content-Type"];
				if (text != null && 0 == 0)
				{
					string[] array = text.Split(';');
					array2 = array;
					num = 0;
					if (num != 0)
					{
						goto IL_0056;
					}
					goto IL_009f;
				}
				fpoqj = string.Empty;
			}
			goto IL_00c0;
			IL_009f:
			if (num < array2.Length)
			{
				goto IL_0056;
			}
			goto IL_00c0;
			IL_0056:
			string text2 = array2[num];
			if (text2.IndexOf("charset", 0, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				string[] array3 = text2.Split('=');
				if (array3.Length > 1)
				{
					fpoqj = array3[1].Trim();
					goto IL_00c0;
				}
			}
			num++;
			goto IL_009f;
			IL_00c0:
			return fpoqj;
		}
	}

	public string ContentEncoding => Headers["Content-Encoding"];

	public string Method
	{
		get
		{
			return tqvsv;
		}
		private set
		{
			tqvsv = value;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (!xubrr.HasValue || 1 == 0)
			{
				string text = Headers["Last-Modified"];
				if (string.IsNullOrEmpty(text) && 0 == 0)
				{
					xubrr = uuzfh;
				}
				else
				{
					if (!dtjod.rvufw(text, out var p))
					{
						throw new ProtocolViolationException(brgjd.edcru("The value of the Last-Modified header is invalid ({0}).", text));
					}
					xubrr = p.ToLocalTime();
				}
			}
			return xubrr.Value;
		}
	}

	internal HttpResponse(thths clientResponse, string method, Uri responseUri, TlsCipher cipher)
	{
		Action<string, string> action = null;
		base._002Ector();
		nreyi = clientResponse;
		rjlbw = responseUri;
		jizck = cipher;
		uuzfh = DateTime.Now;
		Method = method;
		ifert = new WebHeaderCollection();
		pjyrs virwn = clientResponse.virwn;
		if (action == null || 1 == 0)
		{
			action = lmugj;
		}
		virwn.dxkfp(action);
	}

	public override Stream GetResponseStream()
	{
		try
		{
			return nreyi.vhkfm();
		}
		catch (ujepc p)
		{
			throw zjpbq.entdh(p, this);
		}
	}

	public override void Close()
	{
		dkxpa();
	}

	internal void dkxpa()
	{
		nreyi.jpxci();
	}

	private void lmugj(string p0, string p1)
	{
		try
		{
			ifert.Add(p0, p1);
		}
		catch (ArgumentException)
		{
		}
	}
}
