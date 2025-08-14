using System;
using System.Globalization;
using onrkn;

namespace Rebex.Net;

public class SmtpResponse
{
	private int kiiry;

	private string xovjq;

	private string iailo;

	private int qddju;

	private int aoekm;

	public string Raw
	{
		get
		{
			if (iailo == null || 1 == 0)
			{
				iailo = kiiry + " " + xovjq + "\r\n";
			}
			return iailo;
		}
	}

	public string Description => xovjq;

	public int Code => kiiry;

	public int Group => kiiry / 100;

	public bool Success
	{
		get
		{
			if (Group == 2 || Group == 3)
			{
				return true;
			}
			return false;
		}
	}

	public int Class
	{
		get
		{
			if (qddju < 0)
			{
				return -1;
			}
			return kiiry / 100;
		}
	}

	public int Subject => qddju;

	public int Detail => aoekm;

	internal void gogbt()
	{
		int num = xovjq.IndexOf(' ');
		if (num < 0)
		{
			num = xovjq.Length;
		}
		if (num < 5 || num > 9 || xovjq[1] != '.')
		{
			return;
		}
		switch (xovjq[0])
		{
		case '2':
		case '4':
		case '5':
		{
			if (Group != xovjq[0] - 48)
			{
				break;
			}
			string text = xovjq.Substring(2, num - 2);
			string[] array = text.Split('.');
			if (array.Length != 2)
			{
				break;
			}
			try
			{
				qddju = int.Parse(array[0], CultureInfo.InvariantCulture);
				aoekm = int.Parse(array[1], CultureInfo.InvariantCulture);
				xovjq = xovjq.Substring(num + 1);
				break;
			}
			catch (FormatException)
			{
				qddju = -1;
				aoekm = -1;
				break;
			}
		}
		}
	}

	internal void ezfnh(string p0, int p1)
	{
		try
		{
			kiiry = int.Parse(p0.Substring(0, 3), CultureInfo.InvariantCulture);
			if (p1 >= 4)
			{
				xovjq = p0.Substring(4, p1 - 4).Trim();
			}
			else
			{
				xovjq = "";
			}
			iailo = p0.Substring(0, p0.Length - 2);
			qddju = -1;
			aoekm = -1;
			gogbt();
		}
		catch
		{
			throw new SmtpException("Invalid SMTP code.", SmtpExceptionStatus.ServerProtocolViolation);
		}
		if (kiiry < 100 || kiiry >= 600)
		{
			throw new SmtpException("Invalid SMTP response.", SmtpExceptionStatus.ServerProtocolViolation);
		}
	}

	internal void cfxnj(string p0, int p1)
	{
		xovjq = p0.Substring(p1 + 4, p0.Length - p1 - 6);
		iailo = p0;
		gogbt();
	}

	internal SmtpResponse sxztf()
	{
		return new SmtpResponse(kiiry, qddju, aoekm, xovjq, iailo);
	}

	internal SmtpResponse()
	{
	}

	internal void lqgkr()
	{
		kiiry = 0;
		qddju = -1;
		aoekm = -1;
		xovjq = null;
		iailo = null;
	}

	public SmtpResponse(int code, string description, string raw)
	{
		if (code < 100 || code >= 600)
		{
			throw hifyx.nztrs("code", code, "Argument is out of range of valid values.");
		}
		kiiry = code;
		qddju = -1;
		aoekm = -1;
		xovjq = description;
		iailo = raw;
	}

	public SmtpResponse(int code, int subject, int detail, string description, string raw)
	{
		if (code < 100 || code >= 600)
		{
			throw hifyx.nztrs("code", code, "Argument is out of range of valid values.");
		}
		if (detail < -1 || detail > 999)
		{
			throw hifyx.nztrs("detail", detail, "Argument is out of range of valid values.");
		}
		if (detail == -1)
		{
			if (subject != -1)
			{
				throw hifyx.nztrs("subject", subject, "Argument is out of range of valid values.");
			}
		}
		else if (subject < 0 || subject > 999)
		{
			throw hifyx.nztrs("subject", subject, "Argument is out of range of valid values.");
		}
		kiiry = code;
		qddju = subject;
		aoekm = detail;
		xovjq = description;
		iailo = raw;
	}
}
