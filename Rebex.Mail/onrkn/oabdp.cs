using System;
using System.Text;
using Rebex.OutlookMessages;

namespace onrkn;

internal class oabdp
{
	private static readonly byte[] ftual = new byte[24]
	{
		0, 0, 0, 0, 129, 43, 31, 164, 190, 163,
		16, 25, 157, 110, 0, 221, 1, 15, 84, 2,
		0, 0, 1, 128
	};

	private readonly howhn wvzlg;

	private readonly zccyb bnlni;

	public howhn plpif => wvzlg;

	public zccyb sizjo => bnlni;

	public string mrwcz
	{
		get
		{
			qacae qacae2 = wvzlg[MsgPropertyTag.SmtpAddress];
			if (qacae2 != null && 0 == 0 && dzwgu.dovit(qacae2.pzpvc) && 0 == 0 && dzwgu.kqeon((string)qacae2.tgbhs) && 0 == 0)
			{
				return (string)qacae2.tgbhs;
			}
			qacae2 = wvzlg[MsgPropertyTag.EmailAddress];
			if (qacae2 != null && 0 == 0 && dzwgu.dovit(qacae2.pzpvc) && 0 == 0 && dzwgu.kqeon((string)qacae2.tgbhs) && 0 == 0)
			{
				return (string)qacae2.tgbhs;
			}
			return null;
		}
	}

	public string ubimv
	{
		get
		{
			qacae qacae2 = wvzlg[MsgPropertyTag.TransmittableDisplayName];
			if (qacae2 != null && 0 == 0 && dzwgu.dovit(qacae2.pzpvc) && 0 == 0 && ((string)qacae2.tgbhs).Length != 0 && 0 == 0)
			{
				return (string)qacae2.tgbhs;
			}
			qacae2 = wvzlg[MsgPropertyTag.DisplayName];
			if (qacae2 != null && 0 == 0 && dzwgu.dovit(qacae2.pzpvc) && 0 == 0 && ((string)qacae2.tgbhs).Length != 0 && 0 == 0)
			{
				return (string)qacae2.tgbhs;
			}
			qacae2 = wvzlg[MsgPropertyTag.RecipientDisplayName];
			if (qacae2 != null && 0 == 0 && dzwgu.dovit(qacae2.pzpvc) && 0 == 0 && ((string)qacae2.tgbhs).Length != 0 && 0 == 0)
			{
				return (string)qacae2.tgbhs;
			}
			return null;
		}
	}

	internal oabdp(howhn properties)
	{
		wvzlg = properties;
		qacae qacae2 = wvzlg[MsgPropertyTag.RecipientType];
		if (qacae2 != null && 0 == 0)
		{
			bnlni = (zccyb)((int)qacae2.tgbhs & 0xFF);
		}
	}

	internal static byte[] xngmq(string p0, string p1, string p2)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(p2);
		byte[] bytes2 = Encoding.Unicode.GetBytes(p1);
		byte[] bytes3 = Encoding.Unicode.GetBytes(p0);
		byte[] array = new byte[ftual.Length + bytes.Length + bytes2.Length + bytes3.Length + 6];
		int num = 0;
		Array.Copy(ftual, 0, array, num, ftual.Length);
		num += ftual.Length;
		Array.Copy(bytes, 0, array, num, bytes.Length);
		num += bytes.Length + 2;
		Array.Copy(bytes2, 0, array, num, bytes2.Length);
		num += bytes2.Length + 2;
		Array.Copy(bytes3, 0, array, num, bytes3.Length);
		return array;
	}

	internal static void pxlzo(byte[] p0, out string p1, out string p2, out string p3)
	{
		string text = Encoding.Unicode.GetString(p0, ftual.Length, p0.Length - ftual.Length - 2);
		char[] separator = new char[1];
		string[] array = text.Split(separator);
		if (array.Length != 3)
		{
			p1 = (p2 = (p3 = null));
			return;
		}
		p1 = array[0];
		p2 = array[1];
		p3 = array[2];
	}

	internal oabdp(jfxnb owner, zccyb recipientType, string displayName, string smtpAddress, string emailAddress, string addressType, int recipNumber)
	{
		if (addressType == null || 1 == 0)
		{
			throw new ArgumentNullException("addressType", "Value cannot be null.");
		}
		if ((emailAddress == null || 1 == 0) && (smtpAddress == null || 1 == 0))
		{
			throw new ArgumentException("An address must be specified.");
		}
		switch (recipientType)
		{
		default:
			throw hifyx.nztrs("recipientType", recipientType, "Argument is out of range of valid values.");
		case zccyb.pqplk:
		case zccyb.hfokr:
		case zccyb.zgczd:
		{
			bnlni = recipientType;
			wvzlg = new howhn(owner);
			wvzlg.mycww(MsgPropertyTag.ObjectType, 6);
			wvzlg.mycww(MsgPropertyTag.RowId, recipNumber);
			wvzlg.mycww(MsgPropertyTag.RecipientType, (int)recipientType);
			wvzlg.mycww(MsgPropertyTag.RecipientOrder, recipNumber);
			wvzlg.mycww(MsgPropertyTag.DisplayType, 0);
			wvzlg.mycww(MsgPropertyTag.RecipientFlags, 1);
			object obj = displayName;
			if (obj == null || 1 == 0)
			{
				obj = smtpAddress;
				if (obj == null || 1 == 0)
				{
					obj = "";
				}
			}
			oyway((string)obj, displayName, smtpAddress, emailAddress, addressType);
			break;
		}
		}
	}

	internal void oyway(string p0, string p1, string p2, string p3, string p4)
	{
		wvzlg.qrqdp(MsgPropertyTag.AddressType, p4);
		wvzlg.qrqdp(MsgPropertyTag.SmtpAddress, p2);
		wvzlg.qrqdp(MsgPropertyTag.EmailAddress, p3);
		wvzlg.qrqdp(MsgPropertyTag.DisplayName, p0);
		wvzlg.qrqdp(MsgPropertyTag.RecipientDisplayName, p0);
		wvzlg.qrqdp(MsgPropertyTag.TransmittableDisplayName, p1);
		object obj3;
		if (!string.IsNullOrEmpty(p3) || 1 == 0)
		{
			object obj = p4;
			if (obj == null || 1 == 0)
			{
				obj = "SMTP";
			}
			object obj2 = p0;
			if (obj2 == null || 1 == 0)
			{
				obj2 = "";
			}
			obj3 = xngmq(p3, (string)obj, (string)obj2);
		}
		else
		{
			obj3 = null;
		}
		byte[] p5 = (byte[])obj3;
		wvzlg.qrqdp(MsgPropertyTag.EntryId, p5);
		wvzlg.qrqdp(MsgPropertyTag.RecipientEntryId, p5);
	}
}
